using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.TrainRuns.Entities;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using JourneyId = DBetter.Domain.TrainRun.ValueObjects.JourneyId;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionService connectionService) : IConnectionsQueryRepository
{
    public async Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var bahnConnectionRequest = request.ToRequest(page);
        var bahnConnectionRequestEntity = new BahnConnectionRequestEntity(request.Id, bahnConnectionRequest);
        
        var response = await connectionService.GetSuggestionsAsync(bahnConnectionRequest);

        var journeyIds = response.Verbindungen
            .SelectMany(v => v.VerbindungsAbschnitte)
            .Where(va => va.Verkehrsmittel.Typ is not VerkehrsmittelTyp.WALK)
            .Select(va => new JourneyId(va.JourneyId!))
            .Distinct();
        
        var stopEvas = response.Verbindungen
            .SelectMany(v => v.VerbindungsAbschnitte)
            .SelectMany(va => va.Halte)
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .Distinct();
        
        foreach (var journeyId in journeyIds)
        {
            //TODO: handle origin and destination station name
            var originStationEva = journeyId.GetOriginEvaNumber();
            var destinationStationEva = journeyId.GetDestinationEvaNumber();
            Console.WriteLine($"{originStationEva} - {destinationStationEva}");
        }

        var existingTrainRuns = await context.TrainRuns
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId.Value, tr => tr);
        
        var existingStations = await context.Stations
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber.Value, s => s);

        List<Station> stationsToCreate = [];
        List<TrainRunEntity> trainRunsToCreate = [];
        List<ConnectionEntity> connectionsToCreate = [];
        
        var connections = response.Verbindungen.Select(v =>
        {
            var connection = v.ToDomain(
                request.Id,
                existingTrainRuns,
                out var newTrainRuns,
                existingStations,
                out var newStations);
            
            stationsToCreate.AddRange(newStations);
            trainRunsToCreate.AddRange(newTrainRuns);
            
            connectionsToCreate.Add(new ConnectionEntity(connection.Id, bahnConnectionRequestEntity.Id, v.CtxRecon));
            
            return connection;
        }).ToList();
        
        await context.BahnConnectionRequests.AddAsync(bahnConnectionRequestEntity);
        await context.Stations.AddRangeAsync(stationsToCreate);
        await context.TrainRuns.AddRangeAsync(trainRunsToCreate);
        await context.Connections.AddRangeAsync(connectionsToCreate);
        
        return connections;
    }
    
    public async Task<Connection?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        EvaNumber fixedStartEvaNumber,
        DepartureTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        ArrivalTime fixedEndTime)
    {
        var originalConnection = await context.Connections
            .FirstOrDefaultAsync(c => c.Id == id);

        if (originalConnection is null) return null;
        
        var bahnRequest = await context.BahnConnectionRequests
            .FirstOrDefaultAsync(r => r.Id ==  originalConnection.RequestId);
        
        if (bahnRequest is null) return null;

        var request = bahnRequest.Request.ToRequest(
            originalConnection.ContextId,
            fixedStartEvaNumber,
            fixedStartTime,
            fixedEndEvaNumber,
            fixedEndTime);

        var response = await connectionService.GetSuggestionsWithIncreasedTransferTimeAsync(request);

        var journeyIds = response.Verbindung.VerbindungsAbschnitte
            .Where(va => va.Verkehrsmittel.Typ is not VerkehrsmittelTyp.WALK)
            .Select(va => new JourneyId(va.JourneyId!))
            .Distinct();

        var stopEvas = response.Verbindung.VerbindungsAbschnitte
            .SelectMany(va => va.Halte)
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .Distinct();

        foreach (var journeyId in journeyIds)
        {
            //TODO: handle origin and destination station name
            var originStationEva = journeyId.GetOriginEvaNumber();
            var destinationStationEva = journeyId.GetDestinationEvaNumber();
            Console.WriteLine($"{originStationEva} - {destinationStationEva}");
        }

        var existingTrainRuns = await context.TrainRuns
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId.Value, tr => tr);
        
        var existingStations = await context.Stations
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber.Value, s => s);
        
        var connection = response.Verbindung.ToDomain(
            bahnRequest.Id,
            existingTrainRuns,
            out var trainRunsToCreate,
            existingStations,
            out var stationsToCreate);

        var connectionToCreate = new ConnectionEntity(connection.Id, bahnRequest.Id, response.Verbindung.CtxRecon);
        
        await context.Stations.AddRangeAsync(stationsToCreate);
        await context.TrainRuns.AddRangeAsync(trainRunsToCreate);
        await context.Connections.AddAsync(connectionToCreate);
        
        return connection;
    }
}
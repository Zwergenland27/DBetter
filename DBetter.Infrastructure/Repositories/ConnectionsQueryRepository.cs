using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe;
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

        var existingTrainRuns = await context.TrainRuns
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId.Value, tr => tr);

        List<TrainRunEntity> trainRunsToCreate = [];
        List<ConnectionEntity> connectionsToCreate = [];
        
        var connections = response.Verbindungen.Select(v =>
        {
            var connection = v.ToDomain(request.Id, existingTrainRuns, out var newTrainRuns);
            
            trainRunsToCreate.AddRange(newTrainRuns);
            connectionsToCreate.Add(new ConnectionEntity(connection.Id, bahnConnectionRequestEntity.Id, v.CtxRecon));
            
            return connection;
        }).ToList();
        
        context.BahnConnectionRequests.Add(bahnConnectionRequestEntity);
        context.TrainRuns.AddRange(trainRunsToCreate);
        context.Connections.AddRange(connectionsToCreate);
        
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

        var existingTrainRuns = await context.TrainRuns
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId.Value, tr => tr);
        
        var connection = response.Verbindung.ToDomain(bahnRequest.Id, existingTrainRuns, out var trainRunsToCreate);

        var connectionToCreate = new ConnectionEntity(connection.Id, bahnRequest.Id, response.Verbindung.CtxRecon);
        
        context.TrainRuns.AddRange(trainRunsToCreate);
        context.Connections.Add(connectionToCreate);
        
        return connection;
    }
}

//TODO: Store all stations of all connections -> JourneyId LS Value is EVA id of last stop of the planned train run -> lookup in Database instead of rely on db information
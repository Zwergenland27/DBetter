using DBetter.Application.Connections;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BackgroundJobs;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using JourneyId = DBetter.Infrastructure.BahnDe.Routes.DTOs.JourneyId;
using TravelTime = DBetter.Domain.Routes.ValueObjects.TravelTime;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionService connectionService) : IConnectionsQueryRepository
{
    public async Task<ConnectionSuggestionsDto> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var bahnConnectionRequest = request.ToRequest(page);
        var bahnConnectionRequestEntity = new BahnConnectionRequestEntity(request.Id, bahnConnectionRequest);
        
        var fahrplan = await connectionService.GetSuggestionsAsync(bahnConnectionRequest);

        var journeyIds = fahrplan.GetJourneyIds();
        var stopEvas = fahrplan.GetEvaNumbers(journeyIds);

        var existingRoutes = await context.Routes
            .AsNoTracking()
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId, tr => tr);
        
        var existingStations = await context.Stations
            .AsNoTracking()
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);

        var connectionFactory = ConnectionFactory
            .CreateFrom(fahrplan)
            .WithExistingRoutes(existingRoutes)
            .WithExistingStations(existingStations);
        
        TrainRunScraperJob.AddTrainRuns(connectionFactory.TrainRunsToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.BahnConnectionRequests.AddAsync(bahnConnectionRequestEntity);
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.TrainRunsToCreate);
        await context.Connections.AddRangeAsync(connectionFactory.ConnectionsToCreate);
        
        return connectionFactory.SuggestionsDto;
    }
    
    public async Task<ConnectionDto?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        EvaNumber fixedStartEvaNumber,
        TravelTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        TravelTime fixedEndTime)
    {
        throw new NotImplementedException();
        // var originalConnection = await context.Connections
        //     .FirstOrDefaultAsync(c => c.Id == id);

        // if (originalConnection is null) return null;
        
        // var bahnRequest = await context.BahnConnectionRequests
        //     .FirstOrDefaultAsync(r => r.Id ==  originalConnection.RequestId);
        
        // if (bahnRequest is null) return null;

        // var request = bahnRequest.Request.ToRequest(
        //     originalConnection.ContextId,
        //     fixedStartEvaNumber,
        //     fixedStartTime,
        //     fixedEndEvaNumber,
        //     fixedEndTime);

        // var response = await connectionService.GetSuggestionsWithIncreasedTransferTimeAsync(request);

        // var journeyIds = response.Verbindung.VerbindungsAbschnitte
        //     .Where(va => va.Verkehrsmittel.Typ is not VerkehrsmittelTyp.WALK)
        //     .Select(va => new JourneyId(va.JourneyId!))
        //     .Distinct()
        //     .ToList();

        // var stopEvas = response.Verbindung.VerbindungsAbschnitte
        //     .SelectMany(va => va.Halte)
        //     .Select(h => EvaNumber.Create(h.ExtId).Value)
        //     .Distinct()
        //     .Union(
        //         journeyIds.Select(jid => jid.GetDestinationEvaNumber())
        //         .Distinct())
        //     .ToList();

        // var existingTrainRuns = await context.Routes
        //     .Where(tr => journeyIds.Contains(tr.JourneyId))
        //     .ToDictionaryAsync(tr => tr.JourneyId, tr => tr);
        
        // var existingStations = await context.Stations
        //     .Where(s => stopEvas.Contains(s.EvaNumber))
        //     .ToDictionaryAsync(s => s.EvaNumber, s => s);
        
        // var connection = response.Verbindung.ToDomain(
        //     bahnRequest.Id,
        //     existingTrainRuns,
        //     out var trainRunsToCreate,
        //     existingStations,
        //     out var stationsToCreate);

        // var connectionToCreate = new ConnectionEntity(connection.Id, bahnRequest.Id, response.Verbindung.CtxRecon);
        
        // TrainRunScraperJob.AddTrainRuns(trainRunsToCreate
        //     .Where(tr => tr.ScrapingRequired)
        //     .Select(tr => tr.Id)
        //     .ToList());
        
        // await context.Stations.AddRangeAsync(stationsToCreate);
        // await context.Routes.AddRangeAsync(trainRunsToCreate);
        // await context.Connections.AddAsync(connectionToCreate);
        
        // return connection;
    }
}
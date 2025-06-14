using DBetter.Application.Connections;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BackgroundJobs;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionService connectionService) : IConnectionsQueryRepository
{
    public async Task<ConnectionSuggestionsDto> GetConnectionSuggestionsAsync(ConnectionRequest request)
    {
        var requestStationEvas = await context.Stations
            .AsNoTracking()
            .Where(s => request.GetStops().Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.EvaNumber);

        var anfrage = request.ToRequest(requestStationEvas);
        var anfrageEntity = new BahnConnectionRequestEntity(request.Id, anfrage);
        await context.BahnConnectionRequests.AddAsync(anfrageEntity);
        return await GetConnectionSuggestionsAsync(request.Id, anfrage);
    }

    public async Task<ConnectionSuggestionsDto?> GetConnectionSuggestionsAsync(ConnectionRequestId id, string? page)
    {
        var anfrage = await context.BahnConnectionRequests
            .FirstOrDefaultAsync(s => s.Id == id);
        if(anfrage is null) return null;
        
        anfrage.Request.PagingReference = page;
        return await GetConnectionSuggestionsAsync(id, anfrage.Request);
    }

    private async Task<ConnectionSuggestionsDto> GetConnectionSuggestionsAsync(ConnectionRequestId id, ReiseAnfrage anfrage)
    {
        var fahrplan = await connectionService.GetSuggestionsAsync(anfrage);

        var journeyIds = fahrplan.GetJourneyIds();
        var stopEvas = fahrplan.GetEvaNumbers(journeyIds);
        stopEvas.AddRange(anfrage.GetEvaNumbers());
        stopEvas = stopEvas.Distinct().ToList();

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
            .WithRequest(id, anfrage)
            .WithExistingRoutes(existingRoutes)
            .WithExistingStations(existingStations);
        
        RouteScraperJob.AddRoutes(connectionFactory.RoutesToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.RoutesToCreate);
        await context.Connections.AddRangeAsync(connectionFactory.ConnectionsToCreate);
        
        return connectionFactory.SuggestionsDto;
    }

    public async Task<ConnectionDto?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        StationId fixedStartStationId,
        DateTime fixedStartTime,
        StationId fixedEndStationId,
        DateTime fixedEndTime)
    {
        var originalConnection = await context.Connections
            .FirstOrDefaultAsync(c => c.Id == id);

        if (originalConnection is null) return null;
        
        var bahnRequest = await context.BahnConnectionRequests
            .FirstOrDefaultAsync(r => r.Id == originalConnection.RequestId);

        var stationIds = await context.Stations
            .AsNoTracking()
            .Where(s => s.Id == fixedStartStationId || s.Id == fixedEndStationId)
            .ToDictionaryAsync(s => s.Id, s => s.EvaNumber);
        
        if (bahnRequest is null) return null;

        var request = bahnRequest.Request.ToRequest(
            originalConnection.ContextId,
            stationIds[fixedStartStationId],
            fixedStartTime,
            stationIds[fixedEndStationId],
            fixedEndTime);

        var teilstrecke = await connectionService.GetSuggestionsWithIncreasedTransferTimeAsync(request);

        var journeyIds = teilstrecke.Verbindung.GetJourneyIds();

        var stopEvas = teilstrecke.Verbindung.GetEvaNumbers();

        var existingRoutes = await context.Routes
            .Where(tr => journeyIds.Contains(tr.JourneyId))
            .ToDictionaryAsync(tr => tr.JourneyId, tr => tr);
        
        var existingStations = await context.Stations
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);

        var connectionFactory = ConnectionFactory
            .CreateFrom(teilstrecke)
            .WithRequest(originalConnection.RequestId)
            .WithExistingRoutes(existingRoutes)
            .WithExistingStations(existingStations);
        
        RouteScraperJob.AddRoutes(connectionFactory.RoutesToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.RoutesToCreate);
        await context.Connections.AddRangeAsync(connectionFactory.ConnectionsToCreate);
        
        return connectionFactory.ConnectionDto;
    }
}
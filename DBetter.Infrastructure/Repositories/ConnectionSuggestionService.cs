using DBetter.Application.Requests;
using DBetter.Domain.ConnectionRequests;
using DBetter.Infrastructure.BackgroundJobs;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionSuggestionService(
    DBetterContext context,
    ConnectionService connectionService) : IConnectionSuggestionService
{
    public async Task<ConnectionDto> GetConnectionSuggestionsAsync(
        ConnectionRequest request,
        SuggestionMode suggestionMode)
    {
        var requestStationEvas = await context.Stations
            .AsNoTracking()
            .Where(s => request.GetAllStops().Contains(s.Id))
            .ToListAsync();

        var anfrage = RequestFactory
            .CreateFrom(request)
            .WithStations(requestStationEvas)
            .WithSuggestionMode(suggestionMode)
            .RequestObject;
        
        var fahrplan = await connectionService.GetSuggestionsAsync(anfrage);

        var bahnJourneyIds = fahrplan.GetBahnJourneyIds();
        var stopEvas = fahrplan.GetEvaNumbers(bahnJourneyIds);
        stopEvas.AddRange(requestStationEvas.Select(station => station.EvaNumber));
        stopEvas = stopEvas.Distinct().ToList();

        var existingRoutes = await context.Routes
            .AsNoTracking()
            .Where(tr => bahnJourneyIds.Contains(tr.BahnJourneyId))
            .ToDictionaryAsync(tr => tr.BahnJourneyId, tr => tr);
        
        var existingStations = await context.Stations
            .AsNoTracking()
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);
        
        var connectionFactory = new ConnectionFactory(
                fahrplan.Verbindungen,
                anfrage,
                existingRoutes,
                existingStations);
        
        var connections = connectionFactory.GetConnections();
        
        RouteScraperJob.AddRoutes(connectionFactory.RoutesToCreate
            .Where(tr => tr.ScrapingRequired)
            .Select(tr => tr.Id)
            .ToList());
        
        await context.Stations.AddRangeAsync(connectionFactory.StationsToCreate);
        await context.Routes.AddRangeAsync(connectionFactory.RoutesToCreate);
        
        return new ConnectionDto
        {
            Connections = connections,
            EarlierRef = fahrplan.VerbindungReference.Earlier,
            LaterRef = fahrplan.VerbindungReference.Later
        };
    }
}
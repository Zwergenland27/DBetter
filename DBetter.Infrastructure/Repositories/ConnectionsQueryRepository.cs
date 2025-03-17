using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(
    DBetterContext context,
    ConnectionSuggestionService connectionSuggestionService) : IConnectionsQueryRepository
{
    public async Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var sw = Stopwatch.StartNew();
        var response = await connectionSuggestionService.GetSuggestionsAsync(request, page);
        
        sw.Stop();
        Console.WriteLine($"Request time: {sw.ElapsedMilliseconds} ms");
        
        sw.Restart();
        var stations = await GetAndInsertStations(response);
        
        sw.Stop();
        Console.WriteLine($"Station storage time: {sw.ElapsedMilliseconds} ms");
        
        sw.Restart();
        var connections = response.Verbindungen.Select(v => v.ToDomain(stations)).ToList();
        sw.Stop();
        Console.WriteLine($"Mapping time: {sw.ElapsedMilliseconds} ms");
        
        sw.Restart();
        context.Connections.AddRange(connections);
        sw.Stop();
        Console.WriteLine($"Connection storage time: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        await context.SaveChangesAsync();
        sw.Stop();
        
        Console.WriteLine($"Saving time: {sw.ElapsedMilliseconds} ms");
        
        return connections;
    }

    private async Task<List<Station>> GetAndInsertStations(Fahrplan fahrplan)
    {
        var rawStations = fahrplan.Verbindungen
            .SelectMany(x => x.VerbindungsAbschnitte)
            .SelectMany(x => x.Halte)
            .Select(h => new
            {
                EvaNumber = EvaNumber.Create(h.ExtId).Value,
                Name = StationName.Create(h.Name).Value,
                InfoId = h.BahnhofsInfoId != null ? StationInfoId.Create(h.BahnhofsInfoId).Value : null
            })
            .Distinct()
            .ToList();
        
        var test = rawStations.Select(rs => rs.EvaNumber).ToList();

        var existingStations = await context.Stations
            .Where(s => test.Contains(s.EvaNumber))
            .ToListAsync();

        if (rawStations.Count == existingStations.Count)
        {
            return existingStations;
        }
        
        var stationsToInsert = rawStations.Where(rs => existingStations.All(es => es.EvaNumber != rs.EvaNumber));

        var newStations = stationsToInsert.Select(
            s => new Station(
                StationId.CreateNew(),
                s.EvaNumber,
                s.Name,
                null,
                s.InfoId))
            .ToList();
        
        context.Stations.AddRange(newStations);
        
        existingStations.AddRange(newStations);
        
        return existingStations;
    }
}
using System.Net.Http.Json;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Stations;

public class StationService(HttpClient http)
{
    public async Task<List<Haltestelle>> FindAsync(string query, int limit = 10)
    {
        try
        {
            var orte =
                await http.GetFromJsonAsync<List<Ort>>($"reiseloesung/orte?suchbegriff={query}&limit={limit}");

            if (orte is null) throw new BahnDeException("StationService","No station data received");

            return orte
                .Where(ort => ort.ExtId is not null)
                .Select(ort => new Haltestelle
                {
                    ExtId = ort.ExtId!,
                    Name = ort.Name,
                    Lat = ort.Lat,
                    Lon = ort.Lon,
                })
                .ToList();
        }
        catch (HttpRequestException httpException)
        {
            throw new BahnDeException("StationService", $"Instead of success the station API returned {httpException.StatusCode}");
        }
    }
}
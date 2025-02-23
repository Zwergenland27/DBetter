using System.Net.Http.Json;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Stations;

public class StationService(HttpClient http)
{
    public async Task<List<Station>> FindAsync(string query, int limit = 10)
    {
        try
        {
            var searchResults =
                await http.GetFromJsonAsync<List<StationDto>>($"reiseloesung/orte?suchbegriff={query}&limit={limit}");

            if (searchResults is null)
            {
                throw new StationException("No station data received");
            }

            List<Station> results = [];
            
            var stopResults = searchResults.Where(s => s.Type == "ST");
            foreach (var station in stopResults)
            {
                var evaNumber = EvaNumber.Create(station.ExtId);
                if (evaNumber.HasFailed)
                {
                    continue;
                }

                var name = StationName.Create(station.Name);
                if (name.HasFailed)
                {
                    continue;
                }

                var coordinates = new Coordinates(station.Lat, station.Lon);

                results.Add(new Station(StationId.CreateNew(), evaNumber.Value, name.Value, coordinates));
            }

            return results;
        }
        catch (HttpRequestException httpException)
        {
            throw new StationException($"Instead of success the station API returned {httpException.StatusCode}");
        }
    }
}
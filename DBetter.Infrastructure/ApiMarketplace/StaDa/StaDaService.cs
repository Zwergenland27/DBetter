using System.Net.Http.Json;
using DBetter.Infrastructure.ApiMarketplace.StaDa.DTOs;

namespace DBetter.Infrastructure.ApiMarketplace.StaDa;

public class StaDaService(HttpClient http)
{
    public async Task<List<Station>> GetStationData(string evaNumber)
    {
        try
        {
            var response = await http.GetFromJsonAsync<StationQuery>($"stations?eva={evaNumber}");
            
            if (response is null || response.HasFailed) return [];

            return response.Result!;
        }
        catch (Exception e)
        {
            // ignored
        }

        return [];
    }
}
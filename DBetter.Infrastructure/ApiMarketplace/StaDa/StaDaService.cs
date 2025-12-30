using System.Net.Http.Json;
using System.Text.Json;
using DBetter.Infrastructure.ApiMarketplace.StaDa.DTOs;

namespace DBetter.Infrastructure.ApiMarketplace.StaDa;

public class StaDaService(HttpClient http)
{
    public async Task<List<Station>> GetStationData(string evaNumber)
    {
        var response = await http.GetAsync($"stations?eva={evaNumber}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];
        response.EnsureSuccessStatusCode();
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<StationQuery>(stringResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        if (result?.Result is null || result.HasFailed) return [];

        return result.Result;
    }
}
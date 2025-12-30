using System.Xml.Serialization;
using DBetter.Infrastructure.ApiMarketplace.Timetables.DTOs;

namespace DBetter.Infrastructure.ApiMarketplace.Timetables;

public class TimetablesService(HttpClient http)
{
    public async Task<List<Station>> GetStationData(string evaNumber)
    {
        var response = await http.GetAsync($"station/{evaNumber}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return [];
        response.EnsureSuccessStatusCode();
            
        await using var stream = await response.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(MultipleStationData));
            
        var result = serializer.Deserialize(stream) as MultipleStationData;
        if (result is null) return [];
            
        return result.Stations
            .Where(s => !s.Ds100.Any(char.IsDigit))
            .ToList();
    }
}
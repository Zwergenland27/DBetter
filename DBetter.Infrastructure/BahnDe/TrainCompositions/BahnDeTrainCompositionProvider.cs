using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.TrainCompositions;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.TrainCompositions;

public class BahnDeTrainCompositionProvider(HttpClient http) : IExternalTrainCompositionProvider
{
    public async Task<TrainCompositionDto?> GetRealTimeDataAsync(ServiceNumber ServiceNumber, DateOnly Date, EvaNumber AtStation)
    {
        var administrationId = 80;
        var category = "CAT";
        var date = Date.ToString("yyyy-MM-dd");
        var time = $"{date}T00:00:00.000Z";
        var response = await http.GetAsync($"reisebegleitung/wagenreihung/vehicle-sequence?=administrationId={administrationId}&category={category}&date={date}&evaNumber={AtStation.Value}&number={ServiceNumber.Value}&time={time}");

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("BahnDeTrainCompositionProvider.Get", $"Requesting train composition from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var vehicleSequence = JsonSerializer.Deserialize<VehicleSequenceResult>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (vehicleSequence is null)
        {
            throw new BahnDeException("BahnDeTrainCompositionProvider.Get",
                "Response from bahn.de could not be parsed");
        }
        
        return new TrainCompositionDto
        {
            Vehicles = vehicleSequence.Groups.Select(g => new VehicleDto
            {
                Name =  g.Name,
                DestinationStation = StationName.Create(g.Transport.Destination.Name).Value,
                Coaches = g.Vehicles.Select( v => new CoachDto
                {
                    ConstructionType = v.Type.ConstructionType
                }).ToList()
            }).ToList()
        };
    }
}
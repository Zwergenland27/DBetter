using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using DBetter.Application.TrainCompositions;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Infrastructure.BahnDe.TrainCompositions.Planned;
using DBetter.Infrastructure.BahnDe.TrainCompositions.RealTime;
using HtmlAgilityPack;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;

namespace DBetter.Infrastructure.BahnDe.TrainCompositions;

public class BahnDeTrainCompositionProvider(HttpClient http) : IExternalTrainCompositionProvider
{
    private static readonly List<string> CategoryPlaceholder = [
        "ICE",
        "IC"
    ];
    public async Task<TrainCompositionDto?> GetRealTimeDataAsync(ServiceNumber serviceNumber, DateOnly date, EvaNumber atStation)
    {
        var resiliencePipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromSeconds(5),
                ShouldHandle = BahnDeRetryStrategyOptions.IsTransient
            })
            .Build();
            
        const int administrationId = 80;
        var random = new Random();
        var category = CategoryPlaceholder[random.Next(0,  CategoryPlaceholder.Count - 1)];
        var dateString = date.ToString("yyyy-MM-dd");
        var time = $"{dateString}T00:00:00.000Z";

        var url = $"reisebegleitung/wagenreihung/vehicle-sequence?=administrationId={administrationId}&category={category}&date={dateString}&evaNumber={atStation.Value}&number={serviceNumber.Value}&time={time}";
        
        var response = await resiliencePipeline.ExecuteAsync(async ct => await http.GetAsync(url, ct));

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
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

    public async Task<PlannedTrainCompositionDto?> GetPlannedDataAsync(ServiceNumber serviceNumber, EvaNumber originStation, DateTime deparureTime, EvaNumber destinationStation, DateTime arrivalTime)
    {
        var request = new RequestBuilder(serviceNumber, originStation, deparureTime, destinationStation, arrivalTime).Build();
        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var uri = HttpUtility.UrlPathEncode(requestJson);
        var response = await http.GetAsync($"gsd/gsd_v3?data={uri}");
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Conflict) return null;
            throw new BahnDeException("BahnDeTrainCompositionProvider.GetPlanned", $"Requesting planned train composition from bahn.de failed with status code {response.StatusCode}");
        }
        
        var html = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var scriptNode = document.DocumentNode.SelectSingleNode("//script[@type='application/json' and @id='ssr_data']");
        if (scriptNode is null) return null;
        
        var json = scriptNode.InnerText;
        var vehicleSequence = JsonSerializer.Deserialize<PlannedSequence>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (vehicleSequence is null)
        {
            throw new BahnDeException("BahnDeTrainCompositionProvider.GetPlanned",
                "Response from bahn.de could not be parsed");
        }
        
        return new PlannedTrainCompositionDto
        {
            Vehicles = vehicleSequence.Zugfahrt.Zugteile.Select(g => new PlannedVehicleDto
            {
                Coaches = g.Wagen.Select( v => new CoachDto
                {
                    ConstructionType = v.Wagentyp
                }).ToList()
            }).ToList()
        };
    }
}
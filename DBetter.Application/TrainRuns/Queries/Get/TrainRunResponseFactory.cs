using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using StopResponse = DBetter.Contracts.TrainRuns.Queries.Get.Results.StopResponse;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class TrainRunResponseFactory(
    TrainCirculation trainCirculation,
    TrainRun trainRun,
    List<PassengerInformation> passengerInformation,
    List<Station> stations)
{
    public TrainRunResponse MapToResponse(Route route, TrainCompositionResultDto? composition)
    {
        var serviceInformation = trainCirculation.ServiceInformation;
        LineInformationResponse? lineInformation = null;
        if (serviceInformation.LineNumber is not null)
        {
            lineInformation = new LineInformationResponse
            {
                Number = serviceInformation.LineNumber.Number,
                ProductClass = serviceInformation.LineNumber.ProductClass,
                ServiceNumber = serviceInformation.ServiceNumber?.Value,
            };
        }
        return new TrainRunResponse
        {
            TrainComposition = MapToResponse(composition),
            Id = trainRun.Id.Value.ToString(),
            LastUpdatedAt = route.LastUpdatedAt,
            CirculationId = trainRun.CirculationId.Value.ToString(),
            Operator = null,
            BikeCarriage = trainRun.BikeCarriage.ToResponse(),
            Catering = trainRun.Catering.ToResponse(),
            TransportCategory = serviceInformation.TransportCategory.ToString(),
            Line = lineInformation,
            Stops = route.Stops.Select(MapToResponse).ToList(),
            PassengerInformation = passengerInformation.Select(pim => pim.ToResponse()).ToList()
        };
    }

    private GetTrainCompositionResultDto? MapToResponse(TrainCompositionResultDto? composition)
    {
        if (composition is null) return null;
        return new GetTrainCompositionResultDto
        {
            LastUpdatedAt = composition.LastUpdatedAt,
            TrainRunId = composition.TrainRunId,
            Vehicles = composition.Vehicles,
            Source = composition.Source.ToString()
        };
    }
    
    private StopResponse MapToResponse(Stop stop)
    {
        var station = stations.First(station => station.Id == stop.StationId);
        var attributes = stop.Attributes;

        return new StopResponse
        {
            Id = station.Id.Value.ToString(),
            Ril100 = station.Ril100?.Value,
            ArrivalTime = stop.ArrivalTime?.ToResponse(),
            DepartureTime = stop.DepartureTime?.ToResponse(),
            Demand = stop.Demand.ToResponse(),
            IsAdditional = attributes.IsAdditional,
            IsCancelled = attributes.IsCancelled,
            IsEntryOnly = attributes.IsEntryOnly,
            IsExitOnly = attributes.IsExitOnly,
            IsRequestOnly = attributes.IsRequestStop,
            Name = station.Name.Value,
            Platform = stop.Platform?.ToResponse(),
            StopIndex = stop.RouteIndex.Value,
        };
    }
}
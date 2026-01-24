using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.TrainRuns.Dtos;
using DBetter.Contracts.TrainRuns.Queries.Get.Results;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class TrainRunResponseFactory(TrainRun trainRun, List<Station> stations)
{
    public TrainRunResponse MapToResponse(TrainRunDto dto)
    {
        return new TrainRunResponse
        {
            Id = trainRun.Id.Value.ToString(),
            Operator = null,
            BikeCarriage = trainRun.BikeCarriage.ToResponse(),
            Catering = trainRun.Catering.ToResponse(),
            TransportCategory = trainRun.ServiceInformation.ProductClass,
            ProductClass = trainRun.ServiceInformation.ProductClass,
            Line = trainRun.ServiceInformation.LineNumber?.Value,
            ServiceNumber = trainRun.ServiceInformation.ServiceNumber?.Value,
            Stops = dto.Stops.Select(MapToResponse).ToList()
        };
    }
    
    private StopResponse MapToResponse(StopDto dto)
    {
        var station = stations.First(station => station.EvaNumber == dto.EvaNumber);
        var attributes = dto.Attributes;

        return new StopResponse
        {
            Id = station.Id.Value.ToString(),
            Ril100 = station.Ril100?.Value,
            ArrivalTime = dto.ArrivalTime?.ToResponse(),
            DepartureTime = dto.DepartureTime?.ToResponse(),
            Demand = dto.Demand.ToResponse(),
            IsAdditional = attributes.IsAdditional,
            IsCancelled = attributes.IsCancelled,
            IsEntryOnly = attributes.IsEntryOnly,
            IsExitOnly = attributes.IsExitOnly,
            IsRequestOnly = attributes.IsRequestStop,
            Name = station.Name.Value,
            Platform = dto.Platform?.ToResponse(),
            StopIndex = dto.TrainRunIndex.Value,
        };
    }
}
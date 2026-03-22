using System.Runtime.InteropServices.ComTypes;
using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public class ConnectionResponseFactory(
    List<Connection> connections, 
    List<TrainCirculation> trainCirculations,
    List<TrainRun> trainRuns,
    List<Station> stations,
    List<PassengerInformation> passengerInformation,
    List<TrainCompositionResultDto> trainCompositionResults)
{

    private int _transferIndex = 0;
    public ConnectionResponse MapToResponse(ConnectionDto dto)
    {
        _transferIndex = 0;
        var segments = dto.Segments.Select(MapToResponse).ToList();
        var differentOrigin = false;
        var differentDestination = false;
        
        var firstSegment = segments.First();
        if (firstSegment is WalkingSegmentResponse)
        {
            differentOrigin = true;
            _transferIndex++;
            segments.Remove(firstSegment);
        }
        var lastSegment = segments.Last();
        if (lastSegment is WalkingSegmentResponse)
        {
            differentDestination = true;
            segments.Remove(lastSegment);
        }
        
        return new ConnectionResponse
        {
            Id = connections.First(c => c.ContextId == dto.ContextId).Id.Value.ToString(),
            DifferentOrigin = differentOrigin,
            DifferentDestination = differentDestination,
            Demand = dto.Demand.ToResponse(),
            BahnDeUrl = dto.BookingLink.Value,
            Offer = dto.Offer?.ToResponse(),
            Segments = segments
        };
    }

    private SegmentResponse MapToResponse(SegmentDto dto)
    {
        return dto switch
        {
            TransferSegmentDto transferSegmentSnapshot => MapToTransferSegmentResponse(transferSegmentSnapshot),
            TransportSegmentDto transportSegmentSnapshot => MapToTransportSegmentResponse(transportSegmentSnapshot),
            _ => throw new ArgumentOutOfRangeException(nameof(dto))
        };
    }

    private TransferSegmentResponse MapToTransferSegmentResponse(TransferSegmentDto dto)
    {
        if (dto is WalkingSegmentDto walkingSegmentDto)
        {
            return new WalkingSegmentResponse
            {
                TransferId = _transferIndex++,
                Distance = walkingSegmentDto.Distance,
                WalkDuration = walkingSegmentDto.WalkDuration,
            };
        }
        return new TransferSegmentResponse
        {
            TransferId = _transferIndex++
        };
    }

    private TransportSegmentResponse MapToTransportSegmentResponse(TransportSegmentDto dto)
    {
        var originStationEva = dto.JourneyId.OriginEvaNumber;
        var originStationName = stations.FirstOrDefault(station => station.EvaNumber == originStationEva)?.Name;
        var destinationStationEva = dto.JourneyId.DestinationEvaNumber;
        var destinationStationName =  stations.FirstOrDefault(station => station.EvaNumber == destinationStationEva)?.Name;

        var trainCirculation = trainCirculations.First(tc => tc.Identifier == dto.JourneyId.TrainCirculationIdentifier);
        var trainRun = trainRuns.First(tr => tr.CirculationId == trainCirculation.Id && tr.OperatingDay == dto.JourneyId.OperatingDay);
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
        
        return new TransportSegmentResponse
        {
            TrainRunId = trainRun.Id.Value.ToString(),
            DepartureTime = dto.Stops.First().DepartureTime!.ToResponse(),
            ArrivalTime = dto.Stops.Last().ArrivalTime!.ToResponse(),
            BikeCarriage = trainRun.BikeCarriage.ToResponse(),
            Catering = trainRun.Catering.ToResponse(),
            Demand = dto.Demand.ToResponse(),
            Origin = originStationName?.NormalizedValue,
            Destination = destinationStationName?.NormalizedValue ?? dto.Destination?.NormalizedValue,
            Line = lineInformation,
            Operator = null,
            TransportCategory = serviceInformation.TransportCategory.ToString(),
            PassengerInformation = trainRun.PassengerInformation
                .Select(pim => passengerInformation.First(pi => pi.Id == pim.InformationId).ToResponse())
                .ToList(),
            Stops = dto.Stops.Select(MapToResponse).ToList(),
            TrainComposition = MapToResponse(trainRun.Id)
        };
    }
    
    private GetTrainCompositionResultDto? MapToResponse(TrainRunId trainRunId)
    {
        var composition = trainCompositionResults.FirstOrDefault(c => c.TrainRunId == trainRunId.Value.ToString());
        if (composition is null) return null;

        return new GetTrainCompositionResultDto
        {
            LastUpdatedAt = composition.LastUpdatedAt,
            TrainRunId = trainRunId.Value.ToString(),
            Vehicles = composition.Vehicles,
            Source = composition.Source.ToString()
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
            Name = station.Name.NormalizedValue,
            Platform = dto.Platform?.ToResponse(),
            StopIndex = dto.TrainRunIndex.Value,
        };
    }
}
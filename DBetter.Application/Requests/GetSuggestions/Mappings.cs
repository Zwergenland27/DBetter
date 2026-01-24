using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Contracts.Shared.DTOs;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;

namespace DBetter.Application.Requests.GetSuggestions;

public static class Mappings
{
    public static DemandResponse ToResponse(this Demand demand)
    {
        return new DemandResponse
        {
            FirstClass = demand.FirstClass.ToString(),
            SecondClass = demand.SecondClass.ToString(),
        };
    }

    public static OfferResponse ToResponse(this Offer offer)
    {
        return new OfferResponse
        {
            ComfortClass = offer.ComfortClass.ToString(),
            Currency = offer.Currency.ToString(),
            Partial = offer.SectionPrice,
            Price = offer.Price
        };
    }
    
    public static TravelTimeDto ToResponse(this TravelTime travelTime){
        return new TravelTimeDto{
            Planned = travelTime.Planned.ToIso8601(),
            Real = travelTime.Real?.ToIso8601()
        };
    }
    
    public static BikeCarriageInformationDto ToResponse(this BikeCarriageInformation bikeCarriage){
        return new BikeCarriageInformationDto{
            Status = bikeCarriage.Status.ToString(),
            FromStopIndex = bikeCarriage.FromStopIndex.Value,
            ToStopIndex = bikeCarriage.ToStopIndex.Value
        };
    }

    public static CateringInformationDto ToResponse(this CateringInformation catering){
        return new CateringInformationDto{
            Type = catering.Type.ToString(),
            FromStopIndex = catering.FromStopIndex.Value,
            ToStopIndex = catering.ToStopIndex.Value
        };
    }
    
    public static string ToResponse(this PassengerInformation message)
    {
        if (message.Type is PassengerInformationType.FreeText)
        {
            return message.Text.Value;
        }
        
        return message.Code.Value;
    }
    
    public static PlatformDto ToResponse(this Platform platform){
        return new PlatformDto {
            Planned = platform.Planned,
            Real = platform.Real,
            Type = platform.Type.ToString()
        };
    }
}
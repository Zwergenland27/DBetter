using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using Halt = DBetter.Infrastructure.BahnDe.Connections.DTOs.Halt;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class BikeCarriageInformationFactory
{
    private readonly List<IRouteStop> _stops;
    private readonly List<Zugattribut> _zugattribute;
    private readonly List<HimMeldung> _himMeldungen;
    public BikeCarriageInformationFactory(VerbindungsAbschnitt abschnitt)
    {
        _stops = abschnitt.Halte.Select(halt => halt as IRouteStop).ToList();
        _zugattribute = abschnitt.Verkehrsmittel.Zugattribute;
        _himMeldungen = abschnitt.HimMeldungen ?? [];
    }

    public BikeCarriageInformationFactory(Fahrt fahrt)
    {
        _stops = fahrt.Halte.Select(halt => halt as IRouteStop).ToList();
        _zugattribute = fahrt.Zugattribute;
        _himMeldungen = fahrt.HimMeldungen ?? [];
    }
    
    public BikeCarriageInformation ExtractInformation()
    {
        var type = BikeCarriageStatus.NoInfo;
        Zugattribut? attribute = null;

        if (IsBikeCarriageImpossible())
        {
            type = BikeCarriageStatus.NotPossible;
        }else if (IsBikeReservationRequired(out attribute))
        {
            type =  BikeCarriageStatus.ReservationRequired;
        }else if (IsBikeCarriageLimited(out attribute))
        {
            type = BikeCarriageStatus.Limited;
        }
        
        var partialInformation = new PartialStopIndexFactory(_stops, attribute?.Teilstreckenhinweis);
        return new BikeCarriageInformation(
            type,
            partialInformation.From,
            partialInformation.To);
    }

    private bool IsBikeCarriageImpossible()
    {
        return _himMeldungen.Any(info => info.Text is not null && info.Text.Contains("Die Mitnahme von Fahrrädern ist nicht möglich."));
    }
    
    private bool IsBikeReservationRequired([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "FR");
        return attribute is not null;
    }

    private bool IsBikeCarriageLimited([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "FB");
        return attribute is not null;
    }
}
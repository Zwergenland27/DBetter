using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class CateringInformationFactory
{
    private static readonly string[] ClosedTexts =
    [
        "Das Bordrestaurant/Bordbistro ist geschlossen.",
        "Zug verkehrt ohne gastronomische Bewirtschaftung."
    ];
    
    private readonly List<ITrainRunStop> _stops;
    private readonly List<Zugattribut> _zugattribute;
    private readonly List<PriorisierteMeldung> _prioritizedMessages;
    
    public CateringInformationFactory(VerbindungsAbschnitt abschnitt)
    {
        _stops = abschnitt.Halte.Select(halt => halt as ITrainRunStop).ToList();
        _zugattribute = abschnitt.Verkehrsmittel.Zugattribute;
        _prioritizedMessages = abschnitt.PriorisierteMeldungen;
    }

    public CateringInformationFactory(Fahrt fahrt)
    {
        _stops = fahrt.Halte.Select(halt => halt as ITrainRunStop).ToList();
        _zugattribute = fahrt.Zugattribute;
        _prioritizedMessages = fahrt.PriorisierteMeldungen;
    }
    
    public CateringInformation ExtractInformation()
    {
        var type = CateringType.None;
        Zugattribut? attribute = null;
        if (IsClosed())
        {
            type = CateringType.Closed;
        }else if (IsRestaurant(out attribute))
        {
            type = CateringType.Restaurant;
        }else if (IsBistro(out attribute))
        {
            type = CateringType.Bistro;
        }else if (IsSeatServed(out attribute))
        {
            type = CateringType.SeatServed;
        }else if (IsSnack(out attribute))
        {
            type = CateringType.Snack;
        }

        var partialInformation = new PartialStopIndexFactory(_stops, attribute?.Teilstreckenhinweis);
        return new CateringInformation(
            type,
            partialInformation.From,
            partialInformation.To);
    }

    private bool IsClosed()
    {
        return _prioritizedMessages.Any(m =>
            ClosedTexts.Any(text => m.Text.Contains(text)
            ));
    }

    private bool IsRestaurant([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "BR");
        return attribute is not null;
    }

    private bool IsBistro([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "QP");
        return attribute is not null;
    }
    
    private bool IsSnack([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute =_zugattribute.FirstOrDefault(attribute => attribute.Key == "SN");
        return attribute is not null;
    }

    private bool IsSeatServed([MaybeNullWhen(false)] out Zugattribut attribute)
    {
        attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "MP");
        if (attribute is null)
        {
            attribute = _zugattribute.FirstOrDefault(attribute => attribute.Key == "MN");
        }
        return attribute is not null;
    }
}
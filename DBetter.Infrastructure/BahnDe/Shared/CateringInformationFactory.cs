using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class CateringInformationFactory(VerbindungsAbschnitt abschnitt)
{
    private static readonly string[] ClosedTexts =
    [
        "Das Bordrestaurant/Bordbistro ist geschlossen.",
        "Zug verkehrt ohne gastronomische Bewirtschaftung."
    ];
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

        var partialInformation = new PartialStopIndexFactory(abschnitt.Halte, attribute?.Teilstreckenhinweis);
        return new CateringInformation(
            type,
            partialInformation.From,
            partialInformation.To);
    }

    private bool IsClosed()
    {
        return abschnitt.PriorisierteMeldungen.Any(m =>
            ClosedTexts.Any(text => m.Text.Contains(text)
            ));
    }

    private bool IsRestaurant([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "BR");
        return attribute is not null;
    }

    private bool IsBistro([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "QP");
        return attribute is not null;
    }
    
    private bool IsSnack([MaybeNullWhen(false) ]out Zugattribut attribute)
    {
        //TODO: Find out what this is for
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "SN");
        return attribute is not null;
    }

    private bool IsSeatServed([MaybeNullWhen(false)] out Zugattribut attribute)
    {
        attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "MP");
        if (attribute is null)
        {
            attribute = abschnitt.Verkehrsmittel.Zugattribute.FirstOrDefault(attribute => attribute.Key == "MN");
        }
        return attribute is not null;
    }
}
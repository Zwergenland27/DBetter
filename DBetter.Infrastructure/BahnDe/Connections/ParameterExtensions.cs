using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public static class ParameterExtensions
{
    public static ReiseAnfrage ToRequest(this ConnectionRequest request, string? page)
    {
        return new ReiseAnfrage
        {
            AbfahrtsHalt = request.GetAbfahrtsHalt(),
            AnkunftsHalt = request.GetAnkunftsHalt(),
            AnfrageZeitpunkt = request.GetAnfrageZeitpunkt(),
            Klasse = request.GetKlasse(),
            AnkunftSuche = request.GetAnkunftSuche(),
            Produktgattungen = request.GetProduktgattungen(),
            Reisende = request.GetReisende(),
            MaxUmstiege = request.GetMaxUmstiege(),
            MinUmstiegszeit = request.GetMinUmstiegszeit(),
            SchnelleVerbindungen = false,
            SitzplatzOnly = false,
            BikeCarriage = request.AnyBikeCarriage(),
            NurDeutschlandTicketVerbindungen = false,
            Zwischenhalte = request.GetZwischenhalte(),
            PagingReference = page,
        };
    }

    public static TeilstreckeAnfrage ToRequest(
        this ReiseAnfrage bahnRequest,
        string contextId,
        EvaNumber fixedStartEvaNumber,
        TravelTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        TravelTime fixedEndTime)
    {
        var fixedSectionBegin = new TeilstreckenStop
        {
            ExtId = fixedStartEvaNumber.Value.ToString(),
            Zeitpunkt = fixedStartTime.Planned.ToBahnTime()
        };
        
        var fixedSectionEnd = new TeilstreckenStop
        {
            ExtId = fixedEndEvaNumber.Value.ToString(),
            Zeitpunkt = fixedEndTime.Planned.ToBahnTime()
        };
        
        return new TeilstreckeAnfrage
        {
            Klasse = bahnRequest.Klasse,
            AnkunftSuche = bahnRequest.AnkunftSuche,
            Produktgattungen = bahnRequest.Produktgattungen,
            Reisende = bahnRequest.Reisende,
            MaxUmstiege = bahnRequest.MaxUmstiege,
            SchnelleVerbindungen = bahnRequest.SchnelleVerbindungen,
            SitzplatzOnly = bahnRequest.SitzplatzOnly,
            BikeCarriage = bahnRequest.BikeCarriage,
            NurDeutschlandTicketVerbindungen = bahnRequest.NurDeutschlandTicketVerbindungen,
            Zwischenhalte = bahnRequest.Zwischenhalte,
            FixedTeilstrecke = new FixedTeilstrecke
            {
                Begin = fixedSectionBegin,
                End = fixedSectionEnd
            },
            OriginalCtxRecon = contextId
        };
    }

    private static string GetAbfahrtsHalt(this ConnectionRequest request)
    {
        return request.Route.DepartureStop.AsFuzzy();
    }

    private static string GetAnkunftsHalt(this ConnectionRequest request)
    {
        return request.Route.ArrivalStop.AsFuzzy();
    }

    private static string GetAnfrageZeitpunkt(this ConnectionRequest request)
    {
        if (request.DepartureTime is not null)
        {
            return request.DepartureTime.Value.ToBahnTime();
        }

        if (request.ArrivalTime is not null)
        {
            return request.ArrivalTime.Value.ToBahnTime();
        }

        throw new BahnDeException("ConnectionService.MapAnfrageZeitpunkt", "Neither departureTime nor arrivalTime has been set");
    }

    private static Klasse GetKlasse(this ConnectionRequest request)
    {
        return request.Options.ComfortClass switch
        {
            ComfortClass.First => Klasse.KLASSE_1,
            ComfortClass.Second => Klasse.KLASSE_2,
            _ => throw new BahnDeException("ConnectionService.GetKlasse", "Unknown comfort class")
        };
    }
    
    private static AnkunftSuche GetAnkunftSuche(this ConnectionRequest request)
    {
        if (request.DepartureTime is not null)
        {
            return AnkunftSuche.ABFAHRT;
        }

        if (request.ArrivalTime is not null)
        {
            return AnkunftSuche.ANKUNFT;
        }

        throw new BahnDeException("ConnectionService.GetAnkunftSuche", "Neither departureTime nor arrivalTime has been set");
    }

    private static List<Produktgattung> GetProduktgattungen(this ConnectionRequest request)
    {
        return request.Route.AllowedOnFirstSection.GetProduktgattung();
    }

    private static List<Reisender> GetReisende(this ConnectionRequest request)
    {
        if (!request.Passengers.Any())
        {
            return
            [
                new Reisender()
                {
                    Alter = [],
                    Anzahl = 1,
                    Ermaessigungen = [Ermaessigung.Keine()],
                    Typ = ReisenderTyp.ERWACHSENER
                }
            ];
        }
        
        var bikes = request.Passengers.Sum(passenger => passenger.Options.Bikes);
        var dogs =  request.Passengers.Sum(passenger => passenger.Options.Dogs);
        
        var reisende = request.Passengers.Select(passenger => passenger.ToReisender()).ToList();

        if (bikes > 0)
        {
            reisende.Add(new Reisender
            {
                Alter = [],
                Anzahl = bikes,
                Ermaessigungen = [Ermaessigung.Keine()],
                Typ = ReisenderTyp.FAHRRAD
            });
        }

        if (dogs > 0)
        {
            reisende.Add(new Reisender
            {
                Alter = [],
                Anzahl = dogs,
                Ermaessigungen = [Ermaessigung.Keine()],
                Typ = ReisenderTyp.HUND
            });
        }
        
        return reisende;
    }

    private static Reisender ToReisender(this Passenger passenger)
    {
        if(passenger.Age is null) throw new BahnDeException("ConnectionService.ToReisender", "Passenger with birthday is not supported yet");

        var age = passenger.Age.Value;

        ReisenderTyp typ = age switch
        {
            <= 5 => ReisenderTyp.KLEINKIND,
            <= 14 => ReisenderTyp.FAMILIENKIND,
            <= 26 => ReisenderTyp.JUGENDLICHER,
            <= 64 => ReisenderTyp.ERWACHSENER, 
            _ => ReisenderTyp.SENIOR
        };

        return new Reisender
        {
            Alter = [age.ToString()],
            Anzahl = 1,
            Ermaessigungen = passenger.Discounts.ToErmaessigungen(),
            Typ = typ
        };
    }

    private static List<Ermaessigung> ToErmaessigungen(this IEnumerable<PassengerDiscount> discounts)
    {
        var ermaessigungen = discounts.Select(discount => discount.ToErmaessigung()).ToList();

        return ermaessigungen.Any() ? ermaessigungen : [Ermaessigung.Keine()];
    }

    private static Ermaessigung ToErmaessigung(this PassengerDiscount discount)
    {
        if(discount.ValidUntil is not null) throw new BahnDeException("ConnectionService.ToErmaessigung", "Discount with validity date is not supported yet");
        ArtErmaessigung art = discount.Type switch
        {
            DiscountType.BahnCard25 => ArtErmaessigung.BAHNCARD25,
            DiscountType.BahnCard50 => ArtErmaessigung.BAHNCARD50,
            DiscountType.BahnCard100 => ArtErmaessigung.BAHNCARD100,
            _ => throw new BahnDeException("ConnectionService.ToErmaessigung", "Unknown discount type")
        };

        KlasseErmaessigung klasse = discount.ComfortClass switch
        {
            ComfortClass.First => KlasseErmaessigung.KLASSE_1,
            ComfortClass.Second => KlasseErmaessigung.KLASSE_2,
            _ => throw new BahnDeException("ConnectionService.ToErmaessigung", "Unknown comfort class")
        };

        return new Ermaessigung
        {
            Art = art,
            Klasse = klasse,
        };
    }
    
    private static int GetMaxUmstiege(this ConnectionRequest request)
    {
        return request.Options.MaxTransfers;
    }

    private static int GetMinUmstiegszeit(this ConnectionRequest request)
    {
        return request.Options.MinTransferMinutes;
    }

    private static bool AnyBikeCarriage(this ConnectionRequest request)
    {
        return request.Passengers.Any(passenger => passenger.Options.Bikes > 0);
    }

    private static List<Zwischenhalt> GetZwischenhalte(this ConnectionRequest request)
    {
        var zwischenhalte = new List<Zwischenhalt>();

        if (request.Route.FirstStopOver is not null)
        {
            var stopover =  request.Route.FirstStopOver;
            var allowedVehicles = request.Route.AllowedOnSecondSection;
            
            if(allowedVehicles is null) throw new BahnDeException("ConnectionService.GetZwischenhalte", "Allowed vehicles not set for first stopover");
            
            zwischenhalte.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.StayMinutes,
                Id = stopover.Stop.AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = allowedVehicles.GetProduktgattung()
            });
        }
        
        if (request.Route.SecondStopOver is not null)
        {
            var stopover =  request.Route.SecondStopOver;
            var allowedVehicles = request.Route.AllowedOnThirdSection;
            
            if(allowedVehicles is null) throw new BahnDeException("ConnectionService.GetZwischenhalte", "Allowed vehicles not set for second stopover");
            
            zwischenhalte.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.StayMinutes,
                Id = stopover.Stop.AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = allowedVehicles.GetProduktgattung()
            });
        }
        
        return zwischenhalte;
    }

    private static string ToBahnTime(this DateTime date)
    {
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeFromUtc(date, germanTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
    }

    private static List<Produktgattung> GetProduktgattung(this AllowedVehicles allowedVehicles)
    {
        var produktgattungen = new List<Produktgattung>();

        if (allowedVehicles.HighSpeed)
        {
            produktgattungen.AddRange([
                Produktgattung.ICE]);
        }

        if (allowedVehicles.Intercity)
        {
            produktgattungen.AddRange([
                Produktgattung.EC_IC,
                Produktgattung.IR]);
        }

        if (allowedVehicles.Regional)
        {
            produktgattungen.AddRange([
                Produktgattung.REGIONAL]);
        }

        if (allowedVehicles.PublicTransport)
        {
            produktgattungen.AddRange([
                Produktgattung.SBAHN,
                Produktgattung.BUS,
                Produktgattung.SCHIFF,
                Produktgattung.UBAHN,
                Produktgattung.TRAM,
                Produktgattung.ANRUFPFLICHTIG]);
        }

        return produktgattungen;
    }
}
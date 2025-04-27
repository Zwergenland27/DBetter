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
    public static List<StationId> GetStops(this ConnectionRequest request)
    {
        List<StationId> stationIds = [];

        stationIds.Add(request.Route.OriginStationId);
        stationIds.Add(request.Route.DestinationStationId);

        if(request.Route.FirstStopover is not null)
        {
            stationIds.Add(request.Route.FirstStopover.StationId);
        }

        if(request.Route.SecondStopover is not null)
        {
            stationIds.Add(request.Route.SecondStopover.StationId);
        }

        return stationIds;
    }
    public static ReiseAnfrage ToRequest(this ConnectionRequest request, Dictionary<StationId, EvaNumber> requestStationEvas, string? page)
    {
        return new ReiseAnfrage
        {
            AbfahrtsHalt = request.GetAbfahrtsHalt(requestStationEvas),
            AnkunftsHalt = request.GetAnkunftsHalt(requestStationEvas),
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
            Zwischenhalte = request.GetZwischenhalte(requestStationEvas),
            PagingReference = page,
        };
    }

    public static TeilstreckeAnfrage ToRequest(
        this ReiseAnfrage bahnRequest,
        string contextId,
        EvaNumber fixedStartEvaNumber,
        DateTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        DateTime fixedEndTime)
    {
        var fixedSectionBegin = new TeilstreckenStop
        {
            ExtId = fixedStartEvaNumber.Value,
            Zeitpunkt = fixedStartTime.ToBahnTime()
        };
        
        var fixedSectionEnd = new TeilstreckenStop
        {
            ExtId = fixedEndEvaNumber.Value,
            Zeitpunkt = fixedEndTime.ToBahnTime()
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

    private static string GetAbfahrtsHalt(this ConnectionRequest request, Dictionary<StationId, EvaNumber> requestStationEvas)
    {
        return requestStationEvas[request.Route.OriginStationId].AsFuzzy();
    }

    private static string GetAnkunftsHalt(this ConnectionRequest request, Dictionary<StationId, EvaNumber> requestStationEvas)
    {
        return requestStationEvas[request.Route.DestinationStationId].AsFuzzy();
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
        return request.ComfortClass switch
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
        return request.Route.MeansOfTransportFirstSection.GetProduktgattung();
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
        
        var bikes = request.Passengers.Sum(passenger => passenger.Bikes);
        var dogs =  request.Passengers.Sum(passenger => passenger.Dogs);
        
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
        return request.Route.MaxTransfers;
    }

    private static int GetMinUmstiegszeit(this ConnectionRequest request)
    {
        return request.Route.MinTransferTime;
    }

    private static bool AnyBikeCarriage(this ConnectionRequest request)
    {
        return request.Passengers.Any(passenger => passenger.Bikes > 0);
    }

    private static List<Zwischenhalt> GetZwischenhalte(this ConnectionRequest request, Dictionary<StationId, EvaNumber> requestStationEvas)
    {
        var zwischenhalte = new List<Zwischenhalt>();

        if (request.Route.FirstStopover is not null)
        {
            var stopover =  request.Route.FirstStopover;
            var meansOfTransport = request.Route.FirstStopover.MeansOfTransportNextSection;
            
            if(meansOfTransport is null) throw new BahnDeException("ConnectionService.GetZwischenhalte", "Allowed vehicles not set for first stopover");

            zwischenhalte.Add(new Zwischenhalt
            {
                AufenthaltsDauer = stopover.LengthOfStay,
                Id = requestStationEvas[stopover.StationId].AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = meansOfTransport.GetProduktgattung()
            });
        }
        
        if (request.Route.SecondStopover is not null)
        {
            var stopover =  request.Route.SecondStopover;
            var meansOfTransport = request.Route.SecondStopover.MeansOfTransportNextSection;
            
            if(meansOfTransport is null) throw new BahnDeException("ConnectionService.GetZwischenhalte", "Allowed vehicles not set for second stopover");
            
            zwischenhalte.Add(new Zwischenhalt
            {
                AufenthaltsDauer = stopover.LengthOfStay,
                Id = requestStationEvas[stopover.StationId].AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = meansOfTransport.GetProduktgattung()
            });
        }
        
        return zwischenhalte;
    }

    private static string ToBahnTime(this DateTime date)
    {
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeFromUtc(date, germanTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
    }

    private static List<Produktgattung> GetProduktgattung(this MeansOfTransport meansOfTransport)
    {
        var produktgattungen = new List<Produktgattung>();

        if (meansOfTransport.HighSpeedTrains)
        {
            produktgattungen.Add(Produktgattung.ICE);
        }

        if (meansOfTransport.FastTrains)
        {
            produktgattungen.AddRange([
                Produktgattung.EC_IC,
                Produktgattung.IR]);
        }

        if (meansOfTransport.RegionalTrains)
        {
            produktgattungen.Add(Produktgattung.REGIONAL);
        }

        if (meansOfTransport.SuburbanTrains)
        {
            produktgattungen.Add(Produktgattung.SBAHN);
        }
        
        if (meansOfTransport.UndergroundTrains)
        {
            produktgattungen.Add(Produktgattung.UBAHN);
        }
        
        if (meansOfTransport.Trams)
        {
            produktgattungen.Add(Produktgattung.TRAM);
        }
        
        if (meansOfTransport.Busses)
        {
            produktgattungen.Add(Produktgattung.BUS);
        }
        
        if (meansOfTransport.Boats)
        {
            produktgattungen.Add(Produktgattung.SCHIFF);
        }

        return produktgattungen;
    }
}
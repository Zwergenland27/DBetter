using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Domain.Users.ValueObjects;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;
using JourneyId = DBetter.Domain.Connections.ValueObjects.JourneyId;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

public static class DTOExtensions
{
    public static Connection ToDomain(this Verbindung verbindung, ConnectionRequestId requestId)
    {
        Offer? offer = null;
        if (verbindung.AngebotsPreis is not null)
        {
            offer = new Offer(
                verbindung.AngebotsPreisKlasse!.Value.ToDomainClass(),
                verbindung.AngebotsPreis.Betrag,
                verbindung.AngebotsPreis.Waehrung.ToDomainCurrency(),
                verbindung.HasTeilpreis
            );
        }

        List<Section> sections = [];

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK)
            {
                sections.Add(new WalkSection(
                    SectionId.CreateNew(),
                    abschnitt.Distanz!.Value,
                    abschnitt.AbschnittsDauer));
                continue;
            }
            
            sections.Add(abschnitt.ToDomain());
        }
        
        return new Connection(
            ConnectionId.CreateNew(),
            requestId,
            new TripId(verbindung.TripId),
            new ContextId(verbindung.CtxRecon),
            offer,
            verbindung.GetDomainMessages(),
            verbindung.Auslastungsmeldungen.GetDomainDemand(),
            verbindung.FahrradmitnahmeMoeglich.GetBikeCarriageInformation(),
            sections);
    }

    private static ComfortClass ToDomainClass(this Klasse klasse)
    {
        return klasse switch
        {
            Klasse.KLASSE_1 => ComfortClass.First,
            Klasse.KLASSE_2 => ComfortClass.Second,
            _ => throw new BahnDeException("ConnectionSuggestion", $"Unknown class {klasse}")
        };
    }

    private static Currency ToDomainCurrency(this Waehrung currency)
    {
        return currency switch
        {
            Waehrung.EUR => Currency.Euro,
            _ => throw new BahnDeException("ConnectionSuggestion", $"Unknown currency {currency}")
        };
    }

    private static List<PassengerInfo> GetDomainMessages(this IHasMessage obj)
    {
        return [];
    }

    private static Demand GetDomainDemand(this List<AuslastungsMeldung> meldungen)
    {
        var firstClassDemand = meldungen
            .Where(m => m.Klasse == Klasse.KLASSE_1)
            .Select(a => a.Stufe)
            .FirstOrDefault();
        
        var secondClassDemand = meldungen
            .Where(m => m.Klasse == Klasse.KLASSE_2)
            .Select(a => a.Stufe)
            .FirstOrDefault();

        return new Demand(
            firstClassDemand.ToDomainDemandStatus(),
            secondClassDemand.ToDomainDemandStatus());
    }

    private static DemandStatus ToDomainDemandStatus(this AuslastungsStufe stufe)
    {
        return stufe switch
        {
            AuslastungsStufe.Unknown => DemandStatus.Unknown,
            AuslastungsStufe.Low => DemandStatus.Low,
            AuslastungsStufe.Medium => DemandStatus.Medium,
            AuslastungsStufe.High => DemandStatus.High,
            AuslastungsStufe.Extreme => DemandStatus.Extreme,
            AuslastungsStufe.Overbooked => DemandStatus.Overbooked,
            _ => throw  new BahnDeException("ConnectionSuggestion", $"Unknown demand {stufe}")
        };
    }

    private static bool? GetBikeCarriageInformation(this Fahrradmitnahme? status)
    {
        if (status is null) return null;
        
        return status switch
        {
            Fahrradmitnahme.NICHT_BEWERTBAR => null,
            Fahrradmitnahme.NICHT_MOEGLICH => false,
            Fahrradmitnahme.MOEGLICH => true,
            _ => throw new BahnDeException("ConnectionSuggestion", $"Unknown bike carriage status: {status}")
        };
    }

    private static TransportSection ToDomain(this VerbindungsAbschnitt abschnitt)
    {
        StationName? destination = null;

        if (abschnitt.Verkehrsmittel.Richtung is not null)
        {
            var destinationResult = StationName.Create(abschnitt.Verkehrsmittel.Richtung);
            if (!destinationResult.HasFailed)
            {
                destination = destinationResult.Value;
            }
        }
        
        return TransportSection.Create(
            abschnitt.Auslastungsmeldungen.GetDomainDemand(),
            abschnitt.GetDomainSectionMessages(),
            new BahnJourneyId(abschnitt.JourneyId!),
            abschnitt.Verkehrsmittel.MittelText!,
            abschnitt.Verkehrsmittel.LangText!,
            destination,
            abschnitt.GetCateringInformation(),
            abschnitt.GetBikeCarriageInformation(),
            abschnitt.Halte.Select(h => h.ToDomain()).ToList());
    }
    
    private static List<RoutePassengerInfo> GetDomainSectionMessages(this IHasMessage obj)
    {
        return [];
    }

    private static CateringInformation GetCateringInformation(this VerbindungsAbschnitt abschnitt)
    {
        var verkehrsmittel = abschnitt.Verkehrsmittel;
        
        string? validityText = null;
        CateringType type = CateringType.NoInfo;
        
        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "BR"))
        {
            type = CateringType.Restaurant;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "BR").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "QP"))
        {
            type = CateringType.Bistro;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "QP").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "MP"))
        {
            type = CateringType.SeatServed;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "MP").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "SN"))
        {
            type = CateringType.Snack;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "SN").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "KG"))
        {
            type = CateringType.None;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "KG").Teilstreckenhinweis;
        }

        if (verkehrsmittel.ProduktGattung is not Produktgattung.ICE and not Produktgattung.EC_IC and not Produktgattung.IR)
        {
            type =  CateringType.None;
        }

        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, abschnitt.Halte);
        
        return new CateringInformation(
            type,
            partialSectionIndices.Item1,
            partialSectionIndices.Item2);
    }

    private static BikeCarriage GetBikeCarriageInformation(this VerbindungsAbschnitt abschnitt)
    {
        var verkehrsmittel = abschnitt.Verkehrsmittel;

        string? validityText = null;
        BikeStatus status = BikeStatus.NoInfo;
        
        if (abschnitt.HimMeldungen.Any(m => m.Text != null && m.Text.Contains("Die Mitnahme von Fahrrädern ist nicht möglich.")))
        {
            status = BikeStatus.NotPossible;
        }
        
        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "FB"))
        {
            status = BikeStatus.Limited;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "FB").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "FR"))
        {
            status = BikeStatus.ReservationRequired;
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "FR").Teilstreckenhinweis;
        }
        
        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, abschnitt.Halte);
        
        return new BikeCarriage(
            status,
            partialSectionIndices.Item1,
            partialSectionIndices.Item2);
    }

    private static void GetAccessibilityInformation(this VerbindungsAbschnitt abschnitt)
    {
        var verkehrsmittel = abschnitt.Verkehrsmittel;
        
        string? validityText = null;
        
        if(verkehrsmittel.Zugattribute.Any(a => a.Key is "RZ"))
        {
            //Einstieg mit Rollstuhl stufenfrei
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "RZ").Teilstreckenhinweis;
        }

        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "RH"))
        {
            //Vehicle mounted access aid
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "RH").Teilstreckenhinweis;
        }
        
        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "EA"))
        {
            //Behindertengerechte Ausstattung
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "EA").Teilstreckenhinweis;
        }
        
        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "RG"))
        {
            //Rollstuhlgerechtes Fahrzeug
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "RG").Teilstreckenhinweis;
        }
        
        if (verkehrsmittel.Zugattribute.Any(a => a.Key is "RO"))
        {
            //Space for wheelchair
            validityText = verkehrsmittel.Zugattribute.First(a => a.Key is "RO").Teilstreckenhinweis;
        }
        
        
        var partialSectionIndices = GetPartialSectionValidityInfos(validityText, abschnitt.Halte);
    }

    private static Tuple<StopIndex, StopIndex> GetPartialSectionValidityInfos(string? text, List<Halt> halte)
    {
        var firstStopIndex = new StopIndex(halte.First().RouteIdx);
        var lastStopIndex = new StopIndex(halte.Last().RouteIdx);
        
        if (text is null)
        {
            return new (firstStopIndex, lastStopIndex);
        }
        
        var bracesRemoved = text.Substring(1, text.Length - 2);
        var stationNames =  bracesRemoved.Split(" - ");
        
        firstStopIndex = new StopIndex(halte.First(h => h.Name == stationNames[0]).RouteIdx);
        lastStopIndex = new StopIndex(halte.First(h => h.Name == stationNames[1]).RouteIdx);
        
        return new(firstStopIndex, lastStopIndex);
    }

    private static Stop ToDomain(this Halt halt)
    {
        StationInfoId? infoId = null;
        if (halt.BahnhofsInfoId is not null)
        {
            infoId = StationInfoId.Create(halt.BahnhofsInfoId).Value;
        }
        
        Platform? platform = null;
        if (halt.Gleis is not null)
        {
            var planned =  halt.Gleis!;
            var real = halt.EzGleis;

            var type = PlatformType.Platform;
            
            platform = new Platform(planned, real, type);
        }
        
        var additional = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.additional");
        var cancelled = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.cancelled");
        var exitOnly = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.entry.disabled");
        var entryOnly = halt.RisNotizen.Any(n => n.Key is "text.realtime.stop.exit.disabled");
        
        DepartureTime? departureTime = null;
        if (halt.AbfahrtsZeitpunkt is not null)
        {
            var planned = halt.AbfahrtsZeitpunkt.ConvertToDateTime()!.Value;
            var real = halt.EzAbfahrtsZeitpunkt.ConvertToDateTime();
            
            departureTime = new DepartureTime(planned, real);
        }
        
        ArrivalTime? arrivalTime = null;
        if (halt.AnkunftsZeitpunkt is not null)
        {
            var planned = halt.AnkunftsZeitpunkt.ConvertToDateTime()!.Value;
            var real = halt.EzAnkunftssZeitpunkt.ConvertToDateTime();
            
            arrivalTime = new ArrivalTime(planned, real);
        }
        
        return new Stop(
            StationName.Create(halt.Name).Value,
            EvaNumber.Create(halt.ExtId).Value,
            infoId,
            new StopIndex(halt.RouteIdx),
            platform,
            halt.Auslastungsmeldungen.GetDomainDemand(),
            additional,
            cancelled,
            exitOnly,
            entryOnly,
            departureTime,
            arrivalTime,
            halt.GetDomainMessages());
    }

    public static DateTime? ConvertToDateTime(this string? bahnDateString)
    {
        if (bahnDateString is null) return null;
        var germanTime = DateTime.Parse(bahnDateString);
        
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeToUtc(germanTime, germanTimeZone);
    }
}
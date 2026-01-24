using DBetter.Application.Shared;
using DBetter.Contracts.Requests.CreateRequest;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class PassengerInformationTextFactory
{
    private List<RisNotiz> _risNotizen;
    private List<HimMeldung> _himMeldung;
    private List<PriorisierteMeldung> _priorisierteMeldung;
    
    private List<ITrainRunStop> _routeStops;

    public PassengerInformationTextFactory(Fahrt fahrt)
    {
        _risNotizen = fahrt.RisNotizen;
        _himMeldung = fahrt.HimMeldungen ?? [];
        _priorisierteMeldung = fahrt.PriorisierteMeldungen;
        _routeStops = fahrt.Halte.Cast<ITrainRunStop>().ToList();
    }

    public PassengerInformationTextFactory(VerbindungsAbschnitt verbindungsAbschnitt)
    {
        _risNotizen = verbindungsAbschnitt.RisNotizen;
        _himMeldung = verbindungsAbschnitt.HimMeldungen ?? [];
        _priorisierteMeldung = verbindungsAbschnitt.PriorisierteMeldungen;
        _routeStops = verbindungsAbschnitt.Halte.Cast<ITrainRunStop>().ToList();
    }
    
    public List<PassengerInformationDto> ExtractInformation()
    {
        var messages = new List<PassengerInformationDto>();
        foreach (var risNotiz in _risNotizen)
        {
            var fromStopIndex = risNotiz.RouteIdxFrom ?? _routeStops.First().RouteIdx;
            var toStopIndex =  risNotiz.RouteIdxTo ?? _routeStops.Last().RouteIdx;
            messages.Add(new PassengerInformationDto
            {
                OriginalText = new PassengerInformationText(risNotiz.Value),
                FromStopIndex = new StopIndex(fromStopIndex),
                ToStopIndex = new StopIndex(toStopIndex),
                Priority = null
            });
        }

        foreach (var prioritized in _priorisierteMeldung)
        {
            var existingMessage = messages.FirstOrDefault(message => message.OriginalText.Value == prioritized.Text);
            if (existingMessage is null) continue;

            existingMessage.Priority = prioritized.Prioritaet switch
            {
                Prioritaet.HOCH => PassengerInformationPriority.High,
                Prioritaet.NIEDRIG => PassengerInformationPriority.Low,
                _ => throw new ArgumentOutOfRangeException($"Priority {prioritized.Prioritaet} is not supported")
            };
        }

        return messages.Distinct().ToList();
    }
}
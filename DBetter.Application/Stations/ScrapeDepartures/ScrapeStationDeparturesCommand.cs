

using CleanMediator.Commands;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations.ScrapeDepartures;

public record ScrapeStationDeparturesCommand(StationId StationId, DateOnly ForDay): ICommand;
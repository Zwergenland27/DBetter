using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.Requests;

public record ConnectionExtractorResult(
    List<TrainCirculation> TrainCirculationsToCreate,
    List<TrainRun> TrainRunsToCreate,
    List<PassengerInformation> PassengerInformationToCreate,
    List<Station> StationsToCreate,
    List<Connection> FoundConnections);
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.TrainParts;
using Shouldly;

namespace DBetter.Domain.Tests.TrainPartIdentifier;

public class PlannedTrainPartIdentifierTests
{
    public static List<RouteStopSnapshot> Route = [
        new RouteStopSnapshot(0, StationId.Create("8785598c-2f51-44da-ac47-c162b4af91e0").Value),
        new RouteStopSnapshot(1, StationId.Create("20f31a0c-7a39-4375-a6a1-4117ba997cd9").Value),
        new RouteStopSnapshot(2, StationId.Create("497d1216-32f2-438e-a20b-f84d63f1f3e2").Value),
        new RouteStopSnapshot(3, StationId.Create("f85558cb-219a-4e5a-9914-7923bce779a9").Value),
        new RouteStopSnapshot(4, StationId.Create("4b9f8187-0376-4b2c-9f5a-ef694bcf7551").Value),
        new RouteStopSnapshot(5, StationId.Create("47ac9e49-3ce0-44e2-b358-b6e6c3d30b62").Value)
    ];
    
    public static PlannedCoachLayoutId FirstLayoutId = PlannedCoachLayoutId.Create("34fbb2b8-27ef-41cc-a857-ccea22c9dbe9").Value;
    
    public static PlannedCoachLayoutId SecondLayoutId = PlannedCoachLayoutId.Create("6d636966-972f-4475-ac5b-dfb669d2bc5e").Value;
    
    public static PlannedCoachLayoutId ThirdLayoutId = PlannedCoachLayoutId.Create("b47a75ec-d0b4-4140-8405-8c18b1a6c0eb").Value;
    
    [Theory]
    [ClassData(typeof(MissingOriginStopTestData))]
    public void Resolve_ShouldReturnFirstSection_WhenMissing(List<StationId> observedStations)
    {
        //Arrange
        var identifier = new PlannedTrainPartIdentifier(Route);
        foreach (var stationId in observedStations)
        {
            identifier.AddObservation(stationId, [FirstLayoutId]);
        }

        //Act
        var resolved = identifier.Resolve(out var departureStationToScrape, out var arrivalStationToScrape, out var result);

        //Assert
        resolved.ShouldBe(false);
        departureStationToScrape.ShouldBe(Route[0].StationId);
        arrivalStationToScrape.ShouldBe(Route[1].StationId);
        result.ShouldBe(null);
    }
    
    [Theory]
    [ClassData(typeof(MissingDestinationStopTestData))]
    public void Resolve_ShouldReturnLastSection_WhenMissing(List<StationId> observedStations)
    {
        //Arrange
        var identifier = new PlannedTrainPartIdentifier(Route);
        identifier.AddObservation(Route[0].StationId, [FirstLayoutId]);
        foreach (var stationId in observedStations)
        {
            identifier.AddObservation(stationId, [FirstLayoutId]);
        }
        
        //Act
        var resolved = identifier.Resolve(out var departureStationToScrape, out var arrivalStationToScrape, out var result);

        //Assert
        resolved.ShouldBe(false);
        departureStationToScrape.ShouldBe(Route[^2].StationId);
        arrivalStationToScrape.ShouldBe(Route[^1].StationId);
        result.ShouldBe(null);
    }

    [Theory]
    [ClassData(typeof(SimpleTrainPartsTestData))]
    public void Resolve_ShouldReturnResult_WhenFirstAndLastAreEqual(List<PlannedCoachLayoutId> layoutIds)
    {
        //Arrange
        var identifier = new PlannedTrainPartIdentifier(Route);
        identifier.AddObservation(Route[0].StationId, layoutIds);
        identifier.AddObservation(Route[^2].StationId, layoutIds);

        var expectedResult = new List<PlannedTrainPart>();
        foreach (var layoutId in layoutIds)
        {
            expectedResult.Add(new (Route[0].StationId, Route[^1].StationId, layoutId));
        }

        //Act
        var resolved = identifier.Resolve(out var departureStationToScrape, out var arrivalStationToScrape, out var result);

        //Assert
        resolved.ShouldBe(true);
        departureStationToScrape.ShouldBe(null);
        arrivalStationToScrape.ShouldBe(null);
        result!.OrderBy(r => r.PlannedLayoutId.Value)
            .ShouldBeEquivalentTo(expectedResult.OrderBy(r => r.PlannedLayoutId.Value));
    }

    [Theory]
    [ClassData(typeof(AmbigousMiddleStationTestData))]
    public void Resolve_ShouldReturnMiddleStop_WhenAmbiguousTrainPartsFound(List<(StationId, List<PlannedCoachLayoutId>)> observations, StationId expectedDepartureStation, StationId expectedArrivalStation)
    {
        //Arrange
        var identifier = new PlannedTrainPartIdentifier(Route);
        foreach (var (stationId, coachLayoutIds) in observations)
        {
            identifier.AddObservation(stationId, coachLayoutIds);
        }

        //Act
        var resolved = identifier.Resolve(out var departureStationToScrape, out var arrivalStationToScrape, out var result);

        //Assert
        resolved.ShouldBe(false);
        departureStationToScrape.ShouldBe(expectedDepartureStation);
        arrivalStationToScrape.ShouldBe(expectedArrivalStation);
        result.ShouldBeEquivalentTo(null);
    }

    [Theory]
    [ClassData(typeof(ResolvedAmbigousMiddleStationTestData))]
    public void Resolve_ShouldReturnResult_WhenTwoFollowingStationsFound(
        List<(StationId, List<PlannedCoachLayoutId>)> observations, List<PlannedTrainPart> expectedResult)
    {
        //Arrange
        var identifier = new PlannedTrainPartIdentifier(Route);
        foreach (var (stationId, coachLayoutIds) in observations)
        {
            identifier.AddObservation(stationId, coachLayoutIds);
        }

        //Act
        var resolved = identifier.Resolve(out var departureStationToScrape, out var arrivalStationToScrape, out var result);
        
        //Assert
        
        resolved.ShouldBe(true);
        departureStationToScrape.ShouldBe(null);
        arrivalStationToScrape.ShouldBe(null);
        result!.OrderBy(r => r.PlannedLayoutId.Value)
            .ShouldBeEquivalentTo(expectedResult.OrderBy(r => r.PlannedLayoutId.Value));
    }
    
    //TODO: Test für nicht gefunden -> Mittensuche implementieren
    //TODO: Test für Gesamtheit implementieren (Zugtrennung, Zugvereinigung)
    //TODO: Test für Mehrfachteilung implementieren, um zu schauen was passiert
}
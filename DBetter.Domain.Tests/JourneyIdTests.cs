using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using Shouldly;

namespace DBetter.Domain.Tests;

public class JourneyIdTests
{
    [Theory]
    [ClassData(typeof(JourneyIdTestData))]
    public void OriginEvaNumber_ShouldBeExtracted(JourneyIdValues testTestData)
    {
        //Arrange
        var bahnJourneyId = BahnJourneyId.Create(testTestData.JourneyId);
        var expectedEvaNumber = testTestData.Origin;
        
        //Act
        var originEvaNumber = bahnJourneyId.OriginEvaNumber;
        
        //Assert
        originEvaNumber.ShouldBe(expectedEvaNumber);
    }
    
    [Theory]
    [ClassData(typeof(JourneyIdTestData))]
    public void DestinationEvaNumber_ShouldBeExtracted(JourneyIdValues testTestData)
    {
        //Arrange
        var bahnJourneyId = BahnJourneyId.Create(testTestData.JourneyId);
        var expectedEvaNumber = testTestData.Destination;
        
        //Act
        var destionationEvaNumber = bahnJourneyId.DestinationEvaNumber;
        
        //Assert
        destionationEvaNumber.ShouldBe(expectedEvaNumber);
    }
    
    [Theory]
    [ClassData(typeof(JourneyIdTestData))]
    public void OperatingDay_ShouldBeExtracted(JourneyIdValues testTestData)
    {
        //Arrange
        var bahnJourneyId = BahnJourneyId.Create(testTestData.JourneyId);
        var expectedOperatingDay = testTestData.OperatingDay;
        
        //Act
        var operatingDay = bahnJourneyId.OperatingDay;
        
        //Assert
        operatingDay.ShouldBe(expectedOperatingDay);
    }
}
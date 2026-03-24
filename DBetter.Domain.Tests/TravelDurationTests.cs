using DBetter.Domain.TrainRuns.ValueObjects;
using Shouldly;

namespace DBetter.Domain.Tests;

public class TravelDurationTests
{
    [Theory]
    [ClassData(typeof(TravelDurationTestData))]
    public void Create_ShouldCalculateCorrectDuration(HafasTime departureTime, HafasTime arrivalTime, ushort expectedDuration)
    {
        //Act
        var result = TravelDuration.Create(departureTime, arrivalTime);

        //Assert
        result.Minutes.ShouldBe(expectedDuration);
    }
}
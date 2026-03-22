using DBetter.Domain.TrainRuns.ValueObjects;
using Shouldly;

namespace DBetter.Domain.Tests;

public class HafasTimeTests
{
    [Theory]
    [ClassData(typeof(HafasTimeTestData))]
    public void Create_ShouldCalculateCorrectValue(string value, HafasTime expected)
    {
        //Act
        var hafasTime = HafasTime.Create(value);
        
        //Assert
        hafasTime.ShouldBe(expected);
    }
}
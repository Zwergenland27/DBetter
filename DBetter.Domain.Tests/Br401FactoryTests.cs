using DBetter.Domain.Vehicles;
using DBetter.Domain.Vehicles.ValueObjects;
using Shouldly;

namespace DBetter.Domain.Tests;

public class Br401FactoryTests
{
    // [Fact]
    // public void GenerateFromCoachTypes_ShouldReturnValidCoachSequence_WhenOrderIsNormal()
    // {
    //     //Arrange
    //     var factory = new Br401Factory();
    //     List<string> coachTypes = ["I8024-401.LDV", "I8026-401.LDV", "I8023-401.LDV", "I8023-401.LDV", "I8020-401.LDV", "I8031-401.LDV", "I8010-401.LDV", "I8014-401.LDV"];
    //     var expectedCoachSequence = new List<Coach>
    //     {
    //         Coach.CreateByConstructionType(new CoachId(0), "I4010"),
    //         Coach.Create(new CoachId(1), "I8024", "I8024-401.LDV"),
    //         Coach.Create(new CoachId(2), "I8026", "I8026-401.LDV"),
    //         Coach.Create(new CoachId(3), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(4), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(5), "I8020", "I8020-401.LDV"),
    //         Coach.Create(new CoachId(6), "I8031", "I8031-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(7), "I8040"),
    //         Coach.Create(new CoachId(8), "I8010", "I8010-401.LDV"),
    //         Coach.Create(new CoachId(9), "I8014", "I8014-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(10), "I4015"),
    //     };
    //     //Act
    //     
    //     var coachResult = factory.GenerateFromCoachTypes(coachTypes);
    //     
    //     //Assert
    //     coachResult.HasFailed.ShouldBe(false);
    //     coachResult.Value.OrderBy(c => c.Id.Value).ToList().ShouldBeEquivalentTo(expectedCoachSequence);
    // }
    //
    // [Fact]
    // public void GenerateFromCoachTypes_ShouldReturnValidCoachSequence_WhenOrderIsReversed()
    // {
    //     //Arrange
    //     var factory = new Br401Factory();
    //     List<string> coachTypes = ["I8014-401.LDV", "I8010-401.LDV", "I8031-401.LDV", "I8020-401.LDV", "I8023-401.LDV", "I8023-401.LDV", "I8026-401.LDV", "I8024-401.LDV"];
    //     var expectedCoachSequence = new List<Coach>
    //     {
    //         Coach.CreateByConstructionType(new CoachId(0), "I4010"),
    //         Coach.Create(new CoachId(1), "I8024", "I8024-401.LDV"),
    //         Coach.Create(new CoachId(2), "I8026", "I8026-401.LDV"),
    //         Coach.Create(new CoachId(3), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(4), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(5), "I8020", "I8020-401.LDV"),
    //         Coach.Create(new CoachId(6), "I8031", "I8031-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(7), "I8040"),
    //         Coach.Create(new CoachId(8), "I8010", "I8010-401.LDV"),
    //         Coach.Create(new CoachId(9), "I8014", "I8014-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(10), "I4015"),
    //     };
    //     //Act
    //     
    //     var coachResult = factory.GenerateFromCoachTypes(coachTypes);
    //     
    //     //Assert
    //     coachResult.HasFailed.ShouldBe(false);
    //     coachResult.Value.OrderBy(c => c.Id.Value).ToList().ShouldBeEquivalentTo(expectedCoachSequence);
    // }
    //
    // [Fact]
    // public void GenerateFromConstructionType_ShouldReturnValidCoachSequence_WhenOrderIsNormal()
    // {
    //     //Arrange
    //     var factory = new Br401Factory();
    //     List<string> constructionTypes = ["I4010", "I8024", "I8026", "I8023", "I8023", "I8020", "I8031", "I8040", "I8010", "I8014", "I4015"];
    //     var expectedCoachSequence = new List<Coach>
    //     {
    //         Coach.CreateByConstructionType(new CoachId(0), "I4010"),
    //         Coach.Create(new CoachId(1), "I8024", "I8024-401.LDV"),
    //         Coach.Create(new CoachId(2), "I8026", "I8026-401.LDV"),
    //         Coach.Create(new CoachId(3), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(4), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(5), "I8020", "I8020-401.LDV"),
    //         Coach.Create(new CoachId(6), "I8031", "I8031-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(7), "I8040"),
    //         Coach.Create(new CoachId(8), "I8010", "I8010-401.LDV"),
    //         Coach.Create(new CoachId(9), "I8014", "I8014-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(10), "I4015"),
    //     };
    //     //Act
    //     
    //     var coachResult = factory.GenerateFromConstructionTypes(constructionTypes);
    //     
    //     //Assert
    //     coachResult.HasFailed.ShouldBe(false);
    //     coachResult.Value.OrderBy(c => c.Id.Value).ToList().ShouldBeEquivalentTo(expectedCoachSequence);
    // }
    //
    // [Fact]
    // public void GenerateFromConstructionType_ShouldReturnValidCoachSequence_WhenOrderIsReversed()
    // {
    //     //Arrange
    //     var factory = new Br401Factory();
    //     List<string> coachTypes = ["I4015","I8014","I8010","I8040","I8031","I8020","I8023","I8023","I8026","I8024","I4010"];
    //     var expectedCoachSequence = new List<Coach>
    //     {
    //         Coach.CreateByConstructionType(new CoachId(0), "I4010"),
    //         Coach.Create(new CoachId(1), "I8024", "I8024-401.LDV"),
    //         Coach.Create(new CoachId(2), "I8026", "I8026-401.LDV"),
    //         Coach.Create(new CoachId(3), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(4), "I8023", "I8023-401.LDV"),
    //         Coach.Create(new CoachId(5), "I8020", "I8020-401.LDV"),
    //         Coach.Create(new CoachId(6), "I8031", "I8031-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(7), "I8040"),
    //         Coach.Create(new CoachId(8), "I8010", "I8010-401.LDV"),
    //         Coach.Create(new CoachId(9), "I8014", "I8014-401.LDV"),
    //         Coach.CreateByConstructionType(new CoachId(10), "I4015"),
    //     };
    //     //Act
    //     
    //     var coachResult = factory.GenerateFromConstructionTypes(coachTypes);
    //     
    //     //Assert
    //     coachResult.HasFailed.ShouldBe(false);
    //     coachResult.Value.OrderBy(c => c.Id.Value).ToList().ShouldBeEquivalentTo(expectedCoachSequence);
    // }
}
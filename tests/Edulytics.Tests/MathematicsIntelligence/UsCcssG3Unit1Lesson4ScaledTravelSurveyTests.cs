using Edulytics.Core.Mathematics.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored test contracts; do not execute before Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson4ScaledTravelSurveyTests
{
    private static readonly UsCcssG3Unit1Lesson4ScaledTravelSurvey.TravelCount[] Example =
    [
        new("Car", 4), new("Train", 5), new("Boat", 2),
        new("Balloon", 1), new("Plane", 3), new("Helicopter", 3)
    ];

    [Fact]
    public void HalfSymbolRepresentsOddStudentWithoutRounding()
    {
        Assert.Equal((2,1), UsCcssG3Unit1Lesson4ScaledTravelSurvey.GlyphCounts(5));
        Assert.Equal((2,0), UsCcssG3Unit1Lesson4ScaledTravelSurvey.GlyphCounts(4));
        Assert.Equal("● ● ◐", UsCcssG3Unit1Lesson4ScaledTravelSurvey.RepresentWithMarks(5));
        Assert.Equal("● ● ", UsCcssG3Unit1Lesson4ScaledTravelSurvey.RepresentWithMarks(4));
        Assert.Equal("", UsCcssG3Unit1Lesson4ScaledTravelSurvey.RepresentWithMarks(0));
    }

    [Fact]
    public void ClassSurveyMustHaveValidSixCategories()
    {
        Assert.True(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(Example, out _));
        Assert.False(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(null,out _));
        Assert.False(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(
            [new("Car", 4),new("car",5),new("Boat",2),new("Balloon",1),
                new("Plane",3),new("Helicopter",3)], out _));
        Assert.False(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryValidate(
            [new("Car",-1),new("Train",5),new("Boat",2),new("Balloon",1),
                new("Plane",3),new("Helicopter",3)], out _));
    }

    [Theory]
    [InlineData("G3-U01-L04-CD-TRAIN-WHOLE",2)]
    [InlineData("G3-U01-L04-CD-TRAIN-HALF",1)]
    [InlineData("G3-U01-L04-CD-PLANE-PLUS-TWO-WHOLE",2)]
    [InlineData("G3-U01-L04-CD-TOTAL",18)]
    public void OriginalCoolDownNumbersAcceptRightRejectWrong(string id, int n)
    {
        Assert.True(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeCoolDown(id,n,out var correct));
        Assert.True(correct);
        Assert.True(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeCoolDown(id,n+1,out var wrong));
        Assert.False(wrong);
    }

    [Fact]
    public void OpenReasoningIsNeverCertifiedByNumberOnly()
    {
        Assert.False(UsCcssG3Unit1Lesson4ScaledTravelSurvey.TryGradeCoolDown(
            "G3-U01-L04-CD-DRAW",0,out _));
        Assert.True(UsCcssG3Unit1Lesson4ScaledTravelSurvey.RequiresHumanReview("student-drawn-graph"));
        Assert.True(UsCcssG3Unit1Lesson4ScaledTravelSurvey.RequiresHumanReview("actual-survey-provenance"));
    }
}

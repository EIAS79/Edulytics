using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3ThirdLessonBuildRulerTests
{
    [Fact]
    public void SourceNumberTalkAndMarkZeroTickLogic()
    {
        var w=UsCcssG2Unit3ThirdLessonBuildRuler.NumberTalk("60","43","40","39");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3ThirdLessonBuildRuler.NumberTalk("60","43","40","49").Correct);
        Assert.True(UsCcssG2Unit3ThirdLessonBuildRuler.RulerTickMeaning(
            "0","1","10","18","3","15").Correct);
        Assert.False(UsCcssG2Unit3ThirdLessonBuildRuler.RulerTickMeaning(
            "0","1","10","18","3","18").Correct);
    }
    [Fact]
    public void IndependentlyWrittenVirtualRectanglesAndRulerExitAreNotSourceMeasurements()
    {
        var r=UsCcssG2Unit3ThirdLessonBuildRuler.FirstParty("23","17","6","19");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3ThirdLessonBuildRuler.FirstParty("23","17","7","19").Correct);
        Assert.True(UsCcssG2Unit3ThirdLessonBuildRuler.Exit("17").Correct);
        Assert.False(UsCcssG2Unit3ThirdLessonBuildRuler.Exit("21").Correct);
    }
}

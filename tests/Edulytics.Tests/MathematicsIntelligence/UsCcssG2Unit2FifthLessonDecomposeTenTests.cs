using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2FifthLessonDecomposeTenTests
{
    [Fact]
    public void OriginalTalkAndBothActivityCalculationsAreExact()
    {
        var w=UsCcssG2Unit2FifthLessonDecomposeTen.Warmup("10","9","20","18");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen.Warmup("10","9","20","19").Correct);
        Assert.True(UsCcssG2Unit2FifthLessonDecomposeTen.Activity1("73","7","12").Correct);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen.Activity1("73","8","12").Correct);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen.Activity1("73","7","2").Correct);
        Assert.True(UsCcssG2Unit2FifthLessonDecomposeTen
            .Activity2("55","35","2","15","27").Correct);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen
            .Activity2("55","35","3","15","27").Correct);
    }
    [Fact]
    public void FirstPartyExitIsSeparateFromAuthenticTeacherScoredManipulatives()
    {
        var r=UsCcssG2Unit2FifthLessonDecomposeTen.CoolDown("56","66","36");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen.CoolDown("55","66","36").Correct);
        Assert.False(UsCcssG2Unit2FifthLessonDecomposeTen.CoolDown("56","66","nope").Correct);
        Assert.True(UsCcssG2Unit2FifthLessonDecomposeTen.TeacherRubric.Count>=5);
    }
}

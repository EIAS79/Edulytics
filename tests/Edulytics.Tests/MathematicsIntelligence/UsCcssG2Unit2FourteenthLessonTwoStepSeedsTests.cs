using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2FourteenthLessonTwoStepSeedsTests
{
    [Fact]
    public void OriginalWarmupAndAndreTwoStepAnswersExact()
    {
        var w=UsCcssG2Unit2FourteenthLessonTwoStepSeeds.Warmup("19","39","59","89");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.Warmup("19","39","59","88").Correct);
        Assert.True(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SourceAndre("42","77").Correct);
        Assert.False(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SourceAndre("42","42").Correct);
    }
    [Fact]
    public void GroupThreeAnswersAndFinalTotalAreBothRequired()
    {
        var r=UsCcssG2Unit2FourteenthLessonTwoStepSeeds.GroupStories("29","22","41","92");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.GroupStories("29","22","41","51").Correct);
        Assert.True(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SectionSummary("37").Correct);
        Assert.False(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.SectionSummary("28").Correct);
        var i=UsCcssG2Unit2FourteenthLessonTwoStepSeeds.FirstParty("74","50","32","43");
        Assert.True(i.Correct);Assert.False(i.TeacherReviewNeeded);
        Assert.True(UsCcssG2Unit2FourteenthLessonTwoStepSeeds.TeacherRubric.Count>=6);
    }
}

using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3EleventhLessonSareeRibbonTests
{
    [Fact]
    public void WarmupAndFourSourceSareeStoriesCheckCorrectly()
    {
        var w=UsCcssG2Unit3EleventhLessonSareeRibbon.Warmup("38");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EleventhLessonSareeRibbon.Warmup("70").Correct);
        var r=UsCcssG2Unit3EleventhLessonSareeRibbon.SourceFour("26","31","36","63");
        Assert.True(r.Correct);Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EleventhLessonSareeRibbon.SourceFour("26","31","36","13").Correct);
    }
    [Fact]
    public void SeparateFirstPartyTasksDoNotAssertGenuinePublisherAssessment()
    {
        var r=UsCcssG2Unit3EleventhLessonSareeRibbon.FirstParty("36","76","55");
        Assert.True(r.Correct);Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit3EleventhLessonSareeRibbon.FirstParty("36","18","55").Correct);
        Assert.True(UsCcssG2Unit3EleventhLessonSareeRibbon.TeacherRubric.Count>=6);
    }
}

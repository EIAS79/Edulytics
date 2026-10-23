using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4FirstLessonNumberLineTests
{
    [Fact]
    public void SourceTickLabelsAndFreeChoicePointsUseOriginalZeroToTenBounds()
    {
        var ticks=UsCcssG2Unit4FirstLessonNumberLine.FirstSourceLine("3","4","6","7");
        Assert.True(ticks.Correct);
        Assert.True(ticks.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FirstLessonNumberLine.FirstSourceLine("3","4","6","8").Correct);

        var firstChoice=UsCcssG2Unit4FirstLessonNumberLine.SourcePoints("6","7","9","8");
        var otherChoice=UsCcssG2Unit4FirstLessonNumberLine.SourcePoints("6","10","9","0");
        Assert.True(firstChoice.Correct);
        Assert.True(otherChoice.Correct);
        Assert.True(otherChoice.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FirstLessonNumberLine.SourcePoints("6","6","9","0").Correct);
        Assert.False(UsCcssG2Unit4FirstLessonNumberLine.SourcePoints("6","11","9","0").Correct);
        Assert.False(UsCcssG2Unit4FirstLessonNumberLine.SourcePoints("6","7","9","9").Correct);
    }
    [Fact]
    public void IndependentNumbersDoNotCertifyPhysicalPaperOrPartnerDiscourse()
    {
        var digital=UsCcssG2Unit4FirstLessonNumberLine.IndependentlyAuthored("10","13","21");
        Assert.True(digital.Correct);
        Assert.False(digital.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FirstLessonNumberLine.IndependentlyAuthored("10","13","20").Correct);
        Assert.Contains(UsCcssG2Unit4FirstLessonNumberLine.TeacherRubric,
            s=>s.Contains("physical",StringComparison.OrdinalIgnoreCase));
    }
}

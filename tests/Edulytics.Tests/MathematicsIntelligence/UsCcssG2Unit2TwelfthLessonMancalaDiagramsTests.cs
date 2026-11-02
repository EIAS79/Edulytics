using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2TwelfthLessonMancalaDiagramsTests
{
    [Fact]
    public void OptionalNumericalCompanionCannotBeConfusedWithDiagramMatching()
    {
        var s=UsCcssG2Unit2TwelfthLessonMancalaDiagrams
            .SupplementalClareHan("38","70","38","38");
        Assert.True(s.Correct);
        Assert.True(s.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2TwelfthLessonMancalaDiagrams
            .SupplementalClareHan("38","38","38","38").Correct);
    }
    [Fact]
    public void FirstPartyMathDoesNotClaimRealSourcePrintedCardNumbers()
    {
        var s=UsCcssG2Unit2TwelfthLessonMancalaDiagrams
            .FirstParty("36","90","36","36");
        Assert.True(s.Correct);
        Assert.False(s.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2TwelfthLessonMancalaDiagrams
            .FirstParty("36","63","36","36").Correct);
        Assert.True(UsCcssG2Unit2TwelfthLessonMancalaDiagrams.TeacherRubric.Count>=6);
    }
}

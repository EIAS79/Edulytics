using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored only; do not run until Stage D.</summary>
public sealed class UsCcssG2Unit1SixthLessonEdulyticsCenterCheckTests
{
    [Fact]
    public void FourCorrectDigitCardsSolveAllEquationsOnce()
    {
        var r=UsCcssG2Unit1SixthLessonEdulyticsCenterCheck
            .CheckDigitBoard("5","9","3","2");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }

    [Theory]
    [InlineData("5","9","3","3")]
    [InlineData("5","9","4","2")]
    [InlineData("5","8","3","2")]
    [InlineData("5","9","3","abc")]
    public void SameCardTwiceOrFalseEquationIsRejected(
        string a,string b,string c,string d)
    {
        Assert.False(UsCcssG2Unit1SixthLessonEdulyticsCenterCheck
            .CheckDigitBoard(a,b,c,d).Correct);
    }
    [Fact]
    public void WarmUpAndAuthenticOptionalCenterChoicesRemainSeparate()
    {
        Assert.True(UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.CheckWarmup("36").Correct);
        Assert.False(UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.CheckWarmup("35").Correct);
        Assert.Equal(3,UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.SourceAlignedCenterChoices.Count);
        Assert.Equal(4,UsCcssG2Unit1SixthLessonEdulyticsCenterCheck.TeacherRubric.Count);
    }
}

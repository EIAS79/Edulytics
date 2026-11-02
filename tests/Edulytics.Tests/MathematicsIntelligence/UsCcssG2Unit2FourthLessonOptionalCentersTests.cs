using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Separate numeric checks from authentic physical board wins.</summary>
public sealed class UsCcssG2Unit2FourthLessonOptionalCentersTests
{
    [Fact]
    public void WarmupFourSourceMentalFactsAreExact()
    {
        var r=UsCcssG2Unit2FourthLessonOptionalCenters.CheckNumberTalk("3","63","36","33");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckNumberTalk("3","63","63","33").Correct);
    }
    [Fact]
    public void CaptureNeedsLegalSpinnerAndCardAndNeverInfersCapturedSquare()
    {
        var r=UsCcssG2Unit2FourthLessonOptionalCenters.CheckCapture("8","7","15","No");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckCapture("5","7","12","No").Correct);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckCapture("10","11","21","Yes").Correct);
    }
    [Fact]
    public void FiveRowMustMoveOnlyOneClipAndAcceptComposingTen()
    {
        Assert.True(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckFiveTwoTurns("25","8","33","25","9","34").Correct);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckFiveTwoTurns("25","8","33","26","9","35").Correct);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckFiveTwoTurns("25","8","33","25","8","33").Correct);
        Assert.True(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckIndependentCoolDown("13","33","45").Correct);
        Assert.False(UsCcssG2Unit2FourthLessonOptionalCenters
            .CheckIndependentCoolDown("13","32","45").Correct);
    }
}

using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2SixthLessonCompareMethodsTests
{
    [Fact]
    public void CorrectWarmupRequiresThirdEqualityToBeFalseAndDiagramToTradeTen()
    {
        var w=UsCcssG2Unit2SixthLessonCompareMethods.Warmup("True","True","False");
        Assert.True(w.Correct);Assert.True(w.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Warmup("True","True","True").Correct);
        Assert.True(UsCcssG2Unit2SixthLessonCompareMethods.Diagram("7","12","73").Correct);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Diagram("8","12","73").Correct);
    }
    [Fact]
    public void TargetStageFourRequiresLegalCardsModesAndNoNegativeResult()
    {
        var ok=UsCcssG2Unit2SixthLessonCompareMethods.Target(
            "99","4","Ones","95","7","Tens","25");
        Assert.True(ok.Correct);Assert.True(ok.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Target(
            "99","0","Ones","99","7","Tens","29").Correct);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Target(
            "99","4","Tens","95","7","Tens","25").Correct);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Target(
            "99","4","Ones","95","9","Tens","4").Correct);
        Assert.True(UsCcssG2Unit2SixthLessonCompareMethods.Cooldown("63","53","84").Correct);
        Assert.False(UsCcssG2Unit2SixthLessonCompareMethods.Cooldown("63","54","84").Correct);
    }
}

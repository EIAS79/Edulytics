using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C written-only; execution reserved for Stage D.</summary>
public sealed class UsCcssG2Unit1ThirdLessonEdulyticsCoolDownTests
{
    [Fact]
    public void TwentyCubesNeedThirteenHiddenWhenSevenAreVisible()
    {
        var r=UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyHiddenPart("13");
        Assert.True(r.Correct);
        Assert.False(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyHiddenPart("7").Correct);
    }
    [Fact]
    public void UnknownSubtrahendDiffersFromUnknownMinuendChallenge()
    {
        Assert.True(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyRemovedPart("11").Correct);
        Assert.False(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyRemovedPart("9").Correct);
        Assert.True(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyChallengeStartingAmount("25").Correct);
        Assert.False(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.VerifyChallengeStartingAmount("15").Correct);
    }
    [Fact]
    public void TwoOriginalEquationTruthQuestionsRequireDifferentAnswers()
    {
        Assert.True(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown
            .VerifyEquationsTruth("1","0").Correct);
        Assert.False(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown
            .VerifyEquationsTruth("1","1").Correct);
        Assert.False(UsCcssG2Unit1ThirdLessonEdulyticsCoolDown
            .VerifyEquationsTruth("anything","0").Correct);
        Assert.Equal(4,UsCcssG2Unit1ThirdLessonEdulyticsCoolDown.TeacherRubric.Count);
    }
}

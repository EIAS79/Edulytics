using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored only; run in Stage D, not now.</summary>
public sealed class UsCcssG2Unit1FourteenthLessonTapeDiagramsTests
{
    [Fact]
    public void SourceJuiceAndTwoPartyPairsMatchOutlinedMissingDifference()
    {
        Assert.True(UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.JuiceVotes,
            "Apple","Grape","7","8+7=15","15−8=7").Correct);
        Assert.True(UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Pizza","Burgers","11","8+11=19","19−8=11").Correct);
        Assert.True(UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Pizza","Hot dogs","5","14+5=19","19−14=5").Correct);
    }
    [Fact]
    public void WrongCategoryOrderAndTotalInsteadOfGapRejected()
    {
        Assert.False(UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Burgers","Pizza","11","8+11=19","19−8=11").Correct);
        Assert.False(UsCcssG2Unit1FourteenthLessonTapeDiagrams.VerifyTapes(
            UsCcssG2Unit1FourteenthLessonTapeDiagrams.PartyVotes,
            "Pizza","Burgers","27","8+11=19","19−8=11").Correct);
    }
    [Fact]
    public void NewIndependentEdulyticsExitTicketRequiresBothEquations()
    {
        Assert.True(UsCcssG2Unit1FourteenthLessonTapeDiagrams
            .CheckIndependentCoolDown("7","9+7=16","16-9=7").Correct);
        Assert.False(UsCcssG2Unit1FourteenthLessonTapeDiagrams
            .CheckIndependentCoolDown("7","9+7=16","16-7=9").Correct);
        Assert.Equal(5,UsCcssG2Unit1FourteenthLessonTapeDiagrams.TeacherRubric.Count);
    }
}

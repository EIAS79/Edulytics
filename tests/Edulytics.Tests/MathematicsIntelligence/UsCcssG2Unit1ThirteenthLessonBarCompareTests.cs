using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored tests, not executed before Stage D.</summary>
public sealed class UsCcssG2Unit1ThirteenthLessonBarCompareTests
{
    [Fact]
    public void PetsCompareRequiresBothCountingOnAndCountingBack()
    {
        Assert.True(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckSourcePetsDifference("9","8 + 9 = 17","17 − 8 = 9").Correct);
        Assert.False(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckSourcePetsDifference("25","8 + 9 = 17","17 − 8 = 9").Correct);
        Assert.False(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckSourcePetsDifference("9","8 + 9 = 17","17 − 9 = 8").Correct);
    }
    [Fact]
    public void DogCompareUsesTwoCategoryBarsAndRelatedEquations()
    {
        Assert.True(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckDogComparison("Bulldogs","Huskies","14 + 6 = 20","20 − 14 = 6").Correct);
        Assert.True(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckDogComparison("Pugs","Poodles","6 + 2 = 8","8 - 6 = 2").Correct);
        Assert.False(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckDogComparison("Poodles","Bulldogs","6 + 14 = 20","20 - 6 = 14").Correct);
        Assert.Equal(5,UsCcssG2Unit1ThirteenthLessonBarCompare.TeacherRubric.Count);
    }
    [Fact]
    public void OriginalExitTicketAndWarmUpRemainIndependent()
    {
        Assert.True(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckEdulyticsCoolDown("6","7+6=13","13-7=6").Correct);
        Assert.True(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckTrueFalseWarmUp("1","1","0").Correct);
        Assert.False(UsCcssG2Unit1ThirteenthLessonBarCompare
            .CheckTrueFalseWarmUp("1","1","1").Correct);
    }
}

using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored only for Stage D; tests intentionally not executed.</summary>
public sealed class UsCcssG2Unit1EighthLessonEdulyticsCoolDownTests
{
    [Fact]
    public void TwoIndependentSportsPopulationGraphsHaveDifferentCounts()
    {
        Assert.Equal(6,UsCcssG2Unit1EighthLessonEdulyticsCoolDown.ChildrenSports["Soccer"]);
        Assert.Equal(2,UsCcssG2Unit1EighthLessonEdulyticsCoolDown.AdultsSports["Soccer"]);
        Assert.True(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckSportsReading("5","4").Correct);
        Assert.False(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckSportsReading("4","5").Correct);
    }
    [Fact]
    public void OneSymbolPerPersonReadingAndCombinationRemainExact()
    {
        Assert.True(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckArtQuantities("7","7","2","11").Correct);
        Assert.False(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckArtQuantities("7","7","3","11").Correct);
        Assert.True(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckOriginalCoolDown("18","5").Correct);
    }
    [Fact]
    public void CannotInferWhyFromAQuantitativePictograph()
    {
        Assert.True(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckWhichQuestionsGraphCanAnswer("1","1","0","1","1").Correct);
        Assert.False(UsCcssG2Unit1EighthLessonEdulyticsCoolDown
            .CheckWhichQuestionsGraphCanAnswer("1","1","1","1","1").Correct);
        Assert.Equal(5,UsCcssG2Unit1EighthLessonEdulyticsCoolDown.TeacherRubric.Count);
    }
}

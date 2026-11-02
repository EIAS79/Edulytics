using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Author-only Stage C checks; NO execution until Stage D.</summary>
public sealed class UsCcssG2Unit1SecondLessonEdulyticsCoolDownTests
{
    [Theory]
    [InlineData("7",true)]
    [InlineData("6",false)]
    [InlineData("3",false)]
    [InlineData("-7",false)]
    [InlineData("?",false)]
    public void IndependentlyAuthoredTenCubeHiddenPartIsBounded(string answer,bool correct)
    {
        var r=UsCcssG2Unit1SecondLessonEdulyticsCoolDown.VerifyHiddenTen(answer);
        Assert.True(r.MachineScorable);
        Assert.Equal(correct,r.Correct);
    }
    [Theory]
    [InlineData("6",true)]
    [InlineData("4",false)]
    [InlineData("10",false)]
    public void UnknownInSubtractionIsTheRemovedPart(string answer,bool correct)
    {
        Assert.Equal(correct,UsCcssG2Unit1SecondLessonEdulyticsCoolDown
            .VerifySubtractionBlank(answer).Correct);
    }
    [Theory]
    [InlineData("6 + 4","10 - 4",true)]
    [InlineData("4 + 6","10 − 4",true)]
    [InlineData("7 + 3","10 - 4",false)]
    [InlineData("6 + 4","10 - 6",false)]
    [InlineData("10 - 4","6 + 4",false)]
    public void RelatedEquationsMustFitSameSixAndFourTower(
        string addition,string subtraction,bool correct)
    {
        var r=UsCcssG2Unit1SecondLessonEdulyticsCoolDown
            .VerifyRelatedEquations(addition,subtraction);
        Assert.Equal(correct,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }
    [Fact]
    public void NewTwoColorTowersAreTenCubesAndMatchCorrectSourceSkill()
    {
        var rows=UsCcssG2Unit1SecondLessonEdulyticsCoolDown.Towers;
        Assert.Equal(5,rows.Count);
        Assert.All(rows,x=>Assert.Equal(10,x.Blue+x.Yellow));
        Assert.Equal("C",rows.Single(x=>x.Yellow==7).Label);
        Assert.True(UsCcssG2Unit1SecondLessonEdulyticsCoolDown
            .VerifyTowerForTenMinusSeven("c").Correct);
        Assert.False(UsCcssG2Unit1SecondLessonEdulyticsCoolDown
            .VerifyTowerForTenMinusSeven("D").Correct);
    }
}

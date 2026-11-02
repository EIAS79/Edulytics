using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C tests authored but not run before Stage D.</summary>
public sealed class UsCcssG2Unit1NinthLessonEdulyticsCoolDownTests
{
    [Theory]
    [InlineData("13","6","Winter + Spring","26",true)]
    [InlineData("13","6","Winter + Fall","26",true)]
    [InlineData("13","6","Fall + Winter","26",true)]
    [InlineData("13","6","Spring + Fall","26",false)]
    [InlineData("14","6","Winter + Fall","26",false)]
    [InlineData("13","6","Winter + Fall","25",false)]
    public void OriginalSeasonGraphAllowsBothValidMakeTenPairs(
        string summer,string combo,string pair,string total,bool valid)
    {
        var r=UsCcssG2Unit1NinthLessonEdulyticsCoolDown
            .CheckSeasonGraph(summer,combo,pair,total);
        Assert.Equal(valid,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }
    [Fact]
    public void IndependentClubCoolDownIsSourceGoalButNotPublisherReproduction()
    {
        Assert.True(UsCcssG2Unit1NinthLessonEdulyticsCoolDown
            .CheckIndependentSchoolClubCoolDown("7","20","4").Correct);
        Assert.False(UsCcssG2Unit1NinthLessonEdulyticsCoolDown
            .CheckIndependentSchoolClubCoolDown("7","19","4").Correct);
        Assert.Equal(5,UsCcssG2Unit1NinthLessonEdulyticsCoolDown.TeacherRubric.Count);
    }
}

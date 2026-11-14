using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L07WholeByFractionLabTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:04:07:STUDENT-AR-128",F="uae.g6.u04.l07.whole_divide_nonunit_fraction";
    [Fact] public void LabReusesGrade5AndSupportsExactProperFraction()
    {
        var l=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void ExactProperFractionDivisionWorksInArabicAndRejectsInvalid()
    {
        var p=new Dictionary<string,int>{{"n",4},{"d",3},{"a",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"6"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,"٦"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"4"));
        var other=new Dictionary<string,int>{{"n",2},{"d",4},{"a",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,other,"8/3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,other,"16/6"));
        var invalid=new Dictionary<string,int>{{"n",2},{"d",0},{"a",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,invalid,"0"));
    }
    [Fact] public void LegacyGrade5UnitFractionDivisionUnchanged()
    {
        var p=new Dictionary<string,int>{{"n",3},{"d",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.whole_divide_unit_fraction",p,"6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.whole_divide_unit_fraction",p,"2"));
    }
}

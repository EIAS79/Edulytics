using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L10FractionDivisionLessonTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:04:10:STUDENT-AR-128";
    const string Pair="uae.g6.u04.l09.fraction_divide_fraction";
    const string Whole="uae.g6.u04.l09.fraction_divide_whole";
    [Fact] public void OriginalArabicLessonReusesExactFamiliesFromExploration()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void GeneralAndWholeFractionDivisionRemainExact()
    {
        var pair=new Dictionary<string,int>{{"n",2},{"d",3},{"a",1},{"b",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,pair,"4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Pair,pair,"2"));
        var proper=new Dictionary<string,int>{{"n",1},{"d",4},{"a",3},{"b",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,proper,"٢/٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Pair,proper,"4/6"));
        var whole=new Dictionary<string,int>{{"n",3},{"d",4},{"a",6},{"b",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Whole,whole,"1/8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,whole,"3/24"));
    }
    [Fact] public void InvalidSecondFactorIsRejectedWithoutAffectingGrade5()
    {
        var bad=new Dictionary<string,int>{{"n",1},{"d",4},{"a",0},{"b",8}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Pair,bad,"0"));
        var legacy=new Dictionary<string,int>{{"n",3},{"d",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u09.l09.unit_fraction_divide_whole",legacy,"1/12"));
    }
}

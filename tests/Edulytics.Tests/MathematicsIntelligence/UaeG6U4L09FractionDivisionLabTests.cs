using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U4L09FractionDivisionLabTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:04:09:STUDENT-AR-128";
    private const string Pair="uae.g6.u04.l09.fraction_divide_fraction";
    private const string Whole="uae.g6.u04.l09.fraction_divide_whole";
    [Fact] public void ArabicFractionDivisionBarLabIsAvailableWithBoundedFamilies()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.FractionBars);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Fact] public void FractionPairsCanYieldWholeProperOrImproperResults()
    {
        var groups=new Dictionary<string,int>{{"n",4},{"d",5},{"a",2},{"b",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,groups,"2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,groups,"٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Pair,groups,"4"));
        var less=new Dictionary<string,int>{{"n",1},{"d",4},{"a",1},{"b",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,less,"1/2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,less,"١/٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Pair,less,"2/4"));
        var greater=new Dictionary<string,int>{{"n",3},{"d",4},{"a",1},{"b",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Pair,greater,"6"));
    }
    [Fact] public void FractionByWholeUsesExactSimplificationAndRejectsMalformedInputs()
    {
        var p=new Dictionary<string,int>{{"n",2},{"d",3},{"a",2},{"b",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Whole,p,"1/3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,p,"2/6"));
        var wrong=new Dictionary<string,int>{{"n",2},{"d",3},{"a",0},{"b",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Whole,wrong,"0"));
        var legacy=new Dictionary<string,int>{{"n",3},{"d",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u09.l09.unit_fraction_divide_whole",legacy,"1/12"));
    }
}

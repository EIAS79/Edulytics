using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Authored Stage A–B–C, not executed; run only in Stage D.
public sealed class UaeGrade5Unit7Lesson06CompareFractionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:06";
    [Fact] public void FractionsUseLcmAndCrossProductExactness()
    {
        var p=new Dictionary<string,int>{{"n1",2},{"d1",3},{"n2",5},{"d2",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l06.compare_common_denominator",p,"٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l06.find_least_common_denominator",p,"6"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l06.equivalent_numerator",p,"4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l06.compare_common_denominator",p,"2"));
        p["n1"]=3;p["d1"]=9;p["n2"]=1;p["d2"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l06.compare_common_denominator",p,"1"));
    }
    [Fact] public void FamiliesRouteWithValidInteractions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("fractions.compare",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
}
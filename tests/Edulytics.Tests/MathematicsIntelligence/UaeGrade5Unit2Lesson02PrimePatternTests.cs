using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A-B-C: source printed pp87-92; NOT_RUN.
public sealed class UaeGrade5Unit2Lesson02PrimePatternTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:02";
    [Fact] public void FamiliesAreExactAndRegistered()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("supporting.number.factors_primes",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact] public void GrowthUsesMultiplication()
    {
        var p=new Dictionary<string,int>{{"initial",1},{"factor",2},{"rounds",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l02.fold_holes",p,"32"));
        p["factor"]=4;p["rounds"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l02.triangle_growth",p,"64"));
        p["initial"]=2;p["factor"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l02.repeated_growth",p,"54"));
    }
}

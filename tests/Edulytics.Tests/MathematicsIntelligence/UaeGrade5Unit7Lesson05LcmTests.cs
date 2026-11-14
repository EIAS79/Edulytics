using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage A–B–C authored; not executed until Stage D.
public sealed class UaeGrade5Unit7Lesson05LcmTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:05";
    [Fact] public void CorrectsSourceErrorAndVerifiesExactLcm()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l05.lcm_two_numbers",
            new Dictionary<string,int>{{"a",6},{"b",8},{"c",1}},"٢٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l05.lcm_two_numbers",
            new Dictionary<string,int>{{"a",6},{"b",8},{"c",1}},"2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            "uae.g5.u07.l05.lcm_three_numbers",
            new Dictionary<string,int>{{"a",3},{"b",4},{"c",6}},"12"));
    }
    [Fact] public void LessonContractsAndRandomizedFamiliesStayExact()
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
            for(int seed=1;seed<=8;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.DoesNotContain("result",q.Parameters.Keys);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"999999"));
            }
        }
    }
}
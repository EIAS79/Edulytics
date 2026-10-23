using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Unexecuted exact arithmetic and adversarial parameter assertions.</summary>
public sealed class UsCcssG2Unit2Lesson5DecomposeTenQuestionsTests
{
    [Fact]
    public void FourSourceAppropriateFamiliesResolveWithNumericEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L05",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedVariantsRequireRealTenExchangeAndRejectWrongParameters()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L05",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=generator.Generate("im2021-g2-u02-l05",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(q.Parameters["ones"]<q.Parameters["remove"]);
                Assert.Equal(q.Parameters["tens"]*10+q.Parameters["ones"],q.Parameters["whole"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var changed=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["whole"]=999 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,changed,q.CorrectAnswer));
            }
        }
    }
}

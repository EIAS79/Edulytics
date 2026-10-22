using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2Lesson9MixedAddSubtractQuestionsTests
{
    [Fact]
    public void FourExactFamiliesSeparateComposeAndDecomposeBehaviour()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L09",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomizedExactProblemsRejectWrongRegroupingAndAlteredParameters()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L09",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u02-l09",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var onesSum=q.Parameters["aOnes"]+q.Parameters["bOnes"];
                if(family.EndsWith("add_compose_ten",StringComparison.Ordinal))
                    Assert.True(onesSum>=10);
                else if(family.EndsWith("add_no_compose",StringComparison.Ordinal))
                    Assert.True(onesSum<10);
                else if(family.EndsWith("subtract_decompose_ten",StringComparison.Ordinal))
                    Assert.True(q.Parameters["aOnes"]<q.Parameters["bOnes"]);
                else Assert.True(q.Parameters["aOnes"]>=q.Parameters["bOnes"]);
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["a"]=1001 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,altered,q.CorrectAnswer));
            }
        }
    }
}

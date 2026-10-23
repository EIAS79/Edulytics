using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson16PlantHeightsQuestionsTests
{
    [Fact]
    public void FourSourceSkillsHaveExactIntegerInputWithoutInventingHanPlot()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L16",out var contract));
        Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var kind));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,kind!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedFictionalHeightsRecomputeAndRejectTampering()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L16",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u03-l16",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var invalid=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                { ["start"]=999 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,invalid,q.CorrectAnswer));
            }
        }
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Four numeric exact families; executable Stage D intentionally deferred.</summary>
public sealed class UsCcssG2Unit2Lesson6TargetNumbersQuestionsTests
{
    [Fact]
    public void FourSourceAlignedFamiliesUseIntegerEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L06",out var contract));
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
    public void GeneratedParametersRecomputeTrueTotalsAndRejectIllegalCards()
    {
        var gen=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U02:L06",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=gen.Generate("im2021-g2-u02-l06",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(q.Parameters.ContainsKey("card"))altered["card"]=0;
                else altered["whole"]=999;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,q.CorrectAnswer));
            }
        }
    }
}

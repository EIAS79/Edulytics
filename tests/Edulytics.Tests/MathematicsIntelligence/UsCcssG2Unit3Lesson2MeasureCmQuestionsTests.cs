using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson2MeasureCmQuestionsTests
{
    [Fact]
    public void FourCentimeterToolAndDifferenceFamiliesResolveExactIntegerInput()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L02",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var kind));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,kind!.InteractionKind);
        }
    }
    [Fact]
    public void RandomCmCountsAndLengthsRecomputeRejectTampering()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L02",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u03-l02",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.InRange(q.Parameters["lengthCm"],12,80);
                Assert.True(q.Parameters["shorterCm"]<q.Parameters["lengthCm"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["lengthCm"]=1000 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
            }
        }
    }
}

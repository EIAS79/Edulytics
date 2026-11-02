using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4Lesson01NumberLineQuestionsTests
{
    [Fact]
    public void FourExactNumberLineFamiliesHaveRegisteredIntegerKeypadInteractions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L01",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        Assert.Equal("measurement.scale.read_equal_intervals",contract.SkillId);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratorRecomputesEachAnswerAndRejectsShiftedZeroOrUnequalSteps()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L01",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=generator.Generate("source-im2021-g2-u04-l01",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                var x=question.Parameters;
                Assert.Equal(0,x["start"]);
                Assert.Equal(1,x["step"]);
                Assert.Equal(family.Contains("missing_tick")||family.Contains("greater_point")?10:20,x["end"]);
                Assert.True(x["first"]<x["second"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,x,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,x,"-1"));
                var bad=x.ToDictionary(k=>k.Key,k=>k.Value,StringComparer.Ordinal);
                bad["start"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,question.CorrectAnswer));
                bad["start"]=0;
                bad["step"]=2;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,question.CorrectAnswer));
            }
        }
    }
}

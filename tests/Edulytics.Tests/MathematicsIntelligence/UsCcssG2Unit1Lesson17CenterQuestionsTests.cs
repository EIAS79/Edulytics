using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Exact source L17 virtual families; Stage C written only, D unrun.</summary>
public sealed class UsCcssG2Unit1Lesson17CenterQuestionsTests
{
    [Fact]
    public void ReviewedSourceLessonHasExactlyFiveMachineGradeableFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L17",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(5,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedNumbersAndWrongAnswersRespectExactBounds()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L17",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-grade2-u01-l17",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var corrupted=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                corrupted[family.EndsWith("read_bar_graph_difference",StringComparison.Ordinal)
                    ?"difference":"total"]=-1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,corrupted,q.CorrectAnswer));
                if(family.Contains("count_groups",StringComparison.Ordinal))
                    Assert.InRange(q.Parameters["total"],100,120);
                if(family.EndsWith("add_within_fifty",StringComparison.Ordinal))
                    Assert.InRange(q.Parameters["total"],0,50);
            }
        }
    }
}

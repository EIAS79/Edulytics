using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Exact generated fictional survey families; Stage D deferred.</summary>
public sealed class UsCcssG2Unit1Lesson18ClassSurveyQuestionsTests
{
    [Fact]
    public void FourExactFamiliesAndNumericInteractionsBelongOnlyToL18()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L18",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void VerifiedGeneratedResultsRejectWrongAnswersAndCorruptedMetadata()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L18",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var batch=engine.Generate("im2021-g2-u01-l18",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,batch.Count);
            foreach(var q in batch)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"-1"));
                var damaged=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["categoryCount"]=9 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,damaged,q.CorrectAnswer));
                Assert.InRange(q.Parameters["categoryCount"],3,4);
            }
        }
    }
}

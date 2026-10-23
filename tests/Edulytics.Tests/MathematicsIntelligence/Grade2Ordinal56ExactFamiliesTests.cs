using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal56ExactFamiliesTests
{
    [Fact]
    public void FourSourceSeparatedFamiliesResolveAndHaveKeypad()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L04",out var c));
        Assert.NotNull(c);
        Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void SeededGeneratorRequiresExactRecomputationAndRejectsCorruptParameters()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L04",out var c));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u04-l04",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(4,q.Parameters["lesson"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var corrupted=q.Parameters.ToDictionary(p=>p.Key,p=>p.Value,StringComparer.Ordinal);
                corrupted["spin"]=3;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,q.CorrectAnswer));
            }
        }
    }
}

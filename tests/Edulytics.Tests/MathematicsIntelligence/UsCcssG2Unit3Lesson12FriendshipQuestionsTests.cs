using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson12FriendshipQuestionsTests
{
    [Fact]
    public void FourLinkedTwoStepMathFamiliesResolveToNumericEntry()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L12",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void SeededVariantsRecomputeBothStepsAndRejectChangedParameters()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L12",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=generator.Generate("im2021-g2-u03-l12",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var p=q.Parameters;
                int expected=f.EndsWith("two_step_take_from",StringComparison.Ordinal)
                    ?p["total"]-p["first"]-p["second"]
                    :p["second"]+p["total"]-p["first"];
                Assert.Equal(expected.ToString(),q.CorrectAnswer);
                var bad=new Dictionary<string,int>(p,StringComparer.Ordinal) { ["total"]=200 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
}

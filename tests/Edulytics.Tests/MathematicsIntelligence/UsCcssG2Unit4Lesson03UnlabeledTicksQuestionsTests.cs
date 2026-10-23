using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4Lesson03UnlabeledTicksQuestionsTests
{
    [Fact]
    public void FourNumericFamiliesResolveToExactStudentKeypad()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L03",out var contract));
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
    public void RecomputedSparseLabelCoordinatesRejectShiftedWindowsOrChangedAnswer()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L03",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var generated=generator.Generate("im2021-g2-u04-l03",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,generated.Count);
            foreach(var question in generated)
            {
                var p=question.Parameters;
                Assert.True(p["anchor"]>=p["windowStart"]);
                Assert.True(p["anchor"]<=p["windowEnd"]);
                Assert.InRange(p["offset"],1,p["labelStep"]-1);
                Assert.True(p["first"]<p["second"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"-1"));
                var corrupted=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                corrupted["labelStep"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,question.CorrectAnswer));
                corrupted["labelStep"]=p["labelStep"];
                corrupted["windowStart"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,question.CorrectAnswer));
            }
        }
    }
}

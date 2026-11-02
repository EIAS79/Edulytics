using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit4Lesson02NumberLineFeaturesQuestionsTests
{
    [Fact]
    public void FourFamiliesResolveToExactStudentIntegerKeypad()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L02",out var contract));
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
    public void GenerationVerifiesFivesAndEqualUnitSpacingAndRejectsParameterTampering()
    {
        var generator=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U04:L02",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=generator.Generate("g2-u04-l02-source",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                var p=q.Parameters;
                Assert.Equal(0,p["start"]);
                Assert.True(p["first"]<p["second"]);
                if(family.EndsWith("count_by_five",StringComparison.Ordinal))
                {
                    Assert.Equal(100,p["end"]);
                    Assert.Equal(5,p["step"]);
                    Assert.Equal(5,p["second"]-p["first"]);
                }
                else Assert.Equal(1,p["step"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"-1"));
                var mutated=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                mutated["start"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,mutated,q.CorrectAnswer));
                mutated["start"]=0;
                mutated["step"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,mutated,q.CorrectAnswer));
            }
        }
    }
}

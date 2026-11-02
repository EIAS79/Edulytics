using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson10TornTapeQuestionsTests
{
    [Fact]
    public void FourNonzeroRulerExactFamiliesHaveNumericPresentation()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L10",out var contract));
        Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void RandomTickProblemsStayInRangeAndDetectTamperedParameters()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L10",out var contract));
        foreach(var f in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u03-l10",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(q.Parameters["start"]>=1);
                Assert.True(q.Parameters["start"]+q.Parameters["length"]+q.Parameters["shift"]<=50);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var altered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    { ["start"]=99 };
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
            }
        }
    }
}

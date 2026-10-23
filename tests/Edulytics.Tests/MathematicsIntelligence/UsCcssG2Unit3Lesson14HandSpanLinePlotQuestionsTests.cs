using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson14HandSpanLinePlotQuestionsTests
{
    [Fact]
    public void FourNumericalPlotFamiliesHaveExactIntegerPresenter()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L14",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        }
    }
    [Fact]
    public void EverySyntheticDatasetIsValidAndRecomputedFromItsValues()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve("PED:US-CCSS-MATH:G2:U03:L14",out var c));
        foreach(var family in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var questions=engine.Generate("im2021-g2-u03-l14",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.True(q.Parameters["modeCount"]>q.Parameters["firstCount"]);
                Assert.True(q.Parameters["modeCount"]>q.Parameters["lastCount"]);
                Assert.True(q.Parameters["first"]<q.Parameters["mode"]);
                Assert.True(q.Parameters["mode"]<q.Parameters["last"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var manipulated=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    {["modeCount"]=100};
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,manipulated,q.CorrectAnswer));
            }
        }
    }
}

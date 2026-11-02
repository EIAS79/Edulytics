using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson15PencilLinePlotQuestionsTests
{
    [Fact]
    public void FourLinePlotFamiliesAreExactIntegerWithEmptyTick()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L15",out var contract));
        Assert.NotNull(contract);Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void RandomPencilPlotsAreConsecutiveAndAdversariallyChecked()
    {
        var e=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L15",out var contract));
        foreach(var family in contract!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=e.Generate("im2021-g2-u03-l15",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.Equal(q.Parameters["first"]+1,q.Parameters["mode"]);
                Assert.Equal(q.Parameters["first"]+2,q.Parameters["blank"]);
                Assert.Equal(q.Parameters["first"]+3,q.Parameters["last"]);
                Assert.True(q.Parameters["modeCount"]>q.Parameters["firstCount"]);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                var modified=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    {["blank"]=1000};
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,modified,q.CorrectAnswer));
            }
        }
    }
}

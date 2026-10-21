using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-position numeric questions authored but never run before Stage D.</summary>
public sealed class UsCcssG2Unit1Lesson15CompareQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l15.compare_unknown_smaller",
        "usccss.g2.u01.l15.compare_unknown_larger",
        "usccss.g2.u01.l15.compare_unknown_difference"
    ];
    [Fact]
    public void OriginalG2Lesson15RoutesAllUnknownPositionsAndIntegerInteraction()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L15",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void EveryMissingPartIsScoredFromValidWithinHundredSourceTape()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var items=engine.Generate("im2021-g2-u01-l15",family,[family],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,items.Count);
            foreach(var question in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,question.Parameters,question.CorrectAnswer));
                var a=question.Parameters["small"];
                var b=question.Parameters["big"];
                var difference=question.Parameters["diff"];
                Assert.True(a+difference==b);
                Assert.InRange(b,2,100);
                var tampered=new Dictionary<string,int>(question.Parameters,StringComparer.Ordinal)
                    {["big"]=101};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,question.CorrectAnswer));
            }
        }
    }
}

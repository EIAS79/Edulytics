using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Tests authored during C; intentionally NOT run until Stage D.</summary>
public sealed class UsCcssG2Unit1Lesson13BarCompareQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l13.bar_unknown_difference",
        "usccss.g2.u01.l13.related_addition_missing_difference",
        "usccss.g2.u01.l13.related_subtraction_difference"
    ];
    [Fact]
    public void ExactSourceLessonRoutesAllThreeCompareFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L13",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var f in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void GenerateAndCheckUnknownDifferencesWithinTwenty()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var items=engine.Generate("im2021-g2-u01-l13",family,[family],
                ExactSkillQuestionDifficulty.Standard,6,seed,[]);
            Assert.Equal(6,items.Count);
            foreach(var item in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                Assert.True(item.Parameters["higher"]>item.Parameters["lower"]);
                Assert.InRange(item.Parameters["higher"],2,20);
                var tampered=new Dictionary<string,int>(item.Parameters,StringComparer.Ordinal)
                    {["higher"]=21};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,"invalid"));
            }
        }
    }
}

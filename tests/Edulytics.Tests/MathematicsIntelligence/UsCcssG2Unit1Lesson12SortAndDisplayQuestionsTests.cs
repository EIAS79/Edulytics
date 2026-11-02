using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-bounded optional center tests authored for Stage D only.</summary>
public sealed class UsCcssG2Unit1Lesson12SortAndDisplayQuestionsTests
{
    private static readonly string[] Families=[
        "usccss.g2.u01.l12.sort_three_category_count",
        "usccss.g2.u01.l12.sort_three_category_combine"
    ];
    [Fact]
    public void OnlyOriginalThreeCategoryPracticeSubskillsAreAttachedToLesson()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L12",out var contract));
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
    public void HypotheticalCollectionStays20To30WithThreeNonemptyCategories()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var items=engine.Generate("im2021-g2-u01-l12",family,[family],
                ExactSkillQuestionDifficulty.Standard,6,seed,[]);
            Assert.Equal(6,items.Count);
            foreach(var q in items)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                var a=q.Parameters["a"];var b=q.Parameters["b"];var c=q.Parameters["c"];
                Assert.InRange(a,1,30);Assert.InRange(b,1,30);Assert.InRange(c,1,30);
                Assert.InRange(a+b+c,20,30);
                var tampered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal)
                    {["c"]=0};
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,q.CorrectAnswer));
            }
        }
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authoring assertions — do not run until Stage D.</summary>
public sealed class UsCcssG2Unit1Lesson10ConstructGraphsQuestionsTests
{
    private static readonly string[] Families=
    [
        "usccss.g2.u01.l10.picture_symbol_count",
        "usccss.g2.u01.l10.bar_height_for_category",
        "usccss.g2.u01.l10.picture_bar_same_data"
    ];

    [Fact]
    public void OneLessonContainsExactlyThreeSourceSubskillQuestionFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L10",out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families,contract!.AllowedQuestionFamilies);
        foreach(var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var presentation));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,presentation!.InteractionKind);
        }
    }

    [Fact]
    public void NoTamperedCountCanPassAndDifferentGraphCategoriesAreCompared()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261022,20261023})
        {
            var rows=engine.Generate("im2021-g2-u01-l10",family,[family],
                ExactSkillQuestionDifficulty.Standard,7,seed,[]);
            Assert.Equal(7,rows.Count);
            foreach(var q in rows)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,q.Parameters,"not a number"));
                var tampered=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                tampered["count0"]=99;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,tampered,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void TwoGraphsOnlyMatchIfAllFourCategoriesMatchExactly()
    {
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        for(var i=0;i<4;i++)
        {
            p[$"count{i}"]=i+1;
            p[$"bar{i}"]=i+1;
        }
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[2],p,"1"));
        p["bar2"]=8;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Families[2],p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Families[2],p,"1"));
    }
}

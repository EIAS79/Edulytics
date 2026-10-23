using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source-specific tests written but never executed before D.</summary>
public sealed class UsCcssG2Unit1Lesson9BarGraphQuestionsTests
{
    private static readonly string[] Families=
    [
        "usccss.g2.u01.l09.bar_category_height",
        "usccss.g2.u01.l09.bar_combine_or_compare",
        "usccss.g2.u01.l09.bar_total"
    ];

    [Fact]
    public void OrdinalNineRoutesOnlyExactBarGraphSourceSkills()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U01:L09",out var contract));
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
    public void BarHeightsAndOneUnitScaleAreExactAndTamperingIsRejected()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in Families)
        foreach(var seed in new[]{20261021,20261022})
        {
            var questions=engine.Generate("im2021-g2-u01-l09",family,[family],
                ExactSkillQuestionDifficulty.Standard,7,seed,[]);
            Assert.Equal(7,questions.Count);
            Assert.Equal(7,questions.Select(x=>x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());
            foreach(var item in questions)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                var value=int.Parse(item.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,(value+100).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,"invalid"));
                var bad=new Dictionary<string,int>(item.Parameters,StringComparer.Ordinal);
                bad["bar0"]=15;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,bad,item.CorrectAnswer));
                foreach(var i in Enumerable.Range(0,4))
                    Assert.InRange(item.Parameters[$"bar{i}"],0,14);
            }
        }
    }
}

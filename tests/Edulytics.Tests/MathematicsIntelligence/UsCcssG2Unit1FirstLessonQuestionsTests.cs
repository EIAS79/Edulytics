using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Authored Stage C tests. Execution and CI evidence are deliberately deferred
/// to Stage D; these tests do not certify teacher-observed source activities.
/// </summary>
public sealed class UsCcssG2Unit1FirstLessonQuestionsTests
{
    private const string LessonCode = "PED:US-CCSS-MATH:G2:U01:L01";
    private static readonly string[] Families =
    [
        "usccss.g2.u01.l01.two_card_operation",
        "usccss.g2.u01.l01.match_expression_value",
        "usccss.g2.u01.l01.make_ten_missing_addend",
        "usccss.g2.u01.l01.recognize_ten_sum"
    ];

    [Fact]
    public void LessonHasSourceSpecificDigitalFamiliesAndInteractions()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(LessonCode, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Families, contract!.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var ui));
            Assert.NotNull(ui);
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, ui!.InteractionKind);
        }
    }

    [Fact]
    public void SourceDemonstrationTenAndSevenRequiresSubtraction()
    {
        var example = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["left"] = 10, ["right"] = 7, ["operation"] = 1
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Families[0], example, "2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Families[0], example, "1"));
        example["operation"] = 0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Families[0], example, "1")); // 10 + 7 is outside the within-ten task.
    }

    [Fact]
    public void EachFamilyHasSourceBoundsCorrectWrongInvalidAndTamperingCases()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in Families)
        foreach (var seed in new[] {20261021, 20261022})
        {
            var items = engine.Generate("im2021-g2-u01-l01", family, [family],
                ExactSkillQuestionDifficulty.Standard, 8, seed, []);
            Assert.Equal(8, items.Count);
            Assert.Equal(8, items.Select(x => x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());

            foreach (var item in items)
            {
                Assert.Equal(family, item.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, item.CorrectAnswer));

                var expected = int.Parse(item.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, (expected + 3).ToString(
                        System.Globalization.CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, item.Parameters, "not-a-number"));

                var changed = new Dictionary<string, int>(item.Parameters,
                    StringComparer.Ordinal);
                changed[family == Families[2] ? "part" : "left"] = -20;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, changed, item.CorrectAnswer));

                if (family == Families[0])
                {
                    Assert.InRange(item.Parameters["left"] - item.Parameters["right"], 0, 10);
                    if (expected == 1)
                        Assert.InRange(item.Parameters["left"] + item.Parameters["right"], 0, 10);
                    Assert.InRange(item.Parameters["left"], 2, 10);
                    Assert.InRange(expected, 1, 2);
                    Assert.Contains("Which operation", item.Prompt);
                }
                else if (family == Families[1])
                {
                    Assert.InRange(expected, 0, 10);
                    Assert.Contains("Match the expression", item.Prompt);
                }
                else if (family == Families[2])
                {
                    Assert.Equal(10, expected + item.Parameters["part"]);
                    Assert.Contains("= 10", item.Prompt);
                }
                else
                {
                    Assert.Equal(item.Parameters["left"] + item.Parameters["right"] == 10
                        ? 1 : 0, expected);
                    Assert.InRange(expected, 0, 1);
                }
            }
        }
    }
}

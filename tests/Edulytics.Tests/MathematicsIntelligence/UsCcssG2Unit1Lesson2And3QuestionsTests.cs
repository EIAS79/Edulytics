using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authoring checks ONLY. Do not execute until Stage D.</summary>
public sealed class UsCcssG2Unit1Lesson2And3QuestionsTests
{
    private static readonly string[] L02 =
    [
        "usccss.g2.u01.l02.hidden_ten_cubes",
        "usccss.g2.u01.l02.related_ten_equation",
        "usccss.g2.u01.l02.match_ten_cube_tower"
    ];
    private static readonly string[] L03 =
    [
        "usccss.g2.u01.l03.hidden_twenty_cubes",
        "usccss.g2.u01.l03.missing_in_twenty_equation",
        "usccss.g2.u01.l03.extension_unknown_minuend"
    ];

    [Fact]
    public void ExactTwoLessonsHaveOnlyTheirSourceTaskFamilies()
    {
        foreach (var (code, expected) in new[]
        {
            ("PED:US-CCSS-MATH:G2:U01:L02", L02),
            ("PED:US-CCSS-MATH:G2:U01:L03", L03)
        })
        {
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code, out var contract));
            Assert.NotNull(contract);
            Assert.Equal(expected, contract!.AllowedQuestionFamilies);
            foreach (var family in expected)
            {
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
                Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
                Assert.NotNull(interaction);
                Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, interaction!.InteractionKind);
            }
        }
    }

    [Fact]
    public void OriginalTenCubeTowersMapToExactLetters()
    {
        for (var blue = 1; blue <= 5; blue++)
        {
            foreach (var form in new[] {0, 1})
            {
                var task = new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["blue"] = blue, ["form"] = form
                };
                var expected = (6 - blue).ToString(System.Globalization.CultureInfo.InvariantCulture);
                Assert.True(ExactSkillContractQuestionEngine.Verify(L02[2], task, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(L02[2], task,
                    (7 - blue).ToString(System.Globalization.CultureInfo.InvariantCulture)));
            }
        }
    }

    [Fact]
    public void OriginalTwentyCubeUnknownAndBeyondTwentyChallengeAreDistinct()
    {
        var unknown = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["part"] = 18, ["form"] = 0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(L03[1], unknown, "2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(L03[1], unknown, "18"));
        var challenge = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["subtract"] = 5
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(L03[2], challenge, "25"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(L03[2], challenge, "15"));
    }

    [Fact]
    public void SeededGenerationRecomputesCorrectAnswersAndRejectsTampering()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var family in L02.Concat(L03))
        foreach (var seed in new[] {20261021, 20261022})
        {
            var questions = engine.Generate("g2-first-unit-source-v1", family,
                [family], ExactSkillQuestionDifficulty.Standard, 6, seed, []);
            Assert.Equal(6, questions.Count);
            Assert.Equal(6, questions.Select(x => x.ExposureFingerprint)
                .Distinct(StringComparer.Ordinal).Count());

            foreach (var question in questions)
            {
                Assert.Equal(family, question.Family);
                Assert.NotEmpty(question.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, question.CorrectAnswer));
                var answer = int.Parse(question.CorrectAnswer,
                    System.Globalization.CultureInfo.InvariantCulture);
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, (answer + 50).ToString(
                        System.Globalization.CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, "bad"));

                var changed = new Dictionary<string, int>(
                    question.Parameters, StringComparer.Ordinal);
                var param = family switch
                {
                    "usccss.g2.u01.l02.hidden_ten_cubes" => "visible",
                    "usccss.g2.u01.l02.related_ten_equation" => "part",
                    "usccss.g2.u01.l02.match_ten_cube_tower" => "blue",
                    "usccss.g2.u01.l03.hidden_twenty_cubes" => "visible",
                    "usccss.g2.u01.l03.missing_in_twenty_equation" => "part",
                    _ => "subtract"
                };
                changed[param] = -30;
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, changed, question.CorrectAnswer));
            }
        }
    }
}

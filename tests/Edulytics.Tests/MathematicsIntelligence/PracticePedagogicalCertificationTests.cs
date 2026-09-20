using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticePedagogicalCertificationTests
{
    private static readonly ExactSkillQuestionDifficulty[] Difficulties =
    [
        ExactSkillQuestionDifficulty.Standard,
        ExactSkillQuestionDifficulty.Stretch,
        ExactSkillQuestionDifficulty.Challenge
    ];

    private static readonly string[] ForbiddenPlaceholders =
    [
        "TODO",
        "TBD",
        "lorem",
        "placeholder",
        "undefined"
    ];

    [Fact]
    public void EveryRuntimePracticeFamilyGeneratesClearVerifiedSamplesAcrossAllDifficulties()
    {
        var families = LessonPracticeContractRegistry.All
            .SelectMany(contract => contract.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(family => family, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(families);

        var engine = new ExactSkillContractQuestionEngine();
        var failures = new List<string>();
        var seed = 20260920;

        foreach (var family in families)
        {
            if (!ExactSkillContractQuestionEngine.SupportsFamily(family))
            {
                failures.Add($"{family}: exact engine does not support the routed family");
                continue;
            }

            foreach (var difficulty in Difficulties)
            {
                try
                {
                    var question = Assert.Single(engine.Generate(
                        "pedagogical-certification",
                        family,
                        [family],
                        difficulty,
                        1,
                        seed++,
                        []));

                    if (!string.Equals(question.Family, family, StringComparison.Ordinal))
                        failures.Add($"{family}/{difficulty}: generated a different family {question.Family}");

                    if (string.IsNullOrWhiteSpace(question.Prompt))
                        failures.Add($"{family}/{difficulty}: blank prompt");

                    if (string.IsNullOrWhiteSpace(question.Solution))
                        failures.Add($"{family}/{difficulty}: blank solution");

                    if (string.IsNullOrWhiteSpace(question.CorrectAnswer))
                        failures.Add($"{family}/{difficulty}: blank correct answer");

                    if (string.IsNullOrWhiteSpace(question.ExposureFingerprint))
                        failures.Add($"{family}/{difficulty}: blank exposure fingerprint");

                    if (question.Parameters.Count == 0)
                        failures.Add($"{family}/{difficulty}: no reconstructable parameters");

                    if (!ExactSkillContractQuestionEngine.Verify(
                            question.Family,
                            question.Parameters,
                            question.CorrectAnswer))
                    {
                        failures.Add($"{family}/{difficulty}: independent verifier rejected generated answer");
                    }

                    foreach (var token in ForbiddenPlaceholders)
                    {
                        if (question.Prompt.Contains(token, StringComparison.OrdinalIgnoreCase) ||
                            question.Solution.Contains(token, StringComparison.OrdinalIgnoreCase))
                        {
                            failures.Add($"{family}/{difficulty}: contains placeholder token {token}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    failures.Add($"{family}/{difficulty}: {ex.GetType().Name}: {ex.Message}");
                }
            }
        }

        Assert.True(
            failures.Count == 0,
            "Practice pedagogical certification failures: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100 ? $" (+{failures.Count - 100} more)" : string.Empty));
    }

    [Theory]
    [InlineData(ExactSkillQuestionDifficulty.Standard, 51001)]
    [InlineData(ExactSkillQuestionDifficulty.Stretch, 51002)]
    [InlineData(ExactSkillQuestionDifficulty.Challenge, 51003)]
    public void ReadingScalesGoldenSamplesTeachEqualIntervalReasoning(
        ExactSkillQuestionDifficulty difficulty,
        int seed)
    {
        const string family = "measurement.scale.equal_intervals.read_value";
        var question = Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            "golden-scale-reading",
            family,
            [family],
            difficulty,
            1,
            seed,
            []));

        Assert.Equal(family, question.Family);
        Assert.Contains("scale", question.Prompt, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("equal interval", question.Prompt, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("interval", question.Solution, StringComparison.OrdinalIgnoreCase);
        Assert.True(question.Parameters["intervals"] is 2 or 4 or 5 or 10);
        Assert.True(question.Parameters["end"] > question.Parameters["start"]);
        Assert.InRange(
            question.Parameters["pointer"],
            1,
            question.Parameters["intervals"] - 1);
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            question.Family,
            question.Parameters,
            question.CorrectAnswer));
    }

    [Theory]
    [InlineData(ExactSkillQuestionDifficulty.Standard, 52001)]
    [InlineData(ExactSkillQuestionDifficulty.Stretch, 52002)]
    [InlineData(ExactSkillQuestionDifficulty.Challenge, 52003)]
    public void CommonDenominatorGoldenSamplesTestUnlikeFractionComparison(
        ExactSkillQuestionDifficulty difficulty,
        int seed)
    {
        const string family = "fractions.compare.unlike.common_denominator";
        var question = Assert.Single(new ExactSkillContractQuestionEngine().Generate(
            "golden-fraction-compare",
            family,
            [family],
            difficulty,
            1,
            seed,
            []));

        Assert.Equal(family, question.Family);
        Assert.Contains("Compare", question.Prompt, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("common denominator", question.Solution, StringComparison.OrdinalIgnoreCase);
        Assert.NotEqual(question.Parameters["d1"], question.Parameters["d2"]);
        Assert.Contains(question.CorrectAnswer, new[] { "<", ">", "=" });
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            question.Family,
            question.Parameters,
            question.CorrectAnswer));
    }
}

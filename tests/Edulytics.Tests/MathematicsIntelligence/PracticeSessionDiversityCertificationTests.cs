using System.Text.RegularExpressions;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeSessionDiversityCertificationTests
{
    [Fact]
    public void EveryLessonPracticeContractGeneratesVerifiedDiverseSession()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var failures = new List<string>();
        var contracts = LessonPracticeContractRegistry.All
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(4453, contracts.Length);

        for (var index = 0; index < contracts.Length; index++)
        {
            var contract = contracts[index];
            var allowed = contract.AllowedQuestionFamilies
                .Distinct(StringComparer.Ordinal)
                .ToArray();

            if (allowed.Length == 0)
            {
                failures.Add($"{contract.LessonCode}: no allowed question families");
                continue;
            }

            var questionCount = Math.Min(
                12,
                Math.Max(6, allowed.Length));

            try
            {
                var questions = engine.Generate(
                    "lesson-session-certification",
                    contract.LessonCode,
                    allowed,
                    ExactSkillQuestionDifficulty.Standard,
                    questionCount,
                    730000 + index,
                    []);

                if (questions.Count != questionCount)
                    failures.Add($"{contract.LessonCode}: generated {questions.Count}/{questionCount}");

                if (questions
                    .Select(x => x.ExposureFingerprint)
                    .Distinct(StringComparer.Ordinal)
                    .Count() != questions.Count)
                {
                    failures.Add($"{contract.LessonCode}: duplicate exposure fingerprint");
                }

                var observedFamilies = questions
                    .Select(x => x.Family)
                    .Distinct(StringComparer.Ordinal)
                    .ToHashSet(StringComparer.Ordinal);

                foreach (var expectedFamily in allowed.Take(questionCount))
                {
                    if (!observedFamilies.Contains(expectedFamily))
                        failures.Add($"{contract.LessonCode}: family not represented in session: {expectedFamily}");
                }

                foreach (var question in questions)
                {
                    if (!allowed.Contains(question.Family, StringComparer.Ordinal))
                        failures.Add($"{contract.LessonCode}: generated disallowed family {question.Family}");

                    if (string.IsNullOrWhiteSpace(question.Prompt))
                        failures.Add($"{contract.LessonCode}/{question.Family}: blank prompt");

                    if (string.IsNullOrWhiteSpace(question.Solution))
                        failures.Add($"{contract.LessonCode}/{question.Family}: blank solution");

                    if (!ExactSkillContractQuestionEngine.Verify(
                            question.Family,
                            question.Parameters,
                            question.CorrectAnswer))
                    {
                        failures.Add($"{contract.LessonCode}/{question.Family}: verifier rejected answer");
                    }
                }
            }
            catch (Exception ex)
            {
                failures.Add(
                    $"{contract.LessonCode}: {ex.GetType().Name}: {ex.Message}");
            }
        }

        Assert.True(
            failures.Count == 0,
            "Lesson Practice session certification failures: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100
                ? $" (+{failures.Count - 100} more)"
                : string.Empty));
    }

    [Fact]
    public void QuantifiedRelationshipsMultistepFamilyHasMultipleReasoningForms()
    {
        const string family = "supporting.reasoning.multistep";

        var questions = new ExactSkillContractQuestionEngine().Generate(
            "multistep-diversity-certification",
            "PED:CAMBRIDGE-INTL-MATH:S6:6AS-MD-1:BUILD",
            [family],
            ExactSkillQuestionDifficulty.Standard,
            16,
            20260920,
            []);

        Assert.Equal(16, questions.Count);
        Assert.All(
            questions,
            question =>
                Assert.True(
                    ExactSkillContractQuestionEngine.Verify(
                        question.Family,
                        question.Parameters,
                        question.CorrectAnswer)));

        var modes = questions
            .Select(question => question.Parameters["mode"])
            .Distinct()
            .OrderBy(x => x)
            .ToArray();

        Assert.True(
            modes.Length >= 3,
            $"Expected at least 3 multistep reasoning modes, got {string.Join(",", modes)}.");

        var promptShapes = questions
            .Select(question => NormalizePromptShape(question.Prompt))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        Assert.True(
            promptShapes.Length >= 3,
            $"Expected at least 3 prompt structures, got {promptShapes.Length}.");
    }

    private static string NormalizePromptShape(string prompt) =>
        Regex.Replace(
            Regex.Replace(
                prompt.ToLowerInvariant(),
                @"-?\d+(?:\.\d+)?",
                "#",
                RegexOptions.CultureInvariant),
            @"\s+",
            " ",
            RegexOptions.CultureInvariant)
        .Trim();
}

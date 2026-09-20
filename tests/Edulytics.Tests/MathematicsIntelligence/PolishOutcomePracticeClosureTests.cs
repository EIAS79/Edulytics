using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PolishOutcomePracticeClosureTests
{
    [Fact]
    public void Polish_map_resolves_all_306_official_outcomes()
    {
        Assert.Equal(306, PolishOutcomePracticeMapRegistry.All.Count);
        Assert.Equal(
            Enumerable.Range(1, 306),
            PolishOutcomePracticeMapRegistry.All.Values
                .Select(x => x.Serial)
                .OrderBy(x => x));
        Assert.All(
            PolishOutcomePracticeMapRegistry.All.Values,
            row =>
            {
                Assert.NotEmpty(row.TargetRules);
                Assert.NotEmpty(row.SourceLocator);
                Assert.NotEmpty(row.SourceContentHash);
            });
    }

    [Fact]
    public void All_1569_Polish_learner_lessons_resolve_exact_Practice_contracts()
    {
        var polishLessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .ToArray();

        Assert.Equal(1569, polishLessons.Length);
        Assert.All(
            polishLessons,
            lesson =>
            {
                Assert.Single(lesson.OutcomeCodes);
                Assert.True(
                    PolishOutcomePracticeMapRegistry.TryResolve(
                        lesson.OutcomeCodes[0],
                        out var mapping),
                    $"Missing Polish exact outcome mapping: {lesson.OutcomeCodes[0]}");
                Assert.NotNull(mapping);

                Assert.True(
                    LessonPracticeContractRegistry.TryResolve(
                        lesson.LessonCode,
                        out var contract),
                    $"Missing Polish Practice contract: {lesson.LessonCode}");
                Assert.NotNull(contract);
                Assert.Equal("READY_VERIFIED", contract!.Readiness);
                Assert.Equal(
                    "PolishOfficialOutcomeMap",
                    contract.SourceType);
                Assert.NotEmpty(contract.SkillIds);
                Assert.NotEmpty(contract.AllowedQuestionFamilies);
                Assert.All(
                    contract.AllowedQuestionFamilies,
                    family => Assert.True(
                        ExactSkillContractQuestionEngine.SupportsFamily(family),
                        $"Unsupported Polish family {family} for {lesson.LessonCode}"));
            });
    }


    [Fact]
    public void Polish_lesson_bodies_are_rebuilt_from_exact_outcome_evidence()
    {
        var polishLessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .ToArray();

        Assert.Equal(1569, polishLessons.Length);

        Assert.All(
            polishLessons,
            lesson =>
            {
                var outcomeCode = Assert.Single(lesson.OutcomeCodes);
                Assert.True(
                    PolishOutcomeSourceEvidenceRegistry.TryResolve(
                        outcomeCode,
                        out var source),
                    $"Missing pinned Polish official evidence: {outcomeCode}");
                Assert.NotNull(source);

                var polish = Assert.Single(
                    lesson.Translations.Where(x =>
                        x.CultureCode.StartsWith(
                            "pl",
                            StringComparison.OrdinalIgnoreCase)));

                Assert.False(
                    polish.Title.Contains(
                        "ćwiczenie",
                        StringComparison.OrdinalIgnoreCase),
                    $"Fallback title survived Polish remediation: {lesson.LessonCode}");
                Assert.True(
                    polish.Explanation.Contains(
                        outcomeCode,
                        StringComparison.Ordinal),
                    $"OutcomeCode is not traceable in learner explanation: {lesson.LessonCode}");

                var prefixLength = Math.Min(48, source!.OfficialText.Length);
                var officialPrefix = source.OfficialText[..prefixLength];
                Assert.True(
                    polish.Explanation.Contains(
                        officialPrefix,
                        StringComparison.Ordinal),
                    $"Official target evidence is absent from remediated lesson body: {lesson.LessonCode}");

                Assert.False(string.IsNullOrWhiteSpace(polish.KeyConceptsAndRules));
                Assert.False(string.IsNullOrWhiteSpace(polish.WorkedExamples));
                Assert.False(string.IsNullOrWhiteSpace(polish.StepByStepSolutions));
                Assert.False(string.IsNullOrWhiteSpace(polish.CommonMistakes));
                Assert.False(string.IsNullOrWhiteSpace(polish.QuickSummary));

                Assert.True(
                    (lesson.AdaptationStatus ?? string.Empty).Contains(
                        "outcome-specific learner content",
                        StringComparison.OrdinalIgnoreCase),
                    $"Polish remediation provenance missing: {lesson.LessonCode}");
            });
    }


    [Fact]
    public void All_1569_Polish_Practice_contracts_generate_and_verify_deterministically()
    {
        var polishLessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(x => x.PackCode == MathematicsCurriculumPackRegistry.PolandCode)
            .SelectMany(x => x.Lessons)
            .OrderBy(x => x.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(1569, polishLessons.Length);

        var engine = new ExactSkillContractQuestionEngine();
        for (var index = 0; index < polishLessons.Length; index++)
        {
            var lesson = polishLessons[index];
            Assert.True(
                LessonPracticeContractRegistry.TryResolve(
                    lesson.LessonCode,
                    out var contract),
                $"Missing Polish runtime Practice contract: {lesson.LessonCode}");
            Assert.NotNull(contract);

            var seed = 20260920 + index;
            var first = Assert.Single(
                engine.Generate(
                    "polish-final-closure",
                    lesson.LessonCode,
                    contract!.AllowedQuestionFamilies,
                    ExactSkillQuestionDifficulty.Standard,
                    1,
                    seed,
                    Array.Empty<string>()));
            var second = Assert.Single(
                engine.Generate(
                    "polish-final-closure",
                    lesson.LessonCode,
                    contract.AllowedQuestionFamilies,
                    ExactSkillQuestionDifficulty.Standard,
                    1,
                    seed,
                    Array.Empty<string>()));

            Assert.Equal(first.Family, second.Family);
            Assert.Equal(first.Prompt, second.Prompt);
            Assert.Equal(first.CorrectAnswer, second.CorrectAnswer);
            Assert.Equal(first.Solution, second.Solution);
            Assert.Equal(first.ExposureFingerprint, second.ExposureFingerprint);
            Assert.Equal(
                first.Parameters.OrderBy(x => x.Key, StringComparer.Ordinal),
                second.Parameters.OrderBy(x => x.Key, StringComparer.Ordinal));

            Assert.True(
                ExactSkillContractQuestionEngine.Verify(
                    first.Family,
                    first.Parameters,
                    first.CorrectAnswer),
                $"Verifier rejected Polish generated Practice item: {lesson.LessonCode} / {first.Family}");
        }
    }

    [Fact]
    public void Polish_mapping_is_deterministic_and_title_independent()
    {
        var first = PolishOutcomePracticeMapRegistry.All.Values
            .OrderBy(x => x.Serial)
            .First();
        Assert.True(
            PolishOutcomePracticeMapRegistry.TryResolve(
                first.OutcomeCode,
                out var again));
        Assert.NotNull(again);
        Assert.Equal(first.Serial, again!.Serial);
        Assert.Equal(first.TargetRuleIds, again.TargetRuleIds);
    }
}

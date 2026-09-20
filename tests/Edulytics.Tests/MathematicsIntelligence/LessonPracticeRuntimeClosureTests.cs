using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class LessonPracticeRuntimeClosureTests
{
    [Fact]
    public void EveryCanonicalLearnerLessonResolvesVerifiedRuntimePracticeCapability()
    {
        var lessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons)
            .GroupBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(lessons);

        var unresolved = lessons
            .Where(lesson =>
                !LessonPracticeCapabilityResolver.TryResolve(
                    lesson.LessonCode,
                    out _))
            .Select(lesson => lesson.LessonCode)
            .ToArray();

        Assert.True(
            unresolved.Length == 0,
            "Canonical learner lessons without verified runtime Practice capability: " +
            string.Join(", ", unresolved.Take(100)) +
            (unresolved.Length > 100
                ? $" (+{unresolved.Length - 100} more)"
                : string.Empty));
    }

    [Fact]
    public void EveryCanonicalLearnerLessonGeneratesAndIndependentlyVerifiesExactPractice()
    {
        var engine = new ExactSkillContractQuestionEngine();
        var lessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons)
            .GroupBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .Select(group => group.First())
            .OrderBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .ToArray();

        var failures = new List<string>();
        var seed = 910000;

        foreach (var lesson in lessons)
        {
            if (!LessonPracticeCapabilityResolver.TryResolve(
                    lesson.LessonCode,
                    out var contract) ||
                contract is null)
            {
                failures.Add($"{lesson.LessonCode}: capability missing");
                continue;
            }

            try
            {
                var question = Assert.Single(
                    engine.Generate(
                        "catalogue-runtime-closure",
                        lesson.LessonCode,
                        contract.AllowedQuestionFamilies,
                        ExactSkillQuestionDifficulty.Standard,
                        1,
                        seed++,
                        []));

                if (!contract.AllowedQuestionFamilies.Contains(
                        question.Family,
                        StringComparer.Ordinal))
                {
                    failures.Add(
                        $"{lesson.LessonCode}: generated family {question.Family} is not allowed");
                    continue;
                }

                if (!ExactSkillContractQuestionEngine.Verify(
                        question.Family,
                        question.Parameters,
                        question.CorrectAnswer))
                {
                    failures.Add(
                        $"{lesson.LessonCode}: independent verification failed for {question.Family}");
                }
            }
            catch (Exception ex)
            {
                failures.Add(
                    $"{lesson.LessonCode}: {ex.GetType().Name}: {ex.Message}");
            }
        }

        Assert.True(
            failures.Count == 0,
            "Canonical lesson Practice runtime failures: " +
            string.Join(" | ", failures.Take(50)) +
            (failures.Count > 50
                ? $" (+{failures.Count - 50} more)"
                : string.Empty));
    }
}

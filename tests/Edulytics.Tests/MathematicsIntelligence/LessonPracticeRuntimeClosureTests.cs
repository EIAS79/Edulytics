using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class LessonPracticeRuntimeClosureTests
{
    [Fact]
    public void PendingG10DoesNotRestoreAnyRetiredOrUnverifiedPracticeContract()
    {
        var g10 = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(x => x.Lessons)
            .Where(x => x.LessonCode.StartsWith("PED:UAE-MOE-MATH:L10:", StringComparison.Ordinal)).ToArray();
        Assert.Equal(148, g10.Length);
        Assert.All(g10, x =>
        {
            Assert.StartsWith("PED:UAE-MOE-MATH:L10:GENERAL:", x.LessonCode, StringComparison.Ordinal);
            Assert.False(LessonPracticeCapabilityResolver.TryResolve(x.LessonCode, out _));
        });
        Assert.DoesNotContain(LessonPracticeContractRegistry.All,
            x => x.LessonCode.StartsWith("PED:UAE-MOE-MATH:L10:", StringComparison.Ordinal));
    }

    [Fact]
    public void EveryCanonicalLearnerLessonResolvesVerifiedRuntimePracticeCapability()
    {
        var lessons = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons)
            // G9 first-party 82 lessons are a pending scoped production release:
            // 80 digital contracts require a separate owner token and two
            // teacher-review lessons MUST remain non-auto-graded.
            // Their exact 80+2 evidence is covered by G9 Stage D and E2E.
            .Where(lesson => !lesson.LessonCode.StartsWith(
                "PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison.Ordinal))
            // G10 is not a released digital cohort: exact 97 candidates
            // have their own D.2 and authenticated D.3 release gates.
            .Where(lesson => !lesson.LessonCode.StartsWith(
                "PED:UAE-MOE-MATH:L10:", StringComparison.Ordinal))
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
            // G9 first-party 82 lessons are a pending scoped production release:
            // 80 digital contracts require a separate owner token and two
            // teacher-review lessons MUST remain non-auto-graded.
            // Their exact 80+2 evidence is covered by G9 Stage D and E2E.
            .Where(lesson => !lesson.LessonCode.StartsWith(
                "PED:UAE-MOE-MATH:L9:GENERAL:", StringComparison.Ordinal))
            // G10 is not a released digital cohort: exact 97 candidates
            // have their own D.2 and authenticated D.3 release gates.
            .Where(lesson => !lesson.LessonCode.StartsWith(
                "PED:UAE-MOE-MATH:L10:", StringComparison.Ordinal))
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

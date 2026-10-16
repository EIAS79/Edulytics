using System.Text.Json;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent generated-item engine sampling for every actual B001 lesson.
/// This is NOT an authenticated database-persistence or pedagogical-certification
/// test; report status is named to avoid accidental academic approval.
/// </summary>
public sealed class UsCcssB001FiveHundredPracticeEngineTests
{
    private static string FindRepositoryRoot()
    {
        var here = new DirectoryInfo(AppContext.BaseDirectory);
        for (var i = 0; here is not null && i < 12; i++, here = here.Parent)
            if (File.Exists(Path.Combine(here.FullName, "Edulytics.sln")))
                return here.FullName;
        throw new InvalidOperationException("Cannot find checked-out Edulytics solution root");
    }

    [Fact]
    public void EveryB001LessonGeneratesAnswersAndRejectsMalformedResponses()
    {
        var root = FindRepositoryRoot();
        var manifestPath = Path.Combine(root, "docs", "curriculum", "batches",
            "US-CCSS-B001-CATALOGUE.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(manifestPath));
        var lessons = doc.RootElement.GetProperty("lessons")
            .EnumerateArray()
            .Select(row => row.GetProperty("lessonCode").GetString()
                ?? throw new InvalidOperationException("Empty lesson code"))
            .ToArray();
        Assert.Equal(500, lessons.Length);
        Assert.Equal(500, lessons.Distinct(StringComparer.Ordinal).Count());

        var engine = new ExactSkillContractQuestionEngine();
        var rows = new List<object>();
        var errors = new List<string>();
        var generatedCount = 0;
        var passedLessons = 0;
        foreach (var (code, ordinal) in lessons.Select((code, i) => (code, i + 1)))
        {
            var reasons = new List<string>();
            var generatedForLesson = 0;
            var familiesTested = 0;
            if (!LessonPracticeCapabilityResolver.TryResolve(code, out var contract) ||
                contract is null)
            {
                reasons.Add("MISSING_EXACT_CONTRACT");
            }
            else if (contract.AllowedQuestionFamilies.Count == 0)
            {
                reasons.Add("NO_ALLOWED_FAMILIES");
            }
            else
            {
                foreach (var family in contract.AllowedQuestionFamilies
                             .Distinct(StringComparer.Ordinal))
                {
                    familiesTested++;
                    if (!ExactSkillContractQuestionEngine.SupportsFamily(family))
                    {
                        reasons.Add(family + ": FAMILY_NOT_SUPPORTED");
                        continue;
                    }
                    foreach (var difficulty in new[] {
                        ExactSkillQuestionDifficulty.Standard,
                        ExactSkillQuestionDifficulty.Stretch,
                        ExactSkillQuestionDifficulty.Challenge
                    })
                    {
                        // Stable seed and distinct candidate family on each run.
                        var seed = 16000000 + 10000 * ordinal +
                            100 * familiesTested + (int)difficulty;
                        try
                        {
                            var question = Assert.Single(engine.Generate(
                                "us-ccss-b001-sharded-engine-evidence",
                                code, [family], difficulty, 1, seed, []));
                            if (question.Family != family)
                            {
                                reasons.Add(family + ": ROUTED_DIFFERENT_FAMILY");
                                continue;
                            }
                            generatedForLesson++;
                            generatedCount++;
                            if (!ExactSkillContractQuestionEngine.Verify(
                                    family, question.Parameters, question.CorrectAnswer))
                                reasons.Add(family + ": CORRECT_ANSWER_REJECTED");
                            if (ExactSkillContractQuestionEngine.Verify(
                                    family, question.Parameters, question.CorrectAnswer + "@@invalid"))
                                reasons.Add(family + ": MALFORMED_ANSWER_ACCEPTED");
                            if (ExactSkillContractQuestionEngine.Verify(
                                    family, question.Parameters, "@@not-a-student-answer@@"))
                                reasons.Add(family + ": GARBAGE_ANSWER_ACCEPTED");
                            if (string.IsNullOrWhiteSpace(question.ExposureFingerprint))
                                reasons.Add(family + ": MISSING_EXPOSURE_FINGERPRINT");
                        }
                        catch (Exception e)
                        {
                            reasons.Add(family + "/" + difficulty + ": " +
                                e.GetType().Name + ": " + e.Message);
                        }
                    }
                }
            }
            if (reasons.Count > 0)
                errors.Add($"{code}: {string.Join("; ", reasons)}");
            else
                passedLessons++;
            rows.Add(new {
                ordinal, lessonCode = code, familiesTested, generatedCases = generatedForLesson,
                status = reasons.Count == 0 ? "ENGINE_SAMPLES_PASSED" : "ENGINE_SAMPLES_FAILED",
                reasons, authenticatedPersistence = "NOT_TESTED",
                sourceGoalTeachingFidelity = "NOT_CERTIFIED",
                effectiveStudentBodyParity = "NOT_TESTED",
                productionLiveRelease = "NOT_VERIFIED"
            });
        }

        var artifactDir = Path.Combine(root, "artifacts", "curriculum-delivery",
            "US-CCSS-MATH", "B001");
        Directory.CreateDirectory(artifactDir);
        var reportPath = Path.Combine(artifactDir, "practice-engine-slice.json");
        var report = new {
            batchId = "US-CCSS-B001",
            testType = "B001_ENGINE_SAMPLING_NOT_FINAL_ACADEMIC_CERTIFICATION",
            candidateSha = Environment.GetEnvironmentVariable("GITHUB_SHA") ?? "local-unpinned",
            lessonCount = rows.Count,
            lessonEngineSamplePassCount = passedLessons,
            lessonEngineFailureCount = errors.Count,
            generatedCases = generatedCount,
            academicDecision = "NOT_ACCEPTED",
            authenticatedDatabasePersistence = "NOT_TESTED",
            sourceContentAlignment = "NOT_CERTIFIED",
            lessons = rows
        };
        File.WriteAllText(reportPath,
            JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }) + "\n");

        Assert.True(generatedCount >= 500 * 3,
            $"Expected at least 3 generated cases per lesson, got {generatedCount}");
        Assert.True(errors.Count == 0,
            $"{errors.Count} B001 Practice engine sample failures: " +
            string.Join(" | ", errors.Take(40)));
    }
}

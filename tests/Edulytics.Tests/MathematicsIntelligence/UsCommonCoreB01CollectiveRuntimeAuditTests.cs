using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// One simultaneous 500-lesson academic-engine audit on the EFFECTIVE
/// materialized student content, not the uncorrected publisher-derived JSON.
/// Does not certify L1-L6, visuals, rendering, or live student persistence.
/// </summary>
public sealed class UsCommonCoreB01CollectiveRuntimeAuditTests
{
    private const string ManifestRelative =
        "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json";

    [Fact]
    public void InspectAll500EffectiveBodiesAndGenerateEveryPracticeFamily()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(
            File.ReadAllText(Path.Combine(root, ManifestRelative)));
        var entries = manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(500, entries.Length);

        var all = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010")
            .SelectMany(d => d.Lessons.Select(l => (Document: d, Lesson: l)))
            .GroupBy(x => x.Lesson.LessonCode, StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => Assert.Single(g), StringComparer.Ordinal);

        var failures = new List<string>();
        var rows = new List<object>(500);
        var engine = new ExactSkillContractQuestionEngine();
        var seed = 1300000;
        var generatedCases = 0;
        var wrongNumericAnswersRejected = 0;
        var richness = new Dictionary<string, int>(StringComparer.Ordinal);
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var difficultyLevels = new[] {
            ExactSkillQuestionDifficulty.Standard,
            ExactSkillQuestionDifficulty.Stretch,
            ExactSkillQuestionDifficulty.Challenge
        };

        foreach (var entry in entries)
        {
            var ordinal = entry.GetProperty("ordinal").GetInt32();
            var lessonCode = entry.GetProperty("lessonCode").GetString()
                ?? throw new InvalidOperationException("Null lesson code.");
            var issues = new List<string>();
            if (ordinal != rows.Count + 1 || !seen.Add(lessonCode))
                issues.Add("DUPLICATE_OR_UNORDERED_LESSON");
            if (!all.TryGetValue(lessonCode, out var source))
            {
                failures.Add(lessonCode + ": missing EFFECTIVE learner lesson");
                rows.Add(new { ordinal, lessonCode, issues = new[] { "MISSING_EFFECTIVE_CONTENT" } });
                continue;
            }

            var doc = source.Document;
            var lesson = source.Lesson;
            var english = lesson.Translations.SingleOrDefault(t => t.CultureCode == "en");
            if (english is null)
            {
                issues.Add("NO_UNIQUE_ENGLISH_TRANSLATION");
                failures.Add(lessonCode + ": missing English translation");
                rows.Add(new { ordinal, lessonCode, issues });
                continue;
            }
            if (lesson.SourceUrl != entry.GetProperty("sourceUrl").GetString())
                issues.Add("SOURCE_URL_DRIFT");
            if (english.Title != entry.GetProperty("title").GetString())
                issues.Add("TITLE_DRIFT");
            var expectedOutcomes = entry.GetProperty("officialOutcomeCodes")
                .EnumerateArray().Select(x => x.GetString()).ToArray();
            if (!lesson.OutcomeCodes.SequenceEqual(expectedOutcomes, StringComparer.Ordinal))
                issues.Add("OUTCOME_CODES_DRIFT");

            var fingerprint = CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc, lesson);
            if (fingerprint.Length != 64)
                issues.Add("INVALID_EFFECTIVE_CONTENT_FINGERPRINT");
            var audit = RichLessonContentQualityAudit.Evaluate(doc, lesson, english);
            var quality = audit.OverallQuality.ToString();
            richness[quality] = richness.GetValueOrDefault(quality) + 1;

            var familyCount = 0;
            var lessonGenerated = 0;
            var lessonWrongRejected = 0;
            if (!LessonPracticeCapabilityResolver.TryResolve(lessonCode, out var contract) ||
                contract is null || contract.AllowedQuestionFamilies.Count == 0)
            {
                issues.Add("NO_READY_VERIFIED_PRACTICE_CONTRACT");
            }
            else
            {
                foreach (var family in contract.AllowedQuestionFamilies.Distinct(StringComparer.Ordinal))
                {
                    familyCount++;
                    foreach (var difficulty in difficultyLevels)
                    {
                        try
                        {
                            var question = Assert.Single(engine.Generate(
                                "b01-collective-500-runtime",
                                lessonCode, [family], difficulty, 1, seed++, []));
                            if (question.Family != family)
                                issues.Add("WRONG_QUESTION_FAMILY");
                            if (string.IsNullOrWhiteSpace(question.Prompt) ||
                                string.IsNullOrWhiteSpace(question.CorrectAnswer) ||
                                string.IsNullOrWhiteSpace(question.Solution))
                                issues.Add("EMPTY_QUESTION_PROMPT_ANSWER_OR_SOLUTION");
                            if (!ExactSkillContractQuestionEngine.Verify(
                                    family, question.Parameters, question.CorrectAnswer))
                                issues.Add("CORRECT_ANSWER_VERIFICATION_FAILED");
                            lessonGenerated++;
                            generatedCases++;

                            if (int.TryParse(question.CorrectAnswer, NumberStyles.Integer,
                                CultureInfo.InvariantCulture, out var correctNumber))
                            {
                                var wrongAnswer = (correctNumber + 100003).ToString(
                                    CultureInfo.InvariantCulture);
                                if (ExactSkillContractQuestionEngine.Verify(
                                        family, question.Parameters, wrongAnswer))
                                    issues.Add("WRONG_NUMERIC_ANSWER_ACCEPTED");
                                else
                                {
                                    wrongNumericAnswersRejected++;
                                    lessonWrongRejected++;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            issues.Add("GENERATOR_EXCEPTION:" + family + "/" +
                                difficulty + ":" + e.GetType().Name);
                        }
                    }
                }
            }
            failures.AddRange(issues.Select(e => lessonCode + ": " + e));
            rows.Add(new {
                ordinal, lessonCode, effectiveContentSha256 = fingerprint,
                learnerTeachingText = string.Join(" ",
                    english.Explanation,
                    english.KeyConceptsAndRules,
                    english.WorkedExamples,
                    english.StepByStepSolutions),
                sourcePreparationUrl = lesson.SourceUrl,
                effectiveContentVersion = CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(doc, lesson),
                richV2StructuralQuality = quality,
                richV2VisualHeuristic = audit.VisualQuality.ToString(),
                richV2QualityFindings = audit.Findings,
                practiceFamilyCount = familyCount,
                practiceSkillId = contract?.SkillId,
                practiceMechanic = contract?.Mechanic,
                actualQuestionFamilies = contract?.AllowedQuestionFamilies,
                generatedCases = lessonGenerated,
                rejectedWrongNumericAnswers = lessonWrongRejected,
                engineIssues = issues,
                evidenceBoundary = "RUNTIME_CONTENT_AND_GENERATOR_PROBE_ONLY_NOT_L1_L6_CERTIFICATION"
            });
        }

        var report = new {
            kind = "B01_500_EFFECTIVE_CONTENT_AND_REAL_PRACTICE_RUNTIME_PROBE",
            inspected = entries.Length, unique = seen.Count,
            runtimeCasesGenerated = generatedCases,
            wrongNumericAnswersRejected,
            richStructuralQualityCounts = richness,
            runtimeFailures = failures.Count,
            failurePreview = failures.Take(80).ToArray(),
            lessonRows = rows,
            academicDecision = "NOT_CERTIFIED",
            caveat = "Real generator/solver and effective materializer tested. " +
                "No source-semantic, diagram-pixel, browser persistence, or L1-L6 claim."
        };
        var output = Path.Combine(root, "b01-effective-content-and-practice.json");
        File.WriteAllText(output,
            JsonSerializer.Serialize(report, new JsonSerializerOptions {
                WriteIndented = true
            }) + Environment.NewLine);
        Assert.Equal(500, seen.Count);
        Assert.True(generatedCases >= 500 * 3,
            "Missing generated family/difficulty coverage: " + generatedCases);
        Assert.True(failures.Count == 0,
            failures.Count + " B01 effective runtime failures: " +
                string.Join(" | ", failures.Take(45)));
    }

    private static string FindRoot()
    {
        DirectoryInfo? dir = new(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, ManifestRelative)))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new FileNotFoundException("B01 source-grounded manifest not found.");
    }
}

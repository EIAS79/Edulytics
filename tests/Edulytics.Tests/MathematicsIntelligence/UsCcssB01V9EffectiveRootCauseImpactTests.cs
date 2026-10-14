using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// V9 B01: measures the ACTUAL, correction-materialized English canonical body
/// and composed/sidecar Rich V2 for all exact 500 learner lessons in one run.
/// This is root-cause impact evidence, not proof of textbook alignment or L6.
/// </summary>
public sealed class UsCcssB01V9EffectiveRootCauseImpactTests
{
    private static string Sha(string? value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value ?? "")))
            .ToLowerInvariant();

    private static string FindRoot()
    {
        DirectoryInfo? d = new(Directory.GetCurrentDirectory());
        while (d is not null)
        {
            if (File.Exists(Path.Combine(d.FullName,
                "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")))
                return d.FullName;
            d = d.Parent;
        }
        throw new FileNotFoundException("Cannot locate B01 manifest.");
    }

    [Fact]
    public void Write500CorrectedMaterializedAndComposedRootCauseFactsWithoutCertification()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")));
        var entries = manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(500, entries.Length);
        var canonical = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010")
            .SelectMany(d => d.Lessons.Select(l => (Doc: d, Lesson: l)))
            .ToDictionary(x => x.Lesson.LessonCode, StringComparer.Ordinal);
        var rows = new List<object>(500);
        var codes = new HashSet<string>(StringComparer.Ordinal);
        var noRich = new List<string>();
        int explicitSidecars = 0, totalVisuals = 0;
        foreach (var entry in entries)
        {
            int ordinal = entry.GetProperty("ordinal").GetInt32();
            var code = entry.GetProperty("lessonCode").GetString()
                ?? throw new InvalidOperationException("Missing lesson identity");
            Assert.Equal(rows.Count + 1, ordinal);
            Assert.True(codes.Add(code), "Duplicate manifest lesson: " + code);
            Assert.True(canonical.TryGetValue(code, out var item),
                "Missing materialized canonical lesson: " + code);
            var english = item.Lesson.Translations.Single(x => x.CultureCode == "en");
            var sidecar = RichLessonContentV2Registry.Find(code, "en");
            var rich = sidecar ?? RichLessonContentV2RuntimeComposer.TryCompose(
                code, new CanonicalLessonTranslationRecord(
                    english.CultureCode, english.Title, english.Explanation,
                    english.KeyConceptsAndRules, english.WorkedExamples,
                    english.StepByStepSolutions, english.CommonMistakes,
                    english.QuickSummary));
            if (rich is null) noRich.Add(code);
            else
            {
                if (sidecar is not null) explicitSidecars++;
                totalVisuals += rich.Visuals.Count;
            }
            bool measureCue = english.StepByStepSolutions.Contains(
                "Step 1: Identify what needs to be measured.", StringComparison.Ordinal) ||
                english.WorkedExamples.Contains(
                "Approach: Choose the required unit, align equal units from the endpoint",
                StringComparison.Ordinal) ||
                english.KeyConceptsAndRules.Contains(
                "Measurement units must be equal in size and placed or interpreted without gaps",
                StringComparison.Ordinal) ||
                english.CommonMistakes.Contains(
                "Common mistake: using unequal units, leaving gaps, overlapping units",
                StringComparison.Ordinal);
            bool figureCaption = Regex.IsMatch(
                english.WorkedExamples + "\n" + english.Explanation,
                @"\bCaption:\s*[A-Z]\b", RegexOptions.CultureInvariant);
            bool pronounCue = Regex.IsMatch(
                english.Explanation + "\n" + english.WorkedExamples,
                @"\b(?:support them in adding|as they learn a new stage|they are observing)\b",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            rows.Add(new
            {
                ordinal, lessonCode = code,
                effectiveContentVersion = CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(item.Doc, item.Lesson),
                effectiveBodyFingerprint = CanonicalLessonContentMaterializer
                    .ComputeLessonFingerprint(item.Doc, item.Lesson),
                exactEffectiveFieldSha256 = new {
                    stepByStepSolutions = Sha(english.StepByStepSolutions),
                    commonMistakes = Sha(english.CommonMistakes)
                },
                effectiveRawPatternSignals = new {
                    rulerTemplate = measureCue,
                    figureCaptionPlaceholder = figureCaption,
                    learnerTeacherPronoun = pronounCue
                },
                richV2 = new {
                    source = sidecar is null ? "RUNTIME_COMPOSER" : "EXPLICIT_SIDECAR",
                    composed = rich is not null,
                    visualCount = rich?.Visuals.Count ?? 0,
                    visualKinds = rich?.Visuals.Select(v => v.Kind.ToString()).ToArray() ??
                        Array.Empty<string>(),
                    workedExampleCount = rich?.WorkedExamples.Count ?? 0,
                    renderabilityDoesNotProveL6 = true
                },
                academicDisposition = "EFFECTIVE_RUNTIME_EVIDENCE_ONLY_NOT_CERTIFIED"
            });
        }
        Assert.Empty(noRich);
        Assert.Equal(500, codes.Count);
        var report = new {
            schemaVersion = 1, planId = "EDULYTICS-US-CCSS-UNIFIED-V9",
            kind = "B01_V9_EFFECTIVE_500_ROOT_CAUSE_IMPACT",
            batchId = "B01",
            coverage = new { expected = 500, materialized = rows.Count, unique = codes.Count },
            explicitSidecars, totalVisuals, noRich,
            rows, academicDecision = "BATCH_NO-GO",
            warning = "Effective English canonical text and real Rich V2 structural composition " +
                "are observed. Raw-pattern visibility is not semantic defect proof. " +
                "Rendered browser L6, publisher figures and activity equality are not verified."
        };
        File.WriteAllText(Path.Combine(root, "b01-v9-effective-root-cause-500.json"),
            JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true })
                + Environment.NewLine);
    }
}
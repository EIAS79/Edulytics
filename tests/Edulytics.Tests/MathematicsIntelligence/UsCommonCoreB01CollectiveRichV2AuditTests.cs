using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// One 500-lesson pass through actual Rich V2 composition and sidecar resolution.
/// Structural runtime coverage only: screenshots, exact visual correctness,
/// authenticated browser interactions and persistence are separate L6 checks.
/// </summary>
public sealed class UsCommonCoreB01CollectiveRichV2AuditTests
{
    [Fact]
    public void ComposeAll500B01StudentRichV2LessonsAndInventoryRepresentations()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")));
        var specs = manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(500, specs.Length);
        var effective = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(doc => doc.PackCode == "US-CCSS-MATH" &&
                doc.VersionCode == "CCSSM-2010")
            .SelectMany(doc => doc.Lessons.Select(lesson => new {
                Doc = doc, Lesson = lesson
            }))
            .ToDictionary(x => x.Lesson.LessonCode, StringComparer.Ordinal);

        var rows = new List<object>(500);
        var failures = new List<string>();
        var richVisualKinds = new Dictionary<string, int>(StringComparer.Ordinal);
        var countNoVisual = 0;
        var explicitSidecars = 0;
        var runtimeComposed = 0;
        var withRich = 0;
        var totalVisuals = 0;
        var totalExamples = 0;
        foreach (var entry in specs)
        {
            var ordinal = entry.GetProperty("ordinal").GetInt32();
            var code = entry.GetProperty("lessonCode").GetString()
                ?? throw new InvalidOperationException("Missing lesson code");
            if (!effective.TryGetValue(code, out var source))
            {
                failures.Add(code + ": missing effective canonical source");
                rows.Add(new { ordinal, lessonCode = code,
                    status = "NO_CANONICAL_SOURCE" });
                continue;
            }
            var en = source.Lesson.Translations.Single(x => x.CultureCode == "en");
            var sidecar = RichLessonContentV2Registry.Find(code, "en");
            var rich = sidecar ?? RichLessonContentV2RuntimeComposer.TryCompose(
                code, new CanonicalLessonTranslationRecord(
                    en.CultureCode, en.Title, en.Explanation, en.KeyConceptsAndRules,
                    en.WorkedExamples, en.StepByStepSolutions, en.CommonMistakes,
                    en.QuickSummary));
            if (rich is null)
            {
                failures.Add(code + ": no usable Rich V2 content");
                rows.Add(new { ordinal, lessonCode = code,
                    status = "NO_RICH_V2_CONTENT" });
                continue;
            }

            // Verify the fully materialized learner-visible lesson and visual,
            // not merely an isolated helper object or raw JSON screening.
            if (code == "PED:US-CCSS-MATH:G3:U03:L04")
            {
                Assert.DoesNotContain("Read the hour and minute hands separately",
                    en.KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
                var chart = Assert.Single(rich.Visuals.Where(x =>
                    x.Kind == RichLessonVisualKind.PlaceValueChart));
                Assert.Equal(new[] {"Hundreds","Tens","Ones"}, chart.Rows[0].Cells);
                Assert.Equal(new[] {"7","1","6"}, chart.Rows[^1].Cells);
                Assert.Equal(716, 362 + 354);
            }

            if (code == "PED:US-CCSS-MATH:G1:U01:L06")
            {
                Assert.Contains("6 + 4 = 10",en.WorkedExamples);
                var chart=Assert.Single(rich.Visuals.Where(v=>
                    v.Kind==RichLessonVisualKind.PlaceValueChart));
                Assert.Equal(new[] {"●","●","●","●","●"},chart.Rows[0].Cells);
                Assert.Equal(new[] {"●","○","○","○","○"},chart.Rows[1].Cells);
                Assert.Equal(10,chart.Rows.Sum(row=>row.Cells.Count));
                Assert.DoesNotContain("flash cards",en.WorkedExamples,
                    StringComparison.OrdinalIgnoreCase);
            }

            if (code is "PED:US-CCSS-MATH:G1:U01:L07"
                or "PED:US-CCSS-MATH:G1:U01:L08"
                or "PED:US-CCSS-MATH:G1:U01:L09")
            {
                var display=Assert.Single(rich.Visuals.Where(v=>
                    v.Kind==RichLessonVisualKind.PlaceValueChart));
                Assert.Equal(3,display.Rows.Count);
                Assert.Equal(9,display.Rows.Sum(row=>row.Cells.Count));
                Assert.All(display.Rows,row=>
                {
                    Assert.False(string.IsNullOrWhiteSpace(row.Label));
                    Assert.True(row.Cells.Count>=2);
                });
                Assert.Contains("category",display.Description,
                    StringComparison.OrdinalIgnoreCase);
            }

            withRich++;
            if (sidecar is not null) explicitSidecars++;
            else runtimeComposed++;
            totalVisuals += rich.Visuals.Count;
            totalExamples += rich.WorkedExamples.Count;
            if (rich.Visuals.Count == 0) countNoVisual++;
            foreach (var visual in rich.Visuals)
            {
                var kind = visual.Kind.ToString();
                richVisualKinds[kind] = richVisualKinds.GetValueOrDefault(kind) + 1;
            }
            if (rich.LessonCode != code || rich.CultureCode != "en")
                failures.Add(code + ": Rich V2 identity or culture mismatch");
            if (rich.ExplanationParagraphs.Count == 0 || rich.KeyConcepts.Count == 0 ||
                rich.WorkedExamples.Count == 0 || rich.SummaryPoints.Count == 0)
                failures.Add(code + ": Rich V2 is missing essential narrative or worked examples");
            rows.Add(new {
                ordinal, lessonCode = code,
                effectiveBodyFingerprint = CanonicalLessonContentMaterializer
                    .ComputeLessonFingerprint(source.Doc, source.Lesson),
                source = sidecar is null ? "RUNTIME_COMPOSER" : "EXPLICIT_SIDECAR",
                visualCount = rich.Visuals.Count,
                visualKinds = rich.Visuals.Select(x => x.Kind.ToString()).ToArray(),
                workedExampleCount = rich.WorkedExamples.Count,
                explanationParagraphs = rich.ExplanationParagraphs.Count,
                keyConcepts = rich.KeyConcepts.Count,
                commonMistakes = rich.CommonMistakes.Count,
                summaryPoints = rich.SummaryPoints.Count,
                status = rich.Visuals.Count == 0 ?
                    "RICH_V2_RENDERABILITY_PRESENT_VISUAL_REVIEW_REQUIRED" :
                    "RICH_V2_COMPOSED_VISUAL_SEMANTICS_NOT_YET_VERIFIED"
            });
        }

        var report = new {
            kind = "B01_500_REAL_RICH_V2_COMPOSITION_INVENTORY",
            expected = 500, visited = specs.Length, composed = withRich,
            explicitSidecars, runtimeComposed, totalVisuals,
            totalWorkedExamples = totalExamples, lessonsWithoutVisuals = countNoVisual,
            visualKindCounts = richVisualKinds,
            failures, rows, academicDecision = "NOT_CERTIFIED",
            note = "A generated visual component is not proof of pedagogical correctness. " +
                "L6 requires authenticated actual rendering, desktop/mobile, accessibility and grading."
        };
        File.WriteAllText(Path.Combine(root, "b01-rich-v2-500.json"),
            JsonSerializer.Serialize(report, new JsonSerializerOptions {
                WriteIndented = true
            }) + Environment.NewLine);
        Assert.True(failures.Count == 0,
            failures.Count + " Rich V2 runtime failures: " +
                string.Join(" | ", failures.Take(30)));
        Assert.Equal(500, withRich);
    }

    private static string FindRoot()
    {
        DirectoryInfo? dir = new(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName,
                "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new FileNotFoundException("B01 collective manifest not found.");
    }
}

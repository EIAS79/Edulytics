using System.Text.Json;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Tests source-entity presentation for all 20 PHASE 04 flagged US CCSS
/// lessons. Passing this is parser-level evidence, not authenticated UI
/// acceptance or independent academic certification.
/// </summary>
public sealed class CommonCorePhase04SourceEntityPresentationTests
{
    [Fact]
    public void AllFlaggedOriginalExamples_NormalizeEntitiesWithoutChangingSourcePacks()
    {
        var root = FindRoot();
        using var audit = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "artifacts/academic-us-ccss/04-automated-content-risk-audit.json")));

        var flagged = audit.RootElement.GetProperty("lessons")
            .EnumerateArray()
            .Where(record => record.GetProperty("contentRiskFlags")
                .EnumerateArray()
                .Any(flag => flag.GetString() ==
                    "PUBLISHER_HTML_ENTITY_LEAK_IN_WORKED_EXAMPLES"))
            .ToArray();
        Assert.Equal(20, flagged.Length);

        // The exact eight Grade 5 original-source-bound display repairs
        // are now applied in the candidate pack's WORKED EXAMPLES only.
        // This finite reviewed source-identity list must never authorize
        // arbitrary deletion of HTML-entity warnings in another lesson.
        var grade5AuthoringEntityFixes = new HashSet<string>(StringComparer.Ordinal)
        {
            "PED:US-CCSS-MATH:G5:U01:L10",
            "PED:US-CCSS-MATH:G5:U04:L14",
            "PED:US-CCSS-MATH:G5:U05:L11",
            "PED:US-CCSS-MATH:G5:U05:L25",
            "PED:US-CCSS-MATH:G5:U06:L08",
            "PED:US-CCSS-MATH:G5:U08:L02",
            "PED:US-CCSS-MATH:G5:U08:L05",
            "PED:US-CCSS-MATH:G5:U08:L09"
        };
        var observedGrade5EntityFixes = new HashSet<string>(StringComparer.Ordinal);
        var packs = new Dictionary<string, JsonDocument>(StringComparer.Ordinal);
        try
        {
            foreach (var entry in flagged)
            {
                var lessonCode = entry.GetProperty("lessonCode").GetString()!;
                var contentPath = entry.GetProperty("contentSourceFile").GetString()!;
                if (!packs.TryGetValue(contentPath, out var pack))
                {
                    pack = JsonDocument.Parse(File.ReadAllText(Path.Combine(
                        root, contentPath.Replace('/', Path.DirectorySeparatorChar))));
                    packs.Add(contentPath, pack);
                }

                var lesson = pack.RootElement.GetProperty("lessons")
                    .EnumerateArray()
                    .Single(candidate => candidate.GetProperty("lessonCode")
                        .GetString() == lessonCode);
                var english = lesson.GetProperty("translations")
                    .EnumerateArray()
                    .Single(candidate => candidate.GetProperty("cultureCode")
                        .GetString() == "en");
                var originalExamples = english.GetProperty("workedExamples").GetString()!;
                // The Phase 04 audit records the historical original source. The
                // owner-authored Grade 4 replacement may remove raw HTML entities
                // at authoring time; all other flagged source packs must retain
                // their original source warning and still be safely parsed.
                var hasOriginalEntity =
                    originalExamples.Contains("&nbsp;", StringComparison.Ordinal) ||
                    originalExamples.Contains("&lt;", StringComparison.Ordinal);
                var permittedGrade5Repair = grade5AuthoringEntityFixes.Contains(lessonCode);
                Assert.True(hasOriginalEntity ||
                    lessonCode.StartsWith("PED:US-CCSS-MATH:G4:", StringComparison.Ordinal) ||
                    permittedGrade5Repair,
                    $"An unapproved source entity was removed: {lessonCode}");
                if (permittedGrade5Repair)
                {
                    observedGrade5EntityFixes.Add(lessonCode);
                    Assert.DoesNotContain("&nbsp;", originalExamples,
                        StringComparison.Ordinal);
                }

                var parsed = LessonPresentationParser.ToPlainText(originalExamples);
                var readerItems = LessonPresentationParser.Parse(
                    originalExamples, sectionKind: "examples");
                foreach (var item in readerItems)
                {
                    if (item.Text is not null)
                        Assert.DoesNotContain("\u00A0", item.Text, StringComparison.Ordinal);
                    if (item.Visual is not null)
                        Assert.DoesNotContain("\u00A0",
                            item.Visual.AccessibilityText, StringComparison.Ordinal);
                }

                Assert.True(parsed.Length > 20, $"Empty presentation: {lessonCode}");
                Assert.DoesNotContain("&nbsp;", parsed, StringComparison.Ordinal);
                Assert.DoesNotContain("&lt;", parsed, StringComparison.Ordinal);
                Assert.DoesNotContain("\u00A0", parsed, StringComparison.Ordinal);
                Assert.DoesNotContain("<p>", parsed, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("</p>", parsed, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("<strong>", parsed, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("</strong>", parsed, StringComparison.OrdinalIgnoreCase);
            }
        }
        finally
        {
            foreach (var pack in packs.Values)
                pack.Dispose();
        }
        Assert.Equal(
            grade5AuthoringEntityFixes.OrderBy(x => x, StringComparer.Ordinal),
            observedGrade5EntityFixes.OrderBy(x => x, StringComparer.Ordinal));
    }

    [Theory]
    [InlineData("Find 3/5 &lt; 5/8.&nbsp;Check.", "Find 3/5 < 5/8. Check.")]
    [InlineData("&lt;p&gt;Part&nbsp;&amp;nbsp;B&lt;/p&gt;", "Part B")]
    [InlineData("Area: 4 &times; 7 = 28.&nbsp;Done.", "Area: 4 × 7 = 28. Done.")]
    public void EncodedMarkup_KeepsMathAndProducesReadableSpaces(string source, string expected)
    {
        Assert.Equal(expected, LessonPresentationParser.ToPlainText(source));
    }

    private static string FindRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory);
            directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;
        }

        throw new DirectoryNotFoundException("Repository root not found.");
    }
}

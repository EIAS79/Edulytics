using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// PHASE 04: encoded canonical content through the Rich V2 runtime route.
/// Engineering tests cannot replace independent academic approval.
/// </summary>
public sealed class CommonCorePhase04RichV2SourceEntityTests
{
    [Fact]
    public void AllTwentyFlaggedSourceLessons_HaveCleanRichV2LearnerFacingText()
    {
        var root = FindRoot();
        using var audit = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            root, "artifacts", "academic-us-ccss",
            "04-automated-content-risk-audit.json")));

        var flagged = audit.RootElement.GetProperty("lessons")
            .EnumerateArray()
            .Where(record => record.GetProperty("contentRiskFlags")
                .EnumerateArray()
                .Any(flag => flag.GetString() ==
                    "PUBLISHER_HTML_ENTITY_LEAK_IN_WORKED_EXAMPLES"))
            .Select(record => record.GetProperty("lessonCode").GetString()!)
            .ToArray();

        Assert.Equal(20, flagged.Length);
        Assert.Equal(20, flagged.Distinct(StringComparer.Ordinal).Count());

        var lessonsByCode = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(doc => doc.PackCode == "US-CCSS-MATH")
            .SelectMany(doc => doc.Lessons)
            .ToDictionary(lesson => lesson.LessonCode, StringComparer.Ordinal);

        // No CCSS rich-curated sidecar exists; otherwise the reader would
        // take a different path and this acceptance contract must change.
        foreach (var code in flagged)
        {
            Assert.Null(RichLessonContentV2Registry.Find(code, "en"));
            var lesson = lessonsByCode[code];
            var english = Assert.Single(lesson.Translations,
                item => item.CultureCode == "en");

            var rich = RichLessonContentV2RuntimeComposer.TryCompose(
                code, AsBody(english));
            Assert.NotNull(rich);

            var visible = string.Join("\n",
                rich!.ExplanationParagraphs
                    .Concat(rich.KeyConcepts.SelectMany(x =>
                        new[] { x.Title, x.Definition, x.Rule, x.Example }))
                    .Concat(rich.CommonMistakes.SelectMany(x =>
                        new[] { x.Mistake, x.WhyWrong, x.Correction }))
                    .Concat(rich.SummaryPoints)
                    .Concat(rich.WorkedExamples.SelectMany(x =>
                        new[] { x.Title, x.Question, x.Method, x.Answer, x.Check }))
                    .Concat(rich.WorkedExamples.SelectMany(x => x.Steps)));

            Assert.DoesNotContain("&nbsp;", visible, StringComparison.Ordinal);
            Assert.DoesNotContain("&amp;nbsp;", visible, StringComparison.Ordinal);
            Assert.DoesNotContain("&lt;", visible, StringComparison.Ordinal);
            Assert.DoesNotContain("\u00A0", visible, StringComparison.Ordinal);
            Assert.DoesNotContain("<p>", visible, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("</p>", visible, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("<strong>", visible, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("</strong>", visible, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void RichV2Runtime_PreservesComparisonsAndMultiplicationWhileRemovingEncodedTags()
    {
        const string code = "PED:US-CCSS-MATH:G2:U04:L03";
        var lesson = MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .Where(doc => doc.PackCode == "US-CCSS-MATH")
            .SelectMany(doc => doc.Lessons)
            .Single(item => item.LessonCode == code);
        var source = Assert.Single(lesson.Translations,
            item => item.CultureCode == "en");

        var body = new CanonicalLessonTranslationRecord(
            source.CultureCode,
            source.Title,
            "Compare 3/5 &lt; 5/8.&nbsp;Explain why.",
            "<strong>Multiply</strong> numbers; compare 3 &lt; 4.",
            "&lt;p&gt;Compute 4 &times; 7 &amp;nbsp; = 28.&lt;/p&gt;",
            source.StepByStepSolutions,
            source.CommonMistakes,
            source.QuickSummary);

        var rich = RichLessonContentV2RuntimeComposer.TryCompose(code, body);
        Assert.NotNull(rich);
        Assert.Contains("Compare 3/5 < 5/8. Explain why.",
            rich!.ExplanationParagraphs[0], StringComparison.Ordinal);
        Assert.Contains("Multiply numbers; compare 3 < 4.",
            rich.ExplanationParagraphs[1], StringComparison.Ordinal);
        Assert.Contains("Compute 4 × 7 = 28.",
            rich.ExplanationParagraphs[3], StringComparison.Ordinal);
        Assert.DoesNotContain("&nbsp;",
            rich.KeyConcepts[0].Example, StringComparison.Ordinal);
        Assert.DoesNotContain("<p>",
            rich.KeyConcepts[0].Example, StringComparison.Ordinal);
    }

    private static CanonicalLessonTranslationRecord AsBody(
        CanonicalLessonContentPackTranslation t) =>
        new(t.CultureCode, t.Title, t.Explanation, t.KeyConceptsAndRules,
            t.WorkedExamples, t.StepByStepSolutions, t.CommonMistakes,
            t.QuickSummary);

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

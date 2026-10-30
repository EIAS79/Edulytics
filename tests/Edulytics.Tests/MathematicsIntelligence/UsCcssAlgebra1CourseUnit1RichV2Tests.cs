using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-grounded authoring and mathematical invariants for three independently
/// authored Algebra 1 Unit 1 Rich V2 lessons. These are not academic or
/// authenticated production acceptance tests.
/// </summary>
public sealed class UsCcssAlgebra1CourseUnit1RichV2Tests
{
    private const string Prefix = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:";

    [Theory]
    [InlineData("L01", "Getting to Know You", "/1/1/1/preparation.html")]
    [InlineData("L02", "Data Representations", "/1/1/2/preparation.html")]
    [InlineData("L03", "A Gallery of Data", "/1/1/3/preparation.html")]
    public void SourceBoundRichLessonsAreReadableAndContainFullReasoning(
        string ordinal, string title, string sourceSuffix)
    {
        var rich = RichLessonContentV2Registry.Find(Prefix + ordinal, "en");
        Assert.NotNull(rich);
        Assert.Equal(title, rich.Title);
        Assert.Contains(rich.ExplanationParagraphs,
            x => x.Contains(sourceSuffix, StringComparison.Ordinal));
        Assert.True(rich.ExplanationParagraphs.Count >= 2);
        Assert.True(rich.KeyConcepts.Count >= 3);
        Assert.True(rich.WorkedExamples.Count >= 3);
        Assert.All(rich.WorkedExamples,
            example => Assert.True(example.Steps.Count >= 3));
        Assert.True(rich.CommonMistakes.Count >= 2);
        Assert.True(rich.SummaryPoints.Count >= 3);
        Assert.Contains(rich.ExplanationParagraphs,
            x => x.Contains("Teacher", StringComparison.OrdinalIgnoreCase)
                && x.Contains("rubric", StringComparison.OrdinalIgnoreCase));
        Assert.All(rich.Visuals,
            visual => Assert.False(string.IsNullOrWhiteSpace(visual.Description)));
        Assert.Empty(rich.Videos);
    }

    [Fact]
    public void SurveyExampleIsExplicitlySyntheticAndCannotClaimRealStudentParticipation()
    {
        var rich = RichLessonContentV2Registry.Find(Prefix + "L01", "en");
        Assert.NotNull(rich);
        var prose = string.Join(" ", rich.ExplanationParagraphs);
        Assert.Contains("fictional", prose, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("without names", prose, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("human review", prose, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(rich.WorkedExamples,
            example => example.Answer.Contains("5/12", StringComparison.Ordinal));
        Assert.Contains(rich.WorkedExamples,
            example => example.Answer.Contains("11.2", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("Which Question Is Different?", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("15 classmates", StringComparison.Ordinal)
                && p.Contains("non-statistical", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("Cool-down substitute", StringComparison.Ordinal)
                && p.Contains("NOT independently verified", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("Answer set = {A, C, E}", StringComparison.Ordinal));

    }

    [Fact]
    public void StaticAlgebra1Lesson01SelectorUsesCorrectEmbeddedContentVersion()
    {
        // Source-level validation only, not an authenticated Stage D check.
        var assembly = typeof(RichLessonContentV2Registry).Assembly;
        var resource = Assert.Single(assembly.GetManifestResourceNames(), name =>
            name.EndsWith("us-ccss-hs-alg1-u01-l01-2021.rich-lesson-v2.json",
                StringComparison.OrdinalIgnoreCase));
        using var stream = assembly.GetManifestResourceStream(resource);
        Assert.NotNull(stream);
        using var doc = System.Text.Json.JsonDocument.Parse(stream);
        Assert.Equal("alg1-u01-l01-local-source-aligned-20261030-v3",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        Assert.Equal(Prefix + "L01", Assert.Single(doc.RootElement
            .GetProperty("Lessons").EnumerateArray()).GetProperty("LessonCode").GetString());
        var found = RichLessonContentV2Registry.Find(Prefix + "L01", "en");
        Assert.NotNull(found);
        Assert.Contains(found.ExplanationParagraphs, p =>
            p.Contains("First-party warm-up 1.1", StringComparison.Ordinal));
        Assert.Same(found, RichLessonContentV2Registry.Find(Prefix + "L01", "en-US"));
        var codes = RichLessonContentV2Registry.AllDocuments.SelectMany(d => d.Lessons)
            .Where(x => x.LessonCode.StartsWith(
                "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:", StringComparison.Ordinal))
            .Select(x => x.LessonCode).ToArray();
        Assert.Equal(132, codes.Length);
        Assert.Equal(132, codes.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void DraftSelectAllOracleAcceptsOrderAndCaseButRejectsMalformedAnswers()
    {
        // Deliberately an independent *draft* oracle, NOT runtime Practice.
        static bool Match(IReadOnlyList<string>? responses)
        {
            if (responses is null || responses.Count != 3) return false;
            var values = responses.Select(s => s?.Trim().ToUpperInvariant() ?? "").ToArray();
            return values.All(s => s.Length == 1 && s[0] is >= 'A' and <= 'F')
                && values.Distinct(StringComparer.Ordinal).Count() == 3
                && new HashSet<string>(values, StringComparer.Ordinal).SetEquals(["A", "C", "E"]);
        }
        Assert.True(Match(["A", "C", "E"]));
        Assert.True(Match(["e", "a", "c"]));
        Assert.True(Match([" c ", "E", "a"]));
        Assert.False(Match(["A", "B", "C", "E"])); // extra
        Assert.False(Match(["A", "C"])); // missing
        Assert.False(Match(["A", "a", "C", "E"])); // duplicate case variant
        Assert.False(Match(["A", "C", "Z"])); // unsupported
        Assert.False(Match(["A,C,E"])); // malformed
        Assert.False(Match(["A", "C", ""])); // blank
        Assert.False(Match([]));
        Assert.False(Match(null));
    }

    [Fact]
    public void HistogramCountsAndFiveNumberSummaryAreInternallyConsistent()
    {
        int[] data = [2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 10];
        int[] cutPoints = [2, 4, 6, 8, 10, 12];
        int[] expectedCounts = [3, 3, 3, 2, 1];

        for (var i = 0; i < expectedCounts.Length; i++)
            Assert.Equal(expectedCounts[i], data.Count(x =>
                cutPoints[i] <= x && x < cutPoints[i + 1]));

        Assert.Equal(data.Length, expectedCounts.Sum());
        Assert.Equal(5.5, (data[5] + data[6]) / 2.0);
        Assert.Equal(3.5, (data[2] + data[3]) / 2.0);
        Assert.Equal(7.5, (data[8] + data[9]) / 2.0);

        var rich = RichLessonContentV2Registry.Find(Prefix + "L02", "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.WorkedExamples,
            example => example.Answer.Contains("6, 5, 1", StringComparison.Ordinal));
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("left", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void GalleryShiftPreservesRangeAndIqrAndChangesMedianByFive()
    {
        int[] sampleA = [2, 2, 3, 3, 4, 4, 5, 5];
        int[] sampleB = [7, 7, 8, 8, 9, 9, 10, 10];
        Assert.All(Enumerable.Range(0, sampleA.Length),
            i => Assert.Equal(5, sampleB[i] - sampleA[i]));

        static double Median(int left, int right) => (left + right) / 2.0;
        Assert.Equal(5, Median(sampleB[3], sampleB[4])
            - Median(sampleA[3], sampleA[4]));
        Assert.Equal(sampleA[^1] - sampleA[0], sampleB[^1] - sampleB[0]);
        Assert.Equal(2, Median(sampleA[5], sampleA[6])
            - Median(sampleA[1], sampleA[2]));
        Assert.Equal(2, Median(sampleB[5], sampleB[6])
            - Median(sampleB[1], sampleB[2]));

        var rich = RichLessonContentV2Registry.Find(Prefix + "L03", "en");
        Assert.NotNull(rich);
        Assert.Contains(rich.ExplanationParagraphs,
            p => p.Contains("teacher", StringComparison.OrdinalIgnoreCase)
                && p.Contains("critique", StringComparison.OrdinalIgnoreCase));
    }
    [Fact]
    public void All132CanonicalLessonsReconcileWithTheirActualContentRecords()
    {
        var assembly = typeof(RichLessonContentV2Registry).Assembly;
        var resources = assembly.GetManifestResourceNames();
        var blueprintName = Assert.Single(resources, x =>
            x.EndsWith("us-ccss-math-hs-traditional-algebra1-im1e.lesson-blueprint.json",
                StringComparison.OrdinalIgnoreCase));
        var contentName = Assert.Single(resources, x =>
            x.EndsWith("us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json",
                StringComparison.OrdinalIgnoreCase));

        using var bpStream = assembly.GetManifestResourceStream(blueprintName)!;
        using var packStream = assembly.GetManifestResourceStream(contentName)!;
        using var bp = System.Text.Json.JsonDocument.Parse(bpStream);
        using var pack = System.Text.Json.JsonDocument.Parse(packStream);

        Assert.Equal("ALG1", bp.RootElement.GetProperty("CourseCode").GetString());
        Assert.Equal("Traditional", bp.RootElement.GetProperty("Pathway").GetString());
        var lessons = bp.RootElement.GetProperty("Lessons").EnumerateArray().ToArray();
        var contents = pack.RootElement.GetProperty("lessons").EnumerateArray().ToArray();

        Assert.Equal(132, lessons.Length);
        Assert.Equal(132, contents.Length);
        Assert.Equal(132, lessons.Select(x =>
            x.GetProperty("LessonCode").GetString()).Distinct().Count());
        Assert.Equal(132, lessons.Select(x =>
            x.GetProperty("SourceLessonCode").GetString()).Distinct().Count());
        Assert.Equal(new[] {16, 26, 10, 18, 21, 17, 24},
            lessons.GroupBy(x => x.GetProperty("UnitNumber").GetInt32())
                .OrderBy(group => group.Key).Select(group => group.Count()).ToArray());

        var byCode = contents.ToDictionary(
            x => x.GetProperty("lessonCode").GetString()!,
            x => x);
        foreach (var lesson in lessons)
        {
            var code = lesson.GetProperty("LessonCode").GetString()!;
            Assert.StartsWith("PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:", code);
            Assert.True(byCode.TryGetValue(code, out var content),
                "Missing canonical content for " + code);
            Assert.Equal(lesson.GetProperty("SourceUrl").GetString(),
                content.GetProperty("sourceUrl").GetString());
        }
        Assert.True(lessons[0].GetProperty("Alignments").EnumerateArray()
            .All(x => x.GetProperty("Role").GetString() != "Addressing"));
    }

}

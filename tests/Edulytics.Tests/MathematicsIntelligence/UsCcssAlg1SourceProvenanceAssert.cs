using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source lineage is verified against the internal authored course manifest,
/// not by inserting external publisher URLs into learner-facing explanations.
/// This is provenance/identity evidence, not independent 1:1 academic approval.
/// </summary>
internal static class UsCcssAlg1SourceProvenanceAssert
{
    private static readonly Lazy<JsonDocument> Manifest = new(() =>
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory);
             dir is not null; dir = dir.Parent)
        {
            var file = Path.Combine(dir.FullName, "docs", "curriculum",
                "US_CCSS_ALG1_CODER5_COURSE_PROGRESS_20261028.json");
            if (File.Exists(file))
                return JsonDocument.Parse(File.ReadAllText(file));
        }
        throw new FileNotFoundException("Cannot locate Algebra 1 internal source provenance");
    });

    internal static void Verify(string lessonCode, IReadOnlyList<string> learnerParagraphs)
    {
        var lesson = Manifest.Value.RootElement.GetProperty("lessons")
            .EnumerateArray().Where(x =>
                string.Equals(x.GetProperty("lessonCode").GetString(),
                    lessonCode, StringComparison.Ordinal)).ToArray();
        Assert.Single(lesson);
        var source = lesson[0];
        var unit = source.GetProperty("unitNumber").GetInt32();
        var number = source.GetProperty("lessonNumber").GetInt32();
        var official = source.GetProperty("officialTeacherPreparationUrl").GetString();
        Assert.NotNull(official);
        Assert.EndsWith($"/1/{unit}/{number}/preparation.html", official, StringComparison.Ordinal);
        var digest = source.GetProperty("originalSourceSemanticSha256").GetString();
        Assert.True(digest?.Length == 64 && digest.All(Uri.IsHexDigit),
            $"Missing source semantic digest: {lessonCode}");
        var sidecar = source.GetProperty("authoredRichV2Sidecar").GetString();
        Assert.False(string.IsNullOrWhiteSpace(sidecar));
        Assert.NotEmpty(learnerParagraphs);
    }
}

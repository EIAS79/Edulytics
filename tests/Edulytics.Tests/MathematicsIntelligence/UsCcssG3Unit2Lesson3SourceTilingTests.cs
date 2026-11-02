using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 3 U02 L03 original 2021 source contracts. AUTHORING ONLY: NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit2Lesson3SourceTilingTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L03";
    private static string ReadSource(string relative)
    {
        DirectoryInfo? d = new(AppContext.BaseDirectory);
        while (d is not null)
        {
            var path = Path.Combine(d.FullName, relative.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(path)) return File.ReadAllText(path);
            d = d.Parent;
        }
        throw new FileNotFoundException("Missing authored source contract " + relative);
    }

    [Theory]
    [InlineData("G3-U02-L03-A1-01", "25", "24")]
    [InlineData("G3-U02-L03-A1-02", "15", "3")]
    [InlineData("G3-U02-L03-A1-03", "24", "7")]
    [InlineData("G3-U02-L03-A1-04", "12", "4")]
    public void SourceFourOriginalRectangleAreasHaveNumericAnchorsButRequireTeacherTiling(
        string id, string right, string wrong)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var original = lesson!.SourceSubtasks.Single(q => q.Id == id);
        Assert.Equal("integer", original.AnswerType);
        Assert.Equal(right, original.Expected);
        var correct = Grade3SourceCompanionCatalogue.Grade(original, right);
        var incorrect = Grade3SourceCompanionCatalogue.Grade(original, wrong);
        Assert.True(correct.NumericCorrect);
        Assert.False(incorrect.NumericCorrect);
        Assert.True(correct.TeacherReview); // Numeric result is NOT sufficient source activity evidence.
    }

    [Theory]
    [InlineData("G3-U02-L03-WU-A")]
    [InlineData("G3-U02-L03-WU-B")]
    [InlineData("G3-U02-L03-WU-C")]
    [InlineData("G3-U02-L03-WU-D")]
    [InlineData("G3-U02-L03-A1-RULE")]
    [InlineData("G3-U02-L03-A2-NOTICE")]
    [InlineData("G3-U02-L03-A2-EIGHT")]
    [InlineData("G3-U02-L03-A2-SORT")]
    [InlineData("G3-U02-L03-A2-CREATE")]
    [InlineData("G3-U02-L03-SYNTH")]
    public void AuthenticOpenSourceShapeTasksMustNeverBeMachinePassed(string id)
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        var source = lesson!.SourceSubtasks.Single(q => q.Id == id);
        Assert.Equal("teacher", source.AnswerType);
        foreach (var guess in new [] { "8", "12", "16", "24", "25", "A and C", "", "all" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(source, guess);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void Original2021CardsAndPrintMasterAreIndividuallySourceMapped()
    {
        using var doc = JsonDocument.Parse(ReadSource(
            "src/Edulytics.Web/Curriculum/Grade3/g3-unverified-original-companions.json"));
        var lesson = doc.RootElement.GetProperty("lessons").EnumerateArray().Single(
            x => x.GetProperty("ordinal").GetInt32() == 24);
        var source = lesson.GetProperty("sourceCardAreas").EnumerateArray()
            .Select(x => new { Card = x.GetProperty("card").GetString(), Area = x.GetProperty("area").GetInt32() })
            .ToDictionary(x => x.Card!, x => x.Area);
        Assert.Equal(6, source.Count);
        Assert.Equal(12, source["A"]); Assert.Equal(12, source["C"]);
        Assert.Equal(16, source["D"]); Assert.Equal(16, source["E"]);
        Assert.Equal(24, source["B"]); Assert.Equal(24, source["F"]);
        var original = lesson.GetProperty("source2021Original");
        Assert.Equal(6, original.GetProperty("studentPdfPagesRead").GetInt32());
        Assert.Equal(19, original.GetProperty("originalImageAssetsInspected").GetInt32());
        Assert.Equal(2, original.GetProperty("timeToTileMasterPages").GetInt32());
        Assert.Equal(1, original.GetProperty("cardSortMasterPages").GetInt32());
    }

    [Fact]
    public void CanonicalRichV2UsesGuardedEditionSpecificMaterializerAndIndependentGraphics()
    {
        var rich = ReadSource("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u02-l03-2021.rich-lesson-v2.json");
        var bridge = ReadSource("src/Edulytics.Data/Seeding/CommonCoreG3Unit2Lesson3RichV2.cs");
        var csproj = ReadSource("src/Edulytics.Data/Edulytics.Data.csproj");
        var correction = ReadSource("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var razor = ReadSource("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        Assert.Contains("g3-u02-l03-2021-source-abc-firstparty-20261024-v1", rich);
        Assert.Contains("g3-u02-l03-2021-source-abc-firstparty-20261024-v1", bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit2Lesson3RichV2", csproj);
        Assert.Contains("CommonCoreG3Unit2Lesson3RichV2.Apply(lesson)", correction);
        Assert.Contains("four different ways of tiling a rectangle", razor);
        Assert.Contains("GAPS", rich, StringComparison.OrdinalIgnoreCase);
    }
}

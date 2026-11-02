using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson5ShapeSetsTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L05";

    [Theory]
    [InlineData("quadrilateral", "quadrilateral")]
    [InlineData("triangle", "triangle")]
    [InlineData("quadrilateral|rhombus", "rhombus,QUADRILATERAL")]
    [InlineData("quadrilateral|rectangle", "rectangle, quadrilateral")]
    [InlineData("quadrilateral|rectangle|rhombus|square", "square,rectangle,rhombus,quadrilateral")]
    public void SourceShapeSetsRequireAllMathematicallyTrueClasses(string expected, string submitted)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(
            new Grade3OriginalQuestion { AnswerType = "shape-category-set", Expected = expected }, submitted);
        Assert.True(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Theory]
    [InlineData("quadrilateral|rectangle|rhombus|square", "square")]
    [InlineData("quadrilateral|rhombus", "quadrilateral")]
    [InlineData("quadrilateral|rectangle", "quadrilateral,rhombus")]
    [InlineData("triangle", "quadrilateral")]
    [InlineData("quadrilateral", "hexagon")]
    [InlineData("quadrilateral", "quadrilateral,quadrilateral")]
    [InlineData("quadrilateral", "quadrilateral,")]
    [InlineData("quadrilateral", "")]
    [InlineData("quadrilateral", "square;rhombus")]
    [InlineData("quadrilateral", "quadrilateral,free text")]
    public void IncompleteExtraneousDuplicateOrMalformedCategorySetsAreRejected(
        string expected, string submitted)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(
            new Grade3OriginalQuestion { AnswerType = "shape-category-set", Expected = expected }, submitted);
        Assert.False(result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void OriginalSixFiguresAndFourDivideBySevenFactsHaveDistinctQuestionContracts()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(14, lesson!.SourceSubtasks.Count);
        string[] sets = { "quadrilateral", "triangle", "quadrilateral",
            "quadrilateral|rhombus", "quadrilateral|rectangle",
            "quadrilateral|rectangle|rhombus|square" };
        for (var i = 0; i < 6; i++)
        {
            var q = lesson.SourceSubtasks[4 + i];
            Assert.Equal("shape-category-set", q.AnswerType);
            Assert.Equal(sets[i], q.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(
                q, q.Expected!.Replace("|", ",")).NumericCorrect);
        }

        string[] answers = { "10", "11", "9", "8" };
        for (var i = 0; i < 4; i++)
        {
            var q = lesson.SourceSubtasks[i];
            Assert.Equal("integer", q.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, answers[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, "99").NumericCorrect);
        }

        Assert.Equal(4, lesson.SourceSubtasks.Count(q => q.AnswerType == "teacher"));
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, "any number");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void FirstPartyRichV2ResourceAndEditionAreVersionPinned()
    {
        var asm = System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream = asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson5RichV2");
        Assert.NotNull(stream);
        using var doc = JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l05-2021-six-shape-inclusion-and-open-quadrilaterals-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons = doc.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        var entry = lessons[0];
        Assert.Equal(Code, entry.GetProperty("LessonCode").GetString());
        var explanation = string.Join(" ", entry.GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(v => v.GetString()));
        Assert.Contains("77 ÷ 7 = 11", explanation);
        Assert.Contains("ROTATED square", explanation);
        Assert.Contains("GATED", explanation, StringComparison.OrdinalIgnoreCase);
    }
}

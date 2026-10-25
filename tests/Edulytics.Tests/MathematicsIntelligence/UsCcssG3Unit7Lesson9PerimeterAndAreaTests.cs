using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson9PerimeterAndAreaTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L09";

    [Theory]
    [InlineData("8 ft|20 ft|20 ft", "20 ft, 8 ft, 20 feet", true)]
    [InlineData("8 ft|20 ft|20 ft", "8 ft, 20 ft, 20 ft", true)]
    [InlineData("8 ft|20 ft|20 ft", "20 ft", false)]
    [InlineData("8 ft|20 ft|20 ft", "8 ft, 20 ft, 8 ft", false)]
    [InlineData("8 ft|20 ft|20 ft", "8 ft, 20 cm, 20 ft", false)]
    [InlineData("12 ft|4 ft|4 ft", "4 ft,12 feet,4 ft", true)]
    [InlineData("12 ft|4 ft|4 ft", "4 ft,4 ft,4 ft", false)]
    [InlineData("12 ft|4 ft|4 ft", "12 ft,4 ft", false)]
    [InlineData("12 ft|4 ft|4 ft", "4 ft, 12 ft, 4 ft, 4 ft", false)]
    [InlineData("12 ft|4 ft|4 ft", "12 m, 4 ft, 4 ft", false)]
    public void MissingSidesRequireCompleteUnitAwareMultiset(string expected, string entered, bool result)
    {
        var q = new Grade3OriginalQuestion { AnswerType = "linear-length-multiset", Expected = expected };
        var grade = Grade3SourceCompanionCatalogue.Grade(q, entered);
        Assert.Equal(result, grade.NumericCorrect);
        Assert.True(grade.TeacherReview);
    }

    [Fact]
    public void AllTenOriginalSourceTasksRemainInOrderAndRespectAreaDistinction()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(10, lesson!.SourceSubtasks.Count);
        string[] ids = { "WARMUP", "A1-01", "A1-02", "A1-03",
            "A2-01", "A2-02", "A2-03", "A2-04", "A2-05", "SYNTHESIS" };
        for(var i = 0; i < ids.Length; i++)
            Assert.EndsWith(ids[i], lesson.SourceSubtasks[i].Id);

        var estimates = Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[0], "400");
        Assert.Null(estimates.NumericCorrect);
        Assert.True(estimates.TeacherReview);
        var walk = Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[7], "16");
        Assert.True(walk.NumericCorrect);
        Assert.True(walk.TeacherReview);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[7], "15").NumericCorrect);
        var area = Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[8], "80");
        Assert.True(area.NumericCorrect);
        Assert.True(area.TeacherReview);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[8], "36").NumericCorrect);
        Assert.Equal("teacher", lesson.SourceSubtasks[9].AnswerType);
    }

    [Fact]
    public void LinearPerimeterQuestionsNeverAllowWrongPhysicalUnit()
    {
        var q = new Grade3OriginalQuestion { AnswerType = "length-with-unit", Expected = "13 in" };
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q, "13 inches").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q, "13 cm").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q, "13").NumericCorrect);
    }

    [Fact]
    public void GuardedCanonicalMaterializerContainsSourceAndTeachingSeparation()
    {
        var assembly = System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream = assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson9RichV2");
        Assert.NotNull(stream);
        using var doc = JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l09-2021-missing-lengths-context-area-route-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons = doc.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        Assert.Equal(Code, lessons[0].GetProperty("LessonCode").GetString());
        var prose = string.Join(" ", lessons[0].GetProperty("ExplanationParagraphs").EnumerateArray()
            .Select(x => x.GetString()));
        Assert.Contains("16 blocks", prose);
        Assert.Contains("80 tiles", prose);
        Assert.Contains("not accessed", prose);
        Assert.Contains("AREA", prose);
    }
}

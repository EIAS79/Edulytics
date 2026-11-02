using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson6PerimeterUnitTests
{
    private const string LessonCode = "PED:US-CCSS-MATH:G3:U07:L06";

    [Fact]
    public void SourceBlacklinePaperclipCountsAndOriginalFivePerimetersAreCoveredInOrder()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(LessonCode, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(11, lesson!.SourceSubtasks.Count);
        Assert.Equal("teacher", lesson.SourceSubtasks[0].AnswerType);
        string[] paperclipAnswers = { "13", "9", "12", "14" };
        string[] perimeterAnswers = { "22", "26", "33", "32", "30" };
        for (var i = 0; i < 4; i++)
        {
            var question = lesson.SourceSubtasks[1 + i];
            Assert.Contains("PAPERCLIP", question.Id);
            Assert.Equal(paperclipAnswers[i], question.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(question, paperclipAnswers[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(question, "99").NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(question, paperclipAnswers[i]).TeacherReview);
        }
        for (var i = 0; i < 5; i++)
        {
            var question = lesson.SourceSubtasks[5 + i];
            Assert.Contains("PERIMETER", question.Id);
            Assert.Equal(perimeterAnswers[i], question.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(question, perimeterAnswers[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(question, (int.Parse(perimeterAnswers[i]) + 1).ToString()).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(question, perimeterAnswers[i]).TeacherReview);
        }
        Assert.Equal(2, lesson.SourceSubtasks.Count(x => x.AnswerType == "teacher"));
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[0], "10").NumericCorrect);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[10], "33").NumericCorrect);
    }

    [Fact]
    public void SourceSpecificTeachingDistinguishesUnlikeLengthUnitsAndCountingIntervals()
    {
        var asm = System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream = asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson6RichV2");
        Assert.NotNull(stream);
        using var doc = JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l06-2021-physical-paperclips-grid-tick-perimeter-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons = doc.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        Assert.Equal(LessonCode, lessons[0].GetProperty("LessonCode").GetString());
        var content = string.Join(" ", lessons[0].GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(p => p.GetString()));
        Assert.Contains("1¼-inch", content);
        Assert.Contains("80% scale", content);
        Assert.Contains("26 grid units", content);
        Assert.Contains("33 > D 32 > E 30", content);
        Assert.Contains("different physical unit lengths", content);
        Assert.Contains("not automatically transferable", content);
        Assert.Contains("restricted original Cool-down", content);
    }
}

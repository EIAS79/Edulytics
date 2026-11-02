using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson7EqualPerimeterTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L07";

    [Fact]
    public void WarmupAllFourOriginalTruthValuesAreStrictlyChecked()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        string[] correct = { "no", "yes", "no", "yes" };
        Assert.Equal(19, lesson!.SourceSubtasks.Count);
        for (var i = 0; i < correct.Length; i++)
        {
            var q = lesson.SourceSubtasks[i];
            Assert.Equal("yes-no", q.AnswerType);
            Assert.Equal(correct[i], q.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, correct[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,
                correct[i] == "yes" ? "no" : "yes").NumericCorrect);
        }
    }

    [Fact]
    public void AllTenFigurePerimetersMatchTheSourcePdfVectorGeometry()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        string[] letters = { "A","B","C","D","E","F","G","H","I","J" };
        int[] perimeters = { 18,16,10,10,16,22,18,18,10,22 };
        for (var i=0; i < letters.Length; i++)
        {
            var q=lesson!.SourceSubtasks[4+i];
            Assert.EndsWith(letters[i], q.Id);
            Assert.Equal(perimeters[i].ToString(), q.Expected);
            var correct=Grade3SourceCompanionCatalogue.Grade(q, perimeters[i].ToString());
            Assert.True(correct.NumericCorrect);
            Assert.True(correct.TeacherReview);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, (perimeters[i]+1).ToString()).NumericCorrect);
        }
        Assert.Equal(perimeters[0], perimeters[6]); // source A and G
        Assert.Equal(perimeters[6], perimeters[7]); // G and H
        Assert.Equal(perimeters[1], perimeters[4]); // B and E
        Assert.Equal(perimeters[2], perimeters[3]); // C and D
        Assert.Equal(perimeters[2], perimeters[8]); // C and I
        Assert.Equal(perimeters[5], perimeters[9]); // F and J
    }

    [Fact]
    public void DrawingTasksRequireTeacherAndDoNotPromoteNumericToApproval()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(5, lesson!.SourceSubtasks.Count(x=>x.AnswerType == "teacher"));
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            var feedback=Grade3SourceCompanionCatalogue.Grade(q, "48");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        }
        Assert.Contains("ANY THREE", lesson.TeacherReviewNote, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void EmbeddedRichV2SourceIsPinnedToFirstEditionAndAxisAlignedDrawings()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson7RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l07-2021-ten-source-figures-equal-perimeter-drawing-v1",
           doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons=doc.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        Assert.Equal(Code, lessons[0].GetProperty("LessonCode").GetString());
        var text=string.Join(" ",lessons[0].GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()));
        Assert.Contains("FALSE; regrouping", text);
        Assert.Contains("F 22", text);
        Assert.Contains("diagonal", text);
        Assert.Contains("FOUR", text, StringComparison.OrdinalIgnoreCase);
    }
}

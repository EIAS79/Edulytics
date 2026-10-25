using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson10OriginalInfoGapTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L10";

    [Fact]
    public void OriginalFourDivisionTrueFalseRelationshipsAreTruthful()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(9, lesson!.SourceSubtasks.Count);
        var expected = new[] {"yes","yes","no","no"};
        for (var i = 0; i < expected.Length; i++)
        {
            var q=lesson.SourceSubtasks[i];
            Assert.Equal("yes-no",q.AnswerType);
            Assert.Equal(expected[i],q.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i]).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,expected[i]=="yes"?"no":"yes").NumericCorrect);
        }
    }

    [Fact]
    public void RopePerimeterIsLinearAndNumericGradeStillNeedsTeacherArgument()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        var q=lesson!.SourceSubtasks[4];
        Assert.Equal("length-with-unit",q.AnswerType);
        Assert.Equal("76 ft",q.Expected);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"76 feet").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"240 ft").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"76 cm").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"76 ft").TeacherReview);
    }

    [Fact]
    public void HiddenPublisherCardDataIsNotAutoGradedOrRevealedInStudentTasks()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        foreach (var q in lesson!.SourceSubtasks.Where(s => s.Id.Contains("INFO-GAP",StringComparison.Ordinal)))
        {
            Assert.Equal("teacher",q.AnswerType);
            Assert.Null(q.Expected);
            Assert.DoesNotContain("48 square",q.Prompt,StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("38 meters",q.Prompt,StringComparison.OrdinalIgnoreCase);
            var grade=Grade3SourceCompanionCatalogue.Grade(q,"28");
            Assert.Null(grade.NumericCorrect);
            Assert.True(grade.TeacherReview);
        }
    }

    [Fact]
    public void MaterializerUsesSpecificSourceWithNoCopiedOriginalInfoGapAnswers()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson10RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l10-2021-perimeter-area-original-card-info-gap-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lessons=doc.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        Assert.Equal(Code,lessons[0].GetProperty("LessonCode").GetString());
        var text=string.Join(" ",lessons[0].GetProperty("ExplanationParagraphs").EnumerateArray().Select(s=>s.GetString()));
        Assert.Contains("76",text);
        Assert.Contains("not accessed",text,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("information",text,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("84 square metres",text,StringComparison.OrdinalIgnoreCase);
    }
}

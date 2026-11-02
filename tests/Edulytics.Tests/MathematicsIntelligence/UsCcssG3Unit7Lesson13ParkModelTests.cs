using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson13ParkModelTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U07:L13";
    [Fact]
    public void SourceParkPerimeterAndAreaHaveDistinctQuantities()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(7,lesson!.SourceSubtasks.Count);
        Assert.Equal("40",lesson.SourceSubtasks[2].Expected);
        Assert.Equal("84",lesson.SourceSubtasks[3].Expected);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[2],"40").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[2],"84").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[3],"84").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.SourceSubtasks[3],"40").NumericCorrect);
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            var g=Grade3SourceCompanionCatalogue.Grade(q,"40");
            Assert.Null(g.NumericCorrect);
            Assert.True(g.TeacherReview);
        }
        Assert.Equal(5,lesson.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
    }
    [Fact]
    public void OriginalParkDrawingRequiresFiveFeaturesAndThreeDualMeasures()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson13RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l13-2021-park-design-five-features-two-measures-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lesson=doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code,lesson.GetProperty("LessonCode").GetString());
        var paragraphs=string.Join(" ",lesson.GetProperty("ExplanationParagraphs").EnumerateArray()
            .Select(x=>x.GetString()));
        Assert.Contains("FIVE",paragraphs);
        Assert.Contains("THREE",paragraphs);
        Assert.Contains("48 SQUARE YARDS",paragraphs);
        Assert.Contains("40 YARDS",paragraphs);
        Assert.Contains("not reproduced",paragraphs);
    }
}

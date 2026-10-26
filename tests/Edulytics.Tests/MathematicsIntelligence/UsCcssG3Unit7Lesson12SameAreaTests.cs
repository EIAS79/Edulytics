using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson12SameAreaTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U07:L12";

    [Theory]
    [InlineData("12","2,6;3,4",true)]
    [InlineData("12","3,4;4,3",false)]
    [InlineData("20","2,10;4,5",true)]
    [InlineData("20","2,10;10,2",false)]
    [InlineData("42","3,14;6,7",true)]
    [InlineData("42","3,14;6,8",false)]
    [InlineData("48","4,12;6,8",true)]
    [InlineData("48","2,24;6,8",true)]
    [InlineData("48","1,48;6,8",false)]
    [InlineData("48","4,12;12,4",false)]
    [InlineData("48","0,12;6,8",false)]
    [InlineData("48","4,12",false)]
    public void NumericPairRequiresEqualAreaAndUnequalPerimeterOnSourceGrid(
        string area, string answer, bool correct)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="rectangle-pair-area-different-perimeter",
            Expected=area
        };
        var result=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(correct,result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void AllSourceExerciseGroupsArePresentWithTeacherBoundary()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(13,lesson!.SourceSubtasks.Count);
        Assert.Equal(new[]{"10","15","20","24"},
            lesson.SourceSubtasks.Take(4).Select(q=>q.Expected).ToArray());
        Assert.Equal("teacher",lesson.SourceSubtasks[4].AnswerType);
        Assert.Equal(new[]{"12","20","42","48"},
            lesson.SourceSubtasks.Skip(5).Take(4).Select(q=>q.Expected).ToArray());
        Assert.Equal(5,lesson.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));
        foreach(var q in lesson.SourceSubtasks.Where(q=>q.AnswerType=="teacher"))
        {
            var v=Grade3SourceCompanionCatalogue.Grade(q,"28");
            Assert.Null(v.NumericCorrect);
            Assert.True(v.TeacherReview);
        }
    }

    [Fact]
    public void EditionSpecificRichV2KeepsOriginalArea24AndFourTargetAreas()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson12RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l12-2021-equal-area-different-perimeter-source-drawing-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        Assert.Equal(Code,doc.RootElement.GetProperty("Lessons")[0].GetProperty("LessonCode").GetString());
        var text=string.Join(" ",doc.RootElement.GetProperty("Lessons")[0]
            .GetProperty("ExplanationParagraphs").EnumerateArray().Select(p=>p.GetString()));
        Assert.Contains("1×24",text);
        Assert.Contains("12, 20, 42 and 48",text);
        Assert.Contains("19×24",text);
        Assert.Contains("teacher-gated",text);
    }
}

using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson11RectanglePairsTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U07:L11";

    [Theory]
    [InlineData("12","1,5;2,4",true)]
    [InlineData("12","3,3;2,4",true)]
    [InlineData("20","2,8;4,6",true)]
    [InlineData("26","3,10;5,8",true)]
    [InlineData("34","7,10;8,9",true)]
    [InlineData("34","10,7;9,8",true)]
    [InlineData("12","1,5;5,1",false)]
    [InlineData("20","2,8;8,2",false)]
    [InlineData("20","2,8;4,7",false)]
    [InlineData("26","3,10;3,10",false)]
    [InlineData("34","0,17;8,9",false)]
    [InlineData("34","1,16;1,16",false)]
    [InlineData("20","2,8",false)]
    [InlineData("20","2,8;4,6;1,9",false)]
    [InlineData("34","19,1;8,9",false)]
    public void SourcePairRequiresSameTargetPerimeterAndDifferentArea(
        string target, string submitted, bool correct)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="rectangle-pair-perimeter-different-area",
            Expected=target
        };
        var f=Grade3SourceCompanionCatalogue.Grade(q,submitted);
        Assert.Equal(correct,f.NumericCorrect);
        Assert.True(f.TeacherReview);
    }

    [Fact]
    public void SourceContainsFourMentalMathAndOpenConstructionPosterTasks()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var l));
        Assert.NotNull(l);
        Assert.Equal(13,l!.SourceSubtasks.Count);
        string[] facts={"25","50","10","17"};
        for (var i=0; i<4; i++)
        {
            var q=l.SourceSubtasks[i];
            Assert.Equal(facts[i],q.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,facts[i]).NumericCorrect);
        }
        Assert.Equal("teacher",l.SourceSubtasks[4].AnswerType);
        Assert.Equal(new[] { "12","20","26","34" },
            l.SourceSubtasks.Skip(5).Take(4).Select(q=>q.Expected).ToArray());
        Assert.Equal(5,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));
        foreach(var q in l.SourceSubtasks.Where(q=>q.AnswerType=="teacher"))
        {
            var f=Grade3SourceCompanionCatalogue.Grade(q,"10");
            Assert.Null(f.NumericCorrect);
            Assert.True(f.TeacherReview);
        }
    }

    [Fact]
    public void RichV2PreservesSourcePrintedGridConstraintAndLessonGoals()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson11RichV2");
        Assert.NotNull(stream);
        using var doc=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l11-2021-two-rectangles-same-perimeter-distinct-areas-v1",
            doc.RootElement.GetProperty("ContentVersion").GetString());
        var lesson=doc.RootElement.GetProperty("Lessons")[0];
        Assert.Equal(Code,lesson.GetProperty("LessonCode").GetString());
        var explanations=string.Join(" ",lesson.GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(p=>p.GetString()));
        Assert.Contains("19×24",explanations);
        Assert.Contains("12, 20, 26, 34",explanations);
        Assert.Contains("cut-outs",explanations);
        Assert.Contains("teacher-gated",explanations);
    }
}

using System;
using System.Linq;
using System.Text.Json;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG3Unit7Lesson8SourcePerimetersAndLinearUnitTests
{
    private const string LessonCode="PED:US-CCSS-MATH:G3:U07:L08";

    [Fact]
    public void OriginalMentalDivisionFourCasesRemainSourceExact()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(LessonCode,out var lesson));
        Assert.NotNull(lesson);
        int[] expected={10,9,5,6};
        for(var i=0;i<expected.Length;i++)
        {
            var q=lesson!.SourceSubtasks[i];
            Assert.Equal("integer",q.AnswerType);
            Assert.Equal(expected[i].ToString(),q.Expected);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q,expected[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"99").NumericCorrect);
        }
        Assert.Equal(15,lesson!.SourceSubtasks.Count);
    }

    [Theory]
    [InlineData("80 in","80 in",true)]
    [InlineData("80 in","80 inches",true)]
    [InlineData("80 in","80 inch",true)]
    [InlineData("80 in","80 cm",false)]
    [InlineData("80 in","80 ft",false)]
    [InlineData("80 in","80",false)]
    [InlineData("32 ft","32 feet",true)]
    [InlineData("32 ft","32 inches",false)]
    [InlineData("36 cm","36 centimeters",true)]
    [InlineData("36 cm","36 square cm",false)]
    [InlineData("240 in","240 inches",true)]
    [InlineData("240 in","240 centimetres",false)]
    public void SourcePerimetersAreComparedWithExactLinearUnits(
        string expected, string submitted, bool isCorrect)
    {
        var q=new Grade3OriginalQuestion { AnswerType="length-with-unit",Expected=expected };
        var feedback=Grade3SourceCompanionCatalogue.Grade(q,submitted);
        Assert.Equal(isCorrect,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }

    [Fact]
    public void StudentSectionEightPointOneAndEightPointTwoPreserveOriginalUnitsAndMissingEdge()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(LessonCode,out var lesson));
        Assert.NotNull(lesson);
        var one=lesson!.SourceSubtasks.Skip(5).Take(5).ToArray();
        string[] expectedOne={"30 cm","80 in","36 cm","32 ft","36 cm"};
        Assert.Equal(expectedOne,one.Select(x=>x.Expected).ToArray());
        for(var i=0;i<5;i++)
        {
            Assert.Equal("length-with-unit",one[i].AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(one[i],expectedOne[i]).NumericCorrect);
        }
        var two=lesson.SourceSubtasks.Skip(10).Take(3).ToArray();
        string[] expectedTwo={"60 cm","640","240 in"};
        Assert.Equal(expectedTwo,two.Select(x=>x.Expected).ToArray());
        Assert.Equal("integer",two[1].AnswerType); // No unit in source.
        Assert.True(Grade3SourceCompanionCatalogue.Grade(two[1],"640").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(two[1],"440").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(two[1],"480").NumericCorrect);
        foreach(var q in lesson.SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            var feedback=Grade3SourceCompanionCatalogue.Grade(q,"28");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        }
    }

    [Fact]
    public void GuardedSourceSpecificRichV2PreservesOriginalDiagramAndGallery()
    {
        var asm=System.Reflection.Assembly.Load("Edulytics.Data");
        using var stream=asm.GetManifestResourceStream("Edulytics.Data.Grade3.Unit7Lesson8RichV2");
        Assert.NotNull(stream);
        using var document=JsonDocument.Parse(stream!);
        Assert.Equal("g3-u07-l08-2021-eight-source-perimeters-side-symmetry-and-unknown-v1",
            document.RootElement.GetProperty("ContentVersion").GetString());
        var lessons=document.RootElement.GetProperty("Lessons");
        Assert.Single(lessons.EnumerateArray());
        Assert.Equal(LessonCode,lessons[0].GetProperty("LessonCode").GetString());
        var combined=string.Join(" ",lessons[0].GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(q=>q.GetString()));
        Assert.Contains("ELEVEN short",combined);
        Assert.Contains("5×40 = 200",combined);
        Assert.Contains("640",combined);
        Assert.Contains("80 in",combined);
        Assert.Contains("gated",combined,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not accessed",combined,StringComparison.OrdinalIgnoreCase);
    }
}

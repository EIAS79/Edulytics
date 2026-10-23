using Edulytics.Core.Mathematics.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring-only checks; do not execute before Stage D.</summary>
public sealed class UsCcssG3Unit1Lesson3ScaledPictureGraphTests
{
    private const string Code = UsCcssG3Unit1Lesson3ScaledPictureGraph.LessonCode;
    [Fact]
    public void OriginalPictographRepresentationsUseIndependentScales()
    {
        var s = UsCcssG3Unit1Lesson3ScaledPictureGraph.Sports;
        var f = UsCcssG3Unit1Lesson3ScaledPictureGraph.Flowers;
        Assert.Equal(5, s.StudentsPerSymbol);
        Assert.Equal(2, f.StudentsPerSymbol);
        Assert.Equal(45, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(s));
        Assert.Equal(24, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(f));
        Assert.Equal(20, UsCcssG3Unit1Lesson3ScaledPictureGraph.CategoryCount(s, "Basketball"));
        Assert.Equal(10, UsCcssG3Unit1Lesson3ScaledPictureGraph.CategoryCount(f, "Daisies"));
    }
    [Theory]
    [InlineData("SPORTS-TOTAL",45)]
    [InlineData("SPORTS-BASKETBALL",20)]
    [InlineData("SPORTS-COMPARE",10)]
    [InlineData("FLOWERS-ROSES",8)]
    [InlineData("FLOWERS-TOTAL",24)]
    [InlineData("FLOWERS-COMPARE",4)]
    [InlineData("COOL-NATURE",20)]
    [InlineData("COOL-DIFFERENCE",10)]
    public void NumberQuestionsAcceptCorrectAndRejectIncorrect(string id,int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(Code,id,
            expected,out var correct));
        Assert.True(correct);
        Assert.True(UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(Code,id,
            expected-1,out var incorrect));
        Assert.False(incorrect);
    }
    [Fact]
    public void NoCrossLessonOrTeacherReviewedNumericCertification()
    {
        Assert.False(UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(
            "PED:US-CCSS-MATH:G3:U01:L02","COOL-NATURE",20,out _));
        Assert.False(UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(
            Code,"COOL-WRITE",0,out _));
        Assert.True(UsCcssG3Unit1Lesson3ScaledPictureGraph.RequiresTeacherReview(
            "invent-scaled-picture-question"));
        Assert.True(UsCcssG3Unit1Lesson3ScaledPictureGraph.RequiresTeacherReview(
            "original-cool-down-explanation"));
    }
}

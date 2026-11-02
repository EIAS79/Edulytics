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
        var a = UsCcssG3Unit1Lesson3ScaledPictureGraph.SportsUnscaled;
        var s = UsCcssG3Unit1Lesson3ScaledPictureGraph.Sports;
        var f = UsCcssG3Unit1Lesson3ScaledPictureGraph.Flowers;
        var b = UsCcssG3Unit1Lesson3ScaledPictureGraph.Books;
        Assert.Equal(1, a.StudentsPerSymbol);
        Assert.Equal(5, s.StudentsPerSymbol);
        Assert.Equal(5, f.StudentsPerSymbol);
        Assert.Equal(2, b.StudentsPerSymbol);
        Assert.Equal(65, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(a));
        Assert.Equal(65, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(s));
        Assert.Equal(55, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(f));
        Assert.Equal(24, UsCcssG3Unit1Lesson3ScaledPictureGraph.Total(b));
        foreach (var category in new[] { "Football", "Soccer", "Basketball", "Tennis" })
            Assert.Equal(UsCcssG3Unit1Lesson3ScaledPictureGraph.CategoryCount(a, category),
                UsCcssG3Unit1Lesson3ScaledPictureGraph.CategoryCount(s, category));
        Assert.Equal(25, UsCcssG3Unit1Lesson3ScaledPictureGraph.CategoryCount(f, "Daisies"));
    }
    [Theory]
    [InlineData("SPORTS-TOTAL",65)]
    [InlineData("SPORTS-BASKETBALL",20)]
    [InlineData("SPORTS-COMPARE",15)]
    [InlineData("FLOWERS-ROSES",10)]
    [InlineData("FLOWERS-TULIPS",5)]
    [InlineData("FLOWERS-DAISIES",25)]
    [InlineData("FLOWERS-TOTAL",55)]
    [InlineData("FLOWERS-COMPARE",20)]
    [InlineData("FLOWERS-VIOLETS",15)]
    [InlineData("BOOKS-JOKES",8)]
    [InlineData("BOOKS-COMICS",2)]
    [InlineData("BOOKS-SCIENCE",8)]
    [InlineData("BOOKS-MYSTERY",6)]
    [InlineData("BOOKS-TOTAL",24)]
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
    public void OriginalBookGraphHowDoYouKnowHasSeparateHumanOnlyResponse()
    {
        // This is an authored-only Stage C source contract, not a run or a pass.
        Assert.NotNull(typeof(Edulytics.Web.ViewModels.LessonContent.Grade3Lesson3Answers)
            .GetProperty("BooksHowYouKnow"));
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? viewFile = null;
        while (dir is not null)
        {
            var p = Path.Combine(dir.FullName, "src", "Edulytics.Web", "Views",
                "Grade3Lesson3Activities", "Index.cshtml");
            if (File.Exists(p)) { viewFile = p; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(viewFile);
        var razor = File.ReadAllText(viewFile!);
        Assert.Contains("name=\"BooksHowYouKnow\"", razor);
        Assert.Contains("How do you know the number", razor);
        Assert.Contains("reviewed by your teacher", razor);
        Assert.False(UsCcssG3Unit1Lesson3ScaledPictureGraph.TryGradeNumeric(
            Code, "BOOKS-HOW-YOU-KNOW", 24, out _));
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
    [Fact]
    public void Original2021BirdsCoolDownKeyAndCountsAreSourceGrounded()
    {
        var rows = UsCcssG3Unit1Lesson3SourceCoolDown.Values;
        Assert.Equal(new[] { "Cardinals", "Blue Jays", "Pigeons", "Sparrows" },
            rows.Select(x => x.Category));
        Assert.Equal(new[] { 2, 3, 4, 5 }, rows.Select(x => x.Symbols));
        Assert.Equal(2, UsCcssG3Unit1Lesson3SourceCoolDown.BirdsPerSymbol);
        Assert.Equal(4, UsCcssG3Unit1Lesson3SourceCoolDown.Count("Cardinals"));
        Assert.Equal(6, UsCcssG3Unit1Lesson3SourceCoolDown.Count("Blue Jays"));
        Assert.Equal(8, UsCcssG3Unit1Lesson3SourceCoolDown.Count("Pigeons"));
        Assert.Equal(10, UsCcssG3Unit1Lesson3SourceCoolDown.Count("Sparrows"));
        Assert.Equal(28, UsCcssG3Unit1Lesson3SourceCoolDown.Total);
    }

    [Fact]
    public void OriginalBirdCoolDownUsesSeparateNumericAndHumanQuestionFields()
    {
        var answers = typeof(Edulytics.Web.ViewModels.LessonContent.Grade3Lesson3Answers);
        Assert.NotNull(answers.GetProperty("SourceCoolDownSparrows"));
        Assert.NotNull(answers.GetProperty("SourceCoolDownBirdQuestion"));
    }

}

using System;
using System.IO;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class G2Unit9RenderedIndependentPromptsTests
{
    private static string ReadUnit9View()
    {
        for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir is not null; dir = dir.Parent)
        {
            var candidate = Path.Combine(dir.FullName, "src", "Edulytics.Web", "Views",
                "Grade2Unit9Activities", "Index.cshtml");
            if (File.Exists(candidate))
                return File.ReadAllText(candidate);
        }
        throw new FileNotFoundException("Grade2 Unit9 learner view missing from test checkout");
    }

    [Fact]
    public void EveryLearnerQuestionUsesUnit9SourceOfTruthNotUnit8()
    {
        var view = ReadUnit9View();
        Assert.DoesNotContain("UsCcssG2Unit8OriginalActivities.IndependentNumericExamples", view);
        for (var slot = 0; slot < 4; slot++)
        {
            var expected = $"UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[Model.LessonNumber][{slot}]";
            Assert.Contains(expected, view);
        }
    }

    [Fact]
    public void StudentCannotSelfAttestTeacherObservations()
    {
        var view = ReadUnit9View();
        Assert.DoesNotContain("asp-for=\"Answers.TeacherComments\"", view);
        Assert.Contains("recorded by an authenticated teacher", view);
    }

    [Theory]
    [InlineData(1, "6", "15", "7", "15")]
    [InlineData(13, "39", "49", "59", "89")]
    public void FirstAndLastUnit9GradingKeysAreIndependentOfUnit8(
        int lesson, string a, string b, string c, string d)
    {
        Assert.Equal(4, UsCcssG2Unit9OriginalActivities.IndependentNumericExamples[lesson].Length);
        var accepted = UsCcssG2Unit9OriginalActivities.NewMath(lesson, a, b, c, d);
        Assert.True(accepted.Correct);
        Assert.False(accepted.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit9OriginalActivities.NewMath(lesson, "-1", b, c, d).Correct);
    }
}
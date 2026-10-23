using Edulytics.Core.Mathematics.Curriculum;
using Edulytics.Web.ViewModels.LessonContent;
using Edulytics.Web.Controllers;
using System.Reflection;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C authored regression candidates for official 2021 G3 U01 L01
/// numeric anchors only; these tests intentionally remain UNRUN until Stage D.
/// They are not source-specific classroom-task academic approval.
/// </summary>
public sealed class UsCcssG3Unit1Lesson1GraphCandidateTests
{
    [Theory]
    [InlineData("Walk", 4)]
    [InlineData("Bike", 3)]
    [InlineData("Skateboard", 1)]
    [InlineData("Car", 2)]
    [InlineData("Bus", 7)]
    [InlineData("Train", 6)]
    public void OriginalSixGraphCategoryCountsMustStayExact(string label, int number)
    {
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGetCategoryCount(label, out var count));
        Assert.Equal(number, count);
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-count", label, null, number, out var correct));
        Assert.True(correct);
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-count", label, null, number + 1, out var wrong));
        Assert.False(wrong);
    }

    [Fact]
    public void PictographBarGraphValuesAgreeAndTotalIs23()
    {
        Assert.Equal(6, UsCcssG3Unit1Lesson1GraphCandidate.Values.Count);
        Assert.Equal(23, UsCcssG3Unit1Lesson1GraphCandidate.TotalStudents);
        Assert.Equal(1, UsCcssG3Unit1Lesson1GraphCandidate.PictographStudentsPerSymbol);
        Assert.Equal(0, UsCcssG3Unit1Lesson1GraphCandidate.BarYAxisMinimum);
        Assert.Equal(8, UsCcssG3Unit1Lesson1GraphCandidate.BarYAxisMaximum);
        Assert.Equal(1, UsCcssG3Unit1Lesson1GraphCandidate.BarYAxisStep);
        Assert.Equal("Ways to get home", UsCcssG3Unit1Lesson1GraphCandidate.BarXAxisTitle);
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "all-category-total", null, null, 23, out var correct));
        Assert.True(correct);
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "all-category-total", null, null, 22, out var wrong));
        Assert.False(wrong);
    }

    [Theory]
    [InlineData("Bus", "Skateboard", 6)]
    [InlineData("Bus", "Walk", 3)]
    [InlineData("Train", "Car", 4)]
    public void ValidComparisonsUseAbsoluteDifference(string first, string second, int value)
    {
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-difference", first, second, value, out var result));
        Assert.True(result);
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-difference", first, second, value + 1, out var wrong));
        Assert.False(wrong);
    }

    [Theory]
    [InlineData("Walk", "Bike")]
    [InlineData("unknown", null)]
    [InlineData("", null)]
    public void InvalidSingleCountRequestsAreNeverMarkedCorrect(string first, string? second)
    {
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-count", first, second, 4, out var result));
        Assert.False(result);
    }

    [Theory]
    [InlineData("notice-and-wonder")]
    [InlineData("infer-unlabeled-graph-categories")]
    [InlineData("explain-graph-similarities")]
    [InlineData("explain-graph-learnings")]
    [InlineData("author-two-graph-questions")]
    [InlineData("partner-discussion")]
    [InlineData("math-community-norms")]
    [InlineData("oral-synthesis")]
    [InlineData("original-cool-down")]
    public void HumanSourceTasksMayNotBeNumericallyAutoGraded(string task)
    {
        Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.RequiresTeacherJudgment(task));
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.IsAutomaticallyGradeable(task));
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            task, "Walk", null, 4, out var wrongCertification));
        Assert.False(wrongCertification);
    }

    [Fact]
    public void UnsupportedInputDoesNotSilentlyReceiveNumericGrade()
    {
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "all-category-total", "Bus", null, 23, out _));
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "named-category-difference", "Bus", "unknown", 7, out _));
        Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
            "unsupported-task", "Bus", null, 7, out _));
    }
    [Fact]
    public void SequentialPictureGraphMustPreserveBottomAlignedSixColumnsWithTheSameTwentyThreeStudents()
    {
        var values = UsCcssG3Unit1Lesson1GraphCandidate.Values;
        Assert.Equal(new[] { 4, 3, 1, 2, 7, 6 },
            values.Select(v => v.NumberOfStudents));
        Assert.Equal(23, values.Sum(v => v.NumberOfStudents));
        Assert.All(values, row => Assert.InRange(
            row.NumberOfStudents, 0, UsCcssG3Unit1Lesson1GraphCandidate.BarYAxisMaximum));
        Assert.Equal(1, UsCcssG3Unit1Lesson1GraphCandidate.PictographStudentsPerSymbol);
    }

    [Fact]
    public void EveryOriginalStudentFacingSubpartKeepsItsOwnLearnerField()
    {
        // Original lesson 2021: warm-up, picture time, and three distinct
        // Activity 2 subparts. Written only; Stage D tests not run.
        var names = new[]
        {
            nameof(Grade3Lesson1Answers.WarmupObservations),
            nameof(Grade3Lesson1Answers.PlausibleCategories),
            nameof(Grade3Lesson1Answers.GraphComparison),
            nameof(Grade3Lesson1Answers.GraphLearnings),
            nameof(Grade3Lesson1Answers.TwoGraphQuestions)
        };
        Assert.Equal(5, names.Distinct().Count());
        foreach (var field in names)
            Assert.NotNull(typeof(Grade3Lesson1Answers).GetProperty(field));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MissingOriginalHumanResponseNeverCountsAsCompletedOrPassed(string? response)
    {
        var result = Grade3Lesson1TeacherReviewPolicy.AssessForReview(
            "Source activity 2b", response, "Explain a supported claim");
        Assert.Null(result.MachineCorrect);
        Assert.True(result.TeacherReviewRequired);
        Assert.Contains("No written response was submitted", result.Reason);
        Assert.Contains("teacher observation", result.Reason);
    }

    [Fact]
    public void WrittenOriginalHumanResponseIsStillUnscoredUntilTeacherReview()
    {
        var result = Grade3Lesson1TeacherReviewPolicy.AssessForReview(
            "Source activity 2c", "How many travel by bus? How many by train?",
            "Check that two different questions are answerable");
        Assert.Null(result.MachineCorrect);
        Assert.True(result.TeacherReviewRequired);
        Assert.Contains("NOT an automatic pass", result.Reason);
        Assert.Contains("two different questions", result.Reason);
    }

    [Fact]
    public void LearnerViewKeepsOfficial2021PedagogicalOrderAndDistinctHumanTasks()
    {
        // Authored for Stage D only, intentionally NOT RUN during A-B-C.
        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        string? viewPath = null;
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src",
                "Edulytics.Web", "Views", "Grade3Lesson1Activities", "Index.cshtml");
            if (File.Exists(candidate))
            {
                viewPath = candidate;
                break;
            }
            directory = directory.Parent;
        }
        Assert.NotNull(viewPath);
        var view = File.ReadAllText(viewPath!);

        var warmup = view.IndexOf("1. Notice and wonder", StringComparison.Ordinal);
        var picture = view.IndexOf("2. Picture Time", StringComparison.Ordinal);
        var activity = view.IndexOf("3. Two graphs", StringComparison.Ordinal);
        var synthesis = view.IndexOf("4. Lesson synthesis", StringComparison.Ordinal);
        var cooldown = view.IndexOf("5. Edulytics-authored Cool-down", StringComparison.Ordinal);
        Assert.True(warmup >= 0 && warmup < picture &&
                    picture < activity && activity < synthesis && synthesis < cooldown);

        foreach (var distinctField in new[]
        {
            "WarmupObservations", "PlausibleCategories", "GraphComparison",
            "GraphLearnings", "TwoGraphQuestions"
        })
        {
            Assert.Contains("name=\"" + distinctField + "\"", view);
        }

        Assert.Contains("Additional Edulytics numeric practice", view);
        Assert.Contains("teacher observation", view);
        Assert.Contains("Sort and Display", view);
        Assert.Contains("Capture Squares", view);
        Assert.Contains("Horizontal axis: Ways to get home", view);
        Assert.Contains("bar-x-axis-title", view);
        Assert.DoesNotContain("src=\"https://cms-assets.illustrativemathematics.org/", view);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SkippingOptionalNumericPracticeNeverMarksTheAnswerWrong(string? blank)
    {
        // Stage D regression WRITTEN but intentionally NOT RUN.
        var method = typeof(Grade3Lesson1ActivitiesController)
            .GetMethod("CheckSource", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        var feedback = (Grade3Lesson1Feedback)method!.Invoke(null,
            new object?[] { "Bus count", "named-category-count", "Bus", null, blank })!;
        Assert.Null(feedback.MachineCorrect);
        Assert.False(feedback.TeacherReviewRequired);
        Assert.Contains("was not attempted", feedback.Reason);
    }

    [Fact]
    public void SkippingIndependentCooldownDoesNotCreatePublisherAssessmentScore()
    {
        var method = typeof(Grade3Lesson1ActivitiesController)
            .GetMethod("CheckCoolDown", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        var feedback = (Grade3Lesson1Feedback)method!.Invoke(null,
            new object?[] { "Optional first-party assessment",
                "G3-U01-L01-EDU-CD-COUNT", "" })!;
        Assert.Null(feedback.MachineCorrect);
        Assert.False(feedback.TeacherReviewRequired);
        Assert.Contains("No original publisher Cool-down score", feedback.Reason);
    }

    [Fact]
    public void MalformedAttemptedSupplementalNumberStillRejectsBadInput()
    {
        var method = typeof(Grade3Lesson1ActivitiesController)
            .GetMethod("CheckSource", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(method);
        var feedback = (Grade3Lesson1Feedback)method!.Invoke(null,
            new object?[] { "Bus count", "named-category-count", "Bus", null, "not-a-number" })!;
        Assert.False(feedback.MachineCorrect);
        Assert.False(feedback.TeacherReviewRequired);
    }

    [Fact]
    public void UnlabelledSourceGraphNeverAssignsAnAutomaticallyCertifiedCategory()
    {
        foreach(var mode in new [] { "notice-and-wonder", "infer-unlabeled-graph-categories",
                                     "oral-synthesis", "math-community-norms" })
        {
            Assert.True(UsCcssG3Unit1Lesson1GraphCandidate.RequiresTeacherJudgment(mode));
            Assert.False(UsCcssG3Unit1Lesson1GraphCandidate.TryGradeNumeric(
                mode, "Bus", null, 7, out _));
        }
    }

}

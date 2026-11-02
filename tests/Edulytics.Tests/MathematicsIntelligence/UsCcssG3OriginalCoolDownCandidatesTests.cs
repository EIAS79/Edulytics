using Edulytics.Core.Mathematics.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// These first-party Cool-down contracts are WRITTEN but NOT EXECUTED.
/// Testing will take place in the separate Stage D, after A–B–C closure.
/// </summary>
public sealed class UsCcssG3OriginalCoolDownCandidatesTests
{
    [Fact]
    public void FirstPartyCooldownUsesIndependentGraphNotCopiedTravelDataset()
    {
        Assert.Equal(4, UsCcssG3OriginalCoolDownCandidates.Lesson1LibraryGraph.Count);
        Assert.Equal(17, UsCcssG3OriginalCoolDownCandidates.Lesson1LibraryGraph.Sum(x => x.Students));
        Assert.Equal(1, UsCcssG3Unit1Lesson1GraphCandidate.PictographStudentsPerSymbol);
        Assert.DoesNotContain(UsCcssG3OriginalCoolDownCandidates.Lesson1LibraryGraph, x =>
            x.Category.Equals("Bus", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("G3-U01-L01-EDU-CD-COUNT", 5)]
    [InlineData("G3-U01-L01-EDU-CD-COMPARE", 4)]
    public void OriginalLesson1NumericQuestionsAcceptCorrectRejectIncorrect(string id, int answer)
    {
        Assert.True(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson1Code, id, answer, out var right));
        Assert.True(right);
        Assert.True(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson1Code, id, answer + 1, out var wrong));
        Assert.False(wrong);
    }

    [Theory]
    [InlineData("G3-U01-L02-EDU-CD-ONE-STEP", 5)]
    [InlineData("G3-U01-L02-EDU-CD-TWO-STEP", 6)]
    public void OriginalLesson2NumericsPreserveOneAndTwoStepProblemScope(string id, int answer)
    {
        Assert.Equal(16, UsCcssG3OriginalCoolDownCandidates.Lesson2ClubGraph.Sum(x => x.Students));
        Assert.True(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson2Code, id, answer, out var right));
        Assert.True(right);
        Assert.True(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson2Code, id, answer - 1, out var wrong));
        Assert.False(wrong);
    }

    [Theory]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L01", "G3-U01-L01-EDU-CD-EXPLAIN")]
    [InlineData("PED:US-CCSS-MATH:G3:U01:L02", "G3-U01-L02-EDU-CD-DRAW-EXPLAIN")]
    public void AuthoredExplanationAndDrawingTasksRequireTeacher(
        string lessonCode, string id)
    {
        Assert.True(UsCcssG3OriginalCoolDownCandidates.IsTeacherReviewed(lessonCode, id));
        Assert.False(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            lessonCode, id, 0, out var falselyAccepted));
        Assert.False(falselyAccepted);
    }

    [Fact]
    public void RejectWrongLessonWrongIdOrCrossLessonGrading()
    {
        Assert.False(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            "UNKNOWN", "G3-U01-L01-EDU-CD-COUNT", 5, out _));
        Assert.False(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson1Code,
            "G3-U01-L02-EDU-CD-ONE-STEP", 5, out _));
        Assert.False(UsCcssG3OriginalCoolDownCandidates.TryGradeNumeric(
            UsCcssG3OriginalCoolDownCandidates.Lesson2Code,
            "NONEXISTENT", 6, out _));
    }
}

using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C unit tests written but NEVER executed before Stage D.
/// Artificial data here are test fixtures, NOT alleged classroom results.
/// </summary>
public sealed class UsCcssG2Unit1SeventhLessonClassDataTests
{
    [Fact]
    public void FixtureSurveyCanBeReorganizedWithoutDroppingAResponse()
    {
        var fixture=UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses(
            "walk, car, bus, train, walk, car, bus");
        Assert.True(fixture.Accepted);
        Assert.Equal(7,fixture.OriginalResponses.Count);
        Assert.Equal(2,fixture.Counts["Walk"]);
        Assert.Equal(2,fixture.Counts["Car"]);
        Assert.Equal(1,fixture.Counts["Train"]);
        Assert.Equal(2,fixture.Counts["School bus"]);
        Assert.True(UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(fixture,"2","2","1","2").Correct);
        Assert.False(UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(fixture,"2","3","1","1").Correct);
        Assert.True(UsCcssG2Unit1SeventhLessonClassData
            .VerifyOriginalCoolDown(fixture,"7","2","1").Correct);
    }

    [Theory]
    [InlineData("walk, car, Alex")]
    [InlineData("walk, airplane")]
    [InlineData("just testing")]
    [InlineData("")]
    public void NamesOrUnknownClassesCannotBePassedOffAsOriginalSurvey(string raw)
    {
        var result=UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses(raw);
        Assert.False(result.Accepted);
    }

    [Fact]
    public void EmptyCategoriesRemainZeroAndMultipleTransportSpellingsAreAccepted()
    {
        var fixture=UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses(
            "walking; school bus; on foot; Bus");
        Assert.True(fixture.Accepted);
        Assert.Equal(2,fixture.Counts["Walk"]);
        Assert.Equal(2,fixture.Counts["School bus"]);
        Assert.Equal(0,fixture.Counts["Car"]);
        Assert.True(UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(fixture,"2","0","0","2").Correct);
    }

    [Fact]
    public void ReadableOrganizedDisplayMustHaveSameTotalAsUnorganizedResponses()
    {
        var fixture=UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses(
            "walk, train, walk");
        Assert.True(fixture.Accepted);
        Assert.False(UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(fixture,"3","0","1","0").Correct);
        Assert.False(UsCcssG2Unit1SeventhLessonClassData
            .VerifyOriginalCoolDown(fixture,"4","0","1").Correct);
    }

    [Fact]
    public void OriginalLearnerDrawingAndDiscussionRequireTeacherReview()
    {
        Assert.Equal(5,UsCcssG2Unit1SeventhLessonClassData.TeacherRubric.Count);
        var fixture=UsCcssG2Unit1SeventhLessonClassData.ParseActualResponses("car");
        var result=UsCcssG2Unit1SeventhLessonClassData
            .VerifyFourCategoryRepresentation(fixture,"0","1","0","0");
        Assert.True(result.Correct);
        Assert.True(result.TeacherReviewNeeded);
    }
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 3 2021 optional model-and-graph source; authored tests NOT RUN.</summary>
public sealed class UsCcssG3Unit1Lesson21SeatingPlanningTests
{
    [Fact]
    public void AllAuthenticPlanningTasksRemainTeacherReviewedNotUniqueNumeric()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(
            "PED:US-CCSS-MATH:G3:U01:L21",out var lesson));
        Assert.NotNull(lesson);
        Assert.False(lesson.ReadyForStageD);
        Assert.Equal(5,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q=>
        {
            Assert.Equal("teacher",q.AnswerType);
            var f=Grade3SourceCompanionCatalogue.Grade(q,"16");
            Assert.Null(f.NumericCorrect);
            Assert.True(f.TeacherReview);
        });
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Contains("16",lesson.SourceSubtasks.Single(q=>q.Id.Contains("ASSUMPTIONS")).Prompt);
        Assert.Contains("2, 4, 5 or 10",lesson.SourceSubtasks.Single(q=>q.Id.Contains("ASSUMPTIONS")).Prompt);
        Assert.Contains("GREATER than 1",lesson.SourceSubtasks.Single(q=>q.Id=="G3-U01-L21-A2-01").Prompt);
    }
}

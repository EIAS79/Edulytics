using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 Student Facing group-count arithmetic; tests authored, not run.</summary>
public sealed class UsCcssG3Unit4Lesson1HowManyGroupsSourceTests
{
    private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L01",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
    [Theory]
    [InlineData("G3-U04-L01-A1-1", "3", true)]
 [InlineData("G3-U04-L01-A1-1", "4", false)]
 [InlineData("G3-U04-L01-A1-2", "7", true)]
 [InlineData("G3-U04-L01-A1-2", "8", false)]
 [InlineData("G3-U04-L01-A1-3", "8", true)]
 [InlineData("G3-U04-L01-A1-3", "9", false)]
    public void OriginalGroupCountAnswers(string id,string answer,bool expected){var task=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(task,answer);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
    [Fact]
    public void DiagramsAndPublisherWarmupRemainHumanReviewed(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(3,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(6,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"3");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

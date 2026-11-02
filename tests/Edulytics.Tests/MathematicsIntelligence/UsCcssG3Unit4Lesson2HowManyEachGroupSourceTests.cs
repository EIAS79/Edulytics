using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original first-edition 2021 equal-share apple division checks, written but not run.</summary>
public sealed class UsCcssG3Unit4Lesson2HowManyEachGroupSourceTests
{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L02",out var item));return Assert.IsType<Grade3CompanionLesson>(item);}
 [Theory]
 [InlineData("G3-U04-L02-A1-1","5",true)]
    [InlineData("G3-U04-L02-A1-1","6",false)]
    [InlineData("G3-U04-L02-A1-2","6",true)]
    [InlineData("G3-U04-L02-A1-2","7",false)]
    [InlineData("G3-U04-L02-A1-3","5",true)]
    [InlineData("G3-U04-L02-A1-3","6",false)]
    [InlineData("G3-U04-L02-A3-PRIOR-COMPARISON","3",true)]
    [InlineData("G3-U04-L02-A3-PRIOR-COMPARISON","4",false)]
 public void SourceDivisionAnswersAreStrict(string id,string answer,bool correct){var task=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(task,answer);Assert.Equal(correct,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]
 public void OrchardAndGalleryStillRequireTeacher(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(4,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(7,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"5");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

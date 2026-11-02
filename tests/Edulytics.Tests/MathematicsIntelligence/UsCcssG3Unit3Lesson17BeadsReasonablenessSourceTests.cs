using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Archived original 2021 school beads arithmetic tests, authored and not run.</summary>
public sealed class UsCcssG3Unit3Lesson17BeadsReasonablenessSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L17",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L17-WU-1","247",true)]
 [InlineData("G3-U03-L17-WU-1","248",false)]
 [InlineData("G3-U03-L17-WU-2","423",true)]
 [InlineData("G3-U03-L17-WU-2","424",false)]
 [InlineData("G3-U03-L17-WU-3","368",true)]
 [InlineData("G3-U03-L17-WU-3","369",false)]
 [InlineData("G3-U03-L17-PRIYA","422",true)]
 [InlineData("G3-U03-L17-PRIYA","423",false)]
 [InlineData("G3-U03-L17-CLARE","363",true)]
 [InlineData("G3-U03-L17-CLARE","364",false)]
 [InlineData("G3-U03-L17-HAN","610",true)]
 [InlineData("G3-U03-L17-HAN","611",false)]
 [InlineData("G3-U03-L17-JADA","210",true)]
 [InlineData("G3-U03-L17-JADA","211",false)]
 [InlineData("G3-U03-L17-NOAH","804",true)]
 [InlineData("G3-U03-L17-NOAH","805",false)]
 public void OriginalSourceExactBeadCheck(string id,string answer,bool expected){var task=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(task,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void IndependentTeacherEstimationNeverAutoGraded(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(8,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(9,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"400");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

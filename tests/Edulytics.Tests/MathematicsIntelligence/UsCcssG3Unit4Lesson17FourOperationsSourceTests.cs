using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Four operations source numeric checks and teacher-only open questions. Authored unrun.</summary>
public sealed class UsCcssG3Unit4Lesson17FourOperationsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L17",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L17-WU-2X40","80",true)]
    [InlineData("G3-U04-L17-WU-2X40","81",false)]
    [InlineData("G3-U04-L17-WU-3X50","150",true)]
    [InlineData("G3-U04-L17-WU-3X50","151",false)]
    [InlineData("G3-U04-L17-WU-3X40","120",true)]
    [InlineData("G3-U04-L17-WU-3X40","121",false)]
    [InlineData("G3-U04-L17-WU-7X10","70",true)]
    [InlineData("G3-U04-L17-WU-7X10","71",false)]
    [InlineData("G3-U04-L17-A1-ROOM-B","48",true)]
    [InlineData("G3-U04-L17-A1-ROOM-B","49",false)]
    [InlineData("G3-U04-L17-A1-EACH-TABLE","8",true)]
    [InlineData("G3-U04-L17-A1-EACH-TABLE","9",false)]
    [InlineData("G3-U04-L17-A1-SILVERWARE","568",true)]
    [InlineData("G3-U04-L17-A1-SILVERWARE","569",false)]
    [InlineData("G3-U04-L17-A1-PLATES","142",true)]
    [InlineData("G3-U04-L17-A1-PLATES","143",false)]
    [InlineData("G3-U04-L17-A2-PAPER-RINGS","183",true)]
    [InlineData("G3-U04-L17-A2-PAPER-RINGS","184",false)]
    [InlineData("G3-U04-L17-A2-MUFFINS","8",true)]
    [InlineData("G3-U04-L17-A2-MUFFINS","9",false)]
    [InlineData("G3-U04-L17-A2-CUPS","160",true)]
    [InlineData("G3-U04-L17-A2-CUPS","161",false)]
    [InlineData("G3-U04-L17-SUM-4X30","120",true)]
    [InlineData("G3-U04-L17-SUM-4X30","121",false)]
    [InlineData("G3-U04-L17-SUM-7X13","91",true)]
    [InlineData("G3-U04-L17-SUM-7X13","92",false)]
    [InlineData("G3-U04-L17-SUM-3X28","84",true)]
    [InlineData("G3-U04-L17-SUM-3X28","85",false)]
public void OriginalTwoStepArithmetic(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void OpenQuestionsAndUnknownLettersRemainTeacherReviewed(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(14,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"8");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

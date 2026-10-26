using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Written Grade3 L64 source regressions, not executed.</summary>
public sealed class UsCcssG3Unit4Lesson7MultiplicationDivisionSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L07",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L07-WU-SIX-TENS","60",true)]
    [InlineData("G3-U04-L07-WU-SIX-TENS","61",false)]
    [InlineData("G3-U04-L07-A2-KIRAN","8",true)]
    [InlineData("G3-U04-L07-A2-KIRAN","9",false)]
    [InlineData("G3-U04-L07-A2-BOOKS","7",true)]
    [InlineData("G3-U04-L07-A2-BOOKS","8",false)]
    [InlineData("G3-U04-L07-A2-ERASERS","48",true)]
    [InlineData("G3-U04-L07-A2-ERASERS","49",false)]
    [InlineData("G3-U04-L07-A2-LIN","6",true)]
    [InlineData("G3-U04-L07-A2-LIN","7",false)]
    [InlineData("G3-U04-L07-SYN-24","6",true)]
    [InlineData("G3-U04-L07-SYN-24","7",false)]
    [InlineData("G3-U04-L07-SYN-28","7",true)]
    [InlineData("G3-U04-L07-SYN-28","8",false)]
public void NumericSourceOracles(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
[Fact]
public void RoundTableRequiresHumanReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(7,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(15,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));for(int i=1;i<=4;i++){var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L07-A1-BOX"+i);var r=Grade3SourceCompanionCatalogue.Grade(q,"60");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

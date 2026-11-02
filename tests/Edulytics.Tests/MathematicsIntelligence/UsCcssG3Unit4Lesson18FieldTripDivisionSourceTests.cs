using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original larger quotients and derived inverse checks; tests authored not executed.</summary>
public sealed class UsCcssG3Unit4Lesson18FieldTripDivisionSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L18",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L18-A1-GROUPS","12",true)]
    [InlineData("G3-U04-L18-A1-GROUPS","13",false)]
    [InlineData("G3-U04-L18-A1-4X12","48",true)]
    [InlineData("G3-U04-L18-A1-4X12","49",false)]
    [InlineData("G3-U04-L18-A1-40DIV4","10",true)]
    [InlineData("G3-U04-L18-A1-40DIV4","11",false)]
    [InlineData("G3-U04-L18-A1-8DIV4","2",true)]
    [InlineData("G3-U04-L18-A1-8DIV4","3",false)]
    [InlineData("G3-U04-L18-A2-BUS","24",true)]
    [InlineData("G3-U04-L18-A2-BUS","25",false)]
    [InlineData("G3-U04-L18-A2-3X24","72",true)]
    [InlineData("G3-U04-L18-A2-3X24","73",false)]
    [InlineData("G3-U04-L18-A2-TABLES","6",true)]
    [InlineData("G3-U04-L18-A2-TABLES","7",false)]
    [InlineData("G3-U04-L18-A2-12X6","72",true)]
    [InlineData("G3-U04-L18-A2-12X6","73",false)]
    [InlineData("G3-U04-L18-A2-60DIV3","20",true)]
    [InlineData("G3-U04-L18-A2-60DIV3","21",false)]
    [InlineData("G3-U04-L18-A2-12DIV3","4",true)]
    [InlineData("G3-U04-L18-A2-12DIV3","5",false)]
public void LargeQuotientArithmetic(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void TeacherChecksUnknownGroupRole(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(10,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"12");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

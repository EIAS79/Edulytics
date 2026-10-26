using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original source 2021 L76 arithmetic and group-role tests, not run.</summary>
public sealed class UsCcssG3Unit4Lesson19LargerDivisionTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L19",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L19-WARM1","1",true)]
    [InlineData("G3-U04-L19-WARM1","2",false)]
    [InlineData("G3-U04-L19-WARM2","1",true)]
    [InlineData("G3-U04-L19-WARM2","2",false)]
    [InlineData("G3-U04-L19-WARM3","0",true)]
    [InlineData("G3-U04-L19-WARM3","1",false)]
    [InlineData("G3-U04-L19-WARM4","1",true)]
    [InlineData("G3-U04-L19-WARM4","2",false)]
    [InlineData("G3-U04-L19-A1-LAUNCH39","13",true)]
    [InlineData("G3-U04-L19-A1-LAUNCH39","14",false)]
    [InlineData("G3-U04-L19-A1-55","11",true)]
    [InlineData("G3-U04-L19-A1-55","12",false)]
    [InlineData("G3-U04-L19-A1-45","15",true)]
    [InlineData("G3-U04-L19-A1-45","16",false)]
    [InlineData("G3-U04-L19-A1-63","21",true)]
    [InlineData("G3-U04-L19-A1-63","22",false)]
    [InlineData("G3-U04-L19-A1-84","12",true)]
    [InlineData("G3-U04-L19-A1-84","13",false)]
    [InlineData("G3-U04-L19-A1-100","20",true)]
    [InlineData("G3-U04-L19-A1-100","21",false)]
    [InlineData("G3-U04-L19-A2-60","12",true)]
    [InlineData("G3-U04-L19-A2-60","13",false)]
    [InlineData("G3-U04-L19-A2-64","16",true)]
    [InlineData("G3-U04-L19-A2-64","17",false)]
    [InlineData("G3-U04-L19-A2-72","12",true)]
    [InlineData("G3-U04-L19-A2-72","13",false)]
    [InlineData("G3-U04-L19-A2-75","5",true)]
    [InlineData("G3-U04-L19-A2-75","6",false)]
    [InlineData("G3-U04-L19-SYNTHESIS-96","12",true)]
    [InlineData("G3-U04-L19-SYNTHESIS-96","13",false)]
public void MathFromSource(string id,string response,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,response);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void GroupingSourceTasksAreHumanReviewed(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(15,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"12");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source L77 78÷3 distinct strategies and optional Compare tests. Authored but unrun.</summary>
public sealed class UsCcssG3Unit4Lesson20StrategiesSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L20",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L20-WU-1","15",true)]
    [InlineData("G3-U04-L20-WU-1","16",false)]
    [InlineData("G3-U04-L20-WU-2","30",true)]
    [InlineData("G3-U04-L20-WU-2","31",false)]
    [InlineData("G3-U04-L20-WU-3","50",true)]
    [InlineData("G3-U04-L20-WU-3","51",false)]
    [InlineData("G3-U04-L20-WU-4","13",true)]
    [InlineData("G3-U04-L20-WU-4","14",false)]
    [InlineData("G3-U04-L20-A1-78","26",true)]
    [InlineData("G3-U04-L20-A1-78","27",false)]
    [InlineData("G3-U04-L20-A2-80","16",true)]
    [InlineData("G3-U04-L20-A2-80","17",false)]
    [InlineData("G3-U04-L20-A2-68","17",true)]
    [InlineData("G3-U04-L20-A2-68","18",false)]
    [InlineData("G3-U04-L20-A2-91","13",true)]
    [InlineData("G3-U04-L20-A2-91","14",false)]
    [InlineData("G3-U04-L20-A2-84-OPTIONAL","6",true)]
    [InlineData("G3-U04-L20-A2-84-OPTIONAL","7",false)]
    [InlineData("G3-U04-L20-A3-COMPARE1","23",true)]
    [InlineData("G3-U04-L20-A3-COMPARE1","24",false)]
    [InlineData("G3-U04-L20-A3-COMPARE2","24",true)]
    [InlineData("G3-U04-L20-A3-COMPARE2","25",false)]
public void SourceQuotientOracles(string id,string input,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(q,input);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
[Fact]
public void TeacherReviewsThreeStrategies(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(11,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U04-L20-A3-PLAY"&&x.AnswerType=="teacher");}
}

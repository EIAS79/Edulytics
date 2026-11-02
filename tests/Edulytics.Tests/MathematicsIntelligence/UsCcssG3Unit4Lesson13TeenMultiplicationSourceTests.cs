using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 original teen products, contextual units and open gallery walk. Authored not run.</summary>
public sealed class UsCcssG3Unit4Lesson13TeenMultiplicationSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L13",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L13-WARM-4X18","72",true)]
    [InlineData("G3-U04-L13-WARM-4X18","73",false)]
    [InlineData("G3-U04-L13-A1-EGGS","84",true)]
    [InlineData("G3-U04-L13-A1-EGGS","85",false)]
    [InlineData("G3-U04-L13-A1-CHAIRS","75",true)]
    [InlineData("G3-U04-L13-A1-CHAIRS","76",false)]
    [InlineData("G3-U04-L13-A1-TABLE","64",true)]
    [InlineData("G3-U04-L13-A1-TABLE","65",false)]
    [InlineData("G3-U04-L13-FIRSTPARTY-4X10","40",true)]
    [InlineData("G3-U04-L13-FIRSTPARTY-4X10","41",false)]
    [InlineData("G3-U04-L13-FIRSTPARTY-4X8","32",true)]
    [InlineData("G3-U04-L13-FIRSTPARTY-4X8","33",false)]
    [InlineData("G3-U04-L13-FIRSTPARTY-5X10","50",true)]
    [InlineData("G3-U04-L13-FIRSTPARTY-5X10","51",false)]
    [InlineData("G3-U04-L13-FIRSTPARTY-5X5","25",true)]
    [InlineData("G3-U04-L13-FIRSTPARTY-5X5","26",false)]
public void SourceTeenProductOracles(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void GalleryWalkEstimationRequireTeacherReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(8,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"75");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original dollar and tens values from source first edition 2021; authored but not run.</summary>
public sealed class UsCcssG3Unit4Lesson12MultiplesOfTenSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L12",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L12-WU-TWELVE-TENS","120",true)]
    [InlineData("G3-U04-L12-WU-TWELVE-TENS","121",false)]
    [InlineData("G3-U04-L12-A2-24-TENS","240",true)]
    [InlineData("G3-U04-L12-A2-24-TENS","241",false)]
    [InlineData("G3-U04-L12-A1-BILLS-ONE100","100",true)]
    [InlineData("G3-U04-L12-A1-BILLS-ONE100","101",false)]
    [InlineData("G3-U04-L12-A1-BILLS-FOUR20","80",true)]
    [InlineData("G3-U04-L12-A1-BILLS-FOUR20","81",false)]
    [InlineData("G3-U04-L12-A1-BILLS-TEN10","100",true)]
    [InlineData("G3-U04-L12-A1-BILLS-TEN10","101",false)]
    [InlineData("G3-U04-L12-A1-BILLS-TEN5","50",true)]
    [InlineData("G3-U04-L12-A1-BILLS-TEN5","51",false)]
    [InlineData("G3-U04-L12-A1-BILLS-FIVE20","100",true)]
    [InlineData("G3-U04-L12-A1-BILLS-FIVE20","101",false)]
    [InlineData("G3-U04-L12-A1-BILLS-TWENTY10","200",true)]
    [InlineData("G3-U04-L12-A1-BILLS-TWENTY10","201",false)]
    [InlineData("G3-U04-L12-A1-BILLS-TWENTY5","100",true)]
    [InlineData("G3-U04-L12-A1-BILLS-TWENTY5","101",false)]
    [InlineData("G3-U04-L12-A1-BILLS-TWO50","100",true)]
    [InlineData("G3-U04-L12-A1-BILLS-TWO50","101",false)]
    [InlineData("G3-U04-L12-A1-END-ANDRE","140",true)]
    [InlineData("G3-U04-L12-A1-END-ANDRE","141",false)]
    [InlineData("G3-U04-L12-A1-END-CLARE","140",true)]
    [InlineData("G3-U04-L12-A1-END-CLARE","141",false)]
    [InlineData("G3-U04-L12-A1-END-JADA","250",true)]
    [InlineData("G3-U04-L12-A1-END-JADA","251",false)]
    [InlineData("G3-U04-L12-A1-END-LIN","160",true)]
    [InlineData("G3-U04-L12-A1-END-LIN","161",false)]
    [InlineData("G3-U04-L12-A1-END-NOAH","300",true)]
    [InlineData("G3-U04-L12-A1-END-NOAH","301",false)]
    [InlineData("G3-U04-L12-A1-END-TYLER","210",true)]
    [InlineData("G3-U04-L12-A1-END-TYLER","211",false)]
    [InlineData("G3-U04-L12-A2-5X60","300",true)]
    [InlineData("G3-U04-L12-A2-5X60","301",false)]
    [InlineData("G3-U04-L12-A2-8X50","400",true)]
    [InlineData("G3-U04-L12-A2-8X50","401",false)]
    [InlineData("G3-U04-L12-A2-4X30","120",true)]
    [InlineData("G3-U04-L12-A2-4X30","121",false)]
    [InlineData("G3-U04-L12-A2-7X40","280",true)]
    [InlineData("G3-U04-L12-A2-7X40","281",false)]
    [InlineData("G3-U04-L12-A2-9X20","180",true)]
    [InlineData("G3-U04-L12-A2-9X20","181",false)]
public void DollarAndPlaceValueNumericOracles(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void OriginalMoneyCombinationsNeedExplanatoryRubric(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(21,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(11,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"300");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

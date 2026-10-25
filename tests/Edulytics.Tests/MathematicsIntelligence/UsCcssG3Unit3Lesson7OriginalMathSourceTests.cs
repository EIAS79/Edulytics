using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 HTML number-and-open-task authoring regression: written, never run.</summary>
public sealed class UsCcssG3Unit3Lesson7OriginalMathSourceTests
{
  private const string Code="PED:US-CCSS-MATH:G3:U03:L07";
  private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
  [Theory]
  [InlineData("G3-U03-L07-S1-NUM1","40",true)]
  [InlineData("G3-U03-L07-S1-NUM1","41",false)]
  [InlineData("G3-U03-L07-S1-NUM2","48",true)]
  [InlineData("G3-U03-L07-S1-NUM2","49",false)]
  [InlineData("G3-U03-L07-S1-NUM3","238",true)]
  [InlineData("G3-U03-L07-S1-NUM3","239",false)]
  [InlineData("G3-U03-L07-S1-NUM4","234",true)]
  [InlineData("G3-U03-L07-S1-NUM4","235",false)]
  [InlineData("G3-U03-L07-S2-NUM5","215",true)]
  [InlineData("G3-U03-L07-S2-NUM5","216",false)]
  [InlineData("G3-U03-L07-S2-NUM6","107",true)]
  [InlineData("G3-U03-L07-S2-NUM6","108",false)]
  [InlineData("G3-U03-L07-S2-NUM7","267",true)]
  [InlineData("G3-U03-L07-S2-NUM7","268",false)]
  [InlineData("G3-U03-L07-S3-NUM8","127",true)]
  [InlineData("G3-U03-L07-S3-NUM8","128",false)]
  [InlineData("G3-U03-L07-S3-NUM9","71",true)]
  [InlineData("G3-U03-L07-S3-NUM9","72",false)]
  [InlineData("G3-U03-L07-S3-NUM10","120",true)]
  [InlineData("G3-U03-L07-S3-NUM10","121",false)]
  public void ExactOriginalArithmeticChecks(string id,string value,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,value);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
  [Fact] public void TeacherOnlyTasksAreNotAutoCertified(){Assert.True(Lesson().ReadyForStageD);Assert.True(Lesson().OriginalStudentTaskPdfRead);foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"127");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

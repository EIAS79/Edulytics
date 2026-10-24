using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 HTML number-and-open-task authoring regression: written, never run.</summary>
public sealed class UsCcssG3Unit3Lesson8OriginalMathSourceTests
{
  private const string Code="PED:US-CCSS-MATH:G3:U03:L08";
  private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
  [Theory]
  [InlineData("G3-U03-L08-S1-NUM1","2",true)]
  [InlineData("G3-U03-L08-S1-NUM1","3",false)]
  [InlineData("G3-U03-L08-S1-NUM2","2",true)]
  [InlineData("G3-U03-L08-S1-NUM2","3",false)]
  [InlineData("G3-U03-L08-S1-NUM3","102",true)]
  [InlineData("G3-U03-L08-S1-NUM3","103",false)]
  [InlineData("G3-U03-L08-S1-NUM4","106",true)]
  [InlineData("G3-U03-L08-S1-NUM4","107",false)]
  [InlineData("G3-U03-L08-S2-NUM5","176",true)]
  [InlineData("G3-U03-L08-S2-NUM5","177",false)]
  public void ExactOriginalArithmeticChecks(string id,string value,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,value);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
  [Fact] public void TeacherOnlyTasksAreNotAutoCertified(){Assert.False(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"127");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

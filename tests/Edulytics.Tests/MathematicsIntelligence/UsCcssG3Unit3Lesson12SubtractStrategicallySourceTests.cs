using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G3 U03 L12 source-specific numeric and human grading rules, NOT RUN.</summary>
public sealed class UsCcssG3Unit3Lesson12SubtractStrategicallySourceTests
{
private const string Code="PED:US-CCSS-MATH:G3:U03:L12";
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
[Theory]
[InlineData("G3-U03-L12-WU-1","12",true)]
[InlineData("G3-U03-L12-WU-1","13",false)]
[InlineData("G3-U03-L12-WU-2","18",true)]
[InlineData("G3-U03-L12-WU-2","19",false)]
[InlineData("G3-U03-L12-WU-3","14",true)]
[InlineData("G3-U03-L12-WU-3","15",false)]
[InlineData("G3-U03-L12-WU-4","21",true)]
[InlineData("G3-U03-L12-WU-4","22",false)]
[InlineData("G3-U03-L12-A1-1","122",true)]
[InlineData("G3-U03-L12-A1-1","123",false)]
[InlineData("G3-U03-L12-A1-2","192",true)]
[InlineData("G3-U03-L12-A1-2","193",false)]
[InlineData("G3-U03-L12-A1-3","639",true)]
[InlineData("G3-U03-L12-A1-3","640",false)]
[InlineData("G3-U03-L12-A1-4","122",true)]
[InlineData("G3-U03-L12-A1-4","123",false)]
[InlineData("G3-U03-L12-A1-5","88",true)]
[InlineData("G3-U03-L12-A1-5","89",false)]
[InlineData("G3-U03-L12-GAME-EXAMPLE","203",true)]
[InlineData("G3-U03-L12-GAME-EXAMPLE","204",false)]
public void SourceMathChecks(string id,string answer,bool good){var q=Lesson().SourceSubtasks.Single(t=>t.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(good,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]public void SourceGameAndPartnerExplanationsRequireTeacher(){Assert.True(Lesson().OriginalStudentTaskPdfRead);Assert.True(Lesson().ReadyForStageD);foreach(var q in Lesson().SourceSubtasks.Where(t=>t.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"203");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

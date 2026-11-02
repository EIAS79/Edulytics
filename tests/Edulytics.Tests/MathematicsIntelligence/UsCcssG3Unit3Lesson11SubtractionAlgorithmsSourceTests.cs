using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G3 U03 L11 source-semantic regression authored but not run.</summary>
public sealed class UsCcssG3Unit3Lesson11SubtractionAlgorithmsSourceTests
{
private const string Code="PED:US-CCSS-MATH:G3:U03:L11";
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
[Theory]
[InlineData("G3-U03-L11-WU-1","200",true)]
[InlineData("G3-U03-L11-WU-1","201",false)]
[InlineData("G3-U03-L11-WU-2","250",true)]
[InlineData("G3-U03-L11-WU-2","251",false)]
[InlineData("G3-U03-L11-WU-3","245",true)]
[InlineData("G3-U03-L11-WU-3","246",false)]
[InlineData("G3-U03-L11-WU-4","235",true)]
[InlineData("G3-U03-L11-WU-4","236",false)]
[InlineData("G3-U03-L11-A1-DIFF","283",true)]
[InlineData("G3-U03-L11-A1-DIFF","284",false)]
[InlineData("G3-U03-L11-A2-DIFF","134",true)]
[InlineData("G3-U03-L11-A2-DIFF","135",false)]
public void SourceNumericAnchors(string id,string answer,bool correct){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var grade=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(correct,grade.NumericCorrect);Assert.True(grade.TeacherReview);}
[Fact] public void TeacherComparisonsAreNotAutoCertified(){Assert.True(Lesson().OriginalStudentTaskPdfRead);Assert.True(Lesson().ReadyForStageD);foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"283");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

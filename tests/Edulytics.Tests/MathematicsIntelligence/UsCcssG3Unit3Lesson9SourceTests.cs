using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade3 2021 original L09 academic authoring tests, WRITTEN NOT RUN.</summary>
public sealed class UsCcssG3Unit3Lesson9SourceTests
{
 const string Code="PED:US-CCSS-MATH:G3:U03:L09";
 static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
 [Theory]
 [InlineData("LIN-RESULT","272",true)]
 [InlineData("LIN-RESULT","273",false)]
 [InlineData("TRY-1","124",true)]
 [InlineData("TRY-1","125",false)]
 [InlineData("TRY-2","233",true)]
 [InlineData("TRY-2","234",false)]
 [InlineData("TRY-3","174",true)]
 [InlineData("TRY-3","175",false)]
 [InlineData("TRY-4","463",true)]
 [InlineData("TRY-4","464",false)]
 [InlineData("TRY-5","257",true)]
 [InlineData("TRY-5","258",false)]
 public void OriginalSourceExactSubtractionAndWrongAnswersFail(string suffix,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id.EndsWith(suffix,StringComparison.Ordinal));var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
 [Fact] public void Original2021PdfIsReadAndPublisherPromptNotAutoGraded(){Assert.True(Lesson().ReadyForStageD);Assert.True(Lesson().OriginalStudentTaskPdfRead);Assert.Equal("418",Lesson().OriginalEdulyticsCoolDown!.Expected);Assert.Contains("704",Lesson().OriginalEdulyticsCoolDown.Prompt);foreach(var t in Lesson().SourceSubtasks.Where(t=>t.AnswerType=="teacher"))Assert.Null(Grade3SourceCompanionCatalogue.Grade(t,"272").NumericCorrect);}
}

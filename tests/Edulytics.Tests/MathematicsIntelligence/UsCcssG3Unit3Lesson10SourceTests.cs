using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 G3 U03 L10 numeric and teacher source; written not executed.</summary>
public sealed class UsCcssG3Unit3Lesson10SourceTests
{
 const string Code="PED:US-CCSS-MATH:G3:U03:L10";
 static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("A1-SUM","257",true)]
 [InlineData("A1-SUM","258",false)]
 [InlineData("A2-DEMO","382",true)]
 [InlineData("A2-DEMO","383",false)]
 [InlineData("A2-1","333",true)]
 [InlineData("A2-1","334",false)]
 [InlineData("A2-2","342",true)]
 [InlineData("A2-2","343",false)]
 [InlineData("A2-3","135",true)]
 [InlineData("A2-3","136",false)]
 [InlineData("A2-4","167",true)]
 [InlineData("A2-4","168",false)]
 public void ExactOriginalSourceSums(string tail,string response,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id.EndsWith(tail,StringComparison.Ordinal));var r=Grade3SourceCompanionCatalogue.Grade(q,response);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
 [Fact]public void SourcePdfAndFirstPartyExitAreDistinct(){Assert.True(Lesson().ReadyForStageD);Assert.True(Lesson().OriginalStudentTaskPdfRead);Assert.Equal("547",Lesson().OriginalEdulyticsCoolDown!.Expected);Assert.Contains("804",Lesson().OriginalEdulyticsCoolDown.Prompt);foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher"))Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"257").NumericCorrect);}
}

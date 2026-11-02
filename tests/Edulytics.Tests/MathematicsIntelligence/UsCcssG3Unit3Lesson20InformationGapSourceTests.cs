using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original source number-talk checks plus explicitly independent first-party info-gap tasks; unrun.</summary>
public sealed class UsCcssG3Unit3Lesson20InformationGapSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L20",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L20-WU-1","26",true)]
 [InlineData("G3-U03-L20-WU-1","27",false)]
 [InlineData("G3-U03-L20-WU-2","42",true)]
 [InlineData("G3-U03-L20-WU-2","43",false)]
 [InlineData("G3-U03-L20-WU-3","74",true)]
 [InlineData("G3-U03-L20-WU-3","75",false)]
 [InlineData("G3-U03-L20-WU-4","123",true)]
 [InlineData("G3-U03-L20-WU-4","124",false)]
 [InlineData("G3-U03-L20-FIRSTPARTY-CHAIRS","22",true)]
 [InlineData("G3-U03-L20-FIRSTPARTY-CHAIRS","23",false)]
 [InlineData("G3-U03-L20-FIRSTPARTY-BAKE","53",true)]
 [InlineData("G3-U03-L20-FIRSTPARTY-BAKE","54",false)]
 public void ExactOriginalOrExplicitFirstParty(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void PublisherUnknownCardsRemainTeacherOnly(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(6,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(8,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"53");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

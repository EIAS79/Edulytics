using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 source value oracles for nearest-ten and nearest-hundred ranges; unrun.</summary>
public sealed class UsCcssG3Unit3Lesson16RoundingIntervalsSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L16",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L16-WU-1","35",true)]
 [InlineData("G3-U03-L16-WU-1","36",false)]
 [InlineData("G3-U03-L16-WU-2","28",true)]
 [InlineData("G3-U03-L16-WU-2","29",false)]
 [InlineData("G3-U03-L16-WU-3","42",true)]
 [InlineData("G3-U03-L16-WU-3","43",false)]
 [InlineData("G3-U03-L16-WU-4","32",true)]
 [InlineData("G3-U03-L16-WU-4","33",false)]
 [InlineData("G3-U03-L16-A1-50-10-LOW","45",true)]
 [InlineData("G3-U03-L16-A1-50-10-LOW","46",false)]
 [InlineData("G3-U03-L16-A1-50-10-HIGH","54",true)]
 [InlineData("G3-U03-L16-A1-50-10-HIGH","55",false)]
 [InlineData("G3-U03-L16-A1-50-10-COUNT","10",true)]
 [InlineData("G3-U03-L16-A1-50-10-COUNT","11",false)]
 [InlineData("G3-U03-L16-A1-70-10-LOW","65",true)]
 [InlineData("G3-U03-L16-A1-70-10-LOW","66",false)]
 [InlineData("G3-U03-L16-A1-70-10-HIGH","74",true)]
 [InlineData("G3-U03-L16-A1-70-10-HIGH","75",false)]
 [InlineData("G3-U03-L16-A1-70-10-COUNT","10",true)]
 [InlineData("G3-U03-L16-A1-70-10-COUNT","11",false)]
 [InlineData("G3-U03-L16-A1-600-100-LOW","550",true)]
 [InlineData("G3-U03-L16-A1-600-100-LOW","551",false)]
 [InlineData("G3-U03-L16-A1-600-100-HIGH","649",true)]
 [InlineData("G3-U03-L16-A1-600-100-HIGH","650",false)]
 [InlineData("G3-U03-L16-A1-600-100-COUNT","100",true)]
 [InlineData("G3-U03-L16-A1-600-100-COUNT","101",false)]
 [InlineData("G3-U03-L16-A1-100-100-LOW","50",true)]
 [InlineData("G3-U03-L16-A1-100-100-LOW","51",false)]
 [InlineData("G3-U03-L16-A1-100-100-HIGH","149",true)]
 [InlineData("G3-U03-L16-A1-100-100-HIGH","150",false)]
 [InlineData("G3-U03-L16-A1-100-100-COUNT","100",true)]
 [InlineData("G3-U03-L16-A1-100-100-COUNT","101",false)]
 [InlineData("G3-U03-L16-A1-500-100-LOW","450",true)]
 [InlineData("G3-U03-L16-A1-500-100-LOW","451",false)]
 [InlineData("G3-U03-L16-A1-500-100-HIGH","549",true)]
 [InlineData("G3-U03-L16-A1-500-100-HIGH","550",false)]
 [InlineData("G3-U03-L16-A1-500-100-COUNT","100",true)]
 [InlineData("G3-U03-L16-A1-500-100-COUNT","101",false)]
 public void ExactRangeAndMentalMathAnswers(string id,string answer,bool expected){var t=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(t,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void GroupCardAndGraphicsRemainHumanReviewed(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(19,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(8,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var t in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(t,"746");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

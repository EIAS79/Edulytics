using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 Mai unknown/ Mancala source checks, authored not executed.</summary>
public sealed class UsCcssG3Unit3Lesson19MaiMancalaSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L19",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L19-WU-BAR-UNKNOWN","104",true)]
 [InlineData("G3-U03-L19-WU-BAR-UNKNOWN","105",false)]
 [InlineData("G3-U03-L19-MAI-PACK","10",true)]
 [InlineData("G3-U03-L19-MAI-PACK","11",false)]
 [InlineData("G3-U03-L19-MAI-INITIAL","104",true)]
 [InlineData("G3-U03-L19-MAI-INITIAL","105",false)]
 [InlineData("G3-U03-L19-MAI-FINAL","124",true)]
 [InlineData("G3-U03-L19-MAI-FINAL","125",false)]
 [InlineData("G3-U03-L19-MANCALA","86",true)]
 [InlineData("G3-U03-L19-MANCALA","87",false)]
 public void UnknownMeaningAndMancalaExact(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void DiagramsAndEquationsNeedTeacherReview(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(5,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(7,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"124");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

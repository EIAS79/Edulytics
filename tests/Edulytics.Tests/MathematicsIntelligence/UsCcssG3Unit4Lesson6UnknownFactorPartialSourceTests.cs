using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 L63 original market table all four rows verified from source image; tests authored, not run. Historic file name retains PartialSourceTests suffix.</summary>
public sealed class UsCcssG3Unit4Lesson6UnknownFactorPartialSourceTests
{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L06",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
 [Theory]
    [InlineData("G3-U04-L06-WU-MISSING-FACTOR","4",true)]
    [InlineData("G3-U04-L06-WU-MISSING-FACTOR","5",false)]
    [InlineData("G3-U04-L06-WU-QUOTIENT","4",true)]
    [InlineData("G3-U04-L06-WU-QUOTIENT","5",false)]
    [InlineData("G3-U04-L06-A1-ONION-FACTOR","7",true)]
    [InlineData("G3-U04-L06-A1-ONION-FACTOR","8",false)]
    [InlineData("G3-U04-L06-A1-ONION-QUOTIENT","7",true)]
    [InlineData("G3-U04-L06-A1-ONION-QUOTIENT","8",false)]
    [InlineData("G3-U04-L06-A2-ROW1-QUOTIENT","6",true)]
    [InlineData("G3-U04-L06-A2-ROW1-QUOTIENT","7",false)]
    [InlineData("G3-U04-L06-A2-ROW2-QUOTIENT","5",true)]
    [InlineData("G3-U04-L06-A2-ROW2-QUOTIENT","6",false)]
    [InlineData("G3-U04-L06-A2-ROW3-QUOTIENT","3",true)]
    [InlineData("G3-U04-L06-A2-ROW3-QUOTIENT","4",false)]
    [InlineData("G3-U04-L06-A2-ROW4-QUOTIENT","3",true)]
    [InlineData("G3-U04-L06-A2-ROW4-QUOTIENT","4",false)]
 public void OriginalArithmeticAndFourMarketRows(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
 [Fact]
 public void FourOriginalTableRowsAreDistinctAndTeacherMustCheckModels(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(8,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(11,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));for(int i=1;i<=4;i++){Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U04-L06-A2-ROW"+i+"-QUOTIENT");Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U04-L06-A2-ROW"+i+"-MODEL");}var final=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L06-A2-ROW4-OPEN-STORY");var result=Grade3SourceCompanionCatalogue.Grade(final,"3");Assert.Null(result.NumericCorrect);Assert.True(result.TeacherReview);}
}

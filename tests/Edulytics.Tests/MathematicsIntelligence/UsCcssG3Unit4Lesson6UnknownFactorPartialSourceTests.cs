using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Tests for verified parts of source L63; ORIGINAL MARKET TABLE NOT INSPECTED. NOT RUN.</summary>
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
 public void KnownOriginalFactFamiliesHaveCorrectOracles(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
 [Fact]
 public void MissingSourceTablePreventsPromotionAndNumericGrading(){var l=Lesson();Assert.False(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(4,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));var market=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L06-A2-SOURCE-IMAGE-UNAVAILABLE");Assert.Equal("teacher",market.AnswerType);var result=Grade3SourceCompanionCatalogue.Grade(market,"7");Assert.Null(result.NumericCorrect);Assert.True(result.TeacherReview);}
}

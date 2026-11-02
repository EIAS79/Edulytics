using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 exact apple contexts and multi-solution source list. Tests authored not run.</summary>
public sealed class UsCcssG3Unit4Lesson21AppleOperationsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L21",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L21-A1-SAMPLE-BOX","6",true)]
    [InlineData("G3-U04-L21-A1-SAMPLE-BOX","7",false)]
    [InlineData("G3-U04-L21-A2-TYLER","35",true)]
    [InlineData("G3-U04-L21-A2-TYLER","36",false)]
    [InlineData("G3-U04-L21-A2-CLARE","101",true)]
    [InlineData("G3-U04-L21-A2-CLARE","102",false)]
    [InlineData("G3-U04-L21-A2-JARS","4",true)]
    [InlineData("G3-U04-L21-A2-JARS","5",false)]
    [InlineData("G3-U04-L21-SECTION-52","13",true)]
    [InlineData("G3-U04-L21-SECTION-52","14",false)]
    [InlineData("G3-U04-L21-SECTION-40","40",true)]
    [InlineData("G3-U04-L21-SECTION-40","41",false)]
    [InlineData("G3-U04-L21-SECTION-12","12",true)]
    [InlineData("G3-U04-L21-SECTION-12","13",false)]
public void SourceFixedNumericExamples(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void OpenFourNumberTupleNeedsHumanReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(7,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L21-A1-VALID-FOUR");var result=Grade3SourceCompanionCatalogue.Grade(q,"400,280,10,12");Assert.Null(result.NumericCorrect);Assert.True(result.TeacherReview);}
}

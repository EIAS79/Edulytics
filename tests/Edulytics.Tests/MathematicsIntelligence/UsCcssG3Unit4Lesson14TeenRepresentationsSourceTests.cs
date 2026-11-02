using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 original source maths; not executed.</summary>
public sealed class UsCcssG3Unit4Lesson14TeenRepresentationsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L14",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L14-WU-TWO-TWELVES","24",true)]
    [InlineData("G3-U04-L14-WU-TWO-TWELVES","25",false)]
    [InlineData("G3-U04-L14-A1-SEVEN-THIRTEENS","91",true)]
    [InlineData("G3-U04-L14-A1-SEVEN-THIRTEENS","92",false)]
    [InlineData("G3-U04-L14-A1-SEVEN-TENS","70",true)]
    [InlineData("G3-U04-L14-A1-SEVEN-TENS","71",false)]
    [InlineData("G3-U04-L14-A1-SEVEN-THREES","21",true)]
    [InlineData("G3-U04-L14-A1-SEVEN-THREES","22",false)]
    [InlineData("G3-U04-L14-A1-THREE-FOURTEENS","42",true)]
    [InlineData("G3-U04-L14-A1-THREE-FOURTEENS","43",false)]
    [InlineData("G3-U04-L14-A1-OPTIONAL-FOUR-TWELVES","48",true)]
    [InlineData("G3-U04-L14-A1-OPTIONAL-FOUR-TWELVES","49",false)]
    [InlineData("G3-U04-L14-A1-OPTIONAL-FIVE-SIXTEENS","80",true)]
    [InlineData("G3-U04-L14-A1-OPTIONAL-FIVE-SIXTEENS","81",false)]
    [InlineData("G3-U04-L14-A2-THREE-FIFTEENS","45",true)]
    [InlineData("G3-U04-L14-A2-THREE-FIFTEENS","46",false)]
    [InlineData("G3-U04-L14-A2-THREE-TENS","30",true)]
    [InlineData("G3-U04-L14-A2-THREE-TENS","31",false)]
    [InlineData("G3-U04-L14-A2-THREE-FIVES","15",true)]
    [InlineData("G3-U04-L14-A2-THREE-FIVES","16",false)]
    [InlineData("G3-U04-L14-SYNTHESIS-SEVEN-SIXES","42",true)]
    [InlineData("G3-U04-L14-SYNTHESIS-SEVEN-SIXES","43",false)]
public void VerifiedNumericAnchors(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void TeacherMustReviewOriginalDiagrams(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(11,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"45");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

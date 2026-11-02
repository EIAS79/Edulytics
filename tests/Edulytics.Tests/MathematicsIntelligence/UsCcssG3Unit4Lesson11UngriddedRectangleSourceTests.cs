using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Exact original 2021 A–L source card mathematics and ungridded areas. Tests written, not run.</summary>
public sealed class UsCcssG3Unit4Lesson11UngriddedRectangleSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L11",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L11-WU-18","18",true)]
    [InlineData("G3-U04-L11-WU-18","19",false)]
    [InlineData("G3-U04-L11-A1-A","45",true)]
    [InlineData("G3-U04-L11-A1-A","46",false)]
    [InlineData("G3-U04-L11-A1-B","36",true)]
    [InlineData("G3-U04-L11-A1-B","37",false)]
    [InlineData("G3-U04-L11-A1-C","56",true)]
    [InlineData("G3-U04-L11-A1-C","57",false)]
    [InlineData("G3-U04-L11-CARD-A","28",true)]
    [InlineData("G3-U04-L11-CARD-A","29",false)]
    [InlineData("G3-U04-L11-CARD-B","42",true)]
    [InlineData("G3-U04-L11-CARD-B","43",false)]
    [InlineData("G3-U04-L11-CARD-C","28",true)]
    [InlineData("G3-U04-L11-CARD-C","29",false)]
    [InlineData("G3-U04-L11-CARD-D","24",true)]
    [InlineData("G3-U04-L11-CARD-D","25",false)]
    [InlineData("G3-U04-L11-CARD-E","48",true)]
    [InlineData("G3-U04-L11-CARD-E","49",false)]
    [InlineData("G3-U04-L11-CARD-F","24",true)]
    [InlineData("G3-U04-L11-CARD-F","25",false)]
    [InlineData("G3-U04-L11-CARD-G","24",true)]
    [InlineData("G3-U04-L11-CARD-G","25",false)]
    [InlineData("G3-U04-L11-CARD-H","36",true)]
    [InlineData("G3-U04-L11-CARD-H","37",false)]
    [InlineData("G3-U04-L11-CARD-I","42",true)]
    [InlineData("G3-U04-L11-CARD-I","43",false)]
    [InlineData("G3-U04-L11-CARD-J","36",true)]
    [InlineData("G3-U04-L11-CARD-J","37",false)]
    [InlineData("G3-U04-L11-CARD-K","48",true)]
    [InlineData("G3-U04-L11-CARD-K","49",false)]
    [InlineData("G3-U04-L11-CARD-L","28",true)]
    [InlineData("G3-U04-L11-CARD-L","29",false)]
    [InlineData("G3-U04-L11-SUMMARY-MUL","30",true)]
    [InlineData("G3-U04-L11-SUMMARY-MUL","31",false)]
    [InlineData("G3-U04-L11-SUMMARY-DIV5","6",true)]
    [InlineData("G3-U04-L11-SUMMARY-DIV5","7",false)]
    [InlineData("G3-U04-L11-SUMMARY-DIV6","5",true)]
    [InlineData("G3-U04-L11-SUMMARY-DIV6","6",false)]
    [InlineData("G3-U04-L11-SUMMARY-SEVEN3","21",true)]
    [InlineData("G3-U04-L11-SUMMARY-SEVEN3","22",false)]
public void SourceCardAndRectangleMathOracles(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void CardSortAndUngridddedMarkingRequireTeacherReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(20,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var gr in new[]{28,42,24,48,36}){var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U04-L11-A2-SORT-"+gr);var r=Grade3SourceCompanionCatalogue.Grade(q,"24");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

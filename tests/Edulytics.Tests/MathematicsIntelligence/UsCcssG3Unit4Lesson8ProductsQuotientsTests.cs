using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Written 2021 known-products/quotients source and first-party examples; tests not executed.</summary>
public sealed class UsCcssG3Unit4Lesson8ProductsQuotientsTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L08",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L08-WU-1","40",true)]
    [InlineData("G3-U04-L08-WU-1","41",false)]
    [InlineData("G3-U04-L08-WU-2","10",true)]
    [InlineData("G3-U04-L08-WU-2","11",false)]
    [InlineData("G3-U04-L08-WU-3","4",true)]
    [InlineData("G3-U04-L08-WU-3","5",false)]
    [InlineData("G3-U04-L08-WU-4","10",true)]
    [InlineData("G3-U04-L08-WU-4","11",false)]
    [InlineData("G3-U04-L08-A2-EXAMPLE-1","5",true)]
    [InlineData("G3-U04-L08-A2-EXAMPLE-1","6",false)]
    [InlineData("G3-U04-L08-A2-EXAMPLE-2","4",true)]
    [InlineData("G3-U04-L08-A2-EXAMPLE-2","5",false)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-6X7-A","7",true)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-6X7-A","8",false)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-6X7-B","6",true)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-6X7-B","7",false)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-3X8-A","8",true)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-3X8-A","9",false)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-3X8-B","3",true)]
    [InlineData("G3-U04-L08-A2-FIRSTPARTY-3X8-B","4",false)]
public void SourceAndExplicitFirstPartyNumbers(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void PersonalFluencySortNeedsTeacherReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(10,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(11,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.Id.StartsWith("G3-U04-L08-A1-"))){var r=Grade3SourceCompanionCatalogue.Grade(q,"42");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

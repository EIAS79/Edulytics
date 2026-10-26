using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 two-row-table figures and multi-whole source fractions. Authored not run.</summary>
public sealed class UsCcssG3Unit5Lesson3NonUnitSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U05:L03",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U05-L03-WU-1","1",true)]
    [InlineData("G3-U05-L03-WU-1","2",false)]
    [InlineData("G3-U05-L03-WU-2","2",true)]
    [InlineData("G3-U05-L03-WU-2","3",false)]
    [InlineData("G3-U05-L03-WU-3","3",true)]
    [InlineData("G3-U05-L03-WU-3","4",false)]
    [InlineData("G3-U05-L03-WU-4","4",true)]
    [InlineData("G3-U05-L03-WU-4","5",false)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW1","1",true)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW1","2",false)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW2","3",true)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW2","4",false)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW3","4",true)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW3","5",false)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW4","2",true)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW4","3",false)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW5","3",true)]
    [InlineData("G3-U05-L03-A1-TABLE1-ROW5","4",false)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW1","4",true)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW1","5",false)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW2","6",true)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW2","7",false)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW3","7",true)]
    [InlineData("G3-U05-L03-A1-TABLE2-ROW3","8",false)]
    [InlineData("G3-U05-L03-SYN-5OF8","5",true)]
    [InlineData("G3-U05-L03-SYN-5OF8","6",false)]
    [InlineData("G3-U05-L03-SYN-7OF6","7",true)]
    [InlineData("G3-U05-L03-SYN-7OF6","8",false)]
    [InlineData("G3-U05-L03-MATCH-WHOLE2","2",true)]
    [InlineData("G3-U05-L03-MATCH-WHOLE2","3",false)]
    [InlineData("G3-U05-L03-MATCH-WHOLE3","3",true)]
    [InlineData("G3-U05-L03-MATCH-WHOLE3","4",false)]
    [InlineData("G3-U05-L03-MATCH-WHOLE4","4",true)]
    [InlineData("G3-U05-L03-MATCH-WHOLE4","5",false)]
    [InlineData("G3-U05-L03-MATCH-WHOLE6","6",true)]
    [InlineData("G3-U05-L03-MATCH-WHOLE6","7",false)]
public void NumericShadedCountsSourceOriginal(string id,string response,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,response);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void OriginalTableAndCreatedGamePairsRequireHumanReview(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(18,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(15,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var id in new[]{"G3-U05-L03-A1-SECOND-ROWS","G3-U05-L03-A2-FOUR-PAIRS"}){var q=l.SourceSubtasks.Single(x=>x.Id==id);Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"7/6").NumericCorrect);}}
}

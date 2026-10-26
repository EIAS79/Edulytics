using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 rectangle source SVG semantics. Tests authored but not run.</summary>
public sealed class UsCcssG3Unit4Lesson10AreaDecompositionSourceTests
{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L10",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
 [Theory]
    [InlineData("G3-U04-L10-WU-SHADED-FIRST","8",true)]
    [InlineData("G3-U04-L10-WU-SHADED-FIRST","9",false)]
    [InlineData("G3-U04-L10-WU-TOTAL-FIRST","16",true)]
    [InlineData("G3-U04-L10-WU-TOTAL-FIRST","17",false)]
    [InlineData("G3-U04-L10-WU-SPLIT-4X6","24",true)]
    [InlineData("G3-U04-L10-WU-SPLIT-4X6","25",false)]
    [InlineData("G3-U04-L10-WU-FIVE-SHADED","15",true)]
    [InlineData("G3-U04-L10-WU-FIVE-SHADED","16",false)]
    [InlineData("G3-U04-L10-WU-TOTAL-3X6","18",true)]
    [InlineData("G3-U04-L10-WU-TOTAL-3X6","19",false)]
    [InlineData("G3-U04-L10-WU-OTHER-3X6","18",true)]
    [InlineData("G3-U04-L10-WU-OTHER-3X6","19",false)]
    [InlineData("G3-U04-L10-A1-ANDRE","18",true)]
    [InlineData("G3-U04-L10-A1-ANDRE","19",false)]
    [InlineData("G3-U04-L10-A1-ELENA","18",true)]
    [InlineData("G3-U04-L10-A1-ELENA","19",false)]
    [InlineData("G3-U04-L10-A1-FOUR-BY-NINE","36",true)]
    [InlineData("G3-U04-L10-A1-FOUR-BY-NINE","37",false)]
    [InlineData("G3-U04-L10-A2-NOAH","21",true)]
    [InlineData("G3-U04-L10-A2-NOAH","22",false)]
    [InlineData("G3-U04-L10-A2-PRIYA","24",true)]
    [InlineData("G3-U04-L10-A2-PRIYA","25",false)]
    [InlineData("G3-U04-L10-A2-TYLER","64",true)]
    [InlineData("G3-U04-L10-A2-TYLER","65",false)]
 public void SourceRectangleAreas(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
 [Fact]
 public void OriginalShadingIsTeacherReviewed(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"18");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 four Diego partitions and all products. Authored, never run.</summary>
public sealed class UsCcssG3Unit4Lesson16LargerProductsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L16",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L16-WU-3X10","30",true)]
    [InlineData("G3-U04-L16-WU-3X10","31",false)]
    [InlineData("G3-U04-L16-WU-3X20","60",true)]
    [InlineData("G3-U04-L16-WU-3X20","61",false)]
    [InlineData("G3-U04-L16-WU-3X50","150",true)]
    [InlineData("G3-U04-L16-WU-3X50","151",false)]
    [InlineData("G3-U04-L16-WU-3X25","75",true)]
    [InlineData("G3-U04-L16-WU-3X25","76",false)]
    [InlineData("G3-U04-L16-A1-4X23","92",true)]
    [InlineData("G3-U04-L16-A1-4X23","93",false)]
    [InlineData("G3-U04-L16-A1-DIEGO-A","92",true)]
    [InlineData("G3-U04-L16-A1-DIEGO-A","93",false)]
    [InlineData("G3-U04-L16-A1-DIEGO-B","92",true)]
    [InlineData("G3-U04-L16-A1-DIEGO-B","93",false)]
    [InlineData("G3-U04-L16-A1-DIEGO-C","92",true)]
    [InlineData("G3-U04-L16-A1-DIEGO-C","93",false)]
    [InlineData("G3-U04-L16-A1-DIEGO-D","92",true)]
    [InlineData("G3-U04-L16-A1-DIEGO-D","93",false)]
    [InlineData("G3-U04-L16-A1-3X28","84",true)]
    [InlineData("G3-U04-L16-A1-3X28","85",false)]
    [InlineData("G3-U04-L16-A2-2X37","74",true)]
    [InlineData("G3-U04-L16-A2-2X37","75",false)]
    [InlineData("G3-U04-L16-A2-3X32","96",true)]
    [InlineData("G3-U04-L16-A2-3X32","97",false)]
    [InlineData("G3-U04-L16-A2-2X43","86",true)]
    [InlineData("G3-U04-L16-A2-2X43","87",false)]
    [InlineData("G3-U04-L16-A2-4X22","88",true)]
    [InlineData("G3-U04-L16-A2-4X22","89",false)]
    [InlineData("G3-U04-L16-A2-3X29","87",true)]
    [InlineData("G3-U04-L16-A2-3X29","88",false)]
    [InlineData("G3-U04-L16-SYN-3X24","72",true)]
    [InlineData("G3-U04-L16-SYN-3X24","73",false)]
public void OriginalLargeProductMath(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void OriginalOptionalGameAndDiagramJustificationsTeacherReviewed(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(16,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"92");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

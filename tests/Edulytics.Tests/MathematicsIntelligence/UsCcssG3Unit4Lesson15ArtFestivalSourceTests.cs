using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original festival source numeric units and warm-up diagram scale; tests not run.</summary>
public sealed class UsCcssG3Unit4Lesson15ArtFestivalSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L15",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L15-WU-A","98",true)]
    [InlineData("G3-U04-L15-WU-A","99",false)]
    [InlineData("G3-U04-L15-WU-B","91",true)]
    [InlineData("G3-U04-L15-WU-B","92",false)]
    [InlineData("G3-U04-L15-WU-C","98",true)]
    [InlineData("G3-U04-L15-WU-C","99",false)]
    [InlineData("G3-U04-L15-WU-D","98",true)]
    [InlineData("G3-U04-L15-WU-D","99",false)]
    [InlineData("G3-U04-L15-A1-MURAL","60",true)]
    [InlineData("G3-U04-L15-A1-MURAL","61",false)]
    [InlineData("G3-U04-L15-A1-MOSAIC","96",true)]
    [InlineData("G3-U04-L15-A1-MOSAIC","97",false)]
    [InlineData("G3-U04-L15-A1-SIDEWALK","84",true)]
    [InlineData("G3-U04-L15-A1-SIDEWALK","85",false)]
    [InlineData("G3-U04-L15-A1-STICKERS","80",true)]
    [InlineData("G3-U04-L15-A1-STICKERS","81",false)]
    [InlineData("G3-U04-L15-FIRSTPARTY-6X10","60",true)]
    [InlineData("G3-U04-L15-FIRSTPARTY-6X10","61",false)]
    [InlineData("G3-U04-L15-FIRSTPARTY-6X4","24",true)]
    [InlineData("G3-U04-L15-FIRSTPARTY-6X4","25",false)]
    [InlineData("G3-U04-L15-FIRSTPARTY-5X10","50",true)]
    [InlineData("G3-U04-L15-FIRSTPARTY-5X10","51",false)]
    [InlineData("G3-U04-L15-FIRSTPARTY-5X6","30",true)]
    [InlineData("G3-U04-L15-FIRSTPARTY-5X6","31",false)]
public void OriginalSourceAreaValues(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void GalleryWalkAndScaleRequireTeacher(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var r=Grade3SourceCompanionCatalogue.Grade(q,"84");Assert.Null(r.NumericCorrect);Assert.True(r.TeacherReview);}}
}

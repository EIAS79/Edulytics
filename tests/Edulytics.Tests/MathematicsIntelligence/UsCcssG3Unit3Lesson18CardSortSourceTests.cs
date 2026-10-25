using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 card situation/equation anchor tests, not run.</summary>
public sealed class UsCcssG3Unit3Lesson18CardSortSourceTests{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U03:L18",out var x));return Assert.IsType<Grade3CompanionLesson>(x);}
 [Theory]
 [InlineData("G3-U03-L18-CARD-A","203",true)]
 [InlineData("G3-U03-L18-CARD-A","204",false)]
 [InlineData("G3-U03-L18-CARD-F","203",true)]
 [InlineData("G3-U03-L18-CARD-F","204",false)]
 [InlineData("G3-U03-L18-CARD-C","165",true)]
 [InlineData("G3-U03-L18-CARD-C","166",false)]
 [InlineData("G3-U03-L18-CARD-J","165",true)]
 [InlineData("G3-U03-L18-CARD-J","166",false)]
 [InlineData("G3-U03-L18-CARD-D","295",true)]
 [InlineData("G3-U03-L18-CARD-D","296",false)]
 [InlineData("G3-U03-L18-CARD-L","295",true)]
 [InlineData("G3-U03-L18-CARD-L","296",false)]
 [InlineData("G3-U03-L18-CARD-E","285",true)]
 [InlineData("G3-U03-L18-CARD-E","286",false)]
 [InlineData("G3-U03-L18-CARD-G","285",true)]
 [InlineData("G3-U03-L18-CARD-G","286",false)]
 [InlineData("G3-U03-L18-CARD-I","332",true)]
 [InlineData("G3-U03-L18-CARD-I","333",false)]
 public void MathematicalCardAnchors(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var g=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,g.NumericCorrect);Assert.True(g.TeacherReview);}
 [Fact]public void VisualLetterAndGalleryNeverAutograde(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(9,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(7,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"225");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
}

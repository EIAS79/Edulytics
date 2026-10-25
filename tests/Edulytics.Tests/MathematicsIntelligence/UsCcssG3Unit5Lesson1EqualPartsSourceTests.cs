using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 U05 L01 A-L source parts and open folding. Tests authored, not run.</summary>
public sealed class UsCcssG3Unit5Lesson1EqualPartsSourceTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U05:L01",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U05-L01-A1-CARD-A","2",true)]
    [InlineData("G3-U05-L01-A1-CARD-A","3",false)]
    [InlineData("G3-U05-L01-A1-CARD-B","4",true)]
    [InlineData("G3-U05-L01-A1-CARD-B","5",false)]
    [InlineData("G3-U05-L01-A1-CARD-C","2",true)]
    [InlineData("G3-U05-L01-A1-CARD-C","3",false)]
    [InlineData("G3-U05-L01-A1-CARD-D","4",true)]
    [InlineData("G3-U05-L01-A1-CARD-D","5",false)]
    [InlineData("G3-U05-L01-A1-CARD-E","2",true)]
    [InlineData("G3-U05-L01-A1-CARD-E","3",false)]
    [InlineData("G3-U05-L01-A1-CARD-F","2",true)]
    [InlineData("G3-U05-L01-A1-CARD-F","3",false)]
    [InlineData("G3-U05-L01-A1-CARD-G","3",true)]
    [InlineData("G3-U05-L01-A1-CARD-G","4",false)]
    [InlineData("G3-U05-L01-A1-CARD-H","3",true)]
    [InlineData("G3-U05-L01-A1-CARD-H","4",false)]
    [InlineData("G3-U05-L01-A1-CARD-I","2",true)]
    [InlineData("G3-U05-L01-A1-CARD-I","3",false)]
    [InlineData("G3-U05-L01-A1-CARD-J","3",true)]
    [InlineData("G3-U05-L01-A1-CARD-J","4",false)]
    [InlineData("G3-U05-L01-A1-CARD-K","3",true)]
    [InlineData("G3-U05-L01-A1-CARD-K","4",false)]
    [InlineData("G3-U05-L01-A1-CARD-L","4",true)]
    [InlineData("G3-U05-L01-A1-CARD-L","5",false)]
    [InlineData("G3-U05-L01-A2-DENOM-2","2",true)]
    [InlineData("G3-U05-L01-A2-DENOM-2","3",false)]
    [InlineData("G3-U05-L01-A2-DENOM-3","3",true)]
    [InlineData("G3-U05-L01-A2-DENOM-3","4",false)]
    [InlineData("G3-U05-L01-A2-DENOM-4","4",true)]
    [InlineData("G3-U05-L01-A2-DENOM-4","5",false)]
    [InlineData("G3-U05-L01-A2-DENOM-6","6",true)]
    [InlineData("G3-U05-L01-A2-DENOM-6","7",false)]
    [InlineData("G3-U05-L01-A2-DENOM-8","8",true)]
    [InlineData("G3-U05-L01-A2-DENOM-8","9",false)]
public void SourcePartCountsAndDenominators(string id,string response,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var r=Grade3SourceCompanionCatalogue.Grade(q,response);Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);}
[Fact]
public void CardSourceClassificationAndFoldRequireTeacher(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(17,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(30,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(char card in "ABCDEFGHIJKL"){var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U05-L01-A1-VISUAL-"+card);Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"2").NumericCorrect);}for(int n=3;n<=8;n=n==3?4:n==4?6:8){var q=l.SourceSubtasks.Single(x=>x.Id=="G3-U05-L01-A2-FOLD-"+n);Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"1/"+n).TeacherReview);if(n==8)break;}}
}

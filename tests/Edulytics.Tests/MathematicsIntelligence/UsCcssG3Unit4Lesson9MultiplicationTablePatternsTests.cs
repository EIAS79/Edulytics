using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Original 2021 source SVG letter coordinates and fact-family oracles. Authored, never run.</summary>
public sealed class UsCcssG3Unit4Lesson9MultiplicationTablePatternsTests
{
private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L09",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
[Theory]
    [InlineData("G3-U04-L09-A1-A","14",true)]
    [InlineData("G3-U04-L09-A1-A","15",false)]
    [InlineData("G3-U04-L09-A1-B","27",true)]
    [InlineData("G3-U04-L09-A1-B","28",false)]
    [InlineData("G3-U04-L09-A1-C","24",true)]
    [InlineData("G3-U04-L09-A1-C","25",false)]
    [InlineData("G3-U04-L09-A1-D","40",true)]
    [InlineData("G3-U04-L09-A1-D","41",false)]
    [InlineData("G3-U04-L09-A1-E","60",true)]
    [InlineData("G3-U04-L09-A1-E","61",false)]
    [InlineData("G3-U04-L09-A1-F","49",true)]
    [InlineData("G3-U04-L09-A1-F","50",false)]
    [InlineData("G3-U04-L09-A1-G","72",true)]
    [InlineData("G3-U04-L09-A1-G","73",false)]
    [InlineData("G3-U04-L09-A2-KNOWN-1","8",true)]
    [InlineData("G3-U04-L09-A2-KNOWN-1","9",false)]
    [InlineData("G3-U04-L09-A2-KNOWN-2","15",true)]
    [InlineData("G3-U04-L09-A2-KNOWN-2","16",false)]
    [InlineData("G3-U04-L09-A2-KNOWN-3","40",true)]
    [InlineData("G3-U04-L09-A2-KNOWN-3","41",false)]
    [InlineData("G3-U04-L09-A2-KNOWN-4","14",true)]
    [InlineData("G3-U04-L09-A2-KNOWN-4","15",false)]
    [InlineData("G3-U04-L09-A2-KNOWN-5","40",true)]
    [InlineData("G3-U04-L09-A2-KNOWN-5","41",false)]
public void ExactOriginalTableCellsAndProducts(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
[Fact]
public void SourceLettersAndOpenNonUniqueTasks(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(12,l.SourceSubtasks.Count(t=>t.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(t=>t.AnswerType=="teacher"));foreach(var id in new[]{"A","B","C","D","E","F","G"}){Assert.Contains(l.SourceSubtasks,q=>q.Id=="G3-U04-L09-A1-"+id);Assert.Contains(l.SourceSubtasks,q=>q.Id=="G3-U04-L09-A1-POS-"+id);}var three=l.SourceSubtasks.Single(q=>q.Id=="G3-U04-L09-A1-THREE-OTHER");Assert.Null(Grade3SourceCompanionCatalogue.Grade(three,"63").NumericCorrect);}
}

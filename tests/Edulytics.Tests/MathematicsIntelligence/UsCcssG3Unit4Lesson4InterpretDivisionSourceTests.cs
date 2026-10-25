using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>2021 lesson61 arithmetic, expression matches and human diagram boundary. Authored, never run.</summary>
public sealed class UsCcssG3Unit4Lesson4InterpretDivisionSourceTests
{
    private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L04",out var lesson));return Assert.IsType<Grade3CompanionLesson>(lesson);}
    [Theory]
    [InlineData("G3-U04-L04-WU-1","25",true)]
    [InlineData("G3-U04-L04-WU-1","26",false)]
    [InlineData("G3-U04-L04-WU-2","29",true)]
    [InlineData("G3-U04-L04-WU-2","30",false)]
    [InlineData("G3-U04-L04-WU-3","37",true)]
    [InlineData("G3-U04-L04-WU-3","38",false)]
    [InlineData("G3-U04-L04-WU-4","63",true)]
    [InlineData("G3-U04-L04-WU-4","64",false)]
    [InlineData("G3-U04-L04-A1-CLARE","6",true)]
    [InlineData("G3-U04-L04-A1-CLARE","7",false)]
    [InlineData("G3-U04-L04-A1-PRIYA","12",true)]
    [InlineData("G3-U04-L04-A1-PRIYA","13",false)]
    [InlineData("G3-U04-L04-A1-STORE","4",true)]
    [InlineData("G3-U04-L04-A1-STORE","5",false)]
    [InlineData("G3-U04-L04-A1-DIEGO","6",true)]
    [InlineData("G3-U04-L04-A1-DIEGO","7",false)]
    [InlineData("G3-U04-L04-A1-DREIDELS","2",true)]
    [InlineData("G3-U04-L04-A1-DREIDELS","3",false)]
    [InlineData("G3-U04-L04-A2-HAN-THREE-BOXES","7",true)]
    [InlineData("G3-U04-L04-A2-HAN-THREE-BOXES","8",false)]
    [InlineData("G3-U04-L04-A2-HAN-THREE-PER","7",true)]
    [InlineData("G3-U04-L04-A2-HAN-THREE-PER","8",false)]
    [InlineData("G3-U04-L04-A3-KIRAN","3",true)]
    [InlineData("G3-U04-L04-A3-KIRAN","4",false)]
    [InlineData("G3-U04-L04-A3-HAN","3",true)]
    [InlineData("G3-U04-L04-A3-HAN","4",false)]
    [InlineData("G3-U04-L04-A3-JADA","2",true)]
    [InlineData("G3-U04-L04-A3-JADA","3",false)]
    [InlineData("G3-U04-L04-A3-MAI","2",true)]
    [InlineData("G3-U04-L04-A3-MAI","3",false)]
    public void SourceMathOraclesRejectWrongQuotients(string id,string answer,bool expected){var task=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(task,answer);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
    [Fact]
    public void OriginalQuestionFamiliesStayDistinctAndOpenTasksRequireTeacher(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(15,l.SourceSubtasks.Count(q=>q.AnswerType=="integer"));Assert.Equal(11,l.SourceSubtasks.Count(q=>q.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(q=>q.AnswerType=="teacher")){var g=Grade3SourceCompanionCatalogue.Grade(q,"7");Assert.Null(g.NumericCorrect);Assert.True(g.TeacherReview);}}
    [Fact]
    public void TwoDivisionMeaningsAndDiagramsAreNotCollapsed(){var l=Lesson();Assert.Equal("7",l.SourceSubtasks.Single(t=>t.Id=="G3-U04-L04-A2-HAN-THREE-BOXES").Expected);Assert.Equal("7",l.SourceSubtasks.Single(t=>t.Id=="G3-U04-L04-A2-HAN-THREE-PER").Expected);Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U04-L04-A3-DRAW-HAN"&&x.Prompt.Contains("A")&&x.Prompt.Contains("6÷2"));Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U04-L04-A3-DRAW-JADA"&&x.Prompt.Contains("A")&&x.Prompt.Contains("6÷3"));}
}

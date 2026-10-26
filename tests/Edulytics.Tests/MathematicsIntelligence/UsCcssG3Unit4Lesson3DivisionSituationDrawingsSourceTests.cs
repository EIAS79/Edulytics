using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Source 2021 double-role division drawings; authored but not executed.</summary>
public sealed class UsCcssG3Unit4Lesson3DivisionSituationDrawingsSourceTests
{
    private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L03",out var lesson));return Assert.IsType<Grade3CompanionLesson>(lesson);}
    [Theory]
    [InlineData("G3-U04-L03-WU-1","240",true)]
    [InlineData("G3-U04-L03-WU-1","241",false)]
    [InlineData("G3-U04-L03-WU-2","240",true)]
    [InlineData("G3-U04-L03-WU-2","241",false)]
    [InlineData("G3-U04-L03-WU-3","240",true)]
    [InlineData("G3-U04-L03-WU-3","241",false)]
    [InlineData("G3-U04-L03-WU-4","240",true)]
    [InlineData("G3-U04-L03-WU-4","241",false)]
    [InlineData("G3-U04-L03-ELENA","6",true)]
    [InlineData("G3-U04-L03-ELENA","7",false)]
    [InlineData("G3-U04-L03-A3-MAI","2",true)]
    [InlineData("G3-U04-L03-A3-MAI","3",false)]
    [InlineData("G3-U04-L03-A3-KIRAN","10",true)]
    [InlineData("G3-U04-L03-A3-KIRAN","11",false)]
    [InlineData("G3-U04-L03-A3-LIN","4",true)]
    [InlineData("G3-U04-L03-A3-LIN","5",false)]
    [InlineData("G3-U04-L03-A3-PRIYA","3",true)]
    [InlineData("G3-U04-L03-A3-PRIYA","4",false)]
    [InlineData("G3-U04-L03-A3-NOAH","2",true)]
    [InlineData("G3-U04-L03-A3-NOAH","3",false)]
    [InlineData("G3-U04-L03-A3-JADA","5",true)]
    [InlineData("G3-U04-L03-A3-JADA","6",false)]
    public void OriginalSourceDivisionAndNumberTalkOracles(string id,string answer,bool expected){var task=Lesson().SourceSubtasks.Single(x=>x.Id==id);var result=Grade3SourceCompanionCatalogue.Grade(task,answer);Assert.Equal(expected,result.NumericCorrect);Assert.True(result.TeacherReview);}
    [Fact]
    public void SourceDiagramsAndHumanExplanationRemainTeacherReview(){Assert.True(Lesson().ReadyForStageD);Assert.False(Lesson().OriginalStudentTaskPdfRead);Assert.Equal(11,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(9,Lesson().SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var task in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var result=Grade3SourceCompanionCatalogue.Grade(task,"6");Assert.Null(result.NumericCorrect);Assert.True(result.TeacherReview);}}
}

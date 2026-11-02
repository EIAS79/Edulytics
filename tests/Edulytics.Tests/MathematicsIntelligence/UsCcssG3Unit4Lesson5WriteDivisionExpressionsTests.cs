using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade 3 2021 original bug cards and dual-meaning source checks. Written; never run.</summary>
public sealed class UsCcssG3Unit4Lesson5WriteDivisionExpressionsTests
{
 private static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U04:L05",out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
 [Theory]
    [InlineData("G3-U04-L05-WU-1","125",true)]
    [InlineData("G3-U04-L05-WU-1","126",false)]
    [InlineData("G3-U04-L05-WU-2","125",true)]
    [InlineData("G3-U04-L05-WU-2","126",false)]
    [InlineData("G3-U04-L05-WU-3","125",true)]
    [InlineData("G3-U04-L05-WU-3","126",false)]
    [InlineData("G3-U04-L05-WU-4","125",true)]
    [InlineData("G3-U04-L05-WU-4","126",false)]
    [InlineData("G3-U04-L05-A1-A","2",true)]
    [InlineData("G3-U04-L05-A1-A","3",false)]
    [InlineData("G3-U04-L05-A1-B","4",true)]
    [InlineData("G3-U04-L05-A1-B","5",false)]
    [InlineData("G3-U04-L05-A1-C","7",true)]
    [InlineData("G3-U04-L05-A1-C","8",false)]
    [InlineData("G3-U04-L05-A1-D","3",true)]
    [InlineData("G3-U04-L05-A1-D","4",false)]
    [InlineData("G3-U04-L05-A1-E","6",true)]
    [InlineData("G3-U04-L05-A1-E","7",false)]
    [InlineData("G3-U04-L05-A1-F","10",true)]
    [InlineData("G3-U04-L05-A1-F","11",false)]
    [InlineData("G3-U04-L05-SUMMARY-HAN","6",true)]
    [InlineData("G3-U04-L05-SUMMARY-HAN","7",false)]
    [InlineData("G3-U04-L05-SUMMARY-ELENA","6",true)]
    [InlineData("G3-U04-L05-SUMMARY-ELENA","7",false)]
 public void ArithmeticAndBugCardsRejectWrongAnswers(string id,string answer,bool expected){var q=Lesson().SourceSubtasks.Single(x=>x.Id==id);var graded=Grade3SourceCompanionCatalogue.Grade(q,answer);Assert.Equal(expected,graded.NumericCorrect);Assert.True(graded.TeacherReview);}
 [Fact]
 public void AllCardSortsAndHumanTasksAreNotNumericGraded(){var l=Lesson();Assert.True(l.ReadyForStageD);Assert.False(l.OriginalStudentTaskPdfRead);Assert.Equal(12,l.SourceSubtasks.Count(x=>x.AnswerType=="integer"));Assert.Equal(13,l.SourceSubtasks.Count(x=>x.AnswerType=="teacher"));foreach(var q in l.SourceSubtasks.Where(x=>x.AnswerType=="teacher")){var grade=Grade3SourceCompanionCatalogue.Grade(q,"6");Assert.Null(grade.NumericCorrect);Assert.True(grade.TeacherReview);}}
 [Fact]
 public void EveryBugCardHasSeparateClassificationAndCorrectExpression(){var l=Lesson();foreach(var id in new[]{"A","B","C","D","E","F"}){Assert.Contains(l.SourceSubtasks,q=>q.Id=="G3-U04-L05-A1-"+id);Assert.Contains(l.SourceSubtasks,q=>q.Id=="G3-U04-L05-A1-SORT-"+id);}Assert.NotEqual(l.SourceSubtasks.Single(q=>q.Id=="G3-U04-L05-SUMMARY-HAN").Prompt,l.SourceSubtasks.Single(q=>q.Id=="G3-U04-L05-SUMMARY-ELENA").Prompt);}
}

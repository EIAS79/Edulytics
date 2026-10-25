using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Generated first-party arithmetic intake contracts; NO test execution or publisher parity.</summary>
public sealed class UsCcssG3RemainingSourceIntakeContracts
{
    [Fact]
    public void AllRemainingSourceIntakeLessonsRemainUnapproved()
    {
        for(var ordinal=45;ordinal<=143;ordinal++)
        {
            var entry=Grade3SourceCompanionCatalogue.Codes
               .Select(code=>{Grade3SourceCompanionCatalogue.TryGet(code,out var l);return l;})
               .Single(l=>l!.Ordinal==ordinal)!;
            Assert.Equal(ordinal == 45,entry.ReadyForStageD);
            Assert.Equal(ordinal == 45,entry.OriginalStudentTaskPdfRead);
            Assert.NotEmpty(entry.SourceSubtasks);
            foreach(var task in entry.SourceSubtasks)
            {
                if(task.AnswerType=="teacher")
                {
                    var result=Grade3SourceCompanionCatalogue.Grade(task,"123");
                    Assert.Null(result.NumericCorrect);
                    Assert.True(result.TeacherReview);
                }
                else
                {
                    Assert.Equal("integer",task.AnswerType);
                    Assert.True(Grade3SourceCompanionCatalogue.Grade(task,task.Expected).NumericCorrect);
                    Assert.False(Grade3SourceCompanionCatalogue.Grade(task,"invalid").NumericCorrect);
                }
            }
        }
    }
}

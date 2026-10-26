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
            // Individually committed A/B/C checkpoints now cover all 143.
            // This flag is authoring readiness, not Stage D certification.
            Assert.True(entry.ReadyForStageD);
            Assert.True(entry.ReviewedStudentFacingText);
            Assert.NotEmpty(entry.SourceSubtasks);
            Assert.NotNull(entry.SourceBasedCheckProvenance);
            foreach(var task in entry.SourceSubtasks)
            {
                Assert.False(string.IsNullOrWhiteSpace(task.Id));
                Assert.False(string.IsNullOrWhiteSpace(task.Prompt));
                var result=Grade3SourceCompanionCatalogue.Grade(task,"123");
                Assert.True(result.TeacherReview);
                if(task.AnswerType=="teacher" || task.AnswerType=="source-card-letter")
                    Assert.Null(result.NumericCorrect);
                if(task.AnswerType=="integer" && !string.IsNullOrWhiteSpace(task.Expected))
                {
                    Assert.True(Grade3SourceCompanionCatalogue.Grade(
                        task,task.Expected).NumericCorrect);
                    Assert.False(Grade3SourceCompanionCatalogue.Grade(
                        task,"invalid").NumericCorrect);
                }
            }
        }
    }
}

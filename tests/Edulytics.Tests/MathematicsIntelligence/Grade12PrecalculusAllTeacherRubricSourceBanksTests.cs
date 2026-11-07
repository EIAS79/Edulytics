using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Structural teacher source-task bank coverage. Runtime authorization, actual
/// submitted answers and teacher scoring are tested in isolated GitHub HTTPS.
/// </summary>
public sealed class Grade12PrecalculusAllTeacherRubricSourceBanksTests
{
    [Fact]
    public void FirstPartyManualTeacherRubricHasRealLessonSpecificTasksForAll73Lessons()
    {
        var other=Grade12PrecalculusTeacherTaskBank.All;
        Assert.Equal(72,other.Count);
        Assert.DoesNotContain(Grade12PrecalculusTeacherTaskBank.FirstLesson,
            other.Keys);
        var first=Grade12PrecalculusTeacherTaskBank.Find(
            Grade12PrecalculusTeacherTaskBank.FirstLesson,1);
        Assert.NotNull(first);
        Assert.Contains("relation",first.Prompt,StringComparison.OrdinalIgnoreCase);

        var n=0;
        foreach(var (code,tasks) in other)
        {
            Assert.StartsWith(Grade12PrecalculusTeacherTaskBank.LessonPrefix,code,
                StringComparison.Ordinal);
            Assert.NotEmpty(tasks);
            Assert.Equal(tasks.Count,tasks.Select(t=>t.Number).Distinct().Count());
            foreach(var task in tasks)
            {
                Assert.True(task.Number >= 1);
                Assert.True(task.Prompt.Trim().Length >= 12);
                Assert.DoesNotContain("OpenStax",task.Prompt,
                    StringComparison.OrdinalIgnoreCase);
                Assert.Equal(task,Grade12PrecalculusTeacherTaskBank.Find(
                    code,task.Number));
                n++;
            }
        }
        Assert.True(n >= 72);
        Assert.Null(Grade12PrecalculusTeacherTaskBank.Find(
            "PED:US-CCSS-MATH:G5:U01:L01",1));
    }

    [Fact]
    public void FirstPartyMathExplanationsMustRemainTeacherReviewed()
    {
        foreach(var (code,tasks) in Grade12PrecalculusTeacherTaskBank.All)
        {
            var valid=Grade12PrecalculusTeacherTaskBank.ComposeTeacherResponse(
                code,tasks[0].Number,
                "A complete student explanation with the mathematical steps.",
                "Axes and coordinate descriptions from a synthetic CI response.");
            Assert.NotNull(valid);
            Assert.Null(Grade12PrecalculusTeacherTaskBank.ComposeTeacherResponse(
                code,tasks[0].Number,"too short",null));
            Assert.Null(Grade12PrecalculusTeacherTaskBank.ComposeTeacherResponse(
                code,99999,
                "A complete student explanation with the mathematical steps.",null));
        }
    }
}

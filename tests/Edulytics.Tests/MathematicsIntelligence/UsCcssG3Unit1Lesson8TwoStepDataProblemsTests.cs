using Edulytics.Core.Mathematics.Curriculum;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Authoring-only test plan; do not run Stage D yet.</summary>
public sealed class UsCcssG3Unit1Lesson8TwoStepDataProblemsTests
{
    [Fact]
    public void ArchivedTwoStepSourceDataAreExact()
    {
        Assert.Equal(new[]{55,23,25},UsCcssG3Unit1Lesson8TwoStepDataProblems.Feelings.Select(x=>x.Value));
        Assert.Equal(new[]{29,82,15,58},UsCcssG3Unit1Lesson8TwoStepDataProblems.Trees.Select(x=>x.Value));
        Assert.Equal(7,55-(23+25));
        Assert.Equal(38,82-(29+15));
        Assert.Equal(14,58-(29+15));
    }

    [Fact]
    public void FourOriginalRepeatedAdditionPromptsAndBothVerticalSourceAxesAreAuthored()
    {
        // Regression contract written; Stage D/CI have NOT been run.
        DirectoryInfo? dir = new(AppContext.BaseDirectory);
        string? path = null;
        while (dir is not null)
        {
            var p = Path.Combine(dir.FullName, "src", "Edulytics.Web",
                "Views", "Grade3Lesson8Activities", "Index.cshtml");
            if (File.Exists(p)) { path = p; break; }
            dir = dir.Parent;
        }
        Assert.NotNull(path);
        var razor = File.ReadAllText(path!);
        Assert.Contains("<li>2 + 2 + 2 + 2 + 2</li>", razor);
        Assert.Contains("<li>2 + 2 + 2 + 2 + 2 + 2</li>", razor);
        Assert.Contains("<li>5 + 5 + 5 + 5 + 5 + 5</li>", razor);
        Assert.Contains("<li>5 + 5 + 5 + 5 + 5 + 5 + 5</li>", razor);
        Assert.Contains("tick=60;tick>=0;tick-=5", razor);
        Assert.Contains("tick=100;tick>=0;tick-=10", razor);
        Assert.Contains("height:@(item.Value*300/60)px", razor);
        Assert.Contains("height:@(item.Value*300/100)px", razor);
        Assert.Contains("source-v-bars", razor);
    }

    [Theory]
    [InlineData("L08-FEELINGS-TWO-STEP",7)]
    [InlineData("L08-TREES-PINE-FIR",24)]
    [InlineData("L08-TREES-PINE-VS-OAK-MAPLE",38)]
    [InlineData("L08-TREES-PINE-OAK",53)]
    [InlineData("L08-TREES-FIR-VS-OAK-MAPLE",14)]
    [InlineData("L08-EDU-CD-ADVENTURE-SCIENCE",37)]
    [InlineData("L08-EDU-CD-ADVENTURE-VS-SCIENCE-COMICS",19)]
    [InlineData("L08-EDU-CD-TOTAL",101)]
    public void CorrectAndWrongTwoStepArithmetic(string id,int expected)
    {
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.TryGradeNumeric(id,expected,out var yes));
        Assert.True(yes);
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.TryGradeNumeric(id,expected+1,out var no));
        Assert.False(no);
    }

    [Fact]
    public void TeacherJudgmentCannotBeNumericallyPromoted()
    {
        Assert.False(UsCcssG3Unit1Lesson8TwoStepDataProblems.TryGradeNumeric(
            "L08-EDU-CD-EXPLAIN",0,out _));
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.RequiresHumanReview("three-reads"));
        Assert.True(UsCcssG3Unit1Lesson8TwoStepDataProblems.RequiresHumanReview("write-two-methods"));
    }
}

using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Digital arithmetic scoped to accessible 2021 U05 L08 Student Facing
/// exercises. Center turns, real classroom participation and oral reasons
/// are teacher-assessed, not proved by these source arithmetic checks.
/// Prepared but not executed during Stage A-C.
/// </summary>
public sealed class UsCcssG1Lesson96OriginalWarmupAndOptionalStoriesTests
{
    private const string Family="usccss.g1.u05.l08.center_add_within100";

    [Theory]
    [InlineData(68,2,4,70)]
    [InlineData(3,68,5,71)]
    [InlineData(5,45,6,50)]
    [InlineData(45,7,7,52)]
    [InlineData(35,6,8,41)]
    [InlineData(9,45,9,54)]
    public void OriginalNumberTalkAndFootballSourceTasksHaveCorrectTotalsAndCarry(
        int first,int second,int mode,int expectedSum)
    {
        var p=new Dictionary<string,int>{{"a",first},{"b",second},{"mode",mode}};
        var right=$"NEW_TEN=YES;TOTAL={expectedSum}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(right,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,right));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"NEW_TEN=NO;TOTAL={expectedSum}"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,$"NEW_TEN=YES;TOTAL={expectedSum+1}"));
        p["b"]=second+1;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Fact]
    public void AllTenLessonModesGenerateCorrectlyWithoutCertifyingPhysicalCenters()
    {
        var tasks=Enumerable.Range(0,1400).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(31*i+19)))
            .ToArray();
        foreach(var mode in Enumerable.Range(0,10))
            Assert.Contains(tasks,t=>t.Parameters["mode"]==mode);
        Assert.All(tasks,t=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,t.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,t.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,t.Parameters)));
        });
        Assert.All(tasks.Where(t=>t.Parameters["mode"]>=8),t=>
        {
            Assert.Contains("OPTIONAL FOOTBALL STORY",t.Prompt);
            Assert.Contains("teacher assessed",t.Prompt);
        });
    }
}

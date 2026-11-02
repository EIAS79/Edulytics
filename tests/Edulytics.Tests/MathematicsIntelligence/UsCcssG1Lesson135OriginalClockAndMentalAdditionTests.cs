using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lesson135OriginalClockAndMentalAdditionTests
{
    private const string Family="usccss.g1.u07.l16.daily_schedule_time";

    [Theory]
    [InlineData(4,8,32,"RESULT=40")]
    [InlineData(5,8,33,"RESULT=41")]
    [InlineData(6,8,38,"RESULT=46")]
    [InlineData(7,8,48,"RESULT=56")]
    [InlineData(8,6,12,"TIME=6:00;CORRECT=DIEGO")]
    public void AllFourOriginalMentalSumsAndDiegoClockCaseAreCorrect(
        int mode,int a,int b,string expected)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
            Family,p,expected+";EXTRA=1"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(Family,p));
    }

    [Fact]
    public void SeededSourceModesCoexistWithExistingHourAndHalfHourPractice()
    {
        var reached=new HashSet<int>();
        for(var seed=0;seed<1200;seed++)
        {
            var problem=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
                Family,new Random(seed*41+19));
            int mode=problem.Parameters["mode"];
            reached.Add(mode);
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
                Family,problem.Parameters,
                UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(Family,problem.Parameters)));
            if(mode==8)Assert.Contains("Diego",problem.Prompt);
        }
        foreach(var mode in Enumerable.Range(0,9))Assert.Contains(mode,reached);
    }
}

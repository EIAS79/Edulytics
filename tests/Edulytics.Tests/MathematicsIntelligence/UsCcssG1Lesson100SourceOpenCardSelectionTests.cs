using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 Grade 1 Unit 5 Lesson 12 five-card open-choice task.
/// Unlike a single fixed answer, several cases accept different valid
/// source card pairs. Source visual work and volunteer activities are not
/// automatically certified. Authored for Stage A-C; tests not run.
/// </summary>
public sealed class UsCcssG1Lesson100SourceOpenCardSelectionTests
{
    private const string Family="usccss.g1.u05.l12.write_addition_equations";
    private static Dictionary<string,int> Pick(int a,int b,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["mode"]=mode
    };

    [Theory]
    [InlineData(18,22,6,40,true)]
    [InlineData(56,41,7,97,false)]
    [InlineData(37,22,8,59,false)]
    [InlineData(18,41,8,59,false)]
    [InlineData(37,18,9,55,true)]
    [InlineData(18,56,9,74,true)]
    [InlineData(22,18,10,40,true)]
    [InlineData(37,56,11,93,true)]
    [InlineData(56,41,11,97,false)]
    public void StudentSourceConditionsAllowAllCorrectPairChoices(
        int a,int b,int mode,int total,bool madeNewTen)
    {
        var p=Pick(a,b,mode);
        var answer=$"A={a};B={b};NEW_TEN={(madeNewTen?"YES":"NO")};TOTAL={total}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,answer));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            $"A={b};B={a};NEW_TEN={(madeNewTen?"YES":"NO")};TOTAL={total}"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            answer.Replace("TOTAL="+total,"TOTAL="+(total+1))));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            answer+";SOURCE_ARTWORK_APPROVED=YES"));
    }

    [Fact]
    public void OpenVerifierAcceptsAnotherValidPairEvenWhenGeneratedExpectedDiffers()
    {
        var noTen=Pick(37,22,8);
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,noTen,
            "A=18;B=41;NEW_TEN=NO;TOTAL=59"));
        var leftover=Pick(37,18,9);
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,leftover,
            "A=18;B=56;NEW_TEN=YES;TOTAL=74"));
        var closest=Pick(37,56,11);
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,closest,
            "A=56;B=41;NEW_TEN=NO;TOTAL=97"));
        var sameClosestOtherDirection=Pick(56,41,11);
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,sameClosestOtherDirection,
            "A=37;B=56;NEW_TEN=YES;TOTAL=93"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,closest,
            "A=37;B=41;NEW_TEN=NO;TOTAL=78"));
    }

    [Theory]
    [InlineData(18,18,8)]
    [InlineData(17,22,6)]
    [InlineData(37,56,7)]
    [InlineData(22,18,8)]
    [InlineData(37,22,9)]
    [InlineData(37,18,10)]
    [InlineData(37,41,11)]
    public void RejectsUnlistedDuplicatedOrWrongCategoryCards(int a,int b,int mode)
    {
        var p=Pick(a,b,mode);
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
    }

    [Theory]
    [InlineData(7,3,12,10)]
    [InlineData(37,3,13,40)]
    [InlineData(7,8,14,15)]
    [InlineData(57,8,15,65)]
    public void OriginalFourMentalWarmupsAreSourceExact(
        int a,int b,int mode,int sum)
    {
        var p=Pick(a,b,mode);
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal($"TOTAL={sum};NEW_TEN=YES",
            UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            $"TOTAL={sum};NEW_TEN=YES"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,
            $"TOTAL={sum};NEW_TEN=NO"));
    }

    [Fact]
    public void AllSixteenModesGenerateWithValidAnswersIncludingRealSourceSums()
    {
        var problems=Enumerable.Range(0,2500).Select(i=>
            UsCcssG1FullGradeUnit5AdditionQuestions.Build(Family,new Random(i*31+23))).ToArray();
        foreach(int mode in Enumerable.Range(0,16))
            Assert.Contains(problems,q=>q.Parameters["mode"]==mode);
        Assert.All(problems,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
        });
        Assert.Contains(problems,q=>q.Parameters["mode"]==4&&q.Prompt.Contains("48+15"));
        Assert.Contains(problems,q=>q.Parameters["mode"]==5&&q.Prompt.Contains("57+36"));
        Assert.All(problems.Where(q=>q.Parameters["mode"]>=6&&q.Parameters["mode"]<=11),
            q=>Assert.Contains("ANY mathematically valid pair",q.Prompt));
    }
}

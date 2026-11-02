using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// 2021 Grade 1 Unit 5 L14 Student Facing named Partner B grouping:
/// Tyler+Priya (30+24) and Lin+Han (18+13). Real donations, packing,
/// partner turns and written justification are not digitally certified.
/// Authored without executing Stage D tests.
/// </summary>
public sealed class UsCcssG1Lesson102OriginalNamedFoodDrivePairsTests
{
    private const string Family="usccss.g1.u05.l14.optional_food_drive_numeric";

    [Fact]
    public void OriginalPartnerAAndBNamedPairsPreserveEveryCanOnce()
    {
        var partnerA=new Dictionary<string,int>{{"a",18},{"b",24},{"mode",0}};
        Assert.Equal("PAIR=42;ALL=85",
            UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,partnerA));
        var partnerB=new Dictionary<string,int>{{"a",30},{"b",24},{"mode",1}};
        const string answer="FIRST_PAIR=54;SECOND_PAIR=31;ALL=85";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,partnerB));
        Assert.Equal(answer,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,partnerB));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,partnerB,answer));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,partnerB,
            "FIRST_PAIR=54;SECOND_PAIR=30;ALL=85"));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,partnerB,
            answer+";REAL_ACTIVITY=YES"));
        partnerB["a"]=18;
        partnerB["b"]=18;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,partnerB));
    }

    [Fact]
    public void WholeSchoolFourBoxesHaveIndependentDisjointGradeWitness()
    {
        int[] source=[18,51,23,13,39,40,8,29,30];
        Assert.Equal(251,source.Sum());
        Assert.True(source.Sum()>3*65);
        int[][] groups=[[0,4,6],[1,3],[2,5],[7,8]];
        Assert.Equal(9,groups.SelectMany(x=>x).Distinct().Count());
        Assert.Equal(9,groups.Sum(x=>x.Length));
        Assert.All(groups,g=>Assert.InRange(g.Sum(i=>source[i]),35,65));
        var p=new Dictionary<string,int>{{"a",0},{"b",0},{"mode",4}};
        Assert.Equal("MIN_BOXES=4",
            UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,"MIN_BOXES=3"));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
            Family,p,"MIN_BOXES=4"));
    }
}

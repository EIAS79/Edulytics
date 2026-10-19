using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent source-bound digital answer invariants for original 2021
/// first-grade Unit 4 L16-L23 and Unit 5 L01-L14, all new work 81-102.
/// These checks cannot substitute for actual Student Facing source activity
/// comparison, physical teacher task or authenticated content approval.
/// </summary>
public sealed class UsCcssG1FullGradeUnit4Unit5IndependentMathOracleTests
{
    private static string Symbol(int a,int b)=>a>b?">":a<b?"<":"=";
    private static string OriginalUnit4Oracle(int index,int a,int b,int c,int mode,
        IReadOnlyDictionary<string,int> sourceParams)
    {
        switch(index)
        {
            case 0:return "SYMBOL="+Symbol(a,b);
            case 1:return mode==3?"ORDER="+string.Join(",",
                new[]{"a","b","c","d","e","f","g","h","i","j"}
                    .Select(key=>sourceParams[key]).Order()):
                "ORDER="+string.Join(",",mode%2==0?
                    new[]{a,b,c}.Order():new[]{a,b,c}.OrderDescending());
            case 2:return mode%2==0?"ORDER="+string.Join(",",new[]{a,b,c}.Order()):
                "SYMBOL="+Symbol(a,b);
            case 3:return $"TENS={a/10-1};ONES={a%10+10}";
            case 4:return $"NUMBER={a};TENS={a/10};ONES={a%10};ALT_TENS={a/10-Math.Min(a/10,mode+1)};ALT_ONES={a%10+10*Math.Min(a/10,mode+1)};WAYS={a/10+1}";
            case 5:return "SYMBOL="+Symbol(a,b);
            case 6:return mode%2==0?"SYMBOL="+Symbol(a,b):$"NUMBER={a}";
            case 7:return $"COUNT={a};GAP={Math.Abs(a-b)}";
            default:throw new InvalidOperationException("Unknown Unit 4 original family");
        }
    }

    private static string OriginalUnit5Oracle(int index,int a,int b,int mode)
    {
        int total=a+b;
        if(index==13)
            return mode switch
            {
                0=>$"PAIR={total};ALL=85",
                1=>$"FIRST_PAIR={total};SECOND_PAIR={85-total};ALL=85",
                2=>$"TOTAL={total};PACK={(total>=35&&total<=65?"YES":"NO")}",
                _=>$"TOTAL={total+13};PACK={(total+13>=35&&total+13<=65?"YES":"NO")}"
            };
        int carry=(a%10+b%10)/10;
        if(index is 4 or 5)
            return $"MOVE={10-a%10};REST={b-(10-a%10)};TOTAL={total}";
        if(index is 8 or 9 or 10 or 11 or 13)
            return $"TENS={total/10};ONES={total%10};TOTAL={total}";
        return $"NEW_TEN={(carry!=0?"YES":"NO")};TOTAL={total}";
    }

    [Fact]
    public void EightUnit4SourceSkillFamiliesIndependentlyProveComparisonAndRegrouping()
    {
        var list=UsCcssG1FullGradeUnit4FinalQuestions.Families;
        Assert.Equal(8,list.Count);
        for(int i=0;i<8;i++)
        for(int seed=0;seed<70;seed++)
        {
            var problem=UsCcssG1FullGradeUnit4FinalQuestions.Build(
                list[i],new Random(4223+i*137+seed*89));
            var p=problem.Parameters;
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(list[i],p));
            var result=OriginalUnit4Oracle(i,p["a"],p["b"],p["c"],p["mode"],p);
            Assert.Equal(result,UsCcssG1FullGradeUnit4FinalQuestions.Solve(list[i],p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(list[i],p,result));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                list[i],p,result+";EXTRA=1"));
        }
    }

    [Fact]
    public void FourteenUnit5SourceSkillFamiliesIndependentlyProvePlaceValueArithmetic()
    {
        var list=UsCcssG1FullGradeUnit5AdditionQuestions.Families;
        Assert.Equal(14,list.Count);
        for(int i=0;i<14;i++)
        for(int seed=0;seed<70;seed++)
        {
            var problem=UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                list[i],new Random(5233+i*137+seed*89));
            var p=problem.Parameters;
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(list[i],p));
            var result=OriginalUnit5Oracle(i,p["a"],p["b"],p["mode"]);
            Assert.Equal(result,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(list[i],p));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(list[i],p,result));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                list[i],p,result+";WRONG=1"));
        }
    }

    [Fact]
    public void OriginalNonstandardPlaceValueSourceExampleIsReallyDifferentNumbers()
    {
        int elena=5*10+32, kiran=7*10+2;
        Assert.Equal(82,elena);
        Assert.Equal(72,kiran);
        const string family="usccss.g1.u04.l21.compare_nonstandard_forms";
        var p=new Dictionary<string,int>{{"a",82},{"b",72},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        Assert.Equal("SYMBOL=>",UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,"SYMBOL=="));
    }
}

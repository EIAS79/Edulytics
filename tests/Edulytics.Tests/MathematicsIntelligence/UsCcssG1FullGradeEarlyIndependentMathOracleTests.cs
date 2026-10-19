using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent canonical-result oracles for all 29 source-based digital
/// Grade 1 candidates U03 L14-L27 (excluding authored-only L28) and
/// U04 L01-L15, original 2021 learner learning goals.
/// No previous ordinal 1–50 review, and no claim of human/classroom proof.
/// </summary>
public sealed class UsCcssG1FullGradeEarlyIndependentMathOracleTests
{
    private static string FirstFive(int index,int a,int b,int c,int mode)
    {
        int sum=a+b+c;
        return index switch
        {
            0=>mode<=1?$"ANSWER={(mode==0?a+b:a-b)}":$"CARDS={b},{c}",
            1=>$"TOTAL={sum};EQUATION={a}+{b}+{c}={sum}",
            2=>mode<=1?$"TRUE={(mode==0?"YES":"NO")};TOTAL={sum}":$"MATCH=10+{c};TOTAL={sum}",
            3=>$"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
            4=>mode<=1?$"EQUAL={(mode==0?"YES":"NO")};TOTAL={a+b}":
                mode==2?$"MATCH=10+{a+b-10};TOTAL={a+b}":
                $"PAIR={a+1}+{b-1};TOTAL={a+b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string NextFive(int index,int a,int b,int c,int mode)
    {
        int sum=a+b+c;
        return index switch
        {
            0=>$"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
            1=>$"TOTAL={sum};EQUATION={a}+{b}+{c}={sum}",
            2=>mode%2==0?
                $"TOTAL={a+b};EQUATION={a}+{b}={a+b}":
                $"TOTAL={sum};EQUATION={a}+{b}+{c}={sum}",
            3=>$"REACH10={a-10};REST={b-(a-10)};DIFFERENCE={a-b}",
            4=>$"TO10={a-10};REST={b-(a-10)};DIFFERENCE={a-b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string StrategyAndTens(int index,int a,int b,int mode)
    {
        int sum=a+b;
        return index switch
        {
            0 or 1=>$"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            2=>mode switch
            {
                0=>$"UNKNOWN={sum};EQUATION={a}+{b}={sum}",
                1=>$"UNKNOWN={b};EQUATION={a}+{b}={sum}",
                2=>$"UNKNOWN={a};EQUATION={a}+{b}={sum}",
                3=>$"UNKNOWN={a};EQUATION={sum}-{b}={a}",
                4=>$"UNKNOWN={b};EQUATION={sum}-{b}={a}",
                _=>$"UNKNOWN={sum};EQUATION={sum}-{b}={a}"
            },
            3=>$"RESULT={(mode%2==0?sum:a-b)}",
            4=>$"GROUPS={a};TOTAL={a*10}",
            5=>mode==3
                // Independent source match: A/B/C corresponds to the
                // tens-word option at index (number of tens modulo three).
                ?$"MATCH={(char)('A'+a%3)};TENS={a};NUMBER={10*a}"
                :$"TENS={a};NUMBER={10*a}",
            6=>mode switch
            {
                0=>$"TENS={a+1};TOTAL={10*(a+1)}",
                1=>$"TENS={a-1};TOTAL={10*(a-1)}",
                2=>$"TOTALS={string.Join(",",Enumerable.Range(2,8).Select(t=>10*t))};LAST=90",
                _=>$"TOTALS={string.Join(",",Enumerable.Range(1,8).Reverse().Select(t=>10*t))};LAST=10"
            },
            7=>$"TENS={(mode%2==0?sum:a-b)};TOTAL={10*(mode%2==0?sum:a-b)}",
            8=>mode switch
            {
                0=>$"RESULT={10*sum}",
                1=>$"RESULT={10*(a-b)}",
                2=>$"RESULT={sum}",
                3=>$"RESULT={a-b}",
                4=>"MOVE=SUBTRACT;COVER=10;EQUATION=50-40=10",
                _=>$"MOVE=ADD;COVER={10*sum};EQUATION={10*a}+{10*b}={10*sum}"
            },
            _=>throw new InvalidOperationException()
        };
    }
    private static string NumberName(int t,int o)
    {
        string[] tens=["","ten","twenty","thirty","forty","fifty",
                       "sixty","seventy","eighty","ninety"];
        string[] ones=["","one","two","three","four","five",
                       "six","seven","eight","nine"];
        string[] teen=["ten","eleven","twelve","thirteen","fourteen",
                       "fifteen","sixteen","seventeen","eighteen","nineteen"];
        return t==1?teen[o]:o==0?tens[t]:tens[t]+"-"+ones[o];
    }
    private static string TensOnes(int index,int t,int o,int mode)
    {
        int total=t*10+o;
        return index switch
        {
            0=>$"TENS={t};ONES={o};TOTAL={total}",
            1=>$"NAME={NumberName(t,o)}",
            2=>$"TENS={t};ONES={o};NUMBER={total}",
            3=>$"EXPANDED={t*10}+{o};TENS={t};ONES={o};ALT_TENS={t-Math.Min(t,mode+1)};ALT_ONES={o+10*Math.Min(t,mode+1)}",
            4=>$"NUMBER={total}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string TwoDigit(int index,int a,int b,int mode)
    {
        return index switch
        {
            0=>$"TOTAL={a+b};ONES={a%10}",
            1=>$"RESULT={(mode%2==0?a+10:a-10)}",
            2=>$"RESULT={(mode==0?a+b:mode==1?a-b:mode==2?a+b:10*a+b)}",
            3=>$"RELATION={(a>b?"GREATER":a<b?"LESS":"EQUAL")}",
            4=>$"SYMBOL={(a>b?">":a<b?"<":"=")}",
            _=>throw new InvalidOperationException()
        };
    }
    [Fact]
    public void FirstTenNewOriginalThreeGroupAndSubtractionPracticeMatchesIndependentKeys()
    {
        Assert.Equal(5,UsCcssG1FullGradeStartQuestions.Families.Count);
        Assert.Equal(5,UsCcssG1FullGradeNextQuestions.Families.Count);
        for(int i=0;i<10;i++)
        for(int seed=0;seed<75;seed++)
        {
            var random=new Random(3100+i*173+seed*37);
            if(i<5)
            {
                var f=UsCcssG1FullGradeStartQuestions.Families[i];
                var p=UsCcssG1FullGradeStartQuestions.Build(f,random).Parameters;
                var want=FirstFive(i,p["a"],p["b"],p["c"],p["mode"]);
                Assert.Equal(want,UsCcssG1FullGradeStartQuestions.Solve(f,p));
                Assert.True(UsCcssG1FullGradeStartQuestions.Verify(f,p,want));
                Assert.False(UsCcssG1FullGradeStartQuestions.Verify(f,p,want+";BAD=1"));
            }
            else
            {
                var f=UsCcssG1FullGradeNextQuestions.Families[i-5];
                var p=UsCcssG1FullGradeNextQuestions.Build(f,random).Parameters;
                var want=NextFive(i-5,p["a"],p["b"],p["c"],p["mode"]);
                Assert.Equal(want,UsCcssG1FullGradeNextQuestions.Solve(f,p));
                Assert.True(UsCcssG1FullGradeNextQuestions.Verify(f,p,want));
                Assert.False(UsCcssG1FullGradeNextQuestions.Verify(f,p,want+";BAD=1"));
            }
        }
    }
    [Fact]
    public void NineOriginalStoryUnknownAndTensFamiliesMatchIndependentKeys()
    {
        var families=UsCcssG1FullGradeStrategyAndTensQuestions.Families;
        Assert.Equal(9,families.Count);
        for(int i=0;i<families.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var f=families[i];
            var p=UsCcssG1FullGradeStrategyAndTensQuestions.Build(
                f,new Random(3400+i*173+seed*37)).Parameters;
            var want=StrategyAndTens(i,p["a"],p["b"],p["mode"]);
            Assert.Equal(want,UsCcssG1FullGradeStrategyAndTensQuestions.Solve(f,p));
            Assert.True(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(f,p,want));
            Assert.False(UsCcssG1FullGradeStrategyAndTensQuestions.Verify(f,p,want+";BAD=1"));
        }
    }
    [Fact]
    public void FiveOriginalTensAndOnesRepresentationsMatchIndependentPlaceValue()
    {
        var families=UsCcssG1FullGradeTensAndOnesQuestions.Families;
        Assert.Equal(5,families.Count);
        for(int i=0;i<families.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var f=families[i];
            var p=UsCcssG1FullGradeTensAndOnesQuestions.Build(
                f,new Random(3600+i*173+seed*37)).Parameters;
            var want=TensOnes(i,p["tens"],p["ones"],p["mode"]);
            Assert.Equal(want,UsCcssG1FullGradeTensAndOnesQuestions.Solve(f,p));
            Assert.True(UsCcssG1FullGradeTensAndOnesQuestions.Verify(f,p,want));
            Assert.False(UsCcssG1FullGradeTensAndOnesQuestions.Verify(f,p,want+";BAD=1"));
        }
    }
    [Fact]
    public void FiveOriginalTwoDigitCompareAndAddFamiliesMatchIndependentKeys()
    {
        var families=UsCcssG1FullGradeTwoDigitOperationsQuestions.Families;
        Assert.Equal(5,families.Count);
        for(int i=0;i<families.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var f=families[i];
            var p=UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(
                f,new Random(3800+i*173+seed*37)).Parameters;
            var want=TwoDigit(i,p["a"],p["b"],p["mode"]);
            Assert.Equal(want,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,p));
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(f,p,want));
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(f,p,want+";BAD=1"));
        }
    }
}

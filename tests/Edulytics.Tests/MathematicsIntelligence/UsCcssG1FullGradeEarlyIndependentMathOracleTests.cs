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
            2=>mode<=1?$"TRUE={(mode==0?"YES":"NO")};TOTAL={sum}":
                mode==2?$"MATCH=10+{c};TOTAL={sum}":
                $"WRITE=10+{c};TOTAL={sum}",
            3=>mode==2?$"EQUATION=9+{b}={9+b};TEN_EQUATION=10+{b-1}={9+b};TOTAL={9+b}":
                $"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
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
            0=>mode==2?"LIN=8+2+5=15;HAN=7+7+1=15;KIRAN=8+8-1=15;TOTAL=15":
                $"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
            1=>$"TOTAL={sum};EQUATION={a}+{b}+{c}={sum}",
            2=>mode==1?
                $"TOTAL={sum};EQUATION={a}+{b}+{c}={sum}":
                $"TOTAL={a+b};EQUATION={a}+{b}={a+b}",
            3=>b<a-10?$"DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}":
                $"REACH10={a-10};REST={b-(a-10)};DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}",
            4=>b<a-10?$"DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}":
                $"TO10={a-10};REST={b-(a-10)};DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string StrategyAndTens(int index,int a,int b,int c,int mode)
    {
        int sum=a+b;
        return index switch
        {
            0=>mode==4?(a==14?"UNKNOWN=8;EQUATION=8=14-6":
                        $"UNKNOWN={a-b};EQUATION={a}-{b}={a-b}"):
                mode==5?(a==18?"UNKNOWN=13;EQUATION=5+13=18":
                         a==13?"UNKNOWN=7;EQUATION=13=7+6":"UNKNOWN=5;EQUATION=20=15+5"):
                mode==2?$"UNKNOWN={a-b};EQUATION={a}-{b}={a-b}":
                mode==3?$"UNKNOWN={a-b};EQUATION={a-b}+{b}={a}":
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            1=>mode==5?"UNKNOWN=7;EQUATION=8+7=15":
                mode==6?"UNKNOWN=8;EQUATION=17-8=9":
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            2=>mode switch
            {
                0=>$"UNKNOWN={sum};EQUATION={a}+{b}={sum}",
                1=>$"UNKNOWN={b};EQUATION={a}+{b}={sum}",
                2=>$"UNKNOWN={a};EQUATION={a}+{b}={sum}",
                3=>$"UNKNOWN={a};EQUATION={sum}-{b}={a}",
                4=>$"UNKNOWN={b};EQUATION={sum}-{b}={a}",
                6=>"UNKNOWN=6;EQUATION=9+6=15",
                7=>"UNKNOWN=6;EQUATION=15-9=6",
                _=>$"UNKNOWN={sum};EQUATION={sum}-{b}={a}"
            },
            3=>mode==0?$"RESULT={20-a-b};EQUATION=20-{a}-{b}={20-a-b}":
                mode==2||mode==5?$"RESULT={20-a-b-c};EQUATION=20-{a}-{b}-{c}={20-a-b-c}":
                mode==4?"LEFT=15;RIGHT=13;GREATER=LEFT":
                $"RESULT={(mode==1?a+b:a-b)}",
            4=>$"GROUPS={a};TOTAL={a*10}",
            5=>mode==4?"TENS=7;NUMBER=70;WORD=SEVENTY":
                mode==3?$"MATCH={(char)('A'+a%3)};TENS={a};NUMBER={10*a}"
                :$"TENS={a};NUMBER={10*a}",
            6=>mode switch
            {
                0=>$"TENS={a+1};TOTAL={10*(a+1)}",
                1=>$"TENS={a-1};TOTAL={10*(a-1)}",
                2=>$"TOTALS={string.Join(",",Enumerable.Range(2,8).Select(t=>10*t))};LAST=90",
                3=>$"TOTALS={string.Join(",",Enumerable.Range(1,8).Reverse().Select(t=>10*t))};LAST=10",
                4=>"ADD=70;SUBTRACT=50",
                _=>throw new InvalidOperationException("Unknown ten tower source mode")
            },
            7=>mode==6?"ADDITION=80;SUBTRACTION=50":
                $"TENS={(mode%2==0?sum:a-b)};TOTAL={10*(mode%2==0?sum:a-b)}",
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
            0=>mode==5?"DECISION=DISAGREE;TENS=4;ONES=8;TOTAL=48;CLAIM=50":
                $"TENS={t};ONES={o};TOTAL={total}",
            1=>mode==4?"DIGITS=3,5;NUMBER=35;TENS=3;ONES=5;NAME=thirty-five":
                mode==5?"AGREE=DIEGO;TENS=8;ONES=6;NUMBER=86;NAME=eighty-six":
                mode==6?"DECISION=DISAGREE;DRAWN=42;CLAIM=24;TENS=4;ONES=2":
                $"NAME={NumberName(t,o)}",
            2=>mode==4?"MATCH=YES;TENS=7;ONES=3;NUMBER=73;EXPANDED=70+3":
                mode==5?"MATCH=NO;FIRST=41;OTHER=14":
                mode==6?"MATCH=J,S;NUMBER=32;OTHER=23":
                mode==7?"TENS=9;ONES=5;NUMBER=95;EXPANDED=90+5":
                $"TENS={t};ONES={o};NUMBER={total}",
            3=>$"EXPANDED={t*10}+{o};TENS={t};ONES={o};ALT_TENS={t-Math.Min(t,mode+1)};ALT_ONES={o+10*Math.Min(t,mode+1)}",
            4=>mode==5?"NUMBER=7;TENS=0;ONES=7":
                mode==6?"DECISION=DISAGREE;CLAIM=702;NUMBER=72;TENS=7;ONES=2":
                mode==7?"TENS=8;ONES=3;NUMBER=83;EXPANDED=80+3":
                $"NUMBER={total}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string TwoDigit(int index,int a,int b,int mode)
    {
        return index switch
        {
            0=>mode switch
            {
                5=>"TRUE=YES;LEFT=85;RIGHT=85",
                6=>"TRUE=NO;LEFT=71;RIGHT=81",
                7=>"TRUE=NO;LEFT=26;RIGHT=36",
                9=>"DIGIT=5;EQUATION=45+20=65;TRUE=YES",
                10=>"TRUE=NO;MAX=69;TARGET=75",
                11=>"DIGIT=4;EQUATION=56+40=96;TRUE=YES",
                12=>"TRUE=NO;ONES_LEFT=3;ONES_RIGHT=4",
                _=>$"TOTAL={a+b};ONES={(a+b)%10}"
            },
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
            var want=StrategyAndTens(i,p["a"],p["b"],p["c"],p["mode"]);
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

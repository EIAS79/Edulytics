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
            case 0:return mode switch
            {
                9=>"NUMBER=79",10=>"NUMBER=40",11=>"NUMBER=13",
                12=>"LEFT=12;RIGHT=21",13=>"LEFT=21;RIGHT=12",
                14=>"SYMBOL=>;TENS_LEFT=6;TENS_RIGHT=3",
                _=>"SYMBOL="+Symbol(a,b)
            };
            case 1:return mode>=5?
                mode==5?"NUMBER=26":mode==6?"NUMBER=24":
                mode==7?"ORDER=15,33,51,53":"ORDER=4,34,40,43":
                mode>=3?"ORDER="+string.Join(",",
                new[]{"a","b","c","d","e","f","g","h","i","j"}
                    .Select(key=>sourceParams[key]).Order()):
                "ORDER="+string.Join(",",mode%2==0?
                    new[]{a,b,c}.Order():new[]{a,b,c}.OrderDescending());
            case 2:return mode>=4?$"RESULT={(mode==4?42:mode==7?12:52)}":
                mode%2==0?"ORDER="+string.Join(",",new[]{a,b,c}.Order()):
                "SYMBOL="+Symbol(a,b);
            case 3:return mode==4?"PAIR1=36+1;PAIR2=35+2":
                mode==0?"TENS=5;ONES=15":
                mode==1?"TENS=3;ONES=7;ALT_TENS=2;ALT_ONES=17":
                $"TENS={a/10-1};ONES={a%10+10}";
            case 4:return mode switch
            {
                4=>"NUMBER=52",
                5=>"NUMBER=65",
                6=>"TENS=2",
                7=>"ONES=16",
                _=>$"NUMBER={a};TENS={a/10};ONES={a%10};ALT_TENS={a/10-Math.Min(a/10,mode+1)};ALT_ONES={a%10+10*Math.Min(a/10,mode+1)};WAYS={a/10+1}"
            };
            case 5:return mode>=10?$"RESULT={a+b}":"SYMBOL="+Symbol(a,b);
            case 6:return mode>=4?$"TRUE={(mode==5?"NO":"YES")};LEFT={a};RIGHT={b}":
                mode%2==0?"SYMBOL="+Symbol(a,b):$"NUMBER={a}";
            case 7:return mode>=4?
                $"COUNT={a};CATEGORY={(a<20?"LESS_THAN_20":a<=50?"TWENTY_TO_FIFTY":"MORE_THAN_50")};TENS={a/10};ONES={a%10}":
                $"COUNT={a};GAP={Math.Abs(a-b)}";
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
                3=>$"TOTAL={total+13};PACK={(total+13>=35&&total+13<=65?"YES":"NO")}",
                4=>"MIN_BOXES=4",
                _=>throw new InvalidOperationException("Unknown Food Drive source mode")
            };
        bool carries=a%10+b%10>=10;
        // Independent arithmetic expectations by original source-mode family.
        if(index==1&&mode==6)
            return "ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88";
        if(index==2&&mode==10)
            return "LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=86";
        if(index==3&&mode==4)return "SUM=37;MORE=PRIYA;DIFFERENCE=9";
        if(index==5&&mode is >=4 and <=11)return $"MISSING={b};TOTAL={total}";
        if(index==5&&mode>=12)return $"TOTAL={total};NEW_TEN={(carries?"YES":"NO")}";
        if(index is 1 or 2 && mode>=4)
            return $"TENS={(a/10+b/10)*10};ONES={a%10+b%10};TOTAL={total}";
        if(index==8&&mode==8)return "TENS=5;ONES=3;TOTAL=53";
        if(index==9&&mode>=9)
            return $"TENS_PART={(a/10+b/10)*10};ONES_PART={a%10+b%10};NEW_TENS={total/10};ONES={total%10};TOTAL={total}";
        if(index==10&&mode is >=6 and <=8)
        {
            int sourceStated=mode switch{6=>54,7=>47,8=>45,_=>throw new InvalidOperationException()};
            return $"TRUE={(total==sourceStated?"YES":"NO")};ACTUAL={total};STATED={sourceStated}";
        }
        if(index==10&&mode==9)
            return "JADA_TENS=50;JADA_ONES=12;KIRAN_FIRST=42;KIRAN_SECOND=62;TYLER_FIRST=57;TYLER_SECOND=62;TOTAL=62";
        if(index==11&&mode is >=6 and <=11)
            return $"A={a};B={b};NEW_TEN={(carries?"YES":"NO")};TOTAL={total}";
        if(index==11&&mode>=12)return $"TOTAL={total};NEW_TEN={(carries?"YES":"NO")}";
        if(index==12&&mode>=4)
            return $"TENS={total/10};ONES={total%10};NEW_TEN={(carries?"YES":"NO")};TOTAL={total}";
        if(index is 7 or 8 && mode>=4)
            return $"NEW_TEN={(carries?"YES":"NO")};TOTAL={total}";
        if(index==6&&mode>=6)
            return $"NEW_TEN={(carries?"YES":"NO")};TOTAL={total}";
        // Four original 2021 water-smudged open tasks, preserving the
        // exact pictured bases: 32, 16, 8 and 8, and one/two-digit answer domains.
        if(index==6&&mode>=2)
        {
            Assert.Equal(mode==2?32:mode==3?16:8,a);
            if(mode<=3)Assert.InRange(b,0,9);
            else Assert.InRange(b,10,91);
            bool actuallyComposes=a%10+b%10>=10;
            Assert.Equal(mode==2||mode==4,actuallyComposes);
            Assert.InRange(total,0,99);
            return $"MISSING={b};NEW_TEN={(actuallyComposes?"YES":"NO")};TOTAL={total}";
        }
        int carry=(a%10+b%10)/10;
        if(index==5&&mode==2)
            return "MOVE=6;REST=3;TOTAL=43;ELENA=34+6+3=43;ANDRE=9+1+33=43";
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

using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independently derived answer invariants for every registered digital
/// Grade 1 U06 measurement, U07 geometry/time, and U08 review family.
/// Comparison to a separately computed mathematical expectation is stronger
/// than re-submitting the same engine's generated CorrectAnswer, but cannot
/// certify historical classroom tasks or full academic acceptance.
/// All of these are new Grade 1 lessons; historical 1–50 excluded.
/// </summary>
public sealed class UsCcssG1FullGradeSourceIndependentMathOracleTests
{
    private static string Relation(int a,int b,string larger,string smaller) =>
        a>b?larger:a<b?smaller:"EQUAL";

    private static string SourceMeasurementOracle(int i,int a,int b,int c,int mode)
    {
        return i switch
        {
            0=>mode%2==0?$"LONGER={Relation(a,b,"RED","BLUE")};DIFFERENCE={Math.Abs(a-b)}":
                "ORDER="+string.Join(",",
                    (mode==1
                        ?new[]{(Length:a,Label:"A"),(Length:b,Label:"B"),(Length:c,Label:"C")}.OrderBy(x=>x.Length)
                        :new[]{(Length:a,Label:"A"),(Length:b,Label:"B"),(Length:c,Label:"C")}.OrderByDescending(x=>x.Length))
                    .Select(x=>x.Label)),
            1=>$"LONGER={Relation(a,b,"A","B")}",
            2=>mode switch
            {
                0=>$"LONGER={Relation(a,b,"A","B")}",
                1=>$"SHORTER={Relation(a,b,"CLARE","MAI")}",
                _=>$"FITS={(b<=a?"YES":"NO")}"
            },
            3=>mode%2==0?$"RESULT={a+b}":
                $"RELATION={Relation((a%8+1)+(b%8+1),(a/10%8+1)+(b/10%8+1),"GREATER","LESS")}",
            4=>$"LENGTH={a}",
            5=>mode==4?"VALID=YES":mode is 5 or 6 or 7?"VALID=NO":$"LENGTH={a}",
            6=>mode==4?$"SMALL_UNITS={a};MEDIUM_UNITS={a/2};LARGE_UNITS={a/3}":
                $"SMALL_UNITS={a*2}",
            7=>$"TENS={a/10};ONES={a%10};TOTAL={a}",
            8=>$"NUMBER={a}",
            9=>mode==4?
                $"ACTUAL={a};ESTIMATE={b};DIFFERENCE={Math.Abs(a-b)};DIRECTION={(b>a?"HIGH":b<a?"LOW":"EXACT")}":
                mode%2==0?$"RESULT={a+b}":$"LENGTH={a}",
            10=>mode switch {4=>$"TOTAL={a+b}",5=>$"TOTAL={a+b+c}",6=>$"DIFFERENCE={a-b}",7=>$"ELENA={b-a}",_=>$"DIFFERENCE={Math.Abs(a-b)}"},
            11=>$"LENGTH={(mode%2==0?a-b:a+b)};DIFFERENCE={b}",
            12=>mode==4?$"START={9+11}":
                mode%3==0?$"REMAINING={a-b}":
                mode%3==1?$"REMOVED={b}":$"START={a}",
            // Independently compute the unknowns from each documented 2021
            // picture/stamp/sticker story rather than invoking the engine.
            13=>mode switch
            {
                0=>$"OP=SUBTRACT;TOTAL={a-b}",
                1=>$"OP=ADD;TOTAL={a}",
                2=>$"MATCH=A,C;TOTAL={a}",
                3=>$"MATCH=B,C;TOTAL={a}",
                4 or 5 or 6 or 8=>$"MATCH=A,C;UNKNOWN={a-b}",
                7=>$"MATCH=A,C;TOTAL={b+5}",
                9=>$"MATCH=A,C;UNKNOWN={b+5}",
                _=>throw new InvalidOperationException("Invalid source story mode")
            },
            14=>mode==4?$"START={12+7};EQUATION={12+7}-12=7":
                mode%2==0?$"EQUATION={a}-{b}={a-b}":$"EQUATION={a-b}+{b}={a}",
            15=>mode==4?$"TENS={(a-a%10)/10};ONES={a%10};TOTAL={a}":
                mode==5?$"FIRST={65+20+1};SECOND={65+21};EQUAL={(65+20+1==65+21?"YES":"NO")}":
                mode%2==0?$"RESULT={a+b}":$"LENGTH={a}",
            16=>$"DIFFERENCE={b}",
            _=>throw new InvalidOperationException("Unsupported source measurement family")
        };
    }

    private static string ClockWord(int h)=>new[]
    {"","one","two","three","four","five","six","seven","eight","nine","ten","eleven","twelve"}[h];
    private static string SourceGeometryOracle(int i,int a,int b,int mode)
    {
        return i switch
        {
            0=>$"SHAPE={(mode==0?"CUBE":mode==1?"SPHERE":mode==2?"CYLINDER":"CONE")}",
            1=>mode==4?$"PER_LAYER={a*b};LAYERS={2};TOTAL_CUBES={(a*b)+(a*b)}":
                $"PIECES={a+b}",
            2=>mode==4?"CURVED=CIRCLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE":
                $"SHAPE={(mode%2==0?"TRIANGLE":"SQUARE")}",
            3=>$"SIDES={(mode%2==0?3:4)};CORNERS={(mode%2==0?3:4)}",
            4=>mode==4?"TRIANGLES=A,B,C;NONTRIANGLES=D,E,F":"SIDES=3",
            5=>mode switch
            {
                4=>"RECTANGLE=YES;SQUARE=YES",
                5=>"RECTANGLE=YES;SQUARE=NO",
                6=>"RECTANGLE=NO;SQUARE=NO",
                _=>$"ANSWER={(mode%2==0?"YES":"NO")}"
            },
            6=>mode==4?$"TRIANGLES={2*3};TRAPEZOIDS={6/3};SAME_OUTLINE=YES":"PARTS=2",
            7=>mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            8=>mode%2==0?"PART=HALF;PIECES=2":"PART=FOURTH;PIECES=4",
            9=>mode<=1?(mode==0?"PARTS=2;WHOLES=1":"PARTS=4;WHOLES=1"):
                (mode==2?"SORT=UNEQUAL;PIECES=2":"SORT=EQUAL;PIECES=4"),
            10=>"LARGER=HALF;FOURTHS_PER_HALF=2",
            11=>mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            12=>mode<=1?$"TIME={a}:00":$"SAME={(mode==2?"YES":"NO")};TIME={a}:00",
            13=>mode<=1?$"MINUTES=30;HOUR={a}":
                $"COLOR={(mode==2?"BLUE":"RED")};TIME={a}:{(mode==2?"00":"30")};WORDS={(mode==2?ClockWord(a)+"_oclock":"half_past_"+ClockWord(a))}",
            14=>mode<=1?$"TIME={a}:{(mode==0?"00":"30")}":"HOPS=6;MINUTES=30;STOP_AT=6",
            15=>$"TIME={a}:{(mode%2==0?"00":"30")}",
            16=>mode%2==0?$"RESULT={a}":"SHAPE=RECTANGLE",
            _=>throw new InvalidOperationException("Unsupported source geometry/time family")
        };
    }

    private static string SourceReviewOracle(int i,int a,int b,int mode)
    {
        return i switch
        {
            0=>$"RESULT={a+b}",
            1=>mode<=1?$"LEFT={a};WHOLE={a+b}":
                mode==2?$"ADD={a}+{b}={a+b};SUB={a+b}-{b}={a}":
                $"MISSING_ADD={b};MISSING_SUB={a}",
            2=>$"RESULT={(mode%2==0?a+b:a-b)}",
            3=>mode%2==0?$"CHANGE={b};FINAL={a+b}":$"CHANGE={b};FINAL={a}",
            4=>$"MISSING={b};TOTAL={a+b}",
            5=>$"DIFFERENCE={a-b}",
            6=>mode<=1?$"LAST={a+b}":$"TENS={a/10};ONES={a%10};TOTAL={a}",
            7=>mode<=1?$"NEW_TENS={a/10-1};NEW_ONES={a%10+10};TOTAL={a}":
                mode==2?$"ALT_TENS={a/10-2};ALT_ONES={a%10+20};TOTAL={a}":
                $"WAYS={a/10+1};ZERO_TENS_ONES={a};TOTAL={a}",
            8=>mode%2==0?$"CANDIDATES={a},{a+10},{a+20}":$"NUMBER={a+10}",
            9=>$"NUMBER={a};VALID={(mode%2==0?"YES":"NO")}",
            _=>throw new InvalidOperationException("Unsupported source review family")
        };
    }

    [Fact]
    public void EveryOriginalMeasurementDigitalFamilyMatchesIndependentMathOracle()
    {
        var list=UsCcssG1FullGradeUnit6MeasurementQuestions.Families;
        Assert.Equal(17,list.Count);
        for(int i=0;i<list.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var p=UsCcssG1FullGradeUnit6MeasurementQuestions
                .Build(list[i],new Random(7100+seed*67+i*101)).Parameters;
            int a=p["a"],b=p["b"],mode=p["mode"];
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(list[i],p));
            var expected=SourceMeasurementOracle(i,a,b,p["c"],mode);
            Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(list[i],p));
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(list[i],p,expected));
            Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(list[i],p,expected+";FALSE=1"));
        }
    }

    [Fact]
    public void EveryOriginalGeometryTimeDigitalFamilyMatchesIndependentMathOracle()
    {
        var list=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families;
        Assert.Equal(17,list.Count);
        for(int i=0;i<list.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var p=UsCcssG1FullGradeUnit7GeometryTimeQuestions
                .Build(list[i],new Random(7200+seed*67+i*101)).Parameters;
            int a=p["a"],b=p["b"],mode=p["mode"];
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(list[i],p));
            var expected=SourceGeometryOracle(i,a,b,mode);
            Assert.Equal(expected,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(list[i],p));
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(list[i],p,expected));
            Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(list[i],p,expected+";FALSE=1"));
        }
    }

    [Fact]
    public void EveryOriginalReviewDigitalFamilyMatchesIndependentMathOracle()
    {
        var list=UsCcssG1FullGradeUnit8ReviewQuestions.Families;
        Assert.Equal(10,list.Count);
        for(int i=0;i<list.Count;i++)
        for(int seed=0;seed<75;seed++)
        {
            var p=UsCcssG1FullGradeUnit8ReviewQuestions
                .Build(list[i],new Random(7300+seed*67+i*101)).Parameters;
            int a=p["a"],b=p["b"],mode=p["mode"];
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(list[i],p));
            var expected=SourceReviewOracle(i,a,b,mode);
            Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(list[i],p));
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(list[i],p,expected));
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(list[i],p,expected+";FALSE=1"));
        }
    }

    [Fact]
    public void ClockAndNewRiddleHaveOriginalStudentFacingIndependentEdgeCases()
    {
        // Clock face: hour 12 at noon/midnight and correct 12:30.
        var clock=new Dictionary<string,int>{{"a",12},{"b",2},{"c",0},{"mode",1}};
        Assert.Equal("TIME=12:30",SourceGeometryOracle(14,clock["a"],clock["b"],clock["mode"]));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(
            UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families[14],clock,"TIME=12:30"));
        // IM 2021 authoring: secret number may be 120, and last addition
        // expression can be deliberately wrong for critical reading.
        var trueRiddle=new Dictionary<string,int>{{"a",120},{"b",31},{"c",0},{"mode",0}};
        var falseRiddle=new Dictionary<string,int>{{"a",120},{"b",31},{"c",0},{"mode",1}};
        const string family="usccss.g1.u08.l10.write_riddle_numeric_check";
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,trueRiddle));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,trueRiddle,"NUMBER=120;VALID=YES"));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,falseRiddle,"NUMBER=120;VALID=NO"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,falseRiddle,"NUMBER=120;VALID=YES"));
    }
}

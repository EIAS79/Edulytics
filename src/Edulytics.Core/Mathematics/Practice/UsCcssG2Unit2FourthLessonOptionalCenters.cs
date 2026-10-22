using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Exact number talk and first-party numeric turn checks, never a
/// counterfeit claim of real Capture Squares/Five-in-a-Row board completion.</summary>
public static class UsCcssG2Unit2FourthLessonOptionalCenters
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L04";
    public const string Version="edulytics-g2-u02-l04-optional-two-centers-v1";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe four mental subtractions and students' tens-versus-ones justification",
        "Observe REAL Capture Squares pair: spinner6–10 or valid WILD, card0–10, exact within20 sum",
        "Verify every drawn line connects adjacent dots along the correct board target square",
        "Capture a square ONLY on its fourth side; first to capture three squares wins",
        "Observe REAL Five in a Row Stage6: two number clips, only one moved per subsequent turn",
        "Check sums within100 including necessary COMPOSING of a ten, and win only with five continuous own-colored counters",
        "First-party digital example math never certifies authentic board state, pair play or winner"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int lo,int hi,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=lo&&value<=hi;
    public static Review CheckNumberTalk(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==3&&N(b,0,100,out var y)&&y==63
            &&N(c,0,100,out var z)&&z==36&&N(d,0,100,out var w)&&w==33;
        return new(ok,true,ok?"Four answers correct (3,63,36,33). Teacher evaluates the mental tens/ones reasoning."
            :"Check 6−3=3;66−3=63;66−30=36;66−33=33 by place value.");
    }
    public static Review CheckCapture(string? spinner,string? card,string? sum,string? usedWild)
    {
        var ok=N(spinner,6,10,out var s)&&N(card,0,10,out var c)&&
            N(sum,0,20,out var x)&&x==s+c&&(usedWild is "Yes" or "No");
        return new(ok,true,ok?"Valid Stage3 spinner and card sum. Teacher still checks chosen board, adjacent-dot line and actual captured squares."
            :"Select spinner number6–10 (even if WILD), a card0–10, and write their exact sum no larger than20.");
    }
    public static Review CheckFiveTwoTurns(string? topFirst,string? lowerFirst,string? firstSum,
        string? topSecond,string? lowerSecond,string? secondSum)
    {
        var parsed=N(topFirst,0,99,out var a)&&N(lowerFirst,0,99,out var b)&&
            N(firstSum,0,100,out var sum1)&&N(topSecond,0,99,out var c)&&
            N(lowerSecond,0,99,out var d)&&N(secondSum,0,100,out var sum2);
        var changed=parsed&&((a==c&&b!=d)||(a!=c&&b==d));
        var ok=parsed&&a+b==sum1&&c+d==sum2&&sum1<=100&&sum2<=100&&changed;
        return new(ok,true,ok?"Two legal FIRST-PARTY arithmetic turns with exactly one clip moved. Teacher verifies the actual gameboard, player counters and five-in-row."
            :"Calculate each sum and move exactly ONE clip between turns. Never assume a sum automatically scores five-in-row.");
    }
    public static Review CheckIndependentCoolDown(string? captured,string? composing,string? subtract)
    {
        var ok=N(captured,0,20,out var x)&&x==13&&
            N(composing,0,100,out var y)&&y==33&&N(subtract,0,100,out var z)&&z==45;
        return new(ok,false,ok?"Independent Edulytics maths correct:7+6=13,26+7=33 (a ten is composed),75−30=45."
            :"Recheck first-party 7+6,26+7 and75−30. Five-in-a-Row may compose a ten.");
    }
}

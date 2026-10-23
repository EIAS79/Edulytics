using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Checks the FIRST-PARTY numerical portion of IM2021 G2 U02 L05.
/// Physical cubes, diagrams, oral discussion and assessment remain teacher reviewed.</summary>
public static class UsCcssG2Unit2FifthLessonDecomposeTen
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L05";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe original Number Talk reasoning: subtract to a ten before subtracting the rest",
        "Observe authentic 82 cubes/blocks and student-selected subtraction strategy",
        "When using place value, exchange EXACTLY one ten for 10 ones without changing 82",
        "Observe partner construction of Diego's55, removal of two tens, then eight ones",
        "Check original diagram shows 35 as two tens and fifteen ones, not three tens plus fifteen ones",
        "Discuss fixed base-ten blocks versus detachable connecting-cube towers, with MLR8 speaking support",
        "Digital arithmetic does not attest a student's physical blocks, original work or teacher score"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool Read(string? s,int min,int max,out int result)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out result)
        &&result>=min&&result<=max;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=Read(a,0,100,out var a1)&&a1==10&&
            Read(b,0,100,out var b1)&&b1==9&&
            Read(c,0,100,out var c1)&&c1==20&&
            Read(d,0,100,out var d1)&&d1==18;
        return new(ok,true,ok?"Warm-up values10,9,20,18 correct; teacher evaluates mental strategies."
            :"Use17−7=10,17−8=9,26−6=20 and26−8=18.");
    }
    public static Review Activity1(string? answer,string? tradedTens,string? tradedOnes)
    {
        var ok=Read(answer,0,100,out var diff)&&diff==73 &&
            Read(tradedTens,0,9,out var tens)&&tens==7 &&
            Read(tradedOnes,0,19,out var ones)&&ones==12;
        return new(ok,true,ok?"82=7 tens+12 ones;12−9=3, leaving73. Teacher reviews the authentic explanation."
            :"Trade 1 of 8 tens:82=7 tens+12 ones; subtract9 ones and get73.");
    }
    public static Review Activity2(string? start,string? afterTens,string? tensAfterTrade,string? onesAfterTrade,string? end)
    {
        var ok=Read(start,0,100,out var x)&&x==55 &&
            Read(afterTens,0,100,out var y)&&y==35 &&
            Read(tensAfterTrade,0,9,out var t)&&t==2 &&
            Read(onesAfterTrade,0,19,out var o)&&o==15 &&
            Read(end,0,100,out var z)&&z==27;
        return new(ok,true,ok?"Diego:55−20=35=2 tens+15 ones;35−8=27. Teacher checks physical block exchange."
            :"55 starts as5 tens+5 ones; after removing2 tens it is35; trade to2 tens+15 ones and subtract8 to get27.");
    }
    public static Review CoolDown(string? first,string? second,string? third)
    {
        var ok=Read(first,0,100,out var a)&&a==56&&
            Read(second,0,100,out var b)&&b==66&&
            Read(third,0,100,out var c)&&c==36;
        return new(ok,false,ok?"Original Edulytics maths correct:63−7=56;74−8=66;42−6=36."
            :"Try63=50+13,74=60+14 and42=30+12. The publisher's restricted Cool-down is not reproduced.");
    }
}

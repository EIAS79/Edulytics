using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
public static class UsCcssG2Unit3ThirdLessonBuildRuler
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L03";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe genuine source Number Talk 63−3,63−20,63−23,63−24 mental reasoning",
        "Ruler handmade from authentic template and true one-centimeter cubes; each mark0..25 one centimeter apart",
        "Numerals label the actual TICK MARKS, and tick0 represents the beginning of length",
        "Check 0 to10 means10cm; other spans are endpoint minus starting point",
        "Observe real rectangle A–F print sheet and teacher-validated measured lengths, not invented values",
        "Compare original A/B and F/D differences, two longest, and joined total in same unit",
        "Observe actual MLR8 partner talk and checked ruler; typed answers cannot certify real ruler"
    ];
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review NumberTalk(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==60&&N(b,0,100,out var y)&&y==43&&
            N(c,0,100,out var z)&&z==40&&N(d,0,100,out var q)&&q==39;
        return new(ok,true,ok?"Original four Number Talk answers60,43,40,39; teacher checks mental method."
            :"63−3=60,63−20=43,63−23=40,63−24=39.");
    }
    public static Review RulerTickMeaning(string? zero,string? one,string? ten,string? end,string? start,string? length)
    {
        var ok=N(zero,0,25,out var z)&&z==0&&N(one,0,25,out var o)&&o==1&&
            N(ten,0,25,out var t)&&t==10&&N(end,0,25,out var e)&&e==18&&
            N(start,0,25,out var s)&&s==3&&N(length,0,25,out var l)&&l==15;
        return new(ok,true,ok?"Ruler tick0 is origin; 0→1 is1cm, 0→10 is10cm, marks3→18 make15cm."
            :"Tick marks must read0,1,10; distance3→18 is18−3=15cm.");
    }
    public static Review FirstParty(string? longer,string? shorter,string? difference,string? joined)
    {
        var ok=N(longer,0,100,out var l)&&l==23&&N(shorter,0,100,out var s)&&s==17&&
            N(difference,0,100,out var d)&&d==6&&N(joined,0,100,out var j)&&j==19;
        return new(ok,false,ok?"Independent virtual rectangles23−17=6cm and joined11+8=19cm. These are NOT publisher A–F."
            :"Original Edulytics examples:23−17=6cm and11+8=19cm.");
    }
    public static Review Exit(string? length)
    {
        var ok=N(length,0,25,out var n)&&n==17;
        return new(ok,false,ok?"Independently authored ruler interval marks4→21 equals17cm."
            :"To find a span from4 to21, subtract21−4=17cm.");
    }
}

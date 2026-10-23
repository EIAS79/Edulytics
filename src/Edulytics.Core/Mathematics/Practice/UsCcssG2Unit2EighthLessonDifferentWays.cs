using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numerical source checks for G2 U02 L08; original diagrams/discourse teacher-observed.</summary>
public static class UsCcssG2Unit2EighthLessonDifferentWays
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L08";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe original Number Talk strategies and inverse fact-family explanation",
        "Observe actual Lin/Clare drawings of71−56 and why both equal15",
        "Clare subtracts 5 tens before drawing residual21; Lin decomposes from full71 first",
        "Connect source equation sets correctly with regroup-first and tens-first methods",
        "Explain Andre's65−28=37 from65−20=45,45=30+15,15−8=7",
        "Observe source 34−18,82−37 and71−53 student-created diagrams and MLR8 peer explanation",
        "Do not certify authentic student discussion or source cool-down from typed numerical answers"
    ];
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review NumberTalk(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==38&&N(b,0,100,out var y)&&y==48&&
            N(c,0,100,out var z)&&z==18&&N(d,0,100,out var t)&&t==18;
        return new(ok,true,ok?"Original values38,48,18,18 correct; teacher evaluates thinking."
            :"18+10+10=38,18+20+10=48,38−20=18,48−30=18.");
    }
    public static Review LinClare(string? difference,string? residual,string? regroupOnes)
    {
        var ok=N(difference,0,100,out var d)&&d==15&&N(residual,0,100,out var r)&&r==21&&
            N(regroupOnes,0,19,out var o)&&o==11;
        return new(ok,true,ok?"Clare71−50=21=10+11 and11−6=5. Lin71=60+11, both finish15."
            :"Subtract71−56=15; after Clare removes50 remains21=10+11; Lin trades before subtracting tens.");
    }
    public static Review AndreAndPractice(string? andre,string? first,string? second,string? third)
    {
        var ok=N(andre,0,100,out var a)&&a==37&&N(first,0,100,out var b)&&b==16&&
            N(second,0,100,out var c)&&c==45&&N(third,0,100,out var d)&&d==18;
        return new(ok,true,ok?"Andre65−28=37, source tasks34−18=16,82−37=45,71−53=18."
            :"Andre65−20=45,45=30+15,15−8=7; others34−18=16,82−37=45,71−53=18.");
    }
    public static Review Exit(string? a,string? b,string? c)
    {
        var ok=N(a,0,100,out var x)&&x==26&&N(b,0,100,out var y)&&y==26&&
            N(c,0,100,out var z)&&z==36;
        return new(ok,false,ok?"Independent Edulytics exit72−46=26,83−57=26,61−25=36."
            :"Trade ten when needed:72−46=26;83−57=26;61−25=36.");
    }
}

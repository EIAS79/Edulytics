using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
public static class UsCcssG2Unit3SecondLessonMeasureCm
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L02";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Source warm-up images A/B show the labels centimeter and cm for the SAME1cm length",
        "Physically measure the genuine bearded dragon drawing with24 edge-aligned centimeter cubes",
        "Ten-cm rod technique shows two FULL rods plus four additional centimeter cube edges, total24",
        "Observe authentic Reptile Length sheet; day gecko versus threadsnake lengths teacher-measured, not invented",
        "Measure real reptile diagrams with appropriate 1cm cubes and10cm rods and remeasure partner disagreements",
        "MLR8 student explains why ten-cm rods and single cubes are efficient for different lengths",
        "Numerical first-party analogues do not certify physical measurements or source cool-down"
    ];
    private static bool N(string? s,int lo,int hi,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=lo&&value<=hi;
    public static Review BeardedDragon(string? cubes,string? rods,string? extra,string? cm)
    {
        var ok=N(cubes,0,100,out var a)&&a==24&&N(rods,0,10,out var r)&&r==2&&
            N(extra,0,9,out var x)&&x==4&&N(cm,0,100,out var n)&&n==24;
        return new(ok,true,ok?"Source dragon length24cm=24 one-cm cubes=2 ten-cm rods+4 cubes. Teacher reviews genuine alignment."
            :"One 10cm rod=10cm. Dragon24cm requires2 complete rods+4 cubes, or24 cubes.");
    }
    public static Review FirstParty(string? rods,string? cubes,string? compare,string? exitRods,string? exitCubes)
    {
        var ok=N(rods,0,10,out var a)&&a==2&&N(cubes,0,9,out var b)&&b==8&&
            N(compare,0,100,out var c)&&c==15&&
            N(exitRods,0,10,out var d)&&d==3&&N(exitCubes,0,9,out var e)&&e==6;
        return new(ok,false,ok?"Independent Edulytics28cm=2 rods+8 cubes; invented29cm minus14cm difference15;36cm=3 rods+6 cubes."
            :"Original Edulytics examples only:28=20+8,29−14=15,36=30+6.");
    }
}

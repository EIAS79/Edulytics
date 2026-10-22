using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Illustrative repeated-inch counts for source floor tape, not school grade2 conversion requirement.</summary>
public static class UsCcssG2Unit3NinthLessonFishFeet
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L09";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Warm-up actual source fish illustration, boy7 years old and48 inches, estimates high/low/about right NOT fish actual length",
        "Observe teacher-built source floor tape A bass2ft, B spiny dogfish4ft, C catfish3ft, D koi2ft6in",
        "Use actual 0–12 inch segment as one foot and iterate with NO plastic overhang gaps/overlaps",
        "Measure each tape in real feet FIRST, then inch units; record meaningful comparative observations",
        "Explain that larger foot unit gives smaller count than inch unit for SAME physical tape",
        "Grade2 does not need formal abstract foot-inch conversion exercises; true repeat counting is experiential",
        "Inch/foot Activity2 real object estimated before measured with labeled ruler/tape measure and MLR2 peer talk"
    ];
    private static bool N(string? s,int min,int max,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=min&&value<=max;
    public static Review SourceTapeFeet(string? a,string? b,string? c,string? dFeet,string? dExtra)
    {
        var ok=N(a,0,5,out var x)&&x==2&&N(b,0,5,out var y)&&y==4&&
            N(c,0,5,out var z)&&z==3&&N(dFeet,0,5,out var u)&&u==2&&
            N(dExtra,0,11,out var v)&&v==6;
        return new(ok,true,ok?"Teacher source tape targets 2ft,4ft,3ft,2ft6in. Actual laid tape and real measurement need observation."
            :"Strip A2 feet, B4 feet, C3 feet and D2 feet6 inches.");
    }
    public static Review SourceTapeInchIntervals(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,60,out var x)&&x==24&&N(b,0,60,out var y)&&y==48&&
            N(c,0,60,out var z)&&z==36&&N(d,0,60,out var u)&&u==30;
        return new(ok,true,ok?"If floor tapes were accurately laid, repeated one-inch intervals tally24,48,36,30. Real measuring must be teacher verified."
            :"Count consecutive 0–12 inch ruler segments and remaining inches: A24, B48, C36, D30.");
    }
    public static Review FirstParty(string? oneFoot,string? threeFeet,string? mixed)
    {
        var ok=N(oneFoot,0,100,out var a)&&a==12&&N(threeFeet,0,100,out var b)&&b==36&&
            N(mixed,0,100,out var c)&&c==30;
        return new(ok,false,ok?"Illustrative repeat counts: one12-inch segment, three repeated spans36in, two spans+six more30in."
            :"Use physical unit repetition, not an abstract required Grade2 conversion algorithm.");
    }
}

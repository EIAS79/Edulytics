using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Fictional line-plot data checker, not an invented real class measurement.</summary>
public static class UsCcssG2Unit3FourteenthLessonHandSpanLinePlot
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L14";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Display the actual source glove-size bar graph and numeric hand-span line-plot pictures",
        "Student independently traces spread hand, joins thumb and pinky tips and measures nearest inch",
        "Partner checks actual physical inches from ruler zero and one sticky-note X per measurement",
        "Real class 0–10 line plot has equally-spaced ticks, equal-sized Xs and unit label inches",
        "Teacher checks genuine longest, shortest, most frequent hand spans against actual class data",
        "MLR8 discussion compares categories versus measurements and distinguishes counts from lengths",
        "Fictional Edulytics numeric plot does not document authentic class data or source Cool-down"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review FictionalPlot(string? shortest,string? longest,string? mode,string? total)
    {
        var ok=N(shortest,0,10,out var a)&&a==4&&N(longest,0,10,out var b)&&b==8&&
            N(mode,0,10,out var c)&&c==6&&N(total,0,40,out var d)&&d==9;
        return new(ok,false,ok?"Original FICTIONAL plot: shortest4in, longest8in, mode6in, nine measurements."
            :"Read HORIZONTAL length values for shortest/longest/mode; sum all Xs for total.");
    }
    public static Review CountAndRange(string? atSeven,string? range)
    {
        var ok=N(atSeven,0,20,out var a)&&a==2&&N(range,0,10,out var b)&&b==4;
        return new(ok,false,ok?"Fictional 7-inch frequency2; numeric range8−4=4 inches."
            :"At7 inches there are two Xs. Range=longest8−shortest4=4in.");
    }
    public static Review RealMeasurement(string? span)
    {
        var plausible=N(span,0,10,out _);
        return new(plausible,true,plausible?"Whole-inch length within 0–10 recorded; a teacher still checks your real tracing, ruler zero and partner measurement."
            :"Use a real inch ruler to nearest whole inch within original 0–10 class scale; teacher verifies the physical result.");
    }
}

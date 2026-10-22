using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>First-party virtual inch examples are numerical only; real ruler endpoints need a teacher.</summary>
public static class UsCcssG2Unit3EighthLessonWhatIsAnInch
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L08";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Source Notice/Wonder compare genuine inch ruler with centimeter ruler unit sizes",
        "Inch TILE side from real ruler0 to1 equals one inch, not an area measurement",
        "Four students locate TWO genuine classroom objects about one inch long",
        "Ruler zero aligned; measure REAL marker, colored pencil, 11 cubes, book and two chosen objects",
        "Estimate source rectangle, square, longest triangle side BEFORE using physical inch ruler",
        "Triangle side is between5 and6; nearest whole inch depends on real ruler endpoint, NOT asserted source value",
        "MLR8 partner discussion and protected publisher Cool-down are NOT certified or copied digitally"
    ];
    private static bool N(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
    public static Review InchTile(string? side,string? fromZeroToOne)
    {
        var ok=N(side,0,12,out var s)&&s==1&&N(fromZeroToOne,0,12,out var t)&&t==1;
        return new(ok,true,ok?"One inch tile side = length from0 to1 inch mark; teacher validates actual tools."
            :"Each inch tile square SIDE is 1 inch; ruler0-to1 span is also1 inch.");
    }
    public static Review OriginalVirtual(string? fourInchTicks,string? sevenTiles)
    {
        var ok=N(fourInchTicks,0,12,out var a)&&a==4&&N(sevenTiles,0,12,out var b)&&b==7;
        return new(ok,false,ok?"First-party virtual ruler spans4 inches and seven 1-inch tiles span7 inches."
            :"Count each complete equal-size 1-inch interval, beginning at ruler0. No real object was measured.");
    }
    public static Review NearestWholeVirtual(string? belowHalf,string? aboveHalf)
    {
        var ok=N(belowHalf,0,12,out var a)&&a==5&&N(aboveHalf,0,12,out var b)&&b==6;
        return new(ok,false,ok?"Virtual illustrative5.2 inches is about5, and5.8 inches is about6 whole inches."
            :"Choose nearest WHOLE inch in these separate first-party illustrations, not source shapes.");
    }
}

using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numerical comparison of unit lengths; physical ruler and cube measurement teacher-observed.</summary>
public static class UsCcssG2Unit3FirstLessonStandardUnits
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L01";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe genuine original 40-cm prepared string, beginning and end",
        "Half of groups use actual10-cm straws, other half5-cm straws, no gaps or overlaps",
        "Count four10-cm straws and eight5-cm straws without claiming length differs",
        "Check actual one-centimeter cube length edge and count forty cubes along40-cm string",
        "Contrast genuine centimeter cubes with potentially larger connecting cubes",
        "Observe real MLR8 discussion of why identical units standardize a measurement",
        "Digital interval counts are not evidence a child physically measured ribbon or a live iguana"
    ];
    private static bool N(string? s,int lo,int hi,out int value)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out value)&&value>=lo&&value<=hi;
    public static Review SourceUnits(string? longStraws,string? shortStraws,string? cubes)
    {
        var ok=N(longStraws,0,100,out var a)&&a==4&&N(shortStraws,0,100,out var b)&&b==8&&
            N(cubes,0,100,out var c)&&c==40;
        return new(ok,true,ok?"40-cm source string corresponds to4 long10-cm straws,8 short5-cm straws,40 1-cm cubes. Teacher checks real aligned units."
            :"40÷10=4;40÷5=8;40÷1=40. Counts differ only because equal-measurement unit lengths differ.");
    }
    public static Review FirstParty(string? longStraws,string? shortStraws,string? cubes)
    {
        var ok=N(longStraws,0,100,out var a)&&a==3&&N(shortStraws,0,100,out var b)&&b==6&&
            N(cubes,0,100,out var c)&&c==30;
        return new(ok,false,ok?"Original Edulytics30-cm line:3 ten-cm pieces,6 five-cm pieces,30 one-cm cubes."
            :"For first-party30-cm line:30÷10=3,30÷5=6,30÷1=30.");
    }
}

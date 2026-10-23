using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Number-line arithmetic is digital; authentic student diagrams and discussions are educator-reviewed.</summary>
public static class UsCcssG2Unit4FirstLessonNumberLine
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U04:L01";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);

    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Notice and Wonder: compare real publisher number-line image with a 0..12 ruler",
        "First SOURCE 0..10 line has 11 equally spaced ticks; source labels 0,1,2,5,8,9,10",
        "Fill exactly 3,4,6,7 on first original number line; second line labeled 0,5,10 needs 1..4 and 6..9",
        "First original line: a dot directly over 6 and a freely chosen different dot at 7..10",
        "Second original line: a dot directly over 9 and another freely chosen dot at 0..8",
        "Construct a real paper line 0..20, zero anchored with 20 physically equal unit intervals",
        "Mark two real dots at 3 and13 on actual student's paper, not just digital arithmetic",
        "Pupils can choose different object length units, but a single drawn line must use one equal length throughout",
        "Partner explains same/different number lines; original teacher evaluates actual spacing and point alignment",
        "Exact first-party numerical checks never certify physical construction or restricted original publisher Cool-down"
    ];
    private static bool Number(string? s,int minimum,int maximum,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=minimum&&n<=maximum;

    public static Review FirstSourceLine(string? missing3,string? missing4,string? missing6,string? missing7)
    {
        bool valid=Number(missing3,0,10,out var a)&&a==3&&
                   Number(missing4,0,10,out var b)&&b==4&&
                   Number(missing6,0,10,out var c)&&c==6&&
                   Number(missing7,0,10,out var d)&&d==7;
        return new(valid,true,valid
            ?"Source missing labels3,4,6,7 are correct. Educator still checks original diagram and student points."
            :"Count evenly spaced ticks 0,1,2,3,4,5,6,7,8,9,10. Missing labels are3,4,6,7.");
    }
    public static Review SourcePoints(string? atSix,string? greaterThanSix,string? atNine,string? lessThanNine)
    {
        bool valid=Number(atSix,0,10,out var a)&&a==6&&
                   Number(greaterThanSix,0,10,out var b)&&b>6&&
                   Number(atNine,0,10,out var c)&&c==9&&
                   Number(lessThanNine,0,10,out var d)&&d<9;
        return new(valid,true,valid
            ?"Original free choice accepted: more than6 is7..10, and less than9 is0..8. Educator checks position of each physical point."
            :"First line has a point at6 and ANY point7..10; second has a point at9 and ANY point0..8.");
    }
    public static Review IndependentlyAuthored(string? gap3to13,string? zeroTo13,string? ticksZeroTo20)
    {
        bool valid=Number(gap3to13,0,20,out var a)&&a==10&&
                   Number(zeroTo13,0,20,out var b)&&b==13&&
                   Number(ticksZeroTo20,0,21,out var c)&&c==21;
        return new(valid,false,valid
            ?"Independent unit-interval maths correct:13-3=10,0-to-13=13 intervals,0..20 has21 tick marks."
            :"The first-party answers are10 intervals between3 and13,13 from0 to13, and21 ticks from0 through20.");
    }
}

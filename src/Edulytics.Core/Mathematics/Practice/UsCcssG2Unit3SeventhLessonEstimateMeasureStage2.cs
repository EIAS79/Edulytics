using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numeric estimate differences, not proof of hands-on actual object measurement.</summary>
public static class UsCcssG2Unit3SeventhLessonEstimateMeasureStage2
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L07";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe authentic Number Talk mental37−20,37−21,37−17,37−16",
        "Stage2 pairs choose REAL varied-length classroom objects and appropriate centimeter tools",
        "Each genuine partner records an ESTIMATE BEFORE using a ruler, guesses may differ",
        "Teacher checks ruler zero aligns object start and endpoint, equal cm ticks, no gaps/overlap",
        "Check actual measurement in centimeters versus initial estimate and optional absolute difference",
        "Choice time genuine Estimate and Measure Stage2 or Math Stories Stage5 with classroom materials",
        "First-party example data do not imply a physical object was measured or partner played"
    ];
    private static bool Read(string? s,int low,int high,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=low&&n<=high;
    public static Review Talk(string? a,string? b,string? c,string? d)
    {
        var ok=Read(a,0,100,out var v1)&&v1==17&&Read(b,0,100,out var v2)&&v2==16&&
            Read(c,0,100,out var v3)&&v3==20&&Read(d,0,100,out var v4)&&v4==21;
        return new(ok,true,ok?"Number Talk answers17,16,20,21 correct; teacher checks structure explanation."
            :"37−20=17;37−21=16;37−17=20;37−16=21.");
    }
    public static Review SourceNotebook(string? estimate,string? actual,string? difference)
    {
        var ok=Read(estimate,0,100,out var a)&&a==20&&Read(actual,0,100,out var b)&&b==31&&
            Read(difference,0,100,out var d)&&d==11;
        return new(ok,true,ok?"Publisher source illustrative notebook estimated20 cm, actual31 cm, gap11 cm. This is NOT your real measurement."
            :"Read the publisher SYNTHESIS example estimate20 cm, measurement31 cm and gap11 cm.");
    }
    public static Review Independent(string? a,string? b,string? c)
    {
        var ok=Read(a,0,100,out var x)&&x==7&&Read(b,0,100,out var y)&&y==5&&
            Read(c,0,100,out var z)&&z==2;
        return new(ok,false,ok?"Independent illustrative gaps: |45−38|=7, |24−29|=5, |34−32|=2 cm."
            :"Calculate the positive distance between each estimated and actual reading; these are invented for practice.");
    }
}

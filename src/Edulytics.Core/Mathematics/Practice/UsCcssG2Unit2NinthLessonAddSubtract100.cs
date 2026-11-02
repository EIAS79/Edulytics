using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Source mixed-operation arithmetic only; real A–D cards and gallery walk teacher assessed.</summary>
public static class UsCcssG2Unit2NinthLessonAddSubtract100
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L09";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe authentic A–D warm-up diagram comparison, without inventing picture values",
        "Observe original Sort and Find the Value expression/image cards in groups of two or three",
        "Observe each group choose and explain ONE addition and ONE subtraction from actual cards",
        "Verify 27+47=74 and36+38=74 with composing one ten",
        "Verify55−27=28 and82−39=43 with decomposing one ten",
        "Observe student-created diagrams, MLR7 gallery walk and comparison of different strategies for SAME expression",
        "Real card matching, MLR8 discussion and actual gallery walk cannot be certified from numeric input"
    ];
    private static bool Read(string? s,int min,int max,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=min&&v<=max;
    public static Review SourceFour(string? a,string? b,string? c,string? d)
    {
        var ok=Read(a,0,100,out var x)&&x==74&&Read(b,0,100,out var y)&&y==28&&
            Read(c,0,100,out var z)&&z==74&&Read(d,0,100,out var w)&&w==43;
        return new(ok,true,ok?"Original source mixed results74,28,74,43 correct; teacher checks your models and MLR7 talk."
            :"Source27+47=74,55−27=28,36+38=74 and82−39=43.");
    }
    public static Review SourceSectionSummary(string? v)
    {
        var ok=Read(v,0,100,out var n)&&n==45;
        return new(ok,true,ok?"Source summary63−18=45 correct; teacher assesses a real base-ten model."
            :"63−18=45 because63=50+13,13−8=5 and50−10=40.");
    }
    public static Review Independent(string? a,string? b,string? c)
    {
        var ok=Read(a,0,100,out var x)&&x==73&&Read(b,0,100,out var y)&&y==27&&
            Read(c,0,100,out var z)&&z==84;
        return new(ok,false,ok?"First-party Edulytics maths46+27=73,75−48=27,58+26=84."
            :"Compose ones to add46+27=73 and58+26=84; decompose a ten to compute75−48=27.");
    }
}

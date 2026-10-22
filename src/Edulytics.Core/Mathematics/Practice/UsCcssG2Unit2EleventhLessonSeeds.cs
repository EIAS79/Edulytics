using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Source-true seed four story structures, numeric machine checks only.</summary>
public static class UsCcssG2Unit2EleventhLessonSeeds
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L11";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe real What Do You Know About Story Problems warm-up and student-suggested representations",
        "Source compare difference unknown: Diego42 orange and Jada16 apple yields26 more",
        "Source take-from result unknown: Lin31 gave Priya15, so16 remain",
        "Source put-together part unknown: Noah37 total with28 yellow =>9 blue",
        "Source compare smaller unknown: Elena50 and Andre23 fewer =>27 for Andre",
        "Teacher inspects student-created diagrams, base-ten blocks, partner MLR2 and 5 Practices explanations",
        "Correct digital numerical answers do not verify real manipulatives, discussion or restricted source Cool-down"
    ];
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review CompareGap(string? answer)
    {
        bool ok=N(answer,0,50,out var a)&&a==26;
        return new(ok,true,ok?"Diego42 vs Jada16:26 more. Teacher observes true compare tape diagram."
            :"42−16=26 more seeds for Diego than Jada, not a sum.");
    }
    public static Review OtherThree(string? lin,string? noah,string? andre)
    {
        bool ok=N(lin,0,50,out var a)&&a==16&&N(noah,0,50,out var b)&&b==9&&
            N(andre,0,50,out var c)&&c==27;
        return new(ok,true,ok?"Lin16 remains, Noah9 blue, Andre27 pumpkin seeds. Teacher verifies each story relationship."
            :"Lin31−15=16; Noah37−28=9; Andre50−23=27.");
    }
    public static Review Independent(string? a,string? b,string? c,string? d)
    {
        bool ok=N(a,0,50,out var x)&&x==26&&N(b,0,50,out var y)&&y==15&&
            N(c,0,50,out var z)&&z==15&&N(d,0,50,out var w)&&w==22;
        return new(ok,false,ok?"Independent Edulytics maths44−18=26,32−17=15,41−26=15,50−28=22."
            :"All first-party maths stay within50 and require appropriate story-relationship interpretation.");
    }
}

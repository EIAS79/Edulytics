using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Exact source numbers, real group-of-three participation only teacher-attested.</summary>
public static class UsCcssG2Unit2FourteenthLessonTwoStepSeeds
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L14";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Warm-up sums19,39,59,89 and student additive grouping methods",
        "Andre's source two-part25+17=42 THEN42+35=77 and equivalent three-addend equation",
        "Each real group of three students independently assigned authentic Priya, Mai or Kiran story",
        "Verify Priya29, Mai22, Kiran41 first THEN combined group answer92",
        "Assess actual tape diagrams/blocks, unknown labels and group presentation",
        "Observe MLR7 Compare and Connect gallery walk and MLR8 spoken explanations",
        "Numerical companion cannot attest real group actions or protected Jada's Seeds Cool-down"
    ];
    private static bool N(string? s,int lo,int hi,out int x)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out x)&&x>=lo&&x<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        var ok=N(a,0,100,out var x)&&x==19&&N(b,0,100,out var y)&&y==39&&
            N(c,0,100,out var z)&&z==59&&N(d,0,100,out var w)&&w==89;
        return new(ok,true,ok?"Original warm-up19,39,59,89 correct; teacher evaluates composing and grouping."
            :"5+9+5=19,25+9+5=39,25+15+19=59,25+30+15+19=89.");
    }
    public static Review SourceAndre(string? intermediate,string? total)
    {
        var ok=N(intermediate,0,100,out var x)&&x==42&&N(total,0,100,out var y)&&y==77;
        return new(ok,true,ok?"Andre25+17=42 and42+35=77 correct; teacher reviews two-step representation."
            :"Andre first25+17=42, then adds35 cucumbers for total77.");
    }
    public static Review GroupStories(string? priya,string? mai,string? kiran,string? group)
    {
        var ok=N(priya,0,100,out var x)&&x==29&&N(mai,0,100,out var y)&&y==22&&
            N(kiran,0,100,out var z)&&z==41&&N(group,0,100,out var w)&&w==92;
        return new(ok,true,ok?"Original Priya29, Mai22, Kiran41 and GROUP TOTAL92; teacher verifies group discussion."
            :"Priya53−24=29;Mai61−39=22;Kiran24+17=41; together29+22+41=92.");
    }
    public static Review SectionSummary(string? start)
    {
        var ok=N(start,0,100,out var x)&&x==37;
        return new(ok,true,ok?"Source section summary Jada37+28=65; teacher can check student model."
            :"Jada started37 because37+28=65.");
    }
    public static Review FirstParty(string? triple,string? smaller,string? part,string? gain)
    {
        var ok=N(triple,0,100,out var x)&&x==74&&N(smaller,0,100,out var y)&&y==50&&
            N(part,0,100,out var z)&&z==32&&N(gain,0,100,out var w)&&w==43;
        return new(ok,false,ok?"First-party results25+18+31=74,67−17=50,58−26=32,29+14=43."
            :"Use the operations from each independently authored story, with two-step addition for first example.");
    }
}

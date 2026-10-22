using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Supplemental numerical relationships, NOT original diagram-match Activity1 grading.</summary>
public static class UsCcssG2Unit2TwelfthLessonMancalaDiagrams
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L12";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe authentic Mancala Notice and Wonder image and learner questions, without assuming a specific game ruleset",
        "MLR6 Three Reads for Clare54 / Han16 FEWER and unknown smaller quantity, before revealing question",
        "Activity1 explicitly match source tape diagrams for four 54-and-16 contexts WITHOUT solving",
        "Distinguish gap unknown, total unknown, missing hand/board part and missing side",
        "Activity2 genuine pair matches original printed story/diagram cards and solves TWO from A–D",
        "Check original published diagrams L and N during synthesis; never fabricate hidden printed cards",
        "Optional digital numeric companion not teacher certification of real card matching or protected Cool-down"
    ];
    private static bool N(string? s,int lo,int hi,out int v)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out v)&&v>=lo&&v<=hi;
    public static Review SupplementalClareHan(string? smaller,string? together,string? board,string? otherSide)
    {
        var ok=N(smaller,0,100,out var a)&&a==38&&N(together,0,100,out var b)&&b==70&&
            N(board,0,100,out var c)&&c==38&&N(otherSide,0,100,out var d)&&d==38;
        return new(ok,true,ok?"Optional numerical relationships38,70,38,38. Source Activity1 was DIAGRAM MATCHING, not numerical solving; teacher checks original matched diagrams."
            :"Clare54, Han16 fewer=>38. Clare54 and Han16=>70 total. Clare54 holding16=>38 on board. Board54, Clare16=>Han38.");
    }
    public static Review FirstParty(string? smaller,string? total,string? remaining,string? missing)
    {
        var ok=N(smaller,0,100,out var a)&&a==36&&N(total,0,100,out var b)&&b==90&&
            N(remaining,0,100,out var c)&&c==36&&N(missing,0,100,out var d)&&d==36;
        return new(ok,false,ok?"Independent Edulytics numbers36,90,36,36; authentic publisher blackline-card values not claimed."
            :"Independently authored scenarios:63−27=36 and63+27=90; their difference depends on what is asked.");
    }
}

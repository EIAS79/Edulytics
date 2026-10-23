using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Numerical checks, explicitly NOT original unpublished player-card values.</summary>
public static class UsCcssG2Unit2SeventhLessonSubtractTwoDigits
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U02:L07";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe source visual warm-up with two representations of 55, including groups of five forming a ten",
        "Check real 46−28 blocks or drawing: trade to three tens+sixteen ones, finish eighteen",
        "Check real 93−54 blocks or drawing: trade to eight tens+thirteen ones, finish thirty-nine",
        "Observe group-of-four genuine Diego/Lin/Jada/Han printed player cards and action cards",
        "Each real player's number changes twice; check genuine action-card directions and written equations",
        "Check actual group sum of all four ending totals; publisher card quantities not fabricated",
        "Observe MLR8 partner discussion, manipulatives and student-produced diagrams; no digital proof of participation"
    ];
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review SourceProblems(string? a,string? b,string? aTens,string? aOnes,string? bTens,string? bOnes)
    {
        bool ok=N(a,0,100,out var x)&&x==18&&N(b,0,100,out var y)&&y==39&&
            N(aTens,0,9,out var t)&&t==3&&N(aOnes,0,19,out var o)&&o==16&&
            N(bTens,0,9,out var q)&&q==8&&N(bOnes,0,19,out var w)&&w==13;
        return new(ok,true,ok?"Source subtraction numbers and traded forms are correct; teacher must review genuine blocks and diagrams."
            :"46=3 tens+16 ones and 46−28=18; 93=8 tens+13 ones and 93−54=39.");
    }
    public static Review FirstParty(string? a,string? b)
    {
        var ok=N(a,0,100,out var x)&&x==37&&N(b,0,100,out var y)&&y==37;
        return new(ok,false,ok?"Independent examples 64−27=37,72−35=37; NOT publisher game cards."
            :"First-party examples: 64−27=37 and72−35=37.");
    }
    public static Review Exit(string? a,string? b,string? c)
    {
        var ok=N(a,0,100,out var x)&&x==27&&N(b,0,100,out var y)&&y==28&&
            N(c,0,100,out var z)&&z==28;
        return new(ok,false,ok?"Independent exit: 73−46=27,85−57=28,62−34=28."
            :"Trade one ten in each independent two-digit subtraction.");
    }
}

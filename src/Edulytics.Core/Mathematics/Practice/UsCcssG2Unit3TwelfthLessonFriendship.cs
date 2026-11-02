using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Exact source two-step calculations, actual peer diagrams teacher assessed.</summary>
public static class UsCcssG2Unit3TwelfthLessonFriendship
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U03:L12";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Four warm-up place value equalities original true,true,true,false;58 vs48",
        "Original Lin92cm gives Noah35cm and Jada28cm, intermediate57cm then final29cm",
        "Han starts82in, needs48in, donates34in to Clare; Clare27+34=61in",
        "Andre needs50in with28in already: missing22in from Mai; Mai remaining49, initial71in",
        "MLR6 Three Reads before source first activity, two-step quantities mapped in distinct tape diagrams",
        "No mixing cm and inch labels across different source stories; real partner/classroom work teacher observed",
        "Original Sharing Saree Silk Ribbon cooldown restricted, independent first-party examples not attributed"
    ];
    private static bool N(string? s,int lo,int hi,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=lo&&n<=hi;
    public static Review Warmup(string? a,string? b,string? c,string? d)
    {
        bool ok=a=="True"&&b=="True"&&c=="True"&&d=="False";
        return new(ok,true,ok?"Warm-up true,true,true,false. The final RHS20+20+8=48, not58.":
            "Check totals:24=10+14;15+12=27;26=10+6+10;58≠20+20+8.");
    }
    public static Review Lin(string? afterFirst,string? afterSecond)
    {
        bool ok=N(afterFirst,0,100,out var a)&&a==57&&N(afterSecond,0,100,out var b)&&b==29;
        return new(ok,true,ok?"Source Lin92cm−35cm=57cm, then57cm−28cm=29cm. Teacher checks labeled original two-step diagrams.":
            "Start92cm, give35 to Noah (57cm), then28 to Jada, remaining29cm.");
    }
    public static Review TwoOtherStories(string? hanExtra,string? clareFinal,string? andreNeed,string? maiBefore)
    {
        bool ok=N(hanExtra,0,100,out var a)&&a==34&&N(clareFinal,0,100,out var b)&&b==61&&
            N(andreNeed,0,100,out var c)&&c==22&&N(maiBefore,0,100,out var d)&&d==71;
        return new(ok,true,ok?"Han cuts34, Clare ends61; Andre needs22, Mai STARTS71. All are inches; teacher reviews story diagrams.":
            "82−48=34;27+34=61. 50−28=22;49+22=71.");
    }
    public static Review FirstParty(string? remaining,string? gift,string? total)
    {
        bool ok=N(remaining,0,100,out var a)&&a==41&&N(gift,0,100,out var b)&&b==29&&
            N(total,0,100,out var c)&&c==53;
        return new(ok,false,ok?"First-party Iris86−26−19=41cm; Noor75−46=29in; Omar24+29=53in.":
            "Original Edulytics numbers:86−26−19=41;75−46=29;24+29=53.");
    }
}

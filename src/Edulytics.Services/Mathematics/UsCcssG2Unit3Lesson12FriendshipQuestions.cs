using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original randomized two-step tape diagrams with distinct sequential math.</summary>
internal static class UsCcssG2Unit3Lesson12FriendshipQuestions
{
    public const string Take="usccss.g2.u03.l12.two_step_take_from";
    public const string Give="usccss.g2.u03.l12.extra_piece_given_to_friend";
    public const string Unknown="usccss.g2.u03.l12.missing_length_then_start_unknown";
    public const string ChangeAdd="usccss.g2.u03.l12.two_step_change_then_add";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Take or Give or Unknown or ChangeAdd;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not friendship two-step family");
        int total,first,second;
        if(family==Take)
        {
            total=r.Next(60,100);first=r.Next(10,Math.Min(40,total-22));second=r.Next(10,Math.Min(35,total-first));
        }
        else if(family==Give)
        {
            total=r.Next(60,96);first=r.Next(20,total-5);second=r.Next(10,101-(total-first));
            // first is the length kept; total-first is donated and added to second
        }
        else if(family==Unknown)
        {
            total=r.Next(40,80);first=r.Next(10,total-10);second=r.Next(10,101-(total-first));
            // total=desired length, first=current, second=donor remainder
        }
        else
        {
            total=r.Next(45,95);first=r.Next(10,total-10);second=r.Next(10,101-(total-first));
            // first is taken away from total; then remainder goes to second person
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"total",total},{"first",first},{"second",second}};
        var prompt=family switch{
            Take=>$"FIRST-PARTY two-step ribbon: start with {total} centimeters, cut {first} cm for friend A, then {second} cm for friend B. How many cm REMAIN? Use the first remainder to make the second subtraction.",
            Give=>$"FIRST-PARTY gift: a child has {total} inches of ribbon and only needs {first}. Gives ALL extra to a friend who owns {second} inches. How long is the friend's ribbon NOW?",
            Unknown=>$"FIRST-PARTY two-step START UNKNOWN: a child has {first} inches but needs {total}. A donor gives the missing ribbon, then has {second} inches LEFT. How much ribbon did the donor START with?",
            _=>$"FIRST-PARTY two-step change then add: a child has {total} inches, cuts {first} inches, and gives the REMAINDER to a friend with {second} inches. How long is the friend's ribbon NOW?"
        };
        int value=family switch{Take=>total-first-second,Give=>second+total-first,Unknown=>second+total-first,_=>second+total-first};
        string solution=family switch{
            Take=>$"{total}−{first}={total-first}; {total-first}−{second}={value} cm.",
            Give=>$"{total}−{first}={total-first} in extra; {second}+{total-first}={value} in.",
            Unknown=>$"{total}−{first}={total-first} in needed; {second}+{total-first}={value} in original.",
            _=>$"{total}−{first}={total-first} in remainder; {second}+{total-first}={value} in final."
        };
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid two-step ribbon parameters");
        var answer=family==Take?p["total"]-p["first"]-p["second"]:p["second"]+p["total"]-p["first"];
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=3||
            !p.TryGetValue("total",out var total)||total is <40 or >99||
            !p.TryGetValue("first",out var first)||first is <10 or >90||first>=total||
            !p.TryGetValue("second",out var second)||second is <10 or >99)return false;
        if(family==Take)return total>=60&&total-first-second>=0;
        return second+total-first<=100;
    }
}

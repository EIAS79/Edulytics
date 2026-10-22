using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original numerical variants with exact integer solution.
/// Source stage4 real six-turn competition is not verified digitally.</summary>
internal static class UsCcssG2Unit2Lesson6TargetNumbersQuestions
{
    public const string Equivalent="usccss.g2.u02.l06.rewrite_tens_ones_equivalently";
    public const string Diagram="usccss.g2.u02.l06.compare_decomposed_82_style";
    public const string Ones="usccss.g2.u02.l06.target_stage4_subtract_ones";
    public const string Tens="usccss.g2.u02.l06.target_stage4_subtract_tens";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Equivalent or Diagram or Ones or Tens;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L06 source family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==Equivalent)
        {
            var tens=r.Next(3,10); var ones=r.Next(0,10); var removedTens=r.Next(1,tens);
            var whole=10*tens+ones;
            p["whole"]=whole;p["tens"]=tens;p["ones"]=ones;p["removedTens"]=removedTens;
            prompt=$"Start with {whole} ({tens} tens and {ones} ones). Exchange {removedTens} tens for ones without changing the value. How many ONES are in the new representation?";
            solution=$"{whole}={(tens-removedTens)} tens + {ones+10*removedTens} ones.";
        }
        else if(family==Diagram)
        {
            var tens=r.Next(2,10); var ones=r.Next(0,9); var subtract=r.Next(ones+1,10);
            var whole=10*tens+ones;
            p["whole"]=whole;p["tens"]=tens;p["ones"]=ones;p["subtract"]=subtract;
            prompt=$"Source-style comparative diagrams: {whole}−{subtract}. Trade 1 ten and take away {subtract} ones. What is the final number?";
            solution=$"{whole}={(tens-1)} tens +{ones+10} ones, so{whole}−{subtract}={whole-subtract}.";
        }
        else
        {
            var card=r.Next(1,10); var current=r.Next(family==Tens?card*10:card,100);
            p["card"]=card;p["current"]=current;
            prompt=$"FIRST-PARTY Target Numbers Stage4 arithmetic turn: current {current}, card {card}. Choose subtract "+
                (family==Tens?"TENS (ten times the card).":"ONES (exactly the card).")+
                " What is the remaining nonnegative number? This is not a complete real game.";
            solution=$"{current}−{(family==Tens?card*10:card)}="+
                (current-(family==Tens?card*10:card))+".";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Target Stage4 parameters");
        int result=family switch{
            Equivalent=>p["ones"]+10*p["removedTens"],
            Diagram=>p["whole"]-p["subtract"],
            Ones=>p["current"]-p["card"],Tens=>p["current"]-10*p["card"],
            _=>throw new InvalidOperationException("Unsupported Target stage4 question")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Equivalent)
            return p.Count==4&&p.TryGetValue("whole",out var w)&&
                p.TryGetValue("tens",out var t)&&t is >=3 and <=9&&
                p.TryGetValue("ones",out var o)&&o is >=0 and <=9&&w==10*t+o&&
                p.TryGetValue("removedTens",out var r)&&r>=1&&r<t;
        if(family==Diagram)
            return p.Count==4&&p.TryGetValue("whole",out var w)&&
                p.TryGetValue("tens",out var t)&&t is >=2 and <=9&&
                p.TryGetValue("ones",out var o)&&o is >=0 and <=8&&w==10*t+o&&
                p.TryGetValue("subtract",out var s)&&s is >=1 and <=9&&s>o;
        if(p.Count!=2||!p.TryGetValue("card",out var card)||card is <1 or >9||
           !p.TryGetValue("current",out var current)||current is <0 or >99)return false;
        return family==Ones?current>=card:current>=10*card;
    }
}

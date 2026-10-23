using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>First-party mathematical calculation families for optional real
/// center games. Digital sums do not verify captured squares or a five-in-row.</summary>
internal static class UsCcssG2Unit2Lesson4OptionalCenterQuestions
{
    public const string Talk="usccss.g2.u02.l04.subtract_place_value";
    public const string Capture="usccss.g2.u02.l04.capture_squares_add_within20";
    public const string Compose="usccss.g2.u02.l04.five_in_row_composing_ten";
    public const string Simple="usccss.g2.u02.l04.five_in_row_no_compose";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Talk or Capture or Compose or Simple;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L04 optional center");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==Talk)
        {
            var digit=random.Next(4,10);
            var tens=random.Next(4,10);
            var left=10*tens+digit;
            var minusTens=random.Next(1,tens);
            var minusOnes=random.Next(0,digit+1);
            var right=minusTens*10+minusOnes;
            p["left"]=left;p["right"]=right;
            prompt=$"Number Talk: subtract {left}−{right} using tens and ones, no borrowing.";
            solution=$"{left}−{right}={left-right}; subtract {tens} tens−{minusTens} tens and {digit} ones−{minusOnes} ones.";
        }
        else if(family==Capture)
        {
            var spin=random.Next(6,11); var card=random.Next(0,11);
            p["spinner"]=spin;p["card"]=card;
            prompt=$"FIRST-PARTY Capture Squares numeric turn: spinner {spin}, card {card}. What board TARGET sum would you look for? This does not capture a square.";
            solution=$"{spin}+{card}={spin+card}. An adjacent-dot edge must still be drawn on a real board.";
        }
        else
        {
            var hiTen=random.Next(1,6); var loTen=random.Next(1,9-hiTen);
            var hiOne=random.Next(1,10);var loOne=family==Compose
                ?random.Next(10-hiOne,10)
                :random.Next(0,10-hiOne);
            var top=hiTen*10+hiOne; var bottom=loTen*10+loOne;
            p["top"]=top;p["bottom"]=bottom;
            prompt=$"FIRST-PARTY Five in a Row numeric turn: top clip {top}, bottom clip {bottom}. Find sum within100"+
                (family==Compose?" by making a new ten.":" without making a new ten.")+
                " This number alone never proves a win.";
            solution=$"{top}+{bottom}={top+bottom}. "+(family==Compose
                ?"The ones sum is at least ten, so a new ten is composed."
                :"The ones sum is less than ten; no exchange is needed.");
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid optional center math parameters");
        var result=family switch{
            Talk=>p["left"]-p["right"],Capture=>p["spinner"]+p["card"],
            Compose or Simple=>p["top"]+p["bottom"],
            _=>throw new InvalidOperationException("Unsupported center math")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Talk)return p.Count==2&&p.TryGetValue("left",out var l)&&l is >=44 and <=99
            &&p.TryGetValue("right",out var r)&&r is >=10 and <=99
            &&l>r&&l%10>=r%10&&l/10>r/10;
        if(family==Capture)return p.Count==2&&p.TryGetValue("spinner",out var spin)&&spin is >=6 and <=10
            &&p.TryGetValue("card",out var card)&&card is >=0 and <=10;
        if(p.Count!=2||!p.TryGetValue("top",out var t)||!p.TryGetValue("bottom",out var b)||
           t is <10 or >99||b is <10 or >99||t+b>100)return false;
        return family==Compose?t%10+b%10>=10:t%10+b%10<10;
    }
}

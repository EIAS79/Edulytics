using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Numerical source-type variants with four different unknown positions.</summary>
internal static class UsCcssG2Unit2Lesson13StoryEquationsQuestions
{
    public const string Start="usccss.g2.u02.l13.add_to_start_unknown";
    public const string Change="usccss.g2.u02.l13.add_to_change_unknown";
    public const string Total="usccss.g2.u02.l13.put_together_total_unknown";
    public const string Part="usccss.g2.u02.l13.put_together_part_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Start or Change or Total or Part;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L13 exact question");
        var aTens=r.Next(2,8),bTens=r.Next(1,9-aTens);
        var aOnes=r.Next(1,10),bOnes=r.Next(10-aOnes,10);
        var start=10*aTens+aOnes,change=10*bTens+bOnes,total=start+change;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["start"]=start,["change"]=change,["total"]=total,["aTens"]=aTens,
            ["bTens"]=bTens,["aOnes"]=aOnes,["bOnes"]=bOnes};
        string prompt=family switch{
            Start=>$"First-party Add To START unknown: Some seeds were in a bowl; {change} more were added and there are {total} now. How many were there FIRST? Equation ?+{change}={total}.",
            Change=>$"First-party Add To CHANGE unknown: start with {start} seeds, add some, and finish with {total}. How many were ADDED? Equation {start}+?={total}.",
            Total=>$"First-party Put Together TOTAL unknown: {start} blue seeds and {change} yellow seeds. How many altogether? Equation {start}+{change}=?",
            _=>$"First-party Put Together PART unknown: {total} seeds in all, {start} blue, rest yellow. How many yellow? Equation {start}+?={total}."
        };
        var answer=family==Start?start:family==Total?total:change;
        return new Problem(family,prompt,$"{start}+{change}={total}, unknown value {answer}. Two parts and total must be labeled accurately.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid four unknown positions");
        var value=family switch{Start=>p["start"],Change or Part=>p["change"],Total=>p["total"],_=>throw new InvalidOperationException("No such source type")};
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==7&&p.TryGetValue("start",out var start)&&
        p.TryGetValue("change",out var change)&&p.TryGetValue("total",out var total)&&
        p.TryGetValue("aTens",out var at)&&at is >=2 and <=7&&
        p.TryGetValue("bTens",out var bt)&&bt>=1&&at+bt<=8&&
        p.TryGetValue("aOnes",out var ao)&&ao is >=1 and <=9&&
        p.TryGetValue("bOnes",out var bo)&&bo is >=1 and <=9&&ao+bo>=10&&
        start==10*at+ao&&change==10*bt+bo&&total==start+change&&total<=100;
}

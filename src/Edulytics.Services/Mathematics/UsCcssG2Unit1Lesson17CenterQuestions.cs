using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>First-party deterministic *virtual* counting and center examples.
/// Never pretends to observe original physical 100–120 object collection.</summary>
internal static class UsCcssG2Unit1Lesson17CenterQuestions
{
    public const string Tens="usccss.g2.u01.l17.count_groups_of_ten";
    public const string Fives="usccss.g2.u01.l17.count_groups_of_five";
    public const string Talk="usccss.g2.u01.l17.add_multiples_of_ten";
    public const string Add="usccss.g2.u01.l17.add_within_fifty";
    public const string Graph="usccss.g2.u01.l17.read_bar_graph_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Tens or Fives or Talk or Add or Graph;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade 2 L17 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family is Tens or Fives)
        {
            var size=family==Tens?10:5;
            var total=rng.Next(100,121);var groups=total/size;var loose=total%size;
            p["groupSize"]=size;p["groups"]=groups;p["loose"]=loose;p["total"]=total;
            prompt="Independent Edulytics VIRTUAL collection: "+groups+
                " complete groups of "+size+" objects and "+loose+
                " leftover objects. How many in all?";
            solution=groups+" × "+size+" + "+loose+" = "+total;
        }
        else if(family==Talk)
        {
            var start=rng.Next(12,61);var tens=rng.Next(1,5)*10;
            p["start"]=start;p["tens"]=tens;p["total"]=start+tens;
            prompt="Mental place-value Number Talk: "+start+" + "+tens+" = ?";
            solution=start+" + "+tens+" = "+(start+tens)+". Add tens, retain ones.";
        }
        else if(family==Add)
        {
            var left=rng.Next(6,34);var right=rng.Next(3,51-left);
            p["left"]=left;p["right"]=right;p["total"]=left+right;
            prompt="Original Edulytics How Close?/Number Puzzles addition: "+
                left+" + "+right+" = ? (within 50)";
            solution=left+" + "+right+" = "+(left+right);
        }
        else
        {
            var red=rng.Next(2,9);var blue=rng.Next(red+1,13);
            p["red"]=red;p["blue"]=blue;p["difference"]=blue-red;
            prompt="Original Edulytics fictional bar graph, equal scale of one: "+
                "red="+red+", blue="+blue+". How many MORE blue than red?";
            solution=blue+" − "+red+" = "+(blue-red);
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 L17 parameters");
        return (family==Graph?p["difference"]:p["total"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is Tens or Fives)
        {
            var size=family==Tens?10:5;
            return p.Count==4&&p.TryGetValue("groupSize",out var n)&&n==size
                &&p.TryGetValue("groups",out var groups)&&groups>=100/size&&groups<=120/size
                &&p.TryGetValue("loose",out var loose)&&loose>=0&&loose<size
                &&p.TryGetValue("total",out var total)&&total>=100&&total<=120
                &&size*groups+loose==total;
        }
        if(family==Talk)return p.Count==3
            &&p.TryGetValue("start",out var start)&&start>=12&&start<=60
            &&p.TryGetValue("tens",out var tens)&&tens>=10&&tens<=40&&tens%10==0
            &&p.TryGetValue("total",out var sum)&&sum==start+tens&&sum<=100;
        if(family==Add)return p.Count==3
            &&p.TryGetValue("left",out var left)&&left>=6&&left<=33
            &&p.TryGetValue("right",out var right)&&right>=3
            &&p.TryGetValue("total",out var total)&&total==left+right&&total<=50;
        return p.Count==3
            &&p.TryGetValue("red",out var red)&&red>=2&&red<=8
            &&p.TryGetValue("blue",out var blue)&&blue>red&&blue<=12
            &&p.TryGetValue("difference",out var diff)&&diff==blue-red;
    }
}

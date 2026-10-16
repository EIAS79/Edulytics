using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Four first-edition (IM 2021) source-goal Practice families for B001.
/// Correct answers are recomputed from constrained parameters; questions
/// are student-original, not reproductions of copyrighted workbook tasks.
/// </summary>
internal static class UsCcssB001SourceGoalQuestions
{
    public const string NoRegroupBar = "usccss.g2.u02.bar_total_or_difference.no_regroup";
    public const string TwoStepLength = "usccss.g2.u03.two_step_ribbon_length";
    public const string EqualGroups = "usccss.g3.u01.equal_groups.model_product";
    public const string UnknownFactor = "usccss.g3.u01.equal_groups.unknown_factor";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is NoRegroupBar or TwoStepLength or EqualGroups or UnknownFactor;

    public static Problem Build(string family,Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown B001 source-goal family");

        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==NoRegroupBar)
        {
            // Both possible operations can be solved by tens/ones with no
            // composing or decomposing a ten, unlike legacy 32-18 or 27+15.
            var first=random.Next(2,6)*10 + random.Next(4,8);
            var second=random.Next(1,3)*10 + random.Next(1,10-first%10);
            var operation=random.Next(2); // 0 = total; 1 = compare
            var noun=random.Next(3);
            var item=new[]{"popcorn","pretzels","fruit"}[noun];
            p["first"]=first;p["second"]=second;p["operation"]=operation;p["noun"]=noun;
            prompt=$"A school bar graph shows {first} students chose {item} and "+
                $"{second} chose crackers. "+
                (operation==0
                ? "How many students chose either snack altogether?"
                : $"How many more students chose {item} than crackers?")+
                " Use separate tens and ones with no exchanging tens. Enter the number.";
            solution=operation==0
                ? "Combine the tens, then the ones. The ones remain below ten; no new ten is made."
                : "Compare tens and ones separately. There are enough ones to subtract without breaking a ten.";
        }
        else if(family==TwoStepLength)
        {
            var initial=random.Next(65,100);
            var cut1=random.Next(12,25);
            var cut2=random.Next(7,21);
            var unit=random.Next(2);
            p["initial"]=initial;p["cut1"]=cut1;p["cut2"]=cut2;p["unit"]=unit;
            var unitText=unit==0?"cm":"inches";
            prompt=$"Mira has a ribbon {initial} {unitText} long. She cuts off "+
                $"{cut1} {unitText} for one project and then cuts another {cut2} "+
                $"{unitText} from the remaining ribbon. How many {unitText} are left? "+
                "Draw a labelled tape diagram showing both cuts, then enter the remaining length.";
            solution="Take the first cut from the original length. Use that result as the starting length "+
                "for the second cut. Label the remaining length and check both parts add to the original.";
        }
        else
        {
            var groups=random.Next(2,10);
            var size=random.Next(2,11);
            var context=random.Next(3);
            var unknownPosition=family==EqualGroups?0:random.Next(2);
            p["groups"]=groups;p["size"]=size;p["context"]=context;p["unknownPosition"]=unknownPosition;
            var item=new[]{"pencils","stickers","marbles"}[context];
            if(family==EqualGroups)
            {
                prompt=$"There are {groups} equal boxes with {size} {item} in each box. "+
                    $"Draw {groups} groups of {size}, or a labelled tape diagram, and write "+
                    $"a multiplication equation. How many {item} altogether? Enter the total.";
                solution="The group count and size are distinct factors. Multiply the number of "+
                    "equal groups by the objects in each, then check the product against the drawing.";
            }
            else if(unknownPosition==0)
            {
                prompt=$"There are {groups*size} {item} in {groups} equal boxes. "+
                    $"Each box has the same number. Complete {groups} × [ ? ] = {groups*size}. "+
                    $"Draw the groups and enter the missing factor (items per box).";
                solution="The total is shared equally across the known number of groups. "+
                    "Draw the groups and find the size of each group, then check the multiplication.";
            }
            else
            {
                prompt=$"There are {groups*size} {item}, with {size} in every equal box. "+
                    $"How many boxes? Complete [ ? ] × {size} = {groups*size}. "+
                    "Draw or skip-count the equal groups, then enter the missing group count.";
                solution="Find how many equal groups of the known size make the total. "+
                    "Check that the missing group count times the group size gives the total.";
            }
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p))
            throw new InvalidOperationException("Invalid B001 source-goal task parameters");
        var value=family switch
        {
            NoRegroupBar=>p["operation"]==0?p["first"]+p["second"]:p["first"]-p["second"],
            TwoStepLength=>p["initial"]-p["cut1"]-p["cut2"],
            EqualGroups=>p["groups"]*p["size"],
            UnknownFactor=>p["unknownPosition"]==0?p["size"]:p["groups"],
            _=>throw new InvalidOperationException("Unsupported B001 family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string answer) =>
        Valid(family,parameters) &&
        string.Equals(answer,Solve(family,parameters),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==NoRegroupBar)
            return p.Count==4 &&
                p.TryGetValue("first",out var a) && p.TryGetValue("second",out var b) &&
                p.TryGetValue("operation",out var op) && p.TryGetValue("noun",out var noun) &&
                a/10 is >=2 and <=5 && a%10 is >=4 and <=7 &&
                b/10 is >=1 and <=2 && b%10>=1 &&
                a%10+b%10<=9 && a%10>=b%10 &&
                op is 0 or 1 && noun is >=0 and <=2;
        if(family==TwoStepLength)
            return p.Count==4 &&
                p.TryGetValue("initial",out var original) &&
                p.TryGetValue("cut1",out var one) &&
                p.TryGetValue("cut2",out var two) &&
                p.TryGetValue("unit",out var unit) &&
                original is >=65 and <=99 &&
                one is >=12 and <=24 && two is >=7 and <=20 &&
                original-one-two>0 && unit is 0 or 1;
        return p.Count==4 &&
            p.TryGetValue("groups",out var group) &&
            p.TryGetValue("size",out var size) &&
            p.TryGetValue("context",out var context) &&
            p.TryGetValue("unknownPosition",out var position) &&
            group is >=2 and <=9 && size is >=2 and <=10 &&
            context is >=0 and <=2 && position is 0 or 1 &&
            (family!=EqualGroups || position==0);
    }
}

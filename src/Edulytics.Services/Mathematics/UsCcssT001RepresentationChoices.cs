using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Real three-option representation-reading tasks: source-purpose practice
/// extension for T001 L02 / L07 / L08 / L09. Each option represents a
/// distinct expression or a complete three-category chart/table.
/// Integer 1..3 input is a multiple-choice selection, NOT a substitute
/// for independently authoring a diagram or surveying real people.
/// </summary>
internal static class UsCcssT001RepresentationChoices
{
    public sealed record Problem(
        string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly HashSet<string> Families = new(StringComparer.Ordinal)
    {
        "usccss.t001.g1.choose_dot_expression",
        "usccss.t001.g1.choose_tool_sort",
        "usccss.t001.g1.choose_shape_chart",
        "usccss.t001.g1.choose_survey_table"
    };

    public static bool Supports(string family)=>Families.Contains(family);

    private static int AnswerPosition(int a,int b,int c) =>
        1+((a+2*b+3*c)%3);

    private static string PresentOptions(
        string correct,string shiftedOne,string shiftedTwo,int correctPosition)
    {
        var choices=new string[3];
        choices[correctPosition-1]=correct;
        choices[correctPosition%3]=shiftedOne;
        choices[(correctPosition+1)%3]=shiftedTwo;
        return string.Join(" | ",choices.Select((item,index)=>
            (index+1).ToString(CultureInfo.InvariantCulture)+") "+item));
    }

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))
            throw new InvalidOperationException("Unreviewed T001 representation family.");
        var a=rng.Next(1,5);
        var b=rng.Next(1,5);
        var c=rng.Next(1,5);
        if(family!="usccss.t001.g1.choose_dot_expression")
        {
            // Distinct counts are required so rotated tables/graphs are never
            // accidentally also valid answers.
            b=(a%4)+1;
            c=(b%4)+1;
            if(c==a)c=(c%4)+1;
        }
        var position=AnswerPosition(a,b,c);
        string prompt,solution,correct,wrong1,wrong2;
        if(family=="usccss.t001.g1.choose_dot_expression")
        {
            correct=$"{a} + {b}";
            wrong1=$"{a} + {b+1}";
            wrong2=$"{a+1} + {b}";
            // a=b yields two different wrong expressions, but same sum;
            // neither expression matches the pair of observed groups.
            prompt=$"Look at two dot cards: {new string('●',a)} and " +
                $"{new string('●',b)}. Which written addition expression " +
                "matches these two pictured groups? Enter option 1, 2 or 3.";
            solution="Count each separate dot group and choose the expression "+
                "with those exact addends, not just an expression with a similar sum.";
        }
        else
        {
            var labels=family switch
            {
                "usccss.t001.g1.choose_tool_sort"=>
                    new[]{"Counters","Pattern blocks","Inch tiles"},
                "usccss.t001.g1.choose_shape_chart"=>
                    new[]{"Triangles","Squares","Circles"},
                _=>new[]{"Apples","Bananas","Oranges"}
            };
            string List(int x,int y,int z)=>
                $"{labels[0]}: {new string('●',x)}; {labels[1]}: {new string('●',y)}; " +
                $"{labels[2]}: {new string('●',z)}";
            correct=List(a,b,c);
            wrong1=List(b,c,a);
            wrong2=List(c,a,b);
            var data=family switch
            {
                "usccss.t001.g1.choose_tool_sort"=>
                    $"A mixed collection has {a} counters, {b} pattern blocks " +
                    $"and {c} inch tiles. Sort by tool kind.",
                "usccss.t001.g1.choose_shape_chart"=>
                    $"Sort {a} triangle cards, {b} square cards and {c} circle cards. " +
                    "Draw one same-sized dot per card in three labeled rows.",
                _=> $"A fictional survey has {a} apple votes, {b} banana votes " +
                    $"and {c} orange votes. Each student votes once."
            };
            prompt=data+" Which FULL labeled three-category display is correct? "+
                "Enter option 1, 2 or 3.";
            solution="Keep each category label attached to its own count; "+
                "one picture mark represents one object or vote.";
        }
        prompt+=" Options: "+PresentOptions(correct,wrong1,wrong2,position);
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal)
            {
                ["first"]=a,["second"]=b,["third"]=c
            });
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||
           !p.TryGetValue("first",out var a)||
           !p.TryGetValue("second",out var b)||
           !p.TryGetValue("third",out var c)||
           a is <1 or >4||b is <1 or >4||c is <1 or >4)
            return false;
        return family=="usccss.t001.g1.choose_dot_expression"||
            (a!=b && b!=c && a!=c);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> parameters)
    {
        if(!Valid(family,parameters))
            throw new InvalidOperationException("T001 representation input invalid.");
        return AnswerPosition(parameters["first"],parameters["second"],
            parameters["third"]).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Valid(family,p) &&
        int.TryParse(answer,NumberStyles.Integer,CultureInfo.InvariantCulture,
            out var chosen) &&
        chosen==AnswerPosition(p["first"],p["second"],p["third"]);
}

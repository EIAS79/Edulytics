using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original first-party, source-purpose tasks exclusively for US CCSS
/// G1 U01 L02-L10. Zero possibility of selecting these by vague topic keywords.
/// Includes an independent integer grading oracle and strict domain checks.
/// No L1-L6 academic decision is conferred by the engine.
/// </summary>
internal static class UsCcssT001ExactQuestions
{
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    private static readonly HashSet<string> Families = new(StringComparer.Ordinal)
    {
        "usccss.t001.g1.dot_pair_sum",
        "usccss.t001.g1.add_one_or_two",
        "usccss.t001.g1.subtract_one_or_two",
        "usccss.t001.g1.mixed_add_subtract",
        "usccss.t001.g1.make_ten_missing_part",
        "usccss.t001.g1.sort_math_tools",
        "usccss.t001.g1.sort_shape_cards",
        "usccss.t001.g1.class_survey_categories"
    };
    public static bool Supports(string family)=>Families.Contains(family);

    private static IReadOnlyDictionary<string,int> D(params (string K,int V)[] pairs)=>
        pairs.ToDictionary(x=>x.K,x=>x.V,StringComparer.Ordinal);

    // The student must actually categorize the items, not simply read
    // pre-counted labels. Group membership/quantity is kept in parameters
    // for independent grading while the shuffled order changes by seed.
    private static string ShuffledItems(Random rng,params (string Item,int Count)[] groups)
    {
        var items=groups.SelectMany(x=>Enumerable.Repeat(x.Item,x.Count)).ToArray();
        for(var i=items.Length-1;i>0;i--)
        {
            var j=rng.Next(i+1);
            (items[i],items[j])=(items[j],items[i]);
        }
        return string.Join(", ",items);
    }

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))
            throw new InvalidOperationException("Unsupported T001 lesson family.");
        int a,b,c,category,operation;
        switch(family)
        {
            case "usccss.t001.g1.dot_pair_sum":
                a=rng.Next(1,7);b=rng.Next(1,Math.Min(6,10-a)+1);
                return new(family,
                    $"Two dot groups show {new string('●',a)} and {new string('●',b)}. " +
                    $"Write the sum of {a} + {b}.",
                    "Count the dots in each group separately, then count all the dots.",
                    AssessmentItemType.Numeric,D(("left",a),("right",b)));
            case "usccss.t001.g1.add_one_or_two":
                a=rng.Next(0,10);b=rng.Next(1,Math.Min(2,10-a)+1);
                return new(family,$"Start at {a}. Count forward {b} step(s). What is {a}+{b}?",
                    "Count one new number for each forward step, never the starting number.",
                    AssessmentItemType.Numeric,D(("start",a),("step",b)));
            case "usccss.t001.g1.subtract_one_or_two":
                a=rng.Next(1,11);b=rng.Next(1,Math.Min(2,a)+1);
                return new(family,$"Put out {a} counters and take {b} away. How many remain?",
                    "Count backward once for each removed counter, then check by adding back.",
                    AssessmentItemType.Numeric,D(("start",a),("step",b)));
            case "usccss.t001.g1.mixed_add_subtract":
                operation=rng.Next(0,2);
                if(operation==0)
                {
                    a=rng.Next(0,10);b=rng.Next(1,11-a);
                }
                else
                {
                    a=rng.Next(1,11);b=rng.Next(1,a+1);
                }
                return new(family,
                    operation==0?
                        $"Combine {a} counters and {b} more. What is {a}+{b}?" :
                        $"Remove {b} counters from {a}. What is {a}-{b}?",
                    "Choose addition when combining; subtraction when removing. Check with counters.",
                    AssessmentItemType.Numeric,D(("left",a),("right",b),("operation",operation)));
            case "usccss.t001.g1.make_ten_missing_part":
                a=rng.Next(0,11);b=10-a;
                return new(family,$"In Find the Pair, card {a} needs which partner to make 10? Complete {a}+?=10.",
                    "Use a ten-frame: the missing boxes give the partner. Check both addends total ten.",
                    AssessmentItemType.Numeric,D(("known",a),("missing",b)));
            case "usccss.t001.g1.sort_math_tools":
                a=rng.Next(1,7);b=rng.Next(1,7);c=rng.Next(1,7);category=rng.Next(0,3);
                string[] tools=["counters","pattern blocks","inch tiles"];
                var mixedTools=ShuffledItems(rng,("counter",a),("pattern block",b),("inch tile",c));
                return new(family,
                    $"Look at this mixed collection of tools and sort each object by its kind. " +
                    $"Tools: {mixedTools}. How many objects belong in the '{tools[category]}' category?",
                    "Sort by tool kind, count each once, then enter the labelled category count.",
                    AssessmentItemType.Numeric,D(("first",a),("second",b),("third",c),("category",category)));
            case "usccss.t001.g1.sort_shape_cards":
                a=rng.Next(1,7);b=rng.Next(1,7);c=rng.Next(1,7);category=rng.Next(0,3);
                string[] shapes=["triangles","squares","circles"];
                var mixedShapes=ShuffledItems(rng,("▲",a),("■",b),("●",c));
                return new(family,
                    $"Sort this mixed set of shape cards into three groups. " +
                    $"Cards: {mixedShapes}. How many cards belong in '{shapes[category]}'?",
                    "Separate shapes by defining geometry and record each category count.",
                    AssessmentItemType.Numeric,D(("first",a),("second",b),("third",c),("category",category)));
            case "usccss.t001.g1.class_survey_categories":
                a=rng.Next(1,8);b=rng.Next(1,8);c=rng.Next(1,8);category=rng.Next(0,3);
                string[] fruits=["apples","bananas","oranges"];
                var votes=ShuffledItems(rng,("apple",a),("banana",b),("orange",c));
                return new(family,
                    $"A fictional class survey asks each student to vote for one favorite fruit. " +
                    $"Unsorted votes: {votes}. Organize the votes in a labeled table. " +
                    $"How many votes belong in '{fruits[category]}'?",
                    "Record one vote per student, with clear labels for all three categories.",
                    AssessmentItemType.Numeric,D(("first",a),("second",b),("third",c),("category",category)));
            default:
                throw new InvalidOperationException("Unexpected T001 family.");
        }
    }

    private static bool ValidDomain(string f,IReadOnlyDictionary<string,int> p)
    {
        bool In(string key,int min,int max)=>
            p.TryGetValue(key,out var v)&&v>=min&&v<=max;
        int P(string key)=>p[key];
        return f switch
        {
            "usccss.t001.g1.dot_pair_sum"=>
                In("left",1,6)&&In("right",1,6)&&P("left")+P("right")<=10,
            "usccss.t001.g1.add_one_or_two"=>
                In("start",0,9)&&In("step",1,2)&&P("start")+P("step")<=10,
            "usccss.t001.g1.subtract_one_or_two"=>
                In("start",1,10)&&In("step",1,2)&&P("start")>=P("step"),
            "usccss.t001.g1.mixed_add_subtract"=>
                In("operation",0,1)&&In("left",0,10)&&In("right",1,10)&&
                (P("operation")==0 ? P("left")+P("right")<=10 :
                     P("left")>=P("right")),
            "usccss.t001.g1.make_ten_missing_part"=>
                In("known",0,10)&&In("missing",0,10)&&P("known")+P("missing")==10,
            "usccss.t001.g1.sort_math_tools" or
            "usccss.t001.g1.sort_shape_cards"=>
                In("first",1,6)&&In("second",1,6)&&In("third",1,6)&&In("category",0,2),
            "usccss.t001.g1.class_survey_categories"=>
                In("first",1,7)&&In("second",1,7)&&In("third",1,7)&&In("category",0,2),
            _=>false
        };
    }

    private static int Result(string f,IReadOnlyDictionary<string,int> p) =>
        f switch
        {
            "usccss.t001.g1.dot_pair_sum"=>p["left"]+p["right"],
            "usccss.t001.g1.add_one_or_two"=>p["start"]+p["step"],
            "usccss.t001.g1.subtract_one_or_two"=>p["start"]-p["step"],
            "usccss.t001.g1.mixed_add_subtract"=>
                p["operation"]==0?p["left"]+p["right"]:p["left"]-p["right"],
            "usccss.t001.g1.make_ten_missing_part"=>10-p["known"],
            "usccss.t001.g1.sort_math_tools" or
            "usccss.t001.g1.sort_shape_cards" or
            "usccss.t001.g1.class_survey_categories"=>
                p["category"] switch{0=>p["first"],1=>p["second"],2=>p["third"],
                    _=>throw new InvalidOperationException()},
            _=>throw new InvalidOperationException("Unexpected T001 family.")
        };

    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(f)||!ValidDomain(f,p))
            throw new InvalidOperationException("T001 invalid mathematical question domain.");
        return Result(f,p).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer) =>
        Supports(f)&&ValidDomain(f,p)&&
        int.TryParse(answer,NumberStyles.Integer,CultureInfo.InvariantCulture,out var supplied)&&
        supplied==Result(f,p);
}

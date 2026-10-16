using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM2021 G1 Unit 2 L04-L07 story, choice center,
/// no-action total and two-color partition tasks.
/// Integer responses are exact and verified against constrained source parameters.
/// </summary>
internal static class UsCcssG1Unit2Part2Questions
{
    public const string MixedStory="usccss.g1.u02.story.mixed_result_or_change";
    public const string CenterStory="usccss.g1.u02.center.picture_story_equation";
    public const string PetsTotal="usccss.g1.u02.pets.put_together_total";
    public const string ShakeSpill="usccss.g1.u02.shake_spill.two_unknown_parts";
    private static readonly string[] Things=["books","cards","stickers"];
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>
        family is MixedStory or CenterStory or PetsTotal or ShakeSpill;

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G1 U02 Part2 family");
        string prompt,solution;
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        if(family==MixedStory)
        {
            var start=rng.Next(3,8);
            var change=rng.Next(1,4);
            var mode=rng.Next(3);
            var context=rng.Next(Things.Length);
            p["start"]=start;p["change"]=change;p["mode"]=mode;p["context"]=context;
            var noun=Things[context];
            if(mode==0)
                prompt=$"A child has {start} {noun}. A friend gives {change} more. "+
                    "How many are there now? Draw the starting group and the joining group. "+
                    $"Complete {start} + {change} = [ ? ]. Enter the result.";
            else if(mode==1)
                prompt=$"A child has {start} {noun}. A friend gives some more. "+
                    $"Now the child has {start+change} {noun}. How many new {noun} were given? "+
                    $"Complete {start} + [ ? ] = {start+change}. Enter the missing change, not the final total.";
            else
                prompt=$"A child has {start} {noun} and gives away {change}. "+
                    "How many remain? Show a picture where some leave and complete "+
                    $"{start} - {change} = [ ? ]. Enter the result.";
            solution="Show the story with objects; distinguish the result from the change. "+
                "Box the number that actually answers the question and verify your equation.";
        }
        else if(family==CenterStory)
        {
            var start=rng.Next(4,9);
            var change=rng.Next(1,Math.Min(4,11-start));
            var action=rng.Next(2);
            var flip=rng.Next(2);
            p["start"]=start;p["change"]=change;p["action"]=action;p["flip"]=flip;
            var story=action==0
                ? $"The picture shows {start} balls. Then {change} more balls roll into the scene."
                : $"The picture shows {start} balls. Then {change} balls roll away.";
            var addition=$"{start} + {change} = {start+change}";
            var subtraction=$"{start} - {change} = {start-change}";
            var first=flip==0?addition:subtraction;
            var second=flip==0?subtraction:addition;
            prompt=story+" Tell your partner a complete math story and question. "+
                $"Which equation matches your story? Enter 1 or 2: 1) {first}; 2) {second}.";
            solution="Explain what the picture's action means. More joining needs addition; "+
                "some leaving needs subtraction. Point to each amount in the chosen equation.";
        }
        else if(family==PetsTotal)
        {
            var first=rng.Next(1,8);
            var second=rng.Next(1,11-first);
            var context=rng.Next(3);
            var flip=rng.Next(2);
            p["first"]=first;p["second"]=second;p["context"]=context;p["flip"]=flip;
            var kinds=new[]{"red and blue fish","cats and birds","rabbits and hamsters"}[context];
            var names=kinds.Split(" and ");
            prompt=$"There are {first} {names[0]} and {second} {names[1]}. "+
                "Nobody arrives or leaves. How many pets are there altogether? "+
                $"Draw the two parts; write {(flip==0?$"{first} + {second}":$"{second} + {first}")} = [ ? ]. "+
                "Enter the total and explain why switching the parts does not change it.";
            solution="Both parts exist already. Put the two groups together to make the whole. "+
                "Addition in either order gives the same total.";
        }
        else
        {
            var total=rng.Next(6,11);
            var first=rng.Next(1,total);
            var order=rng.Next(2);
            var color=rng.Next(2);
            p["total"]=total;p["first"]=first;p["order"]=order;p["color"]=color;
            prompt=$"Shake and Spill: you have {total} two-color counters in all. "+
                $"After spilling, {first} show {(color==0?"red":"yellow")}. "+
                "How many show the other color? "+
                (order==0?$"Complete {total} = {first} + [ ? ].":$"Complete {first} + [ ? ] = {total}.")+
                " Enter the missing part; imagine another way to split the same total.";
            solution="Count all counters, then the known color. The missing color plus the known "+
                "color must equal the total. The equals sign is true on either side.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))
            throw new InvalidOperationException("Invalid G1 U02 Part2 original source parameters");
        int answer=family switch
        {
            MixedStory=>p["mode"]==1?p["change"]:
                p["mode"]==2?p["start"]-p["change"]:p["start"]+p["change"],
            CenterStory=>(p["action"]==p["flip"])?1:2,
            PetsTotal=>p["first"]+p["second"],
            ShakeSpill=>p["total"]-p["first"],
            _=>throw new InvalidOperationException("Unknown family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string answer)=>
        Valid(family,parameters) &&
        string.Equals(answer,Solve(family,parameters),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==MixedStory)
            return p.Count==4 &&
                p.TryGetValue("start",out var start) &&
                p.TryGetValue("change",out var change) &&
                p.TryGetValue("mode",out var mode) &&
                p.TryGetValue("context",out var context) &&
                start is >=3 and <=7 && change is >=1 and <=3 &&
                start+change<=10 && mode is >=0 and <=2 &&
                context is >=0 and <=2;
        if(family==CenterStory)
            return p.Count==4 &&
                p.TryGetValue("start",out var cstart) &&
                p.TryGetValue("change",out var cchange) &&
                p.TryGetValue("action",out var action) &&
                p.TryGetValue("flip",out var flip) &&
                cstart is >=4 and <=8 && cchange is >=1 and <=3 &&
                cstart+cchange<=10 && action is 0 or 1 && flip is 0 or 1;
        if(family==PetsTotal)
            return p.Count==4 &&
                p.TryGetValue("first",out var first) &&
                p.TryGetValue("second",out var second) &&
                p.TryGetValue("context",out var pet) &&
                p.TryGetValue("flip",out var order) &&
                first is >=1 and <=7 && second>=1 && first+second<=10 &&
                pet is >=0 and <=2 && order is 0 or 1;
        return p.Count==4 &&
            p.TryGetValue("total",out var total) &&
            p.TryGetValue("first",out var known) &&
            p.TryGetValue("order",out var eqOrder) &&
            p.TryGetValue("color",out var color) &&
            total is >=6 and <=10 && known>=1 && known<total &&
            eqOrder is 0 or 1 && color is 0 or 1;
    }
}

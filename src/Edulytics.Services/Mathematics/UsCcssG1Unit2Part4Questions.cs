using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Five source-specific G1 U02 L11-L15 Practice families. Numeric/choice answer
/// correctness is independently testable; drawn representations and actual
/// classroom data/centers are NOT automatically certified by a numeric answer.
/// </summary>
internal static class UsCcssG1Unit2Part4Questions
{
    public const string Towers="usccss.g1.u02.towers.difference";
    public const string Supplies="usccss.g1.u02.supplies.difference";
    public const string ArtData="usccss.g1.u02.artdata.difference";
    public const string Equations="usccss.g1.u02.equations.match_difference";
    public const string MixedStories="usccss.g1.u02.story.mixed_unknowns";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is Towers or Supplies or ArtData or Equations or MixedStories;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))
            throw new InvalidOperationException("Unrecognized original G1 U02 L11-L15 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        var larger=random.Next(6,11);
        var smaller=random.Next(1,larger);
        if(family==Towers)
        {
            var method=random.Next(2);
            p["larger"]=larger;p["smaller"]=smaller;p["method"]=method;
            prompt=$"Make Them the Same: Diego has a tower of {larger} cubes and Jada a tower "+
                $"of {smaller} cubes. How many cubes must "+
                (method==0?"Jada ADD to her shorter tower":"Diego REMOVE from his taller tower")+
                " to make the towers equal? Picture aligned towers and enter the difference.";
            solution="Line up the bases, count unmatched cubes, then check that "+
                "shorter + difference = taller and taller - shorter = difference.";
        }
        else if(family==Supplies)
        {
            var kind=random.Next(3);
            p["larger"]=larger;p["smaller"]=smaller;p["kind"]=kind;
            prompt=kind switch
            {
                0=>$"School Supplies: {larger} students need pencils but only {smaller} pencils "+
                    "are available. How many students will not receive a pencil?",
                1=>$"School Supplies: {larger} markers need caps but only {smaller} caps "+
                    "are available. How many more caps are needed?",
                _=>$"School Supplies: {larger} notebooks each need one label. "+
                    $"There are {smaller} labels. How many additional labels are needed?"
            };
            solution="Pair objects one-to-one; count the unmatched objects. "+
                "Check using available + missing = needed.";
        }
        else if(family==ArtData)
        {
            var kind=random.Next(3);
            p["larger"]=larger;p["smaller"]=smaller;p["kind"]=kind;
            var first=kind switch {0=>"crayons",1=>"pencils",_=>"markers"};
            var second=kind switch {0=>"paintbrushes",1=>"glue sticks",_=>"scissors"};
            prompt=$"Illustrative art-supply preference chart (practice data, not a real "+
                $"student survey): {larger} marks for {first} and {smaller} for {second}. "+
                $"How many MORE marks does {first} have? Read the category labels "+
                "and enter the comparison difference, not the combined total.";
            solution="Pair the two categories' marks. Count the unpaired marks "+
                "or subtract the smaller count from the larger count.";
        }
        else if(family==Equations)
        {
            var mode=random.Next(2);
            var flip=random.Next(2);
            p["larger"]=larger;p["smaller"]=smaller;p["mode"]=mode;p["flip"]=flip;
            var difference=larger-smaller;
            var correct=mode==0?$"{larger} - {smaller} = {difference}":
                $"{smaller} + {difference} = {larger}";
            var incorrect=mode==0?$"{larger} + {smaller} = {difference}":
                $"{smaller} + {larger} = {difference}";
            prompt=$"Compare: There are {larger} glue sticks and {smaller} scissors. "+
                "How many fewer scissors? Which equation models that difference? "+
                (flip==0?$"1) {correct}; 2) {incorrect}":
                    $"1) {incorrect}; 2) {correct}")+
                ". Enter 1 or 2, and explain the connection between "+
                "the related addition and subtraction equations.";
            solution="The difference is larger minus smaller. The equivalent "+
                "addition statement starts at the smaller amount and adds the difference.";
        }
        else
        {
            var mode=random.Next(4);
            p["larger"]=larger;p["smaller"]=smaller;p["mode"]=mode;
            var other=larger-smaller;
            prompt=mode switch
            {
                0=>$"Different Types of Stories — find the whole: "+
                    $"There are {smaller} students and {other} teachers at a table. "+
                    "How many people in all?",
                1=>$"Different Types of Stories — find a part: "+
                    $"{larger} people are at a table and {smaller} are students. "+
                    "How many are teachers?",
                2=>$"Different Types of Stories — find the change: "+
                    $"{smaller} students are playing. More join and now {larger} "+
                    "students are playing. How many joined?",
                _=>$"Different Types of Stories — find the comparison: "+
                    $"There are {larger} red cubes and {smaller} blue cubes. "+
                    "How many MORE red cubes are there than blue cubes?"
            };
            solution="Draw the story, box the quantity asked for, and write "+
                "an addition or subtraction equation matching the whole, part, change, "+
                "or comparison unknown.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))
            throw new InvalidOperationException("Malformed G1 U02 L11-L15 source parameters");
        var larger=p["larger"];var smaller=p["smaller"];
        int answer=family switch
        {
            Equations=>p["flip"]==0?1:2,
            MixedStories when p["mode"]==0 =>larger,
            _=>larger-smaller
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) &&
        string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!p.TryGetValue("larger",out var larger) ||
           !p.TryGetValue("smaller",out var smaller) ||
           larger is <6 or >10 || smaller<1 || smaller>=larger)
            return false;
        if(family==Towers)
            return p.Count==3 && p.TryGetValue("method",out var method) &&
                method is 0 or 1;
        if(family is Supplies or ArtData)
            return p.Count==3 && p.TryGetValue("kind",out var kind) &&
                kind is >=0 and <=2;
        if(family==Equations)
            return p.Count==4 && p.TryGetValue("mode",out var mode) &&
                mode is 0 or 1 && p.TryGetValue("flip",out var flip) &&
                flip is 0 or 1;
        return p.Count==3 && p.TryGetValue("mode",out var mode) &&
            mode is >=0 and <=3;
    }
}

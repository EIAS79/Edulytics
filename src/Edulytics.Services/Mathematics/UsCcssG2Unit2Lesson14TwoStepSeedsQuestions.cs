using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party story-style numeric variants; source two-step structures preserved.</summary>
internal static class UsCcssG2Unit2Lesson14TwoStepSeedsQuestions
{
    public const string Triple="usccss.g2.u02.l14.three_addends_total";
    public const string Smaller="usccss.g2.u02.l14.compare_smaller_unknown";
    public const string Missing="usccss.g2.u02.l14.part_missing_unknown";
    public const string Result="usccss.g2.u02.l14.add_to_result_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Triple or Smaller or Missing or Result;
    public static Problem Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Not Grade2 source Lesson14");
        int a,b,c;string prompt;
        if(f==Triple)
        {
            a=r.Next(18,31);b=r.Next(11,25);c=r.Next(20,41);
            prompt=$"Independent TWO-STEP story: a learner collects {a} beads and {b} shells, then finds {c} pebbles. How many objects IN ALL? Solve intermediate then total.";
        }
        else if(f==Smaller)
        {
            a=r.Next(43,79);b=r.Next(12,31);c=0;
            prompt=$"First-party Compare Smaller Unknown: Tyler has {a} seeds and Priya has {b} FEWER. How many does Priya have?";
        }
        else if(f==Missing)
        {
            a=r.Next(50,81);b=r.Next(21,a-5);c=0;
            prompt=$"First-party missing part: two students have {a} seeds altogether. Jada has {b}. How many does Mai have?";
        }
        else
        {
            a=r.Next(21,61);b=r.Next(11,Math.Min(31,101-a));c=0;
            prompt=$"First-party change/result story: Kiran had {a} seeds and WON {b}. How many seeds does he have NOW?";
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"a",a},{"b",b},{"c",c}};
        int value=f==Triple?a+b+c:f==Smaller||f==Missing?a-b:a+b;
        return new Problem(f,prompt,$"The source-structured answer is {value}, using the labeled story quantities.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid source-structured two-step question");
        int value=f==Triple?p["a"]+p["b"]+p["c"]:f==Result?p["a"]+p["b"]:p["a"]-p["b"];
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(f,p)&&string.Equals(answer,Solve(f,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out var a)||
           !p.TryGetValue("b",out var b)||!p.TryGetValue("c",out var c))return false;
        return f switch{
            Triple=>a is >=18 and <=30&&b is >=11 and <=24&&c is >=20 and <=40&&a+b+c<=100,
            Smaller=>c==0&&a is >=43 and <=78&&b is >=12 and <=30&&a>b,
            Missing=>c==0&&a is >=50 and <=80&&b is >=21 and <=74&&b<a-5,
            Result=>c==0&&a is >=21 and <=60&&b is >=11 and <=30&&a+b<=100,
            _=>false
        };
    }
}

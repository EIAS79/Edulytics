using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics digital mathematics alongside the real hands-on IM2021
/// optional center. Digital entry NEVER attests a physical count or partner work.
/// </summary>
public static class UsCcssG2Unit1SeventeenthLessonCenters
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L17";
    public const string Version="edulytics-g2-u01-l17-count-to120-centers-v1";
    public static readonly IReadOnlyList<string> CenterNames=[
        "BehindBack","HowClose","NumberPuzzles","SortDisplay"
    ];
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe a genuine collection of 100–120 loose objects and a prior written estimate",
        "Check every object was grouped exactly once, in equal groups of 5 or 10 with leftovers visible",
        "Check each learner's OWN picture/equation accurately represents the observed count",
        "Observe two genuine previously taught center choices and discussion with a partner",
        "For student-created graphs, check true category data, title, labels and scale/key",
        "Separately assess the student's Number Talk explanation and counting strategy",
        "Digital numerical correctness alone never certifies the above classroom actions"
    ];
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Explanation);
    private static bool Read(string? s,int min,int max,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)
        && n>=min && n<=max;
    public static Review CheckWarmup(string? a,string? b,string? c,string? d)
    {
        var ok=Read(a,0,120,out var x)&&Read(b,0,120,out var y)
            &&Read(c,0,120,out var z)&&Read(d,0,120,out var w)
            &&x==37&&y==47&&z==57&&w==67;
        return new(ok,true,ok
            ?"All four totals are correct. Teacher checks your mental tens explanation."
            :"Recheck 27+10, 27+10+10, 27+30, and 37+30 using tens and ones.");
    }
    public static Review CheckDigitalCollection(string? grouping,string? groups,string? remainder,string? total)
    {
        var size=grouping switch {"Tens"=>10,"Fives"=>5,_=>0};
        var ok=size>0&&Read(groups,1,24,out var n)
            &&Read(remainder,0,9,out var r)&&r<size
            &&Read(total,100,120,out var v)
            &&size*n+r==112&&v==112;
        return new(ok,true,ok
            ?"The first-party model has 112 objects, independently check your bundles."
            :"For the FIRST-PARTY model use 11 tens plus 2 or 22 fives plus 2: exactly 112.");
    }
    public static Review CheckPhysicalConsistency(string? grouping,string? groups,string? remainder,string? total)
    {
        var size=grouping switch {"Tens"=>10,"Fives"=>5,_=>0};
        var ok=size>0&&Read(groups,1,24,out var n)
            &&Read(remainder,0,9,out var r)&&r<size
            &&Read(total,100,120,out var v)&&size*n+r==v;
        return new(ok,true,ok
            ?"The written equation is arithmetically consistent. A teacher must verify your REAL objects and drawings."
            :"The submitted groups and leftovers do not make a valid 100–120 total. This does NOT evaluate real objects.");
    }
    public static Review CheckChoice(string? choice,string? answer)
    {
        var expected=choice switch {
            "BehindBack"=>8,"HowClose"=>43,"NumberPuzzles"=>40,"SortDisplay"=>3,_=>-1
        };
        var ok=expected>=0&&Read(answer,0,50,out var value)&&value==expected;
        return new(ok,true,ok
            ?"Original Edulytics illustrative center arithmetic is correct. Teacher evaluates the real game/graph."
            :"Check the stated first-party arithmetic or graph example. Real center activity is teacher reviewed.");
    }
    public static Review CheckChoicePair(string? first,string? second)
    {
        var ok=CenterNames.Contains(first??"")&&CenterNames.Contains(second??"")&&first!=second;
        return new(ok,true,ok
            ?"Two distinct centers selected. Actual participation still needs teacher observation."
            :"Choose two DIFFERENT previously taught centers.");
    }
    public static Review CheckCoolDown(string? groupedCount,string? addWithin50,string? graphGap)
    {
        var ok=Read(groupedCount,100,120,out var count)&&count==117
            &&Read(addWithin50,0,50,out var add)&&add==43
            &&Read(graphGap,0,20,out var diff)&&diff==3;
        return new(ok,false,ok
            ?"Independent Edulytics exit mathematics correct: 117, 43 and 3."
            :"Check 23 groups of five plus 2; 19+24; and 8 minus 5.");
    }
}

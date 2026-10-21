using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Three bar-comparison numerical families with explicitly different lower
/// and upper bars, 0-20 axes and exact recomputation of the unknown gap.
/// </summary>
internal static class UsCcssG2Unit1Lesson13BarCompareQuestions
{
    public const string Difference="usccss.g2.u01.l13.bar_unknown_difference";
    public const string Add="usccss.g2.u01.l13.related_addition_missing_difference";
    public const string Subtract="usccss.g2.u01.l13.related_subtraction_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Difference or Add or Subtract;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G2 L13 compare family");
        var lower=rng.Next(1,20);
        var higher=rng.Next(lower+1,21);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["lower"]=lower,["higher"]=higher};
        var prompt=$"Edulytics original bar graph of a school club's choices, " +
            $"unit-one scale from 0 to 20: Art has {higher} choices, Science has {lower}. ";
        prompt+=family switch
        {
            Difference=>"How many MORE chose Art than Science? Enter the unknown DIFFERENCE, not the total.",
            Add=>$"Complete {lower} + ? = {higher}. Enter the missing addend, using counting ON along the bar.",
            Subtract=>$"Complete {higher} − {lower} = ?. Enter the difference, using counting BACK along the bars.",
            _=>throw new InvalidOperationException("Unsupported compare task")
        };
        return new Problem(family,prompt,
            $"The difference is {higher-lower}. Counting on: {lower}+{higher-lower}={higher}. Counting back: {higher}−{lower}={higher-lower}.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid bar compare parameters");
        return (p["higher"]-p["lower"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==2 &&
        p.TryGetValue("lower",out var l) && l is >=1 and <=19 &&
        p.TryGetValue("higher",out var h) && h is >=2 and <=20 && h>l;
}

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party imaginary ruler readings; no source long-side endpoint invented.</summary>
internal static class UsCcssG2Unit3Lesson10TornTapeQuestions
{
    public const string Difference="usccss.g2.u03.l10.subtract_nonzero_ticks";
    public const string CountOn="usccss.g2.u03.l10.count_on_to_endpoint";
    public const string Shift="usccss.g2.u03.l10.shifted_start_equal_length";
    public const string MissingEnd="usccss.g2.u03.l10.missing_endpoint_for_length";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? s)=>s is Difference or CountOn or Shift or MissingEnd;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L10 torn-tape family");
        int start=rng.Next(1,24),length=rng.Next(2,13),shift=rng.Next(1,11);
        // Ensure all ruler labels stay in a child-readable 0..50 range.
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"start",start},{"length",length},{"shift",shift}};
        int end=start+length;
        var prompt=family switch{
            Difference=>$"FIRST-PARTY virtual torn ruler: a real-looking object begins at tick{start} inches and ends at tick{end}. How many inches LONG is it, not what is the last label?",
            CountOn=>$"FIRST-PARTY count-on ruler question: {start} + ? = {end}. How many 1-inch SPACES lie between the two labels?",
            Shift=>$"FIRST-PARTY same object: measuring from{start} to{end} gives a length. Slide it over {shift} printed ticks, from{start+shift} to{end+shift}. What is the length after the slide?",
            _=>$"FIRST-PARTY missing ruler endpoint: an object starts at inch mark {start}, and is {length} inches long. At what END tick does it finish?"
        };
        int result=family==MissingEnd?end:length;
        return new Problem(family,prompt,$"Count tick intervals: {end}−{start}={length} inches. Sliding both labels by {shift} changes no length.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid virtual torn ruler parameters");
        return (family==MissingEnd?p["start"]+p["length"]:p["length"])
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==3&&p.TryGetValue("start",out var s)&&s is >=1 and <=23&&
        p.TryGetValue("length",out var l)&&l is >=2 and <=12&&
        p.TryGetValue("shift",out var shift)&&shift is >=1 and <=10&&s+l+shift<=50;
}

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original randomized seed stories preserving the structural role of each quantity.</summary>
internal static class UsCcssG2Unit2Lesson11SeedsStoriesQuestions
{
    public const string Gap="usccss.g2.u02.l11.compare_difference_unknown";
    public const string Left="usccss.g2.u02.l11.take_from_result_unknown";
    public const string Missing="usccss.g2.u02.l11.put_together_part_unknown";
    public const string Smaller="usccss.g2.u02.l11.compare_smaller_unknown";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Gap or Left or Missing or Smaller;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U02 L11");
        var whole=r.Next(30,51),part=r.Next(11,whole-5);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"whole",whole},{"part",part}};
        var prompt=family switch{
            Gap=>$"FIRST-PARTY compare story: Dalia gathered {whole} stones; Omar gathered {part}. How many MORE stones did Dalia gather?",
            Left=>$"FIRST-PARTY take-from story: Dalia had {whole} beads, gave Omar {part}. How many remain?",
            Missing=>$"FIRST-PARTY unknown part: a mosaic has {whole} total shapes, including {part} red shapes; how many blue shapes are there?",
            _=>$"FIRST-PARTY smaller unknown: Dalia has {whole} marbles, Omar has {part} FEWER. How many does Omar have?"
        };
        return new Problem(family,prompt,$"{whole}−{part}={whole-part}; the answer is the named unknown, not the combined sum.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid seed story parameters");
        return (p["whole"]-p["part"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==2&&p.TryGetValue("whole",out var whole)&&
        whole is >=30 and <=50&&p.TryGetValue("part",out var part)&&part>=11&&part<whole-5;
}

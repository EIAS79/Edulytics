using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent numeric intervals on an imagined cm ruler. Source A–F real lengths teacher-read.</summary>
internal static class UsCcssG2Unit3Lesson3BuildRulerQuestions
{
    public const string Tick="usccss.g2.u03.l03.ruler_tick_interval";
    public const string Shifted="usccss.g2.u03.l03.ruler_from_nonzero_start";
    public const string Compare="usccss.g2.u03.l03.compare_two_lengths";
    public const string Join="usccss.g2.u03.l03.join_two_lengths";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Tick or Shifted or Compare or Join;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L03 exact ruler variant");
        int start=family==Shifted?rng.Next(1,12):0;
        int end=rng.Next(start+1,26);
        int other=rng.Next(1,end-start+1);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"start",start},{"end",end},{"other",other}};
        int len=end-start;
        string prompt=family switch{
            Tick=>$"Original Edulytics numbered ruler tick0 to tick{end}: what is the centimeter length?",
            Shifted=>$"Original Edulytics ruler, marks{start} to{end}: what is the ACTUAL length in cm (not the endpoint label)?",
            Compare=>$"Independent invented rectangles: one {len}cm and another {other}cm. How much longer or equal is the first?",
            _=>$"Independent pair of line segments, lengths {len}cm and {other}cm, laid end-to-end with no overlap. What is total length in cm?"
        };
        int result=family==Compare?len-other:family==Join?len+other:len;
        return new Problem(family,prompt,$"Ruler span=end−start={end}−{start}={len}cm; second length={other}cm. Result={result}cm.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid ruler interval parameters");
        var n=p["end"]-p["start"],other=p["other"];
        var v=f switch{Tick or Shifted=>n,Compare=>n-other,Join=>n+other,_=>throw new InvalidOperationException()};
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(f,p)&&string.Equals(answer,Solve(f,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("start",out var start)||start is <0 or >24||
           !p.TryGetValue("end",out var end)||end is <1 or >25||end<=start||
           !p.TryGetValue("other",out var other)||other<1||other>end-start)return false;
        return f!=Tick||start==0;
    }
}

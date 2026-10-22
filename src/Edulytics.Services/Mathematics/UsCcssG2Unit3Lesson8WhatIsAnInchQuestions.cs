using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Virtual invented ruler/tile readings only, no source triangle precision assumed.</summary>
internal static class UsCcssG2Unit3Lesson8WhatIsAnInchQuestions
{
    public const string Tiles="usccss.g2.u03.l08.count_full_inch_tiles";
    public const string Ticks="usccss.g2.u03.l08.ruler_zero_to_end_ticks";
    public const string Below="usccss.g2.u03.l08.nearest_whole_below_half";
    public const string Above="usccss.g2.u03.l08.nearest_whole_above_half";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Tiles or Ticks or Below or Above;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L08 inches");
        var whole=rng.Next(1,11);
        var fractionalTenths=family==Below?rng.Next(1,5):family==Above?rng.Next(6,10):0;
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"whole",whole},{"tenths",fractionalTenths}};
        string prompt=family switch{
            Tiles=>$"FIRST-PARTY illustrated row has {whole} consecutive 1-inch square tiles, no gaps. How many WHOLE inches long is the row?",
            Ticks=>$"FIRST-PARTY invented ruler: object begins exactly at0 inches and ends at tick {whole} inches. What WHOLE-inch reading do you record?",
            Below=>$"FIRST-PARTY virtual endpoint is {whole}.{fractionalTenths} inches from ruler0. Report only the NEAREST WHOLE inch (don't write a decimal or half inch).",
            _=>$"FIRST-PARTY virtual endpoint is {whole}.{fractionalTenths} inches from ruler0. Report only the NEAREST WHOLE inch (don't write a decimal or half inch)."
        };
        var answer=family==Above?whole+1:whole;
        return new Problem(family,prompt,$"Virtual {whole}.{fractionalTenths} in has nearest whole {answer} inch(es); these are not source print shape measurements.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid virtual ruler data");
        return (family==Above?p["whole"]+1:p["whole"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=2||
           !p.TryGetValue("whole",out var whole)||whole is <1 or >10||
           !p.TryGetValue("tenths",out var tenths))return false;
        return family switch{
            Tiles or Ticks=>tenths==0,
            Below=>tenths is >=1 and <=4,
            Above=>tenths is >=6 and <=9,
            _=>false
        };
    }
}

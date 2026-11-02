using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent schematic centimeter lengths, not measurements of publisher reptile sheets.</summary>
internal static class UsCcssG2Unit3Lesson2MeasureCmQuestions
{
    public const string Cubes="usccss.g2.u03.l02.count_cm_cube_edges";
    public const string Rods="usccss.g2.u03.l02.count_full_ten_cm_rods";
    public const string Extra="usccss.g2.u03.l02.count_extra_cm_edges";
    public const string Compare="usccss.g2.u03.l02.compare_reptile_length_difference";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Cubes or Rods or Extra or Compare;
    public static Problem Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Not G2 U03 L02 source measurement family");
        int len=r.Next(12,81),shorter=r.Next(1,len);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lengthCm",len},{"shorterCm",shorter}};
        string prompt=f switch{
            Cubes=>$"FIRST-PARTY drawing of a {len}cm line: how many unit cubes with1cm edges align from end to end?",
            Rods=>$"FIRST-PARTY line {len}cm long: how many COMPLETE ten-centimeter rods fit without going past the endpoint? Any remaining length must use centimeter cubes.",
            Extra=>$"FIRST-PARTY line {len}cm long: after fitting the maximum number of10cm rods, how many additional1cm cubes are required?",
            _=>$"FIRST-PARTY invented lizard lengths (NOT source worksheet): one {len}cm, another {shorter}cm. How many centimeters longer is the first?"
        };
        int value=f switch{Cubes=>len,Rods=>len/10,Extra=>len%10,_=>len-shorter};
        return new Problem(f,prompt,$"{len}cm={len/10} full ten-cm rods and{len%10} one-cm cubes; comparison{len}−{shorter}={len-shorter}.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid first-party cm lengths");
        int len=p["lengthCm"];
        int result=f switch {Cubes=>len,Rods=>len/10,Extra=>len%10,Compare=>len-p["shorterCm"],_=>throw new InvalidOperationException()};
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(f,p)&&string.Equals(answer,Solve(f,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==2&&p.TryGetValue("lengthCm",out var len)&&len is >=12 and <=80&&
        p.TryGetValue("shorterCm",out var other)&&other>=1&&other<len;
}

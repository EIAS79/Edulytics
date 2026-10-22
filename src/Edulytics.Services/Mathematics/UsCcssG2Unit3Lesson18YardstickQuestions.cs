using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party exact inch-length mathematics without actual pupil measurements.</summary>
internal static class UsCcssG2Unit3Lesson18YardstickQuestions
{
    public const string Mark="usccss.g2.u03.l18.card_three_five_mark";
    public const string Edge="usccss.g2.u03.l18.card_edge_difference";
    public const string Gap="usccss.g2.u03.l18.fictional_measurement_gap";
    public const string Remaining="usccss.g2.u03.l18.remaining_to_yard";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Mark or Edge or Gap or Remaining;
    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 U03 L18");
        int count3=rng.Next(1,5),count5=rng.Next(1,5);
        while(3*count3+5*count5>36){count3--;if(count3==0){count3=1;count5--;}}
        int marked=3*count3+5*count5,desk1=rng.Next(20,32),desk2=rng.Next(desk1+1,37);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"count3",count3},{"count5",count5},
            {"marked",marked},{"desk1",desk1},{"desk2",desk2}};
        var prompt=family switch{
            Mark=>$"First-party 3×5 inch index-card example: measure {count3} short edges of3 inches and {count5} long edges of5 inches end-to-end with no gaps. What inch mark can that SUM locate?",
            Edge=>$"A 3-by-5 inch index card has edges 5 inches and3 inches. What is their difference in inches?",
            Gap=>$"FICTIONAL desk heights are {desk1} inches and{desk2} inches. What is the difference? Not real source class data.",
            _=>$"Yardstick length36in. A made-up marked length is {marked}in. How many inches remain to the36in end?"
        };
        int value=family switch{Mark=>marked,Edge=>2,Gap=>desk2-desk1,_=>36-marked};
        return new Problem(family,prompt,$"Exact first-party inch calculation: {value}. This answer cannot certify real ruler precision.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid yardstick arithmetic");
        int n=family switch{Mark=>p["marked"],Edge=>2,Gap=>p["desk2"]-p["desk1"],Remaining=>36-p["marked"],_=>throw new InvalidOperationException("Unknown family")};
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==5&&p.TryGetValue("count3",out var a)&&a is >=1 and <=4&&
        p.TryGetValue("count5",out var b)&&b is >=1 and <=4&&
        p.TryGetValue("marked",out var m)&&m==3*a+5*b&&m<=36&&
        p.TryGetValue("desk1",out var d1)&&d1 is >=20 and <=31&&
        p.TryGetValue("desk2",out var d2)&&d2>d1&&d2<=36;
}

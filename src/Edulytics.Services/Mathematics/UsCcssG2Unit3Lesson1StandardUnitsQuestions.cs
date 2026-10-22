using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party diagrams of equal segments, not simulated physical measurements.</summary>
internal static class UsCcssG2Unit3Lesson1StandardUnitsQuestions
{
    public const string Fives="usccss.g2.u03.l01.count_five_cm_straws";
    public const string Tens="usccss.g2.u03.l01.count_ten_cm_straws";
    public const string Cubes="usccss.g2.u03.l01.count_one_cm_cubes";
    public const string Compare="usccss.g2.u03.l01.compare_straw_unit_counts";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Fives or Tens or Cubes or Compare;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not G2 Unit3 Lesson1");
        int length=10*r.Next(1,11);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lengthCm",length}};
        string prompt=family switch{
            Fives=>$"FIRST-PARTY numerical diagram: a line is marked {length} centimeters long. How many equal 5-cm units fit end to end with no gaps?",
            Tens=>$"FIRST-PARTY numerical diagram: a line is marked {length} centimeters long. How many equal 10-cm units fit with no gaps?",
            Cubes=>$"FIRST-PARTY numerical diagram: a line is marked {length} centimeters long. How many TRUE one-centimeter cube edges fit along it?",
            _=>$"FIRST-PARTY compare: the same {length}-cm line uses {length/10} straws each10 cm. How MANY MORE pieces are needed if using5-cm straws instead?"
        };
        var result=family switch {Fives=>length/5,Tens=>length/10,Cubes=>length,Compare=>length/10,_=>0};
        return new Problem(family,prompt,$"{length}cm corresponds to{length/10} ten-cm units, {length/5} five-cm units, or{length} one-cm units; answer {result}. Physical measuring must be teacher-observed.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid unit-iteration parameters");
        var length=p["lengthCm"];
        return (family==Fives?length/5:family==Cubes?length:length/10)
            .ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)=>
        Supports(family)&&p.Count==1&&p.TryGetValue("lengthCm",out var len)&&
        len is >=10 and <=100&&len%10==0;
}

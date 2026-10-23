using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Original randomized story TYPES, not protected original cool-down wording.</summary>
internal static class UsCcssG2Unit3Lesson11SareeRibbonQuestions
{
    public const string Decompose="usccss.g2.u03.l11.ribbon_take_from_decompose";
    public const string Simple="usccss.g2.u03.l11.ribbon_take_from_no_decompose";
    public const string Start="usccss.g2.u03.l11.ribbon_start_unknown";
    public const string Cut="usccss.g2.u03.l11.ribbon_missing_cut_amount";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Decompose or Simple or Start or Cut;
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Not source ribbon story type");
        int whole,known;
        if(family==Decompose)
        {
            int tens=random.Next(3,10),ones=random.Next(0,9),subtractTens=random.Next(1,tens);
            int subtractOnes=random.Next(ones+1,10);
            whole=10*tens+ones;known=10*subtractTens+subtractOnes;
        }
        else if(family==Simple)
        {
            int tens=random.Next(3,10),ones=random.Next(1,10),subtractTens=random.Next(1,tens);
            int subtractOnes=random.Next(0,ones+1);
            whole=10*tens+ones;known=10*subtractTens+subtractOnes;
        }
        else if(family==Start)
        {
            known=random.Next(14,48);
            var left=random.Next(15,101-known);
            whole=known+left; // known=cut, unknown=start; left is described in prompt
        }
        else
        {
            whole=random.Next(45,100);known=random.Next(15,whole-10); // known=remaining, unknown=cut
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal){{"whole",whole},{"known",known}};
        var prompt=family switch{
            Decompose=>$"FIRST-PARTY ribbon story: a {whole}-inch ribbon has {known} inches cut away. How many inches REMAIN? Exchanging a ten helps.",
            Simple=>$"FIRST-PARTY ribbon story: a {whole}-inch ribbon loses {known} inches. How many inches REMAIN? Ones can be taken away directly.",
            Start=>$"FIRST-PARTY START UNKNOWN: a student cut off {known} inches and STILL has {whole-known} inches. How long was the ribbon BEFORE the cut?",
            _=>$"FIRST-PARTY CHANGE UNKNOWN: a ribbon originally measured {whole} inches and {known} inches remain. How many inches were CUT away?"
        };
        var answer=family==Start?whole:whole-known;
        var solution=family==Start?$"{known}+{whole-known}={whole} inches before cutting.":
            $"{whole}−{known}={whole-known} inches; label whole and part on a tape diagram.";
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid ribbon story numbers");
        return (family==Start?p["whole"]:p["whole"]-p["known"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=2||
            !p.TryGetValue("whole",out var whole)||whole is <30 or >100||
            !p.TryGetValue("known",out var known)||known is <10 or >99||known>=whole)return false;
        return family switch{
            Decompose=>whole/10>known/10&&whole%10<known%10,
            Simple=>whole/10>known/10&&whole%10>=known%10,
            Start=>known is >=14 and <=47&&whole-known>=15,
            Cut=>whole>=45&&known>=15&&whole-known>=11,
            _=>false
        };
    }
}

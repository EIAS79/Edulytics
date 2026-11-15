using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact rational and decimal additions for UAE G6 U07L03.
 /// Subtraction on each equation side leaves equality intact.</summary>
internal static class UaeG6U7L03AdditionEquationQuestions
{
    internal const string Fraction="uae.g6.u07.l03.fraction_addition_unknown";
    internal const string Decimal="uae.g6.u07.l03.decimal_addition_unknown";
    internal const string Property="uae.g6.u07.l03.identify_subtraction_property";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Fraction or Decimal or Property;
    private static int Gcd(int a,int b)
    {
        while(b!=0){int rem=a%b;a=b;b=rem;}
        return Math.Abs(a);
    }
    private static string Normalize(string? answer)=>new string((answer??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.','⁄'=>'/','−' or '–'=>'-',_=>ch}).ToArray());
    private static bool TryResult(string f,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(f)||p is null)return false;
        if(f==Property)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >1)return false;
            answer=mode==0?"نعم":"لا";return true;
        }
        if(f==Decimal)
        {
            if(p.Count!=2||!p.TryGetValue("totalCents",out int total)||
               !p.TryGetValue("partCents",out int part)||total is <1 or >100000||
               part is <1 or >99999||part>=total)return false;
            answer=((total-part)/100m).ToString("0.00",CultureInfo.InvariantCulture);
            return true;
        }
        if(p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("known",out int known)||!p.TryGetValue("total",out int totalNum)||
           d is <2 or >12||known is <1 or >30||totalNum is <2 or >40||totalNum<=known)return false;
        int numerator=totalNum-known,g=Gcd(numerator,d);
        answer=d/g==1?(numerator/g).ToString(CultureInfo.InvariantCulture):$"{numerator/g}/{d/g}";
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported addition equation family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(f==Property)
        {
            int mode=r.Next(2);p=new(){{"mode",mode}};
            prompt=mode==0?
              "هل طرح العدد نفسه من طرفَي معادلة صحيحة يحافظ على تساويهما؟ أجب نعم أو لا.":
              "هل طرح عدد من الطرف الأيمن فقط يضمن بقاء المعادلة متساوية؟ أجب نعم أو لا.";
        }
        else if(f==Decimal)
        {
            int total=r.Next(200,100001),part=r.Next(100,total);
            p=new(){{"totalCents",total},{"partCents",part}};
            prompt=$"أوجد س بالدرهم من المعادلة س+{(part/100m).ToString("0.00",CultureInfo.InvariantCulture)}={(total/100m).ToString("0.00",CultureInfo.InvariantCulture)}؛ اكتب العدد فقط.";
        }
        else
        {
            int d=r.Next(2,13),known=r.Next(1,13),unknown=r.Next(1,13),whole=known+unknown;
            p=new(){{"denominator",d},{"known",known},{"total",whole}};
            prompt=$"حل المعادلة س+{known}/{d}={whole}/{d}. اكتب س ككسر في أبسط صورة.";
        }
        return new Item(f,prompt,"نطرح المقدار المعلوم من طرفَي المعادلة؛ ونحافظ على قيمة الكسر أو العدد العشري بدقة.",
           AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryResult(f,p,out string answer)?answer:
          throw new InvalidOperationException("Invalid addition-equation parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !TryResult(f,p,out var expected))return false;
        string actual=Normalize(response);
        if(f==Property)return actual==expected;
        if(f==Decimal)return decimal.TryParse(actual,NumberStyles.Number,
            CultureInfo.InvariantCulture,out var x)&&
            decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,
              out var y)&&x==y;
        if(actual==expected)return true;
        int slash=actual.IndexOf('/');
        if(slash<1||slash==actual.Length-1||actual.LastIndexOf('/')!=slash)return false;
        if(!int.TryParse(actual[..slash],NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)||
           !int.TryParse(actual[(slash+1)..],NumberStyles.Integer,CultureInfo.InvariantCulture,out int d)||
           d<=0)return false;
        int targetSlash=expected.IndexOf('/');
        if(targetSlash<0)return int.TryParse(expected,NumberStyles.Integer,CultureInfo.InvariantCulture,out int target)&&n==target*d;
        return int.TryParse(expected[..targetSlash],NumberStyles.Integer,CultureInfo.InvariantCulture,out int en) &&
            int.TryParse(expected[(targetSlash+1)..],NumberStyles.Integer,CultureInfo.InvariantCulture,out int ed) &&
            (long)n*ed==(long)en*d;
    }
}

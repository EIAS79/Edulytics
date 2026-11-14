using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>UAE G6 subtraction equations with exact decimal and fraction answers.
 /// Adding the same amount to both sides preserves equality.</summary>
internal static class UaeG6U7L05SubtractionEquationQuestions
{
    internal const string Decimal="uae.g6.u07.l05.decimal_subtraction",Fraction="uae.g6.u07.l05.fraction_subtraction",Property="uae.g6.u07.l05.addition_property";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Decimal or Fraction or Property;
    private static int Gcd(int a,int b)
    {
        while(b!=0){int rem=a%b;a=b;b=rem;}
        return Math.Abs(a);
    }
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
          >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
          >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
          '٫'=>'.','−' or '–'=>'-','⁄'=>'/',_=>ch}).ToArray());
    private static bool TryAnswer(string f,IReadOnlyDictionary<string,int>? p,out string answer)
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
            if(p.Count!=2||!p.TryGetValue("removedCents",out int removed)||
               !p.TryGetValue("remainCents",out int remain)||removed is <1 or >50000||
               remain is <0 or >50000)return false;
            answer=((removed+remain)/100m).ToString("0.00",CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("removed",out int removedNum)||
           !p.TryGetValue("remain",out int remainNum)||
           d is <2 or >12||removedNum is <1 or >12||remainNum is <1 or >12)return false;
        int sum=removedNum+remainNum,g=Gcd(sum,d);
        answer=d/g==1?(sum/g).ToString(CultureInfo.InvariantCulture):$"{sum/g}/{d/g}";
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported subtraction equation");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(f==Property)
        {
            int mode=r.Next(2);p=new(){{"mode",mode}};
            prompt=mode==0?
              "هل جمع العدد نفسه إلى طرفَي معادلة صحيحة يحفظ التساوي؟ أجب نعم أو لا.":
              "هل إضافة العدد إلى أحد الطرفين فقط تحفظ التساوي دائمًا؟ أجب نعم أو لا.";
        }
        else if(f==Decimal)
        {
            int removed=r.Next(1,50001),remain=r.Next(0,50001);
            p=new(){{"removedCents",removed},{"remainCents",remain}};
            prompt=$"اوجد س من س−{(removed/100m).ToString("0.00",CultureInfo.InvariantCulture)}={(remain/100m).ToString("0.00",CultureInfo.InvariantCulture)}. اكتب القيمة بالدرهم.";
        }
        else
        {
            int d=r.Next(2,13),removed=r.Next(1,13),remain=r.Next(1,13);
            p=new(){{"denominator",d},{"removed",removed},{"remain",remain}};
            prompt=$"حل س−{removed}/{d}={remain}/{d}. اكتب س ككسر في أبسط صورة.";
        }
        return new Item(f,prompt,"نعكس الطرح بالجمع إلى طرفَي المساواة مع التحقق من المجهول.",
             AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryAnswer(f,p,out string answer)?answer:throw new InvalidOperationException("Invalid subtraction params");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !TryAnswer(f,p,out var expected))return false;
        string actual=Norm(response);
        if(f==Property)return actual==expected;
        if(f==Decimal)return decimal.TryParse(actual,NumberStyles.Number,
           CultureInfo.InvariantCulture,out decimal x)&&
           decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out decimal y)&&x==y;
        if(actual==expected)return true;
        var spl=actual.Split('/');
        if(spl.Length!=2||!int.TryParse(spl[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)||
            !int.TryParse(spl[1],NumberStyles.Integer,CultureInfo.InvariantCulture,out int d)||d<=0)return false;
        var tgt=expected.Split('/');
        if(tgt.Length==1)return int.TryParse(tgt[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int z)&&
            (long)n==(long)z*d;
        return int.TryParse(tgt[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int en) &&
           int.TryParse(tgt[1],NumberStyles.Integer,CultureInfo.InvariantCulture,out int ed) &&
           (long)n*ed==(long)en*d;
    }
}

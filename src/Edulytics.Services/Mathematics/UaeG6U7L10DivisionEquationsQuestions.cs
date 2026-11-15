using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact division-equation coefficient inversion including decimal,
/// fraction and unknown divisor. Open explanations are teacher reviewed.</summary>
internal static class UaeG6U7L10DivisionEquationsQuestions
{
    internal const string Decimal="uae.g6.u07.l10.decimal_divisor";
    internal const string Rational="uae.g6.u07.l10.rational_divisor";
    internal const string UnknownDivisor="uae.g6.u07.l10.variable_denominator";
    internal const string Property="uae.g6.u07.l10.multiplication_property";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Decimal or Rational or UnknownDivisor or Property;
    private static int Gcd(int a,int b)
    {
        while(b!=0){int r=a%b;a=b;b=r;}
        return Math.Abs(a);
    }
    private static string Norm(string? s)=>new string((s??"").Where(c=>!char.IsWhiteSpace(c))
        .Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.','⁄'=>'/','−' or '–'=>'-',_=>c}).ToArray());
    private static bool TryExpected(string f,IReadOnlyDictionary<string,int>? p,out string answer)
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
            if(p.Count!=2||!p.TryGetValue("divisor100",out int div)||
               !p.TryGetValue("quotient",out int quot)||div is <10 or >1500||
               quot is <1 or >50)return false;
            answer=(div*quot/100m).ToString("0.##",CultureInfo.InvariantCulture);return true;
        }
        if(f==UnknownDivisor)
        {
            if(p.Count!=2||!p.TryGetValue("numerator",out int num)||
               !p.TryGetValue("quotient",out int q)||num is <2 or >600||
               q is <1 or >50||num%q!=0)return false;
            answer=(num/q).ToString(CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("num",out int n)||
           !p.TryGetValue("den",out int d)||!p.TryGetValue("q",out int quotient)||
           n is <1 or >15||d is <2 or >20||quotient is <1 or >50)return false;
        int top=n*quotient,g=Gcd(top,d);
        answer=d/g==1?(top/g).ToString(CultureInfo.InvariantCulture):$"{top/g}/{d/g}";
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported division equation");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(f==Property)
        {
            int mode=r.Next(2);p=new(){{"mode",mode}};
            prompt=mode==0?"هل ضرب طرفي معادلة صحيحة في العدد نفسه يحفظ التساوي؟ اكتب نعم أو لا.":
            "هل يُسمح باختزال مجهول قد يساوي صفرًا من طرفَي معادلة دون فحص؟ اكتب نعم أو لا.";
        }
        else if(f==Decimal)
        {
            int div=r.Next(10,1501),q=r.Next(1,51);
            p=new(){{"divisor100",div},{"quotient",q}};
            prompt=$"حل س÷{(div/100m).ToString("0.00",CultureInfo.InvariantCulture)}={q}. احسب س دون تقريب.";
        }
        else if(f==UnknownDivisor)
        {
            int q=r.Next(1,26),den=r.Next(1,21),total=q*den;
            p=new(){{"numerator",total},{"quotient",q}};
            prompt=$"حل {total}÷س={q}. يجب أن يكون س غير صفر.";
        }
        else
        {
            int n=r.Next(1,16),d=r.Next(2,21),q=r.Next(1,51);
            p=new(){{"num",n},{"den",d},{"q",q}};
            prompt=$"حل س÷({n}/{d})={q} واكتب س كعدد صحيح أو كسر في أبسط صورة.";
        }
        return new Item(f,prompt,"نضرب الطرفين في المقسوم عليه غير الصفري أو نعكس القسمة بالضرب ثم نتحقق.",
          AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryExpected(f,p,out string answer)?answer:throw new InvalidOperationException("Invalid division parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !TryExpected(f,p,out string expected))return false;
        string s=Norm(response);
        if(f==Property)return s==expected;
        if(s==expected)return true;
        if(f==Decimal||f==UnknownDivisor)
            return decimal.TryParse(s,NumberStyles.Number,CultureInfo.InvariantCulture,out decimal v)&&
                   decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out decimal exp)&&v==exp;
        string[] parts=s.Split('/');
        if(parts.Length!=2||
           !int.TryParse(parts[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int sn)||
           !int.TryParse(parts[1],NumberStyles.Integer,CultureInfo.InvariantCulture,out int sd)||
           sd<=0)return false;
        string[] expectedParts=expected.Split('/');
        if(expectedParts.Length==1)
            return int.TryParse(expectedParts[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int integer)&&
                (long)sn==(long)integer*sd;
        return int.TryParse(expectedParts[0],NumberStyles.Integer,CultureInfo.InvariantCulture,out int en)&&
               int.TryParse(expectedParts[1],NumberStyles.Integer,CultureInfo.InvariantCulture,out int ed)&&
               (long)sn*ed==(long)en*sd;
    }
}

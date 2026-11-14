using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact decimal/rational coefficients in G6 multiplication equations.</summary>
internal static class UaeG6U7L08MultiplicationEquationsQuestions
{
    internal const string Decimal="uae.g6.u07.l08.decimal_coefficient";
    internal const string Fraction="uae.g6.u07.l08.fraction_coefficient";
    internal const string Property="uae.g6.u07.l08.division_property";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Decimal or Fraction or Property;
    private static int Gcd(int a,int b)
    {
        while(b!=0){int r=a%b;a=b;b=r;}
        return Math.Abs(a);
    }
    private static string Norm(string? answer)=>new string((answer??"").Where(c=>!char.IsWhiteSpace(c))
        .Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.','⁄'=>'/', '−' or '–'=>'-',_=>c}).ToArray());
    private static bool TryAnswer(string f,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(f)||p is null)return false;
        if(f==Property)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >1)return false;
            result=mode==0?"نعم":"لا";return true;
        }
        if(f==Decimal)
        {
            if(p.Count!=2||!p.TryGetValue("coef100",out int a)||!p.TryGetValue("value",out int x)||
                a is <10 or >900||x is <1 or >50)return false;
            result=x.ToString(CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("numerator",out int n)||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("value",out int val)||n is <1 or >20||d is <2 or >20||
           val is <1 or >50)return false;
        result=val.ToString(CultureInfo.InvariantCulture);return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported coefficient");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(f==Property)
        {
            int mode=r.Next(2);p=new(){{"mode",mode}};
            prompt=mode==0?"هل قسمة طرفَي معادلة صحيحة على العدد غير الصفري نفسه تحفظ المساواة؟ أجب نعم أو لا.":
            "هل القسمة على صفر مسموحة في خاصية القسمة في المعادلة؟ أجب نعم أو لا.";
        }
        else if(f==Decimal)
        {
            int coef=r.Next(10,901),val=r.Next(1,51);
            p=new(){{"coef100",coef},{"value",val}};
            string a=(coef/100m).ToString("0.00",CultureInfo.InvariantCulture),
                total=(coef*val/100m).ToString("0.00",CultureInfo.InvariantCulture);
            prompt=$"حل {a}س={total} بالقسمة على معامل س غير الصفري. اكتب قيمة س.";
        }
        else
        {
            int d=r.Next(2,21),n=r.Next(1,21),val=r.Next(1,51);
            p=new(){{"numerator",n},{"denominator",d},{"value",val}};
            int top=n*val,g=Gcd(top,d);
            string total=d/g==1?$"{top/g}":$"{top/g}/{d/g}";
            prompt=$"حل ({n}/{d})س={total}. اكتب س عددًا صحيحًا.";
        }
        return new Item(f,prompt,"اقسم الطرفين على المعامل غير الصفري ثم تحقق بالضرب الدقيق.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryAnswer(f,p,out var value)?value:throw new InvalidOperationException("Invalid coefficient parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>70||
           !TryAnswer(f,p,out string expected))return false;
        string actual=Norm(response);
        if(f==Property)return actual==expected;
        return decimal.TryParse(actual,NumberStyles.Number,CultureInfo.InvariantCulture,out var num)&&
            decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out var val)&&num==val;
    }
}

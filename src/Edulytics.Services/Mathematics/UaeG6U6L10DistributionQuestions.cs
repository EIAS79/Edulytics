using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>G6 Arabic U06L10: exact distributive expansion, factoring greatest
/// common factor, decimal mixed arithmetic and bounded true/false equivalence.</summary>
internal static class UaeG6U6L10DistributionQuestions
{
    internal const string Expand="uae.g6.u06.l10.expand_two_term",Factor="uae.g6.u06.l10.factor_common",
        Mixed="uae.g6.u06.l10.mixed_decimal_distribute",Equivalence="uae.g6.u06.l10.distribution_true_false";
    internal const string Combine="uae.g6.u06.l11.combine_like_terms",Pair="uae.g6.u06.l11.equivalent_pair",Tiles="uae.g6.u06.l11.count_variable_tiles";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Expand or Factor or Mixed or Equivalence or Combine or Pair or Tiles;
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '×' or '·'=>'*','−' or '–'=>'-','٫'=>'.',_=>ch}).ToArray());
    private static int Gcd(int a,int b)
    {
        while(b!=0){int remainder=a%b;a=b;b=remainder;}
        return Math.Abs(a);
    }
    private static bool TryAnswer(string family,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(family)||p is null)return false;
        if(family is Combine or Pair or Tiles)
        {
            if(!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("c",out int c)||a is <1 or >15||b is <1 or >30||
               c is <1 or >15)return false;
            if(family==Pair)
            {
                if(p.Count!=4||!p.TryGetValue("mode",out int mode)||
                   mode is not (0 or 1))return false;
                result=mode==0?"نعم":"لا";return true;
            }
            if(p.Count!=3)return false;
            result=family==Combine?$"{a+c}س+{b}":
                (a+c).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if(family==Mixed)
        {
            if(p.Count!=4||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("whole",out int whole)||!p.TryGetValue("n",out int n)||
               !p.TryGetValue("d",out int d)||a is <2 or >12||
               whole is <1 or >12||d is not (2 or 4 or 5 or 10)||
               n is <1 or >9||n>=d)return false;
            result=(a*(whole+n/(decimal)d)).ToString("0.###",CultureInfo.InvariantCulture);
            return true;
        }
        if(family==Equivalence)
        {
            if(p.Count!=3||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||!p.TryGetValue("mode",out int mode)||
               a is <2 or >12||b is <1 or >20||mode is <0 or >1)return false;
            result=mode==0?"نعم":"لا";return true;
        }
        if(family==Expand)
        {
            if(p.Count!=3||!p.TryGetValue("a",out int a)||
                !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
                a is <2 or >12||b is <2 or >9||c is <1 or >20)return false;
            result=$"{a*b}س+{a*c}";return true;
        }
        if(p.Count!=2||!p.TryGetValue("coeff",out int coeff)||
           !p.TryGetValue("constant",out int constant)||
           coeff is <2 or >120||constant is <2 or >120)return false;
        int g=Gcd(coeff,constant);
        if(g<2)return false;
        result=$"{g}({(coeff/g==1?"":(coeff/g).ToString(CultureInfo.InvariantCulture))}س+{constant/g})";
        return true;
    }
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported distributive family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string question;
        if(family is Combine or Pair or Tiles)
        {
            int a=r.Next(1,16),b=r.Next(1,31),c=r.Next(1,16);
            p=new(){{"a",a},{"b",b},{"c",c}};
            if(family==Pair)
            {
                int mode=r.Next(2);p["mode"]=mode;
                int otherConstant=b+(mode==0?0:1);
                question=$"هل التعبيران {a}س+{b}+{c}س و{a+c}س+{otherConstant} متكافئان لكل قيمة س؟ اكتب نعم أو لا.";
            }
            else if(family==Tiles)
                question=$"نموذج جبري يحوي {a} قطع س و{b} قطع آحاد و{c} قطع س إضافية. كم قطعة س بعد الجمع؟";
            else
                question=$"بسّط التعبير {a}س+{b}+{c}س بجمع الحدود المتشابهة؛ احتفظ بالحد الثابت.";
        }
        else if(family==Mixed)
        {
            int[] denominators=[2,4,5,10];
            int d=denominators[r.Next(denominators.Length)],whole=r.Next(1,13),
                n=r.Next(1,d),a=r.Next(2,13);
            p=new(){{"a",a},{"whole",whole},{"n",n},{"d",d}};
            question=$"استخدم التوزيع لإيجاد {a} × ({whole} + {n}/{d}). اكتب الناتج بدقة.";
        }
        else if(family==Equivalence)
        {
            int a=r.Next(2,13),b=r.Next(1,21),mode=r.Next(2);
            p=new(){{"a",a},{"b",b},{"mode",mode}};
            string right=mode==0?$"{a}س + {a*b}":$"{a}س + {b}";
            question=$"هل التعبيران {a}(س+{b}) و{right} متكافئان لكل س؟ اكتب نعم أو لا.";
        }
        else if(family==Expand)
        {
            int a=r.Next(2,13),b=r.Next(2,10),c=r.Next(1,21);
            p=new(){{"a",a},{"b",b},{"c",c}};
            question=$"وسّع التعبير {a}({b}س+{c}) دون أقواس باستعمال التوزيع.";
        }
        else
        {
            int g=r.Next(2,13),b=r.Next(1,10),c=r.Next(1,11);
            p=new(){{"coeff",g*b},{"constant",g*c}};
            question=$"حلل {g*b}س+{g*c} بإخراج العامل المشترك الأكبر.";
        }
        return new Item(family,question,
            "اضرب كل حد عند التوزيع أو أخرج العامل المشترك الأكبر بدقة؛ القيم العشرية لا تُقرّب.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryAnswer(f,p,out string answer)?answer:throw new InvalidOperationException("Invalid distribution");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>100||
           !TryAnswer(f,p,out string expected))return false;
        string answer=Norm(response),wanted=Norm(expected);
        if(answer==wanted)return true;
        if(f==Combine)
        {
            int a=p["a"],b=p["b"],c=p["c"];
            return answer==$"{a+c}*س+{b}"||answer==$"{b}+{a+c}س";
        }
        if(f==Expand)
        {
            int a=p["a"],b=p["b"],c=p["c"];
            return answer==$"{a*b}*س+{a*c}";
        }
        if(f==Factor)
        {
            int coeff=p["coeff"],constant=p["constant"],g=Gcd(coeff,constant);
            string coeffPart=coeff/g==1?"":(coeff/g).ToString(CultureInfo.InvariantCulture);
            return answer==$"{g}*({coeffPart}س+{constant/g})";
        }
        if(f==Mixed)
        {
            return decimal.TryParse(answer,NumberStyles.Number,CultureInfo.InvariantCulture,out var value)&&
                decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out var target)&&value==target;
        }
        return false;
    }
}

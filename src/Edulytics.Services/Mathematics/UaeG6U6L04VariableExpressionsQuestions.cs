using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Exact bounded substitution for G6 Arabic variable expressions.
/// Existing exact grading is reused. Freehand models, stories and proof
/// remain teacher-reviewed rather than scored by fabricated parsing.</summary>
internal static class UaeG6U6L04VariableExpressionsQuestions
{
    internal const string Add="uae.g6.u06.l04.substitute_add";
    internal const string Subtract="uae.g6.u06.l04.substitute_difference";
    internal const string Multiply="uae.g6.u06.l04.substitute_product";
    internal const string Divide="uae.g6.u06.l04.substitute_division";
    internal const string Linear="uae.g6.u06.l04.substitute_linear";
    internal const string FractionSquared="uae.g6.u06.l04.fraction_squared";
    internal const string TwoVariables="uae.g6.u06.l04.two_variable_expression";
    internal const string Cube="uae.g6.u06.l04.cube_volume";
    internal const string Money="uae.g6.u06.l04.money_remaining";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Add or Subtract or Multiply
        or Divide or Linear or FractionSquared or TwoVariables or Cube or Money;

    private static int Gcd(int a,int b)
    {
        while(b!=0){int rem=a%b;a=b;b=rem;}
        return Math.Abs(a);
    }
    private static string FractionAnswer(int num,int den)
    {
        if(den<=0)throw new InvalidOperationException("Invalid denominator");
        int gcd=Gcd(Math.Abs(num),den);
        num/=gcd;den/=gcd;
        return den==1?num.ToString(CultureInfo.InvariantCulture):$"{num}/{den}";
    }
    private static string Format(decimal d)=>d.ToString("0.####################",CultureInfo.InvariantCulture);
    private static string Normalize(string? s)
    {
        if(string.IsNullOrWhiteSpace(s))return "";
        return new string(s.Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ('(' or ')'))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '٫'=>'.','−' or '–'=>'-','⁄'=>'/',_=>ch}).ToArray());
    }
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(family)||p is null)return false;
        if(family==FractionSquared||family==Cube)
        {
            if(p.Count!=2||!p.TryGetValue("n",out int n)||!p.TryGetValue("d",out int d)||
               n is <1 or >5||d is <2 or >10||n>=d)return false;
            expected=family==FractionSquared?FractionAnswer(4*n*n,d*d):
                FractionAnswer(n*n*n,d*d*d);
            return true;
        }
        if(family==Money)
        {
            if(p.Count!=3||!p.TryGetValue("initial",out int initial)||
               !p.TryGetValue("price",out int price)||!p.TryGetValue("q",out int q)||
               initial is <50 or >500||price is <1 or >40||q is <1 or >10)return false;
            expected=Format(initial-price*q);return true;
        }
        if(family==TwoVariables)
        {
            if(p.Count!=4||!p.TryGetValue("x",out int x)||!p.TryGetValue("y",out int y)||
               !p.TryGetValue("cx",out int cx)||!p.TryGetValue("cy",out int cy)||
               x is <1 or >40||y is <1 or >40||cx is <1 or >12||cy is <1 or >12)return false;
            expected=Format(cx*x+cy*y);return true;
        }
        if(family==Subtract)
        {
            if(p.Count!=2||!p.TryGetValue("x",out int x)||!p.TryGetValue("y",out int y)||
               x is <0 or >100||y is <0 or >100)return false;
            expected=Format(x-y);return true;
        }
        if(family==Divide)
        {
            if(p.Count!=2||!p.TryGetValue("x",out int x)||!p.TryGetValue("d",out int d)||
               x is <0 or >100||d is <2 or >12)return false;
            expected=Format(x/(decimal)d);return true;
        }
        if(family==Add)
        {
            if(p.Count!=2||!p.TryGetValue("x",out int x)||!p.TryGetValue("a",out int a)||
               x is <0 or >100||a is <1 or >30)return false;
            expected=Format(x+a);return true;
        }
        if(family==Multiply)
        {
            if(p.Count!=2||!p.TryGetValue("x",out int x)||!p.TryGetValue("a",out int a)||
               x is <0 or >100||a is <1 or >30)return false;
            expected=Format(x*a);return true;
        }
        if(family==Linear)
        {
            if(p.Count!=3||!p.TryGetValue("x",out int x)||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||x is <0 or >30||
               a is <2 or >12||b is <1 or >25)return false;
            expected=Format(a*x+b);return true;
        }
        return false;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported substitution family");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string prompt;
        if(family is FractionSquared or Cube)
        {
            int d=random.Next(2,11),n=random.Next(1,Math.Min(d,6));
            p=new(){{"n",n},{"d",d}};
            prompt=family==FractionSquared?
                $"إذا كان س={n}/{d}، احسب قيمة ٤س² واكتب الناتج كسرًا في أبسط صورة.":
                $"طول ضلع مكعب س={n}/{d} متر، احسب قيمة س³ ككسر في أبسط صورة.";
        }
        else if(family==Money)
        {
            int initial=random.Next(50,501),price=random.Next(1,41),q=random.Next(1,11);
            p=new(){{"initial",initial},{"price",price},{"q",q}};
            prompt=$"من مبلغ {initial} درهمًا اشترى طالب {q} هدايا سعر الواحدة {price} درهمًا. احسب قيمة التعبير {initial}−{q}×{price}.";
        }
        else if(family==TwoVariables)
        {
            int x=random.Next(1,41),y=random.Next(1,41),
                cx=random.Next(1,13),cy=random.Next(1,13);
            p=new(){{"x",x},{"y",y},{"cx",cx},{"cy",cy}};
            prompt=$"إذا س={x} وص={y}، أوجد قيمة {cx}س+{cy}ص. اكتب الناتج.";
        }
        else if(family==Subtract)
        {
            int x=random.Next(0,101),y=random.Next(0,101);
            p=new(){{"x",x},{"y",y}};
            prompt=$"إذا س={x} وص={y}، ما قيمة س−ص؟";
        }
        else if(family==Divide)
        {
            int d=random.Next(2,13),x=random.Next(0,101);
            p=new(){{"x",x},{"d",d}};
            prompt=$"إذا س={x}، احسب س÷{d} (اكتب القيمة العددية).";
        }
        else if(family==Linear)
        {
            int x=random.Next(0,31),a=random.Next(2,13),b=random.Next(1,26);
            p=new(){{"x",x},{"a",a},{"b",b}};
            prompt=$"إذا س={x}، احسب {a}س+{b} بعد التعويض.";
        }
        else
        {
            int x=random.Next(0,101),a=random.Next(1,31);
            p=new(){{"x",x},{"a",a}};
            prompt=family==Add?$"إذا س={x}، احسب س+{a}.":
                $"إذا س={x}، احسب {a}س.";
        }
        return new Item(family,prompt,
            "استبدل المتغير بالقيمة المعطاة مع الحفاظ على ترتيب العمليات؛ لا تعامل الحرف كحرف مجهول بعد التعويض.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out var expected)?expected:
            throw new InvalidOperationException("Invalid substitution parameters.");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryExpected(family,p,out var expected)||string.IsNullOrWhiteSpace(answer)||
           answer.Length>90)return false;
        string normalized=Normalize(answer);
        if(family is FractionSquared or Cube)
        {
            if(string.Equals(normalized,expected,StringComparison.Ordinal))return true;
            if(!expected.Contains('/'))return false;
            var parts=expected.Split('/');
            if(!int.TryParse(parts[0],NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out int n)||
               !int.TryParse(parts[1],NumberStyles.None,
                CultureInfo.InvariantCulture,out int d))return false;
            return decimal.TryParse(normalized,NumberStyles.Number,
                CultureInfo.InvariantCulture,out decimal response)&&response*d==n;
        }
        return decimal.TryParse(normalized,NumberStyles.Number,
            CultureInfo.InvariantCulture,out decimal parsed)&&
            decimal.TryParse(expected,NumberStyles.Number,
            CultureInfo.InvariantCulture,out decimal wanted)&&parsed==wanted;
    }
}

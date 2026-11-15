using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Exact bounded Grade 6 powers and exponents, including decimal
/// and rational bases. Uses integer/decimal arithmetic; no Math.Pow rounding.</summary>
internal static class UaeG6U6L02PowersQuestions
{
    internal const string Repeated="uae.g6.u06.l02.repeated_to_power";
    internal const string Integer="uae.g6.u06.l02.integer_power";
    internal const string Decimal="uae.g6.u06.l02.decimal_power";
    internal const string Fraction="uae.g6.u06.l02.fraction_power";
    internal const string Ten="uae.g6.u06.l02.ten_power";
    internal const string Addition="uae.g6.u06.l02.power_addition";
    internal const string Square="uae.g6.u06.l02.perfect_square";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Repeated or Integer
        or Decimal or Fraction or Ten or Addition or Square;

    private static decimal Pow(decimal b,int exponent)
    {
        decimal result=1m;
        for(int i=0;i<exponent;i++)result=checked(result*b);
        return result;
    }
    private static string Format(decimal n)=>n.ToString("0.############################",CultureInfo.InvariantCulture);
    private static string Normalize(string answer)
    {
        string stripped=new(answer.Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ('(' or ')' or '{' or '}'))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '٫'=>'.','−' or '–'=>'-','⁄'=>'/','＾'=>'^','،'=>',',
                _=>ch}).ToArray());
        return stripped;
    }
    private static int Gcd(int a,int b)
    {
        while(b!=0){int rem=a%b;a=b;b=rem;}
        return Math.Abs(a);
    }
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(family)||p is null)return false;
        if(family==Ten)
        {
            if(p.Count!=1||!p.TryGetValue("e",out int e)||e is <0 or >8)return false;
            expected=Format(Pow(10m,e));return true;
        }
        if(family==Square)
        {
            if(p.Count!=2||!p.TryGetValue("b",out int b)||!p.TryGetValue("offset",out int off)||
                b is <1 or >12||off is not (0 or 1))return false;
            expected=off==0?"نعم":"لا";return true;
        }
        if(family==Fraction)
        {
            if(p.Count!=3||!p.TryGetValue("n",out int n)||!p.TryGetValue("d",out int d)||
               !p.TryGetValue("e",out int e)||n is <1 or >4||d is <2 or >10||
               n>=d||e is <2 or >4)return false;
            int numerator=1,denominator=1;
            for(int i=0;i<e;i++){numerator=checked(numerator*n);denominator=checked(denominator*d);}
            int g=Gcd(numerator,denominator);
            expected=$"{numerator/g}/{denominator/g}";return true;
        }
        if(family==Decimal)
        {
            if(p.Count!=2||!p.TryGetValue("b100",out int b100)||
               !p.TryGetValue("e",out int e)||b100 is <5 or >250||
               e is <2 or >4)return false;
            expected=Format(Pow(b100/100m,e));return true;
        }
        if(family==Addition)
        {
            if(p.Count!=3||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("e",out int e)||!p.TryGetValue("add",out int a)||
               b is <2 or >10||e is <2 or >4||a is <1 or >25)return false;
            expected=Format(Pow(b,e)+a);return true;
        }
        if(p.Count!=2||!p.TryGetValue("b",out int basis)||!p.TryGetValue("e",out int exponent)||
           basis is <1 or >32||exponent is <0 or >7)return false;
        if(family==Repeated&&exponent<2)return false;
        expected=family==Repeated?$"{basis}^{exponent}":Format(Pow(basis,exponent));
        return true;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported power family");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string prompt,explanation="الأساس هو العامل المتكرر، والأس عدد مرات تكراره في حاصل الضرب. لا تستخدم التقريب.";
        if(family==Ten)
        {
            int exp=random.Next(0,9);p=new(){{"e",exp}};
            prompt=$"احسب 10^{exp} بدقة واكتب عددًا صحيحًا.";
        }
        else if(family==Square)
        {
            int b=random.Next(1,13),off=random.Next(2);p=new(){{"b",b},{"offset",off}};
            prompt=$"هل العدد {b*b+off} مربع كامل؟ اكتب نعم أو لا.";
        }
        else if(family==Fraction)
        {
            int d=random.Next(2,11),n=random.Next(1,Math.Min(d,5)),e=random.Next(2,5);
            p=new(){{"n",n},{"d",d},{"e",e}};
            prompt=$"احسب ({n}/{d})^{e} واكتب الناتج كسرًا في أبسط صورة.";
        }
        else if(family==Decimal)
        {
            int b100=random.Next(5,251),e=random.Next(2,5);
            p=new(){{"b100",b100},{"e",e}};
            prompt=$"احسب {Format(b100/100m)}^{e} بدقة دون تقريب.";
        }
        else if(family==Addition)
        {
            int b=random.Next(2,11),e=random.Next(2,5),add=random.Next(1,26);
            p=new(){{"b",b},{"e",e},{"add",add}};
            prompt=$"أوجد قيمة التعبير {b}^{e} + {add}. احسب القوة أولًا.";
        }
        else
        {
            int b=random.Next(1,13),e=random.Next(2,7);
            p=new(){{"b",b},{"e",e}};
            prompt=family==Repeated?
                $"اكتب حاصل الضرب {string.Join(" × ",Enumerable.Repeat(b.ToString(CultureInfo.InvariantCulture),e))} بصورة أسية b^e.":
                $"احسب القوة {b}^{e}؛ اكتب الناتج عددًا صحيحًا.";
        }
        return new Item(family,prompt,explanation,AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p) =>
        TryExpected(family,p,out var expected)?expected:
            throw new InvalidOperationException("Invalid exact powers parameters.");

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>90||
           !TryExpected(family,p,out string expected))return false;
        string normalized=Normalize(answer.Trim());
        if(family is Fraction or Repeated or Square)
            return string.Equals(normalized,expected,StringComparison.Ordinal);
        return decimal.TryParse(normalized,NumberStyles.Number,CultureInfo.InvariantCulture,
            out var actual) &&
            decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,
            out var wanted) && actual==wanted;
    }
}

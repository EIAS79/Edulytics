using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extension to existing exact Practice architecture for UAE G5 Unit 9; G4 unaffected.
internal static class UaeGrade5Unit9UnitFractionDivisionQuestions
{
    internal const string WholeByUnit="uae.g5.u09.l09.whole_divide_unit_fraction";
    internal const string UnitByWhole="uae.g5.u09.l09.unit_fraction_divide_whole";
    internal const string G6WholeByFraction="uae.g6.u04.l07.whole_divide_nonunit_fraction";
    internal const string G6Reciprocal="uae.g6.u04.l08.reciprocal_number";
    internal const string G6FractionByFraction="uae.g6.u04.l09.fraction_divide_fraction";
    internal const string G6FractionByWhole="uae.g6.u04.l09.fraction_divide_whole";
    internal const string G6MixedDivision="uae.g6.u04.l11.mixed_fraction_division_exact";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is WholeByUnit or UnitByWhole or G6WholeByFraction or G6Reciprocal or G6FractionByFraction or G6FractionByWhole or G6MixedDivision;
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported unit fraction division.");
        if(family==G6MixedDivision)return BuildG6MixedDivision(r);
        if(family is G6FractionByFraction or G6FractionByWhole)return BuildG6FractionDivision(family,r);
        if(family==G6Reciprocal)return BuildG6Reciprocal(r);
        if(family==G6WholeByFraction)return BuildG6(r);
        int n=r.Next(2,11),d=r.Next(2,11);
        var p=new Dictionary<string,int>{{"n",n},{"d",d}};
        string prompt=family==WholeByUnit
            ? $"كم كسر وحدة من فئة ١/{d} يوجد في {n} وحدات كاملة؟ أوجد {n} ÷ ١/{d}."
            : $"إذا قُسم كسر الوحدة ١/{d} إلى {n} مجموعات متساوية، فما نصيب المجموعة الواحدة؟ اكتب كسرًا مبسطًا بسط/مقام.";
        return new Item(family,prompt,"أعد الأجزاء أو قسم كسر الوحدة برقائق كسور متساوية، ثم تحقق بالضرب.",
            family==WholeByUnit?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }




    // Grade6 scoped mixed-number division reuses G5 exact fraction-division pipeline.
    // Supports mixed÷proper, mixed÷mixed, whole÷mixed, mixed÷whole with bounded numerators.
    private static Item BuildG6MixedDivision(Random r)
    {
        int scenario=r.Next(4);
        int w1=scenario==2?r.Next(2,10):r.Next(1,9);
        int d1=scenario==2?1:r.Next(2,10);
        int n1=scenario==2?0:r.Next(1,d1);
        int w2=scenario==0?0:scenario==3?r.Next(2,9):r.Next(1,9);
        int d2=scenario==3?1:r.Next(2,10);
        int n2=scenario==3?0:r.Next(1,d2);
        // scenario 0 mixed ÷ proper, 1 mixed ÷ mixed,
        // scenario 2 whole ÷ mixed, 3 mixed ÷ whole
        var p=new Dictionary<string,int>{
            ["w1"]=w1,["n1"]=n1,["d1"]=d1,
            ["w2"]=w2,["n2"]=n2,["d2"]=d2
        };
        string left=d1==1?$"{w1}":$"{w1} و{n1}/{d1}";
        string right=d2==1?$"{w2}":w2==0?$"{n2}/{d2}":$"{w2} و{n2}/{d2}";
        return new Item(G6MixedDivision,
            $"أوجد {left} ÷ {right}. اكتب الناتج ككسر مبسط (بسط/مقام) أو عدد صحيح.",
            "حوّل الأعداد الكسرية إلى كسور غير فعلية، واضرب في المعكوس الضربي للمقسوم عليه؛ تحقق بالضرب.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static bool TryMixedDivision(IReadOnlyDictionary<string,int>? p,out int numerator,out int denominator)
    {
        numerator=denominator=0;
        if(p is null||p.Count!=6||
           !p.TryGetValue("w1",out int w1)||!p.TryGetValue("n1",out int n1)||
           !p.TryGetValue("d1",out int d1)||!p.TryGetValue("w2",out int w2)||
           !p.TryGetValue("n2",out int n2)||!p.TryGetValue("d2",out int d2)||
           w1 is <0 or >9||w2 is <0 or >9||
           d1 is <1 or >9||d2 is <1 or >9||
           n1<0||n1>=d1||n2<0||n2>=d2||
           (d1==1&&n1!=0)||(d2==1&&n2!=0)||
           (w1==0&&n1==0)||(w2==0&&n2==0))return false;
        int dividend=checked(w1*d1+n1),divisor=checked(w2*d2+n2);
        int top=checked(dividend*d2),bottom=checked(d1*divisor);
        int common=Gcd(top,bottom);numerator=top/common;denominator=bottom/common;
        return true;
    }
    private static string SolveMixedDivision(IReadOnlyDictionary<string,int> p)
    {
        if(!TryMixedDivision(p,out int n,out int d))
            throw new InvalidOperationException("Invalid Grade6 mixed division parameters");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    private static bool VerifyMixedDivision(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryMixedDivision(p,out int numerator,out int denominator)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>64)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c))
            .Select(c=>c switch{
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                '⁄'=>'/',_=>c}).ToArray());
        var parts=normalized.Split('/');
        if(denominator==1&&parts.Length==1)
            return int.TryParse(parts[0],NumberStyles.None,
                CultureInfo.InvariantCulture,out int value)&&value==numerator;
        return parts.Length==2&&int.TryParse(parts[0],NumberStyles.None,
            CultureInfo.InvariantCulture,out int top)&&int.TryParse(parts[1],
            NumberStyles.None,CultureInfo.InvariantCulture,out int den)&&
            top>0&&den>0&&Gcd(top,den)==1&&
            top==numerator&&den==denominator;
    }

    // Grade6 exploratory fraction models. The number is scored, freehand bars need teacher review.
    // This scoped extension never alters the existing G5 unit fraction scenarios.
    private static Item BuildG6FractionDivision(string family,Random r)
    {
        int d=r.Next(3,13),n=r.Next(1,d);
        int a=family==G6FractionByWhole?r.Next(2,8):r.Next(1,10);
        int b=family==G6FractionByWhole?1:r.Next(Math.Max(2,a+1),13);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>{
            ["n"]=n,["d"]=d,["a"]=a,["b"]=b
        };
        return new Item(family,
            $"أوجد {n}/{d} ÷ {(b==1?$"{a}":$"{a}/{b}")}. اكتب عددًا صحيحًا أو كسرًا في أبسط صورة (بسط/مقام).",
            "مثّل المقسوم بأشرطة متساوية ثم احسب عدد المجموعات؛ تحقق بضرب ناتج القسمة في المقسوم عليه.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static bool TryG6FractionDivision(string family,IReadOnlyDictionary<string,int>? p,out int numerator,out int denominator)
    {
        numerator=denominator=0;
        if(family is not (G6FractionByFraction or G6FractionByWhole)||p is null||
           p.Count!=4||!p.TryGetValue("n",out int n)||
           !p.TryGetValue("d",out int d)||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||d is <3 or >12||
           n<1||n>=d||a is <1 or >9||
           (family==G6FractionByWhole?(b!=1||a<2):(b is <2 or >12||a>=b)))return false;
        int top=checked(n*b),bottom=checked(d*a),common=Gcd(top,bottom);
        numerator=top/common;denominator=bottom/common;return true;
    }
    private static string SolveG6FractionDivision(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryG6FractionDivision(family,p,out int n,out int d))
            throw new InvalidOperationException("Invalid Grade6 fraction division operands.");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    private static bool VerifyG6FractionDivision(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryG6FractionDivision(family,p,out int n,out int d)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c))
            .Select(c=>c switch{
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
                '⁄'=>'/',_=>c}).ToArray());
        string[] v=normalized.Split('/');
        if(d==1&&v.Length==1)return int.TryParse(v[0],NumberStyles.None,
            CultureInfo.InvariantCulture,out int integer)&&integer==n;
        return v.Length==2&&int.TryParse(v[0],NumberStyles.None,
            CultureInfo.InvariantCulture,out int top)&&int.TryParse(v[1],
            NumberStyles.None,CultureInfo.InvariantCulture,out int bottom)&&
            top>0&&bottom>0&&Gcd(top,bottom)==1&&top==n&&bottom==d;
    }

    // Grade6 reciprocal is mathematically distinct from division; own bounded exact family.
    private static Item BuildG6Reciprocal(Random r)
    {
        int a=r.Next(1,13),b=r.Next(1,13);
        var p=new Dictionary<string,int>{{"a",a},{"b",b}};
        string value=b==1?$"{a}":$"{a}/{b}";
        return new Item(G6Reciprocal,
            $"أوجد المعكوس الضربي للعدد {value}. اكتب عددًا صحيحًا أو كسرًا في أبسط صورة.",
            "المعكوس الضربي لعدد غير صفري يبدّل البسط والمقام، ويكون حاصل ضرب العدد في معكوسه واحدًا.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static bool TryReciprocal(IReadOnlyDictionary<string,int>? p,out int num,out int den)
    {
        num=den=0;
        if(p is null||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <1 or >12||b is <1 or >12)return false;
        int g=Gcd(a,b);num=b/g;den=a/g;return true;
    }
    private static string SolveReciprocal(IReadOnlyDictionary<string,int> p)
    {
        if(!TryReciprocal(p,out int n,out int d))
            throw new InvalidOperationException("Bad reciprocal");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    private static bool VerifyReciprocal(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryReciprocal(p,out int n,out int d)||string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string text=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c))
            .Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),'⁄'=>'/',_=>c}).ToArray());
        var parts=text.Split('/');
        if(d==1&&parts.Length==1)return int.TryParse(parts[0],NumberStyles.None,
            CultureInfo.InvariantCulture,out int whole)&&whole==n;
        return parts.Length==2&&int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)&&
            int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int bottom)&&
            top>0&&bottom>0&&Gcd(top,bottom)==1&&top==n&&bottom==d;
    }

    // Grade6 scoped whole ÷ non-unit proper fraction; G5 behavior preserved.
    private static Item BuildG6(Random r)
    {
        int n=r.Next(2,11),d=r.Next(3,12),a=r.Next(2,d);
        return new Item(G6WholeByFraction,
            $"أوجد {n} ÷ {a}/{d}. اكتب عدد المجموعات كعدد صحيح أو كسر مبسط (بسط/مقام).",
            "قسّم الوحدات إلى أجزاء متساوية، واجمع الأجزاء بحسب بسط المجموعة؛ تحقق بالضرب.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"n",n},{"d",d},{"a",a}});
    }
    private static int Gcd(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return a;}
    private static bool TryG6(IReadOnlyDictionary<string,int>? p,out int numerator,out int denominator)
    {
        numerator=denominator=0;
        if(p is null||p.Count!=3||!p.TryGetValue("n",out int n)||
           !p.TryGetValue("d",out int d)||!p.TryGetValue("a",out int a)||
           n is <2 or >10||d is <3 or >11||a<2||a>=d)return false;
        int top=checked(n*d),g=Gcd(top,a);numerator=top/g;denominator=a/g;return true;
    }
    private static string SolveG6(IReadOnlyDictionary<string,int> p)
    {
        if(!TryG6(p,out int n,out int d))throw new InvalidOperationException("Invalid G6 fraction division");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    private static bool VerifyG6(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryG6(p,out int n,out int d)||string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'⁄'=>'/',_=>ch}).ToArray());
        string[] parts=normalized.Split('/');
        if(d==1&&parts.Length==1)return int.TryParse(parts[0],
            NumberStyles.None,CultureInfo.InvariantCulture,out int x)&&x==n;
        return parts.Length==2&&int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int a)&&
            int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int b)&&
            a>0&&b>0&&Gcd(a,b)==1&&a==n&&b==d;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int n,out int d)
    {
        n=d=0;
        return (family==WholeByUnit||family==UnitByWhole)&&p.Count==2&&p.TryGetValue("n",out n)&&n is >=2 and <=10&&
            p.TryGetValue("d",out d)&&d is >=2 and <=10;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==G6MixedDivision)return SolveMixedDivision(p);
        if(family is G6FractionByFraction or G6FractionByWhole)return SolveG6FractionDivision(family,p);
        if(family==G6Reciprocal)return SolveReciprocal(p);
        if(family==G6WholeByFraction)return SolveG6(p);
        if(!Valid(family,p,out int n,out int d))throw new InvalidOperationException("Invalid G5 fraction division.");
        return family==WholeByUnit?(n*d).ToString(CultureInfo.InvariantCulture):$"1/{n*d}";
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G6MixedDivision)return VerifyMixedDivision(p,answer);
        if(family is G6FractionByFraction or G6FractionByWhole)return VerifyG6FractionDivision(family,p,answer);
        if(family==G6Reciprocal)return VerifyReciprocal(p,answer);
        if(family==G6WholeByFraction)return VerifyG6(p,answer);
        if(!Valid(family,p,out int n,out int d)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c)&&c is not ',' and not '٬')
            .Select(c=>c switch {>= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),'⁄'=>'/',_=>c}).ToArray());
        if(family==WholeByUnit)
            return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int v)&&v==n*d;
        string[] pieces=normalized.Split('/');
        return pieces.Length==2&&int.TryParse(pieces[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)&&
            int.TryParse(pieces[1],NumberStyles.None,CultureInfo.InvariantCulture,out int bottom)&&top==1&&bottom==n*d;
    }
}
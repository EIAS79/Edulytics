using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9FractionMultiplicationQuestions
{
    internal const string ModelRepeatedSum="uae.g4.u09.l08.model_repeated_sum";
    internal const string UnitFractionCount="uae.g4.u09.l08.model_unit_fraction_count";
    internal const string ProductNumerator="uae.g4.u09.l09.product_numerator";
    internal const string ProductWhole="uae.g4.u09.l09.product_whole";
    internal const string ProductRemainder="uae.g4.u09.l09.product_remainder";
    internal const string G5TwoMixed="uae.g5.u09.l07.multiply_two_mixed";
    internal const string G5MixedByFraction="uae.g5.u09.l07.multiply_mixed_by_fraction";
    internal const string G5Scaling="uae.g5.u09.l08.classify_scaling";
    internal const string G6FractionPair="uae.g6.u04.l03.multiply_fraction_pair_simplified";
    private static bool G5MixedProduct(string? f)=>f is G5TwoMixed or G5MixedByFraction;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is ModelRepeatedSum or UnitFractionCount or ProductNumerator or ProductWhole or ProductRemainder || G5MixedProduct(f) || f==G5Scaling || f==G6FractionPair;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction multiplication");
        if(family==G6FractionPair)return BuildG6FractionPair(random);
        if(G5MixedProduct(family))return BuildG5MixedProduct(family,random);
        if(family==G5Scaling)return BuildG5Scaling(random);
        int d=random.Next(2,13),n=random.Next(1,d),count=random.Next(2,8);
        var p=new Dictionary<string,int>{{"denominator",d},{"numerator",n},{"repeatCount",count}};
        string prompt=family switch
        {
            ModelRepeatedSum=>$"مثل {count} مجموعات متطابقة، في كل واحدة {n}/{d} من الوحدة. ما بسط المجموع قبل التبسيط؟",
            UnitFractionCount=>$"كم كسرًا واحديًا من فئة ١/{d} في {count} × {n}/{d}؟",
            ProductNumerator=>$"أوجد بسط ناتج الضرب {count} × {n}/{d} قبل التبسيط.",
            ProductWhole=>$"احسب {count} × {n}/{d}. ما عدد الوحدات الكاملة في الناتج؟",
            _=>$"احسب {count} × {n}/{d}. ما بسط الجزء الكسري بعد فصل الوحدات الكاملة؟"
        };
        return new Item(family,prompt,"الضرب في عدد كلي يعادل جمع الكسر نفسه عدة مرات، ثم أعد تجميع الوحدات الكاملة.",
            AssessmentItemType.Numeric,p);
    }


    // Scaling a positive whole number: compare the second factor to one.
    // This extends the existing fraction multiplication engine without changing G4 families.
    private static Item BuildG5Scaling(Random random)
    {
        int whole=random.Next(2,13),den=random.Next(2,11);
        int kind=random.Next(0,3);
        int num=kind==0?random.Next(1,den):kind==1?den:random.Next(den+1,den*2+1);
        return new Item(G5Scaling,
            $"من دون حساب حاصل الضرب كاملًا: قارن {whole} × {num}/{den} بالعدد {whole}. اكتب 1 إذا كان الناتج أكبر، أو 2 إذا كان أصغر، أو 3 إذا كان يساويه.",
            "قارن عامل المقياس بالواحد: أكبر من ١ يكبّر العدد الموجب، وأصغر من ١ يصغّره، ومساوٍ للواحد لا يغيّره.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"whole",whole},{"num",num},{"den",den}});
    }
    private static bool ValidG5Scaling(IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(p.Count!=3||!p.TryGetValue("whole",out int whole)||
           !p.TryGetValue("num",out int num)||!p.TryGetValue("den",out int den)||
           whole is <2 or >12||den is <2 or >10||num<1||num>2*den)return false;
        result=num>den?1:num<den?2:3;
        return true;
    }
    private static string SolveG5Scaling(IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Scaling(p,out int result))throw new InvalidOperationException("Invalid G5 scaling operands");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5Scaling(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Scaling(p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c}).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }


    // Grade 6 Common Arabic-specific fraction-by-fraction extension.
    // This dispatch leaves all existing G4 and G5 family validators unchanged.
    private static Item BuildG6FractionPair(Random random)
    {
        int d1=random.Next(2,13),d2=random.Next(2,13);
        int n1=random.Next(1,d1),n2=random.Next(1,d2);
        return new Item(G6FractionPair,
            $"أوجد حاصل ضرب {n1}/{d1} × {n2}/{d2} في أبسط صورة. اكتب الإجابة بسط/مقام.",
            "نضرب البسطين والمقامين، ونبسط بقسمة البسط والمقام على العامل المشترك الأكبر. يجوز الاختصار قبل الضرب.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}});
    }
    private static bool TryG6FractionPair(IReadOnlyDictionary<string,int>? p,out int top,out int den)
    {
        top=den=0;
        if(p is null||p.Count!=4||
           !p.TryGetValue("n1",out int n1)||!p.TryGetValue("d1",out int d1)||
           !p.TryGetValue("n2",out int n2)||!p.TryGetValue("d2",out int d2)||
           d1 is <2 or >12||d2 is <2 or >12||
           n1<1||n1>=d1||n2<1||n2>=d2)return false;
        int product=n1*n2,denominator=d1*d2,common=GcdG5(product,denominator);
        top=product/common;den=denominator/common;
        return true;
    }
    private static string SolveG6FractionPair(IReadOnlyDictionary<string,int> p)
    {
        if(!TryG6FractionPair(p,out int top,out int den))
            throw new InvalidOperationException("Invalid G6 fraction pair.");
        return $"{top}/{den}";
    }
    private static bool VerifyG6FractionPair(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryG6FractionPair(p,out int expectedTop,out int expectedDen)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '⁄'=>'/',
                _=>ch
            }).ToArray());
        string[] parts=normalized.Split('/');
        return parts.Length==2&&
            int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)&&
            int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)&&
            top>0&&den>0&&GcdG5(top,den)==1&&
            top==expectedTop&&den==expectedDen;
    }

    private static int GcdG5(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return a;}
    private static Item BuildG5MixedProduct(string family,Random random)
    {
        int w1=random.Next(1,6),w2=family==G5TwoMixed?random.Next(1,6):0;
        int d1=random.Next(2,10),d2=random.Next(2,10);
        int n1=random.Next(1,d1),n2=random.Next(1,d2);
        string prompt=family==G5TwoMixed
            ?$"اضرب العددين الكسريين {w1} و{n1}/{d1} × {w2} و{n2}/{d2}. اكتب الناتج ككسر غير فعلي مبسط (بسط/مقام)."
            :$"اضرب {w1} و{n1}/{d1} في {n2}/{d2}، واكتب الناتج ككسر مبسط (بسط/مقام).";
        return new Item(family,prompt,
          "أحول العدد الكسري إلى كسر غير فعلي، ثم أضرب البسطين والمقامين وأبسط.",
          AssessmentItemType.ShortAnswer,
          new Dictionary<string,int>{{"w1",w1},{"n1",n1},{"d1",d1},
             {"w2",w2},{"n2",n2},{"d2",d2}});
    }
    private static bool G5Valid(string family,IReadOnlyDictionary<string,int> p,
        out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)
    {
        w1=n1=d1=w2=n2=d2=0;
        return G5MixedProduct(family)&&p.Count==6&&
           p.TryGetValue("w1",out w1)&&p.TryGetValue("n1",out n1)&&
           p.TryGetValue("d1",out d1)&&p.TryGetValue("w2",out w2)&&
           p.TryGetValue("n2",out n2)&&p.TryGetValue("d2",out d2)&&
           w1 is >=1 and <=5 &&d1 is >=2 and <=9&&d2 is >=2 and <=9&&
           n1>=1&&n1<d1&&n2>=1&&n2<d2&&
           (family==G5TwoMixed?w2 is >=1 and <=5:w2==0);
    }
    private static string SolveG5MixedProduct(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!G5Valid(family,p,out int w1,out int n1,out int d1,out int w2,out int n2,out int d2))
            throw new InvalidOperationException("Invalid G5 mixed multiplication operands.");
        int top=(w1*d1+n1)*(w2*d2+n2),den=d1*d2,g=GcdG5(top,den);
        return $"{top/g}/{den/g}";
    }
    private static bool VerifyG5MixedProduct(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!G5Valid(family,p,out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
          .Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
             >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'⁄'=>'/',_=>ch}).ToArray());
        var parts=normalized.Split('/');
        if(parts.Length!=2||!int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)||
           top<1||den<1||GcdG5(top,den)!=1)return false;
        return (long)top*d1*d2==(long)den*(w1*d1+n1)*(w2*d2+n2);
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("numerator",out int n)||!p.TryGetValue("repeatCount",out int times)||
           d is <2 or >12||n<1||n>=d||times is <2 or >7)return false;
        int product=n*times;
        answer=family switch
        {
            ModelRepeatedSum or UnitFractionCount or ProductNumerator=>product,
            ProductWhole=>product/d,
            _=>product%d
        };
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==G6FractionPair)return SolveG6FractionPair(p);
        if(G5MixedProduct(family))return SolveG5MixedProduct(family,p);
        if(family==G5Scaling)return SolveG5Scaling(p);
        if(!Valid(family,p,out int result))throw new InvalidOperationException("Invalid fraction multiplication parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G6FractionPair)return VerifyG6FractionPair(p,answer);
        if(G5MixedProduct(family))return VerifyG5MixedProduct(family,p,answer);
        if(family==G5Scaling)return VerifyG5Scaling(p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

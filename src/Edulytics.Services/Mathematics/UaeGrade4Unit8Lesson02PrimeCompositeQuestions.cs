using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson02PrimeCompositeQuestions
{
    internal const string Classification="uae.g4.u08.l02.prime_composite_classification";
    internal const string FactorCount="uae.g4.u08.l02.positive_factor_count";
    // UAE Grade 5 Common: exact complete prime factorization / tree continuation
    // extends the existing G4 prime/composite engine without changing G4 scope.
    internal const string G5Factorize="uae.g5.u02.l01.prime_factorize";
    internal const string G5FactorCount="uae.g5.u02.l01.prime_factor_count";
    internal const string G5MissingPrime="uae.g5.u02.l01.complete_factor_tree";
    internal const string G5FoldHoles="uae.g5.u02.l02.fold_holes";
    internal const string G5TriangleGrowth="uae.g5.u02.l02.triangle_growth";
    internal const string G5RepeatedGrowth="uae.g5.u02.l02.repeated_growth";
    private static bool IsG5Pattern(string? f)=>f is G5FoldHoles or G5TriangleGrowth or G5RepeatedGrowth;
    private static bool IsG5(string? family)=>
        family is G5Factorize or G5FactorCount or G5MissingPrime;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Classification or FactorCount || IsG5(family) || IsG5Pattern(family);


    // Grade5: exact powers derived from the existing prime-factor pattern model.
    private static Item BuildG5Pattern(string family,Random rng)
    {
        int initial=family==G5TriangleGrowth?1:rng.Next(1,5);
        int factor=family==G5TriangleGrowth?4:family==G5FoldHoles?2:rng.Next(2,5);
        int rounds=rng.Next(2,6);
        string prompt=family switch
        {
            G5FoldHoles=>$"في البداية {initial} ثقوب، ويزداد عددها إلى الضعف مع كل طية. كم ثقبًا بعد {rounds} طيات؟",
            G5TriangleGrowth=>$"ينقسم المثلث إلى أربعة مثلثات في كل مرحلة. كم مثلثًا بعد {rounds} مراحل إذا بدأنا بمثلث واحد؟",
            _=>$"يبدأ نمط بعدد {initial}، ثم نضربه في {factor} كل مرحلة. ما العدد بعد {rounds} مراحل؟"
        };
        return new Item(family,prompt,"اضرب عامل النمو في العدد السابق بكل مرحلة وتحقق بجدول.",AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"initial",initial},{"factor",factor},{"rounds",rounds}});
    }
    private static bool ValidG5Pattern(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5Pattern(family)||p.Count!=3||
           !p.TryGetValue("initial",out int initial)||
           !p.TryGetValue("factor",out int factor)||
           !p.TryGetValue("rounds",out int rounds)||rounds is <2 or >5)
            return false;
        if(family==G5FoldHoles&&(factor!=2||initial is <1 or >4))return false;
        if(family==G5TriangleGrowth&&(factor!=4||initial!=1))return false;
        if(family==G5RepeatedGrowth&&(factor is <2 or >4||initial is <1 or >4))return false;
        int value=initial;
        for(int i=0;i<rounds;i++)value=checked(value*factor);
        expected=value;return true;
    }

    private static bool IsPrime(int n)
    {
        if(n<2)return false;
        for(int divisor=2;divisor*divisor<=n;divisor++)
            if(n%divisor==0)return false;
        return true;
    }

    private static int[] Factorize(int n)
    {
        var factors=new List<int>();
        for(int divisor=2;divisor<=n/divisor;divisor++)
        {
            while(n%divisor==0)
            {
                factors.Add(divisor);
                n/=divisor;
            }
        }
        if(n>1)factors.Add(n);
        return factors.ToArray();
    }

    private static Item BuildG5(string family,Random random)
    {
        if(family==G5MissingPrime)
        {
            int[] primes=[2,3,5,7];
            int a=primes[random.Next(primes.Length)];
            int b=primes[random.Next(primes.Length)];
            int c=primes[random.Next(primes.Length)];
            int missing=primes[random.Next(primes.Length)];
            int n=a*b*c*missing;
            var parameters=new Dictionary<string,int>
            {
                ["n"]=n,["a"]=a,["b"]=b,["c"]=c
            };
            return new Item(family,
                $"أكمل شجرة العوامل الأولية للعدد {n}: {a} × {b} × □ × {c}. ما العامل الأولي المفقود؟",
                "احسب حاصل ضرب العوامل المعلومة، اقسم العدد عليه، ثم تحقق أن الناتج عدد أولي.",
                AssessmentItemType.Numeric,parameters);
        }

        int value=random.Next(12,2801);
        while(IsPrime(value))value=random.Next(12,2801);
        string prompt=family==G5Factorize
            ? $"حلل العدد {value} إلى عوامل أولية، واكتب العوامل مفصولة بعلامة ×."
            : $"عند تحليل العدد {value} إلى عوامله الأولية، كم عدد العوامل مع حساب التكرار؟";
        return new Item(family,prompt,
            "اقسم على أصغر عدد أولي ممكن حتى يصبح الناتج ١، ثم تحقق بضرب العوامل جميعًا.",
            family==G5Factorize?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"n",value}});
    }

    private static bool ValidG5Numeric(string family,
        IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!IsG5(family)||!p.TryGetValue("n",out int n)||
           n<4||n>2800||IsPrime(n))return false;
        if(family==G5FactorCount)
        {
            if(p.Count!=1)return false;
            answer=Factorize(n).Length;
            return true;
        }
        if(family==G5MissingPrime)
        {
            if(p.Count!=4||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
               !IsPrime(a)||!IsPrime(b)||!IsPrime(c)||
               a is >13 or <2||b is >13 or <2||c is >13 or <2||
               n%(a*b*c)!=0)return false;
            int missing=n/(a*b*c);
            if(!IsPrime(missing)||missing>13)return false;
            answer=missing;
            return true;
        }
        return false;
    }

    private static string FactorizationString(IReadOnlyDictionary<string,int> p)
    {
        if(p.Count!=1||!p.TryGetValue("n",out int n)||
           n<4||n>2800||IsPrime(n))
            throw new InvalidOperationException("Invalid full-factorization question.");
        return string.Join("×",Factorize(n));
    }

    private static bool VerifyFullFactorization(
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(p.Count!=1||!p.TryGetValue("n",out int n)||
           n<4||n>2800||IsPrime(n)||string.IsNullOrWhiteSpace(answer))
            return false;
        string normalized=new(answer.Trim()
            .Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                'x' or 'X' or '×' or '·'=>'*',
                _=>ch
            })
            .Where(ch=>ch!=' '&&ch!='\u00A0'&&ch!='\u202F')
            .ToArray());
        string[] parts=normalized.Split('*');
        if(parts.Length<2||parts.Length>12)return false;
        long product=1;
        foreach(string token in parts)
        {
            if(!int.TryParse(token,NumberStyles.None,
                CultureInfo.InvariantCulture,out int factor)||
               !IsPrime(factor)||factor>2800)
                return false;
            product*=factor;
            if(product>n)return false;
        }
        return product==n;
    }

    private static int Count(int n)
    {
        if(n<1)return 0;
        int count=0;
        for(int i=1;i<=n;i++)
            if(n%i==0)count++;
        return count;
    }
    private static int Category(int n)
    {
        if(n<=1)return 2;
        return Count(n)==2?1:0;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported number theory family.");
        if(IsG5(family)) return BuildG5(family,random);
        if(IsG5Pattern(family)) return BuildG5Pattern(family,random);
        int n=family==Classification?random.Next(0,96):random.Next(2,96);
        if(family==Classification&&random.Next(0,8)==0)n=random.Next(0,2);
        string prompt=family==Classification?
            $"صنف العدد {n}: اكتب ١ للأولي، و٠ لغير الأولي، و٢ إذا لم يكن أيًا منهما.":
            $"كم عاملًا موجبًا مختلفًا للعدد {n}؟ اكتب عدد العوامل.";
        string solution=family==Classification?
            "اختبر العوامل الموجبة؛ للعدد الأولي عاملان، وغير الأولي أكثر من عاملين، و٠ و١ حالتان خاصتان.":
            "اكتب جميع القواسم الموجبة التي تقسم العدد دون باق ثم احسب عددها.";
        return new Item(family,prompt,solution,AssessmentItemType.Numeric,new Dictionary<string,int>{{"n",n}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(IsG5(family)) return ValidG5Numeric(family,p,out answer);
        if(IsG5Pattern(family)) return ValidG5Pattern(family,p,out answer);
        if(!Supports(family)||p.Count!=1||!p.TryGetValue("n",out int n)||n is <0 or >120||
           (family==FactorCount&&n<2))return false;
        answer=family==Classification?Category(n):Count(n);
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==G5Factorize) return FactorizationString(p);
        if(!Valid(family,p,out int a))throw new InvalidOperationException("Invalid prime problem.");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G5Factorize) return VerifyFullFactorization(p,answer);
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

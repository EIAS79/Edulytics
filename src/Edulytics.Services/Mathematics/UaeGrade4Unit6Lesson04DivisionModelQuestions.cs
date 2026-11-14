using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson04DivisionModelQuestions
{
    internal const string Quotient="uae.g4.u06.l04.model_after_removing_items_quotient";
    internal const string G5DecimalGroup="uae.g5.u05.l10.tenths_equal_group_quotient";
    internal const string G5Exact="uae.g5.u05.l11.exact_decimal_by_whole",G5Rounded="uae.g5.u05.l11.round_decimal_quotient_hundredth",G5Digit="uae.g5.u05.l11.quotient_hundredths_digit";
    private static bool IsG5WholeDecimal(string? f)=>f is G5Exact or G5Rounded or G5Digit;
    internal const string G5ModelGroups="uae.g5.u05.l12.model_decimal_equal_groups";
    internal const string G5ModelHundredths="uae.g5.u05.l12.model_decimal_hundredths_count";
    private static bool IsG5DecimalModel(string? f)=>f is G5ModelGroups or G5ModelHundredths;

    internal const string G5DivideExact="uae.g5.u05.l13.exact_decimal_divide";
    internal const string G5DivideContext="uae.g5.u05.l13.contextual_decimal_unit_rate";
    internal const string G5DivideDigit="uae.g5.u05.l13.quotient_tenths_digit";
    private static bool IsG5DecimalQuotient(string? f)=>f is G5DivideExact or G5DivideContext or G5DivideDigit;

    internal const string G5TenthsEach="uae.g5.u05.l10.tenths_per_group";
    internal const string G5WholeEach="uae.g5.u05.l10.whole_units_per_group";
    private static bool IsG5(string? f)=>f is G5DecimalGroup or G5TenthsEach or G5WholeEach;
    internal const string Remainder="uae.g4.u06.l04.model_after_removing_items_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder || IsG5(f) || IsG5WholeDecimal(f) || IsG5DecimalModel(f) || IsG5DecimalQuotient(f);
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown model division family");
        if(IsG5DecimalQuotient(f))return BuildG5DecimalQuotient(f,random);
        if(IsG5DecimalModel(f))return BuildG5DecimalModel(f,random);
        if(IsG5WholeDecimal(f))return BuildG5WholeDecimal(f,random);
        if(IsG5(f))return BuildG5(f,random);
        int group=random.Next(2,9),each=random.Next(2,26),removed=random.Next(1,10);
        int remainder=random.Next(0,group),total=group*each+removed+remainder;
        string prompt=f==Quotient
         ?$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. صمّم نموذجًا واكتب عدد القطع الكاملة في المجموعة الواحدة."
         :$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. كم قطعة تتبقى بعد تكوين المجموعات المتساوية؟";
        return new Item(f,prompt,"أرسم مجموعات: أطرح غير المطلوب من المجموع، ثم أقسّم الباقي إلى مجموعات متساوية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"total",total},{"removed",removed},{"groups",group}});
    }




    // U05 L13 extends the already routed equal-group engine to exact
    // decimal-by-decimal division; only given integer-scaled operands persist.
    private static int G5Gcd(int a,int b)
    {
        while(b!=0){int t=a%b;a=b;b=t;}
        return a;
    }
    private static Item BuildG5DecimalQuotient(string f,Random random)
    {
        int[] divisors=[3,4,5,10,12,15,20,25,30,40,45,50,60,75,80,100,120,125,130,140,150];
        int d=divisors[random.Next(divisors.Length)];
        int step=100/G5Gcd(100,d);
        int quotientMilli=random.Next(50,900001)/step*step;
        quotientMilli=Math.Max(step,quotientMilli);
        int n=checked(quotientMilli*d/100);
        decimal dividend=n/1000m,divisor=d/100m;
        string lhs=dividend.ToString("0.###",CultureInfo.InvariantCulture);
        string rhs=divisor.ToString("0.##",CultureInfo.InvariantCulture);
        string prompt=f switch
        {
            G5DivideContext=>$"لدى جمعية {lhs} كغم من مادة، تعبّئها في عبوات سعة {rhs} كغم. كم عبوة مكافئة يمكن تكوينها؟",
            G5DivideDigit=>$"في ناتج القسمة {lhs} ÷ {rhs}، ما الرقم في منزلة الأعشار من حاصل القسمة الدقيق؟",
            _=>$"أوجد {lhs} ÷ {rhs} دون تقريب. اضرب الطرفين بقوة عشرة مناسبة ثم تحقق بالضرب."
        };
        return new Item(f,prompt,
            "أضرب المقسوم والمقسوم عليه بالقوة العشرية نفسها حتى يصبح المقسوم عليه كليًا، ثم أقسم وأضرب الناتج للتحقق.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividendThousandths",n},{"divisorHundredths",d}});
    }
    private static bool ValidG5DecimalQuotient(string f,IReadOnlyDictionary<string,int> p,out int n,out int d)
    {
        n=d=0;
        return IsG5DecimalQuotient(f)&&p.Count==2&&
          p.TryGetValue("dividendThousandths",out n)&&p.TryGetValue("divisorHundredths",out d)&&
          n is >=1 and <=1350000&&d is >=2 and <=150&&
          n*100%d==0&&n*100/d is >=1 and <=900000;
    }
    private static string SolveG5DecimalQuotient(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5DecimalQuotient(f,p,out int n,out int d))
            throw new InvalidOperationException("Invalid decimal-by-decimal division parameters");
        int qMilli=n*100/d;
        decimal result=f==G5DivideDigit?(qMilli/100)%10:qMilli/1000m;
        return result.ToString("0.###",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5DecimalQuotient(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5DecimalQuotient(f,p,out int n,out int d)||
           string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch
        {
          >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
          >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
          '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0' and not '\u202f').ToArray());
        if(normalized.Length>24||!normalized.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
        var style=f==G5DivideDigit?NumberStyles.None:NumberStyles.AllowDecimalPoint;
        if(!decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal entered))return false;
        // Independent verifier uses multiplication by the divisor rather than
        // Solve's scaled quotient integer reconstruction.
        return f==G5DivideDigit?entered==((n*100/d/100)%10):
          entered*d*10m==n;
    }

    // Grade 5 U05:L12: reuse the same equal-group engine for hundredth-size
    // blocks. Persist only dividend/divisor hundredths, not solutions.
    private static Item BuildG5DecimalModel(string f,Random random)
    {
        int divisorHundredths=random.Next(2,91),groups=random.Next(2,11);
        int dividendHundredths=checked(divisorHundredths*groups);
        string n=(dividendHundredths/100m).ToString("0.##",CultureInfo.InvariantCulture);
        string d=(divisorHundredths/100m).ToString("0.##",CultureInfo.InvariantCulture);
        string prompt=f==G5ModelGroups
            ?$"باستخدام شبكة أجزاء المئة، كوّن مجموعات قيمة كل واحدة {d} من العدد {n}. كم مجموعة متساوية؟"
            :$"لتمثيل قسمة {n} ÷ {d} بمكعبات أجزاء المئة، كم مربعًا من المئة نمثل به العدد {n} قبل التقسيم؟";
        return new Item(f,prompt,
            "أحوّل الوحدات والأعشار إلى أجزاء من مئة، ثم أعد المجموعات وأتحقق بضرب المقسوم عليه في الحاصل.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividendHundredths",dividendHundredths},{"divisorHundredths",divisorHundredths}});
    }
    private static bool ValidG5DecimalModel(string f,IReadOnlyDictionary<string,int> p,
        out int dividendHundredths,out int divisorHundredths)
    {
        dividendHundredths=divisorHundredths=0;
        return IsG5DecimalModel(f)&&p.Count==2&&
           p.TryGetValue("dividendHundredths",out dividendHundredths)&&
           p.TryGetValue("divisorHundredths",out divisorHundredths)&&
           divisorHundredths is >=2 and <=90&&
           dividendHundredths>=2*divisorHundredths&&
           dividendHundredths<=10*divisorHundredths&&
           dividendHundredths%divisorHundredths==0;
    }
    private static string SolveG5DecimalModel(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5DecimalModel(f,p,out int n,out int d))
            throw new InvalidOperationException("Invalid decimal equal-group model");
        return (f==G5ModelGroups?n/d:n).ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5DecimalModel(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5DecimalModel(f,p,out int n,out int d)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(ch=>ch is not ' ' and not '٬' and not ',' and not '\u00a0' and not '\u202f')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int got))return false;
        return f==G5ModelGroups?got*d==n:got==n;
    }

    // Extend existing equal-group engine for long division of hundredths
    // by an integer, retaining exact thousandths and rounding only on request.
    private static Item BuildG5WholeDecimal(string f,Random random)
    {
        int d=random.Next(2,26),q=random.Next(100,10001);
        while(q*d%10!=0)q++;
        int dividendHundredths=q*d/10;
        decimal x=dividendHundredths/100m;
        string amount=x.ToString("0.00",CultureInfo.InvariantCulture);
        string prompt=f switch
        {
            G5Exact=>$"أوجد حاصل {amount} ÷ {d} دون تقريب. ضع الفاصلة في الحاصل وتحقق بالضرب.",
            G5Rounded=>$"احسب {amount} ÷ {d}، ثم قرّب الحاصل إلى أقرب جزء من مئة.",
            _=>$"في ناتج {amount} ÷ {d} الدقيق، ما رقم منزلة أجزاء المئة؟"
        };
        return new Item(f,prompt,"أحاذي الفاصلة وأقسم كالأعداد الكلية، وأضيف صفرًا عند الحاجة وأراجع الضرب.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividendHundredths",dividendHundredths},{"divisor",d}});
    }
    // Grade6 U03:L09 extends the existing Grade5 whole-divisor decimal
    // family to larger divisors and thousandth-scaled source operands.
    // Exact questions reject non-terminating quotients instead of rounding.
    private static bool ValidG6DecimalByWhole(string f,IReadOnlyDictionary<string,int> p,
        out decimal expected)
    {
        expected=0m;
        if (f is not (G5Exact or G5Rounded)||
            !p.TryGetValue("g6DividendThousandths",out int n)||
            !p.TryGetValue("g6WholeDivisor",out int d)||
            n is <1 or >5000000 || d is <2 or >900)
            return false;
        var exact=(decimal)n/(d*1000m);
        if(f==G5Exact && Math.Round(exact,6,MidpointRounding.AwayFromZero)!=exact)
            return false;
        expected=f==G5Rounded?Math.Round(exact,2,MidpointRounding.AwayFromZero):exact;
        return true;
    }

    private static bool ValidG5WholeDecimal(string f,IReadOnlyDictionary<string,int> p,
        out int qMilli)
    {
        qMilli=0;
        if(!IsG5WholeDecimal(f)||p.Count!=2||
           !p.TryGetValue("dividendHundredths",out int n)||
           !p.TryGetValue("divisor",out int d)||n is <20 or >25002||
           d is <2 or >25||n*10%d!=0)return false;
        qMilli=n*10/d;
        return qMilli is >=100 and <=10001;
    }
    private static string SolveG5WholeDecimal(string f,IReadOnlyDictionary<string,int> p)
    {
        if(p.ContainsKey("g6DividendThousandths"))
        {
            if(!ValidG6DecimalByWhole(f,p,out var value))
                throw new InvalidOperationException("Invalid Grade6 decimal-by-whole parameters.");
            return value.ToString("0.######",CultureInfo.InvariantCulture);
        }

        if(!ValidG5WholeDecimal(f,p,out int q))
            throw new InvalidOperationException("Invalid G5 decimal-by-whole parameters");
        decimal exact=q/1000m;
        decimal expected=f switch{
          G5Rounded=>Math.Round(exact,2,MidpointRounding.AwayFromZero),
          G5Digit=>(q/10)%10,
          _=>exact
        };
        return expected.ToString("0.###",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5WholeDecimal(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(p.ContainsKey("g6DividendThousandths"))
        {
            if(!ValidG6DecimalByWhole(f,p,out var expected)||string.IsNullOrWhiteSpace(answer))
                return false;
            string normalized=new(answer.Trim().Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '٫'=>'.',_=>ch
            }).ToArray());
            if(normalized.Length>36||
               !normalized.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
            return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out var actual)&&actual==expected;
        }

        if(!ValidG5WholeDecimal(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(s.Length>20||!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
        if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal actual))return false;
        int n=p["dividendHundredths"],d=p["divisor"];
        decimal quotient=(decimal)n/(d*100m);
        return f switch{
            G5Rounded=>actual==Math.Round(quotient,2,MidpointRounding.AwayFromZero),
            G5Digit=>actual==((n*10/d/10)%10),
            _=>actual*d*100m==n
        };
    }

    // Extend existing equal-groups division model to tenths; students
    // distribute whole units and tenths to equal groups and check by ×.
    private static Item BuildG5(string f,Random rng)
    {
        int groups=rng.Next(2,9),perGroupTenths=rng.Next(2,30);
        int dividendTenths=checked(groups*perGroupTenths);
        string amount=(dividendTenths/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string prompt=f switch
        {
            G5DecimalGroup=>$"قسّم {amount} إلى {groups} مجموعات متساوية باستخدام وحدات وأعشار. ما قيمة كل مجموعة؟",
            G5TenthsEach=>$"لديك {dividendTenths} قطعة أعشار موزعة بالتساوي على {groups} مجموعات. كم قطعة من عشرة لكل مجموعة؟",
            _=>$"عند تقسيم {amount} إلى {groups} مجموعات متساوية، كم وحدة كاملة في كل مجموعة؟"
        };
        return new Item(f,prompt,
            "أحوّل الوحدات إلى أعشار عند الحاجة، أقسم الأعشار ثم أراجع بالضرب.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividendTenths",dividendTenths},{"groups",groups}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out decimal answer)
    {
        answer=0m;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("dividendTenths",out int n)||
           !p.TryGetValue("groups",out int g)||
           // The generator permits eight groups of 29 tenths (232 tenths).
           n is <4 or >232||g is <2 or >8||
           n%g!=0||n/g is <2 or >29)return false;
        int tenthsEach=n/g;
        answer=f switch{G5DecimalGroup=>tenthsEach/10m,
            G5TenthsEach=>tenthsEach,_=>tenthsEach/10};
        return true;
    }
    private static string SolveG5(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5(f,p,out decimal answer))
            throw new InvalidOperationException("Invalid G5 tenths division model");
        return answer.ToString("0.##",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(ch=>ch is >= '0' and <= '9' or '.'))
            return false;
        if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal provided))return false;
        int total=p["dividendTenths"],groups=p["groups"];
        return f switch{
            G5DecimalGroup=>provided*10m*groups==total,
            G5TenthsEach=>provided*groups==total,
            _=>provided==total/groups/10
        };
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("total",out int n)||
           !p.TryGetValue("removed",out int x)||!p.TryGetValue("groups",out int d)||
           d is <2 or >8||x is <1 or >9||n<=x+d||n>500)return false;
        answer=f==Quotient?(n-x)/d:(n-x)%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5DecimalQuotient(f))return SolveG5DecimalQuotient(f,p);
        if(IsG5DecimalModel(f))return SolveG5DecimalModel(f,p);
        if(IsG5WholeDecimal(f))return SolveG5WholeDecimal(f,p);
        if(IsG5(f))return SolveG5(f,p);
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid division model");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5DecimalQuotient(f))return VerifyG5DecimalQuotient(f,p,answer);
        if(IsG5DecimalModel(f))return VerifyG5DecimalModel(f,p,answer);
        if(IsG5WholeDecimal(f))return VerifyG5WholeDecimal(f,p,answer);
        if(IsG5(f))return VerifyG5(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}

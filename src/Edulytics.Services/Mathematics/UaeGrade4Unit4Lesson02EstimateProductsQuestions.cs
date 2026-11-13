using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Estimate products after rounding the multi-digit factor to its greatest place value.
internal static class UaeGrade4Unit4Lesson02EstimateProductsQuestions
{
    internal const string Estimate="uae.g4.u04.l02.estimate_rounded_product";
    internal const string AboveBelow="uae.g4.u04.l02.estimate_above_below";
    // Reuse G4 rounding estimator for selected Grade 5 source examples.
    internal const string G5One="uae.g5.u02.l08.round_one_factor";
    internal const string G5Both="uae.g5.u02.l08.round_both_factors";
    internal const string G5Compatible="uae.g5.u02.l08.compatible_factors";
    internal const string G5HighLow="uae.g5.u02.l08.estimate_high_or_low";
    internal const string G5U5RoundDecimal="uae.g5.u05.l01.round_decimal_to_whole";
    internal const string G5U5RoundBoth="uae.g5.u05.l01.round_both_factors";
    internal const string G5U5Direction="uae.g5.u05.l01.estimate_above_exact";
    private static bool IsG5U5(string? f)=>f is G5U5RoundDecimal or G5U5RoundBoth or G5U5Direction;
    private static bool IsG5(string? f)=>f is G5One or G5Both or G5Compatible or G5HighLow;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or AboveBelow || IsG5(f) || IsG5U5(f);
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown estimation family");
        if(IsG5U5(f))return BuildG5U5(f,random);
        if(IsG5(f))return BuildG5(f,random);
        int factor=random.Next(2,10),multidigit=random.Next(105,9850);
        int power=multidigit>=1000?1000:100;
        int rounded=((multidigit+power/2)/power)*power;
        string prompt=f==Estimate
          ?$"قرّب العدد {multidigit} إلى أكبر منزلة ثم قدّر ناتج {factor} × {multidigit}. اكتب الناتج التقديري."
          :$"قرّب العدد {multidigit} إلى أكبر منزلة ثم قارن تقدير {factor} × {multidigit} بالناتج الدقيق. اكتب ١ إذا التقدير أكبر أو مساوي، و٠ إذا أصغر.";
        return new Item(f,prompt,"أقرّب العامل الأكبر إلى أكبر قيمة مكانية ثم أضرب وأقارن بالناتج الفعلي.",
          AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"number",multidigit}});
    }


    // Source-grounded Grade 5 decimal product estimate: represent decimal input
    // with integer hundredths and reuse the existing G4 rounding/estimation engine.
    private static Item BuildG5U5(string f,Random r)
    {
        int hundredths=r.Next(85,9996),factor=r.Next(2,16);
        string dec=(hundredths/100m).ToString("0.00",CultureInfo.InvariantCulture);
        int rounded=(hundredths+50)/100;
        int roundedFactor=factor>=10?(factor+5)/10*10:factor;
        string prompt=f switch
        {
            G5U5RoundDecimal=>$"قرّب العدد العشري {dec} لأقرب عدد كلي، ثم قدّر {dec} × {factor}. اكتب حاصل التقدير.",
            G5U5RoundBoth=>$"لتقدير {dec} × {factor}، قرّب {dec} لأقرب عدد كلي وقرّب {factor} لأقرب عشرة إذا كان مكونًا من رقمين. ما حاصل التقدير؟",
            _=>$"التقدير {rounded} × {roundedFactor} لمسألة {dec} × {factor}. اكتب ١ إذا كان التقدير أكبر من الناتج الفعلي أو يساويه، و٠ إذا كان أصغر."
        };
        return new Item(f,prompt,"أستخدم تقريب الأعداد ثم أضرب لأجد تقديرًا معقولًا وأقارن بالعدد الأصلي.",
           AssessmentItemType.Numeric,new Dictionary<string,int>{{"hundredths",hundredths},{"factor",factor}});
    }
    private static bool ValidG5U5(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5U5(f)||p.Count!=2||!p.TryGetValue("hundredths",out int a)||
            !p.TryGetValue("factor",out int b)||a is <85 or >9995||b is <2 or >15)return false;
        int rounded=(a+50)/100;
        int roundedFactor=b>=10?(b+5)/10*10:b;
        int estimate=rounded*(f==G5U5RoundDecimal?b:roundedFactor);
        expected=f==G5U5Direction?(100L*estimate>=(long)a*b?1:0):estimate;
        return true;
    }
    private static bool VerifyG5U5(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5U5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        var raw=new string(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0'&&ch!='\u202f')
           .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                       ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out int provided))return false;
        int hundredths=p["hundredths"],factor=p["factor"];
        long nearestInteger=(hundredths+50L)/100L;
        long nearestFactor=factor>=10?(factor+5L)/10L*10L:factor;
        long estimate=nearestInteger*(f==G5U5RoundDecimal?factor:nearestFactor);
        return f==G5U5Direction
            ? provided is 0 or 1 && (provided==1)==(estimate*100L>=hundredths*(long)factor)
            : provided==estimate;
    }

    // Multiplication estimation is the same pedagogical mechanism as G4,
    // with the 2-3-digit × 1-2-digit range and compatible numbers added.
    private static int RoundTo(int n,int place)=>((n+place/2)/place)*place;
    private static Item BuildG5(string f,Random random)
    {
        int first=random.Next(20,999);
        int second=random.Next(3)==0?random.Next(2,10):random.Next(11,99);
        string question=f switch
        {
            G5One=>$"قرّب العامل {first} إلى أقرب عشرة ثم قدّر حاصل ضربه في {second}.",
            G5Both=>$"قرّب العاملين {first} و{second} إلى أقرب عشرة ثم قدّر ناتج الضرب.",
            G5Compatible=>$"استعمل عددين متوافقين للتقدير: قرب {first} إلى أقرب {(first>=100?100:10)} و{second} إلى أقرب عشرة إذا كان من رقمين، ثم أوجد ناتج ضربهما.",
            _=>$"قرّب العاملين {first} و{second} إلى أقرب عشرة، ثم اكتب ١ إذا التقدير أكبر من الناتج الدقيق أو مساويًا له و٠ إن كان أقل."
        };
        return new Item(f,question,
            "حدد طريقة التقدير المذكورة، أجر التقريب أو اختيار الأعداد المتوافقة، ثم احسب الناتج وتحقق من المعقولية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"first",first},{"second",second}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("first",out int a)||!p.TryGetValue("second",out int b)||
           a is <20 or >=999||b is <2 or >=99)return false;
        int roundedA=RoundTo(a,10),roundedB=b<10?b:RoundTo(b,10);
        int compatibleA=RoundTo(a,a>=100?100:10);
        answer=f switch
        {
            G5One=>roundedA*b,
            G5Both=>roundedA*roundedB,
            G5Compatible=>compatibleA*roundedB,
            _=>roundedA*roundedB>=a*b?1:0
        };
        return true;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5U5(f))return ValidG5U5(f,p,out expected);
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("factor",out int a)||
           !p.TryGetValue("number",out int b)||a is <2 or >9||b is <105 or >=9850)return false;
        int place=b>=1000?1000:100,rounded=((b+place/2)/place)*place;
        expected=f==Estimate?a*rounded:(rounded>=b?1:0);
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid estimate parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5U5(f))return VerifyG5U5(f,p,answer);
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}

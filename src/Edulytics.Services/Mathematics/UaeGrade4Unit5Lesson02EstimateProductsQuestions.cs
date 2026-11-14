using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit5Lesson02EstimateProductsQuestions
{
    internal const string Estimate="uae.g4.u05.l02.estimate_two_digit_product";
    internal const string Compare="uae.g4.u05.l02.estimate_above_or_below";
    internal const string AreaProduct="uae.g4.u05.l03.area_two_digit_product";
    internal const string TensPartial="uae.g4.u05.l03.area_tens_partial";
    internal const string OnesPartial="uae.g4.u05.l03.area_ones_partial";
    // Extend the existing G4 distributive/partial-product engine for Grade 5
    // two-digit multipliers and an optional three-digit challenge.
    internal const string G5Product="uae.g5.u02.l10.multiply_two_digit_factor";
    internal const string G5Ones="uae.g5.u02.l10.ones_partial";
    internal const string G5Tens="uae.g5.u02.l10.tens_partial";
    internal const string G5Estimate="uae.g5.u02.l10.estimate_product";
    internal const string G5Challenge="uae.g5.u02.l10.three_digit_challenge";
    private static bool IsG5(string? f)=>f is G5Product or G5Ones or G5Tens or G5Estimate or G5Challenge;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or Compare or AreaProduct or TensPartial or OnesPartial || IsG5(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown estimate/product family");
        if(IsG5(f))return BuildG5(f,r);
        int a=r.Next(11,100),b=r.Next(11,100);
        string prompt=f switch
        {
            Estimate=>$"قرّب {a} و{b} إلى أقرب عشرة ثم قدّر حاصل الضرب {a} × {b}.",
            Compare=>$"قرّب {a} و{b} لأقرب عشرة ثم قارن التقدير بالناتج الدقيق: اكتب ١ إذا كان أكبر أو مساويًا، و٠ إذا كان أصغر.",
            AreaProduct=>$"مثّل مستطيلًا طوله {a} وعرضه {b}، ثم أوجد ناتج الضرب باستخدام خاصية التوزيع.",
            TensPartial=>$"في نموذج المساحة لـ {a} × {b}، ما حاصل ضرب {a} في عشرات {b} فقط؟",
            _=>$"في نموذج المساحة لـ {a} × {b}، ما حاصل ضرب {a} في آحاد {b} فقط؟"
        };
        return new Item(f,prompt,"أقرّب العاملين إلى أقرب عشرة، ثم أضرب وأقارن حاصل التقدير بالناتج الحقيقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"first",a},{"second",b}});
    }

    private static int Round(int n,int place)=>((n+place/2)/place)*place;
    private static Item BuildG5(string family,Random r)
    {
        int a=family==G5Challenge?r.Next(100,999):r.Next(12,999);
        int b=family==G5Challenge?r.Next(101,151):r.Next(11,100);
        string prompt=family switch
        {
            G5Product=>$"احسب {a} × {b} بالضرب في الآحاد ثم العشرات وجمع نواتج الضرب الجزئية.",
            G5Ones=>$"في حساب {a} × {b}، كم ناتج ضرب {a} في رقم آحاد {b} فقط؟",
            G5Tens=>$"في حساب {a} × {b}، كم ناتج ضرب {a} في قيمة عشرات {b} فقط؟",
            G5Estimate=>$"قدّر {a} × {b} بتقريب الأول إلى أقرب {(a>=100?100:10)} والثاني إلى أقرب عشرة.",
            _=>$"تحدٍّ: استعمل النواتج الجزئية لإيجاد حاصل ضرب {a} × {b} بما فيه منزلة المئات."
        };
        return new Item(family,prompt,
            "أضرب العامل الأول في آحاد الثاني ثم في عشراته، وأضيف ناتج المئات إذا وُجد؛ ثم أجمع النواتج الجزئية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"first",a},{"second",b}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("first",out int a)||!p.TryGetValue("second",out int b)||
           a is <12 or >=999 ||
           (f==G5Challenge?(a<100||b<101||b>150):(b<11||b>=100)))
            return false;
        int roundedFirst=Round(a,a>=100?100:10);
        int roundedSecond=Round(b,10);
        answer=f switch
        {
            G5Product or G5Challenge=>a*b,
            G5Ones=>a*(b%10),
            G5Tens=>a*(b/10%10*10),
            _=>roundedFirst*roundedSecond
        };
        return true;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(family))return ValidG5(family,p,out expected);
        if(!Supports(family)||p.Count!=2||!p.TryGetValue("first",out var a)||
           !p.TryGetValue("second",out var b)||a is <11 or >99||b is <11 or >99)return false;
        int roundedA=(a+5)/10*10,roundedB=(b+5)/10*10;
        expected=family switch
        {
            Estimate=>roundedA*roundedB,
            Compare=>roundedA*roundedB>=a*b?1:0,
            AreaProduct=>a*b,
            TensPartial=>a*(b/10*10),
            _=>a*(b%10)
        };
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid estimation parameters");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==x;
    }
}

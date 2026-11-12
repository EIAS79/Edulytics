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
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or Compare or AreaProduct or TensPartial or OnesPartial;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown grade4 estimate");
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
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
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

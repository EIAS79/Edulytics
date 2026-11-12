using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Estimate products after rounding the multi-digit factor to its greatest place value.
internal static class UaeGrade4Unit4Lesson02EstimateProductsQuestions
{
    internal const string Estimate="uae.g4.u04.l02.estimate_rounded_product";
    internal const string AboveBelow="uae.g4.u04.l02.estimate_above_below";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or AboveBelow;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown estimation family");
        int factor=random.Next(2,10),multidigit=random.Next(105,9850);
        int power=multidigit>=1000?1000:100;
        int rounded=((multidigit+power/2)/power)*power;
        string prompt=f==Estimate
          ?$"قرّب العدد {multidigit} إلى أكبر منزلة ثم قدّر ناتج {factor} × {multidigit}. اكتب الناتج التقديري."
          :$"قرّب العدد {multidigit} إلى أكبر منزلة ثم قارن تقدير {factor} × {multidigit} بالناتج الدقيق. اكتب ١ إذا التقدير أكبر أو مساوي، و٠ إذا أصغر.";
        return new Item(f,prompt,"أقرّب العامل الأكبر إلى أكبر قيمة مكانية ثم أضرب وأقارن بالناتج الفعلي.",
          AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"number",multidigit}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
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
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}

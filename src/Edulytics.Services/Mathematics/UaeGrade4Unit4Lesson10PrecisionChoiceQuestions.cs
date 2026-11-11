using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// U04 L10: choose precise computation or estimation based on question wording.
internal static class UaeGrade4Unit4Lesson10PrecisionChoiceQuestions
{
    internal const string Family="uae.g4.u04.l10.choose_answer_precision";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family==Family;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported precision question");
        int factor=random.Next(2,10),amount=random.Next(101,998),request=random.Next(0,2);
        string prompt=request==1
          ?$"يتعين دفع ثمن {factor} تذاكر، تكلفة الواحدة {amount} درهمًا. هل المطلوب حساب إجابة دقيقة أم تقريبية؟ اكتب ١ للدقيقة و٠ للتقريبية."
          :$"تتوقع مدرسة شراء حوالي {factor} مجموعات، كل مجموعة تكلف {amount} درهمًا. ما نوع الإجابة المناسبة لتقدير الميزانية تقريبيًا؟ اكتب ١ للدقيقة و٠ للتقريبية.";
        return new Item(family,prompt,"العدد المطلوب للدفع بالفعل يستلزم نتيجة دقيقة؛ أما كمية تقريبية أو ميزانية حول قيمة معينة فيناسبها التقدير.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"amount",amount},{"request",request}});
    }
    private static bool Valid(IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(p.Count!=3||!p.TryGetValue("factor",out int a)||!p.TryGetValue("amount",out int b)||
           !p.TryGetValue("request",out int k)||a is <2 or >9||b is <101 or >997||k is <0 or >1)return false;
        expected=k;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p,out int x))throw new InvalidOperationException("Invalid precision parameters");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit5Lesson05MultiStepQuestions
{
    internal const string Net="uae.g4.u05.l05.net_weekly_savings";
    internal const string Missing="uae.g4.u05.l05.missing_after_groups";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Net or Missing;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported multi-step family");
        int a=r.Next(4,21),b=r.Next(1,a),n=r.Next(4,53);
        string prompt=f==Net?$"يدخر طالب {a} درهمًا أسبوعيًا وينفق {b} دراهم. كم يدخر بعد {n} أسبوعًا؟":
            $"يوجد {n} صناديق في كل منها {a} قطع، وأضيفت {b} قطع وقطع مجهولة حتى أصبح المجموع {n*a+b+(a+b)}. ما العدد المجهول؟";
        return new Item(f,prompt,"أمثل المسألة بمعادلة: أحدد المجموعات أو الفرق، وأطبق العمليات بترتيبها ثم أتحقق.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"first",a},{"second",b},{"count",n}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("first",out var a)||
           !p.TryGetValue("second",out var b)||!p.TryGetValue("count",out var n)||
           a is <4 or >20||b<1||b>=a||n is <4 or >52)return false;
        result=f==Net?(a-b)*n:a+b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid multi-step parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==x;
    }
}

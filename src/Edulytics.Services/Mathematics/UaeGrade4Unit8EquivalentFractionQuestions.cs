using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8EquivalentFractionQuestions
{
    internal const string Model="uae.g4.u08.l03.fraction_strip_equivalence";
    internal const string Line="uae.g4.u08.l03.number_line_equivalence";
    internal const string Multiply="uae.g4.u08.l04.multiply_fraction_pair";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Model or Line or Multiply;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction equivalence family");
        int denominator=random.Next(2,9);
        int numerator=random.Next(1,denominator);
        int factor=random.Next(2,6);
        string text=family switch
        {
            Model=>$"يمثل شريط الكسر {numerator}/{denominator}. عند تقسيم كل جزء إلى {factor} أجزاء متساوية، كم جزءًا ملونًا في الشريط الجديد؟",
            Line=>$"تقع النقطة {numerator}/{denominator} على خط من ٠ إلى ١. إذا قسمنا الخط إلى {denominator*factor} جزءًا، عند أي قسم تقع النقطة؟",
            _=>$"أكمل الكسر المكافئ: {numerator}/{denominator} = □/{denominator*factor}. ما قيمة البسط المجهول؟"
        };
        return new Item(family,text,"اضرب البسط والمقام في العدد نفسه ثم تحقق من تساوي المقدارين.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator},{"factor",factor}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||
            !p.TryGetValue("numerator",out int n)||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("factor",out int f)||d is <2 or >8||n<1||n>=d||f is <2 or >5)return false;
        answer=n*f;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int ans))throw new InvalidOperationException("Invalid fraction equivalence parameters");
        return ans.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}

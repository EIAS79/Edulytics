using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson01LikeFractionModelsQuestions
{
    internal const string Add="uae.g4.u09.l01.model_add_like_denominators";
    internal const string Decompose="uae.g4.u09.l01.model_decompose_fraction";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add or Decompose;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported like denominator model");
        int d=random.Next(4,13),a=random.Next(1,d-1),b=random.Next(1,d-a);
        var p=new Dictionary<string,int>{{"denominator",d},{"a",a},{"b",b}};
        string prompt=f==Add?
            $"على شريط مقسم إلى {d} أجزاء متساوية، ظلل {a} أجزاء ثم {b} أجزاء أخرى. ما بسط الكسر المجموع؟":
            $"فكك الكسر {(a+b)}/{d} إلى {a}/{d} + □/{d}. ما البسط المجهول؟";
        return new Item(f,prompt,"اجمع الأجزاء أو أوجد الفرق بين البسط الكلي والجزء المعلوم؛ احتفظ بالمقام.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
            d is <4 or >12||a<1||b<1||a+b>d)return false;
        answer=f==Add?a+b:b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid like denominator parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

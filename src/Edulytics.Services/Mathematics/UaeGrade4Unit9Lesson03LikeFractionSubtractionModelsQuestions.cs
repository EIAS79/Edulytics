using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson03LikeFractionSubtractionModelsQuestions
{
    internal const string Remaining="uae.g4.u09.l03.model_remaining_parts";
    internal const string Removed="uae.g4.u09.l03.model_removed_parts";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Remaining or Removed;
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported model fraction subtraction");
        int d=r.Next(4,13),a=r.Next(2,d+1),b=r.Next(1,a);
        var p=new Dictionary<string,int>{{"denominator",d},{"totalParts",a},{"removedParts",b}};
        string prompt=family==Remaining?
            $"على شريط مقسم إلى {d} أجزاء، ظلل {a} أجزاء ثم اشطب {b} منها. ما بسط الجزء المتبقي؟":
            $"من شريط به {a} أجزاء مظللة من {d}، بقي {a-b} جزءًا بعد الإزالة. كم جزءًا أزيل؟";
        return new Item(family,prompt,"اطرح عدد الأجزاء التي أزيلت من عدد الأجزاء الأولي، واحتفظ بالمقام.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("totalParts",out int a)||!p.TryGetValue("removedParts",out int b)||
           d is <4 or >12||a<2||a>d||b<1||b>=a)return false;
        answer=f==Remaining?a-b:b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid subtraction model parameters");
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

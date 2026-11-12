using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson05WorkingBackwardsQuestions
{
    internal const string MissingPart="uae.g4.u09.l05.missing_fraction_part";
    internal const string ReverseStep="uae.g4.u09.l05.reverse_operation";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family) => family is MissingPart or ReverseStep;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported reverse calculation");
        int denominator=random.Next(4,13),a=random.Next(1,denominator-2),
            b=random.Next(1,denominator-a),missing=denominator-a-b;
        var parameters=new Dictionary<string,int>
        {{"denominator",denominator},{"used",a},{"remaining",b}};
        string prompt=family==MissingPart?
            $"كانت لديك وحدة كاملة من {denominator} أجزاء. استُخدم {a}/{denominator} وبقي {b}/{denominator}. كم جزءًا من {denominator} استُخدم لأمر آخر؟":
            $"كان المجموع {denominator}/{denominator}، وعُرف استخدام {a}/{denominator} وبقاء {b}/{denominator}. ما بسط الكسر المجهول الذي يُكمل الكل؟";
        return new Item(family,prompt,"ابدأ بالكل واطرح الأجزاء المعروفة. تحقق بجمع جميع الأجزاء للحصول على الكل.",
            AssessmentItemType.Numeric,parameters);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("used",out int used)||!p.TryGetValue("remaining",out int remaining)||
           d is <4 or >12||used<1||remaining<1||used+remaining>=d)return false;
        answer=d-used-remaining;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid reverse problem");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string value=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

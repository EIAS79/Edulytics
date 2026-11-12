using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson09MixedNumberQuestions
{
    internal const string Fourths="uae.g4.u08.l09.mixed_as_fourths";
    internal const string UnitFractionCount="uae.g4.u08.l09.mixed_unit_fraction_count";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Fourths or UnitFractionCount;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported mixed number family");
        int whole=random.Next(1,6),d=f==Fourths?4:random.Next(2,9),n=random.Next(1,d);
        var p=new Dictionary<string,int>{{"whole",whole},{"numerator",n},{"denominator",d}};
        string prompt=f==Fourths?
            $"لديك {whole} وحدات كاملة و{n}/{d} وحدة إضافية. كم ربعًا تمثل الكمية كلها؟":
            $"العدد الكسري هو {whole} و{n}/{d}. كم جزءًا من فئة ١/{d} يمثل هذا المقدار؟";
        return new Item(f,prompt,"حوّل كل وحدة كاملة إلى عدد من الأجزاء يساوي المقام ثم أضف البسط.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("whole",out int w)||
           !p.TryGetValue("numerator",out int n)||!p.TryGetValue("denominator",out int d)||
           w is <1 or >6||d is <2 or >8||n<1||n>=d||(f==Fourths&&d!=4))return false;
        answer=w*d+n;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid mixed number parameters");
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

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson07BenchmarkFractionQuestions
{
    internal const string Half="uae.g4.u08.l07.compare_with_half";
    internal const string BenchmarkOrder="uae.g4.u08.l07.benchmark_order";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Half or BenchmarkOrder;
    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unsupported benchmark family");
        int denominator=random.Next(3,13);
        int numerator=random.Next(1,denominator);
        var p=new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator}};
        string prompt=family==Half?
            $"قارن الكسر {numerator}/{denominator} بالنصف: اكتب ٠ للأصغر و١ للمساوي و٢ للأكبر.":
            $"حدد موقع الكسر {numerator}/{denominator} بالنسبة إلى النصف للبدء بترتيب الكسور: ٠ تحته، ١ يساويه، ٢ فوقه.";
        return new Item(family,prompt,"قارن ضعف البسط بالمقام لتحديد موقع الكسر بالنسبة إلى النصف.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("numerator",out int n)||
            !p.TryGetValue("denominator",out int d)||d is <3 or >12||n<1||n>=d) return false;
        result=2*n<d?0:2*n==d?1:2;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid benchmark parameters");
        return x.ToString(CultureInfo.InvariantCulture);
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

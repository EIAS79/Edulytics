using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson10ImproperFractionsQuestions
{
    internal const string Whole="uae.g4.u08.l10.improper_whole";
    internal const string Remainder="uae.g4.u08.l10.improper_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Whole or Remainder;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported improper fraction family");
        int denominator=random.Next(2,11),whole=random.Next(1,6),remainder=random.Next(0,denominator);
        int numerator=whole*denominator+remainder;
        var parameters=new Dictionary<string,int>{{"improper",numerator},{"denominator",denominator}};
        string prompt=f==Whole?
            $"حوّل الكسر المعتل {numerator}/{denominator} إلى عدد كسري. ما عدد الوحدات الكاملة؟":
            $"حوّل الكسر المعتل {numerator}/{denominator} إلى عدد كسري. ما بسط الجزء الكسري المتبقي بعد القسمة؟";
        return new Item(f,prompt,"اقسم البسط على المقام؛ خارج القسمة وحدات كاملة والباقي بسط الكسر.",
            AssessmentItemType.Numeric,parameters);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("improper",out int n)||
           !p.TryGetValue("denominator",out int d)||d is <2 or >10||n<d||n>6*d)return false;
        answer=f==Whole?n/d:n%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid improper fraction parameters");
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

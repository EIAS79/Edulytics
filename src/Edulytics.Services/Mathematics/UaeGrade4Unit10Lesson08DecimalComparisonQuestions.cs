using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson08DecimalComparisonQuestions
{
    internal const string Compare="uae.g4.u10.l08.decimal_compare";
    internal const string Larger="uae.g4.u10.l08.larger_hundredths";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Compare or Larger;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported decimal comparison");
        int a=random.Next(0,100),b=random.Next(0,100);
        if(a==b)b=(b+1)%100;
        var p=new Dictionary<string,int>{{"firstHundredths",a},{"secondHundredths",b}};
        string x=(a/100m).ToString("0.00",CultureInfo.InvariantCulture);
        string y=(b/100m).ToString("0.00",CultureInfo.InvariantCulture);
        string prompt=family==Compare?
            $"قارن {x} و{y}: اكتب ٠ إذا الأول أصغر، و١ إذا تساويا، و٢ إذا الأول أكبر.":
            $"أي العددين أكبر: {x} أو {y}؟ اكتب عدد أجزاء المئة في العدد الأكبر.";
        return new Item(family,prompt,"قارن الأعشار ثم أجزاء المئة، أو استخدم نموذج مئة مربع.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(family)||p.Count!=2||
           !p.TryGetValue("firstHundredths",out int a)||!p.TryGetValue("secondHundredths",out int b)||
           a is <0 or >99||b is <0 or >99)return false;
        result=family==Compare?(a<b?0:a==b?1:2):Math.Max(a,b);
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid decimal comparison");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

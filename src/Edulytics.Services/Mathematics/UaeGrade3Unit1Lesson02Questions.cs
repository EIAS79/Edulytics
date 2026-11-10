using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Extends existing exact engine with source-aligned 4-digit comparisons.
internal static class UaeGrade3Unit1Lesson02Questions
{
    internal const string Compare = "uae.g3.u01.l02.compare_4digit_relation";
    internal const string Greater = "uae.g3.u01.l02.find_greater";
    internal const string Smaller = "uae.g3.u01.l02.find_smaller";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) => family is Compare or Greater or Smaller;
    public static Item Build(string family, Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G3 comparison family");
        var a=rng.Next(1000,10000);var b=rng.Next(1000,10000);
        if(rng.Next(6)==0)b=a;
        // For numeric pick-greater/smaller avoid ambiguous equal answers.
        if(family!=Compare && a==b)b=a==9999?a-1:a+1;
        int rel=a>b?1:a<b?-1:0;
        int result=family==Greater?Math.Max(a,b):family==Smaller?Math.Min(a,b):rel;
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"rel",rel},{"result",result}};
        string prompt=family switch
        {
            Compare=>$"ضع العلامة الصحيحة > أو < أو = بين العددين: {a} □ {b}.",
            Greater=>$"أي العددين أكبر: {a} أم {b}؟ اكتب العدد الأكبر.",
            _=>$"أي العددين أصغر: {a} أم {b}؟ اكتب العدد الأصغر."
        };
        return new Item(family,prompt,
            "ابدأ بمقارنة الآلاف، ثم المئات ثم العشرات ثم الآحاد، وتوقف عند أول اختلاف.",
            family==Compare?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||a is <1000 or >9999||
           !p.TryGetValue("b",out int b)||b is <1000 or >9999||
           !p.TryGetValue("rel",out int rel)||rel!=(a>b?1:a<b?-1:0)||
           !p.TryGetValue("result",out result))return false;
        if(family!=Compare&&a==b)return false;
        return result==(family==Greater?Math.Max(a,b):family==Smaller?Math.Min(a,b):rel);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int r))throw new InvalidOperationException("Bad grade3 compare parameters");
        return family==Compare?(r==1?">":r==-1?"<":"="):r.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int r)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==Compare)return string.Equals(answer.Trim(),r==1?">":r==-1?"<":"=",StringComparison.Ordinal);
        string t=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.Integer,CultureInfo.InvariantCulture,out int entered)&&entered==r;
    }
}

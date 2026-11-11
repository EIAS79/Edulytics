using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE Grade 4 U01 L03 reuses the G3 exact comparison pattern, extended to millions.
internal static class UaeGrade4Unit1ComparisonQuestions
{
    internal const string Relation="uae.g4.u01.l03.compare_whole_numbers";
    internal const string Greater="uae.g4.u01.l03.select_greater";
    internal const string Smaller="uae.g4.u01.l03.select_smaller";

    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family) =>
        family is Relation or Greater or Smaller;

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family)) throw new InvalidOperationException("Unknown UAE G4 compare family");
        int a=rng.Next(10_000,10_000_000),b=rng.Next(10_000,10_000_000);
        if(rng.Next(7)==0)b=a;
        if(family!=Relation&&a==b)b=a==9_999_999?a-1:a+1;
        var parameters=new Dictionary<string,int>{{"a",a},{"b",b}};
        string prompt=family switch
        {
            Relation=>$"ضع > أو < أو = في الفراغ: {a} □ {b}.",
            Greater=>$"أيهما أكبر: {a} أم {b}؟ اكتب العدد الأكبر.",
            _=>$"أيهما أصغر: {a} أم {b}؟ اكتب العدد الأصغر."
        };
        return new Item(family,prompt,
            "قارن عدد الخانات ثم ابحث عن أول منزلة مختلفة من اليسار.",
            family==Relation?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            parameters);
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int a,out int b)
    {
        a=0;b=0;
        return Supports(family)&&p.Count==2&&
            p.TryGetValue("a",out a)&&a>=10_000&&a<=9_999_999&&
            p.TryGetValue("b",out b)&&b>=10_000&&b<=9_999_999&&
            (family==Relation||a!=b);
    }

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int a,out int b))
            throw new InvalidOperationException("Invalid UAE G4 comparison");
        return family switch
        {
            Relation=>a>b?">":a<b?"<":"=",
            Greater=>Math.Max(a,b).ToString(CultureInfo.InvariantCulture),
            _=>Math.Min(a,b).ToString(CultureInfo.InvariantCulture)
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int a,out int b)||string.IsNullOrWhiteSpace(answer))return false;
        if(family==Relation)
            return answer.Trim()==(a>b?">":a<b?"<":"=");
        string normalized=new(answer.Trim()
            .Where(x=>x!=','&&x!='٬'&&x!=' '&&x!='\u00a0')
            .Select(x=>x is >= '٠' and <= '٩'?(char)('0'+x-'٠'):
                       x is >= '۰' and <= '۹'?(char)('0'+x-'۰'):x).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
        return v==(family==Greater?Math.Max(a,b):Math.Min(a,b));
    }
}

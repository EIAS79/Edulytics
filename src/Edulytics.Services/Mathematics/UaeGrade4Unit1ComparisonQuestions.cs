using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// UAE Grade 4 U01 L03 reuses the G3 exact comparison pattern, extended to millions.
internal static class UaeGrade4Unit1ComparisonQuestions
{
    internal const string Relation="uae.g4.u01.l03.compare_whole_numbers";
    internal const string Greater="uae.g4.u01.l03.select_greater";
    internal const string Smaller="uae.g4.u01.l03.select_smaller";
    internal const string OrderAsc="uae.g4.u01.l04.order_three_ascending";
    internal const string OrderDesc="uae.g4.u01.l04.order_three_descending";
    internal const string Middle="uae.g4.u01.l04.middle_of_three";

    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family) =>
        family is Relation or Greater or Smaller or OrderAsc or OrderDesc or Middle;

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family)) throw new InvalidOperationException("Unknown UAE G4 compare family");
        if (family is OrderAsc or OrderDesc or Middle)
        {
            int x=rng.Next(10_000,10_000_000);
            int y=rng.Next(10_000,10_000_000);
            int z=rng.Next(10_000,10_000_000);
            while(x==y) y=rng.Next(10_000,10_000_000); while(y==z || x==z) z=rng.Next(10_000,10_000_000);
            var p3=new Dictionary<string,int>{{"a",x},{"b",y},{"c",z}};
            string prompt3=family switch
            {
                OrderAsc=>$"رتب الأعداد تصاعديًا، واكتبها مفصولة بفاصلة منقوطة: {x}، {y}، {z}.",
                OrderDesc=>$"رتب الأعداد تنازليًا، واكتبها مفصولة بفاصلة منقوطة: {x}، {y}، {z}.",
                _=>$"ما العدد الأوسط بعد ترتيب {x} و{y} و{z}؟"
            };
            return new Item(family,prompt3,
                "قارن عدد الخانات ثم المنازل من اليسار إلى اليمين ورتب القيم.",
                family==Middle?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p3);
        }
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

    private static bool ValidThree(IReadOnlyDictionary<string,int> p,out int[] values)
    {
        values=[];
        if(p.Count!=3 || !p.TryGetValue("a",out int a) ||
           !p.TryGetValue("b",out int b) || !p.TryGetValue("c",out int c) ||
           new[]{a,b,c}.Any(x=>x<10_000||x>9_999_999) ||
           a==b || b==c || a==c) return false;
        values=[a,b,c]; Array.Sort(values); return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family is OrderAsc or OrderDesc or Middle)
        {
            if(!ValidThree(p,out var vals))throw new InvalidOperationException("Invalid G4 ordering");
            if(family==OrderDesc)Array.Reverse(vals);
            return family==Middle?vals[1].ToString(CultureInfo.InvariantCulture)
                :string.Join(";",vals);
        }
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
        if(family is OrderAsc or OrderDesc or Middle)
        {
            if(string.IsNullOrWhiteSpace(answer)||!ValidThree(p,out int[] vals))return false;
            if(family==OrderDesc)Array.Reverse(vals);
            string normalizedOrder=new(answer.Trim().Where(c=>c!='٬'&&c!=','&&c!=' '&&c!='\u00a0')
                .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                          c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
            if(family==Middle)return normalizedOrder==vals[1].ToString(CultureInfo.InvariantCulture);
            return normalizedOrder==string.Join(";",vals);
        }
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

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade3Unit1Lesson03Questions
{
    internal const string Asc="uae.g3.u01.l03.order_three_ascending";
    internal const string Desc="uae.g3.u01.l03.order_three_descending";
    internal const string Middle="uae.g3.u01.l03.identify_middle_number";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Asc or Desc or Middle;
    public static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G3 U01 L03 family");
        int a=r.Next(100,10000),b=r.Next(100,10000),c=r.Next(100,10000);
        while(b==a)b=r.Next(100,10000);
        while(c==a||c==b)c=r.Next(100,10000);
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
        var prompt=f==Asc?$"رتب هذه الأعداد من الأصغر إلى الأكبر مفصولًا بفواصل: {a}؛ {b}؛ {c}."
            :f==Desc?$"رتب هذه الأعداد من الأكبر إلى الأصغر مفصولًا بفواصل: {a}؛ {b}؛ {c}."
            :$"رتب الأعداد ذهنيًا: {a}، {b}، {c}. ما العدد الذي يقع في المنتصف؟";
        var ans=Solve(f,p);
        return new Item(f,prompt,"أقارن من الآلاف إلى الآحاد ثم أضع الأعداد في ترتيبها الصحيح.",
            f==Middle?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        return Supports(f)&&p.Count==3&&p.TryGetValue("a",out var a)&&a is >=100 and <=9999&&
            p.TryGetValue("b",out var b)&&b is >=100 and <=9999&&
            p.TryGetValue("c",out var c)&&c is >=100 and <=9999&&a!=b&&a!=c&&b!=c;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Invalid G3 ordering parameters");
        int[] values=[p["a"],p["b"],p["c"]];Array.Sort(values);
        if(f==Desc)Array.Reverse(values);
        return f==Middle?values[1].ToString(CultureInfo.InvariantCulture):string.Join(",",values);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        var normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        if(f==Middle)return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)
            &&n.ToString(CultureInfo.InvariantCulture)==Solve(f,p);
        var parts=normalized.Replace('؛',',').Replace('،',',').Split(',',StringSplitOptions.TrimEntries|StringSplitOptions.RemoveEmptyEntries);
        if(parts.Length!=3)return false;
        var numbers=new int[3];
        for(int i=0;i<3;i++)
            if(!int.TryParse(parts[i],NumberStyles.Integer,CultureInfo.InvariantCulture,out numbers[i]))return false;
        return string.Join(",",numbers)==Solve(f,p);
    }
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// G3 Unit 3 mental subtraction: extends the shared exact engine,
// alongside existing Grade2 subtraction families. No publisher text/graphics.
internal static class UaeGrade3Unit3Lesson01Questions
{
    internal const string Family = "uae.g3.u03.l01.mental_split_subtract";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) => family == Family;

    public static Item Build(string family,Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G3 mental subtraction");
        int a=rng.Next(100,1000);
        int mode=rng.Next(3);
        int b=mode switch {1=>99,2=>29,_=>rng.Next(11,90)};
        int result=a-b;
        var prompt=mode==1?$"استخدم التعويض: {a} − 99 = {a} − 100 + 1. ما الناتج؟"
           :mode==2?$"استخدم التعويض: {a} − 29 = {a} − 30 + 1. ما الناتج؟"
           :$"فكك {b} إلى عشرات وآحاد واطرح ذهنيًا من {a}. اكتب الفرق.";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",result}};
        return new Item(family,prompt,
            "أطرح العشرات ثم الآحاد أو أطرح عشرة/مئة كاملة وأعوّض بإرجاع ١.",
            AssessmentItemType.Numeric,p);
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(family)||p.Count!=4||!p.TryGetValue("a",out int a)||a is <100 or >999||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("mode",out int mode)||mode is <0 or >2||
           !p.TryGetValue("result",out int supplied))return false;
        if(mode==1&&b!=99||mode==2&&b!=29||mode==0&&b is <11 or >89)return false;
        result=a-b;
        return result>=0&&result==supplied;
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int v))throw new InvalidOperationException("Invalid G3 mental subtraction parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int correct)||string.IsNullOrWhiteSpace(answer))return false;
        string digits=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
           c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out int value)&&value==correct;
    }
}

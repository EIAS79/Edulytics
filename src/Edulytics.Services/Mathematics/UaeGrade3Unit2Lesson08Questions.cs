using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extends the existing exact Practice engine with four-digit column addition.
internal static class UaeGrade3Unit2Lesson08Questions
{
    internal const string Family="uae.g3.u02.l08.add_four_digit";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported 4-digit addition family");
        int mode=r.Next(2),a,b;
        if(mode==0){a=r.Next(1000,5000);b=r.Next(1000,5000);}
        else
        {
            int x=r.Next(6,10),y=r.Next(10-x,10);
            a=r.Next(1,5)*1000+r.Next(10)*100+r.Next(10)*10+x;
            b=r.Next(1,5)*1000+r.Next(10)*100+r.Next(10)*10+y;
        }
        int total=a+b;
        var prompt=mode==0?$"اجمع {a} + {b} باستخدام محاذاة الآلاف والمئات والعشرات والآحاد. ما المجموع؟"
           :$"أوجد مجموع {a} + {b} من أربعة أرقام مع إعادة تجميع الآحاد، ثم تحقق بالتقدير.";
        return new Item(f,prompt,"أجمع الآحاد والعشرات والمئات والآلاف مع حمل كل عشرة إلى المنزلة التالية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",total}});
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int sum)
    {
        sum=0;
        if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||a is <1000 or >4999||
            !p.TryGetValue("b",out int b)||b is <1000 or >4999||
            !p.TryGetValue("mode",out int mode)||mode is <0 or >1||
            !p.TryGetValue("result",out int result))return false;
        if(mode==1&&(a%10+b%10<10))return false;
        sum=a+b;
        return sum==result&&sum<=9999;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int total))throw new InvalidOperationException("Invalid G3 four-digit source parameters");
        return total.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out int total)||string.IsNullOrWhiteSpace(answer))return false;
        string ascii=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
           c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)&&n==total;
    }
}

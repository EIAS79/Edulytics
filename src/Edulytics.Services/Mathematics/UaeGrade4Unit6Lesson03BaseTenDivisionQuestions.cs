using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson03BaseTenDivisionQuestions
{
    internal const string Quotient="uae.g4.u06.l03.base_ten_quotient";
    internal const string Remainder="uae.g4.u06.l03.base_ten_remainder";
    internal const string G5Quotient="uae.g5.u03.l03.two_digit_quotient";
    internal const string G5Remainder="uae.g5.u03.l03.two_digit_remainder";
    private static bool IsG5(string? f)=>f is G5Quotient or G5Remainder;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder || IsG5(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown base-ten division family");
        if(IsG5(f))return BuildG5(f,r);
        int dividend=r.Next(20,10000),divisor=r.Next(2,10);
        string prompt=f==Quotient
          ?$"مثّل {dividend} ÷ {divisor} بمكعبات العشرات والآحاد ثم اكتب ناتج القسمة الصحيح دون الباقي."
          :$"قسّم {dividend} إلى {divisor} مجموعات متساوية باستخدام مكعبات القيمة المكانية. كم آحادًا تتبقى؟";
        return new Item(f,prompt,"أوزع العشرات، وأعيد تجميع العشرات المتبقية إلى آحاد، ثم أوزع الآحاد بالتساوي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }

    // Lesson-specific range extension of the existing base-ten division engine.
    private static Item BuildG5(string f,Random r)
    {
        int n=r.Next(20,100),d=r.Next(2,10);
        string q=f==G5Quotient
            ? $"اقسم {n} على {d} باستعمال العشرات والآحاد، واكتب ناتج القسمة الصحيح قبل الباقي."
            : $"اقسم {n} على {d} باستعمال العشرات والآحاد، ثم اكتب الباقي فقط.";
        return new Item(f,q,
            "أقسم العشرات وأُنزل الآحاد، ثم أتحقق من أن الباقي أصغر من المقسوم عليه.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n<20||n>99||d<2||d>9)return false;
        result=f==G5Quotient?n/d:n%d;
        return true;
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||n is <20 or >9999||d is <2 or >9)return false;
        expected=f==Quotient?n/d:n%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid base-ten division");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

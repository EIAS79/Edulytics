using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson03BaseTenDivisionQuestions
{
    internal const string Quotient="uae.g4.u06.l03.base_ten_quotient";
    internal const string Remainder="uae.g4.u06.l03.base_ten_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown base-ten division family");
        int dividend=r.Next(20,100),divisor=r.Next(2,10);
        string prompt=f==Quotient
          ?$"مثّل {dividend} ÷ {divisor} بمكعبات العشرات والآحاد ثم اكتب ناتج القسمة الصحيح دون الباقي."
          :$"قسّم {dividend} إلى {divisor} مجموعات متساوية باستخدام مكعبات القيمة المكانية. كم آحادًا تتبقى؟";
        return new Item(f,prompt,"أوزع العشرات، وأعيد تجميع العشرات المتبقية إلى آحاد، ثم أوزع الآحاد بالتساوي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||n is <20 or >99||d is <2 or >9)return false;
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

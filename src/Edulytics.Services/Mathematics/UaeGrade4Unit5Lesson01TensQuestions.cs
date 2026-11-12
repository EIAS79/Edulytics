using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE student-book U05 L01: a two-digit number multiplied by a multiple of ten.
internal static class UaeGrade4Unit5Lesson01TensQuestions
{
    internal const string Product="uae.g4.u05.l01.tens_product";
    internal const string UnknownTens="uae.g4.u05.l01.unknown_tens_factor";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Product or UnknownTens;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported tens family");
        var n=r.Next(11,100);var tens=r.Next(1,10);
        var prompt=f==Product?$"احسب {n} × {tens*10} باستخدام حقيقة الضرب ثم مضاعفات العشرة.":
            $"أكمل المعادلة {n} × □ = {n*tens*10}. اكتب قيمة العامل المجهول من مضاعفات العشرة.";
        return new Item(f,prompt,"أضرب في رقم العشرات أولًا، ثم أضرب في عشرة؛ ويمكن التحقق بخاصية التجميع.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"number",n},{"tens",tens}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("number",out int n)||
           !p.TryGetValue("tens",out int t)||n is <11 or >99||t is <1 or >9)return false;
        expected=f==Product?n*t*10:t*10;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var x))throw new InvalidOperationException("Invalid tens parameters");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==x;
    }
}

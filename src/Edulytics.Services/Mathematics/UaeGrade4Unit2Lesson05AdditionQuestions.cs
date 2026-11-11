using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U02 L05 whole-number addition: existing exact-engine route.
internal static class UaeGrade4Unit2Lesson05AdditionQuestions
{
    internal const string Add4="uae.g4.u02.l05.add_four_digits";
    internal const string Add6="uae.g4.u02.l05.add_six_digits";
    internal const string Missing="uae.g4.u02.l05.missing_addend";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add4 or Add6 or Missing;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Invalid G4 addition family");
        int a=f==Add6?rng.Next(100000,750000):rng.Next(1200,8500);
        int b=f==Add6?rng.Next(12000,220000):rng.Next(1100,7500);
        string prompt=f switch
        {
            Add4=>$"اجمع {a} + {b} باستعمال جدول المنازل وإعادة التجميع عند الحاجة.",
            Add6=>$"اجمع {a} + {b} باستعمال الجمع الرأسي وإعادة التجميع ثم تحقق بالتقدير.",
            _=>$"أكمل الجملة العددية: {a} + □ = {a+b}."
        };
        return new Item(f,prompt,
            "أحاذي المنازل، ثم أجمع من الآحاد إلى المنازل الكبرى وأعيد التجميع عند الحاجة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b))return false;
        bool valid=f==Add6
            ? a>=100000&&a<750000&&b>=12000&&b<220000
            : a>=1200&&a<8500&&b>=1100&&b<7500;
        if(!valid)return false;
        expected=f==Missing?b:a+b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Bad G4 addition question");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                       c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

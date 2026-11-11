using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U02 L06; exact practice families on the existing question engine.
internal static class UaeGrade4Unit2Lesson06SubtractionQuestions
{
    internal const string SubFour="uae.g4.u02.l06.subtract_four_digits";
    internal const string SubSix="uae.g4.u02.l06.subtract_six_digits";
    internal const string Missing="uae.g4.u02.l06.missing_subtrahend";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is SubFour or SubSix or Missing;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown Grade 4 subtraction");
        int a=f==SubSix?rng.Next(200000,950000):rng.Next(2200,9800);
        int b=f==SubSix?rng.Next(15000,a-10000):rng.Next(300,a-500);
        string question=f==Missing
            ? $"أوجد العدد المجهول: {a} − □ = {a-b}."
            : $"اطرح {a} − {b} بالمنزلة المناسبة، وأعد التجميع عندما يلزم ثم تحقق بالجمع.";
        return new Item(f,question,
            "أحاذي المنازل، وأعيد تجميع عشرة من المنزلة الأعلى ثم أتحقق بجمع الفرق والمطروح.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b))return false;
        bool valid=f==SubSix
            ? a>=200000&&a<950000&&b>=15000&&b<a-10000
            : a>=2200&&a<9800&&b>=300&&b<a-500;
        if(!valid)return false;
        expected=f==Missing?b:a-b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid subtraction parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string t=new(answer.Trim().Where(c=>c!='٬'&&c!=','&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U02 L08: choose a bar model and calculate total, difference or missing part.
internal static class UaeGrade4Unit2Lesson08BarModelQuestions
{
    internal const string PartWhole="uae.g4.u02.l08.bar_part_whole";
    internal const string Compare="uae.g4.u02.l08.bar_compare";
    internal const string MissingPart="uae.g4.u02.l08.bar_missing_part";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is PartWhole or Compare or MissingPart;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown Grade 4 bar model family");
        int a=rng.Next(1400,79000),b=rng.Next(500,Math.Min(a-10,25000));
        string prompt=f switch
        {
            PartWhole=>$"في مخطط شريطي يمثل الجزءان {a} و{b} طالبًا. أوجد الكل.",
            Compare=>$"في مخطط شريطي يظهر عددان {a} و{b}. كم يزيد العدد الأول عن الثاني؟",
            _=>$"شريط كامل طوله العددي {a}، وأحد جزأيه {b}. ما قيمة الجزء المجهول؟"
        };
        return new Item(f,prompt,
            "أحدد الكل والجزءين في نموذج الشريط. أجمع لإيجاد الكل وأطرح لإيجاد الفرق أو الجزء المجهول.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
            !p.TryGetValue("b",out int b)||a<1400||a>=79000||b<500||
            b>=Math.Min(a-10,25000))return false;
        expected=f==PartWhole?a+b:a-b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid bar model question");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

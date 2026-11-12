using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// A-B-C, source-aimed numeric part of four-step reasoning.
// Explanations and diagramming are subject to teacher review.
internal static class UaeGrade4Unit1FourStepQuestions
{
    internal const string Increase="uae.g4.u01.l06.find_increase";
    internal const string Sum="uae.g4.u01.l06.find_total";
    internal const string Remaining="uae.g4.u01.l06.find_remaining";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Increase or Sum or Remaining;

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G4 four-step family");
        int a=rng.Next(150_000,450_000),b=rng.Next(150_000,450_000);
        string prompt=family switch
        {
            Increase=>$"كان عدد زوار معرض في الفترة الأولى {a} وفي الفترة الثانية {a+b}. أفهم المطلوب وأخطط: ما مقدار الزيادة؟",
            Sum=>$"زار مكتبة {a} زائرًا في أسبوع و{b} زائرًا في الأسبوع التالي. ما العدد الكلي للزوار؟",
            _=>$"كان في مستودع {a+b} كتابًا. نُقل منه {a} كتابًا. كم كتابًا بقي؟"
        };
        return new Item(family,prompt,
            "أفهم المعطيات، أحدد العملية، أحسب، ثم أتحقق بالعملية العكسية.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(family)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||
           a<150_000||a>=450_000||b<150_000||b>=450_000)return false;
        expected=family==Sum?a+b:b;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int value))
            throw new InvalidOperationException("Invalid G4 four-step question");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim()
            .Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                     c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,
            CultureInfo.InvariantCulture,out int v)&&v==expected;
    }
}

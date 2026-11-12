using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 U02 L09: exact multi-step whole-number word problems.
internal static class UaeGrade4Unit2Lesson09MultiStepQuestions
{
    internal const string AddSub="uae.g4.u02.l09.add_then_subtract";
    internal const string SubAdd="uae.g4.u02.l09.subtract_then_add";
    internal const string AddAdd="uae.g4.u02.l09.add_three_parts";
    internal const string SubSub="uae.g4.u02.l09.subtract_two_costs";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,
        IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is AddSub or SubAdd or AddAdd or SubSub;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported Grade4 multistep");
        int a=rng.Next(2000,8500),b=rng.Next(200,1600),c=rng.Next(40,190);
        string prompt=f switch
        {
            AddSub=>$"كان في الصندوق {a} درهم، ثم أضيف {b} درهم وصُرف {c} درهم. كم بقي؟ اكتب معادلة.",
            SubAdd=>$"بدأ المعرض بعدد {a} تذكرة، بيع منها {b}، ثم أضيف {c} تذكرة. كم أصبح العدد؟",
            AddAdd=>$"جمع الفريق {a} قطعة، ثم {b} قطعة، ثم {c} قطعة أخرى. ما العدد الكلي؟",
            _=>$"مع مدرسة {a} درهم. اشترت أدوات بـ{b} درهم وكتبًا بـ{c} درهم. كم بقي؟"
        };
        return new Item(f,prompt,
            "أحدد ترتيب أحداث المسألة، وأكتب معادلة للمجهول وأحسب الجمع والطرح من اليسار إلى اليمين ثم أتحقق.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           a<2000||a>=8500||b<200||b>=1600||c<40||c>=190)return false;
        expected=f switch
        {
            AddSub=>a+b-c,
            SubAdd=>a-b+c,
            AddAdd=>a+b+c,
            _=>a-b-c
        };
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid multistep");
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

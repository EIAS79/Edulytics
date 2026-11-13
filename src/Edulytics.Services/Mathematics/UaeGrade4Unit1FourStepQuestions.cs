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
    // UAE Grade 5 Common expands this already-supported four-step engine
    // with inverse and multiplication/division reasoning; Grade 4 unchanged.
    internal const string G5Missing="uae.g5.u01.l09.find_missing_distance";
    internal const string G5Initial="uae.g5.u01.l09.find_starting_total";
    internal const string G5Budget="uae.g5.u01.l09.budget_purchase_count";
    internal const string G5Undo="uae.g5.u01.l09.undo_four_steps";
    private static bool IsG5(string? f)=>f is G5Missing or G5Initial or G5Budget or G5Undo;
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Increase or Sum or Remaining || IsG5(family);

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported four-step family");
        if (IsG5(family)) return BuildG5(family, rng);
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

    // Four-step Grade 5 story parameters contain only given quantities.
    // The answer is derived by the generator's solver and recomputed by Verify.
    private static Item BuildG5(string family, Random rng)
    {
        int a, b, c, d;
        string prompt;
        switch (family)
        {
            case G5Missing:
                a=rng.Next(3,20); b=rng.Next(3,20); c=rng.Next(3,20);
                d=a+b+c+rng.Next(5,25);
                prompt=$"قطعت طالبة في ثلاثة أيام {a} و{b} و{c} كيلومترًا، وكانت المسافة الكلية لأربعة أيام {d} كيلومترًا. كم قطعت في اليوم الرابع؟";
                break;
            case G5Initial:
                a=rng.Next(14,120); b=rng.Next(10,100);
                c=rng.Next(8,100); d=rng.Next(10,110);
                prompt=$"استخدم فريق {a} ورقة في النشاط الأول و{b} ورقة في الثاني و{d} ورقة في الثالث، وبقي معه {c} ورقة. كم ورقة كانت معه أولًا؟";
                break;
            case G5Budget:
                b=rng.Next(8,26); c=rng.Next(2,4); d=rng.Next(3,14);
                a=b*c+d*rng.Next(2,12);
                prompt=$"مع سارة {a} درهمًا. اشترت {c} علب بسعر {b} درهمًا للعلبة ثم استخدمت الباقي لشراء أدوات بسعر {d} دراهم للقطعة. كم قطعة اشترت؟";
                break;
            default:
                a=rng.Next(2,6); b=rng.Next(3,11);
                c=rng.Next(2,9); d=rng.Next(8,28);
                prompt=$"قسّمت عددًا مجهولًا على {a}، ثم أضفت {b}، ثم طرحت {c} فحصلت على {d}. ما العدد الأصلي؟";
                break;
        }
        return new Item(family,prompt,
            "أفهم المطلوب والمعطيات، أخطط بعلاقة عكسية أو جدول، أحسب ثم أتحقق بالتعويض.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}});
    }

    private static bool ValidG5(string family,
        IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d))
            return false;
        switch(family)
        {
            case G5Missing:
                if(a is <3 or >19||b is <3 or >19||c is <3 or >19||
                   d<a+b+c+5||d>a+b+c+24)return false;
                expected=d-(a+b+c);break;
            case G5Initial:
                if(a is <14 or >119||b is <10 or >99||c is <8 or >99||
                   d is <10 or >109)return false;
                expected=a+b+c+d;break;
            case G5Budget:
                if(b is <8 or >25||c is <2 or >3||d is <3 or >13||
                   a<=b*c||a>1_000||(a-b*c)%d!=0||
                   (a-b*c)/d is <2 or >11)return false;
                expected=(a-b*c)/d;break;
            case G5Undo:
                if(a is <2 or >5||b is <3 or >10||c is <2 or >8||
                   d is <8 or >27)return false;
                expected=(d-b+c)*a;break;
            default:return false;
        }
        return expected>0;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(family)) return ValidG5(family,p,out expected);
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

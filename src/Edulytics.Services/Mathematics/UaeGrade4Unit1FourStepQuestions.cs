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
    internal const string G5TableScale="uae.g5.u02.l05.table_population_scale";
    internal const string G5TableDouble="uae.g5.u02.l05.table_weekly_doubling";
    internal const string G5TableRate="uae.g5.u02.l05.table_constant_rate";
    private static bool IsG5Table(string? f)=>f is G5TableScale or G5TableDouble or G5TableRate;
    private static bool IsG5(string? f)=>f is G5Missing or G5Initial or G5Budget or G5Undo;
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Increase or Sum or Remaining || IsG5(family) || IsG5Table(family);

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported four-step family");
        if (IsG5(family)) return BuildG5(family, rng);
        if (IsG5Table(family)) return BuildG5Table(family,rng);
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

    // G5 Unit 2 L05 reuses the existing four-step numeric question pipeline.
    // Tables and original explanatory reasoning remain in the RichV2 lesson.
    private static Item BuildG5Table(string family,Random random)
    {
        int x,y,z;
        string prompt;
        IReadOnlyDictionary<string,int> p;
        if(family==G5TableScale)
        {
            x=random.Next(12,91);y=100;z=10000;
            prompt=$"إذا كان من كل {y} أسرة عدد {x} يحقق شرطًا، فكم أسرة تقريبًا من أصل {z} أسرة تحقق الشرط نفسه؟ أكمل جدول التناسب.";
            p=new Dictionary<string,int>{{"a",x},{"b",y},{"c",z}};
        }
        else if(family==G5TableDouble)
        {
            x=random.Next(1,6);y=random.Next(2,9);z=2;
            prompt=$"بدأ ادخار أسبوعي بـ {x} دراهم في الأسبوع الأول، ثم تضاعف مبلغ كل أسبوع عن سابقه. كم درهمًا يُدّخر في الأسبوع رقم {y}؟ أنشئ جدولًا.";
            p=new Dictionary<string,int>{{"a",x},{"b",y},{"c",z}};
        }
        else
        {
            x=random.Next(10,101);y=random.Next(1,11);z=random.Next(2,13)*y;
            prompt=$"يتسع نشاط لـ {x} مشاركين خلال {y} دقائق. باستخدام جدول منتظم للمجموعات، كم مشاركًا يستوعب خلال {z} دقيقة بالمعدل نفسه؟";
            p=new Dictionary<string,int>{{"a",x},{"b",y},{"c",z}};
        }
        return new Item(family,prompt,
            "اكتب عمودي المعطيات والقيمة المقابلة لكل مرحلة ثم تحقق من معدل النمو أو التناسب.",
            AssessmentItemType.Numeric,p);
    }
    private static bool ValidG5Table(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5Table(family)||p.Count!=3||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c))return false;
        if(family==G5TableScale)
        {
            if(a is <12 or >90||b!=100||c!=10000)return false;
            expected=checked(a*(c/b));return true;
        }
        if(family==G5TableDouble)
        {
            if(a is <1 or >5||b is <2 or >8||c!=2)return false;
            expected=a;
            for(int i=1;i<b;i++)expected=checked(expected*2);
            return true;
        }
        if(a is <10 or >100||b is <1 or >10||c<2*b||c>12*b||c%b!=0)return false;
        expected=checked(a*(c/b));return true;
    }

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
        if(IsG5Table(family)) return ValidG5Table(family,p,out expected);
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

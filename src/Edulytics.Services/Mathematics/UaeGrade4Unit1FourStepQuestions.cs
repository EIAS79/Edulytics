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
    internal const string G5U4SimpleRate="uae.g5.u04.l06.equal_group_unit_rate";
    internal const string G5U4SimpleScale="uae.g5.u04.l06.simpler_scaled_groups";
    internal const string G5U4SimpleArea="uae.g5.u04.l06.remaining_area";
    internal const string G5U4SimpleWeeks="uae.g5.u04.l06.saving_weeks_strictly_more";
    private static bool IsG5U4Simple(string? f)=>f is G5U4SimpleRate or G5U4SimpleScale or G5U4SimpleArea or G5U4SimpleWeeks;
    internal const string G5Missing="uae.g5.u01.l09.find_missing_distance";
    internal const string G5Initial="uae.g5.u01.l09.find_starting_total";
    internal const string G5Budget="uae.g5.u01.l09.budget_purchase_count";
    internal const string G5Undo="uae.g5.u01.l09.undo_four_steps";
    internal const string G5InfoDivide="uae.g5.u03.l13.ignore_extra_and_divide";
    internal const string G5InfoCompare="uae.g5.u03.l13.ignore_extra_and_compare";
    internal const string G5InfoSufficient="uae.g5.u03.l13.classify_sufficient_information";
    private static bool IsG5Info(string? f)=>f is G5InfoDivide or G5InfoCompare or G5InfoSufficient;
    internal const string G5TableScale="uae.g5.u02.l05.table_population_scale";
    internal const string G5TableDouble="uae.g5.u02.l05.table_weekly_doubling";
    internal const string G5TableRate="uae.g5.u02.l05.table_constant_rate";
    private static bool IsG5Table(string? f)=>f is G5TableScale or G5TableDouble or G5TableRate;
    private static bool IsG5(string? f)=>f is G5Missing or G5Initial or G5Budget or G5Undo;
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Increase or Sum or Remaining || IsG5(family) || IsG5Table(family) || IsG5Info(family) || IsG5U4Simple(family);

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported four-step family");
        if (IsG5U4Simple(family)) return BuildG5U4Simple(family,rng);
        if (IsG5(family)) return BuildG5(family, rng);
        if (IsG5Table(family)) return BuildG5Table(family,rng);
        if (IsG5Info(family)) return BuildG5Info(family,rng);
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


    // UAE G5 Common U04 L06: extend the existing four-step engine with
    // unit rate, scale, area decomposition, and strict saving threshold.
    private static Item BuildG5U4Simple(string f,Random rng)
    {
        string prompt;
        Dictionary<string,int> p;
        if(f is G5U4SimpleRate or G5U4SimpleScale)
        {
            int groups=rng.Next(6,31),perGroup=rng.Next(5,61);
            int total=groups*perGroup,target=rng.Next(3,16);
            p=new Dictionary<string,int>{{"groups",groups},{"total",total}};
            if(f==G5U4SimpleScale)p["target"]=target;
            prompt=f==G5U4SimpleRate
                ?$"توزع {total} قطعة على {groups} مجموعة بالتساوي. كم قطعة في مجموعة واحدة؟"
                :$"توزع {total} قطعة على {groups} مجموعة بالتساوي. كم قطعة نحتاج إلى {target} مجموعات مماثلة؟";
        }
        else if(f==G5U4SimpleArea)
        {
            int length=rng.Next(6,21),width=rng.Next(5,16),side=rng.Next(2,5);
            p=new Dictionary<string,int>{{"length",length},{"width",width},{"side",side}};
            prompt=$"حديقة طولها {length} مترًا وعرضها {width} مترًا، وبداخلها مساحة مربعة غير مزروعة طول ضلعها {side} مترًا. ما مساحة الجزء الباقي بالمتر المربع؟";
        }
        else
        {
            int income=rng.Next(50,121),spend=rng.Next(5,41),target=rng.Next(180,851);
            p=new Dictionary<string,int>{{"income",income},{"spend",spend},{"target",target}};
            prompt=$"يكسب شخص {income} درهمًا في الأسبوع وينفق {spend} درهمًا. كم أسبوعًا يحتاج ليدخر أكثر من {target} درهمًا؟";
        }
        return new Item(f,prompt,"أفهم المسألة وأحدد وحدة أصغر أو جزءًا أبسط، ثم أعد تركيب الإجابة وأتحقق من الشرط.",
            AssessmentItemType.Numeric,p);
    }
    private static bool ValidG5U4Simple(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5U4Simple(f))return false;
        if(f is G5U4SimpleRate or G5U4SimpleScale)
        {
            if(p.Count!=(f==G5U4SimpleRate?2:3)||
               !p.TryGetValue("groups",out int groups)||!p.TryGetValue("total",out int total)||
               groups is <6 or >30||total is <30 or >15_000||total%groups!=0||
               total/groups is <5 or >500)return false;
            if(f==G5U4SimpleRate){expected=total/groups;return true;}
            if(!p.TryGetValue("target",out int target)||target is <3 or >15)return false;
            expected=checked(total/groups*target);return true;
        }
        if(f==G5U4SimpleArea)
        {
            if(p.Count!=3||!p.TryGetValue("length",out int length)||
               !p.TryGetValue("width",out int width)||!p.TryGetValue("side",out int side)||
               length is <6 or >20||width is <5 or >15||side is <2 or >4||
               side>=Math.Min(length,width))return false;
            expected=length*width-side*side;return expected>0;
        }
        if(p.Count!=3||!p.TryGetValue("income",out int income)||
           !p.TryGetValue("spend",out int spend)||!p.TryGetValue("target",out int goal)||
           income is <50 or >120||spend is <5 or >40||income-spend<10||
           goal is <180 or >850)return false;
        expected=goal/(income-spend)+1;
        return true;
    }
    private static bool VerifyG5U4Simple(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5U4Simple(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0'&&ch!='\u202f')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                        ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
        if(f==G5U4SimpleRate)return (long)v*p["groups"]==p["total"];
        if(f==G5U4SimpleScale)return (long)v*p["groups"]==(long)p["total"]*p["target"];
        if(f==G5U4SimpleArea)return v+p["side"]*p["side"]==p["length"]*p["width"];
        int net=p["income"]-p["spend"],goal=p["target"];
        return v>=1&&(long)(v-1)*net<=goal&&(long)v*net>goal;
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
            x=random.Next(10,201);y=random.Next(1,11);z=random.Next(2,13)*y;
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
        if(a is <10 or >200||b is <1 or >10||c<2*b||c>12*b||c%b!=0)return false;
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


    // Grade5 information diagnostics reuse the existing four-step exact engine.
    private static Item BuildG5Info(string family,Random random)
    {
        int a,b,c;string prompt;
        if(family==G5InfoDivide)
        {
            a=random.Next(5,31);b=random.Next(2,13);c=random.Next(1,16);
            prompt=$"لدى مدرسة {a*b} كتابًا، تعبئ كل صندوق بـ{b} كتب. ثمن الملصق {c} دراهم. كم صندوقًا تحتاج؟ تجاهل المعلومة الزائدة.";
        }
        else if(family==G5InfoCompare)
        {
            a=random.Next(50,150);b=random.Next(10,a);c=random.Next(1,15);
            prompt=$"في مخزن أ {a} علبة وفي مخزن ب {b} علبة. يفتح المخزنان الساعة {c}. كم علبة أكثر في المخزن أ؟";
        }
        else
        {
            a=random.Next(0,2);b=random.Next(2,31);c=random.Next(2,7);
            prompt=a==0
                ?$"يعطي معلم {c} أقلام لكل طالب دون ذكر عدد الطلاب. هل تكفي المعطيات لحساب إجمالي الأقلام؟ اكتب ١ للكافية أو ٠ للناقصة."
                :$"يعطي معلم {c} أقلام لكل طالب من {b} طلاب. هل تكفي المعطيات لحساب إجمالي الأقلام؟ اكتب ١ للكافية أو ٠ للناقصة.";
        }
        return new Item(family,prompt,"حدد المطلوب وما يلزمه، واستبعد الوصف الزائد ولا تخترع بيانات ناقصة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool ValidG5Info(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5Info(family)||p.Count!=3||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c))return false;
        switch(family)
        {
            case G5InfoDivide:
                if(a is <5 or >30||b is <2 or >12||c is <1 or >15)return false;
                expected=a;break;
            case G5InfoCompare:
                if(a is <50 or >149||b is <10 or >148||b>=a||c is <1 or >14)return false;
                expected=a-b;break;
            case G5InfoSufficient:
                if(a is <0 or >1||b is <2 or >30||c is <2 or >6)return false;
                expected=a;break;
            default:return false;
        }
        return true;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5U4Simple(family)) return ValidG5U4Simple(family,p,out expected);
        if(IsG5(family)) return ValidG5(family,p,out expected);
        if(IsG5Table(family)) return ValidG5Table(family,p,out expected);
        if(IsG5Info(family)) return ValidG5Info(family,p,out expected);
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
        if(IsG5U4Simple(family))return VerifyG5U4Simple(family,p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim()
            .Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                     c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,
            CultureInfo.InvariantCulture,out int v)&&v==expected;
    }
}

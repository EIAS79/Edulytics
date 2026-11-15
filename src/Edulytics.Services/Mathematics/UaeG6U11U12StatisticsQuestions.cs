using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Arabic G6 statistics and data visualisation family adapter.
/// Reuses ExactSkillContractQuestionEngine; graph drawing and reasoning remain teacher-reviewed.</summary>
internal static class UaeG6U11U12StatisticsQuestions
{
    internal const string Stat = "uae.g6.u11.l01.statistical_question";
    internal const string Mean = "uae.g6.u11.l02.mean";
    internal const string Missing = "uae.g6.u11.l02.missing_observation";
    internal const string Median = "uae.g6.u11.l03.median";
    internal const string Mode = "uae.g6.u11.l03.mode";
    internal const string Logic = "uae.g6.u11.l04.reasoning";
    internal const string Range = "uae.g6.u11.l05.range";
    internal const string Iqr = "uae.g6.u11.l05.iqr";
    internal const string Mad = "uae.g6.u11.l06.mean_absolute_deviation";
    internal const string Best = "uae.g6.u11.l07.choose_centre";
    internal const string Dots = "uae.g6.u12.l01.dot_plot_frequency";
    internal const string Histogram = "uae.g6.u12.l02.histogram_bin_frequency";
    internal const string Box = "uae.g6.u12.l03.box_plot_iqr";
    internal const string Graph = "uae.g6.u12.l04.compare_graph";
    internal const string Shape = "uae.g6.u12.l05.distribution_shape";
    internal const string Collect = "uae.g6.u12.l06.collection_total";
    internal const string Line = "uae.g6.u12.l07.line_graph_change";
    internal const string Choose = "uae.g6.u12.l08.choose_display";
    internal const string Unit = "uae.g6.u12.l09.choose_unit";
    internal const string Tool = "uae.g6.u12.l09.choose_tool";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    internal static bool Supports(string? f) => f is Stat or Mean or Missing or Median
        or Mode or Logic or Range or Iqr or Mad or Best or Dots or Histogram
        or Box or Graph or Shape or Collect or Line or Choose or Unit or Tool;

    private static string Category(string f,int n) => f switch
    {
        Stat => n==0 ? "نعم" : "لا",
        Logic => n==0 ? "نعم" : "لا",
        Best => n switch {0=>"الوسيط",1=>"المتوسط الحسابي",_=>"المنوال"},
        Shape => n switch {0=>"متماثل",1=>"منحرف إلى اليمين",_=>"منحرف إلى اليسار"},
        Choose => n switch {0=>"مخطط نقاط",1=>"مدرج تكراري",2=>"مخطط خطوط",3=>"مخطط أعمدة",_=>"مخطط الصندوق"},
        Unit => n switch {0=>"سم",1=>"كم",2=>"كجم",3=>"مل",_=>"م"},
        Tool => n switch {0=>"مسطرة",1=>"ميزان",2=>"كوب مدرج",_=>"شريط قياس"},
        _=>throw new ArgumentOutOfRangeException(nameof(f))
    };

    // Each classification family needs independently distinct authentic data
    // contexts. Vary the concrete mathematical scenario, not a cosmetic word
    // suffix, while preserving the first-party categorical answer semantics.
    private static string CategoryPrompt(string family, int mode, int variant)
    {
        int k=variant+1;
        string group=variant switch {
            0=>"الصف السادس أ",1=>"الصف السادس ب",2=>"نادي العلوم",
            3=>"فريق الرياضة",4=>"مكتبة المدرسة",_=>"النادي البيئي"
        };
        if(family==Stat)
            return mode==0
                ? $"إذا سألنا أفراد {group} عن عدد الدقائق التي يقرأون فيها يوميًا، هل نتوقع اختلاف الإجابات بما يجعل السؤال إحصائيًا؟"
                : $"هل سؤال: كم يومًا يتكون منه الأسبوع عند تنظيم جدول {group}؟ سؤال إحصائي تتغير إجابته بين الأفراد؟";
        if(family==Logic)
            return mode==0
                ? $"عندما تختلف أوقات وصول أفراد {group} إلى النشاط، هل يجوز أن تكون إجابات السؤال الإحصائي متنوعة؟"
                : $"عند قياس أطوال أفراد {group}، هل يجب أن تتساوى جميع القياسات كي يُعد سؤال الطول إحصائيًا؟";
        if(family==Best)
            return mode switch {
                0=>$"قيم تبرعات {group} هي {k}، {k+1}، {k+2}، {k+3}، {100+k}. ما مقياس المركز الأكثر مقاومة للقيمة الشاذة؟",
                1=>$"درجات نشاط {group} هي {10+k}، {12+k}، {14+k}، {16+k}، {18+k} دون قيم شاذة. ما مقياس المركز الذي يستخدم كل القيم؟",
                _=>$"سجّل {group} ألوان حقائب: أزرق، أحمر، أزرق، أصفر، أزرق. ما مقياس المركز المناسب للبيانات الاسمية؟"
            };
        if(family==Shape)
            return mode switch {
                0=>$"تكرارات القياسات المتتالية لدى {group} هي {k}، {k+2}، {k+4}، {k+2}، {k}. صنف تماثل الشكل.",
                1=>$"تكرارات الفئات المتصاعدة لدى {group} هي {10+k}، {5+k}، {2+k}، 1 مع ذيل ممتد إلى القيم الكبيرة. ما اتجاه الانحراف؟",
                _=>$"تكرارات الفئات المتصاعدة لدى {group} هي 1، {2+k}، {5+k}، {10+k} مع ذيل ممتد إلى القيم الصغيرة. ما اتجاه الانحراف؟"
            };
        if(family==Choose)
            return mode switch {
                0=>$"جمّع {group} قيم عدد الكتب المستعارة من 0 إلى {5+k}، ونريد نقاطًا منفردة فوق كل قيمة بعدد تكرارها. اختر نوع المخطط.",
                1=>$"قسّم {group} أطوال {20+k} نباتًا إلى فئات متصلة بالسنتيمتر لعرض التكرارات. اختر المخطط المناسب.",
                2=>$"رصد {group} درجة الحرارة يوميًا على مدى {6+k} أيام لبيان صعودها وهبوطها. اختر المخطط المناسب.",
                3=>$"عدّ {group} المشاركين في {3+k} مجموعات أسماء أنشطة مختلفة. ما نوع المخطط الذي يقارن هذه الفئات؟",
                _=>$"لدى {group} بيانات {12+k} قياسًا ونريد عرض الوسيط والربيعات والمدى. اختر المخطط المناسب."
            };
        if(family==Unit)
            return mode switch {
                0=>$"ما الوحدة المناسبة لقياس طول قلم من مجموعة أدوات {group}؟",
                1=>$"ما الوحدة المناسبة للمسافة بين موقع {group} ومدينة أخرى؟",
                2=>$"ما الوحدة المناسبة لكتلة حقيبة معدات {group} الثقيلة؟",
                3=>$"ما الوحدة المناسبة لسعة قارورة سائل صغيرة يستخدمها {group}؟",
                _=>$"ما الوحدة المناسبة لارتفاع باب غرفة {group}؟"
            };
        return mode switch {
            0=>$"أي أداة يستخدمها {group} لقياس طول قلم رصاص بالسنتيمتر؟",
            1=>$"أي أداة يستخدمها {group} لمعرفة كتلة حقيبة بالكيلوجرام؟",
            2=>$"أي أداة يستخدمها {group} لقياس حجم سائل بالمليلتر؟",
            _=>$"أي أداة يستخدمها {group} لقياس طول غرفة بأمتار؟"
        };
    }

    private static bool Expected(string? f,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer=string.Empty;
        if (!Supports(f) || p is null) return false;
        if (f is Stat or Logic or Best or Shape or Choose or Unit or Tool)
        {
            // Historical mode-only contracts remain independently verifiable,
            // while new varied prompts carry an explicit bounded scenario.
            if((p.Count!=1 && p.Count!=2)||
               !p.TryGetValue("mode",out int mode))return false;
            if(p.Count==2 && (!p.TryGetValue("scenario",out int v)||
                v is <0 or >5))return false;
            int max=f switch {Stat or Logic=>1,Best or Shape=>2,Tool=>3,_=>4};
            if (mode<0 || mode>max) return false;
            answer=Category(f,mode);return true;
        }
        int result;
        if (f is Mean or Median or Collect or Dots or Histogram)
        {
            if (p.Count!=3 || !p.TryGetValue("a",out int a)
                || !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)
                || a<0 || a>100 || b<0 || b>100 || c<0 || c>100) return false;
            if (f==Mean && (a+b+c)%3!=0) return false;
            result=f switch {
                Mean=>(a+b+c)/3,
                Median=>new[]{a,b,c}.OrderBy(x=>x).ElementAt(1),
                Dots or Histogram=>b,
                _=>a+b+c
            };
        }
        else if (f==Missing)
        {
            if (p.Count!=3||!p.TryGetValue("mean",out int mean)
                ||!p.TryGetValue("count",out int count)||!p.TryGetValue("partial",out int sum)
                ||mean is <1 or >50||count!=3||sum<0||sum>100) return false;
            result=mean*count-sum;
            if(result<0||result>100)return false;
        }
        else if(f==Mode)
        {
            if(p.Count!=2||!p.TryGetValue("x",out int x)||!p.TryGetValue("y",out int y)
                ||x<0||x>100||y<0||y>100||x==y)return false;
            result=x;
        }
        else if(f is Range or Iqr or Box or Line or Graph)
        {
            if(p.Count!=2||!p.TryGetValue("a",out int leftVal)||!p.TryGetValue("b",out int rightVal)
                ||leftVal is <-100 or >100||rightVal is <-100 or >100)return false;
            if((f is Range or Iqr or Box) && leftVal>rightVal)return false;
            result=f==Line?rightVal-leftVal:f==Graph?Math.Abs(rightVal-leftVal):rightVal-leftVal;
        }
        else if(f==Mad)
        {
            if(p.Count!=2||!p.TryGetValue("m",out int m)||!p.TryGetValue("d",out int d)
                ||m<0||m>100||d is <3 or >30||d%3!=0||d>m)return false;
            result=2*d/3;
        }
        else return false;
        answer=result.ToString(CultureInfo.InvariantCulture);
        return true;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unsupported G6 statistics family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string prompt;
        if(family is Stat or Logic or Best or Shape or Choose or Unit or Tool)
        {
            int limit=family switch {Stat or Logic=>2,Best or Shape=>3,Tool=>4,_=>5};
            int mode=random.Next(limit),scenario=random.Next(6);
            p=new(){{"mode",mode},{"scenario",scenario}};
            prompt=CategoryPrompt(family,mode,scenario);
        }
        else if(family is Mean or Median or Collect or Dots or Histogram)
        {
            int a=random.Next(1,13),b=random.Next(1,13),c=random.Next(1,13);
            if(family==Mean)c=3*random.Next(5,12)-a-b;
            if(c<0 || c>100)c=3*10-a-b;
            p=new(){{"a",a},{"b",b},{"c",c}};
            prompt=family switch{
                Mean=>$"احسب المتوسط الحسابي للقيم {a} و{b} و{c}.",
                Median=>$"رتّب القيم {a} و{b} و{c} ثم أوجد الوسيط.",
                Collect=>$"جمع طلاب ثلاثة صفوف {a} و{b} و{c} استجابة. ما مجموع الاستجابات؟",
                Dots=>$"في مخطط نقاط التكرارات عند القيم 1 و2 و3 هي {a} و{b} و{c}. ما تكرار القيمة 2؟",
                _=>$"في مدرج تكراري فئاتها 0-9، 10-19، 20-29 وتكراراتها {a} و{b} و{c}. ما تكرار الفئة الوسطى؟"};
        }
        else if(family==Missing)
        {
            int mean=random.Next(7,25),a=random.Next(2,mean),b=random.Next(2,mean);
            p=new(){{"mean",mean},{"count",3},{"partial",a+b}};
            prompt=$"متوسط ثلاثة أعداد هو {mean}. عددان منهما {a} و{b}. أوجد العدد الثالث.";
        }
        else if(family==Mode)
        {
            int x=random.Next(2,30),y=x+random.Next(1,10);
            p=new(){{"x",x},{"y",y}};
            prompt=$"في البيانات {x},{y},{x},{y},{x}، أوجد المنوال.";
        }
        else if(family==Mad)
        {
            int m=random.Next(14,35),d=3*random.Next(1,4);
            p=new(){{"m",m},{"d",d}};
            prompt=$"للقيم {m-d} و{m} و{m+d} متوسط حسابي {m}. أوجد متوسط الانحراف المطلق.";
        }
        else
        {
            int a=random.Next(1,20),b=a+random.Next(1,20);
            if(family==Line && random.Next(2)==0)b=a-random.Next(1,12);
            p=new(){{"a",a},{"b",b}};
            prompt=family switch{
                Range=>$"أصغر قيمة في بيانات هي {a} وأكبرها {b}. أوجد المدى.",
                Iqr=>$"الربيع الأدنى {a} والربيع الأعلى {b}. أوجد المدى الربيعي.",
                Box=>$"في مخطط صندوق الربيع الأدنى {a} والأعلى {b}. ما طول الصندوق؟",
                Graph=>$"يُظهر رسمان قيمتي تكرار {a} و{b}. ما مقدار الفرق المطلق بينهما؟",
                _=>$"تغيرت قيمة على الرسم الخطي من {a} إلى {b}. أوجد التغير الموقع (النهاية ناقص البداية)."};
        }
        if(!Expected(family,p,out string result))throw new InvalidOperationException("Invalid generated statistics question");
        return new Item(family,prompt,result,AssessmentItemType.ShortAnswer,p);
    }

    private static string Normalize(string value)=>new string(value.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch{
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '−' or '–'=>'-',_=>c}).ToArray());

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p) =>
        Expected(family,p,out string value)?value:throw new ArgumentException("Invalid stats parameters");

    internal static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)
    {
        if(string.IsNullOrWhiteSpace(answer)||answer.Length>90||!Expected(family,p,out var result))return false;
        var normalized=Normalize(answer);
        if(family is Stat or Logic or Best or Shape or Choose or Unit or Tool)
            return string.Equals(normalized,Normalize(result),StringComparison.Ordinal);
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int got)
            && int.TryParse(result,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int expected)
            && got==expected;
    }
}

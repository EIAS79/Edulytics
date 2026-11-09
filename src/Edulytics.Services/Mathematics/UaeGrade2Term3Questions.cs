using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Grade2 Units 09–12 first-party source-scoped question mechanics. Stage D NOT RUN.
internal static class UaeGrade2Term3Questions
{
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int T(string? f)=>f switch
    {
        "uae.g2.u09.l01.time_whole_hour" => 1,
        "uae.g2.u09.l02.time_half_hour" => 2,
        "uae.g2.u09.l03.time_pattern" => 3,
        "uae.g2.u09.l04.time_quarter_hour" => 4,
        "uae.g2.u09.l05.time_five_minutes" => 5,
        "uae.g2.u09.l06.am_pm" => 6,
        "uae.g2.u09.l07.time_relationships" => 7,
        "uae.g2.u09.l08.calendar_weekdays" => 8,
        "uae.g2.u09.l09.time_units" => 9,
        "uae.g2.u10.l01.two_d_shape" => 10,
        "uae.g2.u10.l02.sides_and_vertices" => 11,
        "uae.g2.u10.l03.sketch_shape_counts" => 12,
        "uae.g2.u10.l04.three_d_classify" => 13,
        "uae.g2.u10.l05.faces_edges_vertices" => 14,
        "uae.g2.u10.l06.connect_solids_to_faces" => 15,
        "uae.g2.u10.l07.equal_parts_fraction" => 16,
        "uae.g2.u10.l08.area_unit_squares" => 17,
        "uae.g2.u11.l01.ruler_cm_m" => 18,
        "uae.g2.u11.l02.select_metric_unit" => 19,
        "uae.g2.u11.l03.compare_lengths" => 20,
        "uae.g2.u11.l04.centimeters_meters" => 21,
        "uae.g2.u11.l05.number_line_measure" => 22,
        "uae.g2.u11.l06.measurement_data" => 23,
        "uae.g2.u11.l07.liquid_volume" => 24,
        "uae.g2.u11.l08.metric_liquid_volume" => 25,
        "uae.g2.u12.l01.coin_1_5_10_fils" => 26,
        "uae.g2.u12.l02.coin_25_fils" => 27,
        "uae.g2.u12.l03.count_coin_mix" => 28,
        "uae.g2.u12.l04.act_out_money" => 29,
        "uae.g2.u12.l05.dirham_fils" => 30,
        _=>0
    };
    public static bool Supports(string? f)=>T(f)>0;
    private static int Compute(int t,int a,int b,int c)=>t switch
    {
        1=>a, 2=>30, 3=>b*c, 4=>b, 5=>b, 6=>b,
        7=>a+b, 8=>(a+b-1)%7+1, 9=>b*(a==1?7:a==2?24:60),
        10=>a+2,11=>a+2,12=>a+b,13=>a,
        14=>b==1?6:b==2?12:8,15=>a==3?2:6,
        16=>a,17=>a*b,
        18=>b-a,19=>a==1?1:2,20=>a-b,21=>a*100,
        22=>b-a,23=>a+b+c,24=>a*b,25=>4*a,
        26=>a+5*b+10*c,27=>25*a+5*b+c,
        28=>25*a+10*b+5*c,29=>a-b-c,30=>a*100+b,
        _=>throw new InvalidOperationException("Unsupported Grade2 term3 mechanic")
    };
    private static bool Allowed(int t,int a,int b,int c)=>t switch
    {
        1=>a is >=1 and <=12&&b==0&&c==0,
        2=>a is >=1 and <=12&&b==30&&c==0,
        3=>a is >=1 and <=5&&b is 15 or 30&&c is >=2 and <=4,
        4=>a is >=1 and <=12&&b is 15 or 45&&c==0,
        5=>a is >=1 and <=12&&b is >=5 and <=55&&b%5==0&&c==0,
        6=>a is >=1 and <=12&&b is 1 or 2&&c==0,
        7=>a is >=1 and <=8&&b is >=1 and <=3&&c==0,
        8=>a is >=1 and <=7&&b is >=1 and <=6&&c==0,
        9=>a is >=1 and <=3&&b is >=1 and <=4&&c==0,
        10 or 11=>a is >=1 and <=4&&b==0&&c==0,
        12=>a is >=1 and <=4&&b is >=1 and <=4&&c==0,
        13=>a is >=1 and <=4&&b==0&&c==0,
        14=>a is 1 or 2&&b is >=1 and <=3&&c==0,
        15=>a is >=1 and <=3&&b==0&&c==0,
        16=>a is >=2 and <=4&&b>=1&&b<=a&&c==0,
        17=>a is >=2 and <=8&&b is >=2 and <=8&&c==0,
        18=>a is >=1 and <=6&&b>a&&b<=18&&c==0,
        19=>a is >=1 and <=3&&b==0&&c==0,
        20=>a is >=10 and <=30&&b is >=1 and <=9&&c==0,
        21=>a is >=1 and <=9&&b==0&&c==0,
        22=>a is >=0 and <=12&&b>a&&b<=20&&c==0,
        23=>a is >=1 and <=6&&b is >=1 and <=6&&c is >=1 and <=6,
        24=>a is >=1 and <=6&&b is >=1 and <=4&&c==0,
        25=>a is >=1 and <=5&&b==4&&c==0,
        26=>a is >=0 and <=10&&b is >=0 and <=10&&c is >=0 and <=7,
        27=>a is >=1 and <=3&&b is >=0 and <=5&&c is >=0 and <=5,
        28=>a is >=0 and <=3&&b is >=0 and <=6&&c is >=0 and <=6,
        29=>a is 75 or 100&&b is >=5 and <=40&&b%5==0&&
            c is >=5 and <=15&&c%5==0&&a>b+c,
        30=>a is >=1 and <=8&&b is 0 or 25 or 50 or 75&&c==0,
        _=>false
    };
    public static Item Build(string family,Random r)
    {
        var t=T(family);
        if(t==0)throw new InvalidOperationException("Unknown UAE Grade2 Units 9–12 family");
        int a=0,b=0,c=0;string prompt;
        switch(t)
        {
            case 1:
                a=r.Next(1,13);
                prompt=$"في ساعة ذات عقارب يشير عقرب الدقائق إلى12، والقصير إلى{a}. ما رقم الساعة؟";break;
            case 2:
                a=r.Next(1,13);b=30;
                prompt=$"الوقت {a}:30، أي {a} والنصف. كم دقيقة مرت بعد تمام الساعة؟";break;
            case 3:
                a=r.Next(1,6);b=r.Next(2)==0?15:30;c=r.Next(2,5);
                prompt=$"بدءًا من الساعة {a}:00 يحدث نشاط كل {b} دقيقة، وتكرر الفاصل {c} مرات. كم دقيقة انقضت إجمالًا؟";break;
            case 4:
                a=r.Next(1,13);b=r.Next(2)==0?15:45;
                prompt=$"الوقت {a}:{b:00}. كم دقيقة مرت بعد تمام الساعة؟";break;
            case 5:
                a=r.Next(1,13);b=r.Next(1,12)*5;
                prompt=$"الوقت {a}:{b:00}. عقرب الدقائق ينتقل5 دقائق لكل رقم. كم دقيقة مرت بعد الساعة؟";break;
            case 6:
                a=r.Next(1,13);b=r.Next(1,3);
                prompt=$"موعد النشاط الساعة {a}:00 {(b==1?"صباحًا":"مساءً")}. اكتب1 للصباح و2 للمساء.";break;
            case 7:
                a=r.Next(1,9);b=r.Next(1,4);
                prompt=$"بدأ التمرين الساعة {a}:00 واستمر {b} ساعة. في أي ساعة كاملة انتهى (اكتب رقم الساعة)؟";break;
            case 8:
                a=r.Next(1,8);b=r.Next(1,7);
                prompt=$"رقم اليوم في التقويم {a} (الأحد1، الاثنين2، …، السبت7). ما رقم اليوم بعد {b} أيام؟";break;
            case 9:
                a=r.Next(1,4);b=r.Next(1,5);
                prompt=a==1?$"كم يومًا في {b} أسابيع؟":a==2?$"كم ساعة في {b} أيام؟":$"كم دقيقة في {b} ساعات؟";break;
            case 10:
                a=r.Next(1,5);
                prompt=$"الشكل المستوي رقم {a}: {(a==1?"مثلث":a==2?"مربع":a==3?"خماسي":"سداسي")}. كم ضلعًا مستقيمًا له؟";break;
            case 11:
                a=r.Next(1,5);
                prompt=$"مضلع له {a+2} أضلاع. كم زاوية له إذا كان بسيطًا؟";break;
            case 12:
                a=r.Next(1,5);b=r.Next(1,5);
                prompt=$"تصميم رسم تخطيطي لبيت من {a} مستطيلات و{b} مثلثات منفصلة. كم شكلًا مستويًا استخدمت؟";break;
            case 13:
                a=r.Next(1,5);
                prompt=$"حدد رمز المجسم: {(a==1?"له6 أوجه مربعة":a==2?"له رأس واحد وقاعدة دائرية":a==3?"له سطح مستدير كالكرة": "له قاعدتان دائريتان")}. اكتب1 للمكعب،2 للمخروط،3 للكرة،4 للأسطوانة.";break;
            case 14:
                a=r.Next(1,3);b=r.Next(1,4);
                prompt=$"في {(a==1?"المكعب":"متوازي المستطيلات")}، كم {(b==1?"وجهًا":b==2?"حافة":"رأسًا")}؟";break;
            case 15:
                a=r.Next(1,4);
                prompt=$"في {(a==1?"مكعب":a==2?"متوازي مستطيلات":"منشور ثلاثي")}، كم {(a==3?"وجهًا مثلثيًا":"وجهًا مسطحًا")}؟";break;
            case 16:
                a=r.Next(2,5);b=r.Next(1,a+1);
                prompt=$"قسم مستطيل بالتساوي إلى {a} أجزاء ولوّن {b} منها. كم جزءًا متساويًا يمثل المقام في هذا الكسر؟";break;
            case 17:
                a=r.Next(2,9);b=r.Next(2,9);
                prompt=$"مستطيل مرسوم على شبكة فيها {a} صفوف و{b} أعمدة من مربعات الوحدة. ما مساحته بالمربعات؟";break;
            case 18:
                a=r.Next(1,7);b=r.Next(a+1,19);
                prompt=$"قطعة على المسطرة تبدأ عند {a} سم وتنتهي عند {b} سم. كم طولها بالسنتيمتر؟";break;
            case 19:
                a=r.Next(1,4);
                prompt=$"اختر وحدة مناسبة لقياس {(a==1?"قلم صغير":a==2?"طول غرفة":"ارتفاع باب")}. اكتب1 للسنتيمتر و2 للمتر.";break;
            case 20:
                a=r.Next(10,31);b=r.Next(1,10);
                prompt=$"شريط أ طوله {a} سم وشريط ب طوله {a-b} سم. ما الفرق بين طولي الشريطين؟";break;
            case 21:
                a=r.Next(1,10);
                prompt=$"حوّل {a} أمتار إلى سنتيمترات مع العلم أن1 متر=100 سم.";break;
            case 22:
                a=r.Next(0,13);b=r.Next(a+1,21);
                prompt=$"يشغل جسم خط الأعداد من {a} سم إلى {b} سم. كم طوله بالسنتيمتر؟";break;
            case 23:
                a=r.Next(1,7);b=r.Next(1,7);c=r.Next(1,7);
                prompt=$"بيانات أطوال: 8سم تكرر {a} مرات،10سم تكرر {b} مرات،13سم تكرر {c} مرات. كم قياسًا جُمِع؟";break;
            case 24:
                a=r.Next(1,7);b=r.Next(1,5);
                prompt=$"هناك {a} أوعية، يسع كل واحد منها {b} أكواب ماء. كم كوبًا تتسع لها الأوعية كلها؟";break;
            case 25:
                a=r.Next(1,6);b=4;
                prompt=$"وفق مفتاح النشاط التقريبي، كل لتر يملأ4 أكواب متساوية. كم كوبًا تقريبا تملأ {a} لترات؟";break;
            case 26:
                a=r.Next(0,11);b=r.Next(0,11);c=r.Next(0,8);
                prompt=$"كم فلسًا مجموع {a} عملات من فئة1 فلس و{b} عملات5 فلوس و{c} عملات10 فلوس؟";break;
            case 27:
                a=r.Next(1,4);b=r.Next(0,6);c=r.Next(0,6);
                prompt=$"لدي {a} عملات25 فلسًا، و{b} عملات5 فلوس، و{c} عملات1 فلس. كم مجموع الفلوس؟";break;
            case 28:
                a=r.Next(0,4);b=r.Next(0,7);c=r.Next(0,7);
                prompt=$"عدّ العملات: {a} من25 فلسًا، و{b} من10 فلوس، و{c} من5 فلوس. ما القيمة كلها بالفلوس؟";break;
            case 29:
                a=r.Next(2)==0?75:100;b=r.Next(1,9)*5;c=r.Next(1,4)*5;
                prompt=$"معك {a} فلسًا. اشتريت كتابًا بمبلغ {b} فلسًا ثم دفعت {c} فلسًا لأداة. كم يبقى؟ مثل القطع على ورق.";break;
            case 30:
                a=r.Next(1,9);b=r.Next(0,4)*25;
                prompt=$"حوّل {a} دراهم و{b} فلسًا إلى الفلوس. الدرهم يساوي100 فلس.";break;
            default:throw new InvalidOperationException("Unsupported family");
        }
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"result",Compute(t,a,b,c)}};
        return new Item(family,prompt,
            "أطبق النموذج الحسابي المناسب للدرس، وأتحقق من الوحدة والقيم والمعطيات.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p)
    {
        var t=T(f);
        return t>0&&p.Count==4&&p.TryGetValue("a",out var a)&&
            p.TryGetValue("b",out var b)&&p.TryGetValue("c",out var c)&&
            p.TryGetValue("result",out var v)&&Allowed(t,a,b,c)&&Compute(t,a,b,c)==v;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p))throw new InvalidOperationException("Grade2 term3 invalid exact question contract");
        return p["result"].ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p)||string.IsNullOrWhiteSpace(answer))return false;
        var normalized=new string(answer.Trim().Select(c=>
            c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)
            &&n==p["result"];
    }
}

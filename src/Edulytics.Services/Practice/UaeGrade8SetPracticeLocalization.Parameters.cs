using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>
/// Deterministic, parameter-specific Arabic rendering of finite existing
/// shared-grade mathematical scenarios. The exact generator and verifier remain
/// untouched; only learner-facing prose is localized to the Grade8 context.
/// </summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? Grade8ParameterizedSharedPracticePrompt(ExactSkillGeneratedQuestion q)
    {
        if(!q.Parameters.TryGetValue("kind",out var kind) ||
           kind is <0 or >4)
            return null;
        var hasFeature=q.Parameters.TryGetValue("feature",out var feature) &&
                       feature is >=0 and <=3;
        return q.Family switch
        {
            "usccss.g7.u02.l12.compare_same_axes_rates" when q.Parameters.Count==2 && hasFeature =>
                CompareRates(kind,feature),
            "usccss.g7.u01.l10.change_scale_length_and_area" when
                q.Parameters.Count==3 && hasFeature &&
                q.Parameters.TryGetValue("style",out var style) &&
                style is >=0 and <=3 => ChangingScale(kind,feature,style),
            "usccss.g7.u01.l06.scaled_area_square_factor" when
                q.Parameters.Count==4 &&
                q.Parameters.TryGetValue("units",out var units) &&
                units is >=1 and <=4 &&
                q.Parameters.TryGetValue("factor",out var factor) &&
                factor is >=0 and <=6 &&
                q.Parameters.TryGetValue("asked",out var asked) && asked is 0 or 1 =>
                AreaScaling(kind,units,factor,asked),
            _ => null
        };
    }

    private static string CompareRates(int kind,int feature)
    {
        string[] proportionalConstants =
            ["A=5، B=3","A=4/3، B=3/2","A=7/4، B=5/4","A=2.5، B=2.25"];
        string[] pointPairs =
            ["عند x=2: قيمة y في A تساوي 6، وفي B تساوي 4",
             "عند x=4: قيمة y في A تساوي 5، وفي B تساوي 7",
             "عند x=3: قيمة y في A تساوي 9، وفي B تساوي 6",
             "عند x=5: قيمة y في A تساوي 10، وفي B تساوي 12.5"];
        string[] speedPairs =
            ["A: مسافة 50 مترًا في 40 ثانية؛ B: مسافة 50 مترًا في 30 ثانية",
             "A: مسافة 24 مترًا في 12 ثانية؛ B: مسافة 30 مترًا في 10 ثوانٍ",
             "A: مسافة 18 مترًا في 9 ثوانٍ؛ B: مسافة 14 مترًا في 7 ثوانٍ",
             "A: مسافة 7.5 متر في 3 ثوانٍ؛ B: مسافة 9 أمتار في 4.5 ثوانٍ"];
        string[] comparisons =
            ["عند قيمة x موجبة واحدة يقع A فوق B",
             "عند قيمة x موجبة واحدة يقع B فوق A",
             "على امتداد أفقي واحد يرتفع A أكثر من B",
             "على امتداد أفقي واحد يرتفع B أكثر من A"];
        string[] clockPlots =
            ["من (0,0) مرورًا بالنقطة (40,50)",
             "يبدأ متأخرًا من (10,0) وينتهي عند (40,50)",
             "من (0,0) مرورًا بالنقطة (20,50)",
             "من (0,5) مرورًا بالنقطة (20,50)"];
        return kind switch
        {
            0 => $"مستقيمان تناسبيان لهما المحاور والمقاييس نفسها وثابتا التناسب {proportionalConstants[feature]}. اكتب أكبر ثابت تناسب.",
            1 => $"مستقيمان تناسبيان يمران بالأصل ولهما المحاور نفسها. {pointPairs[feature]}. اكتب 1 إذا كان A أشد ميلًا، أو 2 إذا كان B أشد ميلًا.",
            2 => $"نموذجا حركة بسرعة ثابتة على المحاور نفسها: {speedPairs[feature]}. اكتب السرعة الأكبر بالمتر لكل ثانية.",
            3 => $"مستقيمان تناسبيان موجبان على المحاور والمقاييس نفسها. {comparisons[feature]}. اكتب 1 إذا كان ثابت التناسب في A أكبر، أو 2 إذا كان في B أكبر.",
            _ => $"يُقاس الزمن من ساعة مشتركة. رسم المسافة مقابل الزمن {clockPlots[feature]}. هل المسافة تتناسب طرديًا مع الزمن المنقضي من بدء الساعة؟ اكتب 1 لنعم و0 للا."
        };
    }

    private static string ChangingScale(int kind,int feature,int style)
    {
        int[] scales=[5,15,50,200];
        string[] precision =
            ["9 وربع بوصة","9 و5/64 بوصة","23.5 سنتيمترًا","23.47659 سنتيمترًا"];
        return kind switch
        {
            0 => $"حديقة مستطيلة رسمها 10 سم × 4 سم، وكل 1 سم في الرسم يمثل 90 مترًا في الواقع. أُعيد رسم الحديقة نفسها بمقياس 1 سم لكل 30 مترًا. " +
                 (feature switch {0=>"أوجد عرض الرسم الجديد بالسنتيمتر.",1=>"أوجد ارتفاع الرسم الجديد بالسنتيمتر.",
                  2=>"بأي معامل زادت مساحة الرسم؟",_=>"ما المساحة الحقيقية بالأمتار المربعة التي يقابلها 1 سم² في الرسم الجديد؟"}),
            1 => $"أرض مثلثة طول قاعدتها الحقيقي 120 مترًا وارتفاعها 90 مترًا. في رسمها يمثل كل 1 سم {scales[style]} مترًا. " +
                 (feature switch {0=>"ما طول القاعدة في الرسم بالسنتيمتر؟",1=>"ما الارتفاع العمودي في الرسم بالسنتيمتر؟",
                  2=>"ما مساحة المثلث المرسوم بالسنتيمتر المربع؟",_=>"ما المساحة الحقيقية بالأمتار المربعة التي يمثلها 1 سم² في الرسم؟"}),
            2 => "رسمان لملعب واحد: في الأول كل 1 سم يمثل 30 مترًا، وفي الثاني كل 1 سم يمثل 20 مترًا. " +
                 (feature switch {0=>"ما معامل زيادة الأطوال في الرسم الجديد؟",1=>"ما معامل زيادة المساحة في الرسم الجديد؟",
                  2=>"ما المساحة الحقيقية بالمتر المربع التي يمثلها 1 سم² في الرسم الأول؟",
                  _=>"ما المساحة الحقيقية بالمتر المربع التي يمثلها 1 سم² في الرسم الثاني؟"}),
            3 => "مخطط أرضية مطعم، كل بوصة واحدة في الرسم تمثل 12 قدمًا حقيقيًا، ومساحة الرسم 60 بوصة مربعة. " +
                 (feature switch {0=>"ما المساحة الحقيقية بالأقدام المربعة؟",
                  1=>"ما المساحة الحقيقية بالأقدام المربعة التي تقابل بوصة مربعة واحدة من الرسم؟",
                  2=>"يدّعي شخص أن المساحة الحقيقية 720 قدمًا مربعة؛ اكتب 1 إن كان ذلك صحيحًا، أو 0 إن كان خاطئًا.",
                  _=>"ما القيمة العددية الخاطئة للمساحة لو ضربنا مساحة الرسم 60 في معامل الأطوال 12 بدل مربعه؟"}),
            _ => $"مسطرة لا تميز إلا عُشر سنتيمتر أو ربع بوصة. هل القياس «{precision[feature]}» مبرر بهذه الدقة؟ اكتب 1 إذا كان مبررًا أو 0 إن كانت دقة زائفة."
        };
    }

    private static string AreaScaling(int kind,int units,int factor,int asked)
    {
        string[] factors=["1","2","3","4","1/2","1/3","3/5"];
        string shape=kind switch
        {
            0 => $"شكل من {units} قطع مساحة كل واحدة وحدة مربعة",
            1 => "مثلث مساحته 6 وحدات مربعة",
            _ => "متوازي أضلاع مساحته 8 وحدات مربعة"
        };
        return $"لدينا {shape}. صُنع شكل مشابه بمعامل أطوال k={factors[factor]}. " +
               (asked==0
                 ?"ما مساحة الشكل الجديد بوحدات مربعة؟ اكتب عددًا صحيحًا أو كسرًا مبسطًا دقيقًا."
                 :"ما معامل تغير المساحة، وليس الأطوال؟ اكتب عددًا أو كسرًا مبسطًا دقيقًا.");
    }
}

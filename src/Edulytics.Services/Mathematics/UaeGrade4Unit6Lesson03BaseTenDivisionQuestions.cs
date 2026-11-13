using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson03BaseTenDivisionQuestions
{
    internal const string Quotient="uae.g4.u06.l03.base_ten_quotient";
    internal const string Remainder="uae.g4.u06.l03.base_ten_remainder";
    internal const string G5U4AdjustQuotient="uae.g5.u04.l04.corrected_trial_quotient";
    internal const string G5U4AdjustHigh="uae.g5.u04.l04.trial_too_high_indicator";
    internal const string G5U4AdjustRemainder="uae.g5.u04.l04.adjusted_remainder";
    private static bool IsG5U4Adjust(string? f)=>f is G5U4AdjustQuotient or G5U4AdjustHigh or G5U4AdjustRemainder;
    internal const string G5U4LongQuotient="uae.g5.u04.l03.two_digit_long_quotient";
    internal const string G5U4LongRemainder="uae.g5.u04.l03.two_digit_long_remainder";
    private static bool IsG5U4Long(string? f)=>f is G5U4LongQuotient or G5U4LongRemainder;
    internal const string G5U4BlockQuotient="uae.g5.u04.l02.base_ten_group_quotient";
    internal const string G5U4BlockRemainder="uae.g5.u04.l02.base_ten_group_remainder";
    private static bool IsG5U4Block(string? f)=>f is G5U4BlockQuotient or G5U4BlockRemainder;
    internal const string G5Quotient="uae.g5.u03.l03.two_digit_quotient";
    internal const string G5Remainder="uae.g5.u03.l03.two_digit_remainder";
    internal const string G5Distributed="uae.g5.u03.l07.distributed_quotient";
    internal const string G5PartialSum="uae.g5.u03.l07.sum_partial_quotients";
    internal const string G5PartialTens="uae.g5.u03.l07.tens_partial_quotient";
    internal const string G5LongQuotient="uae.g5.u03.l08.long_division_quotient";
    internal const string G5LongRemainder="uae.g5.u03.l08.long_division_remainder";
    internal const string G5FirstPlace="uae.g5.u03.l09.first_digit_place";
    internal const string G5FirstDigit="uae.g5.u03.l09.first_quotient_digit";
    internal const string G5ZeroQuotient="uae.g5.u03.l10.middle_zero_quotient";
    internal const string G5ZeroRemainder="uae.g5.u03.l10.middle_zero_remainder";
    internal const string G5ModelGroup="uae.g5.u03.l11.equal_group_count";
    internal const string G5ModelLeftover="uae.g5.u03.l11.model_leftover_count";
    internal const string G5RemainderCeil="uae.g5.u03.l12.minimum_groups_ceil";
    internal const string G5RemainderFloor="uae.g5.u03.l12.full_groups_floor";
    internal const string G5RemainderLeftover="uae.g5.u03.l12.leftover_amount";
    private static bool IsG5ContextRemainder(string? f)=>f is G5RemainderCeil or G5RemainderFloor or G5RemainderLeftover;
    private static bool IsG5Model(string? f)=>f is G5ModelGroup or G5ModelLeftover;
    private static bool IsG5Zero(string? f)=>f is G5ZeroQuotient or G5ZeroRemainder;
    private static bool IsG5First(string? f)=>f is G5FirstPlace or G5FirstDigit;
    private static bool IsG5Long(string? f)=>f is G5LongQuotient or G5LongRemainder;
    private static bool IsG5Partial(string? f)=>f is G5Distributed or G5PartialSum or G5PartialTens;
    private static bool IsG5(string? f)=>f is G5Quotient or G5Remainder;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder || IsG5(f) || IsG5Partial(f) || IsG5Long(f) || IsG5First(f) || IsG5Zero(f) || IsG5Model(f) || IsG5ContextRemainder(f) || IsG5U4Block(f) || IsG5U4Long(f) || IsG5U4Adjust(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown base-ten division family");
        if(IsG5U4Adjust(f))return BuildG5U4Adjust(f,r);
        if(IsG5U4Long(f))return BuildG5U4Long(f,r);
        if(IsG5U4Block(f))return BuildG5U4Block(f,r);
        if(IsG5(f))return BuildG5(f,r);
        if(IsG5Partial(f))return BuildG5Partial(f,r);
        if(IsG5Long(f))return BuildG5Long(f,r);
        if(IsG5First(f))return BuildG5First(f,r);
        if(IsG5Zero(f))return BuildG5Zero(f,r);
        if(IsG5Model(f))return BuildG5Model(f,r);
        if(IsG5ContextRemainder(f))return BuildG5ContextRemainder(f,r);
        int dividend=r.Next(20,10000),divisor=r.Next(2,10);
        string prompt=f==Quotient
          ?$"مثّل {dividend} ÷ {divisor} بمكعبات العشرات والآحاد ثم اكتب ناتج القسمة الصحيح دون الباقي."
          :$"قسّم {dividend} إلى {divisor} مجموعات متساوية باستخدام مكعبات القيمة المكانية. كم آحادًا تتبقى؟";
        return new Item(f,prompt,"أوزع العشرات، وأعيد تجميع العشرات المتبقية إلى آحاد، ثم أوزع الآحاد بالتساوي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }




    // G5 Unit 4 L04: retain the G4 exact long-division solver architecture.
    // Trial is a proposed quotient, NOT an answer field.
    private static Item BuildG5U4Adjust(string f,Random rng)
    {
        int divisor=rng.Next(11,91),quotient=rng.Next(3,56),remainder=rng.Next(0,divisor);
        int n=checked(divisor*quotient+remainder),trial=quotient+(rng.Next(2)==0?-1:1);
        string question=f switch
        {
            G5U4AdjustQuotient=>$"في قسمة {n} على {divisor} جرّب أحد الطلاب ناتجًا قدره {trial}. تحقق بضرب {divisor}×{trial} ثم اكتب الحاصل الصحيح.",
            G5U4AdjustHigh=>$"في قسمة {n}÷{divisor} جُرّب الحاصل {trial}. اكتب ١ إذا كان مرتفعًا جدًا (حاصل الضرب يتجاوز المقسوم)، وإلا اكتب ٠.",
            _=>$"بعد تصحيح تقدير {trial} لحاصل {n}÷{divisor}، ما الباقي الصحيح بعد القسمة؟"
        };
        return new Item(f,question,"أضرب المقسوم عليه في الحاصل المؤقت، وأرفعه أو أخفضه حتى يصبح الفرق غير سالب وأصغر من المقسوم عليه.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",divisor},{"trial",trial}});
    }
    private static bool ValidG5U4Adjust(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5U4Adjust(f)||p.Count!=3||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||!p.TryGetValue("trial",out int trial)||
           d is <11 or >90||n is <33 or >5039||
           n/d is <3 or >55||Math.Abs(trial-n/d)!=1)return false;
        expected=f switch
        {
            G5U4AdjustQuotient=>n/d,
            G5U4AdjustHigh=>trial*d>n?1:0,
            _=>n%d
        };
        return true;
    }
    private static bool VerifyG5U4Adjust(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5U4Adjust(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0'&&ch!='\u202f')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                        ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
        int n=p["dividend"],d=p["divisor"],trial=p["trial"];
        return f switch
        {
            G5U4AdjustQuotient=>v>=0&&(long)v*d<=n&&n-(long)v*d<d,
            G5U4AdjustHigh=>v is 0 or 1 && (v==1)==((long)trial*d>n),
            _=>v>=0&&v<d&&(n-v)%d==0
        };
    }

    // G5 U04 L03 shares the G4/U3 exact long-division engine; new source range
    // uses two-digit divisors and 3–4 digit dividends, with quotient and remainder.
    private static Item BuildG5U4Long(string f,Random rng)
    {
        int divisor=rng.Next(11,99),quotient=rng.Next(3,53),remainder=rng.Next(0,divisor);
        int dividend=checked(divisor*quotient+remainder);
        string prompt=f==G5U4LongQuotient
            ?$"اقسم {dividend} على {divisor} بالقسمة المطولة؛ اكتب ناتج القسمة الصحيح دون الباقي."
            :$"اقسم {dividend} على {divisor} باستخدام التقدير والضرب والطرح؛ ما الباقي بعد توزيع المجموعات الكاملة؟";
        return new Item(f,prompt,"قدّر، ثم اقسم واضرب واطرح وأنزل الرقم التالي. تحقق بأن الباقي أصغر من المقسوم عليه.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool ValidG5U4Long(string f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!IsG5U4Long(f)||p.Count!=2||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||d is <11 or >98||
           n is <33 or >5194||n/d is <3 or >52)return false;
        value=f==G5U4LongQuotient?n/d:n%d;
        return true;
    }
    private static bool VerifyG5U4Long(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5U4Long(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=new(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0'&&ch!='\u202f')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                        ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        if(!int.TryParse(raw,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
        int n=p["dividend"],d=p["divisor"];
        return f==G5U4LongQuotient
            ?v>=0&&(long)v*d<=n&&n-(long)v*d<d
            :v>=0&&v<d&&(n-v)%d==0;
    }

    // G5 U04 L02: extend the same base-ten regrouping mechanism to a
    // two-digit group count. No change to previously registered families.
    private static Item BuildG5U4Block(string family,Random random)
    {
        int divisor=random.Next(11,23),quotient=random.Next(9,19),remainder=random.Next(0,divisor);
        int dividend=checked(divisor*quotient+remainder);
        string prompt=family==G5U4BlockQuotient
            ?$"مثّل {dividend} بمكعبات القيمة المكانية ثم أعد تجميع المئات والعشرات ووزّعها على {divisor} مجموعة متساوية. كم قطعة في كل مجموعة كاملة؟"
            :$"في توزيع {dividend} مكعبًا إلى {divisor} مجموعة متساوية باستخدام المئات والعشرات والآحاد، كم مكعبًا يبقى بعد توزيع المجموعات الكاملة؟";
        return new Item(family,prompt,
            "أحوّل مئة إلى عشر عشرات ثم أحوّل العشرات عند الحاجة إلى آحاد. أتحقق: المقسوم = عدد المجموعات × نصيب المجموعة + الباقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool ValidG5U4Block(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5U4Block(family)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           d is <11 or >22||n is <99 or >505||n/d is <9 or >22)return false;
        expected=family==G5U4BlockQuotient?n/d:n%d;
        return true;
    }
    private static bool VerifyG5U4Block(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5U4Block(family,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(x=>x!=','&&x!='٬'&&x!=' '&&x!='\u00a0'&&x!='\u202f')
            .Select(x=>x is >= '٠' and <= '٩'?(char)('0'+x-'٠'):
                       x is >= '۰' and <= '۹'?(char)('0'+x-'۰'):x).ToArray());
        if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
        int n=p["dividend"],d=p["divisor"];
        return family==G5U4BlockQuotient
            ?v>=0&&(long)v*d<=n&&n-(long)v*d<d
            :v>=0&&v<d&&(n-v)%d==0;
    }

    // Lesson-specific range extension of the existing base-ten division engine.
    private static Item BuildG5(string f,Random r)
    {
        int n=r.Next(20,100),d=r.Next(2,10);
        string q=f==G5Quotient
            ? $"اقسم {n} على {d} باستعمال العشرات والآحاد، واكتب ناتج القسمة الصحيح قبل الباقي."
            : $"اقسم {n} على {d} باستعمال العشرات والآحاد، ثم اكتب الباقي فقط.";
        return new Item(f,q,
            "أقسم العشرات وأُنزل الآحاد، ثم أتحقق من أن الباقي أصغر من المقسوم عليه.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n<20||n>99||d<2||d>9)return false;
        result=f==G5Quotient?n/d:n%d;
        return true;
    }

    // G5 partial quotients share this existing numerical division engine.
    // Persistence contains source parameters only, never a solved answer.
    private static Item BuildG5Partial(string f, Random r)
    {
        int divisor, quotient, dividend;
        do
        {
            divisor=r.Next(2,8);
            quotient=r.Next(100,450);
            dividend=divisor*quotient;
        }
        while(dividend>999 || (quotient/10)%10==0);
        int hundreds=(quotient/100)*100;
        int tens=((quotient/10)%10)*10;
        int ones=quotient%10;
        int a=divisor*hundreds,b=divisor*tens,c=divisor*ones;
        string prompt=f switch
        {
            G5Distributed=>$"باستخدام خاصية التوزيع احسب ({a} + {b} + {c}) ÷ {divisor}، واكتب الحاصل.",
            G5PartialSum=>$"اقسم {dividend} على {divisor} بنواتج القسمة الجزئية: اطرح {a} ثم {b} ثم {c}. ما مجموع الحواصل الجزئية؟",
            _=>$"في قسمة {dividend} على {divisor} اخترنا الأجزاء {a} و{b} و{c}. ما ناتج القسمة الجزئي للجزء {b}؟"
        };
        return new Item(f,prompt,
            "اقسم كل جزء متوافق على المقسوم عليه نفسه، واجمع نواتج القسمة الجزئية وتحقق بالضرب.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool ValidG5Partial(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5Partial(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <200 or >999||d is <2 or >7||n%d!=0)return false;
        int quotient=n/d;
        if(quotient is <100 or >=450 || (quotient/10)%10==0)return false;
        int hundreds=(quotient/100)*100;
        int tens=((quotient/10)%10)*10;
        int ones=quotient%10;
        if(checked(d*hundreds+d*tens+d*ones)!=n)return false;
        result=f==G5PartialTens?tens:hundreds+tens+ones;
        return true;
    }


    // G5 3/4-digit long division is a source-range extension of the same exact
    // quotient/remainder engine; existing G4 question families are unchanged.
    private static Item BuildG5Long(string f,Random random)
    {
        int divisor=random.Next(2,10),dividend=random.Next(100,10_000);
        string prompt=f==G5LongQuotient
            ?$"اقسم {dividend} على {divisor} بالقسمة المطولة. اكتب الحاصل الصحيح قبل الباقي."
            :$"اقسم {dividend} على {divisor} بالقسمة المطولة. ما الباقي فقط؟";
        return new Item(f,prompt,
            "أقسم وأضرب وأطرح وأنزّل، وأتحقق من العلاقة المقسوم = المقسوم عليه × الحاصل + الباقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>
            {{"dividend",dividend},{"divisor",divisor}});
    }

    private static bool ValidG5Long(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5Long(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <100 or >9999||d is <2 or >9)return false;
        result=f==G5LongQuotient?n/d:n%d;
        return true;
    }

    private static bool VerifyG5Long(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!IsG5Long(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <100 or >9999||d is <2 or >9||string.IsNullOrWhiteSpace(answer))
            return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                       c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int value))
            return false;
        if(f==G5LongQuotient)
            return value>=0&&(long)value*d<=n&&n-(long)value*d<d;
        int quotient=n/d;
        return value>=0&&value<d&&(long)quotient*d+value==n;
    }


    private static Item BuildG5First(string f,Random r)
    {
        int n=r.Next(100,10_000),d=r.Next(2,10);
        string prompt=f==G5FirstPlace
           ?$"في القسمة المطولة {n} ÷ {d}، ما قيمة منزلة أول رقم غير صفري في الحاصل (١ أو ١٠ أو ١٠٠ أو ١٠٠٠)؟"
           :$"في القسمة المطولة {n} ÷ {d}، ما أول رقم غير صفري يُكتب في الحاصل من اليسار؟";
        return new Item(f,prompt,
           "قارن أول منزلة بالمقسوم عليه، ثم ضم الرقم التالي عند الحاجة قبل كتابة أول رقم في الحاصل.",
           AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5First(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5First(f)||p.Count!=2||
            !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
            n is <100 or >9999||d is <2 or >9)return false;
        int q=n/d,place=1;
        while(q>=10){q/=10;place*=10;}
        expected=f==G5FirstPlace?place:q;
        return true;
    }
    private static bool VerifyG5First(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!IsG5First(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <100 or >9999||d is <2 or >9||string.IsNullOrWhiteSpace(answer))
            return false;
        int q=n/d;
        string digits=q.ToString(CultureInfo.InvariantCulture);
        int expected=f==G5FirstPlace? (int)Math.Pow(10,digits.Length-1):digits[0]-'0';
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }


    // Grade 5 quotient-with-internal-zero: same original numeric engine.
    private static Item BuildG5Zero(string f,Random random)
    {
        int divisor=random.Next(2,10);
        int hundreds=random.Next(1,10),ones=random.Next(1,10);
        int quotient=hundreds*100+ones;
        int remainder=f==G5ZeroRemainder?random.Next(1,divisor):0;
        int dividend=quotient*divisor+remainder;
        string prompt=f==G5ZeroQuotient
            ?$"اقسم {dividend} على {divisor} بالقسمة المطولة، واحفظ صفر المنزلة الوسطى. اكتب الحاصل الصحيح."
            :$"في القسمة المطولة {dividend} ÷ {divisor} يكون في الحاصل صفر وسطي. اكتب الباقي فقط.";
        return new Item(f,prompt,
            "أكتب صفر المنزلة التي لا تتكوّن فيها مجموعة كاملة، ثم أتحقق بالضرب وإضافة الباقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>
            {{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool ValidG5Zero(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5Zero(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <200 or >9999||d is <2 or >9)return false;
        int quotient=n/d,remainder=n%d;
        if(quotient<101||quotient>909||(quotient/10)%10!=0||
           quotient%10==0||quotient/100<1||quotient/100>9)return false;
        if(f==G5ZeroQuotient && remainder!=0)return false;
        if(f==G5ZeroRemainder && remainder==0)return false;
        expected=f==G5ZeroQuotient?quotient:remainder;
        return true;
    }
    private static bool VerifyG5Zero(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Zero(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))
            return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int value)
           &&value==expected;
    }


    // G5 reuses the exact quotient/remainder engine, changing the modeled context.
    private static Item BuildG5Model(string f,Random r)
    {
        int d=r.Next(2,10),q=r.Next(5,22),remaining=r.Next(1,d);
        int n=d*q+remaining;
        string prompt=f==G5ModelGroup
            ?$"وزع {n} قطعة بالتساوي على {d} مجموعات، وبَيّن الباقي خارج المجموعات. كم قطعة كاملة في كل مجموعة؟"
            :$"قسّم {n} عنصرًا إلى {d} مجموعات متساوية باستخدام نموذج. كم عنصرًا يبقى دون توزيع؟";
        return new Item(f,prompt,
            "مثل مجموعات متساوية، واحتفظ بالباقي خارجها، ثم تحقق أن العدد = المجموعات×العناصر لكل مجموعة + الباقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>
            {{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5Model(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!IsG5Model(f)||p.Count!=2||
          !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
          n is <11 or >200||d is <2 or >9||n%d==0||n/d is <5 or >21)return false;
        answer=f==G5ModelGroup?n/d:n%d;
        return true;
    }
    private static bool VerifyG5Model(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Model(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))
            return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
             .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                       c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int provided)
            &&provided==expected;
    }


    // Contextual remainder interpretation extends numeric division, not G4 logic.
    private static Item BuildG5ContextRemainder(string f,Random random)
    {
        int d=random.Next(2,10),q=random.Next(5,38),r=random.Next(1,d);
        int n=q*d+r;
        string prompt=f switch
        {
            G5RemainderCeil=>$"يريد {n} طالبًا ركوب حافلات صغيرة تسع كل واحدة {d} طلاب. كم حافلة نحتاج لنقل جميع الطلاب؟",
            G5RemainderFloor=>$"لديك {n} لعبة وتملأ كل علبة مكتملة بـ{d} ألعاب. كم علبة مكتملة فقط تستطيع تجهيزها؟",
            _=>$"نوزع {n} كرة على مجموعات متساوية تضم كل منها {d} كرات. كم كرة تتبقى دون مجموعة كاملة؟"
        };
        return new Item(f,prompt,
            "فسّر الباقي حسب السؤال: أضف مجموعة لنقل الجميع، أو استخدم المجموعات الممتلئة فقط، أو اكتب الباقي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5ContextRemainder(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5ContextRemainder(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <11 or >350||d is <2 or >9||n%d==0||n/d is <5 or >37)
           return false;
        int quotient=n/d;
        expected=f switch
        {
            G5RemainderCeil=>quotient+1,
            G5RemainderFloor=>quotient,
            _=>n%d
        };
        return true;
    }
    private static bool VerifyG5ContextRemainder(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5ContextRemainder(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))
            return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int provided))
            return false;
        int n=p["dividend"],d=p["divisor"];
        // Context-aware independent check uses inequalities and composition,
        // not a memorized answer field.
        return f switch
        {
            G5RemainderCeil=>provided*d>=n&&(provided-1)*d<n,
            G5RemainderFloor=>provided*d<n&&(provided+1)*d>n,
            _=>provided>=0&&provided<d&&((n-provided)%d)==0&&provided==expected
        };
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5U4Adjust(f))return ValidG5U4Adjust(f,p,out expected);
        if(IsG5U4Long(f))return ValidG5U4Long(f,p,out expected);
        if(IsG5U4Block(f))return ValidG5U4Block(f,p,out expected);
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(IsG5Partial(f))return ValidG5Partial(f,p,out expected);
        if(IsG5Long(f))return ValidG5Long(f,p,out expected);
        if(IsG5First(f))return ValidG5First(f,p,out expected);
        if(IsG5Zero(f))return ValidG5Zero(f,p,out expected);
        if(IsG5Model(f))return ValidG5Model(f,p,out expected);
        if(IsG5ContextRemainder(f))return ValidG5ContextRemainder(f,p,out expected);
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||n is <20 or >9999||d is <2 or >9)return false;
        expected=f==Quotient?n/d:n%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid base-ten division");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5U4Adjust(f))return VerifyG5U4Adjust(f,p,answer);
        if(IsG5U4Long(f))return VerifyG5U4Long(f,p,answer);
        if(IsG5U4Block(f))return VerifyG5U4Block(f,p,answer);
        if(IsG5Long(f))return VerifyG5Long(f,p,answer);
        if(IsG5First(f))return VerifyG5First(f,p,answer);
        if(IsG5Zero(f))return VerifyG5Zero(f,p,answer);
        if(IsG5Model(f))return VerifyG5Model(f,p,answer);
        if(IsG5ContextRemainder(f))return VerifyG5ContextRemainder(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

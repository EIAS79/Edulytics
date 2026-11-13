using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson03BaseTenDivisionQuestions
{
    internal const string Quotient="uae.g4.u06.l03.base_ten_quotient";
    internal const string Remainder="uae.g4.u06.l03.base_ten_remainder";
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
    private static bool IsG5Zero(string? f)=>f is G5ZeroQuotient or G5ZeroRemainder;
    private static bool IsG5First(string? f)=>f is G5FirstPlace or G5FirstDigit;
    private static bool IsG5Long(string? f)=>f is G5LongQuotient or G5LongRemainder;
    private static bool IsG5Partial(string? f)=>f is G5Distributed or G5PartialSum or G5PartialTens;
    private static bool IsG5(string? f)=>f is G5Quotient or G5Remainder;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder || IsG5(f) || IsG5Partial(f) || IsG5Long(f) || IsG5First(f) || IsG5Zero(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown base-ten division family");
        if(IsG5(f))return BuildG5(f,r);
        if(IsG5Partial(f))return BuildG5Partial(f,r);
        if(IsG5Long(f))return BuildG5Long(f,r);
        if(IsG5First(f))return BuildG5First(f,r);
        if(IsG5Zero(f))return BuildG5Zero(f,r);
        int dividend=r.Next(20,10000),divisor=r.Next(2,10);
        string prompt=f==Quotient
          ?$"مثّل {dividend} ÷ {divisor} بمكعبات العشرات والآحاد ثم اكتب ناتج القسمة الصحيح دون الباقي."
          :$"قسّم {dividend} إلى {divisor} مجموعات متساوية باستخدام مكعبات القيمة المكانية. كم آحادًا تتبقى؟";
        return new Item(f,prompt,"أوزع العشرات، وأعيد تجميع العشرات المتبقية إلى آحاد، ثم أوزع الآحاد بالتساوي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
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

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(IsG5Partial(f))return ValidG5Partial(f,p,out expected);
        if(IsG5Long(f))return ValidG5Long(f,p,out expected);
        if(IsG5First(f))return ValidG5First(f,p,out expected);
        if(IsG5Zero(f))return ValidG5Zero(f,p,out expected);
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
        if(IsG5Long(f))return VerifyG5Long(f,p,answer);
        if(IsG5First(f))return VerifyG5First(f,p,answer);
        if(IsG5Zero(f))return VerifyG5Zero(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

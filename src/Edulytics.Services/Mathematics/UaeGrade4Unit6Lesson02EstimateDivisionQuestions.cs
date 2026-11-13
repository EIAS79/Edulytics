using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson02EstimateDivisionQuestions
{
    internal const string Estimate="uae.g4.u06.l02.estimate_compatible_quotient";
    internal const string CompatibleDividend="uae.g4.u06.l02.compatible_dividend";
    internal const string G5Quotient="uae.g5.u03.l05.estimate_compatible_quotient";
    internal const string G5Dividend="uae.g5.u03.l05.select_compatible_dividend";
    internal const string G5Above="uae.g5.u03.l05.is_estimate_above_exact";
    internal const string G5TwoDigitEstimate="uae.g5.u04.l01.estimate_two_digit_quotient";
    internal const string G5TwoDigitCompatible="uae.g5.u04.l01.choose_compatible_dividend";
    internal const string G5TwoDigitAbove="uae.g5.u04.l01.estimate_above_exact";
    private static bool IsG5TwoDigit(string? family)=>family is G5TwoDigitEstimate or G5TwoDigitCompatible or G5TwoDigitAbove;
    private static bool IsG5(string? f)=>f is G5Quotient or G5Dividend or G5Above;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or CompatibleDividend || IsG5(f) || IsG5TwoDigit(f);
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported estimation family");
        if(IsG5(f))return BuildG5(f,random);
        if(IsG5TwoDigit(f))return BuildG5TwoDigit(f,random);
        int divisor=random.Next(2,10),dividend=random.Next(101,9000);
        int place=dividend>=1000?100:10;
        int q=Math.Max(place,((dividend+(divisor*place)/2)/(divisor*place))*place);
        string prompt=f==Estimate
            ?$"قدّر ناتج {dividend} ÷ {divisor} باستخدام عددين متوافقين. اكتب ناتج القسمة التقديري."
            :$"اختر العدد المتوافق الأقرب إلى {dividend} الذي يسهل قسمته على {divisor} إلى عشرات أو مئات. اكتب المقسوم المتوافق.";
        return new Item(f,prompt,"أختار مقسومًا متوافقًا مع المقسوم عليه، وأتحقق باستخدام الضرب.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }

    private static readonly int[] CompatibleQuotientsG5=
        [10,20,25,30,40,50,60,70,75,80,90,100,120,150,200,250,300];
    private static int ClosestCompatibleG5(int n,int d)=>
        CompatibleQuotientsG5.OrderBy(q=>Math.Abs(q*d-n)).ThenBy(q=>q).First();
    private static Item BuildG5(string f,Random r)
    {
        int dividend=r.Next(120,901),divisor=r.Next(2,10);
        int approximate=ClosestCompatibleG5(dividend,divisor);
        int compatible=approximate*divisor;
        string prompt=f switch
        {
            G5Quotient=>$"استعمل عددًا متوافقًا مع {divisor} لتقدير ناتج {dividend} ÷ {divisor}. ما ناتج القسمة التقديري؟",
            G5Dividend=>$"ما المقسوم المتوافق القريب من {dividend} لتقدير القسمة على {divisor}؟",
            _=>$"بعد اختيار العدد المتوافق {compatible} لتقدير {dividend}÷{divisor}، اكتب ١ إذا التقدير أكبر من الناتج الدقيق أو مساويًا له، و٠ إذا كان أقل."
        };
        return new Item(f,prompt,
            "أبحث عن عدد قريب يقبل القسمة بسهولة على المقسوم عليه ثم أراجع المعقولية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
           n is <120 or >900||d is <2 or >9)return false;
        int q=ClosestCompatibleG5(n,d);
        expected=f switch
        {
            G5Quotient=>q,
            G5Dividend=>q*d,
            _=>q*d>=n?1:0
        };
        return true;
    }


    // G5 Unit4: two-digit divisor extension reusing the existing estimator.
    // Deterministic convention: round divisor to nearest ten; choose the
    // nearest multiple of that rounded divisor to the original dividend.
    private static int RoundedTens(int d)=>((d+5)/10)*10;
    private static int G5TwoDigitCompatibleQuotient(int n,int d)
    {
        var rounded=RoundedTens(d);
        return Math.Max(1,(n+rounded/2)/rounded);
    }
    private static Item BuildG5TwoDigit(string f,Random random)
    {
        int n=random.Next(240,951),d=random.Next(12,90);
        int rounded=RoundedTens(d),q=G5TwoDigitCompatibleQuotient(n,d),compatible=q*rounded;
        string prompt=f switch
        {
            G5TwoDigitEstimate=>$"قدّر {n} ÷ {d}: قرّب المقسوم عليه إلى {rounded}، ثم اختر أقرب مضاعف له إلى {n}. ما التقدير؟",
            G5TwoDigitCompatible=>$"لتقدير {n} ÷ {d} قرّب المقسوم عليه إلى {rounded}. ما أقرب مقسوم متوافق وهو مضاعف {rounded}؟",
            _=>$"قدّر {n} ÷ {d} باختيار {compatible} ÷ {rounded}. اكتب ١ إذا كان التقدير أكبر من الناتج الدقيق أو يساويه، و٠ إذا كان أصغر."
        };
        return new Item(f,prompt,
            "أقرب المقسوم عليه إلى العشرة، وأختار المضاعف الأقرب، ثم أراجع معقولية الناتج.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",n},{"divisor",d}});
    }
    private static bool ValidG5TwoDigit(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5TwoDigit(f)||p.Count!=2||
            !p.TryGetValue("dividend",out int n)||!p.TryGetValue("divisor",out int d)||
            n is <240 or >950||d is <12 or >89)return false;
        int rounded=RoundedTens(d),q=G5TwoDigitCompatibleQuotient(n,d);
        expected=f switch
        {
            G5TwoDigitEstimate=>q,
            G5TwoDigitCompatible=>q*rounded,
            _=>q*d>=n?1:0
        };
        return true;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(IsG5TwoDigit(f))return ValidG5TwoDigit(f,p,out expected);
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("dividend",out int n)||
           !p.TryGetValue("divisor",out int d)||n is <101 or >=9000||d is <2 or >9)return false;
        int place=n>=1000?100:10;
        int q=Math.Max(place,((n+(d*place)/2)/(d*place))*place);
        expected=f==Estimate?q:q*d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid division estimate");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}

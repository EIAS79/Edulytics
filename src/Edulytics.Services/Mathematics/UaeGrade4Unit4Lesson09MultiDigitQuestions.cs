using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 Unit 4 Lesson 9: exact products of 3-4 digit numbers by one-digit factors.
internal static class UaeGrade4Unit4Lesson09MultiDigitQuestions
{
    internal const string Product="uae.g4.u04.l09.multidigit_product";
    internal const string PartialHundreds="uae.g4.u04.l09.partial_hundreds";
    internal const string RoundEstimate="uae.g4.u04.l09.round_estimate";
    // Extend G4 exact multiplication without changing its Grade 4 families.
    internal const string G5Product="uae.g5.u02.l09.exact_one_digit_product";
    internal const string G5Ones="uae.g5.u02.l09.ones_partial";
    internal const string G5Tens="uae.g5.u02.l09.tens_partial";
    internal const string G5Estimate="uae.g5.u02.l09.round_and_estimate";
    private static bool IsG5(string? f)=>f is G5Product or G5Ones or G5Tens or G5Estimate;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Product or PartialHundreds or RoundEstimate || IsG5(f);
    internal static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported multi-digit family");
        if(IsG5(family))return BuildG5(family,rng);
        int a=rng.Next(2,10),n=rng.Next(101,9999),hundreds=(n/100%10)*100;
        string prompt=family switch
        {
            Product=>$"أوجد ناتج {a} × {n} مستخدمًا نواتج الضرب الجزئية أو الضرب الرأسي.",
            PartialHundreds=>$"عند ضرب {a} × {n}، كم ناتج ضرب المئات فقط قبل إعادة التجميع؟",
            _=>$"قرّب {n} لأكبر قيمة مكانية، ثم احسب تقدير {a} × {n}."
        };
        return new Item(family,prompt,"أضرب الآحاد والعشرات والمئات والألوف ثم أجمع النواتج الجزئية وأتحقق بالتقدير.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",a},{"number",n}});
    }

    private static Item BuildG5(string family,Random random)
    {
        int factor=random.Next(2,10),number=random.Next(21,999);
        string prompt=family switch
        {
            G5Product=>$"أوجد حاصل ضرب {number} × {factor} باستعمال الضرب الرأسي وإعادة التجميع.",
            G5Ones=>$"عند حساب {number} × {factor} ما ناتج ضرب منزلة الآحاد فقط قبل إعادة التجميع؟",
            G5Tens=>$"عند حساب {number} × {factor}، ما ناتج ضرب قيمة منزلة العشرات فقط؟",
            _=>$"قدّر {number} × {factor} بتقريب العامل الأكبر إلى أكبر منزلة."
        };
        return new Item(family,prompt,
            "أضرب آحاد العدد ثم عشراته ثم مئاته، وأعيد تجميع العشرات والمئات وأتحقق بالتقدير.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"number",number}});
    }
    private static bool ValidG5(string family,
        IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5(family)||p.Count!=2||!p.TryGetValue("factor",out int a)||
           !p.TryGetValue("number",out int n)||
           a is <2 or >9||n is <21 or >=999)return false;
        int place=n>=100?100:10;
        int rounded=((n+place/2)/place)*place;
        expected=family switch
        {
            G5Product=>a*n,
            G5Ones=>a*(n%10),
            G5Tens=>a*((n/10%10)*10),
            _=>a*rounded
        };
        return true;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("factor",out var a)||
           !p.TryGetValue("number",out var n)||a is <2 or >9||n is <101 or >9998)return false;
        int place=n>=1000?1000:100,rounded=(n+place/2)/place*place;
        expected=f switch {Product=>a*n,PartialHundreds=>a*((n/100%10)*100),_=>a*rounded};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int value))throw new InvalidOperationException("Invalid multidigit problem");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

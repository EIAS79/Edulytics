using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 Unit 4 Lesson 9: exact products of 3-4 digit numbers by one-digit factors.
internal static class UaeGrade4Unit4Lesson09MultiDigitQuestions
{
    internal const string Product="uae.g4.u04.l09.multidigit_product";
    internal const string PartialHundreds="uae.g4.u04.l09.partial_hundreds";
    internal const string RoundEstimate="uae.g4.u04.l09.round_estimate";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Product or PartialHundreds or RoundEstimate;
    internal static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade4 multidigit family");
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
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
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

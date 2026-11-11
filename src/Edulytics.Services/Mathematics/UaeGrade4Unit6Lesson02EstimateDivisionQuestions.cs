using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson02EstimateDivisionQuestions
{
    internal const string Estimate="uae.g4.u06.l02.estimate_compatible_quotient";
    internal const string CompatibleDividend="uae.g4.u06.l02.compatible_dividend";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Estimate or CompatibleDividend;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported estimation family");
        int divisor=random.Next(2,10),dividend=random.Next(101,9000);
        int place=dividend>=1000?100:10;
        int q=Math.Max(place,((dividend+(divisor*place)/2)/(divisor*place))*place);
        string prompt=f==Estimate
            ?$"قدّر ناتج {dividend} ÷ {divisor} باستخدام عددين متوافقين. اكتب ناتج القسمة التقديري."
            :$"اختر العدد المتوافق الأقرب إلى {dividend} الذي يسهل قسمته على {divisor} إلى عشرات أو مئات. اكتب المقسوم المتوافق.";
        return new Item(f,prompt,"أختار مقسومًا متوافقًا مع المقسوم عليه، وأتحقق باستخدام الضرب.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"dividend",dividend},{"divisor",divisor}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
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

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 Common U02 L07: subtraction requiring regrouping across zero-valued columns.
internal static class UaeGrade4Unit2Lesson07ZeroSubtractionQuestions
{
    internal const string Four="uae.g4.u02.l07.subtract_through_zero_four";
    internal const string Five="uae.g4.u02.l07.subtract_through_zero_five";
    internal const string Six="uae.g4.u02.l07.subtract_through_zero_six";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Four or Five or Six;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 zero-subtraction family");
        int a=f switch
        {
            Four=>rng.Next(4,10)*1000+100,
            Five=>rng.Next(12,90)*1000+200,
            _=>rng.Next(200,900)*1000
        };
        int b=rng.Next(f==Six?10000:1000,a-(f==Six?1000:100));
        return new Item(f,$"اطرح {a} − {b}. أعد التجميع عبر منزلة الصفر ثم تحقق بالجمع.",
            "أعيد التجميع بدءًا من أول منزلة غير صفرية على اليسار، ثم أطرح وأتحقق بجمع الفرق والمطروح.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b))return false;
        bool valid=f switch
        {
            Four=>a>=4100&&a<=9100&&a%1000==100&&b>=1000&&b<a-100,
            Five=>a>=12200&&a<=89200&&a%1000==200&&b>=1000&&b<a-100,
            _=>a>=200000&&a<=899000&&a%1000==0&&b>=10000&&b<a-1000
        };
        if(!valid)return false;
        expected=a-b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid zero subtraction");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string text=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                       c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(text,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}

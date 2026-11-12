using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 Common U02 L04: estimate a sum or difference by rounding both terms.
internal static class UaeGrade4Unit2Lesson04EstimateQuestions
{
    internal const string SumHundred="uae.g4.u02.l04.sum_nearest_hundred";
    internal const string DiffHundred="uae.g4.u02.l04.diff_nearest_hundred";
    internal const string SumThousand="uae.g4.u02.l04.sum_nearest_thousand";
    internal const string DiffThousand="uae.g4.u02.l04.diff_nearest_thousand";
    internal const string SumTenThousand="uae.g4.u02.l04.sum_nearest_ten_thousand";
    internal const string DiffTenThousand="uae.g4.u02.l04.diff_nearest_ten_thousand";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is SumHundred or DiffHundred
        or SumThousand or DiffThousand or SumTenThousand or DiffTenThousand;
    private static (int place,bool subtract) Mode(string f)=>f switch
    {
        SumHundred=>(100,false),DiffHundred=>(100,true),
        SumThousand=>(1000,false),DiffThousand=>(1000,true),
        SumTenThousand=>(10000,false),DiffTenThousand=>(10000,true),
        _=>throw new InvalidOperationException("Unknown estimation family")
    };
    private static int Round(int n,int place)=>checked(((n+place/2)/place)*place);
    internal static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported estimate family");
        var (place,subtract)=Mode(family);
        int a=rng.Next(17000,85000),b=rng.Next(1500,16000);
        string placeName=place switch {100=>"مئة",1000=>"ألف",_=>"عشرة آلاف"};
        string op=subtract?"−":"+";
        return new Item(family,$"قدّر قيمة {a} {op} {b} بالتقريب إلى أقرب {placeName} لكل عدد أولًا.",
            "قرّب كلا الحدين إلى المنزلة المحددة ثم أجمع أو أطرح العددين المقرَّبين.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int n)
    {
        n=0;
        if(!Supports(family)||p.Count!=2||!p.TryGetValue("a",out int a)||
            !p.TryGetValue("b",out int b)||a is <17000 or >=85000||
            b is <1500 or >=16000)return false;
        var(place,subtract)=Mode(family);
        n=subtract?Round(a,place)-Round(b,place):Round(a,place)+Round(b,place);
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out var n))throw new InvalidOperationException("Bad estimation item");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string text=new(answer.Trim().Where(c=>c!='٬'&&c!=','&&c!=' '&&c!='\u00a0')
          .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                     c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(text,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)
            &&actual==n;
    }
}

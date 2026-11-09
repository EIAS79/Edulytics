using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Exact-scored approximation questions in the existing lesson Practice engine.
internal static class UaeGrade3Unit2Lesson05Questions
{
    internal const string Family="uae.g3.u02.l05.estimate_sum_by_rounding";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    private static int Round(int n,int b)=>((n+b/2)/b)*b;
    private static int Highest(int n)=>n>=1000?1000:n>=100?100:10;
    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported estimate sum family");
        int mode=rng.Next(3),a,b,place;
        if(mode==0){a=rng.Next(20,900);b=rng.Next(20,900);place=10;}
        else if(mode==1){a=rng.Next(100,9000);b=rng.Next(100,9000);place=100;}
        else {bool four=rng.Next(2)==0;a=four?rng.Next(1000,4999):rng.Next(100,899);b=four?rng.Next(1000,4999):rng.Next(100,899);place=Highest(a);}
        int result=Round(a,place)+Round(b,place);
        string rounding=place==10?"عشرة":place==100?"مئة":"ألف";
        string prompt=mode==2?$"قدّر مجموع {a} + {b} بالتقريب إلى أعلى منزلة في كل عدد، ثم اجمع الناتجين. ما التقدير؟"
          :$"قدّر مجموع {a} + {b} بالتقريب إلى أقرب {rounding} لكل حد جمعي أولًا. ما التقدير؟";
        return new Item(family,prompt,"أقرّب كل حد جمعي أولًا ثم أجمع العددين المقربين؛ هذا تقدير لا جمع دقيق.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"place",place},{"result",result}});
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||a is <20 or >8999||
           !p.TryGetValue("b",out int b)||b is <20 or >8999||
           !p.TryGetValue("mode",out int m)||m is <0 or >2||
           !p.TryGetValue("place",out int place)||place is not (10 or 100 or 1000)||
           !p.TryGetValue("result",out int actual))return false;
        if(m==0&&(place!=10||a>=900||b>=900)||m==1&&(place!=100||a<100||b<100)||
            m==2&&(a<100||b<100||Highest(a)!=place||Highest(b)!=place))return false;
        value=Round(a,place)+Round(b,place);return value==actual;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid G3 estimated sum parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out int result)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)&&n==result;
    }
}

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson04FractionSubtractionQuestions
{
    internal const string Difference="uae.g4.u09.l04.difference_numerator";
    internal const string ReducedNumerator="uae.g4.u09.l04.reduced_difference_numerator";
    internal const string ReducedDenominator="uae.g4.u09.l04.reduced_difference_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Difference or ReducedNumerator or ReducedDenominator;
    private static int Gcd(int x,int y){while(y!=0){(x,y)=(y,x%y);}return x;}
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported subtraction");
        int d=random.Next(3,13),a=random.Next(2,d+1),b=random.Next(1,a);
        var p=new Dictionary<string,int>{{"denominator",d},{"a",a},{"b",b}};
        string prompt=family switch{
            Difference=>$"أوجد بسط الفرق قبل التبسيط: {a}/{d} − {b}/{d}.",
            ReducedNumerator=>$"احسب {a}/{d} − {b}/{d} بأبسط صورة. ما البسط؟",
            _=>$"احسب {a}/{d} − {b}/{d} بأبسط صورة. ما المقام؟"
        };
        return new Item(family,prompt,"اطرح البسطين واحتفظ بالمقام، ثم اقسم على العامل المشترك الأكبر إن وجد.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           d is <3 or >12||a<2||a>d||b<1||b>=a)return false;
        int difference=a-b,gcd=Gcd(difference,d);
        answer=family switch{Difference=>difference,ReducedNumerator=>difference/gcd,_=>d/gcd};
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid fraction subtraction parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

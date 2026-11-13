using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson04FractionSubtractionQuestions
{
    internal const string Difference="uae.g4.u09.l04.difference_numerator";
    internal const string ReducedNumerator="uae.g4.u09.l04.reduced_difference_numerator";
    internal const string ReducedDenominator="uae.g4.u09.l04.reduced_difference_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Difference or ReducedNumerator or ReducedDenominator or G5Extended;
    private static int Gcd(int x,int y){while(y!=0){(x,y)=(y,x%y);}return x;}

    // G5 extends G4 exact-like subtraction up to twenty-eighths,
    // retaining the existing G4 parameters and behavior.
    internal const string G5Extended="uae.g5.u08.l03.subtract_like_extended_fraction";
    private static Item BuildG5Extended(Random rng)
    {
        int d=rng.Next(13,29),a=rng.Next(2,d+1),b=rng.Next(1,a);
        return new Item(G5Extended,$"احسب {a}/{d} - {b}/{d} بأبسط صورة (بسط/مقام).",
            "اطرح البسطين وثبت المقام ثم بسط الفرق.",AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"denominator",d},{"a",a},{"b",b}});
    }
    private static bool ValidG5Extended(IReadOnlyDictionary<string,int> p,out int diff,out int d)
    {
        diff=d=0;
        if(p.Count!=3||!p.TryGetValue("denominator",out d)||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           d is <13 or >28||a<2||a>d||b<1||b>=a)return false;
        diff=a-b;return true;
    }
    private static string SolveG5Extended(IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Extended(p,out int diff,out int d))
            throw new InvalidOperationException("Invalid G5 like-fraction parameters");
        int g=Gcd(diff,d);return $"{diff/g}/{d/g}";
    }
    private static bool VerifyG5Extended(IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!ValidG5Extended(p,out int diff,out int d)||
           string.IsNullOrWhiteSpace(response)||response.Length>48)return false;
        string s=new(response.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch!='٬')
            .Select(ch=>ch switch {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '⁄'=>'/',_=>ch}).ToArray());
        string[] parts=s.Split('/');
        if(parts.Length!=2||!int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int n)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)||
           n<1||den<1)return false;
        int x=n,y=den;
        while(y!=0){int rem=x%y;x=y;y=rem;}
        return x==1&&(long)n*d==(long)diff*den;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported subtraction");
        if(family==G5Extended)return BuildG5Extended(random);
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
        if(family==G5Extended)return SolveG5Extended(p);
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid fraction subtraction parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G5Extended)return VerifyG5Extended(p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

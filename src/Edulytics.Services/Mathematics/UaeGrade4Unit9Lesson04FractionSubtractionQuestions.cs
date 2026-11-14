using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson04FractionSubtractionQuestions
{
    internal const string Difference="uae.g4.u09.l04.difference_numerator";
    internal const string ReducedNumerator="uae.g4.u09.l04.reduced_difference_numerator";
    internal const string ReducedDenominator="uae.g4.u09.l04.reduced_difference_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Difference or ReducedNumerator or ReducedDenominator or G5Extended || G5Unlike(f);
    private static int Gcd(int x,int y){while(y!=0){(x,y)=(y,x%y);}return x;}

    // G5 extends G4 exact-like subtraction up to twenty-eighths,
    // retaining the existing G4 parameters and behavior.
    internal const string G5Extended="uae.g5.u08.l03.subtract_like_extended_fraction";
    internal const string G5UnlikeCommon="uae.g5.u08.l06.model_subtract_common_denominator";
    internal const string G5UnlikeDifference="uae.g5.u08.l06.model_subtract_difference";
    internal const string G5UnlikeSecond="uae.g5.u08.l06.model_second_equivalent_numerator";
    private static bool G5Unlike(string? f)=>f is G5UnlikeCommon or G5UnlikeDifference or G5UnlikeSecond;
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


    // Grade5 model for unlike denominators; preserves G4 like-denominator behavior.
    private static Item BuildG5Unlike(string family,Random rng)
    {
        int n1=0,d1=0,n2=0,d2=0;
        for(int trial=0;trial<128;trial++)
        {
            d1=rng.Next(3,11);do{d2=rng.Next(2,12);}while(d2==d1);
            n1=rng.Next(1,d1);n2=rng.Next(1,d2);
            if(n1*d2>n2*d1)break;
        }
        if(n1*d2<=n2*d1)throw new InvalidOperationException("Unable to generate positive G5 fraction difference.");
        int lcm=d1/Gcd(d1,d2)*d2;
        string prompt=family switch
        {
            G5UnlikeCommon=>$"لطرح {n1}/{d1} − {n2}/{d2} بمقاطع شرائط كسور، ما المقام المشترك الأصغر؟",
            G5UnlikeSecond=>$"في {n1}/{d1} − {n2}/{d2} يصبح المقام المشترك {lcm}. ما بسط الكسر الثاني المكافئ؟",
            _=>$"أمثل {n1}/{d1} − {n2}/{d2} بشرائط كسور متساوية الطول، فما الفرق في أبسط صورة (بسط/مقام)؟"
        };
        return new Item(family,prompt,
          "أعيد تقسيم الشرائط إلى مقام مشترك، أطرح الأجزاء المتساوية، ثم أبسط.",
          family==G5UnlikeDifference?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
          new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}});
    }
    private static bool ValidG5Unlike(string family,IReadOnlyDictionary<string,int> p,
        out int n1,out int d1,out int n2,out int d2,out int lcm)
    {
        n1=d1=n2=d2=lcm=0;
        if(!G5Unlike(family)||p.Count!=4||!p.TryGetValue("n1",out n1)||
           !p.TryGetValue("d1",out d1)||!p.TryGetValue("n2",out n2)||
           !p.TryGetValue("d2",out d2)||d1 is <3 or >10||d2 is <2 or >11||
           d1==d2||n1<1||n1>=d1||n2<1||n2>=d2||n1*d2<=n2*d1)return false;
        lcm=d1/Gcd(d1,d2)*d2;return true;
    }
    private static string SolveG5Unlike(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Unlike(f,p,out int n1,out int d1,out int n2,out int d2,out int lcm))
            throw new InvalidOperationException("Invalid G5 fraction subtraction model.");
        if(f==G5UnlikeCommon)return lcm.ToString(CultureInfo.InvariantCulture);
        if(f==G5UnlikeSecond)return (n2*lcm/d2).ToString(CultureInfo.InvariantCulture);
        int numerator=n1*(lcm/d1)-n2*(lcm/d2),g=Gcd(numerator,lcm);
        return $"{numerator/g}/{lcm/g}";
    }
    private static bool VerifyG5Unlike(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Unlike(f,p,out int n1,out int d1,out int n2,out int d2,out int lcm)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>45)return false;
        string t=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not '٬' and not ',')
          .Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
             >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
             '⁄'=>'/',_=>ch}).ToArray());
        if(f!=G5UnlikeDifference)
            return int.TryParse(t,NumberStyles.None,CultureInfo.InvariantCulture,out int a)&&
                a==(f==G5UnlikeCommon?lcm:n2*lcm/d2);
        string[] parts=t.Split('/');
        if(parts.Length!=2||!int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int n)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int d)||
           n<1||d<1||Gcd(n,d)!=1)return false;
        return (long)n*d1*d2==(long)d*(n1*d2-n2*d1);
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported subtraction");
        if(family==G5Extended)return BuildG5Extended(random);
        if(G5Unlike(family))return BuildG5Unlike(family,random);
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
        if(G5Unlike(family))return SolveG5Unlike(family,p);
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid fraction subtraction parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G5Extended)return VerifyG5Extended(p,answer);
        if(G5Unlike(family))return VerifyG5Unlike(family,p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

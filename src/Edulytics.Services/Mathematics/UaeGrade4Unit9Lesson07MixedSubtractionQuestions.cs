using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson07MixedSubtractionQuestions
{
    internal const string Whole="uae.g4.u09.l07.difference_whole";
    internal const string Fraction="uae.g4.u09.l07.difference_fraction_numerator";
    internal const string Improper="uae.g4.u09.l07.difference_improper_numerator";
    internal const string G5MixedDifferenceFraction="uae.g5.u08.l12.mixed_difference_fraction";
    internal const string G5MixedDifferenceWhole="uae.g5.u08.l12.mixed_difference_whole";
    internal const string G5BorrowMixedDifference="uae.g5.u08.l13.borrow_mixed_difference";
    private static bool IsG5Mixed(string? f)=>f is G5MixedDifferenceFraction or G5MixedDifferenceWhole or G5BorrowMixedDifference;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Whole or Fraction or Improper || IsG5Mixed(family);
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported mixed subtraction");
        if(IsG5Mixed(family))return BuildG5Mixed(family,random);
        int d=random.Next(2,11),w1=random.Next(2,9),w2=random.Next(1,w1),n1=random.Next(1,d),n2=random.Next(1,d);
        var p=new Dictionary<string,int>
        {{"denominator",d},{"whole1",w1},{"whole2",w2},{"numerator1",n1},{"numerator2",n2}};
        string prefix=$"احسب {w1} و{n1}/{d} − {w2} و{n2}/{d}.";
        string prompt=family switch
        {
            Whole=>prefix+" كم عدد الوحدات الكاملة في الناتج بعد إعادة التجميع؟",
            Fraction=>prefix+" ما بسط الجزء الكسري المتبقي؟ (اكتب ٠ إذا كان الناتج كليًا.)",
            _=>prefix+" ما بسط الكسر المعتل المكافئ للفرق؟"
        };
        return new Item(family,prompt,"حوّل الأعداد الكسرية إلى كسور معتلة، ثم اطرح البسطين وأعد التجميع.",
            AssessmentItemType.Numeric,p);
    }

    // Extend Grade4 mixed-subtraction structure to unequal denominators, with
    // Int32 component parameters and exact rational answer; Grade4 unchanged.
    private static int GcdG5(int a,int b)
    {while(b!=0){int rem=a%b;a=b;b=rem;}return a;}
    private static Item BuildG5Mixed(string f,Random rng)
    {
        int w1=rng.Next(3,10),w2=rng.Next(1,w1);
        int d1=rng.Next(2,11),d2=rng.Next(2,12);
        int n1=rng.Next(1,d1),n2=rng.Next(1,d2);
        if(f==G5BorrowMixedDifference)
        {
            // A borrowed unit is required when the first fractional part is smaller.
            for(int i=0;i<128 && n1*d2>=n2*d1;i++)
            {
                d1=rng.Next(3,11);d2=rng.Next(3,12);
                n1=rng.Next(1,d1);n2=rng.Next(1,d2);
            }
            if(n1*d2>=n2*d1)
                throw new InvalidOperationException("Could not generate a regrouping example.");
        }
        var p=new Dictionary<string,int>{{"w1",w1},{"n1",n1},{"d1",d1},
            {"w2",w2},{"n2",n2},{"d2",d2}};
        string prompt=f is G5MixedDifferenceFraction or G5BorrowMixedDifference
            ?$"أوجد الفرق {w1} و{n1}/{d1} − {w2} و{n2}/{d2}، ثم اكتب الناتج في صورة كسر مبسط (بسط/مقام)."
            :$"بعد طرح {w2} و{n2}/{d2} من {w1} و{n1}/{d1}، كم وحدة كاملة في الفرق؟";
        return new Item(f,prompt,
           "حوّل الأعداد الكسرية إلى كسور غير فعلية، وحّد المقامات ثم اطرح وتحقق بالجمع.",
           f is G5MixedDifferenceFraction or G5BorrowMixedDifference ?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,p);
    }
    private static bool G5Valid(string f,IReadOnlyDictionary<string,int> p,
        out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)
    {
        w1=n1=d1=w2=n2=d2=0;
        return IsG5Mixed(f)&&p.Count==6&&
            p.TryGetValue("w1",out w1)&&p.TryGetValue("n1",out n1)&&
            p.TryGetValue("d1",out d1)&&p.TryGetValue("w2",out w2)&&
            p.TryGetValue("n2",out n2)&&p.TryGetValue("d2",out d2)&&
            w1 is >=3 and <=9 && w2>=1&&w2<w1 &&
            d1 is >=2 and <=10 &&d2 is >=2 and <=11 &&
            n1>=1&&n1<d1&&n2>=1&&n2<d2&&
            (f!=G5BorrowMixedDifference||n1*d2<n2*d1);
    }
    private static string SolveG5Mixed(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!G5Valid(f,p,out int w1,out int n1,out int d1,out int w2,out int n2,out int d2))
            throw new InvalidOperationException("Invalid G5 mixed subtraction.");
        int top=(w1*d1+n1)*d2-(w2*d2+n2)*d1,den=d1*d2;
        if(top<=0)throw new InvalidOperationException("Non-positive G5 difference.");
        if(f==G5MixedDifferenceWhole)return (top/den).ToString(CultureInfo.InvariantCulture);
        int g=GcdG5(top,den);
        return $"{top/g}/{den/g}";
    }
    private static bool VerifyG5Mixed(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!G5Valid(f,p,out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>40)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
            .Select(ch=>ch switch {>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
              >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'⁄'=>'/',_=>ch}).ToArray());
        int lhs=(w1*d1+n1)*d2-(w2*d2+n2)*d1;
        if(lhs<=0)return false;
        if(f==G5MixedDifferenceWhole)
            return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int whole)
                &&whole==lhs/(d1*d2);
        string[] parts=normalized.Split('/');
        if(parts.Length!=2||
           !int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int n)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int d)||
           n<1||d<1||GcdG5(n,d)!=1)return false;
        return (long)n*d1*d2==(long)d*lhs;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=5||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("whole1",out int w1)||!p.TryGetValue("whole2",out int w2)||
            !p.TryGetValue("numerator1",out int n1)||!p.TryGetValue("numerator2",out int n2)||
            d is <2 or >10||w1 is <2 or >8||w2<1||w2>=w1||n1<1||n2<1||n1>=d||n2>=d)return false;
        int numerator=(w1-w2)*d+n1-n2;
        if(numerator<=0)return false;
        answer=family switch{Whole=>numerator/d,Fraction=>numerator%d,_=>numerator};
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Mixed(family))return SolveG5Mixed(family,p);
        if(!Valid(family,p,out int a))throw new InvalidOperationException("Invalid mixed subtraction parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Mixed(family))return VerifyG5Mixed(family,p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

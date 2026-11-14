using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson06MixedAdditionQuestions
{
    internal const string Whole="uae.g4.u09.l06.sum_whole";
    internal const string Fraction="uae.g4.u09.l06.sum_fraction_numerator";
    internal const string Improper="uae.g4.u09.l06.sum_improper_numerator";
    internal const string G5MixedModelSum="uae.g5.u08.l10.model_mixed_sum_fraction";
    internal const string G5MixedModelWhole="uae.g5.u08.l10.model_mixed_sum_whole";
    private static bool IsG5Model(string? f)=>f is G5MixedModelSum or G5MixedModelWhole;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Whole or Fraction or Improper || IsG5Model(family);
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported mixed addition");
        if(IsG5Model(family))return BuildG5Model(family,random);
        int d=random.Next(2,11),w1=random.Next(1,8),w2=random.Next(1,8),n1=random.Next(1,d),n2=random.Next(1,d);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>
        {{"denominator",d},{"whole1",w1},{"whole2",w2},{"numerator1",n1},{"numerator2",n2}};
        string prefix=$"اجمع {w1} و{n1}/{d} مع {w2} و{n2}/{d}.";
        string prompt=family switch
        {
            Whole=>prefix+" كم عدد الوحدات الكاملة في الناتج بعد إعادة التجميع؟",
            Fraction=>prefix+" ما بسط الجزء الكسري في العدد الكسري الناتج؟ (اكتب ٠ إذا كان الناتج كليًا.)",
            _=>prefix+" ما بسط الكسر المعتل المكافئ للمجموع؟"
        };
        return new Item(family,prompt,"اجمع الأجزاء الكلية والبسوط، وأعد تجميع الكسور المعتلة إذا لزم.",
            AssessmentItemType.Numeric,p);
    }

    private static int GcdG5(int a,int b)
    {while(b!=0){int rem=a%b;a=b;b=rem;}return a;}
    private static Item BuildG5Model(string f,Random r)
    {
        int w1=r.Next(1,8),w2=r.Next(1,8),d1=r.Next(2,10);
        int d2=r.Next(2,11),n1=r.Next(1,d1),n2=r.Next(1,d2);
        var p=new Dictionary<string,int>{{"w1",w1},{"n1",n1},{"d1",d1},
            {"w2",w2},{"n2",n2},{"d2",d2}};
        string prompt=f==G5MixedModelSum
          ?$"مثّل {w1} و{n1}/{d1} + {w2} و{n2}/{d2} على دوائر متساوية الكل، ثم اكتب المجموع ككسر غير فعلي مبسط (بسط/مقام)."
          :$"عند جمع {w1} و{n1}/{d1} مع {w2} و{n2}/{d2} في نموذج الدوائر، كم وحدة كاملة في الناتج بعد إعادة التجميع؟";
        return new Item(f,prompt,"أجمع الوحدات الكلية أولًا ثم الأجزاء الكسرية المكافئة، وأعيد تجميع وحدة إذا لزم.",
          f==G5MixedModelSum?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,p);
    }
    private static bool G5Valid(string f,IReadOnlyDictionary<string,int> p,
        out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)
    {
        w1=n1=d1=w2=n2=d2=0;
        return IsG5Model(f)&&p.Count==6&&
           p.TryGetValue("w1",out w1)&&p.TryGetValue("n1",out n1)&&
           p.TryGetValue("d1",out d1)&&p.TryGetValue("w2",out w2)&&
           p.TryGetValue("n2",out n2)&&p.TryGetValue("d2",out d2)&&
           w1 is >=1 and <=7 &&w2 is >=1 and <=7&&
           d1 is >=2 and <=9&&d2 is >=2 and <=10&&
           n1>=1&&n1<d1&&n2>=1&&n2<d2;
    }
    private static string SolveG5Model(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!G5Valid(f,p,out int w1,out int n1,out int d1,
            out int w2,out int n2,out int d2))
            throw new InvalidOperationException("Invalid G5 mixed addition model.");
        int top=(w1*d1+n1)*d2+(w2*d2+n2)*d1,den=d1*d2;
        if(f==G5MixedModelWhole)return (top/den).ToString(CultureInfo.InvariantCulture);
        int gcd=GcdG5(top,den);return $"{top/gcd}/{den/gcd}";
    }
    private static bool VerifyG5Model(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!G5Valid(f,p,out int w1,out int n1,out int d1,out int w2,out int n2,out int d2)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c)&&c is not ',' and not '٬')
          .Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),
              >= '۰' and <= '۹'=>(char)('0'+c-'۰'),'⁄'=>'/',_=>c}).ToArray());
        int n1im=w1*d1+n1,n2im=w2*d2+n2;
        if(f==G5MixedModelWhole)
            return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int whole)&&
                whole==(n1im*d2+n2im*d1)/(d1*d2);
        var parts=normalized.Split('/');
        if(parts.Length!=2||!int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)||
           top<=0||den<=0||GcdG5(top,den)!=1)return false;
        return (long)top*d1*d2==(long)den*(n1im*d2+n2im*d1);
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=5||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("whole1",out int w1)||!p.TryGetValue("whole2",out int w2)||
            !p.TryGetValue("numerator1",out int n1)||!p.TryGetValue("numerator2",out int n2)||
            d is <2 or >10||w1 is <1 or >8||w2 is <1 or >8||n1<1||n2<1||n1>=d||n2>=d)return false;
        int numerator=(w1+w2)*d+n1+n2;
        answer=family switch{Whole=>numerator/d,Fraction=>numerator%d,_=>numerator};
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Model(family))return SolveG5Model(family,p);
        if(!Valid(family,p,out int a))throw new InvalidOperationException("Invalid mixed addition parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Model(family))return VerifyG5Model(family,p,answer);
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

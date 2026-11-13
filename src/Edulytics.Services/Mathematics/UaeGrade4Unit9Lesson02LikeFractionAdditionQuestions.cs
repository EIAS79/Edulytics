using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson02LikeFractionAdditionQuestions
{
    internal const string Sum="uae.g4.u09.l02.sum_numerator";
    internal const string ReduceNum="uae.g4.u09.l02.simplified_sum_numerator";
    internal const string ReduceDen="uae.g4.u09.l02.simplified_sum_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Sum or ReduceNum or ReduceDen or G5Extended || G5Unlike(family);
    private static int Gcd(int a,int b){while(b!=0){(a,b)=(b,a%b);}return a;}

    // Grade5 extends existing exact like-fraction engine to 13–28 without
    // modifying any Grade4 family generation or grading.
    internal const string G5Extended="uae.g5.u08.l02.add_like_extended_fraction";
    internal const string G5ModelDen="uae.g5.u08.l04.model_common_denominator";
    internal const string G5ModelEquivalent="uae.g5.u08.l04.equivalent_second_numerator";
    internal const string G5ModelSum="uae.g5.u08.l04.model_unlike_sum_simplified";
    private static bool G5Unlike(string? f)=>f is G5ModelDen or G5ModelEquivalent or G5ModelSum;
    private static Item BuildG5Extended(Random rng)
    {
        int d=rng.Next(13,29),a=rng.Next(1,d),b=rng.Next(1,d);
        return new Item(G5Extended,
            $"اجمع {a}/{d} و{b}/{d}؛ اكتب الناتج في أبسط صورة (بسط/مقام).",
            "أجمع البسطين وأُثبت المقام، ثم أبسط بقسمة البسط والمقام على العامل المشترك الأكبر.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"a",a},{"b",b},{"denominator",d}});
    }
    private static bool ValidG5Extended(IReadOnlyDictionary<string,int> p,out int sum,out int d)
    {
        sum=d=0;
        if(p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("denominator",out d)||d is <13 or >28||a<1||a>=d||b<1||b>=d)return false;
        sum=a+b;return true;
    }
    private static string SolveG5Extended(IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Extended(p,out int sum,out int d))
            throw new InvalidOperationException("Invalid G5 like-fraction parameters");
        int g=Gcd(sum,d);
        return $"{sum/g}/{d/g}";
    }
    private static bool VerifyG5Extended(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Extended(p,out int sum,out int d)||string.IsNullOrWhiteSpace(answer)||
           answer.Length>40)return false;
        string s=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch!='٬')
           .Select(ch=>ch switch {
              >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
              >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
              '⁄'=>'/',_=>ch}).ToArray());
        string[] split=s.Split('/');
        if(split.Length!=2||!int.TryParse(split[0],NumberStyles.None,CultureInfo.InvariantCulture,out int n)||
           !int.TryParse(split[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)||
           n<=0||den<=0)return false;
        int x=n,y=den;
        while(y!=0){int remainder=x%y;x=y;y=remainder;}
        return x==1&&(long)n*d==(long)sum*den;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported like fraction addition");
        if(family==G5Extended)return BuildG5Extended(random);
        if(G5Unlike(family))return BuildG5Unlike(family,random);
        int d=random.Next(3,13),a=random.Next(1,d),b=random.Next(1,d);
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"denominator",d}};
        string prompt=family switch
        {
            Sum=>$"اجمع {a}/{d} + {b}/{d}. ما بسط المجموع قبل التبسيط؟",
            ReduceNum=>$"اجمع {a}/{d} + {b}/{d} ثم بسّط. ما بسط الناتج في أبسط صورة؟",
            _=>$"اجمع {a}/{d} + {b}/{d} ثم بسّط. ما مقام الناتج في أبسط صورة؟"
        };
        return new Item(family,prompt,"اجمع البسطين، احتفظ بالمقام، واقسم البسط والمقام على العامل المشترك الأكبر عند التبسيط.",
            AssessmentItemType.Numeric,p);
    }

    // Reuse Grade 4 exact fraction-addition architecture with unequal denominators.
    // No answer is persisted in generated parameters.
    private static Item BuildG5Unlike(string f,Random rng)
    {
        int d1=rng.Next(2,10),d2;
        do{d2=rng.Next(2,12);}while(d2==d1);
        int n1=rng.Next(1,d1),n2=rng.Next(1,d2),lcm=d1/Gcd(d1,d2)*d2;
        string prompt=f switch
        {
            G5ModelDen=>$"لجمع {n1}/{d1} مع {n2}/{d2} بشريطي كسور متساويين، ما المضاعف المشترك الأصغر للمقامين؟",
            G5ModelEquivalent=>$"إذا أصبح المقام المشترك لـ {n1}/{d1} + {n2}/{d2} هو {lcm}، فما بسط الكسر المكافئ للكسر الثاني؟",
            _=>$"مثّل {n1}/{d1} + {n2}/{d2} بشرائط ذات أجزاء متساوية واكتب المجموع ككسر مبسط (بسط/مقام)."
        };
        var parameters=new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}};
        return new Item(f,prompt,"أوجد المقام المشترك، واكتب الكسور المكافئة واجمع البسطين ثم بسط.",
            f==G5ModelSum?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,parameters);
    }
    private static bool G5Valid(string f,IReadOnlyDictionary<string,int> p,
        out int n1,out int d1,out int n2,out int d2,out int lcm)
    {
        n1=d1=n2=d2=lcm=0;
        if(!G5Unlike(f)||p.Count!=4||!p.TryGetValue("n1",out n1)||
           !p.TryGetValue("d1",out d1)||!p.TryGetValue("n2",out n2)||
           !p.TryGetValue("d2",out d2)||d1 is <2 or >9||d2 is <2 or >11||
           d1==d2||n1<1||n1>=d1||n2<1||n2>=d2)return false;
        lcm=d1/Gcd(d1,d2)*d2;return true;
    }
    private static string SolveG5Unlike(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!G5Valid(f,p,out int n1,out int d1,out int n2,out int d2,out int lcm))
            throw new InvalidOperationException("Invalid unlike-fraction model parameters.");
        if(f==G5ModelDen)return lcm.ToString(CultureInfo.InvariantCulture);
        if(f==G5ModelEquivalent)return (n2*lcm/d2).ToString(CultureInfo.InvariantCulture);
        int numerator=n1*(lcm/d1)+n2*(lcm/d2),g=Gcd(numerator,lcm);
        return $"{numerator/g}/{lcm/g}";
    }
    private static bool VerifyG5Unlike(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!G5Valid(f,p,out int n1,out int d1,out int n2,out int d2,out int lcm)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>45)return false;
        string response=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not '٬' and not ',')
          .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '⁄'=>'/',_=>ch}).ToArray());
        if(f!=G5ModelSum)
            return int.TryParse(response,NumberStyles.None,CultureInfo.InvariantCulture,out int v)
                &&v==(f==G5ModelDen?lcm:n2*lcm/d2);
        var pair=response.Split('/');
        if(pair.Length!=2||
           !int.TryParse(pair[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)||
           !int.TryParse(pair[1],NumberStyles.None,CultureInfo.InvariantCulture,out int den)||
           top<1||den<1||Gcd(top,den)!=1)return false;
        return (long)top*d1*d2==(long)den*(n1*d2+n2*d1);
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("denominator",out int d)||
           d is <3 or >12||a<1||a>=d||b<1||b>=d)return false;
        int sum=a+b,gcd=Gcd(sum,d);
        answer=f switch{Sum=>sum,ReduceNum=>sum/gcd,_=>d/gcd};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(f==G5Extended)return SolveG5Extended(p);
        if(G5Unlike(f))return SolveG5Unlike(f,p);
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid like addition parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(f==G5Extended)return VerifyG5Extended(p,answer);
        if(G5Unlike(f))return VerifyG5Unlike(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}

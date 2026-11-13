using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson02LikeFractionAdditionQuestions
{
    internal const string Sum="uae.g4.u09.l02.sum_numerator";
    internal const string ReduceNum="uae.g4.u09.l02.simplified_sum_numerator";
    internal const string ReduceDen="uae.g4.u09.l02.simplified_sum_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Sum or ReduceNum or ReduceDen or G5Extended;
    private static int Gcd(int a,int b){while(b!=0){(a,b)=(b,a%b);}return a;}

    // Grade5 extends existing exact like-fraction engine to 13–28 without
    // modifying any Grade4 family generation or grading.
    internal const string G5Extended="uae.g5.u08.l02.add_like_extended_fraction";
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
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid like addition parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(f==G5Extended)return VerifyG5Extended(p,answer);
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

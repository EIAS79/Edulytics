using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson02LikeFractionAdditionQuestions
{
    internal const string Sum="uae.g4.u09.l02.sum_numerator";
    internal const string ReduceNum="uae.g4.u09.l02.simplified_sum_numerator";
    internal const string ReduceDen="uae.g4.u09.l02.simplified_sum_denominator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Sum or ReduceNum or ReduceDen;
    private static int Gcd(int a,int b){while(b!=0){(a,b)=(b,a%b);}return a;}
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported like fraction addition");
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
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid like addition parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
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

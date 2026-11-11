using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson05SimplestFormQuestions
{
    internal const string ReduceNumerator="uae.g4.u08.l05.reduced_numerator";
    internal const string ReduceDenominator="uae.g4.u08.l05.reduced_denominator";
    internal const string IsSimplest="uae.g4.u08.l05.is_simplest";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is ReduceNumerator or ReduceDenominator or IsSimplest;
    private static int Gcd(int a,int b)
    {
        while(b!=0){(a,b)=(b,a%b);}
        return a;
    }
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported simplest fraction family.");
        int denominator,numerator;
        if(family==IsSimplest)
        {
            denominator=r.Next(3,39);
            numerator=r.Next(1,denominator);
        }
        else
        {
            int b=r.Next(3,13),a=r.Next(1,b),scale=r.Next(2,7);
            numerator=a*scale;
            denominator=b*scale;
        }
        var p=new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator}};
        string prompt=family switch
        {
            ReduceNumerator=>$"اكتب بسط أبسط صورة للكسر {numerator}/{denominator}.",
            ReduceDenominator=>$"اكتب مقام أبسط صورة للكسر {numerator}/{denominator}.",
            _=>$"هل الكسر {numerator}/{denominator} في أبسط صورة؟ اكتب ١ لنعم أو ٠ للا."
        };
        return new Item(family,prompt,"أوجد العامل المشترك الأكبر وقسم عليه البسط والمقام.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("numerator",out int n)||
           !p.TryGetValue("denominator",out int d)||n<1||d<2||n>=d||d>100)return false;
        int gcd=Gcd(n,d);
        answer=f switch{ReduceNumerator=>n/gcd,ReduceDenominator=>d/gcd,_=>gcd==1?1:0};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid simplest form problem");
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

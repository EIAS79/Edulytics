using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson06FractionComparisonQuestions
{
    internal const string Comparison="uae.g4.u08.l06.compare_fraction_sign";
    internal const string Smallest="uae.g4.u08.l06.select_smaller_fraction";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Comparison or Smallest || IsG5Comparison(family);

    // UAE G5 U07 L06 extends the existing exact G4 comparison engine:
    // compare proper unlike fractions and derive the least common denominator.
    internal const string G5Compare="uae.g5.u07.l06.compare_common_denominator";
    internal const string G5Denominator="uae.g5.u07.l06.find_least_common_denominator";
    internal const string G5Equivalent="uae.g5.u07.l06.equivalent_numerator";
    private static bool IsG5Comparison(string? family)=>
        family is G5Compare or G5Denominator or G5Equivalent;
    private static int GcdG5(int a,int b)
    {
        while(b!=0){int r=a%b;a=b;b=r;}return a;
    }
    private static Item BuildG5Comparison(string family,Random random)
    {
        int d1=random.Next(2,25),d2=random.Next(2,25);
        int n1=random.Next(1,d1),n2=random.Next(1,d2);
        string prompt=family switch
        {
            G5Compare=>$"قارن {n1}/{d1} و{n2}/{d2}؛ اكتب ٠ إذا كان الكسر الأول أصغر، ١ إذا تساويا، و٢ إذا كان الأول أكبر.",
            G5Denominator=>$"أوجد أصغر مقام مشترك للعددين {d1} و{d2} لمقارنة الكسرين {n1}/{d1} و{n2}/{d2}.",
            _=>$"حوّل الكسر {n1}/{d1} إلى كسر مكافئ مقامه المضاعف المشترك الأصغر للمقامين {d1} و{d2}. ما البسط الجديد؟"
        };
        return new Item(family,prompt,
            "استخدم المضاعف المشترك الأصغر للمقامين واضرب البسط والمقام في العامل نفسه ثم قارن البسطين.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}});
    }
    private static bool ValidG5Comparison(string f,IReadOnlyDictionary<string,int> p,
        out int n1,out int d1,out int n2,out int d2)
    {
        n1=d1=n2=d2=0;
        return IsG5Comparison(f)&&p.Count==4&&
            p.TryGetValue("n1",out n1)&&p.TryGetValue("d1",out d1)&&
            p.TryGetValue("n2",out n2)&&p.TryGetValue("d2",out d2)&&
            d1 is >=2 and <=24&&d2 is >=2 and <=24&&
            n1>=1&&n1<d1&&n2>=1&&n2<d2;
    }
    private static string SolveG5Comparison(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Comparison(f,p,out int n1,out int d1,out int n2,out int d2))
            throw new InvalidOperationException("Invalid G5 compare fraction parameters");
        int common=d1/GcdG5(d1,d2)*d2;
        int answer=f switch
        {
            G5Compare=>(n1*d2).CompareTo(n2*d1) switch{<0=>0,0=>1,_=>2},
            G5Denominator=>common,
            _=>n1*(common/d1)
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5Comparison(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!ValidG5Comparison(f,p,out int n1,out int d1,out int n2,out int d2)||
           string.IsNullOrWhiteSpace(response)||response.Length>32)return false;
        string normalized=new(response.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                _=>ch}).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int given))return false;
        if(f==G5Compare)
        {
            int crossLeft=n1*d2,crossRight=n2*d1;
            return given==(crossLeft<crossRight?0:crossLeft>crossRight?2:1);
        }
        // Independently enumerate multiples rather than calling the solver's gcd.
        int common=0;
        for(int candidate=1;candidate<=d1*d2;candidate++)
            if(candidate%d1==0&&candidate%d2==0){common=candidate;break;}
        return f==G5Denominator?given==common:given*d1==n1*common;
    }

    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction comparison family.");
        if(IsG5Comparison(family))return BuildG5Comparison(family,r);
        int d1=r.Next(2,11),d2=r.Next(2,11),n1=r.Next(1,d1),n2=r.Next(1,d2);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>{{"numerator1",n1},{"denominator1",d1},{"numerator2",n2},{"denominator2",d2}};
        string prompt=family==Comparison?
            $"قارن الكسرين {n1}/{d1} و{n2}/{d2}: اكتب ٠ إذا كان الأول أصغر، و١ للتساوي، و٢ إذا كان الأول أكبر.":
            $"أيّ الكسرين أصغر: {n1}/{d1} أم {n2}/{d2}؟ اكتب ١ للأول، أو ٢ للثاني، أو ٠ إذا تساويا.";
        return new Item(family,prompt,"كوّن كسورًا مكافئة بمقام مشترك أو قارن حاصلَي الضرب التبادلي.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=4||
           !p.TryGetValue("numerator1",out int n1)||!p.TryGetValue("denominator1",out int d1)||
           !p.TryGetValue("numerator2",out int n2)||!p.TryGetValue("denominator2",out int d2)||
           d1 is <2 or >10||d2 is <2 or >10||n1<1||n2<1||n1>=d1||n2>=d2)return false;
        int cmp=(n1*d2).CompareTo(n2*d1);
        answer=f==Comparison?(cmp<0?0:cmp==0?1:2):(cmp<0?1:cmp==0?0:2);
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Comparison(f))return SolveG5Comparison(f,p);
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid fraction comparison parameters.");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Comparison(f))return VerifyG5Comparison(f,p,answer);
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

using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson06FractionComparisonQuestions
{
    internal const string Comparison="uae.g4.u08.l06.compare_fraction_sign";
    internal const string Smallest="uae.g4.u08.l06.select_smaller_fraction";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Comparison or Smallest;
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction comparison family.");
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
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid fraction comparison parameters.");
        return answer.ToString(CultureInfo.InvariantCulture);
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

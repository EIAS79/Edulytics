using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson02PrimeCompositeQuestions
{
    internal const string Classification="uae.g4.u08.l02.prime_composite_classification";
    internal const string FactorCount="uae.g4.u08.l02.positive_factor_count";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Classification or FactorCount;
    private static int Count(int n)
    {
        if(n<1)return 0;
        int count=0;
        for(int i=1;i<=n;i++)
            if(n%i==0)count++;
        return count;
    }
    private static int Category(int n)
    {
        if(n<=1)return 2;
        return Count(n)==2?1:0;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported number theory family.");
        int n=family==Classification?random.Next(0,96):random.Next(2,96);
        if(family==Classification&&random.Next(0,8)==0)n=random.Next(0,2);
        string prompt=family==Classification?
            $"صنف العدد {n}: اكتب ١ للأولي، و٠ لغير الأولي، و٢ إذا لم يكن أيًا منهما.":
            $"كم عاملًا موجبًا مختلفًا للعدد {n}؟ اكتب عدد العوامل.";
        string solution=family==Classification?
            "اختبر العوامل الموجبة؛ للعدد الأولي عاملان، وغير الأولي أكثر من عاملين، و٠ و١ حالتان خاصتان.":
            "اكتب جميع القواسم الموجبة التي تقسم العدد دون باق ثم احسب عددها.";
        return new Item(family,prompt,solution,AssessmentItemType.Numeric,new Dictionary<string,int>{{"n",n}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=1||!p.TryGetValue("n",out int n)||n is <0 or >120||
           (family==FactorCount&&n<2))return false;
        answer=family==Classification?Category(n):Count(n);
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int a))throw new InvalidOperationException("Invalid prime problem.");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
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

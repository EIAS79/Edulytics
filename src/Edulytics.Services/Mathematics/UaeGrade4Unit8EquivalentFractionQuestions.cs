using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8EquivalentFractionQuestions
{
    internal const string Model="uae.g4.u08.l03.fraction_strip_equivalence";
    internal const string Line="uae.g4.u08.l03.number_line_equivalence";
    internal const string Multiply="uae.g4.u08.l04.multiply_fraction_pair";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Model or Line or Multiply || IsG5Simplest(f);

    // UAE G5 U07 L03: use G4 exact equivalent-fraction engine to
    // reduce a proper fraction by GCF. Require canonical simplest form.
    internal const string G5Simplify="uae.g5.u07.l03.simplify_fraction";
    internal const string G5Gcf="uae.g5.u07.l03.gcf_for_simplification";
    internal const string G5Numerator="uae.g5.u07.l03.reduced_numerator";
    private static bool IsG5Simplest(string? f)=>f is G5Simplify or G5Gcf or G5Numerator;
    private static int GcdG5(int a,int b)
    {
        while(b!=0){int r=a%b;a=b;b=r;}return a;
    }
    private static Item BuildG5Simplest(string f,Random random)
    {
        int n,d;
        do
        {
            d=random.Next(3,14);
            n=random.Next(1,d);
        }while(GcdG5(n,d)!=1);
        int k=random.Next(2,7),numerator=n*k,denominator=d*k;
        string prompt=f switch
        {
            G5Simplify=>$"اكتب الكسر {numerator}/{denominator} في أبسط صورة (بسط/مقام).",
            G5Gcf=>$"ما العامل المشترك الأكبر للبسط والمقام في الكسر {numerator}/{denominator}؟",
            _=>$"بسط ومقام الكسر {numerator}/{denominator} يقسمان على العامل المشترك الأكبر. ما بسط الكسر في أبسط صورة؟"
        };
        return new Item(f,prompt,
          "اقسم البسط والمقام على العامل المشترك الأكبر؛ لا تغيّر القيمة.",
          f==G5Simplify?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
          new Dictionary<string,int>{{"n",numerator},{"d",denominator},{"k",k}});
    }
    private static bool ValidG5Simplest(string f,IReadOnlyDictionary<string,int> p,
       out int n,out int d,out int k)
    {
        n=d=k=0;
        return IsG5Simplest(f)&&p.Count==3&&
          p.TryGetValue("n",out n)&&p.TryGetValue("d",out d)&&
          p.TryGetValue("k",out k)&&
          d is >=6 and <=78 && n>0&&n<d &&
          k is >=2 and <=6&&n%k==0&&d%k==0&&
          GcdG5(n,d)==k;
    }
    private static string SolveG5Simplest(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Simplest(f,p,out int n,out int d,out int k))
            throw new InvalidOperationException("Invalid G5 simplest fraction parameters");
        return f==G5Simplify?$"{n/k}/{d/k}":
               (f==G5Gcf?k:n/k).ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5Simplest(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Simplest(f,p,out int n,out int d,out int k)||
          string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c)&&c is not '٬')
           .Select(c=>c switch{
             >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
             >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
             '⁄'=>'/',_=>c
           }).ToArray());
        if(f!=G5Simplify)
            return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)
                &&actual==(f==G5Gcf?k:n/k);
        string[] parts=normalized.Split('/');
        if(parts.Length!=2||
           !int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)||
           !int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int bottom))
           return false;
        // An equivalent but unreduced answer must fail for 'simplest form'.
        return top>0&&bottom>0&&GcdG5(top,bottom)==1&&
               (long)top*d==(long)bottom*n;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction equivalence family");
        if(IsG5Simplest(family))return BuildG5Simplest(family,random);
        int denominator=random.Next(2,9);
        int numerator=random.Next(1,denominator);
        int factor=random.Next(2,6);
        string text=family switch
        {
            Model=>$"يمثل شريط الكسر {numerator}/{denominator}. عند تقسيم كل جزء إلى {factor} أجزاء متساوية، كم جزءًا ملونًا في الشريط الجديد؟",
            Line=>$"تقع النقطة {numerator}/{denominator} على خط من ٠ إلى ١. إذا قسمنا الخط إلى {denominator*factor} جزءًا، عند أي قسم تقع النقطة؟",
            _=>$"أكمل الكسر المكافئ: {numerator}/{denominator} = □/{denominator*factor}. ما قيمة البسط المجهول؟"
        };
        return new Item(family,text,"اضرب البسط والمقام في العدد نفسه ثم تحقق من تساوي المقدارين.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator},{"factor",factor}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||
            !p.TryGetValue("numerator",out int n)||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("factor",out int f)||d is <2 or >8||n<1||n>=d||f is <2 or >5)return false;
        answer=n*f;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Simplest(family))return SolveG5Simplest(family,p);
        if(!Valid(family,p,out int ans))throw new InvalidOperationException("Invalid fraction equivalence parameters");
        return ans.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Simplest(family))return VerifyG5Simplest(family,p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}

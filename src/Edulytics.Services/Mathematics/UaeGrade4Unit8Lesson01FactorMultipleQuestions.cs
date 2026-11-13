using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson01FactorMultipleQuestions
{
    internal const string MissingFactor="uae.g4.u08.l01.missing_factor";
    internal const string IsMultiple="uae.g4.u08.l01.is_multiple";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is MissingFactor or IsMultiple || IsG5Gcf(f);

 // UAE G5 U07 L02: GCF from two or three sets, reuse G4 factor engine.
 internal const string G5GcfPair="uae.g5.u07.l02.gcf_two_numbers";
 internal const string G5GcfTriple="uae.g5.u07.l02.gcf_three_numbers";
 internal const string G5Common="uae.g5.u07.l02.test_common_factor";
 private static bool IsG5Gcf(string? f)=>f is G5GcfPair or G5GcfTriple or G5Common;
 private static Item BuildG5Gcf(string f,Random random)
 {
    int g=random.Next(2,11),a=g*random.Next(2,12),b=g*random.Next(2,12),c=g*random.Next(2,12);
    if(f==G5Common)
    {
       int candidate=random.Next(2,16);
       var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",candidate}};
       return new Item(f,$"هل العدد {candidate} عامل مشترك للعددين {a} و{b}؟ اكتب ١ لنعم و٠ للا.",
          "العدد عامل مشترك إذا قسم كل عدد دون باق.",AssessmentItemType.Numeric,p);
    }
    return new Item(f,f==G5GcfPair?
       $"أوجد العامل المشترك الأكبر للعددين {a} و{b}.":
       $"أوجد العامل المشترك الأكبر للأعداد {a} و{b} و{c}.",
       "كوّن قائمة العوامل أو حلل الأعداد لعوامل أولية ثم خذ العوامل المشتركة بأقل تكرار.",
       AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
 }
 private static bool ValidG5Gcf(string f,IReadOnlyDictionary<string,int> p,out int expected)
 {
    expected=0;
    if(!IsG5Gcf(f)||p.Count!=3||
       !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
       !p.TryGetValue("c",out int c)||a is <2 or >120||b is <2 or >120||
       c is <2 or >120)return false;
    if(f==G5Common)
    {
       if(c>15)return false;
       expected=a%c==0&&b%c==0?1:0;return true;
    }
    int gcd=GcdG5(a,b);
    expected=f==G5GcfTriple?GcdG5(gcd,c):gcd;
    return true;
 }
 private static int GcdG5(int a,int b)
 {
    while(b!=0){int r=a%b;a=b;b=r;}
    return a;
 }
 private static string SolveG5Gcf(string f,IReadOnlyDictionary<string,int> p)
 {
    if(!ValidG5Gcf(f,p,out int ans))throw new InvalidOperationException("Invalid Grade5 GCF parameters");
    return ans.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Gcf(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
    if(!ValidG5Gcf(f,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
    string s=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
       .Select(ch=>ch switch{
           >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),_=>ch
       }).ToArray());
    if(!int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int v))return false;
    int a=p["a"],b=p["b"],c=p["c"];
    if(f==G5Common)return v==(a%c==0&&b%c==0?1:0);
    int max=f==G5GcfTriple?Math.Min(a,Math.Min(b,c)):Math.Min(a,b);
    for(int i=max;i>=1;i--)if(a%i==0&&b%i==0&&(f!=G5GcfTriple||c%i==0))
       return v==i;
    return false;
 }

    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported factor family");
        if(IsG5Gcf(f))return BuildG5Gcf(f,random);
        int a=random.Next(2,13),b=random.Next(2,13),offset=random.Next(0,2);
        IReadOnlyDictionary<string,int> parameters=f==MissingFactor?
            new Dictionary<string,int>{{"a",a},{"b",b},{"offset",0}}:
            new Dictionary<string,int>{{"a",a},{"b",b},{"offset",offset}};
        string prompt=f==MissingFactor?
            $"أكمل زوج العوامل: {a*b} = {a} × □. اكتب العدد الناقص.":
            $"هل العدد {a*b+offset} من مضاعفات {a}؟ اكتب ١ لنعم أو ٠ للا.";
        string solution=f==MissingFactor?
            $"اقسم {a*b} على {a} لتحصل على العامل الآخر.":
            $"أوجد باقي قسمة {a*b+offset} على {a}؛ إذا كان صفرًا فالعدد مضاعف.";
        return new Item(f,prompt,solution,AssessmentItemType.Numeric,parameters);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||
            !p.TryGetValue("b",out int b)||!p.TryGetValue("offset",out int offset)||
            a is <2 or >12 || b is <2 or >12 || offset is <0 or >1 ||
            (f==MissingFactor && offset!=0))return false;
        answer=f==MissingFactor?b:offset==0?1:0;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5Gcf(f))return SolveG5Gcf(f,p);
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5Gcf(f))return VerifyG5Gcf(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        var value=new string(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}

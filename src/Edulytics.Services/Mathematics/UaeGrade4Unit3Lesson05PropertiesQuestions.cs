using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U03 L05: commutative, identity, zero, and division rules.
internal static class UaeGrade4Unit3Lesson05PropertiesQuestions
{
    internal const string Swap="uae.g4.u03.l05.commutative_missing_factor";
    internal const string G5Swap="uae.g5.u05.l08.commutative_decimal_factor";
    internal const string G5Group="uae.g5.u05.l08.associative_decimal_product";
    internal const string G5Identity="uae.g5.u05.l08.multiplicative_identity";
    private static bool IsG5(string? f)=>f is G5Swap or G5Group or G5Identity;
    internal const string Identity="uae.g4.u03.l05.identity";
    internal const string Zero="uae.g4.u03.l05.zero";
    internal const string DivideOne="uae.g4.u03.l05.divide_by_one";
    internal const string DivideSelf="uae.g4.u03.l05.divide_by_self";
    internal const string ZeroDividend="uae.g4.u03.l05.zero_dividend";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,
        IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Swap or Identity or Zero or DivideOne or DivideSelf or ZeroDividend || IsG5(f);
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 arithmetic property");
        if(IsG5(f))return BuildG5(f,rng);
        int a=rng.Next(2,30),b=rng.Next(2,13);
        string prompt=f switch
        {
            Swap=>$"أكمل الجملة باستخدام خاصية التبديل: {a} × {b} = □ × {a}.",
            Identity=>$"أكمل باستخدام خاصية المحايد: {a} × ١ = □.",
            Zero=>$"أوجد الناتج: {a} × ٠ = □.",
            DivideOne=>$"أكمل قاعدة العدد ١: {a} ÷ ١ = □.",
            DivideSelf=>$"أكمل: {a} ÷ {a} = □.",
            _=>$"أكمل قاعدة الصفر في القسمة: ٠ ÷ {a} = □."
        };
        return new Item(f,prompt,"أحدد الخاصية أو القاعدة ثم أعوض في الجملة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }

    // Reuse Grade 4 multiplication-property generator with decimal-scale G5 cases.
    private static Item BuildG5(string f,Random random)
    {
        int a=random.Next(11,901),b=random.Next(2,21),c=random.Next(2,11);
        string x=(a/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string prompt=f switch
        {
            G5Swap=>$"أكمل خاصية التبديل: {x} × □ = {b} × {x}.",
            G5Group=>$"أوجد ({x} × {b}) × {c} باستعمال خاصية التجميع.",
            _=>$"أكمل خاصية المحايد: {x} × □ = {x}."
        };
        return new Item(f,prompt,"أستعمل التبديل أو التجميع أو المحايد دون تغيير حاصل الضرب.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"aTenths",a},{"b",b},{"c",c}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,
        out decimal expected)
    {
        expected=0m;
        if(!IsG5(f)||p.Count!=3||!p.TryGetValue("aTenths",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           a is <11 or >900||b is <2 or >20||c is <2 or >10)return false;
        expected=f switch {G5Swap=>b,G5Group=>(decimal)a*b*c/10m,_=>1m};
        return true;
    }
    private static string SolveG5(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5(f,p,out decimal expected))
            throw new InvalidOperationException("Invalid G5 multiplication-property parameters");
        return expected.ToString("0.###",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(ch=>ch is >= '0' and <= '9' or '.'))
            return false;
        if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal provided))return false;
        int a=p["aTenths"],b=p["b"],c=p["c"];
        return f switch{
            G5Swap=>provided==b,
            G5Group=>provided*10m==(decimal)a*b*c,
            _=>provided==1m
        };
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <2 or >=30||b is <2 or >=13)return false;
        value=f switch {Swap=>b,Identity=>a,Zero=>0,DivideOne=>a,DivideSelf=>1,_=>0};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5(f))return SolveG5(f,p);
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid arithmetic rule parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5(f))return VerifyG5(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int n)&&n==expected;
    }
}

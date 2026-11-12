using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U03 L05: commutative, identity, zero, and division rules.
internal static class UaeGrade4Unit3Lesson05PropertiesQuestions
{
    internal const string Swap="uae.g4.u03.l05.commutative_missing_factor";
    internal const string Identity="uae.g4.u03.l05.identity";
    internal const string Zero="uae.g4.u03.l05.zero";
    internal const string DivideOne="uae.g4.u03.l05.divide_by_one";
    internal const string DivideSelf="uae.g4.u03.l05.divide_by_self";
    internal const string ZeroDividend="uae.g4.u03.l05.zero_dividend";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,
        IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Swap or Identity or Zero or DivideOne or DivideSelf or ZeroDividend;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 arithmetic property");
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
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid arithmetic rule parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int n)&&n==expected;
    }
}

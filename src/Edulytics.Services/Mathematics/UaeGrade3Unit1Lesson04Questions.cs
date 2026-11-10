using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// First-party UAE G3 U01 L04; exact engine extension, not a new engine.
internal static class UaeGrade3Unit1Lesson04Questions
{
    internal const string Family="uae.g3.u01.l04.round_nearest_ten";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Family;
    public static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G3 rounding-ten family");
        int n=rng.Next(1,10000);
        int rounded=((n+5)/10)*10;
        int mode=rng.Next(0,2);
        int lower=n/10*10,upper=lower+10;
        string prompt=mode==0?$"قرّب العدد {n} إلى أقرب عشرة."
            :$"على خط الأعداد بين {lower} و{upper} يقع العدد {n}. إلى أي عشرة أقرب؟";
        var pars=new Dictionary<string,int>{{"number",n},{"mode",mode},{"result",rounded}};
        return new Item(f,prompt,"أحدد الآحاد وأقرب من ٠ إلى ٤ إلى العشرة الأدنى، ومن ٥ إلى ٩ إلى الأعلى.",
            AssessmentItemType.Numeric,pars);
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int rounded)
    {
        rounded=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("number",out int n)||n is <1 or >9999||
            !p.TryGetValue("mode",out int m)||m is <0 or >1||
            !p.TryGetValue("result",out int r))return false;
        rounded=((n+5)/10)*10;
        return r==rounded;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid G3 rounding-ten parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out var value)||string.IsNullOrWhiteSpace(answer))return false;
        var t=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.Integer,CultureInfo.InvariantCulture,out var n)&&n==value;
    }
}

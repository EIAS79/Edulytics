using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade3 Lesson5 reuses existing exact math engine, a single bounded question family.
internal static class UaeGrade3Unit1Lesson05Questions
{
    internal const string Family="uae.g3.u01.l05.round_nearest_hundred";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown Grade3 rounding-hundred family");
        int n=random.Next(1,10000);
        int rounded=(n+50)/100*100,mode=random.Next(0,2);
        int low=n/100*100;
        var prompt=mode==0?$"قرّب العدد {n} إلى أقرب مئة."
            :$"على خط الأعداد بين {low} و{low+100} يقع {n}. ما أقرب مئة له؟";
        return new Item(family,prompt,
            "أفحص العشرات: ٠–٤ أبقى في المئة الأدنى، و٥–٩ أتجه إلى الأعلى.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"number",n},{"mode",mode},{"result",rounded}});
    }
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("number",out int n)||n is <1 or >9999||
           !p.TryGetValue("mode",out int mode)||mode is <0 or >1||
           !p.TryGetValue("result",out int result))return false;
        value=(n+50)/100*100;
        return result==value;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> parameters)
    {
        if(!Valid(family,parameters,out int v))throw new InvalidOperationException("Invalid G3 nearest-hundred parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string answer)
    {
        if(!Valid(family,parameters,out int v)||string.IsNullOrWhiteSpace(answer))return false;
        string digits=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
           c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out int entered)&&entered==v;
    }
}

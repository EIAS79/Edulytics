using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extension to existing exact Practice architecture for UAE G5 Unit 9; G4 unaffected.
internal static class UaeGrade5Unit9UnitFractionDivisionQuestions
{
    internal const string WholeByUnit="uae.g5.u09.l09.whole_divide_unit_fraction";
    internal const string UnitByWhole="uae.g5.u09.l09.unit_fraction_divide_whole";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is WholeByUnit or UnitByWhole;
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported unit fraction division.");
        int n=r.Next(2,11),d=r.Next(2,11);
        var p=new Dictionary<string,int>{{"n",n},{"d",d}};
        string prompt=family==WholeByUnit
            ? $"كم كسر وحدة من فئة ١/{d} يوجد في {n} وحدات كاملة؟ أوجد {n} ÷ ١/{d}."
            : $"إذا قُسم كسر الوحدة ١/{d} إلى {n} مجموعات متساوية، فما نصيب المجموعة الواحدة؟ اكتب كسرًا مبسطًا بسط/مقام.";
        return new Item(family,prompt,"أعد الأجزاء أو قسم كسر الوحدة برقائق كسور متساوية، ثم تحقق بالضرب.",
            family==WholeByUnit?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int n,out int d)
    {
        n=d=0;
        return Supports(family)&&p.Count==2&&p.TryGetValue("n",out n)&&n is >=2 and <=10&&
            p.TryGetValue("d",out d)&&d is >=2 and <=10;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int n,out int d))throw new InvalidOperationException("Invalid G5 fraction division.");
        return family==WholeByUnit?(n*d).ToString(CultureInfo.InvariantCulture):$"1/{n*d}";
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int n,out int d)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        string normalized=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c)&&c is not ',' and not '٬')
            .Select(c=>c switch {>= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),'⁄'=>'/',_=>c}).ToArray());
        if(family==WholeByUnit)
            return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int v)&&v==n*d;
        string[] pieces=normalized.Split('/');
        return pieces.Length==2&&int.TryParse(pieces[0],NumberStyles.None,CultureInfo.InvariantCulture,out int top)&&
            int.TryParse(pieces[1],NumberStyles.None,CultureInfo.InvariantCulture,out int bottom)&&top==1&&bottom==n*d;
    }
}
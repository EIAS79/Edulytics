using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Extension to existing exact Practice architecture for UAE G5 Unit 9; G4 unaffected.
internal static class UaeGrade5Unit9UnitFractionDivisionQuestions
{
    internal const string WholeByUnit="uae.g5.u09.l09.whole_divide_unit_fraction";
    internal const string UnitByWhole="uae.g5.u09.l09.unit_fraction_divide_whole";
    internal const string G6WholeByFraction="uae.g6.u04.l07.whole_divide_nonunit_fraction";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is WholeByUnit or UnitByWhole or G6WholeByFraction;
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported unit fraction division.");
        if(family==G6WholeByFraction)return BuildG6(r);
        int n=r.Next(2,11),d=r.Next(2,11);
        var p=new Dictionary<string,int>{{"n",n},{"d",d}};
        string prompt=family==WholeByUnit
            ? $"كم كسر وحدة من فئة ١/{d} يوجد في {n} وحدات كاملة؟ أوجد {n} ÷ ١/{d}."
            : $"إذا قُسم كسر الوحدة ١/{d} إلى {n} مجموعات متساوية، فما نصيب المجموعة الواحدة؟ اكتب كسرًا مبسطًا بسط/مقام.";
        return new Item(family,prompt,"أعد الأجزاء أو قسم كسر الوحدة برقائق كسور متساوية، ثم تحقق بالضرب.",
            family==WholeByUnit?AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,p);
    }

    // Grade6 scoped whole ÷ non-unit proper fraction; G5 behavior preserved.
    private static Item BuildG6(Random r)
    {
        int n=r.Next(2,11),d=r.Next(3,12),a=r.Next(2,d);
        return new Item(G6WholeByFraction,
            $"أوجد {n} ÷ {a}/{d}. اكتب عدد المجموعات كعدد صحيح أو كسر مبسط (بسط/مقام).",
            "قسّم الوحدات إلى أجزاء متساوية، واجمع الأجزاء بحسب بسط المجموعة؛ تحقق بالضرب.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"n",n},{"d",d},{"a",a}});
    }
    private static int Gcd(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return a;}
    private static bool TryG6(IReadOnlyDictionary<string,int>? p,out int numerator,out int denominator)
    {
        numerator=denominator=0;
        if(p is null||p.Count!=3||!p.TryGetValue("n",out int n)||
           !p.TryGetValue("d",out int d)||!p.TryGetValue("a",out int a)||
           n is <2 or >10||d is <3 or >11||a<2||a>=d)return false;
        int top=checked(n*d),g=Gcd(top,a);numerator=top/g;denominator=a/g;return true;
    }
    private static string SolveG6(IReadOnlyDictionary<string,int> p)
    {
        if(!TryG6(p,out int n,out int d))throw new InvalidOperationException("Invalid G6 fraction division");
        return d==1?n.ToString(CultureInfo.InvariantCulture):$"{n}/{d}";
    }
    private static bool VerifyG6(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryG6(p,out int n,out int d)||string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),'⁄'=>'/',_=>ch}).ToArray());
        string[] parts=normalized.Split('/');
        if(d==1&&parts.Length==1)return int.TryParse(parts[0],
            NumberStyles.None,CultureInfo.InvariantCulture,out int x)&&x==n;
        return parts.Length==2&&int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int a)&&
            int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int b)&&
            a>0&&b>0&&Gcd(a,b)==1&&a==n&&b==d;
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int n,out int d)
    {
        n=d=0;
        return (family==WholeByUnit||family==UnitByWhole)&&p.Count==2&&p.TryGetValue("n",out n)&&n is >=2 and <=10&&
            p.TryGetValue("d",out d)&&d is >=2 and <=10;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==G6WholeByFraction)return SolveG6(p);
        if(!Valid(family,p,out int n,out int d))throw new InvalidOperationException("Invalid G5 fraction division.");
        return family==WholeByUnit?(n*d).ToString(CultureInfo.InvariantCulture):$"1/{n*d}";
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family==G6WholeByFraction)return VerifyG6(p,answer);
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
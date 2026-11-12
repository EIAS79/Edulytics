using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Extends existing exact rounding architecture to whole numbers up to millions.
internal static class UaeGrade4Unit1RoundingQuestions
{
    internal const string Prefix="uae.g4.u01.l05.round_nearest_";
    private static readonly Dictionary<string,(int Base,string ArabicName)> Families=new()
    {
        [Prefix+"ten"]=(10,"عشرة"),
        [Prefix+"hundred"]=(100,"مئة"),
        [Prefix+"thousand"]=(1000,"ألف"),
        [Prefix+"ten_thousand"]=(10000,"عشرة آلاف"),
        [Prefix+"hundred_thousand"]=(100000,"مئة ألف"),
        [Prefix+"million"]=(1000000,"مليون")
    };
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    internal static bool Supports(string? family)=>family is not null && Families.ContainsKey(family);

    internal static Item Build(string family,Random rng)
    {
        if(!Families.TryGetValue(family,out var target))
            throw new InvalidOperationException("Unknown G4 rounding family");
        int n=rng.Next(1_000,10_000_000);
        return new Item(family,
            $"قرّب العدد {n} إلى أقرب {target.ArabicName}.",
            "حدد المنزلة المطلوبة ثم قارن مع مضاعفَي منزلة التقريب باستخدام منتصف المسافة.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"n",n}});
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Families.TryGetValue(family,out var target)||p.Count!=1||
           !p.TryGetValue("n",out int n)||n<1_000||n>9_999_999)return false;
        result=checked(((n+target.Base/2)/target.Base)*target.Base);
        return true;
    }

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int result))
            throw new InvalidOperationException("Invalid G4 rounding parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string t=new(answer.Trim().Where(c=>c!='٬'&&c!=','&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                        c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.None,CultureInfo.InvariantCulture,out int entered)
            &&entered==expected;
    }
}

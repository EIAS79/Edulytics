using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Grade 9 General rational exponent exact perfect-power subset.
/// Irrational and signed-base free explanations remain teacher reviewed.</summary>
internal static class UaeG9RationalExponentQuestions
{
    internal const string Positive="uae.g9.general.u08.l03.rational_positive_exact";
    internal const string Negative="uae.g9.general.u08.l03.rational_negative_exact";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    internal static bool Supports(string? family) => family is Positive or Negative;

    private static int Power(int value,int exponent)
    {
        var result=1;
        for(int i=0;i<exponent;i++)result=checked(result*value);
        return result;
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p) =>
        Supports(family) && p.Count==3 && p.TryGetValue("root",out int root) &&
        p.TryGetValue("numerator",out int numerator) &&
        p.TryGetValue("denominator",out int denominator) &&
        root is >= 2 and <= 5 && numerator is >= 1 and <= 3 && denominator is >= 2 and <= 4;

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Invalid G9 rational exponent family");
        ArgumentNullException.ThrowIfNull(random);
        int root=random.Next(2,6),numerator=random.Next(1,4),denominator=random.Next(2,5);
        var p=new Dictionary<string,int>{{"root",root},{"numerator",numerator},{"denominator",denominator}};
        var baseValue=Power(root,denominator);
        var sign=family==Negative?"−":"";
        return new Item(family,$"احسب {baseValue}^({sign}{numerator}/{denominator}) باستخدام الجذر النوني ثم القوة. اكتب الإجابة الدقيقة.",
            "نحسب الجذر النوني للعدد ثم نرفعه إلى قوة البسط؛ إذا كان الأس سالبًا نأخذ المقلوب.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G9 rational exponent parameters.");
        var n=Power(p["root"],p["numerator"]);
        return family==Negative?FormattableString.Invariant($"1/{n}"):n.ToString(CultureInfo.InvariantCulture);
    }
    private static string Normalize(string? input) =>
        new string((input??"").Trim().Replace('−','-').Replace('–','-').Replace('٫','.')
            .Where(c=>!char.IsWhiteSpace(c)&&c is not '\u200E' and not '\u200F' and not '\u061C')
            .Select(c=>c switch{>= '٠' and <= '٩'=>(char)('0'+c-'٠'),>= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c}).ToArray());
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>60)return false;
        var value=Normalize(answer);
        if(string.Equals(value,Solve(family,p),StringComparison.Ordinal))return true;
        var expected=Power(p["root"],p["numerator"]);
        return family==Negative && decimal.TryParse(value,NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out var numeric) && numeric==1m/expected;
    }
}

using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Arabic UAE Grade 9 General U02L05 exact absolute-value equations.
/// Only objectively gradable numerical cases are digitized; graph drawings,
/// modelling explanations and open responses remain teacher reviewed.
/// </summary>
internal static class UaeG9AbsoluteValueEquationQuestions
{
    internal const string Two = "uae.g9.general.u02.l05.two_solutions";
    internal const string Zero = "uae.g9.general.u02.l05.one_solution";
    internal const string None = "uae.g9.general.u02.l05.no_solution";
    internal const string Evaluate = "uae.g9.general.u02.l05.evaluate_absolute";
    internal const string Center = "uae.g9.general.u02.l05.midpoint";
    internal const string Tolerance = "uae.g9.general.u02.l05.context_tolerance_endpoints";
    internal const string Composite = "uae.g9.general.u02.l05.composite_evaluation";
    internal const string OuterImpossible = "uae.g9.general.u02.l05.outer_coefficient_no_solution";
    internal sealed record Item(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) =>
        family is Two or Zero or None or Evaluate or Center or Tolerance or Composite or OuterImpossible;

    private static bool Valid(string family, IReadOnlyDictionary<string, int>? p)
    {
        if (!Supports(family) || p is null || p.Count != 3 ||
            !p.TryGetValue("a", out var a) ||
            !p.TryGetValue("m", out var m) ||
            !p.TryGetValue("r", out var r))
            return false;
        return a is >= 1 and <= 6 && m is >= -20 and <= 20 &&
            r is >= 1 and <= 15;
    }

    private static string Normalized(string? raw)
    {
        var text = (raw ?? "").Trim().Replace("−", "-").Replace("–", "-")
            .Replace('،', ',').Replace('؛', ',').Replace(';', ',').Replace('٫', '.');
        var chars = text.Where(c => !char.IsWhiteSpace(c) && c is not '\u200E' and not '\u200F' and not '\u061C')
            .Select(c => c switch
            {
                >= '٠' and <= '٩' => (char)('0' + c - '٠'),
                >= '۰' and <= '۹' => (char)('0' + c - '۰'),
                _ => c
            }).ToArray();
        return new string(chars);
    }

    private static bool TryExpected(string family, IReadOnlyDictionary<string,int> p,
        out string expected)
    {
        expected = "";
        if (!Valid(family, p)) return false;
        var a=p["a"]; var m=p["m"]; var r=p["r"];
        expected=family switch
        {
            Two or Tolerance => FormattableString.Invariant($"{m-r},{m+r}"),
            Zero => m.ToString(CultureInfo.InvariantCulture),
            None => "لايوجدحل",
            Evaluate => (a * r).ToString(CultureInfo.InvariantCulture),
            Center => m.ToString(CultureInfo.InvariantCulture),
            Composite => (a*r-r-3).ToString(CultureInfo.InvariantCulture),
            OuterImpossible => "لايوجدحل",
            _ => ""
        };
        return true;
    }

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G9 absolute-value family.");
        ArgumentNullException.ThrowIfNull(random);
        int a=random.Next(1,7), m=random.Next(-20,21), r=random.Next(1,16);
        Dictionary<string,int> p=new() { ["a"]=a, ["m"]=m, ["r"]=r };
        int b=-a*m, target=a*r;
        string prompt=family switch
        {
            Two => TwoSolutionPrompt(a, b, target, random),
            Tolerance => $"درجة الحرارة المطلوبة {m} درجة مئوية، ويُسمح باختلاف مقداره {r} درجات بالضبط. ما درجتا الحرارة اللتان تبعدان هذه المسافة عن القيمة المطلوبة؟ اكتب القيمتين بفاصلة.",
            Zero => $"حل المعادلة |{a}س{Signed(b)}|=0. اكتب قيمة س.",
            None => $"حل المعادلة |{a}س{Signed(b)}|=−{target}. إذا لم يوجد حل، اكتب: لا يوجد حل.",
            Evaluate => $"إذا كانت س={m+r}، فأوجد قيمة |{a}س{Signed(b)}|.",
            Composite => CompositePrompt(a, b, m, r, random),
            OuterImpossible => $"حل المعادلة {a}−{r}|س{Signed(-m)}|={a+r}. إذا لم يوجد حل، اكتب: لا يوجد حل.",
            _ => $"نقطتان على خط الأعداد عند {m-r} و{m+r}. ما منتصفهما (مركز معادلة القيمة المطلقة)؟"
        };
        return new Item(family,prompt,
            "القيمة المطلقة مسافة غير سالبة؛ أعزلها ثم أتحقق من حالتي الموجب والسالب، أو حالة الصفر أو انعدام الحل.",
            AssessmentItemType.ShortAnswer,p);
    }

    // The independent verifier still receives a,m,r and checks both roots.
    // Positive external multipliers and rational internal coefficients preserve
    // the exact same roots; generating their source-like forms does not create a
    // duplicate engine or make human-reviewed proofs automatically gradable.
    private static string TwoSolutionPrompt(int a, int b, int target, Random random)
    {
        const string instruction = " اكتب الحلين تصاعديًا مفصولين بفاصلة.";
        return random.Next(4) switch
        {
            0 => $"حل المعادلة |{a}س{Signed(b)}|={target}." + instruction,
            1 => $"حل المعادلة ٢|{a}س{Signed(b)}|={2 * target}." + instruction,
            2 => $"حل المعادلة |({a}س{Signed(b)})/٢|={target}/٢." + instruction,
            _ => $"حل المعادلة {target}/|{a}س{Signed(b)}|+{a}={a+1}. شرط التعريف: {a}س{Signed(b)}≠٠." + instruction
        };
    }

    private static string CompositePrompt(int a, int b, int m, int r, Random random)
    {
        // Equivalent source-skill variants, all evaluating to a*r-r-3.
        // Independent verification remains based solely on the immutable parameters.
        return random.Next(5) switch
        {
            0 => $"إذا كانت س={m+r}، احسب |{a}س{Signed(b)}|−{r+3}.",
            1 => $"إذا كانت س={m+r} وص=−{r}، احسب |{a}س{Signed(b)}|−|ص|−٣.",
            2 => $"إذا كانت س={m+r} وص={r}، احسب ٢|{a}س{Signed(b)}|−|{a}س{Signed(b)}|−|ص|−٣.",
            3 => $"إذا كانت س={m+r} وص={r}، احسب |{a}س{Signed(b)}|+|ص|−{2*r+3}.",
            _ => $"إذا كانت س={m+r}، احسب −|{a}س{Signed(b)}|{Signed(2*a*r-r-3)}."
        };
    }

    private static string Signed(int n) => n<0 ? n.ToString(CultureInfo.InvariantCulture) :
        "+"+n.ToString(CultureInfo.InvariantCulture);

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out var expected)?expected:
        throw new InvalidOperationException("Invalid absolute-value question parameters.");

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer) || answer.Length > 80 ||
            !TryExpected(family,p,out var expected)) return false;
        var actual=Normalized(answer);
        if (family is None or OuterImpossible) return actual is "لايوجدحل" or "لاحل" or "لامجموعةحل" or "مجموعةالحلخالية" or "∅" or "{}";
        if (family is Two or Tolerance)
        {
            // Do not silently accept missing answers such as "1,9," or ",1,9".
            var values=actual.Split(',');
            if (values.Length!=2 || !decimal.TryParse(values[0],NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out var first) ||
                !decimal.TryParse(values[1],NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out var second)) return false;
            // Accept either ordering; reject duplicates and extraneous entries.
            return first!=second &&
                ((first==p["m"]-p["r"] && second==p["m"]+p["r"]) ||
                 (second==p["m"]-p["r"] && first==p["m"]+p["r"]));
        }
        // Equivalent representations (+3, 03, 3.0, and Arabic-Indic decimal digits)
        // must receive the same grade for a mathematically identical integer answer.
        return decimal.TryParse(actual, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out var numeric) &&
            decimal.TryParse(expected, NumberStyles.Integer, CultureInfo.InvariantCulture,
                out var target) && numeric == target;
    }
}

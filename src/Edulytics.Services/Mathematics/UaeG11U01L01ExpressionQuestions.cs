using System.Globalization;
using System.Text;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Bounded Grade 11 extension of the existing exact substitution
/// mechanism; no new engine or production routing. No general CAS.
/// All parameters are integers (decimal values are stored as tenths).</summary>
internal static class UaeG11U01L01ExpressionQuestions
{
    internal const string Signed = "uae.g11.unified.u01.l01.signed_substitution";
    internal const string Decimal = "uae.g11.unified.u01.l01.decimal_substitution";
    internal const string Rational = "uae.g11.unified.u01.l01.rational_substitution";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) => family is Signed or Decimal or Rational;

    private static bool TryParameters(string family, IReadOnlyDictionary<string, int>? p,
        out int a, out int b, out int c)
    {
        a = b = c = 0;
        if (!Supports(family) || p is null || p.Count != 3 ||
            !p.TryGetValue("a", out a) || !p.TryGetValue("b", out b) || !p.TryGetValue("c", out c))
            return false;
        if (family == Decimal)
            return a is >= -50 and <= 50 && b is >= -5 and <= 5 && c is >= -50 and <= 50;
        return a is >= -9 and <= 9 && b is >= -9 and <= 9 && c is >= -9 and <= 9
            && (family != Rational || c != a);
    }

    private static string Number(decimal value) => value.ToString("0.##########", CultureInfo.InvariantCulture);

    private static long Gcd(long a, long b)
    {
        a = Math.Abs(a); b = Math.Abs(b);
        while (b != 0) { long rem = a % b; a = b; b = rem; }
        return a;
    }

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unsupported G11 expression family.");
        ArgumentNullException.ThrowIfNull(random);
        int a, b, c;
        if (family == Decimal)
        {
            a = random.Next(-50, 51); b = random.Next(-5, 6); c = random.Next(-50, 51);
        }
        else
        {
            a = random.Next(-9, 10); b = random.Next(-9, 10);
            do c = random.Next(-9, 10); while (family == Rational && c == a);
        }
        string prompt = family switch
        {
            Signed => $"إذا كان أ={a}، ب={b}، ج={c}، أوجد قيمة أ+(ب−ج)². اكتب الناتج العددي.",
            Decimal => $"إذا كان أ={Number(a / 10m)}، ب={b}، ج={Number(c / 10m)}، احسب أ+ب²(ج−أ). اكتب الناتج العشري الدقيق.",
            _ => $"إذا كان أ={a}، ب={b}، ج={c}، احسب (أ²−٢ب)/(ج−أ). اكتب عددًا صحيحًا أو كسرًا دقيقًا."
        };
        return new Item(family, prompt,
            "عوّض بالقيم مع وضع السالب بين قوسين، وطبّق ترتيب العمليات. افحص المقام قبل القسمة.",
            family == Rational ? AssessmentItemType.ShortAnswer : AssessmentItemType.Numeric,
            new Dictionary<string, int> { ["a"] = a, ["b"] = b, ["c"] = c });
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!TryParameters(family, p, out int a, out int b, out int c))
            throw new InvalidOperationException("Invalid bounded G11 substitution parameters.");
        if (family == Signed) return Number(a + (b-c)*(b-c));
        if (family == Decimal) return Number(a / 10m + b*b*(c / 10m - a / 10m));
        long n = a*a - 2*b, den = c-a;
        if (den < 0) { n = -n; den = -den; }
        long g = Gcd(n, den);
        n /= g; den /= g;
        return den == 1 ? n.ToString(CultureInfo.InvariantCulture) :
            $"{n.ToString(CultureInfo.InvariantCulture)}/{den.ToString(CultureInfo.InvariantCulture)}";
    }

    private static string Normalize(string response)
    {
        var sb = new StringBuilder(response.Length);
        foreach (char ch in response.Trim())
            sb.Append(ch switch {
                >= '٠' and <= '٩' => (char)('0' + ch - '٠'),
                >= '۰' and <= '۹' => (char)('0' + ch - '۰'),
                '−' or '–' => '-', '٫' => '.', '⁄' => '/', _ => ch
            });
        return sb.ToString();
    }

    private static bool TryResponse(string response, bool fractions, out decimal n, out decimal den)
    {
        n = 0; den = 1;
        string s = Normalize(response);
        const NumberStyles numeric = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
        if (s.Contains('/'))
        {
            if (!fractions) return false;
            string[] parts = s.Split('/');
            if (parts.Length != 2 || !long.TryParse(parts[0], NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out long num) ||
                !long.TryParse(parts[1], NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture, out long divisor) || divisor == 0 ||
                num is < -1000000000L or > 1000000000L || divisor is < -1000000000L or > 1000000000L) return false;
            n = num; den = divisor;
            return true;
        }
        if (!decimal.TryParse(s, numeric, CultureInfo.InvariantCulture, out decimal val)
            || val is < -1000000000m or > 1000000000m) return false;
        n = val; return true;
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? response)
    {
        if (!TryParameters(family, p, out int a, out int b, out int c) ||
            string.IsNullOrWhiteSpace(response) || response.Length > 40 ||
            !TryResponse(response, family == Rational, out decimal n, out decimal den)) return false;
        // Independent algebraic evaluation: expand the square and distribute
        // the product instead of calling Solve; compare the rational value by
        // cross multiplication without rounded or stored expected answers.
        return family switch
        {
            Signed => n == a + b*b - 2*b*c + c*c,
            Decimal => 10*n == a*(1-b*b) + c*b*b,
            _ => n*(c-a) == den*(a*a-2*b)
        };
    }
}

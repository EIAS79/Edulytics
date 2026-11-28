using System.Globalization;
using System.Text;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Source-aligned U01L02: distribution followed by a unique integer-root
/// linear equation. Quadratic terms cancel in mode 1. Not a general CAS.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10DistributedEquation(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int limit = difficulty switch
        {
            ExactSkillQuestionDifficulty.Challenge => 6,
            ExactSkillQuestionDifficulty.Stretch => 5,
            _ => 4
        };
        int mode = random.Next(2);
        int a, d, b, g;
        do a = random.Next(-limit, limit + 1); while (a == 0);
        if (mode == 1) d = a;
        else { do d = random.Next(-limit, limit + 1); while (d == 0 || d == a); }
        b = random.Next(-7, 8);
        do g = random.Next(-7, 8); while (mode == 1 && g == b);
        int c = random.Next(-20, 21), root = random.Next(-8, 9);
        // Construct the right constant using direct evaluation at a chosen
        // root. The root is deliberately not stored as a question parameter.
        int multiplier = mode == 1 ? root : 1;
        int e = a * multiplier * (root + b) + c - d * multiplier * (root + g);
        string first = mode == 1 ? $"({a}س)" : $"({a})";
        string second = mode == 1 ? $"({d}س)" : $"({d})";
        return Problem(family,
            $"حل المعادلة {first}(س+({b}))+({c}) = {second}(س+({g}))+({e}). وزّع كل عامل أولًا، ثم اجمع الحدود المتشابهة. اكتب قيمة س فقط.",
            "وزّع الطرفين، وألغِ الحدود المتساوية، واعزل س. تحقق بالتعويض في المعادلة الأصلية.",
            AssessmentItemType.Numeric, ("mode",mode),("a",a),("b",b),("d",d),("g",g),("c",c),("e",e));
    }
}

internal static class UaeG10DistributedEquationResponse
{
    internal const string Family = "uae.g10.general.u01.l02.distributed_linear_equation_integer_root";
    private static bool TryRoot(IReadOnlyDictionary<string,int> p, out int root)
    {
        root = 0;
        string[] keys = ["mode","a","b","d","g","c","e"];
        if (p.Count != keys.Length || keys.Any(k => !p.ContainsKey(k))) return false;
        int mode=p["mode"], a=p["a"], b=p["b"], d=p["d"], g=p["g"], c=p["c"], e=p["e"];
        if (mode is not (0 or 1) || a is < -6 or > 6 or 0 || d is < -6 or > 6 or 0 ||
            b is < -7 or > 7 || g is < -7 or > 7 || c is < -20 or > 20 || e is < -1000 or > 1000)
            return false;
        if (mode == 1 && (a != d || b == g) || mode == 0 && a == d) return false;
        // Independent reduction of the two polynomial expressions.
        int coefficient = mode == 1 ? a*b-d*g : a-d;
        int constant = mode == 1 ? c-e : a*b+c-d*g-e;
        if (coefficient == 0 || (-constant) % coefficient != 0) return false;
        root = -constant / coefficient;
        return root is >= -8 and <= 8 && SubstitutionMatches(p,root);
    }
    private static bool SubstitutionMatches(IReadOnlyDictionary<string,int> p,int x)
    {
        long multiplier = p["mode"] == 1 ? x : 1;
        long left = p["a"] * multiplier * (x+p["b"]) + p["c"];
        long right = p["d"] * multiplier * (x+p["g"]) + p["e"];
        return left == right;
    }
    internal static string CorrectAnswer(IReadOnlyDictionary<string,int> p)
    {
        if (!TryRoot(p,out int root)) throw new InvalidOperationException("Invalid bounded G10 distribution equation.");
        return root.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if (!TryRoot(p,out int root) || string.IsNullOrWhiteSpace(answer) || answer.Length > 16) return false;
        var normalized = new StringBuilder(answer.Length);
        foreach (char ch in answer.Trim())
            normalized.Append(ch switch
            {
                >= '٠' and <= '٩' => (char)('0'+ch-'٠'),
                >= '۰' and <= '۹' => (char)('0'+ch-'۰'),
                '−' or '–' => '-',
                _ => ch
            });
        return int.TryParse(normalized.ToString(),NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int actual)
            && actual == root && SubstitutionMatches(p,actual);
    }
}

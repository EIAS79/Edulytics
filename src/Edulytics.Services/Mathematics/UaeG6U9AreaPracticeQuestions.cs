using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Narrow Arabic UAE G6 Unit 9 adapter for the existing exact Practice engine.
/// Reuses area arithmetic, decomposition, and coordinate-grid principles.
/// Freehand models, cutting and graph design require teacher review.
/// </summary>
internal static class UaeG6U9AreaPracticeQuestions
{
    internal const string Conservation = "uae.g6.u09.l01.area_conservation";
    internal const string ParallelogramArea = "uae.g6.u09.l02.parallelogram_area";
    internal const string ParallelogramHeight = "uae.g6.u09.l02.parallelogram_height";
    internal const string TriangleLab = "uae.g6.u09.l03.half_parallelogram";
    internal const string TriangleArea = "uae.g6.u09.l04.triangle_area";
    internal const string TrapezoidLab = "uae.g6.u09.l05.trapezoid_decomposition";
    internal const string TrapezoidArea = "uae.g6.u09.l06.trapezoid_area";
    internal const string ScaleArea = "uae.g6.u09.l08.scale_area";
    internal const string CoordinateArea = "uae.g6.u09.l09.coordinate_polygon_area";
    internal const string GridArea = "uae.g6.u09.l10.grid_area";
    internal const string CompositeArea = "uae.g6.u09.l11.composite_area";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? f) => f is Conservation or ParallelogramArea
        or ParallelogramHeight or TriangleLab or TriangleArea or TrapezoidLab
        or TrapezoidArea or ScaleArea or CoordinateArea or GridArea or CompositeArea;

    private static bool Range(int a, int low, int high) => a >= low && a <= high;

    private static bool Expected(string? f, IReadOnlyDictionary<string, int>? p, out string answer)
    {
        answer = string.Empty;
        if (!Supports(f) || p is null) return false;
        if (f == Conservation)
        {
            if (p.Count != 1 || !p.TryGetValue("mode", out int mode) || !Range(mode, 0, 1))
                return false;
            answer = mode == 0 ? "لا" : "نعم";
            return true;
        }
        if (f == GridArea)
        {
            if (p.Count != 2 || !p.TryGetValue("full", out int full)
                || !p.TryGetValue("halfPairs", out int pairs)
                || !Range(full, 1, 80) || !Range(pairs, 1, 20)) return false;
            answer = (full + pairs).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (f == CoordinateArea)
        {
            if (p.Count != 4 || !p.TryGetValue("x", out int x)
                || !p.TryGetValue("y", out int y)
                || !p.TryGetValue("w", out int w)
                || !p.TryGetValue("h", out int h)
                || !Range(x, -15, 15) || !Range(y, -15, 15)
                || !Range(w, 1, 30) || !Range(h, 1, 30)) return false;
            answer = (w * h).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (f == CompositeArea)
        {
            if (p.Count != 4 || !p.TryGetValue("w", out int w)
                || !p.TryGetValue("h", out int h)
                || !p.TryGetValue("cutW", out int cw)
                || !p.TryGetValue("cutH", out int ch)
                || !Range(w, 2, 40) || !Range(h, 2, 40)
                || !Range(cw, 1, w - 1) || !Range(ch, 1, h - 1)) return false;
            answer = (w * h - cw * ch).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (f == TrapezoidArea || f == TrapezoidLab)
        {
            if (p.Count != 3 || !p.TryGetValue("a", out int a)
                || !p.TryGetValue("b", out int b) || !p.TryGetValue("h", out int height)
                || !Range(a, 1, 40) || !Range(b, 1, 40)
                || !Range(height, 1, 30) || (a + b) * height % 2 != 0) return false;
            answer = (((a + b) * height) / 2).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (f == ScaleArea)
        {
            if (p.Count != 3 || !p.TryGetValue("b", out int b)
                || !p.TryGetValue("h", out int height) || !p.TryGetValue("k", out int k)
                || !Range(b, 1, 30) || !Range(height, 1, 30) || !Range(k, 2, 5)) return false;
            answer = (b * height * k * k).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (f == ParallelogramHeight)
        {
            if (p.Count != 2 || !p.TryGetValue("b", out int b)
                || !p.TryGetValue("area", out int area) || !Range(b, 1, 30)
                || !Range(area, 1, 1200) || area % b != 0) return false;
            answer = (area / b).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (p.Count != 2 || !p.TryGetValue("b", out int baseLength)
            || !p.TryGetValue("h", out int perpendicularHeight)
            || !Range(baseLength, 1, 40) || !Range(perpendicularHeight, 1, 30)) return false;
        int product = baseLength * perpendicularHeight;
        if (f is TriangleArea or TriangleLab)
        {
            if (product % 2 != 0) return false;
            product /= 2;
        }
        else if (f != ParallelogramArea) return false;
        answer = product.ToString(CultureInfo.InvariantCulture);
        return true;
    }

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family)) throw new ArgumentOutOfRangeException(nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int b = random.Next(2, 21), h = random.Next(2, 17);
        Dictionary<string, int> p;
        string question;
        if (family == Conservation)
        {
            int mode = random.Next(2);
            p = new() { ["mode"] = mode };
            question = mode == 0
                ? "هل تتغير مساحة متوازي الأضلاع إذا قصصنا مثلثًا من أحد طرفيه ونقلناه إلى الطرف الآخر دون فقد أو تداخل؟ أجب نعم أو لا."
                : "هل تتغير المساحة إذا حذفنا جزءًا من الشكل ولم ننقله إلى مكان آخر؟ أجب نعم أو لا.";
        }
        else if (family == ParallelogramHeight)
        {
            p = new() { ["b"] = b, ["area"] = b * h };
            question = $"متوازي أضلاع مساحته {b * h} سم² وقاعدته {b} سم. ما طول الارتفاع العمودي بالسنتيمتر؟";
        }
        else if (family is ParallelogramArea or TriangleArea or TriangleLab)
        {
            if (family is TriangleArea or TriangleLab) b *= 2;
            p = new() { ["b"] = b, ["h"] = h };
            string shape = family == ParallelogramArea ? "متوازي أضلاع" : "مثلث";
            question = family == TriangleLab
                ? $"كوّن مثلثين متطابقين من متوازي أضلاع قاعدته {b} وارتفاعه العمودي {h}. ما مساحة مثلث واحد بالوحدات المربعة؟"
                : $"ما مساحة {shape} قاعدته {b} وارتفاعه العمودي {h} بالوحدات المربعة؟";
        }
        else if (family is TrapezoidArea or TrapezoidLab)
        {
            int a = 2 * random.Next(2, 12), c = 2 * random.Next(2, 12);
            p = new() { ["a"] = a, ["b"] = c, ["h"] = h };
            question = family == TrapezoidLab
                ? $"كوّن متوازي أضلاع من شبه منحرفين متطابقين، قاعدتاهما المتوازيتان {a} و{c} وارتفاعهما {h}. ما مساحة شبه منحرف واحد؟"
                : $"ما مساحة شبه منحرف قاعدتاه المتوازيتان {a} و{c} وارتفاعه العمودي {h}؟";
        }
        else if (family == ScaleArea)
        {
            int k = random.Next(2, 5);
            p = new() { ["b"] = b, ["h"] = h, ["k"] = k };
            question = $"متوازي أضلاع قاعدته {b} وارتفاعه {h}. إذا ضُرب طول كل بعد في {k}، فما المساحة الجديدة؟";
        }
        else if (family == CoordinateArea)
        {
            int x = random.Next(-6, 7), y = random.Next(-6, 7);
            p = new() { ["x"] = x, ["y"] = y, ["w"] = b, ["h"] = h };
            question = $"رؤوس مستطيل على المستوى الإحداثي هي ({x}،{y}) و({x+b}،{y}) و({x+b}،{y+h}) و({x}،{y+h}). ما مساحته بالوحدات المربعة؟";
        }
        else if (family == GridArea)
        {
            int full = random.Next(2, 19), pairs = random.Next(1, 9);
            p = new() { ["full"] = full, ["halfPairs"] = pairs };
            question = $"شكل غير منتظم على شبكة يحوي {full} مربعات كاملة و{pairs * 2} أنصاف مربعات. قدّر مساحته بوحدات مربعة.";
        }
        else
        {
            int cw = random.Next(1, b), ch = random.Next(1, h);
            p = new() { ["w"] = b, ["h"] = h, ["cutW"] = cw, ["cutH"] = ch };
            question = $"شكل مركب داخل مستطيل أبعاده {b} في {h} مع إزالة مستطيل صغير {cw} في {ch}. ما المساحة الباقية؟";
        }
        if (!Expected(family, p, out string solution))
            throw new InvalidOperationException("Invalid generated G6 U09 question.");
        return new Item(family, question, solution, AssessmentItemType.ShortAnswer, p);
    }

    private static string Normalize(string response)
    {
        return new string(response.Trim().Where(c => !char.IsWhiteSpace(c)).Select(c =>
            c switch {
                >= '٠' and <= '٩' => (char)('0' + c - '٠'),
                >= '۰' and <= '۹' => (char)('0' + c - '۰'),
                _ => c
            }).ToArray());
    }

    internal static string Solve(string family, IReadOnlyDictionary<string,int> parameters) =>
        Expected(family, parameters, out var s) ? s : throw new ArgumentException("Invalid parameters");

    internal static bool Verify(string family, IReadOnlyDictionary<string,int>? parameters, string? response)
    {
        if (string.IsNullOrWhiteSpace(response) || response.Length > 64
            || !Expected(family, parameters, out var answer)) return false;
        string normalized = Normalize(response);
        if (family == Conservation) return normalized == answer;
        return int.TryParse(normalized, NumberStyles.Integer, CultureInfo.InvariantCulture, out int number)
               && int.TryParse(answer, NumberStyles.Integer, CultureInfo.InvariantCulture, out int expected)
               && number == expected;
    }
}

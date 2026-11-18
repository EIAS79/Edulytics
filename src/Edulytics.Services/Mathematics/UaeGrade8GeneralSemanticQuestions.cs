using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original, Arabic, scope-limited exact numerical/selection questions for six UAE
/// G8 General lessons. Full drawings, geometric proofs and explanations are not
/// automatically graded and remain teacher-reviewed. No publisher questions reused.
/// </summary>
internal static class UaeGrade8GeneralSemanticQuestions
{
    internal const string ProofReason = "uae.g8.u05.l02.proof_reason_classify";
    internal const string Rotation = "uae.g8.u06.l03.rotation_origin_coordinate";
    internal const string TrapezoidMidsegment = "uae.g8.u11.l06.trapezoid_midsegment";
    internal const string KiteArea = "uae.g8.u11.l06.kite_diagonal_area";
    internal const string SetMembership = "uae.g8.u12.l01.membership_finite_roster";
    internal const string SetClassification = "uae.g8.u12.l01.empty_finite_infinite_classify";
    internal const string SetSubset = "uae.g8.u12.l02.subset_classify";
    internal const string SetUnion = "uae.g8.u12.l02.union_cardinality";
    internal const string SetIntersection = "uae.g8.u12.l02.intersection_cardinality";
    internal const string SetDifference = "uae.g8.u12.l02.difference_cardinality";
    internal const string VennRegions = "uae.g8.u12.l03.three_set_venn_region_count";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) => family is
        ProofReason or Rotation or TrapezoidMidsegment or KiteArea or
        SetMembership or SetClassification or SetSubset or SetUnion or
        SetIntersection or SetDifference or VennRegions;

    private static string F(string format, params object[] values) =>
        string.Format(CultureInfo.InvariantCulture, format, values);

    internal static Item Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown G8 General semantic family.");

        var p = new Dictionary<string, int>();
        string prompt;
        string solution;
        switch (family)
        {
            case ProofReason:
            {
                var kind = random.Next(1, 4);
                var given = random.Next(20, 155);
                p["kind"] = kind;
                p["given"] = given;
                var stem = kind switch
                {
                    1 => F("في برهان هندسي: إذا كان قياس أ + قياس ب = 180° وقياس أ + قياس ج = 180°، فلماذا قياس ب = قياس ج؟ مثال: قياس أ = {0}°.", given),
                    2 => F("إذا ثبت في البرهان أن أ = ب وأن ب = ج، فما السبب الذي يسمح باستنتاج أ = ج؟ افترض أن قياس ب = {0}°.", given),
                    _ => F("مستقيمان متقاطعان؛ قياس زاوية رأسية = {0}°. ما السبب الصحيح لتساوي الزاوية المقابلة بالرأس معها؟", given)
                };
                prompt = stem + " اختر رقم السبب: 1) طرح المقدار نفسه من طرفي مساواتين؛ 2) خاصية التعدي؛ 3) نظرية الزوايا المتقابلة بالرأس.";
                solution = "برر الانتقال باستخدام الخاصية أو النظرية، لا نتيجة مطلوب إثباتها.";
                break;
            }
            case Rotation:
            {
                var x = random.Next(-12, 13);
                var y = random.Next(-12, 13);
                if (x == 0 && y == 0) x = 4;
                var quarter = random.Next(1, 4);
                var component = random.Next(0, 2);
                p["x"] = x; p["y"] = y; p["quarter"] = quarter; p["component"] = component;
                prompt = F("دُوّرت النقطة أ({0}، {1}) عكس اتجاه عقارب الساعة حول الأصل بمقدار {2}°. ما الإحداثي {3} للنقطة الناتجة؟", x, y, quarter * 90, component == 0 ? "السيني" : "الصادي");
                solution = "90°: (−ص، س)، 180°: (−س، −ص)، 270°: (ص، −س). طبق قاعدة الزاوية ثم اختر الإحداثي المطلوب.";
                break;
            }
            case TrapezoidMidsegment:
            {
                var a = random.Next(3, 25) * 2;
                var b = random.Next(3, 25) * 2;
                // The source uses the exclusive trapezoid definition.
                // Equal bases would describe a parallelogram instead.
                while (b == a) b = random.Next(3, 25) * 2;
                p["base1"] = a; p["base2"] = b;
                prompt = F("شبه منحرف قاعدتاه المتوازيتان {0} و{1} سم. ما طول القطعة المتوسطة الواصلة بين منتصفي ساقيه، بالسنتيمتر؟", a, b);
                solution = "طول القطعة المتوسطة يساوي نصف مجموع القاعدتين.";
                break;
            }
            case KiteArea:
            {
                var d1 = random.Next(2, 15) * 2;
                var d2 = random.Next(2, 15);
                p["d1"] = d1; p["d2"] = d2;
                prompt = F("طائرة ورقية قطراها المتعامدان {0} سم و{1} سم. احسب مساحتها بالسنتيمتر المربع.", d1, d2);
                solution = "مساحة الطائرة الورقية تساوي نصف حاصل ضرب القطرين.";
                break;
            }
            case SetMembership:
            {
                var step = random.Next(2, 7);
                var yes = random.Next(0, 2) == 1;
                var number = yes ? step * random.Next(1, 7) : step * random.Next(1, 7) + 1;
                p["step"] = step; p["number"] = number;
                prompt = F("لتكن أ = {{{0}}}. هل {1} ∈ أ؟ أدخل 1 إذا كان عنصرًا، و0 إذا لم يكن.", string.Join("، ", Enumerable.Range(1, 6).Select(k => k * step)), number);
                solution = "الانتماء يعني وجود العنصر نفسه ضمن المجموعة؛ لا تغيّر الترتيب أو تكرّر العناصر.";
                break;
            }
            case SetClassification:
            {
                var kind = random.Next(0, 3);
                var cap = random.Next(3, 11);
                p["kind"] = kind; p["cap"] = cap;
                prompt = kind switch
                {
                    0 => "المجموعة أ = {س | س عدد طبيعي موجب وأصغر من الصفر}.",
                    1 => F("المجموعة أ = {{س | س عدد طبيعي موجب وأصغر من {0}}}.", cap),
                    _ => F("المجموعة أ = {{ {0}، {1}، {2}، ... }} وتمثل جميع المضاعفات الطبيعية الموجبة للعدد {0}.", cap, 2 * cap, 3 * cap)
                };
                prompt += " صنّف أ: 0) خالية؛ 1) منتهية غير خالية؛ 2) غير منتهية. أدخل الرقم.";
                solution = "المجموعة الخالية لا عناصر لها، والمنتهية محدودة العناصر، وغير المنتهية تستمر بلا حد.";
                break;
            }
            case SetSubset:
            case SetUnion:
            case SetIntersection:
            case SetDifference:
            {
                var a = random.Next(1, 64);
                var b = random.Next(1, 64);
                p["a"] = a; p["b"] = b;
                var stem = F("أ = {{{0}}}، ب = {{{1}}}. ", Elements(a), Elements(b));
                prompt = stem + (family switch
                {
                    SetSubset => "هل أ ⊆ ب؟ أدخل 1 إذا كان كل عنصر من أ ينتمي إلى ب، وإلا أدخل 0.",
                    SetUnion => "كم عدد العناصر المختلفة في أ∪ب؟",
                    SetIntersection => "كم عدد العناصر في أ∩ب؟",
                    _ => "كم عدد العناصر في أ\\ب (العناصر الموجودة في أ وغير الموجودة في ب)؟"
                });
                solution = "افحص انتماء كل عنصر، ولا تحسبه مرتين في الاتحاد. ترتيب طرح المجموعتين مهم.";
                break;
            }
            case VennRegions:
            {
                for (var i = 1; i <= 7; i++) p["r" + i] = random.Next(0, 7);
                p["outside"] = random.Next(0, 7);
                var mode = random.Next(0, 6);
                p["mode"] = mode;
                prompt = F("في مخطط فن بثلاث مجموعات، المناطق المنفصلة: أ فقط={0}، ب فقط={1}، ج فقط={2}، أ وب فقط={3}، أ وج فقط={4}، ب وج فقط={5}، المجموعات الثلاث معًا={6}، خارج الدوائر={7}. ",
                    p["r1"], p["r2"], p["r3"], p["r4"], p["r5"], p["r6"], p["r7"], p["outside"]);
                prompt += mode switch
                {
                    0 => "كم عدد عناصر أ∩ب∩ج؟",
                    1 => "كم عدد عناصر أ∩ب (بما فيها عناصر ج المشتركة)؟",
                    2 => "كم عدد عناصر أ∪ب؟",
                    3 => "كم عدد عناصر (أ∪ب∪ج)ᶜ ضمن المجموعة الشاملة؟",
                    4 => "كم عدد عناصر أ\\ب؟",
                    _ => "كم عدد العناصر في أ فقط؟"
                };
                solution = "اجمع مناطق مخطط فن المنفصلة المطلوبة مرة واحدة؛ التقاطع الثلاثي يدخل ضمن التقاطعات الثنائية.";
                break;
            }
            default: throw new InvalidOperationException("Unknown semantic family.");
        }
        return new Item(family, prompt, solution, AssessmentItemType.Numeric, p);
    }

    private static string Elements(int mask) =>
        string.Join("، ", Enumerable.Range(1, 6)
            .Where(i => (mask & (1 << (i - 1))) != 0));

    private static int CountBits(int mask) =>
        Enumerable.Range(0, 6).Count(i => (mask & (1 << i)) != 0);

    private static bool ExactKeys(IReadOnlyDictionary<string, int> p, params string[] keys) =>
        p.Count == keys.Length && keys.All(p.ContainsKey);

    private static bool TryExpected(string family, IReadOnlyDictionary<string, int> p, out int expected)
    {
        expected = 0;
        if (!Supports(family)) return false;

        switch (family)
        {
            case ProofReason:
                if (!ExactKeys(p, "kind", "given") ||
                    p["kind"] is < 1 or > 3 || p["given"] is < 20 or > 154) return false;
                expected = p["kind"];
                return true;
            case Rotation:
                if (!ExactKeys(p, "x", "y", "quarter", "component") ||
                    p["x"] is < -12 or > 12 || p["y"] is < -12 or > 12 ||
                    p["quarter"] is < 1 or > 3 || p["component"] is < 0 or > 1 ||
                    (p["x"] == 0 && p["y"] == 0)) return false;
                var x = p["x"]; var y = p["y"];
                var nx = p["quarter"] switch { 1 => -y, 2 => -x, _ => y };
                var ny = p["quarter"] switch { 1 => x, 2 => -y, _ => -x };
                expected = p["component"] == 0 ? nx : ny;
                return true;
            case TrapezoidMidsegment:
                if (!ExactKeys(p, "base1", "base2") ||
                    p["base1"] is < 6 or > 48 || p["base2"] is < 6 or > 48 ||
                    p["base1"] % 2 != 0 || p["base2"] % 2 != 0 ||
                    p["base1"] == p["base2"]) return false;
                expected = (p["base1"] + p["base2"]) / 2;
                return true;
            case KiteArea:
                if (!ExactKeys(p, "d1", "d2") ||
                    p["d1"] is < 4 or > 28 || p["d1"] % 2 != 0 ||
                    p["d2"] is < 2 or > 14) return false;
                expected = p["d1"] * p["d2"] / 2;
                return true;
            case SetMembership:
                if (!ExactKeys(p, "step", "number") ||
                    p["step"] is < 2 or > 6 || p["number"] is < 1 or > 37) return false;
                expected = Enumerable.Range(1, 6).Any(k => k * p["step"] == p["number"]) ? 1 : 0;
                return true;
            case SetClassification:
                if (!ExactKeys(p, "kind", "cap") || p["kind"] is < 0 or > 2 ||
                    p["cap"] is < 3 or > 10) return false;
                expected = p["kind"]; return true;
            case SetSubset:
            case SetUnion:
            case SetIntersection:
            case SetDifference:
                if (!ExactKeys(p, "a", "b") ||
                    p["a"] is < 1 or > 63 || p["b"] is < 1 or > 63) return false;
                expected = family switch
                {
                    SetSubset => (p["a"] & ~p["b"]) == 0 ? 1 : 0,
                    SetUnion => CountBits(p["a"] | p["b"]),
                    SetIntersection => CountBits(p["a"] & p["b"]),
                    _ => CountBits(p["a"] & ~p["b"])
                };
                return true;
            case VennRegions:
                if (!ExactKeys(p, "r1", "r2", "r3", "r4", "r5", "r6", "r7", "outside", "mode") ||
                    p["mode"] is < 0 or > 5 ||
                    Enumerable.Range(1, 7).Any(i => p["r" + i] is < 0 or > 6) ||
                    p["outside"] is < 0 or > 6) return false;
                expected = p["mode"] switch
                {
                    0 => p["r7"],
                    1 => p["r4"] + p["r7"],
                    2 => p["r1"] + p["r2"] + p["r4"] + p["r5"] + p["r6"] + p["r7"],
                    3 => p["outside"],
                    4 => p["r1"] + p["r5"],
                    _ => p["r1"]
                };
                return true;
            default: return false;
        }
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!TryExpected(family, p, out var expected))
            throw new InvalidOperationException("Invalid G8 semantic question parameters.");
        return expected.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!TryExpected(family, p, out var expected) || string.IsNullOrWhiteSpace(answer))
            return false;
        var normalized = string.Concat(answer.Trim().Select(c => c switch
        {
            >= '٠' and <= '٩' => (char)('0' + c - '٠'),
            >= '۰' and <= '۹' => (char)('0' + c - '۰'),
            '−' => '-', _ => c
        }));
        return int.TryParse(normalized, NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture, out var result) && result == expected;
    }
}

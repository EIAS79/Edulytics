using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Arabic grade-6 solid-measurement adapter registered within the existing
/// ExactSkillContractQuestionEngine. Teacher grades physical nets, models and proofs.
/// </summary>
internal static class UaeG6U10SolidPracticeQuestions
{
    internal const string Cubes = "uae.g6.u10.l01.cube_layers";
    internal const string RectVolume = "uae.g6.u10.l02.rect_prism_volume";
    internal const string TriVolume = "uae.g6.u10.l03.tri_prism_volume";
    internal const string PyrVolume = "uae.g6.u10.l05.pyramid_volume";
    internal const string RectSurface = "uae.g6.u10.l08.rect_prism_surface";
    internal const string TriSurface = "uae.g6.u10.l10.tri_prism_surface";
    internal const string PyrSurface = "uae.g6.u10.l12.pyramid_surface";
    internal const string CompVolume = "uae.g6.u10.l14.composite_volume";
    internal const string CompSurface = "uae.g6.u10.l14.composite_surface";
    internal const string NetFaces = "uae.g6.u10.l07.net_faces";

    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    internal static bool Supports(string? f) => f is Cubes or RectVolume or TriVolume or PyrVolume
        or RectSurface or TriSurface or PyrSurface or CompVolume or CompSurface or NetFaces;

    private static bool ValidDim(int v) => v is >= 1 and <= 40;

    private static bool Expected(string? family, IReadOnlyDictionary<string,int>? p, out string answer)
    {
        answer = string.Empty;
        if (!Supports(family) || p is null) return false;
        if (family == NetFaces)
        {
            if (p.Count != 1 || !p.TryGetValue("type", out int type) || type is <0 or >2)
                return false;
            answer = type == 0 ? "6" : "5";
            return true;
        }
        if (family == TriSurface)
        {
            if (p.Count != 1 || !p.TryGetValue("length", out int length) || !ValidDim(length))
                return false;
            // Right-triangular base has sides 3,4,5, area 6 and perimeter 12.
            answer = (12 + 12 * length).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (family == PyrSurface)
        {
            if (p.Count != 2 || !p.TryGetValue("a", out int a)
                || !p.TryGetValue("slant", out int slant)
                || !ValidDim(a) || !ValidDim(slant) || slant * 2 < a)
                return false;
            answer = (a * a + 2 * a * slant).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (family == CompSurface)
        {
            if (p.Count != 4 || !p.TryGetValue("w", out int w)
                || !p.TryGetValue("h", out int h) || !p.TryGetValue("l1", out int l1)
                || !p.TryGetValue("l2", out int l2)
                || !ValidDim(w) || !ValidDim(h) || !ValidDim(l1) || !ValidDim(l2))
                return false;
            // Two rectangular prisms joined flush along a w×h end face.
            // The two hidden contact faces must not be included.
            answer = (2 * (w * h + (w + h) * (l1 + l2))).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (family == CompVolume)
        {
            if (p.Count != 6 || !p.TryGetValue("l1", out int firstVolLength)
                || !p.TryGetValue("w1", out int w1) || !p.TryGetValue("h1", out int h1)
                || !p.TryGetValue("l2", out int secondVolLength) || !p.TryGetValue("w2", out int w2)
                || !p.TryGetValue("h2", out int h2)
                || !ValidDim(firstVolLength) || !ValidDim(w1) || !ValidDim(h1)
                || !ValidDim(secondVolLength) || !ValidDim(w2) || !ValidDim(h2))
                return false;
            answer = (firstVolLength * w1 * h1 + secondVolLength * w2 * h2).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (family == TriVolume)
        {
            if (p.Count != 3 || !p.TryGetValue("b", out int b)
                || !p.TryGetValue("triH", out int triH)
                || !p.TryGetValue("length", out int triangularPrismLength)
                || !ValidDim(b) || !ValidDim(triH) || !ValidDim(triangularPrismLength)
                || b * triH % 2 != 0)
                return false;
            answer = ((b * triH / 2) * triangularPrismLength).ToString(CultureInfo.InvariantCulture);
            return true;
        }
        if (p.Count != 3 || !p.TryGetValue("l", out int l)
            || !p.TryGetValue("w", out int width) || !p.TryGetValue("h", out int height)
            || !ValidDim(l) || !ValidDim(width) || !ValidDim(height)) return false;
        int v = family switch
        {
            Cubes or RectVolume => l * width * height,
            RectSurface => 2 * (l * width + l * height + width * height),
            PyrVolume when (l * width * height) % 3 == 0 => (l * width * height) / 3,
            _ => -1
        };
        if (v < 0) return false;
        answer = v.ToString(CultureInfo.InvariantCulture);
        return true;
    }

    internal static Item Build(string f, Random random)
    {
        if (!Supports(f)) throw new ArgumentOutOfRangeException(nameof(f));
        ArgumentNullException.ThrowIfNull(random);
        int l = random.Next(2,13), w = random.Next(2,12), h = random.Next(2,10);
        Dictionary<string,int> p;
        string question;
        if (f == NetFaces)
        {
            int type = random.Next(3);
            p = new() { ["type"] = type };
            string name = type switch { 0 => "المنشور المستطيل", 1 => "المنشور الثلاثي", _ => "الهرم الرباعي" };
            question = $"ما عدد أوجه {name} في شبكته الكاملة؟ اكتب العدد فقط.";
        }
        else if (f == TriSurface)
        {
            p = new() { ["length"] = l };
            question = $"منشور ثلاثي قائم قاعدته مثلث قائم أضلاعه 3 و4 و5 سم وطوله {l} سم. ما مساحة سطحه الكلية بالسم²؟";
        }
        else if (f == PyrSurface)
        {
            int a = 2 * random.Next(2,9), slant = random.Next(a/2+1,a+10);
            p = new() { ["a"] = a, ["slant"] = slant };
            question = $"هرم قاعدته مربعة ضلعها {a} سم، وارتفاع وجهه المائل {slant} سم. ما مساحة سطحه الكلية بالسم²؟";
        }
        else if (f == CompSurface)
        {
            int l2 = random.Next(2,12);
            p = new() { ["w"] = w, ["h"] = h, ["l1"] = l, ["l2"] = l2 };
            question = $"منشوران مستطيلان بعرض {w} وارتفاع {h} وطولين {l} و{l2}، اتصلا تمامًا عند وجهيهما المتطابقين. ما المساحة الخارجية بعد حذف الوجهين المخفيين؟";
        }
        else if (f == CompVolume)
        {
            int l2 = random.Next(2,12),w2 = random.Next(2,12),h2 = random.Next(2,10);
            p = new() { ["l1"] = l, ["w1"] = w, ["h1"] = h, ["l2"] = l2, ["w2"] = w2, ["h2"] = h2 };
            question = $"مجسمان منفصلان غير متداخلين: أبعاد الأول {l}×{w}×{h} والثاني {l2}×{w2}×{h2}. ما مجموع حجميهما؟";
        }
        else if (f == TriVolume)
        {
            int b = 2 * random.Next(2,12),triH = random.Next(2,12);
            p = new() { ["b"] = b, ["triH"] = triH, ["length"] = l };
            question = $"منشور ثلاثي مساحة قاعدته نصف ({b}×{triH}) وطوله {l}. ما حجمه بوحدات مكعبة؟";
        }
        else
        {
            if (f == PyrVolume) h = 3 * random.Next(1,10);
            p = new() { ["l"] = l, ["w"] = w, ["h"] = h };
            question = f switch
            {
                Cubes => $"نموذج منشور يتكون من {h} طبقات، بكل طبقة {l} صفوف و{w} مكعبات في الصف. كم مكعب وحدة فيه؟",
                RectVolume => $"ما حجم منشور مستطيل طوله {l} وعرضه {w} وارتفاعه {h} بوحدات مكعبة؟",
                PyrVolume => $"هرم مساحة قاعدته المستطيلة {l}×{w} وارتفاعه العمودي {h}. ما حجمه؟",
                _ => $"ما مساحة سطح منشور مستطيل أبعاده {l} و{w} و{h} بوحدات مربعة؟"
            };
        }
        if (!Expected(f,p,out string solution)) throw new InvalidOperationException("Invalid G6 U10 generated math");
        return new Item(f,question,solution,AssessmentItemType.ShortAnswer,p);
    }

    private static string Normalize(string value) =>
        new string(value.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch {
            >= '٠' and <= '٩' => (char)('0'+c-'٠'),
            >= '۰' and <= '۹' => (char)('0'+c-'۰'),
            _=>c }).ToArray());

    internal static string Solve(string f,IReadOnlyDictionary<string,int> p) =>
        Expected(f,p,out var s) ? s : throw new ArgumentException("Invalid solid parameters");

    internal static bool Verify(string f,IReadOnlyDictionary<string,int>? p,string? response)
    {
        if (string.IsNullOrWhiteSpace(response) || response.Length>64
            || !Expected(f,p,out string correct)) return false;
        return int.TryParse(Normalize(response),NumberStyles.Integer,CultureInfo.InvariantCulture,out int actual)
            && int.TryParse(correct,NumberStyles.Integer,CultureInfo.InvariantCulture,out int expected)
            && actual==expected;
    }
}

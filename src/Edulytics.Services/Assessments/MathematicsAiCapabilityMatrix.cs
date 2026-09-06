using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Assessments;

public enum MathematicsAiCapabilityLevel
{
    ManualOnly = 0,
    AiAssisted = 1,
    VerifiedAi = 2
}

/// <summary>
/// One curriculum-neutral capability decision for a Mathematics learning outcome.
/// Verified AI means a reviewed native provider can generate and validate the
/// requested canonical skills. AI-assisted means Edulytics can generate a local,
/// deterministic curriculum-contextual question while clearly preserving that it
/// is not a native solver for the complete mathematical skill. Manual-only remains
/// fail-closed for text that is not recognizably Mathematics curriculum context.
/// </summary>
public sealed record MathematicsAiCapability(
    MathematicsAiCapabilityLevel Level,
    IReadOnlyList<CanonicalMathematicsSkill> CanonicalSkills,
    IReadOnlyList<MathematicsGeneratorFamily> GenerationFamilies,
    string? ProviderKey,
    string ReasonCode)
{
    public IReadOnlyList<MathematicsGeneratorFamily> VerifiedFamilies =>
        Level == MathematicsAiCapabilityLevel.VerifiedAi
            ? GenerationFamilies
            : [];

    public bool CanGenerateVerified =>
        Level == MathematicsAiCapabilityLevel.VerifiedAi &&
        GenerationFamilies.Count > 0 &&
        !string.IsNullOrWhiteSpace(ProviderKey);

    public bool CanGenerateAssisted =>
        Level == MathematicsAiCapabilityLevel.AiAssisted &&
        GenerationFamilies.Count > 0 &&
        !string.IsNullOrWhiteSpace(ProviderKey);

    public bool CanGenerate => CanGenerateVerified || CanGenerateAssisted;
}

/// <summary>
/// Canonical source of truth for Mathematics AI capability classification.
/// UI, assessment generation and student private practice derive availability
/// from this matrix instead of maintaining separate curriculum-code allowlists.
/// Native deterministic families remain VerifiedAi. Other recognizable
/// Mathematics curriculum outcomes use the local curriculum-contextual provider
/// and are explicitly classified AiAssisted rather than being falsely labelled
/// native or left as ManualOnly.
/// </summary>
public static class MathematicsAiCapabilityMatrix
{
    private const string ContextualProviderKey = "edulytics-contextual-mathematics";

    private static readonly IMathematicsGenerationCapabilityProvider VerifiedProvider =
        new NativeMathematicsGenerationCapabilityProvider();

    public static MathematicsAiCapability Resolve(
        string? outcomeCode,
        string? description)
    {
        var skills = CanonicalMathematicsSkillMapper.Resolve(
            outcomeCode,
            description);
        var verifiedFamilies = VerifiedProvider.ResolveFamilies(skills);
        if (verifiedFamilies.Count > 0)
        {
            return new MathematicsAiCapability(
                MathematicsAiCapabilityLevel.VerifiedAi,
                skills,
                verifiedFamilies,
                VerifiedProvider.ProviderKey,
                "ReviewedNativeProvider");
        }

        if (LooksLikeMathematicsCurriculum(outcomeCode, description))
        {
            return new MathematicsAiCapability(
                MathematicsAiCapabilityLevel.AiAssisted,
                skills,
                [MathematicsGeneratorFamily.CurriculumContextCheck],
                ContextualProviderKey,
                skills.Count == 0
                    ? "CurriculumContextFallback"
                    : "CanonicalSkillContextFallback");
        }

        return new MathematicsAiCapability(
            MathematicsAiCapabilityLevel.ManualOnly,
            skills,
            [],
            null,
            skills.Count == 0
                ? "NoCanonicalSkillMapping"
                : "NoConfiguredProviderForAllSkills");
    }

    private static bool LooksLikeMathematicsCurriculum(
        string? outcomeCode,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return false;

        var code = outcomeCode?.Trim().ToUpperInvariant() ?? string.Empty;
        var text = description.Trim().ToUpperInvariant();

        // Official mathematics-pack identities are useful scope evidence, but a
        // synthetic/unknown code alone is never enough: the context must also
        // carry mathematical or explicit Mathematics-pack semantics.
        var knownMathPack =
            code.StartsWith("CCSS:", StringComparison.Ordinal) ||
            code.StartsWith("CAM:", StringComparison.Ordinal) ||
            code.StartsWith("PL:", StringComparison.Ordinal) ||
            code.StartsWith("UAE:", StringComparison.Ordinal) ||
            code.StartsWith("MAT.", StringComparison.Ordinal) ||
            code.StartsWith("MATH-", StringComparison.Ordinal) ||
            code.StartsWith("CURRICULUM-X:", StringComparison.Ordinal);

        var mathematicalVocabulary = ContainsAny(
            text,
            "MATHEMATICS", "MATHEMATICAL", "NUMBER", "NUMBERS", "INTEGER", "INTEGERS",
            "ADD", "SUBTRACT", "MULTIP", "DIVID", "ARITHMET", "PLACE VALUE",
            "FRACTION", "DECIMAL", "PERCENT", "RATIO", "RATE", "PROPORTION",
            "EQUATION", "INEQUALITY", "ALGEBRA", "EXPRESSION", "VARIABLE",
            "FUNCTION", "GRAPH", "COORDINATE", "SLOPE", "GRADIENT", "SEQUENCE",
            "GEOMET", "AREA", "PERIMETER", "ANGLE", "TRIANGLE", "CIRCLE", "SHAPE",
            "MEASURE", "LENGTH", "VOLUME", "MASS", "TIME", "UNIT",
            "STATISTIC", "MEAN", "AVERAGE", "MEDIAN", "PROBABILITY", "DATA",
            "EXPONENT", "POWER", "ROOT", "LOGARITH", "VECTOR", "MATRIX",
            "DERIVATIVE", "DIFFERENTIAT", "INTEGRAL", "CALCULUS", "TRIGONOMET",
            "LICZB", "DODAW", "ODEJM", "MNOŻ", "MNOZ", "DZIEL", "UŁAM", "ULAM",
            "PROCENT", "PROPORCJ", "RÓWNAN", "ROWNAN", "NIERÓWN", "NIEROWN",
            "FUNKCJ", "GEOMETR", "POLE", "OBWÓD", "OBWOD", "KĄT", "KAT",
            "ŚREDNI", "SREDNI", "PRAWDOPODOB", "POTĘG", "POTEG", "PIERWIAST",
            "CIĄG", "CIAG", "WEKTOR", "MACIERZ", "POCHODN", "CAŁK", "CALK",
            "LOGARYTM", "DZIESIĘTN", "DZIESIETN",
            "رياض", "عدد", "أعداد", "جمع", "طرح", "ضرب", "قسمة", "كسر", "كسور",
            "عشري", "نسبة", "تناسب", "مئوية", "معادلة", "معادلات", "متباينة",
            "جبر", "دالة", "هندسة", "مساحة", "محيط", "زاوية", "مثلث", "دائرة",
            "قياس", "طول", "حجم", "متوسط", "احتمال", "بيانات", "أس", "جذر",
            "متتالية", "متجه", "مصفوف", "مشتق", "تكامل", "لوغاريتم");

        return mathematicalVocabulary ||
            (knownMathPack && ContainsAny(text, "CURRICULUM", "REQUIREMENT", "OBJECTIVE", "STANDARD"));
    }

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 B01 source-verified shared concept contamination repair, exactly five.
/// Removes a wrongly reused geometric CLASSIFICATION rule from division,
/// decimal-place multiplication, fraction and cost lessons. No publisher
/// copyrighted educational expression, asset or figure is copied.
/// This fix does not grant an L1-L6 or eight-dimension academic certification.
/// </summary>
public static class CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-unrelated-geometry-concepts-5-v1";
    private const string OriginalMisplacedRule =
        "- Use mathematical attributes such as sides, angles, parallelism, or length—not size or orientation—to justify a classification.";

    private sealed record Guard(string Code, string SourceUrl, string RawBodySha,
        string OutcomeCodes, string OriginalMathExplanation);

    private static readonly Guard[] VerifiedCohort =
    [
        new Guard("PED:US-CCSS-MATH:G3:U04:L03", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-3/preparation.html", "205f92de0bdb9d61f7f70ddeb7882c4b1c420f1fc8ebd64b2fc45d27dd63d809", "CCSS:3.NBT.A.2|CCSS:3.OA.A.2", "For division, identify whether the unknown is the number of equal groups or the number in each group."),
        new Guard("PED:US-CCSS-MATH:G3:U04:L12", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-12/preparation.html", "49e7d0aedfae6d8fb7f29fd0693cdc0f8c75072f5706404675a4f50d955e14c2", "CCSS:3.NBT.A.3", "Multiplying by a multiple of ten can be reasoned about as groups of tens: for example, 4 times 30 equals 12 tens, or 120."),
        new Guard("PED:US-CCSS-MATH:G3:U05:L10", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-10/preparation.html", "87f4149b044d42ade0dd6819e6213e40d017b7f6f8bce657de01d739820082e5", "CCSS:3.NF.A.3", "Equivalent fractions name the same amount of the same whole, even when the parts are partitioned differently."),
        new Guard("PED:US-CCSS-MATH:G3:U08:L01", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-8/lesson-1/preparation.html", "9af7452882227dec9eb391337cf76a47c64497fcba9e30f126cb80b04af63da3", "CCSS:3.NF.A.1|CCSS:3.NF.A.2", "Estimate a fraction by comparing the represented parts with equal-sized unit-fraction benchmarks."),
        new Guard("PED:US-CCSS-MATH:G3:U08:L05", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-8/lesson-5/preparation.html", "a3925007b50ab1a32b1c80c13e651e379c6d3e62da61a0c4fbe7c43aa9314cd5", "CCSS:3.NBT.A.2|CCSS:3.OA.D.8", "To calculate a room's finishing cost, multiply each item price by its quantity, add the subtotals, and compare with an estimate."),
    ];

    private static readonly IReadOnlyDictionary<string, Guard> ByCode =
        VerifiedCohort.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static int ExpectedCount => VerifiedCohort.Length;

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == "US-CCSS-MATH" &&
        document.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Guard VerifySource(CanonicalLessonContentPackLesson lesson)
    {
        var guard = ByCode[lesson.LessonCode];
        if (lesson.SourceUrl != guard.SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, guard.RawBodySha,
                StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", lesson.OutcomeCodes) != guard.OutcomeCodes ||
            lesson.Translations.Count != 1 || lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException(
                "V9 geometry concept correction rejected source, standards or body drift: " +
                lesson.LessonCode);
        return guard;
    }

    private static string UpdatedRule(Guard guard) => "- " + guard.OriginalMathExplanation;

    private static string ExpectedOriginalConcepts(string updated, Guard guard)
    {
        var ending = UpdatedRule(guard);
        if (!updated.EndsWith(ending, StringComparison.Ordinal))
            throw new InvalidOperationException("V9 corrected concept guard is missing.");
        return updated[..^ending.Length] + OriginalMisplacedRule;
    }

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != "US-CCSS-MATH" ||
            document.VersionCode != "CCSSM-2010") return;

        foreach (var lesson in document.Lessons)
        {
            if (!ByCode.ContainsKey(lesson.LessonCode)) continue;
            var guard = VerifySource(lesson);
            var translation = lesson.Translations.Single();
            var replacement = UpdatedRule(guard);

            if (translation.KeyConceptsAndRules.EndsWith(replacement,
                    StringComparison.Ordinal)) continue;

            if (!translation.KeyConceptsAndRules.EndsWith(OriginalMisplacedRule,
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "V9 geometry concept repair rejected unreviewed or partial text drift: " +
                    lesson.LessonCode);

            translation.KeyConceptsAndRules =
                translation.KeyConceptsAndRules[..^OriginalMisplacedRule.Length] + replacement;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string previous) =>
        IsTarget(document, lesson) ? CorrectedVersion : previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string version) =>
        IsTarget(document, lesson) &&
        (version == CorrectedVersion ||
         version == CommonCoreRichContentCorrections.CorrectionContentVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException("V9 geometry concept predecessor not approved.");

        var guard = VerifySource(lesson);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 geometry concept language drift.");

        var old = persisted.Single();
        var expected = lesson.Translations.Single();
        var expectedRawConcepts = ExpectedOriginalConcepts(
            expected.KeyConceptsAndRules, guard);
        if (old.KeyConceptsAndRules != expectedRawConcepts &&
            old.KeyConceptsAndRules != expected.KeyConceptsAndRules)
            throw new InvalidOperationException(
                "V9 geometry concept persisted unverified body drift: " + lesson.LessonCode);

        if (old.Title != expected.Title ||
            old.Explanation != expected.Explanation ||
            old.WorkedExamples != expected.WorkedExamples ||
            old.StepByStepSolutions != expected.StepByStepSolutions ||
            old.CommonMistakes != expected.CommonMistakes ||
            old.QuickSummary != expected.QuickSummary)
            throw new InvalidOperationException(
                "V9 geometry concept unexpected unrelated field drift: " + lesson.LessonCode);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Repairs an actually observed unrelated clock-reading rule in three B01
/// lessons whose original teacher goals instead teach equal groups,
/// base-ten addition algorithms, and two-step word problems.
/// Source checked against read-only IM K-5 2021 teacher preparations.
/// Does not award academic certification.
/// </summary>
public static class CommonCoreB01V9OffTopicClockConceptCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-offtopic-clock-concepts-3-v1";

    private const string WrongRule =
        "- Read the hour and minute hands separately; for hour and half-hour times, :00 and :30 must match the minute-hand position.";

    private sealed record Guard(string Code, string SourceUrl, string BodySha,
        string Outcomes, string Replacement, string PreviousVersion);

    private static readonly Guard[] Verified =
    [
        new(
            "PED:US-CCSS-MATH:G3:U01:L09",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-9/preparation.html",
            "6c907b7f1945d2b123f490edb6995a800fe142ee682231097b0f2433cd7eb2d4",
            "CCSS:3.OA.A.1",
            "- Equal-sized groups can be modeled with counters or pictures: four groups of three counters make twelve counters. Check that every group contains the same number.",
            CommonCoreB01V9SharedComparisonProcedureCorrections.CorrectedVersion),
        new(
            "PED:US-CCSS-MATH:G3:U03:L04",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-4/preparation.html",
            "bb79cad9214030608ea3dd58b7845479fb14f532145cb28bf4fad106ca80574e",
            "CCSS:3.NBT.A.2",
            "- In an addition algorithm, combine hundreds with hundreds, tens with tens, and ones with ones. Ten ones can be composed as a ten, and ten tens as a hundred; the written steps must agree with base-ten drawings.",
            CommonCoreB01V9SharedAdditionProcedureCorrections.CorrectedVersion),
        new(
            "PED:US-CCSS-MATH:G3:U03:L20",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-20/preparation.html",
            "5a7867f5109b55af25504221988b434dcaae33dd63da249d0d88e584bfdb7cbe",
            "CCSS:3.OA.C.7|CCSS:3.OA.D.8",
            "- For a two-step story, identify information that is missing, find the intermediate quantity, and then solve the final question. Check that both conditions of the story are satisfied.",
            CommonCoreB01V9SharedAdditionProcedureCorrections.CorrectedVersion)
    ];

    private static readonly IReadOnlyDictionary<string, Guard> ByCode =
        Verified.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static int ExpectedCount => Verified.Length;

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == "US-CCSS-MATH" &&
        document.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Guard Verify(CanonicalLessonContentPackLesson lesson)
    {
        var g = ByCode[lesson.LessonCode];
        if (lesson.SourceUrl != g.SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, g.BodySha,
                StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", lesson.OutcomeCodes) != g.Outcomes ||
            lesson.Translations.Count != 1 ||
            lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException(
                "Off-topic clock concept source/outcome/body drift: " + lesson.LessonCode);
        return g;
    }

    private static string OriginalConcepts(string corrected, Guard g)
    {
        if (!corrected.EndsWith(g.Replacement, StringComparison.Ordinal))
            throw new InvalidOperationException("Expected corrected mathematical concept missing.");
        return corrected[..^g.Replacement.Length] + WrongRule;
    }

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != "US-CCSS-MATH" ||
            document.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in document.Lessons)
        {
            if (!ByCode.ContainsKey(lesson.LessonCode)) continue;
            var g = Verify(lesson);
            var t = lesson.Translations.Single();
            if (t.KeyConceptsAndRules.EndsWith(g.Replacement,
                StringComparison.Ordinal)) continue;
            if (!t.KeyConceptsAndRules.EndsWith(WrongRule, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "Off-topic clock rule unexpected or partial text drift: " + lesson.LessonCode);
            t.KeyConceptsAndRules =
                t.KeyConceptsAndRules[..^WrongRule.Length] + g.Replacement;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string previous) =>
        IsTarget(doc, lesson) ? CorrectedVersion : previous;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string version) =>
        IsTarget(doc, lesson) &&
        (version == CorrectedVersion || version == ByCode[lesson.LessonCode].PreviousVersion);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(doc, lesson, existingVersion))
            throw new InvalidOperationException("Off-topic clock predecessor not approved.");
        var g = Verify(lesson);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("Off-topic clock translation drift.");
        var old = persisted.Single();
        var current = lesson.Translations.Single();
        if (old.KeyConceptsAndRules != current.KeyConceptsAndRules &&
            old.KeyConceptsAndRules != OriginalConcepts(current.KeyConceptsAndRules, g))
            throw new InvalidOperationException("Off-topic clock concepts diverged.");

        if (old.Title != current.Title ||
            old.Explanation != current.Explanation ||
            old.WorkedExamples != current.WorkedExamples ||
            old.StepByStepSolutions != current.StepByStepSolutions ||
            old.CommonMistakes != current.CommonMistakes ||
            old.QuickSummary != current.QuickSummary)
            throw new InvalidOperationException("Off-topic clock unrelated lesson field drift.");
    }
}

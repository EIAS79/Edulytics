using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// B01 source-bound correction candidate, not academic certification:
/// 2021 Illustrative Mathematics G1 U01 L02 teacher preparation and teacher
/// Activity 1 (matching/drawing dot cards) and Activity 2 (Check It Off).
/// Original publisher pack remains immutable; exact source body identity
/// and prior learner prose are checked before any replacement.
/// </summary>
public static class CommonCoreG1DotCardMatchingCorrection
{
    public const string LessonCode = "PED:US-CCSS-MATH:G1:U01:L02";
    public const string CorrectedVersion = "usccss-b01-g1-u01-l02-source-dot-cards-v1";
    private const string SourceUrl = "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html";
    private const string CanonicalSourceSha = "7d8c9aa59717d264f80522bcf07c5385b5f332b2808b66c5c75fb19672c70e33";
    private static readonly string Original_title = "Explore Expressions and Sums";
    private static readonly string Original_explanation = "In this lesson, you will write addition expressions within 10 and find the sums. It also builds on the work of kindergarten, where you wrote addition expressions to represent images and situations.";
    private static readonly string Original_keyConceptsAndRules = "Learning goals:\n• Write addition expressions within 10 based on images and add in a way that makes sense to you.\n\nKey idea: An addition expression shows the addends being combined. The sum is the total value of the expression, and different addend orders can have the same sum.";
    private static readonly string Original_workedExamples = "Example 1: The source task combines 3 and 2. Start at 3 and count on 2 more to reach 5. Therefore, 3 + 2 = 5.\n\nExample 2: The source task combines 4 and 2. Start at 4 and count on 2 more to reach 6. Therefore, 4 + 2 = 6.\n\nExample 3: The source task combines 5 and 3. Start at 5 and count on 3 more to reach 8. Therefore, 5 + 3 = 8.";
    private static readonly string Original_stepByStepSolutions = "Step 1: Identify the two quantities, 3 and 2.\nStep 2: Start at 3 and count on 2 more, or combine the two groups.\nStep 3: The total is 5, so 3 + 2 = 5.\nStep 4: Check that the result matches the quantities and the lesson goal.";
    private static readonly string Original_commonMistakes = "Do not confuse an expression with its value. The addends form the expression; the sum is the total after the addition is evaluated.";
    private static readonly string Original_quickSummary = "In this lesson, you will write addition expressions within 10 and find the sums. Key idea: An addition expression shows the addends being combined. The sum is the total value of the expression, and different addend orders can have the same sum.";
    public static readonly string Improved_keyConceptsAndRules = "Learning goals:\n• Match a picture of TWO dot groups to the addition expression it represents.\n• Draw dots for a missing part of an expression and find the total within 10.\n• Choose two number cards from 0 to 5, find the sum and record the matching addition expression.\n\nAn addition expression such as 3 + 2 names the two dot groups. The sum, 5, tells how many dots there are altogether. Changing the order of the groups does not change the sum.";
    public static readonly string Improved_workedExamples = "Example 1 — Match dot pictures to an expression. The first picture has ● ● ● (3 dots). The second has ● ● (2 dots). Count the groups separately, then match to 3 + 2, not 3 + 3 or 4 + 2. Counting all dots gives 3 + 2 = 5.\n\nExample 2 — Compare another pair. ● ● ● ● and ● ● contain 4 and 2 dots. Connect their picture to 4 + 2 and find its total: 6. Check by counting each dot once.\n\nExample 3 — Draw the missing dots. The expression is 5 + 2. Draw one group of five dots and another group of exactly two dots: ● ● ● ● ●  and  ● ●. Count the seven dots to check 5 + 2 = 7. The same process also works for 2 + 6 = 8.\n\nExample 4 — Play Check It Off. Take number cards 4 and 3 from a deck using only 0–5. Combine the values, check off 7, and record the expression 4 + 3 by the sum. You can also record 3 + 4 for the same sum.";
    public static readonly string Improved_stepByStepSolutions = "Step 1: Look at each dot picture as a separate group; count its dots once.\nStep 2: Write an addition expression using the two group counts, for example 3 + 2.\nStep 3: Find the sum by combining both groups or counting on: 3, then 4, 5. Check 3 + 2 = 5.\nStep 4: If an expression has a missing dot group, draw exactly as many dots as its missing addend; for 5 + 2, draw two dots beside the group of five.\nStep 5: In Check It Off, choose two number cards no larger than 5, calculate their sum and record the expression next to that total.\nStep 6: Confirm that the displayed picture, written expression and total all describe the SAME two groups, and that the total is no more than 10.";
    public static readonly string Improved_commonMistakes = "Mistake: matching two dot pictures by guessing their total without counting each group. Correction: count the dots in each picture first, then locate that exact addition expression.\nMistake: drawing the total number of dots in the missing group. Correction: draw only the missing addend (for 5 + 2, add two dots, not seven).\nMistake: putting cards larger than 5 into the Check It Off activity. Correction: use only cards 0 through 5 so both selected numbers and their sum stay within 10.\nMistake: treating 4 + 3 as a different sum from 3 + 4. Correction: switching the groups keeps the total 7.";
    public static readonly string Improved_quickSummary = "Match two separate dot groups to an addition expression. Draw a missing group for a given expression and count all dots to check the sum. In Check It Off, pick two cards from 0 to 5, find their sum within 10 and record the expression next to the total.";

    public static bool IsTarget(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l) =>
        d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010" &&
        l.LessonCode == LessonCode;

    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        if (d.PackCode != "US-CCSS-MATH" || d.VersionCode != "CCSSM-2010")
            return;
        foreach (var l in d.Lessons.Where(x => x.LessonCode == LessonCode))
        {
            if (l.SourceUrl != SourceUrl ||
                !string.Equals(l.CanonicalBodySha256, CanonicalSourceSha,
                    StringComparison.OrdinalIgnoreCase) ||
                !l.OutcomeCodes.SequenceEqual(["CCSS:1.OA.C.5","CCSS:1.OA.C.6"]))
                throw new InvalidOperationException(
                    "G1 U01 L02 source URL, canonical body SHA or CCSS mapping changed.");
            var t = l.Translations.Single(x => x.CultureCode == "en");
            if (MatchesImproved(t)) continue;
            if (!MatchesOriginal(t))
                throw new InvalidOperationException(
                    "Refusing G1 U01 L02 content repair on unreviewed or partial learner prose.");
            t.KeyConceptsAndRules = Improved_keyConceptsAndRules;
            t.WorkedExamples = Improved_workedExamples;
            t.StepByStepSolutions = Improved_stepByStepSolutions;
            t.CommonMistakes = Improved_commonMistakes;
            t.QuickSummary = Improved_quickSummary;
        }
    }
    private static bool MatchesOriginal(CanonicalLessonContentPackTranslation t) =>
        t.Title == Original_title &&
        t.Explanation == Original_explanation &&
        t.KeyConceptsAndRules == Original_keyConceptsAndRules &&
        t.WorkedExamples == Original_workedExamples &&
        t.StepByStepSolutions == Original_stepByStepSolutions &&
        t.CommonMistakes == Original_commonMistakes &&
        t.QuickSummary == Original_quickSummary;

    private static bool MatchesImproved(CanonicalLessonContentPackTranslation t) =>
        t.Title == Original_title &&
        t.Explanation == Original_explanation &&
        t.KeyConceptsAndRules == Improved_keyConceptsAndRules &&
        t.WorkedExamples == Improved_workedExamples &&
        t.StepByStepSolutions == Improved_stepByStepSolutions &&
        t.CommonMistakes == Improved_commonMistakes &&
        t.QuickSummary == Improved_quickSummary;

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l, string existing) =>
        IsTarget(d,l) ? CorrectedVersion : existing;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,
        string priorVersion) =>
        IsTarget(d,l) && priorVersion ==
            CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,
        string priorVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(d,l,priorVersion))
            throw new InvalidOperationException(
                "G1 U01 L02 source repair requires the exact previous content version.");
        if (persisted.Count==0) return;
        if (persisted.Count!=1 || persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("G1 U01 L02 unexpected translation cultures.");
        var t = persisted.Single();
        var current = BodyHash(t.Title, t.Explanation, t.KeyConceptsAndRules, t.WorkedExamples, t.StepByStepSolutions, t.CommonMistakes, t.QuickSummary);
        var original = BodyHash(Original_title, Original_explanation, Original_keyConceptsAndRules, Original_workedExamples, Original_stepByStepSolutions, Original_commonMistakes, Original_quickSummary);
        var improved = BodyHash(Original_title, Original_explanation, Improved_keyConceptsAndRules, Improved_workedExamples, Improved_stepByStepSolutions, Improved_commonMistakes, Improved_quickSummary);
        if (current != original && current != improved)
            throw new InvalidOperationException(
                "Refusing G1 U01 L02 upgrade because persisted learner prose diverged.");
    }
    private static string BodyHash(params string[] values) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", values)+"\n")))
            .ToLowerInvariant();
}

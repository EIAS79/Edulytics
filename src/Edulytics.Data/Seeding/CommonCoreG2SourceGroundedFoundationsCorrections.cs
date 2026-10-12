using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Five source-verified Grade 2 instructional defect candidates. Precise, independently
/// authored mathematical replacements are applied only on the original source SHA,
/// source URL, official outcome codes, and complete preceding English-body hash.
/// This is NOT independent academic certification or source licence clearance.
/// </summary>
public static class CommonCoreG2SourceGroundedFoundationsCorrections
{
    public const string CorrectedContentVersion =
        "us-ccss-g2-source-grounded-foundations-v1";

    private sealed record Repair(
        string Code,
        string Url,
        string SourceSha,
        string OriginalEnglishBodySha,
        string[] Outcomes,
        string Concepts,
        string Examples,
        string Steps,
        string Mistakes);

    private static readonly Repair[] Repairs =
    [
        new Repair(
            "PED:US-CCSS-MATH:G2:U01:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-1/preparation.html",
            "e44c70959ac06629edd2682e60c4e124614a6875d932628f597dd3a7ddc43dbe",
            "b9335829c914c4363f625f25f6287366a3f9b02fb1b90716cfdea7705a85d2df",
            ["CCSS:2.OA.B.2"],
            "Addition combines parts to make a total; subtraction finds an amount left, a missing part, or a difference.\nWithin 10, make a ten with pairs such as 6 + 4 and 7 + 3; use a known fact or count on or back when helpful.\nRelated facts check each other: 7 + 3 = 10, so 10 − 3 = 7.",
            "Example 1 — Add within 10. Seven counters and two more make 7 + 2 = 9. Count 7, 8, 9 to check.\nExample 2 — Subtract within 10. A ten-frame has 9 counters; remove 3. Then 9 − 3 = 6. Check: 6 + 3 = 9.\nExample 3 — Find more than one expression for 5: 1 + 4 = 5 and 8 − 3 = 5. For 10, 6 + 4 = 10 and 10 + 0 = 10.",
            "Step 1: Read the addition or subtraction sign and identify the numbers, all within 10.\nStep 2: Represent the expression with counters, a ten-frame, or a remembered fact.\nStep 3: Count on or back, make a ten, or use a known relationship to calculate.\nStep 4: State the expression and answer; for example, 8 − 3 = 5.\nStep 5: Verify with the inverse relationship, such as 5 + 3 = 8, or recount the objects.",
            "Mistake: treating 8 − 3 as addition or taking away more than the starting amount. Read the operation and count exactly three removed counters.\nMistake: assuming an addition expression has only one way to make a target; 5 can be 1 + 4 or 2 + 3."),
        new Repair(
            "PED:US-CCSS-MATH:G2:U06:L07",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-7/preparation.html",
            "44bc4f51d84f720f52a10e6b0751a03677bfc8a23964df62b0d5fa1fa092d3f2",
            "b73e4f984f507471975e1a0d2f2789f6b81b308ee398142409e658657ce585df",
            ["CCSS:2.G.A.3"],
            "A half is one of 2 equal-area parts of the same whole, a third is one of 3, and a fourth or quarter is one of 4.\nThe parts must be equal in size. Merely drawing 2, 3, or 4 differently sized pieces does not create halves, thirds, or fourths.\nFold or draw lines on a rectangle or circle, then explain how the pieces cover the entire shape with equal area.",
            "Example 1 — Halves. Partition a 12-square-unit rectangle into 2 equal parts of 6 square units each. Each piece is one half.\nExample 2 — Thirds. Divide that same rectangle into 3 parts of 4 square units each. Each part is one third.\nExample 3 — Fourths. Divide a 12-square-unit rectangle into 4 pieces of 3 square units each. Each is a fourth (quarter). Four unequal parts, such as 2, 3, 3, and 4 units, are not fourths.",
            "Step 1: Name the whole circle or rectangle you are partitioning.\nStep 2: Decide whether to make 2, 3, or 4 equal-area pieces.\nStep 3: Draw or fold the shape so every part has equal area and together they cover the whole.\nStep 4: Name one part a half, third, or fourth/quarter according to the number of equal pieces.\nStep 5: Check equal size and full coverage; count pieces alone is not enough.",
            "Mistake: calling one of 3 unequal parts a third. Three parts must each be one third of the same whole.\nMistake: counting pieces from different-sized wholes as equivalent; compare parts only relative to the same whole."),
        new Repair(
            "PED:US-CCSS-MATH:G2:U06:L08",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-8/preparation.html",
            "5fa6bc7c47b5fd908aa23cdd90eb004faaaebf98c4aa40f4080739bbdbd5cd64",
            "33c1df0d9810d23c900b05efc8dc7ffcb0fd0de77a1b61125788e8a0e3333324",
            ["CCSS:2.G.A.3", "CCSS:2.NBT.A.2"],
            "Equal fractional pieces of the SAME whole can have different shapes. They must have equal area, not necessarily identical outlines.\nA rectangle divided into 4 equal-area regions has fourths even when some regions are triangles and others are rectangles.\nTo decide whether pieces are equal, cover them with the same unit squares, compare areas, or rearrange pieces without gaps or overlaps. Skip-counting by tens is separate place-value practice.",
            "Example 1 — Same-sized fourths, different shapes. Draw a 4-by-3 rectangle with area 12 square units. Two equal-area partitions can use four 1-by-3 strips or four 3-square-unit regions arranged differently. Each part is one fourth because each has area 3.\nExample 2 — Not fourths. If a 12-square-unit whole is split into regions of area 2, 3, 3, and 4, the regions are not equal; none should be labeled a fourth.\nPlace-value warm-up (2.NBT.A.2): Count by tens: 40, 50, 60, 70, 80. This is not a method for deciding fractional area.",
            "Step 1: Keep the whole shape unchanged when comparing two partitions.\nStep 2: Count the number of regions and identify each region's area with unit squares or a justified decomposition.\nStep 3: Confirm each region covers an equal portion of that same whole.\nStep 4: Name pieces as halves, thirds, or fourths when the area and count meet the definition, even if their shapes differ.\nStep 5: Reject partitions with unequal areas and explain which comparison proves the difference.",
            "Mistake: requiring each fourth to have the same outline; equal AREA, not identical shape, is essential.\nMistake: calling four differently sized regions fourths solely because there are four of them."),
        new Repair(
            "PED:US-CCSS-MATH:G2:U06:L09",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-9/preparation.html",
            "ba42dfc57607015de8bf0bb2898979dafbc2519a1f0fa8347f4b8b2a14b6cddc",
            "92863635095c3f03b41faeb5c1b8503eadbce9d098de4682245e25aecbb70317",
            ["CCSS:2.G.A.3", "CCSS:2.NBT.A.2"],
            "The whole can be composed of 2 equal halves, 3 equal thirds, or 4 equal fourths (quarters).\nOne half is one of 2 equal pieces of a whole; a third is one of 3; a quarter is one of 4. Name both the part and the original whole.\nPutting ALL equal pieces together recreates exactly one whole with no overlap or missing region.",
            "Example 1 — Halves. Divide a paper circle into 2 equal-area parts and shade both: 2 halves make 1 whole.\nExample 2 — Thirds. Divide an equal-size rectangle into 3 equal-area parts. Shade all 3: 3 thirds make 1 whole.\nExample 3 — Quarters. Partition a circle into 4 equal-area regions. With 3 shaded, one quarter is missing; shade the fourth to make 1 whole.\nPlace-value warm-up (2.NBT.A.2): Count by tens: 80, 90, 100, 110, 120. This is different from counting fractional pieces.",
            "Step 1: Identify one original whole shape, such as one circle or rectangle.\nStep 2: Partition it into 2, 3, or 4 equal-area parts.\nStep 3: Name each part as a half, third, or fourth/quarter.\nStep 4: Put together or shade all 2 halves, all 3 thirds, or all 4 fourths.\nStep 5: Check the pieces cover the original whole exactly, without gaps or overlaps.",
            "Mistake: describing 3 thirds as less than a whole because each individual third is small. All 3 thirds cover the entire original whole.\nMistake: combining pieces from different-sized wholes; pieces must refer to the same whole."),
        new Repair(
            "PED:US-CCSS-MATH:G2:U09:L05",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-5/preparation.html",
            "eadddc5d9f13e9433dc593a4b6a7cdc0a5b99e64e1b245a66fab459069ae4f25",
            "d7900b4490916b371c70b5303c904fb745327c4ca856d502472df06b7b5ee453",
            ["CCSS:2.NBT.A.1"],
            "Three-digit numbers describe hundreds, tens, and ones. In 356, the digits mean 3 hundreds, 5 tens, and 6 ones.\nYou can trade 1 hundred for 10 tens, and 1 ten for 10 ones, without changing the value.\nComposing and decomposing 356 produces equivalent base-ten models, equations, and words, even when a model uses more than 9 tens or ones.",
            "Example 1 — Show 356 as 3 hundreds + 5 tens + 6 ones: 300 + 50 + 6 = 356.\nExample 2 — Trade one hundred for ten tens: 2 hundreds + 15 tens + 6 ones = 200 + 150 + 6 = 356.\nExample 3 — Trade all hundreds for tens: 35 tens + 6 ones = 350 + 6 = 356. As ones, it is 356 ones.\nExample 4 — Explore zero tens: 308 is 3 hundreds + 0 tens + 8 ones, or 2 hundreds + 10 tens + 8 ones. Both show 308.",
            "Step 1: Write the three-digit number and identify the value of each digit.\nStep 2: Build it with hundreds, tens, and ones, or draw a matching base-ten diagram.\nStep 3: Exchange one hundred for ten tens or one ten for ten ones, keeping the value the same.\nStep 4: Record another representation in blocks, words, or a place-value equation, such as 200 + 150 + 6 = 356.\nStep 5: Recount all represented hundreds, tens, and ones to verify the total is unchanged.",
            "Mistake: confusing the digit 5 in 356 with 5 ones; it means 5 tens, or 50.\nMistake: exchanging one hundred for one ten; one hundred equals TEN tens.\nMistake: believing 2 hundreds + 15 tens + 6 ones is a different number from 356.")
    ];

    public static IReadOnlyCollection<string> TargetLessonCodes =>
        Repairs.Select(x => x.Code).ToArray();

    private static Repair? Find(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        // The source-anchored Edulytics correction applies ONLY to the
        // committed canonical CCSS lesson-content edition. Ad-hoc pedagogical
        // fixtures may legitimately use the same official LessonCode with
        // their own reviewed content/version, and must remain independently
        // subject to the generic seeder's same-version body drift checks.
        string.Equals(document.PackCode, MathematicsCurriculumPackRegistry.CommonCoreCode,
            StringComparison.Ordinal) &&
        string.Equals(document.VersionCode, "CCSSM-2010", StringComparison.Ordinal) &&
        string.Equals(document.ContentVersion, "phase30-common-core-rich-content-v1",
            StringComparison.Ordinal) ?
            Repairs.SingleOrDefault(x => string.Equals(x.Code, lesson.LessonCode,
                StringComparison.Ordinal)) :
            null;

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) => Find(document, lesson) is not null;

    private static void AssertIdentity(
        CanonicalLessonContentPackLesson lesson,
        Repair repair)
    {
        if (!string.Equals(lesson.SourceUrl, repair.Url, StringComparison.Ordinal) ||
            !string.Equals(lesson.CanonicalBodySha256, repair.SourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(repair.Outcomes))
            throw new InvalidOperationException(
                $"Grade 2 source / outcome identity drift for {lesson.LessonCode}.");
    }

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(document.PackCode, MathematicsCurriculumPackRegistry.CommonCoreCode,
                StringComparison.Ordinal) ||
            !string.Equals(document.VersionCode, "CCSSM-2010", StringComparison.Ordinal))
            return;

        foreach (var lesson in document.Lessons)
        {
            var repair = Find(document, lesson);
            if (repair is null)
                continue;

            AssertIdentity(lesson, repair);
            var english = lesson.Translations.Single(t =>
                string.Equals(t.CultureCode, "en", StringComparison.Ordinal));
            var existingSha = ComputeBodySha(
                english.CultureCode, english.Title, english.Explanation,
                english.KeyConceptsAndRules, english.WorkedExamples,
                english.StepByStepSolutions, english.CommonMistakes, english.QuickSummary);

            if (MatchesCorrection(english, repair))
                continue;

            if (!string.Equals(existingSha, repair.OriginalEnglishBodySha,
                    StringComparison.Ordinal))
                throw new InvalidOperationException(
                    $"Unexpected or partially edited Grade 2 lesson {lesson.LessonCode}; refusing overwrite.");

            english.KeyConceptsAndRules = repair.Concepts;
            english.WorkedExamples = repair.Examples;
            english.StepByStepSolutions = repair.Steps;
            english.CommonMistakes = repair.Mistakes;
        }
    }

    private static bool MatchesCorrection(
        CanonicalLessonContentPackTranslation current,
        Repair repair) =>
        string.Equals(current.KeyConceptsAndRules, repair.Concepts, StringComparison.Ordinal) &&
        string.Equals(current.WorkedExamples, repair.Examples, StringComparison.Ordinal) &&
        string.Equals(current.StepByStepSolutions, repair.Steps, StringComparison.Ordinal) &&
        string.Equals(current.CommonMistakes, repair.Mistakes, StringComparison.Ordinal);

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string previousVersion) =>
        IsTarget(document, lesson) ? CorrectedContentVersion : previousVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion) =>
        IsTarget(document, lesson) &&
        string.Equals(existingVersion,
            CommonCoreRichContentCorrections.CorrectionContentVersion,
            StringComparison.Ordinal);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persistedTranslations)
    {
        var repair = Find(document, lesson) ??
            throw new InvalidOperationException("Not a Grade 2 foundation correction lesson.");
        AssertIdentity(lesson, repair);

        if (!CanUpgradeExisting(document, lesson, existingVersion))
            throw new InvalidOperationException(
                "Grade 2 correction requires exact preceding Common Core content version.");

        // In ReadFromJson production, only the lesson content metadata is persisted.
        if (persistedTranslations.Count == 0)
            return;

        if (persistedTranslations.Count != 1 ||
            !string.Equals(persistedTranslations.Single().CultureCode, "en",
                StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Unexpected Grade 2 persisted language set; refusing upgrade.");

        var current = persistedTranslations.Single();
        var currentHash = ComputeBodySha(
            current.CultureCode, current.Title, current.Explanation,
            current.KeyConceptsAndRules, current.WorkedExamples,
            current.StepByStepSolutions, current.CommonMistakes, current.QuickSummary);

        var newEnglish = lesson.Translations.Single(t => t.CultureCode == "en");
        var expectedCorrectedHash = ComputeBodySha(
            newEnglish.CultureCode, newEnglish.Title, newEnglish.Explanation,
            newEnglish.KeyConceptsAndRules, newEnglish.WorkedExamples,
            newEnglish.StepByStepSolutions, newEnglish.CommonMistakes,
            newEnglish.QuickSummary);

        if (!string.Equals(currentHash, repair.OriginalEnglishBodySha,
                StringComparison.Ordinal) &&
            !string.Equals(currentHash, expectedCorrectedHash,
                StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Unexpected persisted Grade 2 lesson body {lesson.LessonCode}; refusing overwrite.");
    }

    private static string ComputeBodySha(params string[] fields) =>
        Convert.ToHexString(SHA256.HashData(
            Encoding.UTF8.GetBytes(string.Join("\n", fields) + "\n")))
            .ToLowerInvariant();
}

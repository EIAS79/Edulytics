using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 B01 two verified instructional root-cause defects: an unrelated
/// linear-ruler template in G3 gram/kilogram weight and G4 10,000 place-value
/// instruction. Goals were checked against original 2021 preparation pages
/// on authorized read-only Our-CS. Replacement examples are newly authored.
/// Neither this repair nor successful unit tests certify L1–L6.
/// </summary>
public static class CommonCoreB01UnitAndPlaceValuePurposeCorrections
{
    public const string CorrectedVersion = "usccss-b01-unrelated-ruler-template-v1";
    private sealed record Repair(
        string Code, string Url, string RawSha, string SourceSemanticSha, string OutcomeCodes,
        string Title, string Explanation, string Summary, string OldConcepts,
        string OldExamples, string OldSteps, string OldMistakes, string NewConcepts,
        string NewExamples, string NewSteps, string NewMistakes);

    private static readonly Repair[] Repairs =
    [
        new Repair(
            "PED:US-CCSS-MATH:G3:U06:L06",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-6/lesson-6/preparation.html",
            "448b2775ed29334fe0f76f2624b0590ae733b4004f03b1b87a39a03d84ab1521",
            "5e6a3f410e4f84e04f476636a4b478c970841abe61412e6aa227d5bb87bff429",
            "CCSS:3.MD.A.2",
            "Estimate and Measure Weight",
            "Learning goal: You will learn to measure and estimate the weight of objects in grams or kilograms.\nIn previous grades, you learned that weight is a measurable attribute and directly compared the weights of two objects. In this lesson, you learn that weight is a measure of how heavy something is. You are introduced to grams and kilograms as metric units for measuring weight.",
            "You will learn to measure and estimate the weight of objects in grams or kilograms. In previous grades, you learned that weight is a measurable attribute and directly compared the weights of two objects.",
            "Key ideas:\n- Measure and estimate weights of objects using standard units of grams (g) and kilograms (kg)\n- In previous grades, you learned that weight is a measurable attribute and directly compared the weights of two objects\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: What do you notice? What do you wonder? Attribution: By terimakasih0. Pixabay. Pixabay. Source .\nApproach: “How could weight be used to decide how much a piece of fruit should cost?” (The more the fruit weighs, the more it’s going to cost because we’re getting more.)\nExample 2: This paper clip weighs about 1 gram. Attribution: By Jess Bailey Designs. Pexel License. Pexels. Source . This basket of apples weighs about 1 kilogram. Attribution: By Pixabay. Pexel License. Pexels. Source . For each weight measurement, find an example of something you think has about the same weight. 1 gram 10 grams 100 grams 1 kilogram 2 kilograms...\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- We estimate how heavy an object is using grams (g) for lighter objects and kilograms (kg) for heavier objects.\n- A kilogram equals 1,000 grams. Choose an appropriate unit before reading a scale.\n- A measured value can be compared with an estimate by finding the numerical difference in the same unit.",
            "Example 1: A learner predicts that a small lunch box weighs about 250 g. A classroom scale shows 280 g. How much higher is the measured weight than the estimate?\nSolution: 280 g - 250 g = 30 g. The estimate was 30 g too low.\nExample 2: One sealed package reads 1 kg on a scale and another reads 650 g. Which package is heavier, and by how much?\nSolution: 1 kg = 1,000 g. Calculate 1,000 g - 650 g = 350 g. The 1 kg package is 350 g heavier.",
            "Step 1: Decide whether grams or kilograms are suitable for the object and explain why.\nStep 2: Estimate the object's weight using that unit and record your estimate.\nStep 3: If using a scale, place the object correctly and read the displayed value with its unit.\nStep 4: Convert to a common unit if you need to compare two weights (1 kg = 1,000 g).\nStep 5: Subtract or compare the estimate and measurement, then state the result with an appropriate unit.",
            "Mistake: applying ruler tick marks or centimeter endpoints to a weighing task. Use a weighing scale and read grams or kilograms, not length.\nMistake: comparing 1 kg directly with 650 g as if 1 were smaller than 650. First convert 1 kg to 1,000 g, then compare."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U04:L06",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-6/preparation.html",
            "a1b7caf9e850ebeff56480ea5c6ea1faf4f784ed3766bca7f4f6cddf2df70bab",
            "9a4a0fce2f61cbe38200afabe77cbe4519d3bde55a610239923303b2ac3a86f8",
            "CCSS:4.NBT.A.1",
            "How Much is 10,000? PLC Activity",
            "Learning goal: You will develop a relative sense of ten-thousand and understand it as a unit consisting of 10 units of one-thousand.\nIn this lesson, you build on your understanding of the base-ten structure to develop a sense of the magnitude of 10,000. You first use base-ten blocks and base-ten diagrams to build four-digit and five-digit numbers. You then use a 10-by-10 grid to represent 100 and work together to build a representation of 1,000, and then 10,000.",
            "You will develop a relative sense of ten-thousand and understand it as a unit consisting of 10 units of one-thousand. In this lesson, you build on your understanding of the base-ten structure to develop a sense of the magnitude of 10,000.",
            "Key ideas:\n- Develop a sense of the relative magnitude of 10,000\n- Recognize ten-thousand as 10 groups of 1,000\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example: develop a relative sense of ten-thousand and understand it as a unit consisting of 10 units of one-thousand.\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- One ten-thousand contains 10 groups of one thousand, so 10 × 1,000 = 10,000.\n- Every whole-number place is ten times the place immediately to its right.\n- A place-value chart or groups of base-ten blocks can show why 10,000 is ten times 1,000.",
            "Example 1: A library stacks books into 10 equal groups of 1,000 books. How many books are there altogether?\nSolution: 10 groups × 1,000 books per group = 10,000 books. The same amount is one group of ten-thousand.\nExample 2: A place-value chart has 1 ten-thousand, 0 thousands, 0 hundreds, 0 tens, and 0 ones. What number is represented?\nSolution: 1 × 10,000 + 0 × 1,000 + 0 × 100 + 0 × 10 + 0 × 1 = 10,000. We write five digits, including the four zeros.",
            "Step 1: Start with one group representing 1,000.\nStep 2: Make or imagine 10 equal groups, each containing 1,000.\nStep 3: Multiply 10 × 1,000 to find the total 10,000.\nStep 4: Exchange 10 groups of 1,000 for 1 group of 10,000 on a place-value chart.\nStep 5: Explain why the new place is ten times the thousands place and check the five-digit numeral.",
            "Mistake: treating 10,000 as 10 hundreds rather than 10 thousands. Ten hundreds make 1,000; ten thousands make 10,000.\nMistake: writing 1,000 when asked for one ten-thousand. Include all four zero place holders: 10,000."
        )
    ];
    private static readonly Dictionary<string, Repair> ByCode =
        Repairs.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == "US-CCSS-MATH" && document.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(lesson.LessonCode);

    private static void VerifySource(CanonicalLessonContentPackLesson lesson, Repair repair)
    {
        if (lesson.SourceUrl != repair.Url ||
            !string.Equals(lesson.CanonicalBodySha256, repair.RawSha,
                StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", lesson.OutcomeCodes) != repair.OutcomeCodes ||
            lesson.Translations.Count != 1 || lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException(
                "V9 B01 unexpected source identity, language, CCSS or learner baseline: " + lesson.LessonCode);
    }

    private static bool Same(CurriculumLessonContentTranslation t, Repair r, bool after) =>
        t.Title == r.Title && t.Explanation == r.Explanation && t.QuickSummary == r.Summary &&
        t.KeyConceptsAndRules == (after ? r.NewConcepts : r.OldConcepts) &&
        t.WorkedExamples == (after ? r.NewExamples : r.OldExamples) &&
        t.StepByStepSolutions == (after ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (after ? r.NewMistakes : r.OldMistakes);

    private static bool Same(CanonicalLessonContentPackLessonTranslation t, Repair r, bool after) =>
        t.Title == r.Title && t.Explanation == r.Explanation && t.QuickSummary == r.Summary &&
        t.KeyConceptsAndRules == (after ? r.NewConcepts : r.OldConcepts) &&
        t.WorkedExamples == (after ? r.NewExamples : r.OldExamples) &&
        t.StepByStepSolutions == (after ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (after ? r.NewMistakes : r.OldMistakes);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != "US-CCSS-MATH" ||
            document.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in document.Lessons)
        {
            if (!ByCode.TryGetValue(lesson.LessonCode, out var repair)) continue;
            VerifySource(lesson, repair);
            var t = lesson.Translations[0];
            if (Same(t, repair, true)) continue;
            if (!Same(t, repair, false))
                throw new InvalidOperationException(
                    "V9 B01 rejected partial/unreviewed instructional replacement: " + lesson.LessonCode);
            t.KeyConceptsAndRules = repair.NewConcepts;
            t.WorkedExamples = repair.NewExamples;
            t.StepByStepSolutions = repair.NewSteps;
            t.CommonMistakes = repair.NewMistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string previous) =>
        IsTarget(document, lesson) ? CorrectedVersion : previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string version) =>
        IsTarget(document, lesson) &&
        (version == CommonCoreRichContentCorrections.CorrectionContentVersion ||
         version == CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(document, lesson, version))
            throw new InvalidOperationException("V9 B01 unexpected content version predecessor.");
        var repair = ByCode[lesson.LessonCode];
        VerifySource(lesson, repair);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 B01 unexpected persisted culture.");
        var t = persisted.Single();
        if (!Same(t, repair, true) && !Same(t, repair, false))
            throw new InvalidOperationException("V9 B01 stored learner body drift: " + lesson.LessonCode);
    }
}

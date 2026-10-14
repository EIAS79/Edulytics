using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 B01: one root-cause cohort, three teacher-source-verified non-length lessons.
/// The length-ruler template is unrelated to their exact objectives. This is a
/// bounded correction, not automatic academic L1-L6 certification.
/// </summary>
public static class CommonCoreB01NonLengthRulerTemplateCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-nonlength-ruler-cohort-v1";
    private sealed record Repair(string Code, string Url, string RawSha, string Outcomes,
        string Title, string Explanation, string Summary,
        string OldConcepts, string OldExamples, string OldSteps, string OldMistakes,
        string NewConcepts, string NewExamples, string NewSteps, string NewMistakes);

    private static readonly Repair[] Repairs =
    [
        new Repair(
                        "PED:US-CCSS-MATH:G2:U05:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-1/preparation.html",
            "8b95f1c712928a475193eb9bb76a77698d724a68e85231a340721f7531f018a1",
            "CCSS:2.NBT.A.1|CCSS:2.NBT.A.2",
            "How Do We Compose a Hundred?",
            "Learning goal: You will make sense of a hundred as a unit composed of 10 tens or 100 ones.\nIn grade 1, you were introduced to a ten as a unit made of 10 ones. You used that understanding to represent two-digit numbers and add within 100. You used connecting cubes to make and break apart two-digit numbers.",
            "You will make sense of a hundred as a unit composed of 10 tens or 100 ones. In grade 1, you were introduced to a ten as a unit made of 10 ones.",
            "Key ideas:\n- Recognize that each hundred is composed of 100 ones or 10 tens\n- In grade 1, you were introduced to a ten as a unit made of 10 ones\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: How many do you see? How do you see them? Andre added more blocks. What is the value of Andre’s blocks now? How many tens and ones are in this number? Andre made the same number with the fewest amount of blocks possible. Draw a base-ten diagram to show what Andre’s number looks like now. Use your base-ten blocks to help.\nApproach: “What is a number we could place in the fourth row? Use a pattern we discussed to explain how you know your number would belong.” (You could put 122 in the group with 92, 102, and 112. The ones place has a 2, it is getting bigger by 10, and I think the 1 should stay the same like in 102 and 112.)\nExample 2: Three students were looking at 100 small squares arranged like this: Match the diagrams to the statements. Label each diagram with A, B, or C. Be prepared to explain your matches. Priya said, “I see 100 ones.” Kiran said, “I see 10 tens.” Lin said, “I see 1 hundred.” ___ ___ ___ Represent \\(100 + 11\\) with blocks or a diagram.\nApproach: “If Andre used the fewest number of blocks, how many tens did he use?” (He had 10 tens.) “What if Andre wanted to use the greatest amount of blocks to show this number? What would he do?” (He’d decompose all the tens to use only ones. He’d use 100 ones.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- One hundred is exactly 10 groups of ten or 100 individual ones.\n- Grouping 10 ones makes 1 ten; grouping 10 tens makes 1 hundred.\n- The same number can be represented using a hundred block, ten tens, or one hundred ones.",
            "Example 1: A class has 7 bundles of 10 counters and 30 loose counters. How many counters are there?\nSolution: 7 × 10 + 30 = 70 + 30 = 100 counters. Regroup the 30 ones into 3 tens to make 10 tens, or 1 hundred.\nExample 2: A pupil builds 100 using 10 ten-rods. How many individual cubes would be needed to make the same amount?\nSolution: Each ten-rod represents 10 cubes; 10 × 10 = 100 cubes. Both models represent the same hundred.",
            "Step 1: Identify groups of tens and individual ones in the representation.\nStep 2: Count 10 for each ten and 1 for each one.\nStep 3: Regroup every 10 ones into a ten, then every 10 tens into a hundred.\nStep 4: Check the total using 100 ones = 10 tens = 1 hundred.",
            "Mistake: treating 10 tens as 10 ones. Ten groups of ten make 100, not 10.\nMistake: counting both a hundred block and its ten constituent ten-rods as different amounts. They are alternative representations of the same 100."
        ),
        new Repair(
                        "PED:US-CCSS-MATH:G3:U07:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-2/preparation.html",
            "6ea5ddf667fe820b72879c2440cbb4d5d6f585e98015cd48eb5ff515f2afa31b",
            "CCSS:3.G.A.1|CCSS:3.NBT.A.3",
            "Attributes of Triangles and Quadrilaterals",
            "Learning goal: You will use attributes of triangles and quadrilaterals to sort them into more specific categories.\nIn the previous lesson, you learned that shapes could be sorted into more than one category depending on the attributes being considered. In this lesson, you learn that shapes with the same number of sides can be further sorted into subgroups based on other attributes. As you sort triangles and quadrilaterals into subgroups, you focus on attributes other than the number of sides, such as the length of the sides or the number of angles.",
            "You will use attributes of triangles and quadrilaterals to sort them into more specific categories. In the previous lesson, you learned that shapes could be sorted into more than one category depending on the attributes being considered.",
            "Key ideas:\n- Sort triangles and quadrilaterals into subcategories\n- Understand that shared attributes of shapes can define a larger category, such as triangle or quadrilateral\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(3\\times60=9\\times10\\) \\(180=3\\times60\\) \\(6\\times40=24\\times10\\) \\(24\\times10=240\\)\nApproach: Represent the quantities, write a matching addition relationship, then combine or count on to find and check the total.\nExample 2: Sort the triangles into categories. Record your categories and be prepared to explain how you sorted the shapes.\nApproach: Represent the quantities, write a matching addition relationship, then combine or count on to find and check the total.",
            "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.",
            "Common mistake: counting an item twice or combining the wrong quantities. Check the addends and verify the total.",
            "Key ideas:\n- A triangle has exactly 3 straight sides; a quadrilateral has exactly 4.\n- Within each broad category, classify shapes using shared attributes such as equal side lengths, parallel sides, and right angles.\n- A shape may belong to multiple nested categories: every square is a rectangle, and both are quadrilaterals.",
            "Example 1: One triangle has side lengths 5 cm, 5 cm, and 3 cm; another has three sides of different lengths. How can we sort them?\nSolution: The first has two equal sides, so it belongs in an isosceles-triangle group. The second is scalene. Both are triangles because they have three sides.\nExample 2: A four-sided figure has 4 right angles and 4 equal sides. Does it belong to the rectangle category as well as the square category?\nSolution: Yes. Its four right angles make it a rectangle; its four equal sides also make it a square. Both categories are quadrilaterals.",
            "Step 1: Count the straight sides to decide whether a shape is a triangle or quadrilateral.\nStep 2: Compare relevant properties, including equal sides, parallel sides, and right angles.\nStep 3: Sort shapes into more specific groups based on shared properties.\nStep 4: Explain each classification using the observed attributes, not the shape's orientation.\nStep 5: Check whether a shape belongs to more than one valid group.",
            "Mistake: sorting only by how a shape is rotated or how large it looks. Orientation and size do not change the number of sides or right angles.\nMistake: assuming a square cannot also be a rectangle. A square has all the properties required for a rectangle."
        ),
        new Repair(
                        "PED:US-CCSS-MATH:G4:U04:L19",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-19/preparation.html",
            "79bd6d01fc5dae31f38e5730438b26c3649ce3849c7af656c8ef1d86a4d7e12f",
            "CCSS:4.NBT.B.4|CCSS:4.NF.B.3",
            "Compose and Decompose to Add and Subtract",
            "Learning goal: You will review how to add and subtract multi-digit numbers with composition and decomposition.\nIn earlier grades, you encountered subtraction problems which required decomposing a unit. In this lesson, you review this idea of decomposition and revisit how it is recorded when using the standard algorithm to subtract numbers through the thousands place. You also use composition to add multi-digit numbers through the hundred-thousands place. - Representation",
            "You will review how to add and subtract multi-digit numbers with composition and decomposition. In earlier grades, you encountered subtraction problems which required decomposing a unit.",
            "Key ideas:\n- Add and subtract multi-digit numbers, with composing or decomposing, using the standard algorithm\n- In earlier grades, you encountered subtraction problems which required decomposing a unit\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Add \\(8,299+1,111\\).\nApproach: Align place values. Ones: \\(9+1=10\\), so write 0 ones and compose 1 ten. Tens: \\(9+1+1=11\\), so write 1 ten and compose 1 hundred. Continue by place value to get \\(8,299+1,111=9,410\\).\n\nExample 2: Subtract \\(5,203-2,786\\).\nApproach: Decompose as needed so each place can be subtracted. The standard algorithm gives \\(5,203-2,786=2,417\\). Check with the inverse operation: \\(2,417+2,786=5,203\\).",
            "Step 1: Write the numbers vertically with ones, tens, hundreds, and thousands aligned.\nStep 2: Add or subtract one place at a time from right to left.\nStep 3: When a place has 10 or more, compose 10 units into 1 unit of the next place; when subtraction needs more units, decompose 1 unit from the next place.\nStep 4: Record each composed or decomposed unit clearly in the standard algorithm.\nStep 5: Check subtraction with the inverse operation or check addition with estimation.",
            "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
            "Key ideas:\n- In the standard addition and subtraction algorithms, align digits by their place values before calculating.\n- Compose 10 ones as 1 ten, 10 tens as 1 hundred, and so on when carrying in addition.\n- Decompose 1 unit of a higher place into 10 units of the next lower place when subtracting.\n- Check subtraction by adding the difference to the subtrahend; check addition using place-value estimates.",
            "Example 1: Add \\(8,299+1,111\\).\nApproach: Align place values. Ones: \\(9+1=10\\), so write 0 ones and compose 1 ten. Tens: \\(9+1+1=11\\), so write 1 ten and compose 1 hundred. Continue by place value to get \\(8,299+1,111=9,410\\).\n\nExample 2: Subtract \\(5,203-2,786\\).\nApproach: Decompose as needed so each place can be subtracted. The standard algorithm gives \\(5,203-2,786=2,417\\). Check with the inverse operation: \\(2,417+2,786=5,203\\).",
            "Step 1: Write the numbers vertically with ones, tens, hundreds, and thousands aligned.\nStep 2: Add or subtract one place at a time from right to left.\nStep 3: When a place has 10 or more, compose 10 units into 1 unit of the next place; when subtraction needs more units, decompose 1 unit from the next place.\nStep 4: Record each composed or decomposed unit clearly in the standard algorithm.\nStep 5: Check subtraction with the inverse operation or check addition with estimation.",
            "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation."
        )
    ];

    private static readonly IReadOnlyDictionary<string, Repair> ByCode =
        Repairs.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static bool IsTarget(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        document.PackCode == "US-CCSS-MATH" && document.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(lesson.LessonCode);

    private static void VerifySource(CanonicalLessonContentPackLesson lesson, Repair repair)
    {
        if (lesson.SourceUrl != repair.Url ||
            !string.Equals(lesson.CanonicalBodySha256, repair.RawSha, StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", lesson.OutcomeCodes) != repair.Outcomes ||
            lesson.Translations.Count != 1 || lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException("V9 non-length ruler source/body drift: " + lesson.LessonCode);
    }

    private static bool Same(CanonicalLessonContentPackTranslation t, Repair r, bool after) =>
        t.Title == r.Title && t.Explanation == r.Explanation && t.QuickSummary == r.Summary &&
        t.KeyConceptsAndRules == (after ? r.NewConcepts : r.OldConcepts) &&
        t.WorkedExamples == (after ? r.NewExamples : r.OldExamples) &&
        t.StepByStepSolutions == (after ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (after ? r.NewMistakes : r.OldMistakes);

    private static bool Same(CurriculumLessonContentTranslation t, Repair r, bool after) =>
        t.Title == r.Title && t.Explanation == r.Explanation && t.QuickSummary == r.Summary &&
        t.KeyConceptsAndRules == (after ? r.NewConcepts : r.OldConcepts) &&
        t.WorkedExamples == (after ? r.NewExamples : r.OldExamples) &&
        t.StepByStepSolutions == (after ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (after ? r.NewMistakes : r.OldMistakes);

    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        if (document.PackCode != "US-CCSS-MATH" || document.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in document.Lessons)
        {
            if (!ByCode.TryGetValue(lesson.LessonCode, out var repair)) continue;
            VerifySource(lesson, repair);
            var t = lesson.Translations[0];
            if (Same(t, repair, true)) continue;
            if (!Same(t, repair, false))
                throw new InvalidOperationException("V9 non-length ruler partial/unreviewed update: " + lesson.LessonCode);
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
        (version == CommonCoreRichContentCorrections.CorrectionContentVersion || version == CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson, string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(document, lesson, version))
            throw new InvalidOperationException("V9 non-length ruler unexpected version predecessor.");
        var repair = ByCode[lesson.LessonCode];
        VerifySource(lesson, repair);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 non-length ruler unexpected persisted culture.");
        var t = persisted.Single();
        if (!Same(t, repair, false) && !Same(t, repair, true))
            throw new InvalidOperationException("V9 non-length ruler persisted body drift: " + lesson.LessonCode);
    }
}

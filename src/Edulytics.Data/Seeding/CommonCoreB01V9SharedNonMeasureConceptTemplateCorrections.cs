using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 source-verified ROOT-CAUSE correction of one unrelated measurement-rule
/// line across 23 non-length instructional objectives. Four members also share
/// the inappropriate ruler procedure; their replacements use the correct
/// mathematical family, not separate ad-hoc lesson rewrites.
/// The 18 legitimate length and 4 number-line counterparts are untouched.
/// This cohort is NOT an L1-L6 or batch certificate.
/// </summary>
public static class CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-nonmeasure-shared-template-23-v1";
    private const string IrrelevantRule = "\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.";
    private sealed record Guard(
        string Code, string SourceUrl, string RawSha, string Outcomes,
        string OldConcepts, string OldSteps, string OldMistakes, string Mode);

    private static readonly Guard[] Cohort =
    [
        new Guard("PED:US-CCSS-MATH:G2:U05:L02", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-2/preparation.html", "b007f773be8b6f7c7e1a91c85fbcf4c472c3f418dee2af93ac86de98451b6fba", "CCSS:2.NBT.A.1|CCSS:2.NBT.A.2", "Key ideas:\n- Read, write, and represent multiples of 100\n- In a previous lesson, you learned that a hundred is composed of 10 tens or 100 ones\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U05:L03", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-3/preparation.html", "0a79d7bc3df54dde4cf80c1994ab8668ded02660cdd94edd67562da0eb456b8e", "CCSS:2.NBT.A.1|CCSS:2.NBT.A.2|CCSS:2.NBT.B.5", "Key ideas:\n- Compose three-digit numbers using place value understanding\n- In previous units, you used base-ten blocks and diagrams to compose and decompose tens when adding and subtracting by place\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U05:L05", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-5/preparation.html", "0e7aa16652f3cd69fd74424460a333ed95dbfc52a44f9a9a207a73fc53ca3e81", "CCSS:2.NBT.A.1|CCSS:2.NBT.A.3", "Key ideas:\n- Read, write, and represent three-digit numbers using base-ten numerals and expanded form\n- In previous lessons, you represented three-digit numbers by recording how many of each unit (for example, 357 as 3 hundreds, 5 tens, 7 ones)\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U07:L01", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-1/preparation.html", "2da4188392d740b0c0d36219b5b548bd82ee75d3ec57a87ee2ed6f1f6ed1f04d", "CCSS:2.NBT.A.2|CCSS:2.NBT.B.7|CCSS:2.NBT.B.8", "Key ideas:\n- Add and subtract within 1,000 using number relationships\n- In a previous unit, you compared three-digit numbers using a number line and considered how close you were to one another and your distance from zero\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.", "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U07:L02", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-2/preparation.html", "b82d311093f66de2c059a2a34451df799df4af84983e4316795dea3702fe30d0", "CCSS:2.NBT.B.5|CCSS:2.NBT.B.7", "Key ideas:\n- Add and subtract multiples of 10 or 100 to/from a three-digit number\n- In grade 1, you added and subtracted multiples of 10 within 100\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.", "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U07:L07", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-7/preparation.html", "86d860dff91f48b270f0952ce2ab3d7429c5914db6b46878cd0eafeaf736fdaa", "CCSS:2.NBT.B.5|CCSS:2.NBT.B.7|CCSS:2.NBT.B.8", "Key ideas:\n- Add numbers within 1,000 using place value strategies that include composing a ten or hundred\n- In previous lessons, you used your understanding of place value to add and subtract within 100 by composing and decomposing tens\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.", "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.", "COMPOSE_HUNDRED"),
        new Guard("PED:US-CCSS-MATH:G2:U07:L08", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-8/preparation.html", "39685c550ac5d1511fd4fcb56c608f9145165270dd043cb4ebdb4363c54cc59b", "CCSS:2.NBT.B.7|CCSS:2.NBT.B.9", "Key ideas:\n- Add a two-digit number to a three-digit number using place value strategies that include composing units\n- In previous lessons, you related composing a ten and a hundred when adding within 1,000\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.", "Common mistake: counting an item twice or combining the wrong quantities. Check the addends and verify the total.", ""),
        new Guard("PED:US-CCSS-MATH:G2:U07:L14", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-14/preparation.html", "39d38e4d26d5fe0529d824668936c0eabc8cb1985438774d99623507648c79c9", "CCSS:2.NBT.A.1|CCSS:2.NBT.B.7|CCSS:2.NBT.B.9", "Key ideas:\n- Subtract a two-digit number from a three-digit number using place value strategies that include decomposing 2 units\n- In a previous lesson, you subtracted from a three-digit number and decomposed a ten or a hundred to subtract by place\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.", "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.", "DECOMPOSE_SUBTRACTION"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L07", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-7/preparation.html", "2e4275d29eaa9fd2312771c81e52435a7810bcd34bed33d6b3725962ae7ec4be", "CCSS:2.OA.B.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4", "Key ideas:\n- Describe an array as an arrangement of objects into rows with an equal number of objects in each row\n- In the first section of this unit, you determined whether a group of objects was odd or even by organizing them in pairs or putting them into 2 equal groups\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", ""),
        new Guard("PED:US-CCSS-MATH:G3:U01:L15", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-15/preparation.html", "aac738c55f6a91abc87d7a65ca0c7daef877875a719e3ca398e2725691b8fd7f", "CCSS:3.OA.A.3|CCSS:3.OA.A.4|CCSS:3.OA.D.9", "Key ideas:\n- Solve multiplication problems\n- You write equations with a symbol for the unknown to represent multiplication problems and then solve the problems\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.", "Common mistake: counting an item twice or combining the wrong quantities. Check the addends and verify the total.", ""),
        new Guard("PED:US-CCSS-MATH:G3:U04:L01", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-1/preparation.html", "232f25d1843cbc426ee7fa87a4ed0de1c65fcbd22d35f5a4e3fcae7731401ed3", "CCSS:3.OA.A.2|CCSS:3.OA.A.3", "Key ideas:\n- Solve “how many groups?” problems in a way that makes sense to them\n- In a previous unit, you were introduced to multiplication\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G3:U05:L15", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-15/preparation.html", "f8de6e9aa0e5714bdb49d17c9b975412d759c60cd417435e96498c96dc325609", "CCSS:3.NF.A.3", "Key ideas:\n- Compare two fractions with the same denominator by reasoning about your size\n- In previous lessons, you learned that fractions are built from unit fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", ""),
        new Guard("PED:US-CCSS-MATH:G3:U05:L16", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-16/preparation.html", "bada06143397cce740c85f140b257439d6af8f316a99c5207a671958f4042cf0", "CCSS:3.NF.A.3", "Key ideas:\n- Compare two fractions with the same numerator by reasoning about your size\n- In this lesson, you reason that fractions with the same numerator have the same number of parts, and that the denominator shows the size or length of those parts\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U02:L03", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-3/preparation.html", "e6adaba46979c40b780994cdcf7abd16d0d54f5b12e2f8b585fbe32897685985", "CCSS:4.NBT.B.4|CCSS:4.NF.A.2", "Key ideas:\n- Compare fractions with the same numerator or the same denominator using physical or visual representations\n- Use the meaning of numerator and denominator to reason about the size of fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U02:L11", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-11/preparation.html", "c7813e90b601d53d321cfcc27378f75c7b9819b772ee91eaa357b38f476c03ef", "CCSS:4.NF.A.1", "Key ideas:\n- Generate equivalent fractions by using factors of the numerator and denominator\n- Reason about fraction equivalence numerically, by using multiples or factors of the numerator and denominator\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U02:L16", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-16/preparation.html", "ed47c1e7a729ade583b01059b66d20ec7def82cd930629320485d024e22f2221", "CCSS:4.NF.A.2", "Key ideas:\n- Compare and order fractions using any strategy\n- Throughout the unit, you have encountered a wide range of fractions and learned a variety of ways to represent and compare fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U03:L03", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-3/preparation.html", "814c8ccbe20f8b3b778e0a712ebff7140b8c613ff85672272d584b7d2e6c9f7e", "CCSS:4.NF.B.4", "Key ideas:\n- Evaluate multiplication expressions and recognize that $n \\times \\frac{1}{b} = \\frac{n}{b}$\n- In this lesson, you analyze two sets of multiplication expressions: one in which the number of groups is kept constant, and another in which the size of each group (a unit fraction) is kept constant\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.", "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.", "FRACTION_UNIT_PRODUCT"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L05", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-5/preparation.html", "6794f5fa78219b1d82ca9cf0f864fae715ba610b17ead63e31f0eafd832d05d2", "CCSS:4.NF.B.4", "Key ideas:\n- Write equivalent expressions for the multiplication of a fraction by a whole number and explain or show that the expressions are equivalent\n- In previous lessons, you multiplied unit and non-unit fractions by a whole number and represented your reasoning with diagrams and expressions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.", "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.", "FRACTION_EQUIVALENT_PRODUCT"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L07", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-7/preparation.html", "f8c7926744970af311cff3665bf33200dde6085f49e9e4e18671e9c65ab5ce27", "CCSS:4.NF.B.3", "Key ideas:\n- Recognize that a fraction can be decomposed into a sum of fractions with the same denominator\n- Write equations to represent fraction decomposition\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.", "Common mistake: counting an item twice or combining the wrong quantities. Check the addends and verify the total.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U03:L10", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-10/preparation.html", "fb49c05cb7bcda80756290eb9db3079ce3401b7ffe5b7f62e95b1cb192fb9008", "CCSS:4.NF.B.3|CCSS:4.NF.B.4", "Key ideas:\n- Subtract a fraction from a whole number by decomposing the whole number and reasoning about equivalence\n- In earlier lessons, you explored addition and subtraction of fractions in context and out of context\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.", "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U03:L15", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-15/preparation.html", "91b0725bf14db783b5847264d77b1bbc7d815df33d57569d7c6ea6b9f024f58b", "CCSS:4.NF.C.5", "Key ideas:\n- Use equivalence to reason about addition and subtraction problems\n- In a previous unit, you learned to recognize and generate equivalent fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U03:L19", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-19/preparation.html", "0e90ff4c7a65dd8aa88af5cbf3e11d2db49b7c4e31bd54663c9720ccc5087664", "CCSS:4.NF.B.3|CCSS:4.NF.B.4|CCSS:4.NF.C.5", "Key ideas:\n- Interpret and solve problems that involve the addition, subtraction, and multiplication of fractions\n- This optional lesson gives you additional opportunities to integrate and apply the work from this unit to solve novel contextual problems\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.", "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.", ""),
        new Guard("PED:US-CCSS-MATH:G4:U04:L05", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-5/preparation.html", "461fa55eae1385e7d6498dcd5b7d6825225df4e88c095347c4d80e70c306d42a", "CCSS:4.NF.C.5|CCSS:4.NF.C.6|CCSS:4.NF.C.7", "Key ideas:\n- Compare and order fractions and decimals to the hundredths by reasoning about your size\n- In a previous unit, you compared and ordered fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.", "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.", "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.", "")
    ];
    private static readonly IReadOnlyDictionary<string, Guard> ByCode =
        Cohort.ToDictionary(x => x.Code, StringComparer.Ordinal);
    public static int ExpectedCount => Cohort.Length;

    // Four skill-family variants; all 23 remove the same confirmed generic
    // measurement claim, while non-ruler worked mathematics is retained.
    private static (string Steps, string Mistakes) Replacements(Guard g) =>
        g.Mode switch
        {
        "COMPOSE_HUNDRED" => ("Step 1: Represent each addend using hundreds, tens, and ones.\nStep 2: Add like place-value units. When a place has 10 or more units, compose 10 ones into 1 ten or 10 tens into 1 hundred.\nStep 3: Read the total using the new hundreds, tens, and ones.\nStep 4: Check by decomposing a composed unit and confirming the sum stays unchanged.", "Mistake: adding a ten as if it were one instead of ten ones. Regroup only 10 equal place-value units into 1 unit of the next place; the total is preserved."),
        "DECOMPOSE_SUBTRACTION" => ("Step 1: Write the three-digit starting number and the two-digit amount to subtract in aligned hundreds, tens, and ones columns.\nStep 2: Subtract the ones. If there are not enough ones, decompose 1 ten as 10 ones.\nStep 3: Subtract the tens. If there are not enough tens, decompose 1 hundred as 10 tens.\nStep 4: Explain each exchange using a place-value drawing or equation, then check that difference plus subtrahend equals the starting number.", "Mistake: removing tens from the hundreds digit without exchanging 1 hundred for 10 tens. Decompose the higher unit into 10 equal units of the next place while preserving the overall amount."),
        "FRACTION_UNIT_PRODUCT" => ("Step 1: Identify the number n of equal copies and the unit fraction 1/b.\nStep 2: Draw n copies of 1/b using the same-sized whole and equal partitions.\nStep 3: Combine the n copies to obtain n/b, so n × (1/b) = n/b.\nStep 4: Check the product against a fraction model and explain what n and b mean.", "Mistake: multiplying the denominator instead of counting copies of the unit fraction. For n copies of 1/b, keep the denominator b and count n unit parts: n/b."),
        "FRACTION_EQUIVALENT_PRODUCT" => ("Step 1: Identify the whole-number factor n and the fraction a/b in the expression n × (a/b).\nStep 2: Represent n groups, each containing a unit-fraction parts of size 1/b.\nStep 3: Regroup the total unit parts to show n × (a/b) = (n × a)/b and connect equivalent expressions.\nStep 4: Confirm the resulting fraction with a diagram or a second numerical representation using the same whole.", "Mistake: multiplying the denominator by the whole-number factor when finding n groups of a/b. Each group's parts are still sized 1/b, so multiply the number of parts a by n, not b."),
            _ => (g.OldSteps, g.OldMistakes)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l) =>
        d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(l.LessonCode);

    private static void VerifySource(CanonicalLessonContentPackLesson l, Guard g)
    {
        if (l.SourceUrl != g.SourceUrl ||
            !string.Equals(l.CanonicalBodySha256, g.RawSha,
                StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", l.OutcomeCodes) != g.Outcomes ||
            l.Translations.Count != 1 ||
            l.Translations[0].CultureCode != "en")
            throw new InvalidOperationException("V9 shared non-measure source/CCSS/body drift: " + l.LessonCode);
        if (!g.OldConcepts.EndsWith(IrrelevantRule, StringComparison.Ordinal))
            throw new InvalidOperationException("V9 shared root template baseline drift: " + l.LessonCode);
    }

    private static string NewConcepts(Guard g) =>
        g.OldConcepts[..^IrrelevantRule.Length];

    private static bool Same(string concepts, string steps, string mistakes, Guard g, bool after)
    {
        var (newSteps,newMistakes) = Replacements(g);
        return concepts == (after ? NewConcepts(g) : g.OldConcepts) &&
            steps == (after ? newSteps : g.OldSteps) &&
            mistakes == (after ? newMistakes : g.OldMistakes);
    }

    private static bool Same(CanonicalLessonContentPackTranslation t, Guard g, bool after) =>
        Same(t.KeyConceptsAndRules, t.StepByStepSolutions, t.CommonMistakes, g, after);

    private static bool Same(CurriculumLessonContentTranslation t, Guard g, bool after) =>
        Same(t.KeyConceptsAndRules, t.StepByStepSolutions, t.CommonMistakes, g, after);

    // Exact prior-to-after concept states for the sole overlap with the new
    // V9 comparison procedure repair. No other lesson or arbitrary prose passes.
    public static bool IsVerifiedOriginalOrCorrectedConceptState(string code, string concepts) =>
        ByCode.TryGetValue(code, out var g) &&
        (concepts == g.OldConcepts || concepts == NewConcepts(g));

    private static bool SameWithLaterSharedRoot(string concepts, string steps,
        string mistakes, Guard g) =>
        concepts == NewConcepts(g) &&
        (CommonCoreB01V9SharedComparisonProcedureCorrections.MatchesFinalProcedure(
            g.Code, steps, mistakes) ||
         CommonCoreB01V9SharedAdditionProcedureCorrections.MatchesFinalProcedure(
            g.Code, steps, mistakes));

    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        if (d.PackCode != "US-CCSS-MATH" || d.VersionCode != "CCSSM-2010") return;
        foreach (var l in d.Lessons)
        {
            if (!ByCode.TryGetValue(l.LessonCode, out var g)) continue;
            VerifySource(l,g);
            var t=l.Translations[0];
            if (Same(t,g,true) || SameWithLaterSharedRoot(
                t.KeyConceptsAndRules, t.StepByStepSolutions, t.CommonMistakes, g))
                continue;
            if (!Same(t,g,false))
                throw new InvalidOperationException("V9 shared root rejected partial or unexpected learner changes: " + l.LessonCode);
            var (steps,mistakes) = Replacements(g);
            t.KeyConceptsAndRules = NewConcepts(g);
            t.StepByStepSolutions = steps;
            t.CommonMistakes = mistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l, string prior) =>
        IsTarget(d,l) ? CorrectedVersion : prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l, string oldVersion) =>
        IsTarget(d,l) && (
            oldVersion == CorrectedVersion ||
            oldVersion == CommonCoreRichContentCorrections.CorrectionContentVersion ||
            (l.LessonCode == "PED:US-CCSS-MATH:G4:U02:L03" &&
             oldVersion == CommonCoreSourceBoundHtmlEntityCorrections.CorrectedContentVersion));

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,
        string persistedVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(d,l,persistedVersion))
            throw new InvalidOperationException("V9 shared non-measure root predecessor version rejected.");
        var g = ByCode[l.LessonCode];
        VerifySource(l,g);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 shared non-measure persisted culture drift.");
        var t=persisted.Single();
        if (!Same(t,g,false) && !Same(t,g,true) &&
            !SameWithLaterSharedRoot(t.KeyConceptsAndRules,
                t.StepByStepSolutions, t.CommonMistakes, g))
            throw new InvalidOperationException("V9 shared non-measure persisted learner body drift: " + l.LessonCode);
    }
}

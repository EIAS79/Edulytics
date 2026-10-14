using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 one-cohort source-verified repair for 25 repeated ADDITION procedure templates:
/// 21 wrong/insufficient mathematical-purpose procedures, 3 appropriate controls and 1 earlier-corrected lesson.
/// 25 original official teacher goals and full activity families checked; no
/// original publisher prose or assets copied. NOT an academic L1-L6 certificate.
/// </summary>
public static class CommonCoreB01V9SharedAdditionProcedureCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-shared-addition-procedure-21-v1";
    private const string OldSteps = "Step 1: Identify the quantities being combined.\nStep 2: Represent each quantity with objects, a drawing, number line, or expression.\nStep 3: Write an addition relationship that matches the representation.\nStep 4: Find the total using an efficient strategy.\nStep 5: Check that the total matches the original quantities.";
    private const string OldMistakes = "Common mistake: counting an item twice or combining the wrong quantities. Check the addends and verify the total.";
    private sealed record Guard(string Code,string SourceUrl,string RawSha,string Outcomes,string MathFamily);
    private static readonly Guard[] Cohort =
    [
        new Guard("PED:US-CCSS-MATH:G2:U06:L15","https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-15/preparation.html","5d48b72bb8e11dcaa1c4ef923fe6d489e8763eb542b7054acae6e6e98a91ff41","CCSS:2.MD.C.8|CCSS:2.NBT.A.2|CCSS:2.NBT.B.5|CCSS:2.NBT.B.6|CCSS:2.NBT.B.8","COINS_PENNY_NICKEL_DIME"),
        new Guard("PED:US-CCSS-MATH:G2:U06:L16","https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-16/preparation.html","e932f98fe77b9468b727f303ca4daf5e704be37d30bf43f7b4bf2d115edecb39","CCSS:2.MD.C.8|CCSS:2.NBT.A.2|CCSS:2.NBT.B.5","COINS_QUARTER_COMBINATIONS"),
        new Guard("PED:US-CCSS-MATH:G2:U06:L21","https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-21/preparation.html","ed6a0440699a25453e04bd2b6da709ff3e514d8222511b3db0ff1b31a214b2c8","CCSS:2.G.A.1|CCSS:2.MD.C.8|CCSS:2.NBT.B.5|CCSS:2.OA.A.1","PATTERN_BLOCK_COMPOSITION_AND_MONEY"),
        new Guard("PED:US-CCSS-MATH:G2:U07:L06","https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-6/preparation.html","a5db512e710043d2b8dd898d5fc0309e10c51379aef59bbfe5a1f267839cac2a","CCSS:2.NBT.B.5|CCSS:2.NBT.B.7","PLACE_VALUE_COMPOSE_TEN"),
        new Guard("PED:US-CCSS-MATH:G2:U07:L08","https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-8/preparation.html","39685c550ac5d1511fd4fcb56c608f9145165270dd043cb4ebdb4363c54cc59b","CCSS:2.NBT.B.7|CCSS:2.NBT.B.9","PLACE_VALUE_COMPOSE_UNITS"),
        new Guard("PED:US-CCSS-MATH:G2:U07:L09","https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-9/preparation.html","4fb3d4ecbac1648e7aa09b95fd8a7c7395e8ad06b903b0b8c08681748cc050ec","CCSS:2.NBT.B.7|CCSS:2.NBT.B.9","PLACE_VALUE_DOUBLE_COMPOSITION"),
        new Guard("PED:US-CCSS-MATH:G2:U07:L10","https://im.kendallhunt.com/k5/teachers/grade-2/unit-7/lesson-10/preparation.html","c672c392ff6458929f0a5d04442397336cd694efb2e5adda5cd8d80e87a3cdc1","CCSS:2.NBT.B.7|CCSS:2.NBT.B.9","PLACE_VALUE_PROPERTIES_OF_OPERATIONS"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L04","https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-4/preparation.html","44e41667d1493f8c8c0c7fc42233ac5bef3e504f97e817eff0fbe2f47a13cbae","CCSS:2.OA.B.2|CCSS:2.OA.C.3","EVEN_EQUAL_ADDENDS"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L09","https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-9/preparation.html","bcb72af780ce091cc45149ffbe48c2e8b3bf7f65bd96ca98538d30d5ea880d61","CCSS:2.OA.B.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4","ARRAY_REPEATED_ADDITION"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L10","https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-10/preparation.html","3e31c5d65eba1e6839a052f2d5e823e6e0d0229ae979fbcd8b7aec49c78615ed","CCSS:2.NBT.A.2|CCSS:2.OA.B.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4","ARRAY_ROWS_COLUMNS_EQUATIONS"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L15","https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-15/preparation.html","aac738c55f6a91abc87d7a65ca0c7daef877875a719e3ca398e2725691b8fd7f","CCSS:3.OA.A.3|CCSS:3.OA.A.4|CCSS:3.OA.D.9","MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U03:L04","https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-4/preparation.html","bb79cad9214030608ea3dd58b7845479fb14f532145cb28bf4fad106ca80574e","CCSS:3.NBT.A.2","BASETEN_DIAGRAM_TO_ALGORITHM"),
        new Guard("PED:US-CCSS-MATH:G3:U03:L05","https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-5/preparation.html","4b4412f78f5bcd35cee414156b1473ccee5cf3f49b194e9279e7b9459b0caffa","CCSS:3.NBT.A.2|CCSS:3.OA.D.9","COMPARE_ADDITION_ALGORITHMS"),
        new Guard("PED:US-CCSS-MATH:G3:U03:L20","https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-20/preparation.html","5a7867f5109b55af25504221988b434dcaae33dd63da249d0d88e584bfdb7cbe","CCSS:3.OA.C.7|CCSS:3.OA.D.8","TWO_STEP_WORD_PROBLEM"),
        new Guard("PED:US-CCSS-MATH:G3:U03:L21","https://im.kendallhunt.com/k5/teachers/grade-3/unit-3/lesson-21/preparation.html","c5844c16777e6e34dadac5b10cbfaa1b9ef27896bac4543b57a7a126c4e21ad4","CCSS:3.NBT.A.1|CCSS:3.NBT.A.2","REAL_WORLD_ROUNDING_BUDGET"),
        new Guard("PED:US-CCSS-MATH:G4:U01:L05","https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-5/preparation.html","d0bee0e28c6f3d3a1958f3cdbb6f187e3949b7d25ff76cf894f95b491f088ec9","CCSS:4.OA.A.3|CCSS:4.OA.B.4","MULTIPLICATION_AND_MULTIPLES_CONTEXT"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L07","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-7/preparation.html","f8c7926744970af311cff3665bf33200dde6085f49e9e4e18671e9c65ab5ce27","CCSS:4.NF.B.3","FRACTION_SAME_DENOM_DECOMPOSITION"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L08","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-8/preparation.html","24cd2a891cdadbefada6db4f5b95dfbbc665cf24aa3a7742094429bba5f7c77c","CCSS:4.NF.B.3","FRACTION_NUMBERLINE_DECOMPOSITION"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L16","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-16/preparation.html","bd1b8765c9388f9f72ecd8ad147ded865ca3ca4294d9a2d7467d183a233263b0","CCSS:4.NF.A.1|CCSS:4.NF.C.5","FRACTION_TENTHS_HUNDREDTHS_TO_ONE"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L17","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-17/preparation.html","640a762617a88788dcbe1d71780829aea7225a80ae4d68bc007e5eaad3a58291","CCSS:4.NF.C.5","FRACTION_TENTHS_HUNDREDTHS_OVER_ONE"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L18","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-18/preparation.html","c605e2391d478dfe9cb890f3da034c86f5b1f34b26095861195ff80365a52a7f","CCSS:4.NF.B.4|CCSS:4.NF.C.5","FRACTION_ASSOCIATIVE_TENTHS_HUNDREDTHS"),
    ];
    private static readonly IReadOnlyDictionary<string,Guard> ByCode =
        Cohort.ToDictionary(x=>x.Code,StringComparer.Ordinal);

    public static int ExpectedCount => Cohort.Length;
    public static int MathFamilyCount => Cohort.Select(x=>x.MathFamily).Distinct().Count();

    // A prior source-verified root can remain strictly idempotent even after
    // this separately source-bound addition upgrade. Never accept arbitrary text.
    public static bool MatchesFinalProcedure(string code,string steps,string mistakes) =>
        ByCode.TryGetValue(code,out var guard) &&
        (steps,mistakes)==ForFamily(guard.MathFamily);

    private static (string Steps,string Mistakes) ForFamily(string family) =>
        family switch
        {
        "COINS_PENNY_NICKEL_DIME" => ("Step 1: Name penny, nickel, and dime denominations in the given set.\nStep 2: Count the coins at 1, 5, and 10 cents respectively; group equal denominations and count on or skip count.\nStep 3: Record the total value in cents and check against the actual coin counts.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Counting each coin as one cent or confusing a coin count with its monetary value."),
        "COINS_QUARTER_COMBINATIONS" => ("Step 1: Identify quarters as 25 cents alongside pennies, nickels and dimes.\nStep 2: Compute each group of coins by denomination and find alternative coin combinations with the same value.\nStep 3: Verify both representations add to the stated number of cents.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Ignoring the 25-cent value of a quarter or assuming different coin counts mean different total values."),
        "PATTERN_BLOCK_COMPOSITION_AND_MONEY" => ("Step 1: Identify the smaller pattern blocks and the figure the task asks you to construct.\nStep 2: Fit blocks together without unintended overlaps; where the task gives prices, add prices for the chosen blocks.\nStep 3: Check both the composed shape and the total monetary amount against the given constraints.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Treating the block arrangement as only a numerical addition and overlooking the geometric construction."),
        "PLACE_VALUE_COMPOSE_TEN" => ("Step 1: Write each addend as hundreds, tens and ones.\nStep 2: Add units in the same place; when the ones make ten, exchange ten ones for one ten.\nStep 3: Combine the place-value totals and verify the sum within 1,000.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Adding place values without composing a new ten when needed."),
        "PLACE_VALUE_COMPOSE_UNITS" => ("Step 1: Decompose the two-digit and three-digit addends into hundreds, tens and ones.\nStep 2: Combine like units, exchanging ten ones for a ten or ten tens for a hundred when required.\nStep 3: Read the resulting hundreds, tens and ones and check the sum.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Failing to exchange a complete group of ten for one unit in the next place."),
        "PLACE_VALUE_DOUBLE_COMPOSITION" => ("Step 1: Align the two three-digit addends by place value.\nStep 2: Add ones and tens, regrouping at both places when the sum makes new tens and hundreds.\nStep 3: Add the composed hundreds and verify with a second place-value strategy.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Regrouping only the ones and forgetting to compose a hundred from ten tens."),
        "PLACE_VALUE_PROPERTIES_OF_OPERATIONS" => ("Step 1: Identify hundreds, tens and ones and look for numbers that can be combined efficiently.\nStep 2: Use decomposing, compensating, or reordering of addends while preserving equality.\nStep 3: Explain the chosen place-value strategy and verify the total by a second method.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Changing one addend during compensation without balancing the other."),
        "EVEN_EQUAL_ADDENDS" => ("Step 1: Check that the starting number is even.\nStep 2: Split it into two groups with exactly the same number of objects and write n = a + a.\nStep 3: Recombine the two equal addends to verify the original total.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Using two unequal addends when the target is to show an even number as a double."),
        "ARRAY_REPEATED_ADDITION" => ("Step 1: Identify the rectangular array's number of rows and objects per row.\nStep 2: Write equal addends for the row totals; do the same by columns if the task asks.\nStep 3: Count every array object once and check the repeated-addition expression.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Adding row and column counts instead of adding the equal groups represented by the array."),
        "ARRAY_ROWS_COLUMNS_EQUATIONS" => ("Step 1: Count the rows and the number of objects in each row of the given array.\nStep 2: Write a repeated-addition equation for the rows and another for the columns.\nStep 3: Check both equations give the same total number of array objects.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Writing an equation with a number of addends different from the rows or columns shown."),
        "MULTIPLICATION_EQUAL_GROUPS" => ("Step 1: Identify how many equal groups there are and the number in each group.\nStep 2: Represent the situation as a multiplication equation with two factors.\nStep 3: Compute the product and check it matches the total in the groups.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Using an addition-only rule for a task that asks for a multiplication relationship."),
        "BASETEN_DIAGRAM_TO_ALGORITHM" => ("Step 1: Represent both addends with hundreds, tens and ones, using base-ten drawings if provided.\nStep 2: Write the same place values in a vertical or expanded algorithm, composing new units when needed.\nStep 3: Show how the algorithm matches the diagram and verify the final sum.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Treating the written carrying digit as unrelated to a group of ten base-ten units."),
        "COMPARE_ADDITION_ALGORITHMS" => ("Step 1: Identify the same addends in the two displayed written methods.\nStep 2: Explain how each method composes hundreds, tens and ones, even when their written notation differs.\nStep 3: Calculate the sum using both algorithms and compare their place-value reasoning.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Treating different notation or a differently placed regrouping mark as a different mathematical total."),
        "TWO_STEP_WORD_PROBLEM" => ("Step 1: Identify what the story asks for and which intermediate quantity must be found first.\nStep 2: Write and solve the first equation, then use its result in the second operation.\nStep 3: Check the final result against both story conditions and label the requested quantity.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Stopping after the first operation or combining all numbers without their story relationships."),
        "REAL_WORLD_ROUNDING_BUDGET" => ("Step 1: Read each item quantity or price and decide what the problem asks you to estimate or calculate.\nStep 2: Round whole numbers to the required ten or hundred; add or subtract according to the stated budget or comparison.\nStep 3: Distinguish the estimate from an exact total and check whether the result answers the real-world question.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Using a rounded estimate as an exact price or always adding when the story asks for a difference."),
        "MULTIPLICATION_AND_MULTIPLES_CONTEXT" => ("Step 1: Identify the size of each equal group and the total required in the real-world situation.\nStep 2: Use multiplication and related multiples to compare available groupings or supplies.\nStep 3: Check that the proposed grouping satisfies all quantities and leftover constraints.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Adding unrelated numbers instead of reasoning with factors, multiples and equal groups."),
        "FRACTION_SAME_DENOM_DECOMPOSITION" => ("Step 1: Identify the original fraction and its denominator as the unit-part size.\nStep 2: Decompose the numerator into addends while keeping the same denominator for every part.\nStep 3: Write the fraction-addition equation and verify the numerators sum to the original numerator.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Adding denominators when breaking one fraction into a sum of equal-sized fractional parts."),
        "FRACTION_NUMBERLINE_DECOMPOSITION" => ("Step 1: Locate the improper fraction on a number line divided into the given denominator-sized steps.\nStep 2: Show addend jumps with the same-sized fractional units and, when possible, split into a whole plus a proper fraction.\nStep 3: Check the total jump lands on the target fraction and write the matching equation.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Changing the unit fraction size between jumps or assuming a fraction greater than one cannot be decomposed."),
        "FRACTION_TENTHS_HUNDREDTHS_TO_ONE" => ("Step 1: Convert tenths to equivalent hundredths before combining different denominator terms.\nStep 2: Add numerators measured in hundredths and express the result as a fraction no greater than one.\nStep 3: Check the equivalent fraction and the given model agree with the resulting total.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Adding numerator and denominator separately instead of using equivalent hundredths."),
        "FRACTION_TENTHS_HUNDREDTHS_OVER_ONE" => ("Step 1: Rewrite tenths as equivalent hundredths so both addends share one denominator.\nStep 2: Find the sum and explain when it passes one whole, including any missing addend.\nStep 3: Check the result against a whole plus a fractional remainder or the original hundredths.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Treating any numerator over one hundred as invalid or dropping the whole when the sum exceeds one."),
        "FRACTION_ASSOCIATIVE_TENTHS_HUNDREDTHS" => ("Step 1: Rewrite all tenths and hundredths using common hundredths units.\nStep 2: Reorder or regroup three or more addends to make helpful whole- or part-hundred sums.\nStep 3: Verify the final total is unchanged by the commutative or associative regrouping.\nStep 4: Verify that the result answers the given task and preserves its mathematical quantities.","Mistake: Combining convenient numerators while changing their fractional unit size."),
            _ => throw new InvalidOperationException("Unreviewed V9 addition purpose: "+family)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode=="US-CCSS-MATH"&&doc.VersionCode=="CCSSM-2010"&&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Guard Verify(CanonicalLessonContentPackLesson lesson)
    {
        var guard=ByCode[lesson.LessonCode];
        if(lesson.SourceUrl!=guard.SourceUrl||
            !string.Equals(lesson.CanonicalBodySha256,guard.RawSha,StringComparison.OrdinalIgnoreCase)||
            string.Join("|",lesson.OutcomeCodes)!=guard.Outcomes||
            lesson.Translations.Count!=1||lesson.Translations[0].CultureCode!="en")
            throw new InvalidOperationException("V9 addition source/standard/body SHA drift: "+lesson.LessonCode);
        return guard;
    }

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH"||doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            if(!ByCode.ContainsKey(lesson.LessonCode))continue;
            var guard=Verify(lesson);
            var translation=lesson.Translations[0];
            var (newSteps,newMistakes)=ForFamily(guard.MathFamily);
            if(translation.StepByStepSolutions==newSteps&&translation.CommonMistakes==newMistakes)continue;
            if(translation.StepByStepSolutions!=OldSteps||translation.CommonMistakes!=OldMistakes)
                throw new InvalidOperationException("V9 addition unexpected partial procedure drift: "+lesson.LessonCode);
            translation.StepByStepSolutions=newSteps;
            translation.CommonMistakes=newMistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string prior) =>
        IsTarget(doc,lesson)?CorrectedVersion:prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson)&&
        (version==CorrectedVersion||
         version==CommonCoreRichContentCorrections.CorrectionContentVersion||
         version==CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CorrectedVersion||
         version==CommonCoreB01NonLengthRulerTemplateCorrections.CorrectedVersion||
         version==CommonCoreB01FractionPurposeCorrections.CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException("V9 addition predecessor refused: "+lesson.LessonCode);
        var guard=Verify(lesson);
        if(persisted.Count==0)return;
        if(persisted.Count!=1||persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("V9 addition persisted culture drift: "+lesson.LessonCode);
        var body=persisted.Single();
        var current=lesson.Translations.Single();
        var (newSteps,newMistakes)=ForFamily(guard.MathFamily);
        if((body.StepByStepSolutions!=OldSteps||body.CommonMistakes!=OldMistakes)&&
           (body.StepByStepSolutions!=newSteps||body.CommonMistakes!=newMistakes))
            throw new InvalidOperationException("V9 addition stale/new persisted procedure drift: "+lesson.LessonCode);
        if(body.Title!=current.Title||body.Explanation!=current.Explanation||
           body.KeyConceptsAndRules!=current.KeyConceptsAndRules||
           body.WorkedExamples!=current.WorkedExamples||
           body.QuickSummary!=current.QuickSummary)
            throw new InvalidOperationException("V9 addition unrelated persisted content drift: "+lesson.LessonCode);
    }
}

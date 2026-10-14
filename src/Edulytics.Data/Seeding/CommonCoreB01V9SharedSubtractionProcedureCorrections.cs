using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-verified V9 shared subtraction-root fix: all 55 candidate teacher
/// purposes reviewed, 16 specific task-family mismatches repaired together.
/// Preserve other 39 (not proven defective) without ungrounded blanket edits.
/// Not L1-L6 certification or a publisher-license grant.
/// </summary>
public static class CommonCoreB01V9SharedSubtractionProcedureCorrections
{
    public const string CorrectedVersion="usccss-b01-v9-subtraction-shared-procedure-16-v1";
    private const string OldSteps="Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.";
    private const string OldMistakes="Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.";
    private sealed record Guard(string Code,string SourceUrl,string RawSha,string Outcomes,string Family);
    private static readonly Guard[] Cohort=
    [
        new Guard("PED:US-CCSS-MATH:G2:U01:L14","https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-14/preparation.html","f7bbe33e9ffd30ba24d06284d7c5deb2453591f2aa073d2545bb08e7004f5344","CCSS:2.MD.D.10","COMPARE_TAPE_GRAPH"),
        new Guard("PED:US-CCSS-MATH:G2:U03:L18","https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-18/preparation.html","19028c1fad43d97892d7bc067214e8abcf7110e68d0130f550975d96235c5588","CCSS:2.MD.A.1|CCSS:2.MD.A.4|CCSS:2.MD.B.5|CCSS:2.MD.B.6|CCSS:2.MD.D.9|CCSS:2.OA.B.2","YARDSTICK_MARKS"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L06","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-6/preparation.html","d56627a509a6e2fe8575ec3b180b282b773ad986ac49e549e2220dfe2746a6ed","CCSS:2.MD.B.6|CCSS:2.NBT.A.2|CCSS:2.NBT.B.5","PLACE_NUMBERS_ON_LINE"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L07","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-7/preparation.html","a9cac619384b144ba8ff64fbb9ae61f0d306568b1afa47bc1a8dd9799ced3d0e","CCSS:2.MD.B.6","NUMBER_LINE_BOTH_DIRECTIONS"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L09","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-9/preparation.html","54b2b10fb7e95ee9f5d5ccc701e46aae02051ff394f47a8bb70c29f4f56d0721","CCSS:2.MD.B.5|CCSS:2.MD.B.6|CCSS:2.NBT.B.5","NUMBER_LINE_COUNT_ON_BACK"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L10","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-10/preparation.html","4d89c06b99b5ee5bf8c77567b4de987c36ca9601e1f16eca2b65864e6c2f1233","CCSS:2.MD.B.6|CCSS:2.NBT.B.5","NUMBER_LINE_BASETEN_NO_REGROUP"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L11","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-11/preparation.html","1ebf1543e4f042b5b77782c37c5379b33cd1a009041b4995a03e1f1015bad34c","CCSS:2.MD.B.6|CCSS:2.NBT.B.5","NUMBER_LINE_WITH_REGROUP"),
        new Guard("PED:US-CCSS-MATH:G2:U04:L14","https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-14/preparation.html","baa4248104aaba69cf51caf7d8bf676cff352c19b3908c06747b923c58231772","CCSS:2.MD.B.6|CCSS:2.NBT.B.5","NUMBER_LINE_ADDSUB_PRACTICE"),
        new Guard("PED:US-CCSS-MATH:G2:U05:L07","https://im.kendallhunt.com/k5/teachers/grade-2/unit-5/lesson-7/preparation.html","fc5517a9c6a940a221cfae873c6739ac981bb60d8f1157c14068adeea61cf2ee","CCSS:2.MD.B.6|CCSS:2.NBT.A.1|CCSS:2.NBT.B.5|CCSS:2.OA.B.2","PLACE_VALUE_MYSTERY_NUMBER"),
        new Guard("PED:US-CCSS-MATH:G2:U06:L17","https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-17/preparation.html","0dba45c55c637e5546d77fba549fd98f35bae28d79b61243d38ac62abf3dc96c","CCSS:2.MD.C.8|CCSS:2.NBT.B.5|CCSS:2.NBT.B.6","DOLLAR_COIN_COMBINATIONS"),
        new Guard("PED:US-CCSS-MATH:G2:U06:L18","https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-18/preparation.html","5ae950269317f0614c6f970242c2014bdb34a8d8670aff26af4ff2ad6b9c0331","CCSS:2.MD.C.8|CCSS:2.NBT.B.5|CCSS:2.OA.A.1","SHOP_MONEY_CHANGE"),
        new Guard("PED:US-CCSS-MATH:G3:U08:L15","https://im.kendallhunt.com/k5/teachers/grade-3/unit-8/lesson-15/preparation.html","0889ecd2c3c613ceccbf6cbbcbb3728dfdef1f004bd425ae2cd79c6b7ff64b50","CCSS:3.NBT.A.2","DESIGN_NUMBER_TALK"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L09","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-9/preparation.html","fcdeb78ba3ec245bd2b0c97134bb80c4fb4307bd79999a776e928bad4b9c1851","CCSS:4.NF.B.3","SUBTRACT_FRACTION_NUMBER_LINE"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L11","https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-11/preparation.html","9fa230d63937bc9ad9a19be825d0068232505e63512e7cb6ca25c173ba36f963","CCSS:4.NF.B.3","SUBTRACT_MIXED_FRACTION"),
        new Guard("PED:US-CCSS-MATH:G4:U04:L18","https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-18/preparation.html","48fecf51470af851903a1b51647b77efb79faa7fcec758e0203971553f5cab77","CCSS:4.NBT.B.4","MULTIDIGIT_STANDARD_NO_DECOMPOSE"),
        new Guard("PED:US-CCSS-MATH:G4:U04:L20","https://im.kendallhunt.com/k5/teachers/grade-4/unit-4/lesson-20/preparation.html","26096e487bc9df48865e2a3485c3001d75ab7d8a4a147524b5d7a527fef9ddac","CCSS:4.NBT.B.4","MULTIDIGIT_MULTIPLE_REGROUP"),
    ];
    private static readonly IReadOnlyDictionary<string,Guard> ByCode=
        Cohort.ToDictionary(x=>x.Code,StringComparer.Ordinal);
    public static int ExpectedCount=>Cohort.Length;
    public static int StrategyCount=>Cohort.Select(x=>x.Family).Distinct().Count();
    private static (string Steps,string Mistakes) ForFamily(string f) =>
        f switch
        {
        "COMPARE_TAPE_GRAPH" => ("Step 1: Identify each category's value from the bar graph or the two quantities described.\nStep 2: Draw a tape diagram with two aligned bars and mark the difference or missing part.\nStep 3: Use an addition/subtraction equation to find the unknown comparison amount.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Comparing the bars without aligning their scales, or treating an unknown difference as an unknown total."),
        "YARDSTICK_MARKS" => ("Step 1: Locate the zero and inch marks and identify how many equal inches form each requested section.\nStep 2: Use addition or subtraction to label missing inch positions consistently along the yardstick.\nStep 3: Measure each object from the same zero point and compare lengths using matching units.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Starting measurements at the edge rather than at zero, or mixing inch counts with lengths."),
        "PLACE_NUMBERS_ON_LINE" => ("Step 1: Identify the endpoints, equal tick spacing, and numerical step size on the number line.\nStep 2: Count equal jumps to place the target number, using the tick value rather than the drawing's visual length.\nStep 3: Check that the number fits the ordered scale and the interval increments.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Plotting a number based on the drawing's spacing rather than the marked numerical interval."),
        "NUMBER_LINE_BOTH_DIRECTIONS" => ("Step 1: Mark the starting number and the jump size on the number line.\nStep 2: Draw rightward jumps for addition and leftward jumps for subtraction, with one consistent scale.\nStep 3: Write and check the matching addition or subtraction equation.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Reversing the subtraction jump or treating a rightward jump as removing a quantity."),
        "NUMBER_LINE_COUNT_ON_BACK" => ("Step 1: Locate the first quantity and the second quantity or difference on the line.\nStep 2: Choose to count backward from the whole or count forward from the smaller number; mark each jump size.\nStep 3: Combine jumps to find and verify the required difference.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Counting jumps as tick marks rather than intervals or counting in the wrong direction."),
        "NUMBER_LINE_BASETEN_NO_REGROUP" => ("Step 1: Represent each number with hundreds, tens, and ones and mark a matching line position.\nStep 2: Make addition/subtraction jumps of tens and ones without exchanging place-value units.\nStep 3: Compare the jump model to the base-ten diagram and check the equation.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Jumping the wrong number of tens or changing the unit without a task that requires regrouping."),
        "NUMBER_LINE_WITH_REGROUP" => ("Step 1: Identify the operation and the hundreds, tens, and ones involved.\nStep 2: Use line jumps and place-value exchanges to compose or decompose a ten when required.\nStep 3: Explain how each jump matches a regrouped base-ten quantity and check the final equation.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Moving across a ten without representing the exchange or reversing the jump sign."),
        "NUMBER_LINE_ADDSUB_PRACTICE" => ("Step 1: Mark the start value on a consistently labeled number line.\nStep 2: Show positive jumps for addition and negative jumps for subtraction, combining intervals as necessary.\nStep 3: Check both the end point and the written equation within 100.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Counting only the marks rather than the distance between marks or using the wrong direction."),
        "PLACE_VALUE_MYSTERY_NUMBER" => ("Step 1: Identify the clues about hundreds, tens, ones and comparisons in the task.\nStep 2: Build candidate three-digit numbers and test each against every place-value clue.\nStep 3: Explain why the remaining number satisfies all constraints; check with expanded form.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Choosing a number that satisfies one clue but violates the required hundreds or tens."),
        "DOLLAR_COIN_COMBINATIONS" => ("Step 1: Convert one dollar to 100 cents before comparing any coin combinations.\nStep 2: Count quarters, dimes, nickels or pennies by their cent values and record different sets totaling 100.\nStep 3: Check the equality between each combination and the target of 100 cents.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Treating 100 coins as 100 cents regardless of their denominations."),
        "SHOP_MONEY_CHANGE" => ("Step 1: List the price or value of every coin/item and identify what the story asks.\nStep 2: Add prices or count coin values; subtract from available money only when determining change or what remains.\nStep 3: State the answer in cents or dollars and verify it matches the buying situation.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Counting coins as identical denominations or subtracting before identifying whether change is requested."),
        "DESIGN_NUMBER_TALK" => ("Step 1: Design a Number Talk around an addition or subtraction calculation within 1,000 with a strategy worth discussing.\nStep 2: Write a sequence of prompts that invites mental decompositions, compensation or place-value connections.\nStep 3: Check all arithmetic and plan questions that compare at least two valid strategies.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Designing a routine with no mathematical prompts or presenting only an answer without reasoning."),
        "SUBTRACT_FRACTION_NUMBER_LINE" => ("Step 1: Locate the first fraction on a number line divided into equal denominator-sized units.\nStep 2: Move left by the amount subtracted while keeping each fractional jump in the same unit.\nStep 3: Write an equation and verify the end point is the correct fraction difference.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Changing the denominator while subtracting or treating one jump as one whole."),
        "SUBTRACT_MIXED_FRACTION" => ("Step 1: Express both quantities using the same fractional unit and identify whole and fractional parts.\nStep 2: Decompose a whole into equivalent fractional pieces when the fractional subtraction needs regrouping.\nStep 3: Subtract like-denominator parts, recombine the whole parts and check with addition.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Subtracting denominators or trying to subtract a larger fractional part without decomposing a whole."),
        "MULTIDIGIT_STANDARD_NO_DECOMPOSE" => ("Step 1: Align the multi-digit addends or subtraction terms by place value.\nStep 2: Add digits with carrying as needed, and subtract column by column without borrowing where no decomposition is required.\nStep 3: Check place-value column results by estimation or inverse operations.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Misaligning place values or borrowing in a column that already supports direct subtraction."),
        "MULTIDIGIT_MULTIPLE_REGROUP" => ("Step 1: Write every digit in the correct place-value column.\nStep 2: For addition compose a new ten as needed; for subtraction decompose one higher unit to resolve insufficient columns, possibly more than once.\nStep 3: Complete the standard algorithm and check by inverse arithmetic and estimation.\nStep 4: Check the result against the quantities, diagrams and conditions in the original task.","Mistake: Losing a borrowed or carried unit when more than one adjacent place-value exchange is required."),
            _=>throw new InvalidOperationException("Unreviewed V9 subtraction mathematical-purpose family: "+f)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode=="US-CCSS-MATH"&&d.VersionCode=="CCSSM-2010"&&ByCode.ContainsKey(l.LessonCode);

    private static Guard Verify(CanonicalLessonContentPackLesson l)
    {
        var g=ByCode[l.LessonCode];
        if(l.SourceUrl!=g.SourceUrl||
            !string.Equals(l.CanonicalBodySha256,g.RawSha,StringComparison.OrdinalIgnoreCase)||
            string.Join("|",l.OutcomeCodes)!=g.Outcomes||
            l.Translations.Count!=1||l.Translations[0].CultureCode!="en")
            throw new InvalidOperationException("V9 subtraction source/standards/body SHA drift: "+l.LessonCode);
        return g;
    }

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH"||doc.VersionCode!="CCSSM-2010")return;
        foreach(var l in doc.Lessons)
        {
            if(!ByCode.ContainsKey(l.LessonCode))continue;
            var guard=Verify(l);var t=l.Translations[0];
            var (steps,mistakes)=ForFamily(guard.Family);
            if(t.StepByStepSolutions==steps&&t.CommonMistakes==mistakes)continue;
            if(t.StepByStepSolutions!=OldSteps||t.CommonMistakes!=OldMistakes)
                throw new InvalidOperationException("V9 subtraction unverified partial procedure drift: "+l.LessonCode);
            t.StepByStepSolutions=steps;
            t.CommonMistakes=mistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string prior)=>IsTarget(d,l)?CorrectedVersion:prior;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string version)=>
        IsTarget(d,l)&&(version==CorrectedVersion||
            version==CommonCoreRichContentCorrections.CorrectionContentVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        if(!CanUpgradeExisting(d,l,version))
            throw new InvalidOperationException("V9 subtraction predecessor rejected: "+l.LessonCode);
        var g=Verify(l);
        if(existing.Count==0)return;
        if(existing.Count!=1||existing.Single().CultureCode!="en")
            throw new InvalidOperationException("V9 subtraction persisted language drift: "+l.LessonCode);
        var t=existing.Single();var effective=l.Translations.Single();
        var (steps,mistakes)=ForFamily(g.Family);
        if((t.StepByStepSolutions!=OldSteps||t.CommonMistakes!=OldMistakes)&&
           (t.StepByStepSolutions!=steps||t.CommonMistakes!=mistakes))
            throw new InvalidOperationException("V9 subtraction persisted version procedure drift: "+l.LessonCode);
        if(t.Title!=effective.Title||t.Explanation!=effective.Explanation||
           t.KeyConceptsAndRules!=effective.KeyConceptsAndRules||
           t.WorkedExamples!=effective.WorkedExamples||
           t.QuickSummary!=effective.QuickSummary)
            throw new InvalidOperationException("V9 subtraction unrelated persisted learner body drift: "+l.LessonCode);
    }
}

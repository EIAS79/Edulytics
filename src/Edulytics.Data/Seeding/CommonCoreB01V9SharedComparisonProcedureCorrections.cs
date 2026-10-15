using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// One V9 academic cause: comparing two numbers is the wrong common procedural
/// default for 20 source-verified non-comparison mathematical objectives.
/// Cohort-level, fail-closed original source/body/outcome checks; 12
/// mathematical-family templates; 24 relevant/mixed/corrected controls untouched.
/// Original publisher worked examples and explanatory paragraphs are retained.
/// No academic L1-L6 certification is conferred by this correction.
/// </summary>
public static class CommonCoreB01V9SharedComparisonProcedureCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-shared-comparison-procedure-20-v1";
    private const string OldSteps = "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.";
    private const string OldMistakes = "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.";
    private sealed record Guard(string Code, string SourceUrl, string Sha,
        string Outcomes, string Family);

    private static readonly Guard[] Cohort =
    [
        new Guard("PED:US-CCSS-MATH:G2:U04:L04", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-4/preparation.html", "d2482530d3b7c83eb48bb27c7e6f95cd438ce7f6d8e077089a7c7fb33b6ee11c", "CCSS:2.MD.B.6|CCSS:2.NBT.B.5", "NUMBER_LINE_ORIENTATION"),
        new Guard("PED:US-CCSS-MATH:G2:U06:L06", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-6/preparation.html", "a6d43e4a5c2324e6571857d05e492091fc735c487825c4a4a9e58af49ef0e347", "CCSS:2.G.A.1", "SHAPE_COMPOSITION"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L01", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-1/preparation.html", "1ddbcdadb7bf76ed1b9b1807dab711656c98756f0fbdf2d902f9701aa40901cd", "CCSS:2.OA.C.3", "EQUAL_GROUPS_ODD_EVEN"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L07", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-7/preparation.html", "2e4275d29eaa9fd2312771c81e52435a7810bcd34bed33d6b3725962ae7ec4be", "CCSS:2.OA.B.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4", "ARRAY_ROWS_COLUMNS"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L08", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-8/preparation.html", "be9f247e9eedf7d2ebbae8cc3636da6283c8bd8d2837aefb48b60a301134a5ad", "CCSS:2.OA.B.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4", "ARRAY_ROWS_COLUMNS"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L12", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-12/preparation.html", "c5b98eb205dbd29fc855ea6c8a4f31fe813ba9701e94f8d84e5300b398494883", "CCSS:2.G.A.2|CCSS:2.OA.C.4", "RECTANGLE_GRID_SQUARE_COUNT"),
        new Guard("PED:US-CCSS-MATH:G2:U09:L09", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-9/lesson-9/preparation.html", "f98057f3c3fa8bf15d987df9251191a0d6ac5d962b08fcdf91d4139ef2581c4b", "CCSS:2.NBT.B.5|CCSS:2.NBT.B.9|CCSS:2.OA.A.1", "ADD_SUBTRACT_STORY_PROBLEM"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L09", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-9/preparation.html", "6c907b7f1945d2b123f490edb6995a800fe142ee682231097b0f2433cd7eb2d4", "CCSS:3.OA.A.1", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L10", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-10/preparation.html", "3d3266868dd51d7118c7a6b14d4c12f232655597dd6b4adc10eca3114ba42c8a", "CCSS:3.OA.A.1", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L11", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-11/preparation.html", "98e4ea44d5225f15913066f0b4bae291c2d2e2f10dbffaa52dadaf60f252e0b8", "CCSS:3.OA.A.1", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L17", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-17/preparation.html", "fd31091b323ed508c4bc69e9006ce9f72c944de32a7c52f6dbb64fdeb422fbdc", "CCSS:3.OA.A.1", "ARRAY_ROWS_COLUMNS"),
        new Guard("PED:US-CCSS-MATH:G3:U01:L21", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-1/lesson-21/preparation.html", "8f30af56d84ce0d04500f12e0bd8dae4513c3765df8d339cf1cde346637d64aa", "CCSS:3.MD.B.3|CCSS:3.OA.A.3", "MULTIPLICATION_SCALED_DATA_GRAPH"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L13", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-13/preparation.html", "a191bd40497aef3abd22954524b31bbc7044b7c8132aa8763cc51dc3acd1c9fb", "CCSS:3.OA.A.3|CCSS:3.OA.B.5", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L15", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-15/preparation.html", "89ed7539905eec8ad315639c003331ddd008a90007b81c5434db0cc47788abff", "CCSS:3.MD.C.7|CCSS:3.OA.A.3|CCSS:3.OA.B.5", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L16", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-16/preparation.html", "84c22cd203d038d65b580b91e001ba008263600b5ee7a85ffe930950d0541bf1", "CCSS:3.OA.B.5", "MULTIPLICATION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L18", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-18/preparation.html", "f2cf68dea86266218e6a2661a221e3368b8ad1f15463fcb8b0bcada6ef5db732", "CCSS:3.OA.A.2|CCSS:3.OA.A.3", "DIVISION_EQUAL_GROUPS"),
        new Guard("PED:US-CCSS-MATH:G3:U05:L01", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-1/preparation.html", "36b30473f29a14dd4eace5ca2fbb5e8caf038a7e90d9e6be54a99d14c63d8889", "CCSS:3.G.A.2|CCSS:3.NF.A.1", "EQUAL_AREA_FRACTION_PARTITION"),
        new Guard("PED:US-CCSS-MATH:G3:U08:L12", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-8/lesson-12/preparation.html", "f680878ea326845edc3482ec4c1cb211701e5c013b7bbfa23f1c18ecb3833b8f", "CCSS:3.OA.A.1", "CREATE_EQUAL_GROUPS_MATH_DISCUSSION"),
        new Guard("PED:US-CCSS-MATH:G3:U08:L13", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-8/lesson-13/preparation.html", "cea5f489575e5a2663537a79d826e6ec93f3f4e9b452eb07a3a39e5d02dd8e20", "CCSS:3.OA.A.1", "CREATE_EQUAL_GROUPS_MATH_DISCUSSION"),
        new Guard("PED:US-CCSS-MATH:G4:U03:L06", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-6/preparation.html", "e9538bb7bf2c4cfcac55d5aade6cb473ce0b667cd2017a21e157498c09ca6b54", "CCSS:4.NF.B.4", "FRACTION_MULTIPLY_BY_WHOLE")
    ];
    private static readonly IReadOnlyDictionary<string, Guard> ByCode =
        Cohort.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static int ExpectedCount => Cohort.Length;
    public static int FamilyCount => Cohort.Select(x => x.Family).Distinct().Count();

    private static (string Steps, string Mistakes) ForFamily(string family) =>
        family switch
        {
        "NUMBER_LINE_ORIENTATION" => ("Step 1: Locate zero and read the labeled values and equal intervals on the number line.\nStep 2: Locate the given numbers or compute the value of an expression before marking it.\nStep 3: Explain why moving to the right shows increasing numbers and moving to the left decreasing numbers.\nStep 4: Verify positions using tick-mark spacing and explain any comparison using the numbers, not the drawing alone.", "Mistake: assuming a point is greater because it is drawn larger rather than farther right at a correct labeled position. Read the number-line scale first."),
        "SHAPE_COMPOSITION" => ("Step 1: Identify the equal-size pieces you are allowed to use to build shapes.\nStep 2: Arrange and connect the specified number of pieces without overlaps or gaps.\nStep 3: Trace the outline and name the new composed shape using its sides and defining attributes.\nStep 4: Decompose the new shape to check the number and identity of its pieces.", "Mistake: naming a composed figure using only its size or visual appearance. Check the outer boundary and the exact component shapes."),
        "EQUAL_GROUPS_ODD_EVEN" => ("Step 1: Count the objects in the chosen collection.\nStep 2: Share the objects alternately into two groups until none are left to distribute.\nStep 3: Compare the group sizes and record whether zero or one object remains.\nStep 4: Explain whether the total is even (two equal groups, no leftovers) or odd (one leftover).", "Mistake: calling two groups equal while an object is left over, or counting an object twice. Every object must be assigned once."),
        "ARRAY_ROWS_COLUMNS" => ("Step 1: Arrange the counters in complete rows or columns with the same number in each group.\nStep 2: Count the rows and counters per row, or count the columns and counters per column.\nStep 3: Represent the total by repeated addition of equal groups and check by counting all objects.\nStep 4: Explain why any irregular row or column prevents the arrangement from being a rectangular array.", "Mistake: counting incomplete rows as full rows or switching the number of rows with total objects. Identify each equal group before counting."),
        "RECTANGLE_GRID_SQUARE_COUNT" => ("Step 1: Partition the entire rectangle into equal-size squares without gaps or overlapping pieces.\nStep 2: Count the equal rows and the number of squares in each row.\nStep 3: Use repeated addition or rows times squares per row to count all squares.\nStep 4: Recount by columns to verify that every square is included exactly once.", "Mistake: counting only the outline squares or mixing unequal-size tiles. Count every equal square covering the entire rectangle."),
        "ADD_SUBTRACT_STORY_PROBLEM" => ("Step 1: Read the story and identify known quantities, changes, and the unknown quantity.\nStep 2: Represent the situation with a drawing, a tape diagram, or a matching equation.\nStep 3: Decide whether the unknown is found by addition or subtraction, and solve.\nStep 4: Check the result against the story and explain why the equation matches the situation.", "Mistake: choosing addition or subtraction only because of a word such as 'more'. Identify the actual quantities and the unknown in the story."),
        "MULTIPLICATION_EQUAL_GROUPS" => ("Step 1: Identify the number of equal groups and the size of one group in the situation.\nStep 2: Draw equal groups, a tape diagram, or an array and match it to a multiplication expression.\nStep 3: Calculate the product using equal-group counting, a known fact, or decomposition of a larger factor.\nStep 4: Check the total and explain what each factor represents in the original situation.", "Mistake: treating the group count as the total, or combining groups of different sizes. Multiplication counts a fixed number of equal-size groups."),
        "MULTIPLICATION_SCALED_DATA_GRAPH" => ("Step 1: Identify the player counts for each game and the total seating or table constraints.\nStep 2: Design a feasible arrangement that groups players correctly, and account for all available tables.\nStep 3: Record the game/category counts and display the data using an accurately labeled, consistent graph scale.\nStep 4: Check that the graph matches the seating model and all game-group constraints.", "Mistake: drawing a scaled graph without interpreting its key, or using more seats or tables than the situation allows. Verify the arrangement first."),
        "DIVISION_EQUAL_GROUPS" => ("Step 1: Identify the total number of objects and the given size of each equal group.\nStep 2: Represent the total in groups of the specified size using a diagram or equal-group subtraction.\nStep 3: Find how many groups are formed and write a matching division equation.\nStep 4: Check by multiplying the number of groups by the size of each group.", "Mistake: reporting the size of a group when the question asks for the number of groups. In total ÷ group size, the quotient counts groups."),
        "EQUAL_AREA_FRACTION_PARTITION" => ("Step 1: Identify the whole shape and the requested number of equal-area parts.\nStep 2: Partition the whole into regions whose areas are all equal, even when their outlines look different.\nStep 3: Name one part as the unit fraction 1/n and count how many of those parts make the whole.\nStep 4: Explain why unequal-area regions cannot be named equal unit-fraction parts of the same whole.", "Mistake: naming any two regions 'halves' just because there are two. The parts of the same whole must have equal area."),
        "CREATE_EQUAL_GROUPS_MATH_DISCUSSION" => ("Step 1: Choose or create a dot picture showing equal groups and decide what classmates could notice or count.\nStep 2: Prepare an open question asking classmates what they see, wonder, or how many objects they see.\nStep 3: Present the picture and record more than one equal-group counting strategy.\nStep 4: Connect each grouping to a repeated-addition or multiplication representation and discuss why the counts agree.", "Mistake: giving the intended answer before classmates can explain groupings. Use a clear visual and ask for multiple counting strategies."),
        "FRACTION_MULTIPLY_BY_WHOLE" => ("Step 1: Identify the fraction a/b and how many equal copies or groups n are needed.\nStep 2: Model n groups of a/b using equal-size parts of the same whole.\nStep 3: Count n times a parts of size 1/b and write n × (a/b) = (n × a)/b.\nStep 4: Simplify only when useful and verify the result with a fraction diagram or repeated addition.", "Mistake: multiplying the denominator by the number of groups. Each part stays size 1/b; multiply the number of parts in the numerator."),
            _ => throw new InvalidOperationException("Unreviewed shared academic procedure family: " + family)
        };

    // Exact final state for the ONE intersection with the earlier shared
    // non-measure root. This is checked again on repeated materialization.
    /// <summary>
    /// Exact approved predecessor or source-verified replacement; used when a
    /// later concept-only correction overlays this shared procedure safely.
    /// </summary>
    public static bool MatchesOriginalOrFinalProcedure(
        string lessonCode, string steps, string mistakes) =>
        ByCode.ContainsKey(lessonCode) &&
        ((steps == OldSteps && mistakes == OldMistakes) ||
         MatchesFinalProcedure(lessonCode, steps, mistakes));

    public static bool MatchesFinalProcedure(string lessonCode, string steps, string mistakes)
    {
        if (!ByCode.TryGetValue(lessonCode, out var guard)) return false;
        var (expectedSteps, expectedMistakes) = ForFamily(guard.Family);
        return steps == expectedSteps && mistakes == expectedMistakes;
    }

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == "US-CCSS-MATH" && doc.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Guard Verify(CanonicalLessonContentPackLesson lesson)
    {
        var g = ByCode[lesson.LessonCode];
        if (lesson.SourceUrl != g.SourceUrl ||
            !string.Equals(lesson.CanonicalBodySha256, g.Sha, StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", lesson.OutcomeCodes) != g.Outcomes ||
            lesson.Translations.Count != 1 || lesson.Translations[0].CultureCode != "en")
            throw new InvalidOperationException(
                "V9 comparison shared-source/body/standards drift: " + lesson.LessonCode);
        return g;
    }

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if (doc.PackCode != "US-CCSS-MATH" || doc.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in doc.Lessons)
        {
            if (!ByCode.ContainsKey(lesson.LessonCode)) continue;
            var g = Verify(lesson);
            var trans = lesson.Translations[0];
            var (newSteps, newMistakes) = ForFamily(g.Family);
            if (trans.StepByStepSolutions == newSteps && trans.CommonMistakes == newMistakes)
                continue; // Idempotent
            if (trans.StepByStepSolutions != OldSteps || trans.CommonMistakes != OldMistakes)
                throw new InvalidOperationException(
                    "Refusing partial/unreviewed comparison procedure drift: " + lesson.LessonCode);
            trans.StepByStepSolutions = newSteps;
            trans.CommonMistakes = newMistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string previous) =>
        IsTarget(doc, lesson) ? CorrectedVersion : previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string version)
    {
        if (!IsTarget(doc, lesson)) return false;
        var g = ByCode[lesson.LessonCode];
        return version == CorrectedVersion ||
            version == CommonCoreRichContentCorrections.CorrectionContentVersion ||
            (g.Code == "PED:US-CCSS-MATH:G2:U08:L07" &&
             version == CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CorrectedVersion);
    }

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(doc, lesson, version))
            throw new InvalidOperationException("V9 shared comparison predecessor version rejected: " + lesson.LessonCode);
        var g = Verify(lesson);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 shared comparison persisted culture drift: " + lesson.LessonCode);
        var actual = persisted.Single();
        var effective = lesson.Translations.Single();
        var (newSteps, newMistakes) = ForFamily(g.Family);
        if ((actual.StepByStepSolutions != OldSteps || actual.CommonMistakes != OldMistakes) &&
            (actual.StepByStepSolutions != newSteps || actual.CommonMistakes != newMistakes))
            throw new InvalidOperationException("V9 shared comparison stored steps/mistakes drift: " + lesson.LessonCode);
        // The overlapping G2 array lesson can legitimately move directly
        // from pre-V9 Rich Content to both source-verified shared corrections.
        // Accept only its frozen raw or first-layer concept body, never drift.
        var allowedOldConcept = g.Code == "PED:US-CCSS-MATH:G2:U08:L07" &&
            version == CommonCoreRichContentCorrections.CorrectionContentVersion &&
            CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
                .IsVerifiedOriginalOrCorrectedConceptState(g.Code, actual.KeyConceptsAndRules);
        if (actual.Title != effective.Title ||
            actual.Explanation != effective.Explanation ||
            (actual.KeyConceptsAndRules != effective.KeyConceptsAndRules && !allowedOldConcept) ||
            actual.WorkedExamples != effective.WorkedExamples ||
            actual.QuickSummary != effective.QuickSummary)
            throw new InvalidOperationException("V9 shared comparison stored unrelated mathematics drift: " + lesson.LessonCode);
    }
}

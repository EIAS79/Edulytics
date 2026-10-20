using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source-goal teaching for G1 ordinals 81–85, continuous with
/// Unit 4 L21 source-audited work and Unit 4 L22–23 full-grade work.
/// No original student-task or teacher PLC certification is implied.</summary>
public static class CommonCoreG1FullGradeFinalUnit4Corrections
{
    public const string Version="us-ccss-g1-full-grade-u04-l16-l20-source-teaching-v1";
    public const string OriginalComparisonsVersion="us-ccss-g1-u04-l16-l18-original-activities-v2";
    public const string OriginalLesson16SourceContentVersion="us-ccss-g1-u04-l16-original-student-activities-v3";
    public const string OriginalLesson17SourceContentVersion="us-ccss-g1-u04-l17-original-student-activities-v3";
    public const string OriginalLesson18SourceContentVersion="us-ccss-g1-u04-l18-original-student-activities-v3";
    private static bool IsOriginalComparisonLesson(string code)=>code is
        "PED:US-CCSS-MATH:G1:U04:L16" or
        "PED:US-CCSS-MATH:G1:U04:L17" or
        "PED:US-CCSS-MATH:G1:U04:L18";
    private sealed record Repair(string Code,string SourceUrl,string Sha,string[] Outcomes,
        string Title,string Explanation,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new(@"PED:US-CCSS-MATH:G1:U04:L16",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-16/preparation.html",@"d764f755ec9e673edcc52916b619fd43ce29871330e55b616c82235b7ec33121",[@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Write Comparisons with Symbols PLC Activity",
            @"Compare two-digit numbers and WRITE true statements with >, < or =. In the teacher-led PLC discussion, explain the place-value reasoning used to choose a symbol.",
            @"Example 1: The digits 8 and 5 can form 85 and 58; 85 > 58 because eight tens are more than five tens. Example 2: 42 < 47 because they have equal tens but 2 ones is fewer than 7. Example 3: 63 = 63. Original 2021 warm-up: 49>45 and 45<49 express the SAME comparison with reversed sides. Original symbol blanks: 56>26; 72<78; 6<55; 92>29; 23=23. Original source number boxes: □>78 accepts ANY two-digit 79 to 99; 39<□ accepts ANY two-digit 40 to 99; 13=□ requires exactly 13; □<□ and □>□ each accept MANY ordered pairs of two-digit numbers. Do not reduce student-choice comparisons to one preselected answer.",
            @"Step 1: Model or identify the tens and ones in each number. Step 2: Compare tens first and then ones if tied. Step 3: Choose and WRITE >, < or =. Step 4: Explain why the statement is true. Step 5: In the original Greatest of Them All classroom game, place drawn digits into a two-digit number without moving a previously placed digit; compare both numbers and award the partner point according to the teacher-led rules.",
            @"Do not compare only the ones digits or reverse the symbol. Rotating a card does not change its place value; 85 is greater than 58.",
            @"I can write true comparisons using two-digit place value and symbols."),
        new(@"PED:US-CCSS-MATH:G1:U04:L17",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-17/preparation.html",@"2ed667761176401d13ff64b8670dd09c4e643480721d3d46588b29737d7b106b",[@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Compare and Order Numbers",
            @"Compare and ORDER several numbers within 99 using tens and ones. The original 2021 Student Facing activity begins with the ordered sequence 1, 5, 10, 50, 99; students must INSERT five more numbers 49, 8, 25, 98, 13 in their correct positions, then justify two placements. A three-number order check is only a small subset of the source task.",
            @"Example 1: 37 < 49 < 52 is increasing because 3 tens, 4 tens, and 5 tens. Example 2: Order 64, 69, 61 as 61 < 64 < 69. Example 3 (original insertion): Start at 1, 5, 10, 50, 99. Insert 49, 8, 25, 98 and 13, yielding 1, 5, 8, 10, 13, 25, 49, 50, 98, 99. Explain how each inserted number fits between neighbors. Original 2021 warm-up Which One Does Not Belong? asks pupils to discuss four image cards A/B/C/D; original teacher identifies C (35<20) as FALSE since 3 tens exceed 2 tens. The original card figures and pupil explanations require source media/teacher review, not digital inference.
Original 2021 Activity 1 orders physical card sets A, B, C and D by represented quantity, including cubes/base-ten diagrams. It allows mixing two sets if there is time; actual board artwork/partner sorting remains teacher-assessed.
Original 2021 extension also asks for numbers that make 25<□ and 25>□ true; ANY correct number is valid, for example 26/98 and 24/8; do not auto-grade one exemplar as the only possible answer.",
            @"Step 1: Read every number card. Step 2: Compare tens. Step 3: If tens match, compare ones. Step 4: Place every card in the requested order. Step 5: Read each comparison to check it is true.",
            @"Do not sort by the ones alone; 49 is less than 52 even though nine ones are greater than two ones.",
            @"I can order two-digit numbers and justify each adjacent comparison."),
        new(@"PED:US-CCSS-MATH:G1:U04:L18",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-18/preparation.html",@"431537a16be04c98f8b8f7bd149d2202ae2c6202f890e8e741aabeb2a137848f",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.NBT.C.6"],
            @"Center Day 3",
            @"Teacher-led center day: count collections, create/compare/order numbers through 99 and write numerals. Different center activities have different response formats.",
            @"Example 1: Five tens and two ones make 52. Example 2: 53 > 47 because 5 tens exceed 4. Example 3: Order 32, 48 and 39 as 32 < 39 < 48. Original 2021 number talk: 32+10=42; 32+10+10=52; 32+20=52; 32-20=12. The two twenty-addition expressions give equal sums but different written groups of tens. Students explain why 32+10+10 and 32+20 both give 52.
Original Activity 1 Get Your Numbers in Order requires two-digit game cards to be placed in increasing order without rearranging previous placements; cards that cannot fit cause a PASS and one penalty point, fewest points wins under teacher supervision.
Original Activity 2 real center choices: Greatest of Them All, Grab and Count, or Write Numbers. Students may choose and switch activities; their partner cooperation, strategy and gameboard cannot be certified by an automatically graded digital number answer.",
            @"Step 1: Read the actual center instruction. Step 2: Bundle complete tens when counting. Step 3: Compare tens before ones for ordering. Step 4: Write accurate two-digit numerals and check with objects or number cards. Step 5: For the original Get Your Numbers in Order center, players take turns placing two-digit cards in increasing order; an unplaceable card is a pass that scores a point, and the lower total wins under teacher supervision.",
            @"Solving one digital number fact does not prove a physical center game or partner conversation was completed.",
            @"I can count, compare, order and write two-digit numbers in the appropriate center."),
        new(@"PED:US-CCSS-MATH:G1:U04:L19",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-19/preparation.html",@"d74fcda3398cc4843865133bfdbac41151b56c6c46fb0f35a2e1992300b4eeec",[@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.B.2",@"CCSS:1.NBT.B.3"],
            @"Make Two-digit Numbers",
            @"Original 2021 Activity 1: make 65 from a bag with FIVE existing ten-towers and FIFTEEN loose ones. You CANNOT break a tower or build a new tower in that task; keep five tens plus fifteen ones. Original Activity 2 then asks for DIFFERENT ways to make 37, where decomposing a ten into ten ones IS allowed. Distinguish the two activities; a nonstandard representation can have more than nine loose ones.",
            @"Original example 1: 65 = 5 pre-existing ten-towers and 15 loose ones, without breaking or constructing towers. Original example 2: 37 = 3 tens + 7 ones = 2 tens + 17 ones = 1 ten + 27 ones. Further practice: 43 = 4 tens and 3 ones = 3 tens and 13 ones.",
            @"Step 1: In the original 65-cube bag task, count the FIVE pre-existing ten-towers and fifteen loose ones; DO NOT break or create towers. Step 2: Write 5 tens + 15 ones = 65. Step 3: For the different original 37 task, start with 3 tens + 7 ones; now you MAY break one complete ten into ten singles. Step 4: Record a different equivalent way such as 2 tens + 17 ones. Step 5: Verify each representation keeps the original total.",
            @"Three tens and thirteen ones is 43, NOT 313. Do not reduce a ten without adding exactly ten ones.",
            @"I can represent two-digit numbers with different tens-and-ones groupings."),
        new(@"PED:US-CCSS-MATH:G1:U04:L20",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-4/lesson-20/preparation.html",@"e144eb9ac5e352e009778971ebfff7333dcd91a7112273c51bf4d2bb7438944c",[@"CCSS:1.NBT.B.2"],
            @"Make Two-Digit Numbers in Different Ways PLC Activity",
            @"Teacher-led PLC: identify and explain two-digit numbers represented in different tens-and-ones groupings. Original learner activity asks HOW MANY WAYS you can make a number (e.g. 94). Show at least two distinct correct models, exchange whole tens for 10 ones without changing the total, and reason about ALL possibilities from zero full tens up to the usual tens count. Physical cube arrangements, diagrams and partner discussion remain teacher-reviewed.",
            @"Example 1: 62 = 6 tens 2 ones = 5 tens 12 ones = 4 tens 22 ones; with 0 through 6 tens there are seven distinct decompositions. Example 2: 47 = 4 tens 7 ones = 3 tens 17 ones = 2 tens 27 ones. Example 3: 94 = 9 tens 4 ones = 8 tens 14 ones = ... = 0 tens 94 ones, ten different decompositions if loose ones are allowed.",
            @"Step 1: Read a two-digit numeral. Step 2: Show a tens-and-ones model. Step 3: Trade one ten for ten ones. Step 4: Compare the equivalent representations. Step 5: Discuss why they are equal when a teacher or peer is available. ORIGINAL ACTIVITY 2 missing-number bags also require reasoning, not just exchanging full tens: Bag A has five tens and two ones (52). Bag B has four tens and twenty-five ones (65). Bag C has 49 cubes with 29 ones, so it has TWO tens. Bag D has 36 cubes with two tens, so it has SIXTEEN ones. Learners show each bag in drawings, numbers or words; the optional pupil-created mystery bag and partner exchange are teacher-reviewed.",
            @"Do not reject a valid regrouping just because it has 12 loose ones, and do not claim physical PLC discussion was digitally certified.",
            @"I can match and explain equivalent two-digit representations.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&doc.VersionCode=="CCSSM-2010"
        ?Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode):null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not a G1 U04 L16-L20 original-source target");
        if(lesson.SourceUrl!=r.SourceUrl||!string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("G1 U04 source/body/outcome drift: "+lesson.LessonCode);
        return r;
    }
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(en.Explanation==r.Explanation&&en.WorkedExamples==r.Examples&&
               en.StepByStepSolutions==r.Steps&&en.CommonMistakes==r.Mistakes&&en.QuickSummary==r.Summary)continue;
            if(!en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
               !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
                throw new InvalidOperationException("G1 U04 previous teaching body is unexpected: "+lesson.LessonCode);
            en.Title=r.Title;en.Explanation=r.Explanation;en.KeyConceptsAndRules=r.Explanation;
            en.WorkedExamples=r.Examples;en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)
            ?(lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L16"?OriginalLesson16SourceContentVersion:lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L17"?OriginalLesson17SourceContentVersion:lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L18"?OriginalLesson18SourceContentVersion:IsOriginalComparisonLesson(lesson.LessonCode)?OriginalComparisonsVersion:Version)
            :previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)&&
        (previous==CommonCoreRichContentCorrections.CorrectionContentVersion||
         (IsOriginalComparisonLesson(lesson.LessonCode)&&previous==Version)||
         (lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L16"&&previous==OriginalComparisonsVersion)||
         (lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L17"&&previous==OriginalComparisonsVersion)||
         (lesson.LessonCode=="PED:US-CCSS-MATH:G1:U04:L18"&&previous==OriginalComparisonsVersion));
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G1 U04 previous source body version drift: "+lesson.LessonCode);
        if(translations.Count==0)return;
        if(translations.Count!=1||translations.Single().CultureCode!="en"||
           !translations.Single().WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !translations.Single().StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("G1 U04 previous persisted text drift: "+lesson.LessonCode);
    }
}

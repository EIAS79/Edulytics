using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-specific IM 2021 Grade 2 Unit 1 Lessons 6-7: optional physical
/// Number Puzzles center and authentic class-collected transportation data.
/// Strict source/body/edition and persisted-past-version guards. No fake
/// teacher observations or publisher-copied figures.
/// </summary>
public static class CommonCoreG2Unit1CentersAndRealDataCorrections
{
    public const string Version = "us-ccss-g2-u01-l06-l07-real-source-v1";
    private sealed record Repair(
        string Code, string Url, string CanonicalSha, string[] Outcomes,
        string OldExplanation, string OldConcepts, string OldSteps,
        string OldMistakes, string OldSummary,
        string NewExplanation, string NewConcepts, string NewSteps,
        string NewMistakes, string NewSummary);

    private static readonly Repair[] Repairs =
    [
        new Repair("PED:US-CCSS-MATH:G2:U01:L06", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-6/preparation.html", "f85ba3b60a0addb08537b07a32e829ca42afe54ad64d84d47a908fa68340762f",
            new[] { "CCSS:2.NBT.B.5", "CCSS:2.OA.B.2" },
            "Learning goal: You will practice adding and subtracting within 10, 20, or 50.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In both activities, you have a choice between 2 centers introduced in previous lessons. You choose the center you would like to participate in, and work with a partner on that activity.",
            "Key ideas:\n- Learn the structure of center day lessons\n- Practice adding and subtracting within 10 or 20\n- Practice adding within 50\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
            "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
            "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
            "You will practice adding and subtracting within 10, 20, or 50. This lesson is optional because it is an opportunity for extra practice that not all classes may need.",
            "Optional Center Day 1: practise addition and subtraction with a partner. Start with the number talk 14 + 22, 19 + 22, 15 + 25, 17 + 25; explain how tens and ones help. Activity 1 is Number Puzzles Stage 2, where digit cards complete addition and subtraction equations within 20 and each physical card can be used only once on the whole gameboard. Activity 2 offers a free choice of previously taught centers: What's Behind My Back?, How Close?, and Number Puzzles. Choose, work with a partner, and reflect on what you improved. The teacher evaluates gameboard placement and partner participation.",
            "Number Puzzles Stage 2: fill missing numbers in addition and subtraction equations within 20, using every digit card at most once on the entire gameboard, not once per equation.\nCenter choices: What's Behind My Back?, How Close?, Number Puzzles; choose a stage you have already learned and work with a partner.\nFor two-digit sums within 50, split tens and ones; 14 + 22 = 36 and 19 + 22 = 41. A mental answer alone does not demonstrate the student-led center task.",
            "Step 1: In the warm-up, calculate each two-digit sum using tens and ones, and describe the strategy.\nStep 2: For Number Puzzles Stage 2, inspect the teacher-provided actual gameboard, each blank and available physical digit cards; use each card at most once across the gameboard.\nStep 3: Check every addition and subtraction equation within 20 and check the global once-per-card constraint. Do not certify a gameboard without knowing the actual cards and equations.\nStep 4: During center choice, choose an introduced activity, record mathematical reasoning as appropriate, and exchange turns with a partner.\nStep 5: Reflect with the teacher on the new strategy or activity that became easier. Human classroom practice is not proven by a digital warm-up score.",
            "Mistake: using one digit card repeatedly in different equations. The source says a card can be used only once per gameboard.\nMistake: describing both center activities as an unrestricted choice; Activity 1 is Number Puzzles, whereas Activity 2 allows several center choices.\nMistake: claiming an automatically correct numerical answer proves physical card placement, partner turns, or Math Community discussion.",
            "Optional Center Day 1: reason about two-digit sums within 50, solve physical within-20 Number Puzzles with digit cards used only once across the board, then select a previously taught partner center. Teacher observes the center tasks."),
        new Repair("PED:US-CCSS-MATH:G2:U01:L07", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-7/preparation.html", "679c5c596daffb1be11201abf369b2fff0d5e9e696bdd450aea8e545e730e0ea",
            new[] { "CCSS:2.MD.D.10" },
            "Learning goal: You will the mathematical purpose of this lesson is to elicit and build on your previous understandings and experiences with organizing and representing data.\nIn grade 1, you collected data and represented it in different ways. In this lesson, you collect data about your class, answer questions, discuss the need to represent data in an organized way, and create your own representations of your data. In upcoming lessons, you learn how to interpret and represent data using picture and bar graphs.",
            "Key ideas:\n- Analyze different representations of the same data to identify common features\n- Represent data in a way that makes sense to them\n- Read category labels, values, and scales before comparing or calculating with a data display.",
            "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
            "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
            "You will the mathematical purpose of this lesson is to elicit and build on your previous understandings and experiences with organizing and representing data. In grade 1, you collected data and represented it in different ways.",
            "Collect and represent data from your REAL CLASS about how people traveled to school today. First notice and wonder about transportation pictures; then each learner chooses a truthful travel category and adds their own response to a class display. This display may be disorganized at first. Partners later reorganize the same genuine responses using categories, labels, pictures, tally marks or other symbols. Compare different representations, explain why one is easier to read and discuss what the data show. Never fabricate actual classmates' choices.",
            "Data are information about a group. Each student's own travel mode is one observation; do not treat a classroom survey as a fictional number puzzle.\nCollect responses, then make category counts without omissions or double counting. For the SAME collected data, all faithful representations must keep each category's total unchanged.\nA display does not have to be grouped at first; organizing alike responses and labeling categories helps others read it.\nThe source lesson is formative, relying on real student responses, group-created visual representations and teacher comparison, not a single predetermined numerical answer.",
            "Step 1: Look at the transportation pictures (walking, car, train, school bus); say what you notice and wonder.\nStep 2: Survey your actual classroom: every participating child chooses one truthful way they traveled to school today, and adds one response to the class display.\nStep 3: Count each real category carefully and keep a record of the total number of responses.\nStep 4: In pairs, organize and represent the SAME data visually; choose pictures, grouped marks, tallies or labeled category symbols.\nStep 5: Compare two authentic displays, checking category totals and labels and explaining which is easier to interpret.\nStep 6: Give another student a chance to describe your display; teacher reviews the survey, drawings, collaboration and justification.",
            "Mistake: inventing class data and claiming they were observed. Only actual survey responses constitute original-source evidence.\nMistake: counting a student's response twice or dropping one while reorganizing.\nMistake: claiming an unorganized display is automatically wrong; the source intentionally starts with a freely arranged class display.\nMistake: presenting a pre-drawn bar graph as if it proves the original student-created representation.",
            "Collect authentic travel-to-school responses from the class; first make a freely arranged display, then reorganize the same data into clear grouped or labeled representations. Compare and explain them with the teacher.")
    ];

    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode == "CCSSM-2010" &&
        doc.ContentVersion == "phase30-common-core-rich-content-v1"
            ? Repairs.SingleOrDefault(r => r.Code == lesson.LessonCode) : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) => Find(doc,lesson) is not null;

    private static Repair Guard(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        var r = Find(doc,lesson) ??
            throw new InvalidOperationException("Not an original G2 center/data lesson");
        if (lesson.SourceUrl != r.Url ||
            !string.Equals(lesson.CanonicalBodySha256, r.CanonicalSha,
                StringComparison.OrdinalIgnoreCase) ||
            !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException(
                "Grade 2 center/data source URL, SHA or official outcome drift");
        return r;
    }

    private static bool Matches(CanonicalLessonContentPackTranslation t, Repair r, bool newer) =>
        t.Explanation == (newer ? r.NewExplanation : r.OldExplanation) &&
        t.KeyConceptsAndRules == (newer ? r.NewConcepts : r.OldConcepts) &&
        t.StepByStepSolutions == (newer ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (newer ? r.NewMistakes : r.OldMistakes) &&
        t.QuickSummary == (newer ? r.NewSummary : r.OldSummary);

    private static bool Matches(CurriculumLessonContentTranslation t, Repair r, bool newer) =>
        t.Explanation == (newer ? r.NewExplanation : r.OldExplanation) &&
        t.KeyConceptsAndRules == (newer ? r.NewConcepts : r.OldConcepts) &&
        t.StepByStepSolutions == (newer ? r.NewSteps : r.OldSteps) &&
        t.CommonMistakes == (newer ? r.NewMistakes : r.OldMistakes) &&
        t.QuickSummary == (newer ? r.NewSummary : r.OldSummary);

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            var r=Find(doc,lesson);
            if(r is null) continue;
            Guard(doc,lesson);
            var t=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,r,true)) continue;
            if(!Matches(t,r,false))
                throw new InvalidOperationException(
                    "G2 source-specific lesson has unexpected previous prose: "+lesson.LessonCode);
            t.Explanation=r.NewExplanation;
            t.KeyConceptsAndRules=r.NewConcepts;
            t.StepByStepSolutions=r.NewSteps;
            t.CommonMistakes=r.NewMistakes;
            t.QuickSummary=r.NewSummary;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previousVersion) =>
        IsTarget(doc,lesson)?Version:previousVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson) &&
        version == CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson, string previousVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        var r=Guard(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previousVersion))
            throw new InvalidOperationException("Unexpected prior G2 center/data content version");
        if(translations.Count==0) return; // JSON-only metadata.
        if(translations.Count!=1 || translations.Single().CultureCode!="en")
            throw new InvalidOperationException("Unexpected G2 center/data persisted languages");
        var old=translations.Single();
        var current=lesson.Translations.Single(t=>t.CultureCode=="en");
        if(old.Title!=current.Title ||
           old.WorkedExamples!=current.WorkedExamples ||
           !(Matches(old,r,false)||Matches(old,r,true)))
            throw new InvalidOperationException(
                "G2 center/data persisted prose is not a reviewed predecessor");
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// IM2021 Grade 2 U01 L15 source-specific compare tape stories.
/// Distinguish unknown LARGER, SMALLER and DIFFERENCE, preserve multiple
/// mathematically valid equation cards. Exact canonical predecessor guard.
/// </summary>
public static class CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l15-three-compare-unknown-positions-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L15";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-15/preparation.html";
    private const string SourceSha="ecb8c8c64a965e0680d39195d58a4c4b03ed79f77c86a161a993d059bf43df92";
    private sealed record Content(
        string OldExplanation,string OldConcepts,string OldExamples,
        string OldSteps,string OldMistakes,string OldSummary,
        string NewExplanation,string NewConcepts,string NewExamples,
        string NewSteps,string NewMistakes,string NewSummary);
    private static readonly Content C=new(
        "Learning goal: You will connect Compare problems with unknowns in all positions to tape diagrams.\nIn this lesson, you continue to make sense of tape diagrams as you connect them to story problems. The story problems in this lesson include Compare problems with an unknown difference, an unknown larger number, or an unknown smaller number. Because these problem types are familiar from grade 1, you also begin to work with numbers within 100 in this lesson.",
        "Key ideas:\n- Interpret tape diagrams that represent Compare problems, with unknowns in all positions within 100\n- In this lesson, you continue to make sense of tape diagrams as you connect them to story problems\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Find the value of each sum mentally. \\(4 + 8 + 2\\) \\(4 + 5 + 3 + 2\\) \\(9 + 3 + 1\\) \\(9 + 5\\)\nApproach: Represent both quantities, compare their values, and state the relationship using words, symbols, or a clear explanation.\nExample 2: Diego has 55 shells. Lin has 20 fewer shells than Diego. Use the story problem to complete the diagram. Write an equation to show how you could find the unknown number of shells. Use a question mark to represent the unknown.\nApproach: “How did you use the diagram to write your equation?” (The diagram helped me see I could start with 55 and take away 20 to find Lin’s shells. The diagram helped me see that Lin’s shells and 20 more would be the same as 55.)",
        "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.",
        "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.",
        "You will connect Compare problems with unknowns in all positions to tape diagrams. In this lesson, you continue to make sense of tape diagrams as you connect them to story problems.",
        "Goal: interpret Compare word problems with the unknown in ANY of three positions (smaller quantity, larger quantity or difference) within 100, and match EACH situation to a correctly labelled tape diagram and any equivalent equations. Warm-up: make ten mentally in 4+8+2, 4+5+3+2, 9+3+1, and 9+5, and explain groupings. Activity 1 Shell Collections: story is FIRST given without numbers; Diego has MORE shells than Lin. When numbers are revealed, Diego has 55 shells and Lin has 20 FEWER. Label Diego's full strip 55, Lin's shorter unknown strip ?, and the extra dashed strip 20; find Lin's 35 shells, showing 55−20=35 and 35+20=55. Activity 2 Card Sort At the Beach: learners work in pairs matching a written Compare story to the correct tape diagrams and MORE THAN ONE valid equivalent equation; explain which position contains the unknown. Source card images are not reproduced; Edulytics creates its own card scenarios, label-safe diagrams and original exit ticket.",
        "A Compare situation has two quantities and a difference. The larger quantity = smaller quantity + difference. In a tape diagram, the longer whole strip represents the LARGER value and the second aligned strip can be divided into smaller value and outlined difference.\nUNKNOWN SMALLER: Diego has 55 shells; Lin has 20 fewer. Tape shows Diego55 = Lin? + extra20. Equations 55−20=35, 35+20=55, or 55−?=20, so Lin35.\nUNKNOWN LARGER: if a child has 34 stones and another has 20 more, then bigger is ?, smaller34, difference20. 34+20=54 or 54−20=34.\nUNKNOWN DIFFERENCE: if a child has 62 stones and another has 50, then the missing extra part is 12. 62−50=12 or 50+12=62.\nAlways decide WHO has more and WHAT the question mark represents BEFORE choosing an operation. 'Fewer' alone never proves whether to add or subtract until you identify the requested unknown.\nFor card sort, equivalent equations or differently orientated diagrams can match the SAME story. Students should justify using named quantities and unknown placement, not mark one equivalent equation as uniquely correct.",
        "Number Talk: 4+8+2=14 by 8+2=10 then+4; 4+5+3+2=14 because 5+3+2=10; 9+3+1=13; 9+5=14 using 9+1+4.\nSource activity Shell Collections: Diego55 shells, Lin20 fewer. Two aligned tapes show Diego55, Lin? with extra20. Lin35; 55−20=35, 35+20=55 and 55−35=20 all explain the same story.\nOriginal Edulytics card: 'Tala has 34 seashells. Rami has 20 MORE shells than Tala. How many does Rami have?' Unknown larger54, 34+20=54 and 54−20=34.\nOriginal Edulytics card: 'Nora has 62 pebbles, Omar has 50. How many MORE does Nora have?' Unknown difference12, 62−50=12 and 50+12=62.\nIndependent Edulytics Cool-down: 'Mira has 67 postcards, Sami has 30 FEWER. How many does Sami have?' Unknown smaller37. 67−30=37 and 37+30=67. A different larger-unknown and difference-unknown prompt reinforces that unknown position is not always the same.",
        "Step 1: Read the compare story WITHOUT calculating. Name the people and decide who has MORE and who has FEWER. Do not choose addition or subtraction just from a keyword.\nStep 2: Identify the unknown position: (i) larger quantity ?, (ii) smaller quantity ?, or (iii) difference ?. Label that specific strip or extra dashed part with a question mark.\nStep 3: Draw or place two aligned original rectangular tape strips. The larger quantity is the full length; the shorter amount plus outlined gap reaches the same total. Give each person's strip a label and units.\nStep 4: Form an equation matching the placement: smaller + difference = larger, larger − smaller = difference, or larger − difference = smaller. Leave an explicit unknown in the equation before solving.\nStep 5: In the public Shell Collections example, Diego55 and Lin20 fewer, therefore Lin35 and valid equations 55−20=35, 35+20=55 or 55−35=20.\nStep 6: For source Card Sort, examine story cards, independent Edulytics tape-diagram cards and multiple equation cards. Match those with the SAME larger, smaller and difference quantities. Several equations may be mathematically equivalent. Explain your choice to a partner.\nStep 7: Reconsider whether the dashed question mark refers to the bigger total, smaller bar or missing difference. Teacher reviews the genuine original diagram, partner debate and mathematical language, not just the integer answer.",
        "Mistake: assuming the word FEWER always means the larger number is unknown. Diego55 is the LARGER known number; Lin's SMALLER value35 is unknown.\nMistake: placing 20 as Lin's shell count rather than the DIFFERENCE between Diego and Lin.\nMistake: counting the dashed region as another student's separate collection instead of the extra part that aligns two strips.\nMistake: writing 55+20=75 for a problem where Lin is FEWER, or subtracting 20 from 34 when another child has MORE than Tala.\nMistake: solving a difference-unknown 62 versus 50 problem as 62+50 instead of finding12.\nMistake: rejecting an equivalent equation card simply because it uses addition rather than subtraction; multiple equations may represent the same tape diagram.\nMistake: automatically grading student-made tape drawings or pair discussion from a single correct integer. Teacher review is essential.",
        "Read Compare stories within 100, locate whether the LARGER, SMALLER or DIFFERENCE is unknown, label first-party aligned tape diagrams and match each story to MORE THAN ONE equivalent equation. Original Diego55/Lin20-fewer gives Lin35; make-ten warm-up, independent card sort and new Edulytics Cool-down require mathematical explanation and teacher review.");

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase30-common-core-rich-content-v1" &&
        lesson.LessonCode==Code;

    private static void Guard(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        if(!IsTarget(doc,lesson)||lesson.SourceUrl!=SourceUrl||
            !string.Equals(lesson.CanonicalBodySha256,SourceSha,
                StringComparison.OrdinalIgnoreCase)||
            !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.OA.A.1","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U01 L15 source identity/standard changed");
    }

    private static bool Matches(CanonicalLessonContentPackTranslation t,bool after)=>
        t.Explanation==(after?C.NewExplanation:C.OldExplanation)&&
        t.KeyConceptsAndRules==(after?C.NewConcepts:C.OldConcepts)&&
        t.WorkedExamples==(after?C.NewExamples:C.OldExamples)&&
        t.StepByStepSolutions==(after?C.NewSteps:C.OldSteps)&&
        t.CommonMistakes==(after?C.NewMistakes:C.OldMistakes)&&
        t.QuickSummary==(after?C.NewSummary:C.OldSummary);
    private static bool Matches(CurriculumLessonContentTranslation t,bool after)=>
        t.Explanation==(after?C.NewExplanation:C.OldExplanation)&&
        t.KeyConceptsAndRules==(after?C.NewConcepts:C.OldConcepts)&&
        t.WorkedExamples==(after?C.NewExamples:C.OldExamples)&&
        t.StepByStepSolutions==(after?C.NewSteps:C.OldSteps)&&
        t.CommonMistakes==(after?C.NewMistakes:C.OldMistakes)&&
        t.QuickSummary==(after?C.NewSummary:C.OldSummary);

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            Guard(doc,lesson);
            var english=lesson.Translations.Single(t=>t.CultureCode=="en");
            if(Matches(english,true))continue;
            if(!Matches(english,false))
                throw new InvalidOperationException("G2 U01 L15 unexpected previous learner prose");
            english.Explanation=C.NewExplanation;
            english.KeyConceptsAndRules=C.NewConcepts;
            english.WorkedExamples=C.NewExamples;
            english.StepByStepSolutions=C.NewSteps;
            english.CommonMistakes=C.NewMistakes;
            english.QuickSummary=C.NewSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous) =>
        IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous) =>
        IsTarget(doc,lesson)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("G2 U01 L15 expected original rich-content version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L15 unexpected previous translations");
        var old=rows.Single();var incoming=lesson.Translations.Single(t=>t.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Matches(old,false)||Matches(old,true)))
            throw new InvalidOperationException("G2 U01 L15 persisted teaching changed");
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Source SHA guarded G2 U04 L02 count-by-five, physical classroom yarn and actual student diagram review.</summary>
public static class CommonCoreG2Unit4NumberLineFeaturesTeachingCorrection
{
    public const string Version="usccss-g2-u04-l02-source-number-line-features-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L02";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-2/preparation.html";
    private const string Sha="de2373a2185d359f59a63fa2b36550e169e6591f27c90026c7fadeb39ca45cb1";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
       ["Learning goal: You will identify and describe the defining features of a number line.\nIn a previous lesson, you were introduced to number lines and represented the location of numbers with labeled tick marks and points within 20. In the first activity, you each get a number and work together to decide how to place your numbers on a class number line, thinking about the importance of equal spacing and sequencing numbers from left to right. In the second activity, you analyze different student-created number lines and suggest revisions that should be made to make each number line a more accurate representation.",
        "Key ideas:\n- Describe the structure of a number line\n- In a previous lesson, you were introduced to number lines and represented the location of numbers with labeled tick marks and points within 20\n- The addends are the quantities being combined and the sum is the resulting total; the expression must match the representation.",
        "Example: identify and describe the defining features of a number line.\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.",
        "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
        "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
        "You will identify and describe the defining features of a number line. In a previous lesson, you were introduced to number lines and represented the location of numbers with labeled tick marks and points within 20."],
       ["IM K–5 2021 Grade2 Unit4 Lesson2 Features of a Number Line. CCSS2.MD.B.6 and2.NBT.A.2. Warm-up CHORAL COUNT: start at0 and count by FIVES (5,10,15,...,100), reflecting on repeating final digits 0/5 and two 5s building a new ten. Activity1 Class Number Line: on REAL taut yarn stretched across the room, place SOURCE number cards0 through30. The teacher hangs0 first and calls additional pupils with number cards in unpredictable/random order; pupils justify where each card belongs relative to zero and others and move/revise placements when more cards arrive. Not all cards must be handed out. Equal-distance unit intervals and ascending labels make an accurate line; class participation, physical positions and student discussion are teacher-only. Activity2 Analyze Number Lines: ORIGINAL drawings attributed to Jada, Andre and Elena each require improvements and three-pupil MLR8 discussion. The public text confirms Jada's diagram is 0..10 with a blank tick between1 and2 and between5 and6, but visual details of Andre and Elena's separate images are not embedded as accessible numeric data. Do NOT invent their positions, tick counts or specific unique corrections: educator must display/inspect original artwork and pupils should justify repairs to equal spacing and correct numeric positions from zero. Original independent task: complete ALL labels on an evenly spaced 0..10 line whose first tick is0 (11 ticks in total). Student-artifact work and original restricted Cool-down are not automated. Edulytics supplies independent, explicitly first-party numerical exercises.",
        "Count in groups of5 from0 up to100 inclusive: 0,5,10,...,95,100. Each advance adds5; even multiples of5 end in0 or5, and two successive +5 jumps form10. A correct number line maps every integer n to a point n identical-length unit intervals RIGHT of zero. On a classroom STRING line representing0–30, placings are judged by actual physical DISTANCE ratios, not just left-to-right order: 15 must be halfway between0 and30 if equal units are respected; missing cards do not remove unit spaces. All cards from0 to30 are valid; one card at a time added out of order may require rearrangement. On a 0–10 paper diagram, there are11 consecutive whole-number TICKS but only10 intervals. Evenly spaced tick marks with incorrect number labels still fail to give true numerical lengths from0; unlike sorting cards, a correct scale requires both ORDER and EQUAL SPACING, as well as zero anchor. Jada's published accessible description reports blank marks between1 and2 and between5 and6; neither missing labels nor their physical correction may be inferred by interpolating invented marks without viewing original figure. Andre and Elena's original drawing details remain teacher/image-only. First-party independent numerical prompts are labeled separate from original publisher images and Cool-down.",
        "PUBLIC original warm-up: the choral 5-count starts0,5,10,15,20,25...95,100; 5+5=10 is one repeated pattern. Real source demonstration: hang0 on genuine taut string, then if pupils with cards10,30 and20 arrive in unpredictable order, they must arrange by increasing lengths from0 and check whether10-to20 and20-to30 use the same number of unit intervals; these particular arrivals are an Edulytics EXAMPLE, not an asserted original observed class outcome. Source Activity2: inspect actual Jada/Andre/Elena diagrams; for each, identify one feature done well and an evidence-based revision. Text confirms Jada shows incorrect blank ticks between1 and2 and5 and6, but Andre/Elena positions cannot be extracted from available text and no automatic image-specific grading is claimed. SOURCE final task: an evenly spaced line0–10 with zero labeled requires labels1,2,3,4,5,6,7,8,9,10. FIRST-PARTY fictional point card at18 on a 0–30 line means18 unit intervals from0, point12 to18 distance6, and a separate missing tick between4 and6 is5.",
        "1. CHORAL COUNT aloud by5 from0 through100, noting 5+5 makes10, and ask students to explain observed repetitions; teacher observes actual oral participation. 2. Make a taut REAL physical yarn number line, place card0 at the beginning and invite pupils bearing SOURCE 0..30 number cards in random order. 3. Position each card at its true distance in equal unit intervals from zero; check that every label lies to the right of smaller numbers, and that multiples10 and20 are appropriately spaced on the same line. Revise physical card placements as new cards are added, discussing why the gap matters. 4. In source Activity2 display each authentic Jada, Andre and Elena figure from the original publisher. In groups of3 identify strengths and at least one needed revision for EACH figure: evaluate equal intervals, consecutive tick labels, and numerical lengths from zero. DO NOT assert exact Andre/Elena diagram faults without viewing their pictures. 5. On the independently printed source 11-tick 0–10 blank line, keep all ticks and write labels0,1,2,3,4,5,6,7,8,9,10 from left to right (zero already shown). 6. Independently solve Edulytics-made exact numerical count-by5, card positions, interval gaps and label-repair questions, checking arithmetic without claiming teacher-reviewed classroom accuracy. 7. Teacher reviews real string/card work, actual image-specific diagram revisions and MLR8 group explanations.",
        "Common mistakes: claiming consecutive5-counts are all ten apart, forgetting zero or counting to105 when warm-up ends at100. Placing classroom cards in numeric order BUT at unequal physical spaces, or treating vacant cards as removed physical unit intervals. Confusing the physical yarn's total length with a fixed standard inch/centimeter unit. Treating printed tick marks as correctly numbered when the interval scale or labels are wrong. Overlooking an extra blank tick between labels1 and2 or5 and6 in Jada's source drawing; assigning a made-up specific fault to Andre or Elena without looking at their actual publisher images. Claiming numeric responses certify peer cooperation or real physical card placement. For a 0..10 line, counting eleven tick marks as eleven intervals instead of ten.",
        "Original-source G2 U04 L02: count by five orally from0 through100; build a REAL 0..30 yarn class number line with number cards placed in random order and revise distances; critique ORIGINAL source Jada/Andre/Elena 0..10 number-line figures with teacher visual access and group discussion; complete labels1..10 on an 11-tick 0..10 line. Teacher-only physical and image-bound tasks are not replaced with invented data. Edulytics independent exact numerical companion is separate from restricted original assessment."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson)=>
        document.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode=="CCSSM-2010" &&
        document.ContentVersion=="phase30-common-core-rich-content-v1" &&
        lesson.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson)
    {
        if(!IsTarget(document,lesson)||lesson.SourceUrl!=Url||
           !string.Equals(lesson.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.6","CCSS:2.NBT.A.2"}))
            throw new InvalidOperationException("G2 U04 L02 original source or standards drift");
    }
    private static bool Matches(string[] current,bool after)=>current.SequenceEqual(after?S.After:S.Before);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach(var lesson in document.Lessons)
        {
            if(!IsTarget(document,lesson))continue;
            Guard(document,lesson);
            var translation=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Same(translation,true))continue;
            if(!Same(translation,false))throw new InvalidOperationException("G2 U04 L02 six original learner fields drift");
            translation.Explanation=S.After[0];translation.KeyConceptsAndRules=S.After[1];
            translation.WorkedExamples=S.After[2];translation.StepByStepSolutions=S.After[3];
            translation.CommonMistakes=S.After[4];translation.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,
        string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> stored)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))throw new InvalidOperationException("G2 U04 L02 predecessor version unknown");
        if(stored.Count==0)return;
        if(stored.Count!=1||stored.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L02 language drift");
        var old=stored.Single();var candidate=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=candidate.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 U04 L02 persisted fields drift");
    }
}

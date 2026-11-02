using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// IM2021 G2 U01 L14 tapes as bars aligned to unknown difference,
/// source PIZZA/HOT DOGS/BURGERS and apple/grape. Corrects ellipsis
/// original partial examples, not original publisher diagrams.
/// Locked URL/SHA and 2.MD.D.10 addressing outcome.
/// </summary>
public static class CommonCoreG2Unit1TapeDiagramTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l14-tape-compare-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L14";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-14/preparation.html";
    private const string Sha="f7bbe33e9ffd30ba24d06284d7c5deb2453591f2aa073d2545bb08e7004f5344";
    private sealed record Texts(
        string OldExplanation,string OldRules,string OldExamples,string OldSteps,
        string OldMistakes,string OldSummary,string NewExplanation,string NewRules,
        string NewExamples,string NewSteps,string NewMistakes,string NewSummary);
    private static readonly Texts T=new(
        "Learning goal: You will use your understanding of bar graphs to interpret tape diagrams and solve Compare problems with the difference unknown within 20.\nIn this lesson, you use your understanding of bar graphs to make sense of a new representation, the tape diagram. The tape diagram is a representation that can be used by you to represent story problems.",
        "Key ideas:\n- Solve Compare problems with an unknown difference within 20\n- Use understanding of bar graphs to make sense of tape diagrams\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: What do you notice? What do you wonder? Description: Diagram. Two rectangles of equal length. Rectangle on top labeled apple, partitioned into 15 equally shaded parts, total length, 15. Rectangle on the bottom labeled grape. partitioned into two parts. First part, partitioned into 8 equally shaded parts, total length, 8. Second part, partitioned into 7...\nApproach: “What does the question mark in the diagram represent?” (The difference between the number of you that picked apple juice and grape juice, how many more you picked apple juice than grape juice, how many fewer you picked grape juice than apple juice.)\nExample 2: Glue down the two bars that compare the number of students who picked hot dogs to the number who picked burgers. Write two statements that compare the number of students who picked hot dogs to the number who picked burgers.\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will use your understanding of bar graphs to interpret tape diagrams and solve Compare problems with the difference unknown within 20. In this lesson, you use your understanding of bar graphs to make sense of a new representation, the tape diagram.",
        "Grade 2 Unit 1 Lesson 14 moves from BAR GRAPHS to TAPE DIAGRAMS for Compare problems whose DIFFERENCE is unknown within 20. Warm-up Notice and Wonder: examine the public numerical values from a juice-flavor horizontal bar graph: lemonade 12, fruit punch 6, apple 15, grape 8. A new Edulytics two-bar tape diagram compares apple 15 with grape 8: draw the shorter 8 part plus an unknown difference to equal the longer 15. Activity 1 Party Time (Part 1): source food votes pizza 19, burgers 8 and hot dogs 14; physically cut or independently sketch rectangles proportional to their counts, place hot dogs and burgers bars side-by-side, label quantities, and write TWO truthful more/fewer comparison statements. Activity 2 Party Time (Part 2): complete tape diagrams with a total rectangular strip of 19 and another shorter strip of 8 or 14; label the missing difference, connect 19−8=11 and 19−14=5 to related addition equations. A tape diagram does not need the graph's numbered axis but MUST show which strip is larger, which amount is known and what the dashed ? means. Student-authored visual strips and real partner explanations remain teacher-reviewed. The original publisher Cool-down is restricted; Edulytics authors its own comparison mini-assessment with original diagrams.",
        "A BAR graph uses a numbered scale to compare category counts, while a TAPE diagram represents quantities with labeled rectangular strips: a LONGER strip is a larger amount, a SHORTER strip is a smaller amount, and a dashed or outlined missing part represents the DIFFERENCE, not another measured group.\nWhen apple is 15 and grape is 8, the tape diagram shows grape 8 + missing 7 = apple 15; 15−8=7. The question mark is the unknown difference, not the total or the grape quantity.\nThe public source Party Time data are pizza 19, burgers 8, hot dogs 14. The unknown comparison difference between hot dogs and burgers is 14−8=6; pizza and burgers is 19−8=11; pizza and hot dogs is 19−14=5.\nAn accurate tape diagram names BOTH actual categories and both KNOWN values, places the shorter bar against the longer bar's start and represents extra length with a missing part. Unit-one bar graph scale is optional after the tape labels are established.\nRelated equations smaller + difference = larger and larger − smaller = difference describe the SAME comparison. A subtraction equation such as 19−?=14 is another equivalent representation of the missing difference 5.\nThe student must make truthful 'more' and 'fewer' statements from the correct graph, explain why each number stands for its group or the gap, and show an original diagram, not just give the difference.",
        "Warm-up numerical facts (independent Edulytics redrawing): juice votes Lemonade 12, Fruit punch 6, Apple 15, Grape 8. Tape diagram: APPLE [15] compared with GRAPE [8] plus [??]. Missing part 7, since 8+7=15 and 15−8=7.\nActivity 1 source food votes (new Edulytics rectangles): Pizza19, Burgers8, Hot dogs14. Student compares hot dogs14 with burgers8, writes 14−8=6 or 8+6=14, and tells partner 'Six more selected hot dogs than burgers' and 'Six fewer selected burgers than hot dogs.'\nActivity 2 tape diagram A: larger pizza19 and smaller burgers8; difference ?=11. Equations 8+11=19 and 19−8=11. Tape diagram B: larger pizza19 and smaller hot dogs14; difference ?=5. Equations 14+5=19 and 19−14=5.\nIndependent Edulytics Cool-down: sample library choices Adventure16 and Fantasy9 (fictional teaching counts). Draw a larger bar 16 and a shorter bar9, label the missing outlined part7, and check 9+7=16 and 16−9=7.",
        "Step 1: Read the BAR GRAPH title and category labels, then notice which two quantities the problem COMPARES. Use the original scale to read each count.\nStep 2: Draw or arrange a first longer rectangle for the greater amount, and a second shorter rectangle for the lesser amount with both starting on the left. Name the categories next to the strips.\nStep 3: Add an outlined or dashed section to the shorter tape until both strips reach the SAME total length. Label the unknown extra section '?' because it is the DIFFERENCE, not an extra category.\nStep 4: Connect the tape to an addition equation: smaller + ? = larger. Solve for the difference by counting on or subtraction, and check with larger − smaller = ?.\nStep 5: Using original Party Time values, place Hot dogs14 above Burgers8 and write two truthful more/fewer statements. Their missing difference is 6.\nStep 6: Using source Party Time Part 2, complete Pizza19 compared with Burgers8 (gap11) and Pizza19 compared with Hot dogs14 (gap5). Explain where 19, 8, 14, 11 and 5 lie on the tapes.\nStep 7: Compare a student's physical diagram or drawing to a partner's: labels, known parts, larger total, and outlined missing section must all match the real mathematical quantities. Teacher checks genuine reasoning and diagram construction.",
        "Mistake: using a tape diagram to add food groups when the story asks 'how many more'; the answer is the gap, not the total 19+8.\nMistake: drawing both strips as equal solid lengths and placing '?' as a separate group; the dashed section extends the shorter strip to the SAME length as the larger.\nMistake: labeling 'apple 8 and grape15' because the bars were swapped. Recheck that the larger source apple count is15 and grape is8.\nMistake: putting 19 in the missing part and 8 as the total for pizza/burgers. Larger group is pizza19; the extra section is 11.\nMistake: losing category labels when strips are cut and rearranged physically. Write a clear name and quantity on each tape piece.\nMistake: treating the building-towards Grade2 2.OA.A.1 note as a new official addressing outcome. The Grade2 content pack officially maps 2.MD.D.10 here; 2.OA.A.1 is a source 'building towards' reference.\nMistake: marking an integer gap as evidence the pupil physically cut, glued or explained an accurate original tape diagram; rubric and teacher verification remain necessary.",
        "Compare quantities within 20 using first-party BAR GRAPHS and TAPE DIAGRAMS. Align strips, label known category counts and the missing DIFFERENCE, write both smaller+gap=larger and larger−smaller=gap. Source Party Time includes pizza19, burgers8 and hot dogs14; original diagrams and partner explanations are teacher-checked.");

    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" && d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10"}))
            throw new InvalidOperationException("G2 U01 L14 original tape diagram source identity drift");
    }
    private static bool Match(CanonicalLessonContentPackTranslation x,bool updated)=>
        x.Explanation==(updated?T.NewExplanation:T.OldExplanation)&&
        x.KeyConceptsAndRules==(updated?T.NewRules:T.OldRules)&&
        x.WorkedExamples==(updated?T.NewExamples:T.OldExamples)&&
        x.StepByStepSolutions==(updated?T.NewSteps:T.OldSteps)&&
        x.CommonMistakes==(updated?T.NewMistakes:T.OldMistakes)&&
        x.QuickSummary==(updated?T.NewSummary:T.OldSummary);
    private static bool Match(CurriculumLessonContentTranslation x,bool updated)=>
        x.Explanation==(updated?T.NewExplanation:T.OldExplanation)&&
        x.KeyConceptsAndRules==(updated?T.NewRules:T.OldRules)&&
        x.WorkedExamples==(updated?T.NewExamples:T.OldExamples)&&
        x.StepByStepSolutions==(updated?T.NewSteps:T.OldSteps)&&
        x.CommonMistakes==(updated?T.NewMistakes:T.OldMistakes)&&
        x.QuickSummary==(updated?T.NewSummary:T.OldSummary);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))throw new InvalidOperationException("L14 original prior learner prose mismatch");
            en.Explanation=T.NewExplanation;en.KeyConceptsAndRules=T.NewRules;
            en.WorkedExamples=T.NewExamples;en.StepByStepSolutions=T.NewSteps;
            en.CommonMistakes=T.NewMistakes;en.QuickSummary=T.NewSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))
            throw new InvalidOperationException("L14 unexpected predecessor content version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("L14 prior language rows unexpected");
        var old=rows.Single();var now=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=now.Title||!(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("L14 previous persisted learner content has drifted");
    }
}

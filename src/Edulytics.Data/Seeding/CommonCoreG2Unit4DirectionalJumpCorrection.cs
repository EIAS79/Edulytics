using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;
/// <summary>IM 2021 G2 2.4.7, exact source URL/SHA, standards and six-field persisted predecessor guards.</summary>
public static class CommonCoreG2Unit4DirectionalJumpCorrection
{
    public const string Version="g2u04-l07-original-arrow-direction-source-v1";
    private const string LessonCode="PED:US-CCSS-MATH:G2:U04:L07";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-7/preparation.html";
    private const string SourceSha="a9cac619384b144ba8ff64fbb9ae61f0d306568b1afa47bc1a8dd9799ced3d0e";
    private static readonly string[] Outcomes=["CCSS:2.MD.B.6"];
    private static readonly string[] OriginalFields=[
        "Learning goal: You will attend to the direction of the arrow on the number line to match addition and subtraction expressions.\nIn previous lessons, you learned that whole numbers can be represented by tick marks and points on the number line. You learned that numbers farther to the right on a number line are greater than those to the left. In this lesson, you build on that understanding to connect representations that use an arrow to show moving from one number to another on the number line (“jumps”) to the operations of addition and subtraction.",
        "Key ideas:\n- Recognize that on a number line, jumps to the right represent addition and jumps to the left represent subtraction\n- In previous lessons, you learned that whole numbers can be represented by tick marks and points on the number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Circle the equation represented on the number line. \\(10 + 5 = 15\\) \\(15 - 5 = 10\\) \\(8 + 5 = 13\\) \\(13 - 8 = 5\\) \\(12 - 2 = 10\\) \\(12 - 10 = 2\\) \\(4 + 10 = 14\\) \\(10 + 4 = 14\\) Explain why the other equation doesn’t match this number line.\nApproach: “We have learned that \\(4 + 10\\) and \\(10 + 4\\) have the same value. Why doesn’t \\(10 + 4\\) match this number line?” (The arrow shows addition, but the first point is on 4 and the arrow shows moving 10 units to the right.)\nExample 2: Cut out the equations. Paste each equation next to the number line that represents it. Paste the equation that didn’t have a match and represent it on a number line. A Caption: A B Caption: B C Caption: C D Caption: D E Caption: E F Caption: F G Caption: G H Caption: H \\(3 + 7 = 10\\) \\(10 - 7 = 3\\) \\(12 + 6 = 18\\) \\(6 + 12 = 18\\) \\(14 - 5 = 9\\) \\(9 + 5 =...\nApproach: “What is the same about these number lines? What is different?” (You both have 3, 17, and 20. One has a long jump of 17, but the other has a small jump of 3. The difference between 17 and 20 is 3 and 20 is 17 away from 3.)",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will attend to the direction of the arrow on the number line to match addition and subtraction expressions. In previous lessons, you learned that whole numbers can be represented by tick marks and points on the number line."
    ];
    private static readonly string[] CorrectedFields=[
        "IM 2021 Grade2 Unit4 Lesson7 Addition and Subtraction on the Number Line. Warm-up Notice and Wonder shows two authentic0..15 lines connecting8 and12 with opposite arrow directions; 8+4=12 moves RIGHT four equal units, 12−4=8 moves LEFT four. The mathematical START and LENGTH matter in matching an equation, even when commutativity gives the same sum. Original Activity1 displays four numbered diagrams and two candidate equations each: arrow10→15 represents10+5=15; 13→5 represents13−8=5;12→2 represents12−10=2;4→14 represents4+10=14, not10+4=14 because actual start4. Students must view genuine publisher images with a teacher to justify the arrow's direction and endpoints. Activity2 original matching asks students to cut and glue eight equation strips by lettered line A–H, with ONE original unmatched equation that pupils draw on a blank number line; original line details include A6→18, B10→3,C9→14,D12→18,E3→20,F3→10,G20→17 and H blank. Teacher checks original image matches and actual paper cuts; independently authored digital computations are not substituted for visual grading. Original restricted Cool-down not copied.",
        "On a number line with unit-spaced integers, an arrow has a START, a FINISH and a DIRECTION. Moving right from a to b means ADD b−a, and moving left means SUBTRACT a−b. A length from zero is a coordinate; the arrow length is the count of unit intervals between two endpoints. Two equivalent sums, such as4+10 and10+4, are not always interchangeable diagram descriptions because first arrow starts at4 and jumps10, whereas the other starts10 and jumps4. Source examples8→12 gives8+4=12;12→8 gives12−4=8. A diagram of 13→5 gives13−8=5 (NOT8+5=13 as an arrow description starting at8). Unmatched original equation is teacher-reviewed in source's actual cut/glue image and cannot be selected solely from a generic list.",
        "Source Activity1 arrow10→15 means10+5=15,13→5 means13−8=5,12→2 means12−10=2, and4→14 means4+10=14; the incorrect same-result candidate10+4=14 starts at a different position. Source Activity2 uses REAL A–H line pictures and cut/paste equations; letters and the published equation strips have one unmatched example. Independently authored illustration: point7 to16 travels9 spaces to the right,7+9=16; another arrow16 to7 travels9 left,16−9=7. These invented cases are not publisher source diagrams and do not establish student drawing.",
        "1. Review real source warm-up two0..15 diagrams with partner: compare arrow8→12 and12→8, unit length4 and plus-versus-minus. 2. For original four Activity1 question diagrams, read arrow START and END from true image, compute absolute distance and check DIRECTION; source answers10+5=15,13−8=5,12−10=2,4+10=14. 3. Explain why10+4=14 is arithmetically true but not the pictured4→14 arrow. 4. In actual Activity2, use authentic source A–H charts plus eight publisher equations, cut/paste each only where start, jump and direction match; place the one unmatched equation into extra box and draw its own line. 5. Discuss partner similarities, differences and teacher assess true cut/glue work including H. 6. Solve distinctly labelled Edulytics generated start/distance/end examples; no automatic certification of original publisher images or Cool-down.",
        "Mistaking the same arithmetic total for matching an arrow whose START is different; confusing5→13 with13→5. Treating right-pointing arrow as subtraction and left-pointing as addition. Using endpoint number as the moved distance (13 to5 moves eight, not five). Assuming original H lettered blank source has fixed printed arrow; failing to draw an unmatched equation. Fabricating exact source diagram matches without looking at original A–H images. Claiming peer cut-and-glue evidence from online arithmetic.",
        "Source IM2021 G2U04L07: authentic arrows8↔12 show +/-4; activity1 exact arrows10→15,13→5,12→2,4→14; match eight equation strips to actual A–H diagrams, one unmatched to draw; teacher must examine image and physical worksheet. Independent authored numeric arrow practice separate."
    ];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==LessonCode;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=SourceUrl||
           !string.Equals(l.CanonicalBodySha256,SourceSha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Outcomes))
            throw new InvalidOperationException("G2 2.4.7 original source identity mismatch");
    }
    private static bool Equal(string[] a,bool updated)=>a.SequenceEqual(updated?CorrectedFields:OriginalFields);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool updated)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    private static bool Same(CurriculumLessonContentTranslation t,bool updated)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.7 learner prose predecessor changed");
            t.Explanation=CorrectedFields[0];t.KeyConceptsAndRules=CorrectedFields[1];
            t.WorkedExamples=CorrectedFields[2];t.StepByStepSolutions=CorrectedFields[3];
            t.CommonMistakes=CorrectedFields[4];t.QuickSummary=CorrectedFields[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> stored)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 2.4.7 unapproved persisted predecessor");
        if(stored.Count==0)return;
        if(stored.Count!=1||stored.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.7 persisted language rows drift");
        var previous=stored.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(previous.Title!=incoming.Title||!(Same(previous,false)||Same(previous,true)))
            throw new InvalidOperationException("G2 2.4.7 original stored fields drift");
    }
}

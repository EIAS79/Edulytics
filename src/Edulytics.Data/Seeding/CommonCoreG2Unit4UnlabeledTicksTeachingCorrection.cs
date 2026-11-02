using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>IM2021 G2 U04 L03: original numbered axes and figure-only evidence remain source SHA guarded.</summary>
public static class CommonCoreG2Unit4UnlabeledTicksTeachingCorrection
{
    public const string Version="usccss-g2-u04-l03-original-unlabeled-ticks-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L03";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-3/preparation.html";
    private const string Sha="f52cfba81e09474a951333db5d4a62d83c3145c74d8dd7c5ecbcaadb97bcdb99";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will represent numbers within 100 on number lines that do not label each tick mark.\nIn a previous lesson, you were introduced to the number line and represented the location of numbers with labeled tick marks and points up to 20. In this lesson, you use multiples of 5 and 10 to locate numbers up to 100 on the number line. You leverage your understanding of skip counting by 5 and 10 to locate numbers and build on your understanding of the number line as a representation that includes all numbers.",
         "Key ideas:\n- Represent a whole number on a number line and describe the point in terms of its length from 0\n- Use skip-counting patterns to locate numbers on a number line\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
         "Example 1: Locate 24 on the number line. Mark it with a point. Locate 37 on the number line. Mark it with a point. Locate 48 on the number line. Mark it with a point. Locate 83 on the number line. Mark it with a point. Explain how you know the numbers you located are at the right spot on each number line.\nApproach: “How do you know your point is the right distance from 0?” (It’s like when we measured from different spots on a ruler. The tick marks are spaced the same space apart and count by 1 length unit. We could draw the line back and find where 0 is.)\nExample 2: Complete each number line by filling in the labels with the number the tick mark represents. Then, locate each number, mark it with a point, and label it with the number it represents. Locate and label 17 on the number line. Description: Number line.&nbsp;31 evenly spaced tick marks. First tick mark labeled 10, sixth mark labeled blank, eleventh mark...\nApproach: “How could you find 35 without filling in the missing labels?” (We could count back from 50. We could count on from 10.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will represent numbers within 100 on number lines that do not label each tick mark. In a previous lesson, you were introduced to the number line and represented the location of numbers with labeled tick marks and points up to 20."],
        ["IM K–5 2021 Grade 2 Unit 4 Lesson 3 Unlabeled Tick Marks. Lesson goal: represent whole numbers to 100 when only some ticks have printed labels, using equal one-unit intervals and multiples of5 or10 as landmarks. Warm-up Notice and Wonder compares THREE separate 0..30 number lines: one advanced in one-unit steps, a second with six five-unit hops, and a third with three ten-unit hops. A large skipped LABEL does not remove the whole numbers between. Activity 1 Locate Numbers on a Number Line: original teacher asks pupils to mark24 on a 0..30 axis labelled in multiples of5, 37 on0..40 labelled by10, 48 on20..60 labelled by10, and83 on60..90 labelled by5. Point24 is one unit before25;37 three after34 or three before40;48 two before50;83 three after80. Position comparisons are based on genuine distance from0, even if the drawing is cropped to begin20 or60. Original Activity 2 Number Lines With Missing Numbers: investigate FOUR publisher number-line FIGURES for17,59,43 and35, add any needed labels, draw points and explain. The textual source identifies a 31-tick line from10 to40 with some printed25/30, used to locate17; one graphic from35..55 showing landmarks for43; the complete source images for59 and35 cannot be accurately reconstructed from available text, so teacher reviews those ORIGINAL figures. One-unit gaps must remain equal, and pupils use nearby printed landmark numbers to determine positions without claiming an unseen source drawing was verified. Classroom points, drawn spacing, genuine diagrams and partner MLR2 explanations require the teacher. Original restricted Cool-down is not copied; Edulytics arithmetic practice is independently authored.",
         "On any WHOLE-NUMBER line, a point with coordinate n represents n equal unit lengths counted from0. Number lines can show a cropped window beginning at20 or60 without changing the fact that displayed value83 is eighty-three units from the unshown zero. The distance between printed labels5 and10 is FIVE one-unit intervals; between30 and40 it is TEN. When not every tick label is printed, omitted labels still describe real integer positions. To find24 on a0..30 line, start from landmark25 and go one 1-unit tick LEFT. For37 from landmark40 go three LEFT; for48 from50 go two LEFT; for83 from80 go three RIGHT. Teacher should check original mark density before describing its specific picture; the concept remains genuine numeric unit distance. A figure from10 to40 has 31 one-unit ticks and30 intervals. From printed15, number17 lies two units right; from printed40, number35 lies five units left. A 35..55 window has21 integer tick positions, and43 is three right of40. Depending on the particular SOURCE drawing, visible marks may show only landmarks spaced5 or10: never substitute one large landmark gap for a single unit tick. CCSS 2.MD.B.6 and 2.NBT.A.2.",
         "PUBLIC source Activity1 original point targets are24,37,48,83 on the distinct teacher-drawn lines:24=25−1 (0..30, fives printed),37=40−3 (0..40, tens printed),48=50−2 (20..60, tens printed),83=80+3 (60..90, fives printed). These relationships express numerical coordinates and not fabricated pixel positions. Activity2 source target17 lies two units beyond15 on a10..40 line: first value10, sixteenth printed25, twenty-first30 and blank marks at15,20,35,40. Source target43 on35..55 is three greater than printed40; original figure also requires teacher for exact visible marks. Targets59 and35 are SOURCE questions but underlying images are not completely present in archived readable text, so do not infer their graphical marks. FIRST-PARTY independently authored practice examples: on a newly generated 40..70 uniformly marked line, place53 three after50; a point at68 is two before70; invented points at24 and31 differ7 unit intervals; and a visible 30..50 window has21 one-unit ticks. These are NOT original publisher Cool-down or original pictures.",
         "1. During real Notice and Wonder, view the three authentic0..30 publisher diagrams and explain the relationship between1,5 and10 sized jumps:0 to30 can be covered by30 unit moves, six five-moves, or three ten-moves. Teacher checks actual student observations. 2. Locate original source24 by identifying printed25 and moving ONE unit left; label exactly24 at the corresponding point if the original publisher one-unit marks are visible. 3. Use printed tens to locate37 three units left of40 and48 two left of50, and printed fives to locate83 three right of80; compare the displayed window with the length-from-zero rule even if0 is outside the image. 4. For source Activity2 work with teacher-visible ORIGINAL diagrams:17 on a10..40 line is two after15; 43 on35..55 is three after40. Mark source59 and35 on their actual provided figures, not on invented replacement pictures, and explain the nearby anchor printed numbers chosen. 5. Check each unit step has the intended actual equal distance when interpreting a line; a gap between labels e.g.40 and50 may contain ten one-unit positions. 6. Discuss with a peer how to locate a value without labelling every intermediate tick; educator reviews original-source pictures, dot placements and MLR2 discourse. 7. Independently solve Edulytics created numeric anchor and unit-gap questions. The online arithmetic does not certify physical accuracy nor reconstruct the restricted original Cool-down.",
         "Common mistakes: interpreting missing labels as missing numbers, so counting from30 straight to40 in one unit; confusing a skipped label interval of10 with one actual one-unit space. Labeling a point exactly ON40 as37 or forgetting the direction when going back from50 to48. Treating an axis displaying60 at its left edge as if that position means0; true integer coordinates retain length-from-zero meaning. Assuming the arithmetic fact83=80+3 proves a dot was marked exactly on the actual publisher source image. Using a copied imagined layout for original question59 or35 without access to their original figures. Counting31 ticks on the original10..40 line as31 intervals instead of30. Applying a fixed 5-unit jump to every visible tiny tick when equal one-unit intermediate ticks are intended. Writing answers for restricted publisher assessment that has not been accessed.",
         "Original IM2021 G2 U04 L03 source 2.4.3: compare three ways to show0..30 with jumps1/5/10, then mark24,37,48,83 on sparse-labeled real source number lines, and mark17,59,43,35 on four other separate original student diagrams (some graphics image-only). A whole number retains its unit-distance from0 even when displayed cropped and labels omitted. Teacher checks original graphics, dot positions and pair reasoning; independently invented Edulytics numeric checks are separate and not the original Cool-down."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.6","CCSS:2.NBT.A.2"}))
            throw new InvalidOperationException("G2 U04 L03 source SHA or official standards drift");
    }
    private static bool Match(string[] fields,bool after)=>fields.SequenceEqual(after?S.After:S.Before);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U04 L03 six predecessor learner fields drift");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)?Version:prev;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))
            throw new InvalidOperationException("G2 U04 L03 unknown persisted predecessor");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U04 L03 persisted content language drift");
        var oldText=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(oldText.Title!=incoming.Title||!(Same(oldText,false)||Same(oldText,true)))
            throw new InvalidOperationException("G2 U04 L03 previous learner text drift");
    }
}

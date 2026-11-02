using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>IM2021 G2 U04 L01 number lines: exact source SHA and six-field predecessor guard.</summary>
public static class CommonCoreG2Unit4NumberLineTeachingCorrection
{
    public const string Version="usccss-g2-u04-l01-original-number-line-0-20-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L01";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-1/preparation.html";
    private const string Sha="b9bf1f4c584581c4eda97feb0f144dcf4d2d8d07fafa0cb35e86673a88a0e120";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S = new(
        ["Learning goal: You will understand the structure of the number line and learn how whole numbers can be represented on the number line with tick marks and points.\nIn a previous unit, you learned about standard length units. You created your own rulers by iterating centimeter cubes and used your understanding of length units to measure the length of objects with rulers, meter sticks, and other tools. You interpreted the scale of line plots and created your own line plots by relating your structure to the length units on measurement tools.",
         "Key ideas:\n- Locate whole numbers on a number line\n- Make sense of the structure of a number line\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Label each tick mark with the number it represents. Locate 6 on the number line. Mark it with a point. Locate a number on the number line that is greater than 6. Mark it with a point. Label each tick mark with the number it represents. Locate 9 on the number line. Mark it with a point. Locate a number on the number line that is less than 9. Mark it with...\nApproach: “How did you know which numbers to use to label the tick marks?” (It’s like a ruler, I figured out how many length units each tick mark was from 0. I just counted because I know the numbers need to go in order from left to right.)\nExample 2: Make a number line that goes from 0 to 20. Locate 13 on your number line. Mark it with a point. Locate 3 on your number line. Mark it with a point. Compare your number line with your partner’s.\nApproach: “What is the same and what is different about these representations?” (You all have equally spaced tick marks. You all show 0 to 20 in order. You all show a point on 3 and 13. The length of the space you used between tick marks is different. You used different objects to create your number lines.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will understand the structure of the number line and learn how whole numbers can be represented on the number line with tick marks and points. In a previous unit, you learned about standard length units."],
        ["IM K–5 2021 Grade 2 Unit 4 Lesson 1 Whole Numbers on the Number Line. A number line represents WHOLE NUMBERS as lengths counted from ZERO using equally spaced marks. Warm-up Notice and Wonder: observe the real publisher number-line diagram and a ruler labelled 0 through 12 by ones; explain that both have evenly spaced ticks and ascending whole numbers, but the number line need not use centimeters or inches. Activity 1 What Is a Number Line?: the FIRST original 0–10 line has visible labels 0,1,2,5,8,9,10. Complete the unlabeled ticks at 3,4,6,7, mark a POINT directly over 6, and choose ONE valid number GREATER than 6 for a second point (7,8,9 or10). On the SECOND original 0–10 line only 0,5,10 are labeled: fill every intermediate value, mark a point over 9, and a second point LESS than9 (any0 through8). Student choices above/below are not one fixed publisher answer. Activity 2 Make Your Own Number Line: on a REAL paper strip 24–30 inches long, iterate ONE pupil-chosen physical object as an identical unit interval to construct ticks from0 to20; put points at13 and3, exchange and compare drawings with a partner. Every INTERVAL on one particular line must have the same length, though two students can choose different physical lengths for their unit. The original Cool-down is login-restricted: Edulytics supplies its own independent numeric questions, not the original assessment. Drawing quality, notice/wonder and partner dialogue require a real educator.",
         "Number lines and rulers share regular, equally spaced marks labelled in ascending order from left to right. The zero tick is the anchor: the point at n is n equal unit intervals RIGHT of zero, not n tick marks if counting the zero tick as one. A point is a dot DIRECTLY above its number/tick, rather than between labels. Distinct consecutive whole numbers differ by EXACTLY one unit interval. On a 0–10 number line, positions are0,1,2,3,4,5,6,7,8,9,10; gaps in the printed labels do not remove ticks. The original first line has missing labels 3,4,6,7; the second includes only 0,5,10 and still has all intervening unit marks. To mark a number greater than6 on 0–10, choose7,8,9,or10, and less than9 choose0..8. On pupils' independently made 0–20 paper lines, 3 is three units from zero, 13 thirteen units, so positions differ by ten UNIT INTERVALS. Unlike a centimeter ruler, the unit interval on a NUMBER LINE may be any consistent convenient length. Exactly-equal intervals within each student's line are essential, not shared paper measurements. Outcome: CCSS 2.MD.B.6.",
         "PUBLIC source activity examples, teacher paraphrase: Given an eleven-tick line 0–10 with labels0,1,2,5,8,9,10, write the four omitted labels3,4,6,7; a point over6 means6 units from0, and a chosen point over8 (ONE possible answer) is greater than6 because8>6. A different eleven-tick line shows only0,5,10: its other labels are1–4 and6–9; point at9 and a chosen4 below9 are valid EXAMPLES, not fixed answers for the open-choice question. Source make-your-own model: use a single paper clip or another physical unit repeatedly to mark21 ticks0–20 on paper; mark point at3 and another at13. Edulytics first-party numeric example, not a copy of the restricted Cool-down: two invented dots at5 and12 on a uniformly spaced line have12−5=7 unit intervals between them. Another first-party example: counting eight equal hops right from0 lands at8. Never claim a text answer proves a physical drawing uses equal spacings.",
         "1. Find the left-hand mark0 and check numbers increase by one at each equally spaced neighboring tick. 2. On the source 0–10 diagram, enumerate all tick labels in order0,1,2,3,4,5,6,7,8,9,10 even where text labels were omitted. Source first missing positions3,4,6,7; source second printed labels0,5,10 only, so add every intervening number. 3. Place one dot directly over6 on the first line. Place ANOTHER dot over any valid7–10; justify that larger numbers lie to the RIGHT. 4. On the second line place a dot over9 and another over any valid0–8, to the LEFT. Open choices can have multiple correct answers. 5. Construct a REAL paper number line from0 through20: select a paper clip or other object as a repeatable unit, anchor zero, make20 equal intervals without gaps or overlaps, label21 ticks, put dots at3 and13. 6. Compare student lines with a partner: overall distances may differ, but each one's tick intervals are uniform and its dots represent3 and13 units from its OWN zero. Teacher checks physical construction and oral reasoning. 7. Independently check Edulytics new numerical point/interval questions with whole-number arithmetic; do not present those digital answers as the source's restricted publisher Cool-down or as proof of physical accuracy.",
         "Common mistakes: counting tick0 as the FIRST hop makes a point one mark too far left; each equal space, not each printed digit, represents1 unit. Treating only0,5,10 labels as three tick marks and omitting the other intermediate positions. Putting a dot between two ticks, confusing a point's height with its horizontal numeric value, or declaring a chosen point at6 is GREATER than6 (it is equal). Misreading greater-than6 to allow positions greater than10 on a source line ending at10. Requiring both pupil-made lines to use the SAME real-world length: unit lengths can differ BETWEEN drawings but must be equal WITHIN each. Unequal gaps, shifted start zero, overlapping repeated objects, and treating number lines as automatically centimeter rulers are errors. Instructors must view the real original publisher warm-up image before certifying diagram-specific answers; no protected cool-down is reconstructed.",
         "Source-aligned lesson 2.4.1: read and complete two equally spaced 0–10 number lines; fill first missing3,4,6,7 and all missing labels on second 0,5,10-anchored one, show point6 and another greater than6, then point9 and another less than9. Make a REAL individual 0–20 line with one chosen constant length unit, points3 and13, and partner compare. All number labels represent unit counts FROM ZERO. Online Edulytics questions are separately authored checks, not real teacher-reviewed drawing or publisher assessment."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson)=>
        document.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        document.VersionCode=="CCSSM-2010" &&
        document.ContentVersion=="phase30-common-core-rich-content-v1" &&
        lesson.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson)
    {
        if(!IsTarget(document,lesson)||lesson.SourceUrl!=Url||
           !string.Equals(lesson.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.6"}))
            throw new InvalidOperationException("G2 U04 L01 publisher source or outcome identity drift");
    }
    private static bool Matches(string[] fields,bool after)=>fields.SequenceEqual(after?S.After:S.Before);
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
            var t=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U04 L01 six predecessor fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(document,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(document,lesson)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument document,CanonicalLessonContentPackLesson lesson,
        string predecessor,IReadOnlyCollection<CurriculumLessonContentTranslation> stored)
    {
        Guard(document,lesson);
        if(!CanUpgradeExisting(document,lesson,predecessor))
            throw new InvalidOperationException("G2 U04 L01 persisted predecessor version unknown");
        if(stored.Count==0)return;
        if(stored.Count!=1||stored.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U04 L01 persisted language drift");
        var previous=stored.Single();
        var incoming=lesson.Translations.Single(x=>x.CultureCode=="en");
        if(previous.Title!=incoming.Title||!(Same(previous,false)||Same(previous,true)))
            throw new InvalidOperationException("G2 U04 L01 persisted learner fields changed");
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L14.</summary>
public static class CommonCoreG2Unit4JumpTheLineCenterCorrection
{
    public const string Version="usccss-g2-u04-l14-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L14";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-14/preparation.html";
    private const string Sha="baa4248104aaba69cf51caf7d8bf676cff352c19b3908c06747b923c58231772";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5"];
    private static readonly string[] Before=["Learning goal: You will practice adding and subtracting within 100 and representing addition and subtraction on the number line.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In Activity 1, you learn stage 1 of the Jump the Line center. In this new center, you take turns making strategic choices about numbers to add or subtract to reach target numbers.",
        "Key ideas:\n- Practice addition and subtraction within 100\n- Represent addition and subtraction on the number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(28 + 2 + 20 = 50\\) \\(16 + 3 + 30 = 50\\) \\(36 + 4 + 20 = 50\\)\nApproach: “How can you explain your answer to the last statement without finding the sum of all three addends?” (I could add 36 and 20 and see that it’s more than 56. I could add 36 and 4 to get 40. I know I would need 10 more to get to 50 and I see more than 10 left to add.)",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will practice adding and subtracting within 100 and representing addition and subtraction on the number line. This lesson is optional because it is an opportunity for extra practice that not all classes may need."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson14 Center Day 2 (optional). This is OPTIONAL original IM2021 center day. True/False warm-up:28+2+20=50 TRUE;16+3+30=50 FALSE (49);36+4+20=50 FALSE (60). Real Stage1 Jump the Line: group2 picks THREE target numbers on original gameboard, both start at beginning, spin all THREE authentic spinners and strategically SELECT one move (plus or minus), aim to land EXACTLY on targets; first to land on TWO targets wins. Do not invent publisher spinner contents, scale, physical outcomes, game scores or target locations. Activity2 genuine choice: Jump the Line1, Number Line Scoot1, or How Close3. Teacher-only physical gameplay and choice. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-14/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is optional-center-target-game. This is OPTIONAL original IM2021 center day. True/False warm-up:28+2+20=50 TRUE;16+3+30=50 FALSE (49);36+4+20=50 FALSE (60). Real Stage1 Jump the Line: group2 picks THREE target numbers on original gameboard, both start at beginning, spin all THREE authentic spinners and strategically SELECT one move (plus or minus), aim to land EXACTLY on targets; first to land on TWO targets wins. Do not invent publisher spinner contents, scale, physical outcomes, game scores or target locations. Activity2 genuine choice: Jump the Line1, Number Line Scoot1, or How Close3. Teacher-only physical gameplay and choice. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Optional physical-game rule: ONLY genuine exact target landing counts; exact authentic spinner values must be observed.",
        "Original SOURCE expressions/context examples (not invented image locations): 28+2+20=50; 16+3+30=49; 36+4+20=60; FICTIONAL marker16+4→20. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: optional-center-target-game. 3. On authentic physical pupil work interpret correctly the original source activity: This is OPTIONAL original IM2021 center day. True/False warm-up:28+2+20=50 TRUE;16+3+30=50 FALSE (49);36+4+20=50 FALSE (60). Real Stage1 Jump the Line: group2 picks THREE target numbers on original gameboard, both start at beginning, spin all THREE authentic spinners and strategically SELECT one move (plus or minus), aim to land EXACTLY on targets; first to land on TWO targets wins. Do not invent publisher spinner contents, scale, physical outcomes, game scores or target locations. Activity2 genuine choice: Jump the Line1, Number Line Scoot1, or How Close3. Teacher-only physical gameplay and choice. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for optional-center-target-game: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: The authentic gameboard, all three spinner readings, strategic move selection, two-target win and actual center choices cannot be auto-certified.",
        "IM2021 Grade2 Unit4 Lesson14 original topic Center Day 2 (optional). Accurate published examples: 28+2+20=50, 16+3+30=49, 36+4+20=60, FICTIONAL marker16+4→20. First-party numeric practice is independent. The authentic gameboard, all three spinner readings, strategic move selection, two-target win and actual center choices cannot be auto-certified."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L14 original lesson source identity drift");
    }
    private static bool Same(string[] values,bool updated)=>values.SequenceEqual(updated?After:Before);
    private static bool Has(CanonicalLessonContentPackTranslation t,bool updated)=>
       Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    private static bool Has(CurriculumLessonContentTranslation t,bool updated)=>
       Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
       foreach(var l in d.Lessons)
       {
           if(!IsTarget(d,l))continue;Check(d,l);
           var t=l.Translations.Single(x=>x.CultureCode=="en");
           if(Has(t,true))continue;
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L14 source predecessor fields mismatch");
           t.Explanation=After[0];t.KeyConceptsAndRules=After[1];t.WorkedExamples=After[2];
           t.StepByStepSolutions=After[3];t.CommonMistakes=After[4];t.QuickSummary=After[5];
       }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
       IsTarget(d,l)?Version:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
       IsTarget(d,l)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior,
       IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
       Check(d,l);
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L14 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L14 saved language drift");
       var old=persisted.Single(),incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L14 saved prose is not an approved predecessor");
    }
}

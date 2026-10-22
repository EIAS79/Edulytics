using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L12.</summary>
public static class CommonCoreG2Unit4UnknownArrowRiddlesCorrection
{
    public const string Version="usccss-g2-u04-l12-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L12";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-12/preparation.html";
    private const string Sha="83eedaf9016fa030f05ec35783ce909c8f204cd669679045f618d42d9295b202";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5","CCSS:2.OA.A.1"];
    private static readonly string[] Before=["Learning goal: You will represent equations with an unknown in all positions on the number line and find the unknown values.\nIn previous lessons, you found the value of sums and differences within 100, represented your thinking on the number line, and compared different methods. In this lesson, you find the number that makes addition and subtraction equations true within 100 in equations with unknowns in all positions. You continue to think about the relationships between the numbers in each equation and reason about how you can use the structure of the number line, the relationship between addition and subtraction, and your understanding of place value to find the number that makes each equation true (MP7).",
        "Key ideas:\n- Write equations and represent sums and differences on a number line\n- In previous lessons, you found the value of sums and differences within 100, represented your thinking on the number line, and compared different methods\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(40 = 10 + 27 + 3\\) \\(47 = 20 + 7 + 3 + 10\\) \\(60 = 3 + 47 + 10\\)\nApproach: “How could we change the second equation to make it true?” (We could change the 10 to 17 or the 20 to 27 because we need 7 more.)\nExample 2: Solve riddles to find the mystery number. For each riddle: Write an equation that represents the riddle and write a ? for the unknown. Write the mystery number. Represent the equation on the number line. I started at 15 and jumped 17 to the right. Where did I end? Equation: ___ Mystery number: ___ I started at a number and jumped 20 to the left. I ended...\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.",
        "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
        "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
        "You will represent equations with an unknown in all positions on the number line and find the unknown values. In previous lessons, you found the value of sums and differences within 100, represented your thinking on the number line, and compared different methods."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson12 Equations with Unknowns. Original True/False warm-up: 40=10+27+3 TRUE;47=20+7+3+10 FALSE (right side40);60=3+47+10 TRUE. Original Activity1 unknown in every position. Source riddle: start15 jump right17 end32; unknown start and jump20 LEFT end33 gives start53; start42 end80 jump38;76 jump27 LEFT end49; unknown start jump19 RIGHT end67 gives start48. Source uses '?' in equations, honest student working on genuine 0..100 number lines, not just answer key. Teacher checks diagrams and reasoned inverse relationship. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-12/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is unknown-position-number-line. Original True/False warm-up: 40=10+27+3 TRUE;47=20+7+3+10 FALSE (right side40);60=3+47+10 TRUE. Original Activity1 unknown in every position. Source riddle: start15 jump right17 end32; unknown start and jump20 LEFT end33 gives start53; start42 end80 jump38;76 jump27 LEFT end49; unknown start jump19 RIGHT end67 gives start48. Source uses '?' in equations, honest student working on genuine 0..100 number lines, not just answer key. Teacher checks diagrams and reasoned inverse relationship. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Use place-value tens/ones and true inverse relationships without inventing source-only diagram features.",
        "Original SOURCE expressions/context examples (not invented image locations): 15+17=32; ?−20=33→53; 42+?=80→38; 76−27=49; ?+19=67→48. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: unknown-position-number-line. 3. On authentic physical pupil work interpret correctly the original source activity: Original True/False warm-up: 40=10+27+3 TRUE;47=20+7+3+10 FALSE (right side40);60=3+47+10 TRUE. Original Activity1 unknown in every position. Source riddle: start15 jump right17 end32; unknown start and jump20 LEFT end33 gives start53; start42 end80 jump38;76 jump27 LEFT end49; unknown start jump19 RIGHT end67 gives start48. Source uses '?' in equations, honest student working on genuine 0..100 number lines, not just answer key. Teacher checks diagrams and reasoned inverse relationship. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for unknown-position-number-line: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: Actual riddle number-line jumps, mathematical equations with unknown marked ?, source partner modelling observed by teacher.",
        "IM2021 Grade2 Unit4 Lesson12 original topic Equations with Unknowns. Accurate published examples: 15+17=32, ?−20=33→53, 42+?=80→38, 76−27=49, ?+19=67→48. First-party numeric practice is independent. Actual riddle number-line jumps, mathematical equations with unknown marked ?, source partner modelling observed by teacher."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L12 original lesson source identity drift");
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
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L12 source predecessor fields mismatch");
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
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L12 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L12 saved language drift");
       var old=persisted.Single(),incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L12 saved prose is not an approved predecessor");
    }
}

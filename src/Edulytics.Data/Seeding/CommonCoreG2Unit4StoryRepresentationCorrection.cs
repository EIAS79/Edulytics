using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L13.</summary>
public static class CommonCoreG2Unit4StoryRepresentationCorrection
{
    public const string Version="usccss-g2-u04-l13-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L13";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-13/preparation.html";
    private const string Sha="e91a93b2c1693784ad98771db75b788883f10b679cad4ce9419f55d4728a573a";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5","CCSS:2.OA.A.1"];
    private static readonly string[] Before=["Learning goal: You will make connections between different representations of situations for sums and differences.\nIn previous lessons, you were introduced to the number line. You learned that numbers are located at points on the line, depending on your distance from zero. You represented addition and subtraction equations with unknowns in all positions on number lines.",
        "Key ideas:\n- Represent addition and subtraction story problems using equations, tape diagrams, and number lines\n- In previous lessons, you were introduced to the number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Solve each problem. Show your thinking. Use a number line or diagram if it helps. Clare started with 24 cubes and added on some more. Clare made a train with 42 cubes. How many cubes did Clare add on? Andre had 37 cubes. Then he added 39 more to make the train longer. How many cubes did Andre use? Mai wanted her train to be 55 cubes long. So far she has...\nApproach: “How are these representations of the same problem the same? How are you different?” (I can see the whole length of the train in the diagram, but I only see a point at 37 on the number line and I don’t see anything but the numbers in the equation. You all show 37 as one part and you all show addition.)",
        "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
        "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
        "You will make connections between different representations of situations for sums and differences. In previous lessons, you were introduced to the number line."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson13 Represent Story Problems. Source Notice/Wonder compares authentic0..10 diagram arrow5→9 length4 and a tape diagram showing 5+4=9. Activity1 original 16 printed cards A–P combine STORY, EQUATION, NUMBER LINE and TAPE DIAGRAM, matched in genuine groups3; teacher sees the actual publisher graphics rather than fabricated card letters or group outcomes. Activity2 class cube-train context: Clare begins24 cubes, ends42, added18; Andre starts37 adds39 =>76; Mai targets55 cubes with partial starting train that must be read from the actual source. Numerical practice is original independently-authored unless original text explicitly provides values. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-13/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is story-numberline-tape. Source Notice/Wonder compares authentic0..10 diagram arrow5→9 length4 and a tape diagram showing 5+4=9. Activity1 original 16 printed cards A–P combine STORY, EQUATION, NUMBER LINE and TAPE DIAGRAM, matched in genuine groups3; teacher sees the actual publisher graphics rather than fabricated card letters or group outcomes. Activity2 class cube-train context: Clare begins24 cubes, ends42, added18; Andre starts37 adds39 =>76; Mai targets55 cubes with partial starting train that must be read from the actual source. Numerical practice is original independently-authored unless original text explicitly provides values. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Use place-value tens/ones and true inverse relationships without inventing source-only diagram features.",
        "Original SOURCE expressions/context examples (not invented image locations): 5+4=9; 42−24=18; 37+39=76. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: story-numberline-tape. 3. On authentic physical pupil work interpret correctly the original source activity: Source Notice/Wonder compares authentic0..10 diagram arrow5→9 length4 and a tape diagram showing 5+4=9. Activity1 original 16 printed cards A–P combine STORY, EQUATION, NUMBER LINE and TAPE DIAGRAM, matched in genuine groups3; teacher sees the actual publisher graphics rather than fabricated card letters or group outcomes. Activity2 class cube-train context: Clare begins24 cubes, ends42, added18; Andre starts37 adds39 =>76; Mai targets55 cubes with partial starting train that must be read from the actual source. Numerical practice is original independently-authored unless original text explicitly provides values. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for story-numberline-tape: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: Actual A–P publisher card-sort artwork, original cube train drawing, correct tape diagram and student MLR8 discourse require human teacher.",
        "IM2021 Grade2 Unit4 Lesson13 original topic Represent Story Problems. Accurate published examples: 5+4=9, 42−24=18, 37+39=76. First-party numeric practice is independent. Actual A–P publisher card-sort artwork, original cube train drawing, correct tape diagram and student MLR8 discourse require human teacher."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L13 original lesson source identity drift");
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
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L13 source predecessor fields mismatch");
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
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L13 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L13 saved language drift");
       var old=persisted.Single(),incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L13 saved prose is not an approved predecessor");
    }
}

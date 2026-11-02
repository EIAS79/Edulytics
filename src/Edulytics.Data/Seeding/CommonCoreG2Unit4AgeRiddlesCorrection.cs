using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L15.</summary>
public static class CommonCoreG2Unit4AgeRiddlesCorrection
{
    public const string Version="usccss-g2-u04-l15-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L15";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-15/preparation.html";
    private const string Sha="f1d65383452dcb2bed10a548a8626118ce9d23db843f8770e6f602cc68102d74";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5","CCSS:2.OA.A.1"];
    private static readonly string[] Before=["Learning goal: You will generate your own addition and subtraction stories, and use the number line as a tool to write and solve story problems.\nThis lesson is optional because it does not address any new mathematical content standards. This lesson does provide you with an opportunity to apply precursor skills of mathematical modeling. In previous lessons, you represented addition and subtraction on the number line and considered different methods based on the numbers being added and subtracted.",
        "Key ideas:\n- Solve addition and subtraction problems to investigate in context\n- Write addition and subtraction story problems\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Solve Kiran’s age riddles. Show your thinking. Use a number line if it helps. I’m 7. My sister is 5 years older than I am. How old is she? ___ years old If you add 27 years to my sister’s age, you get my mom’s age. How old is my mom? ___ years old My brother is 24 years younger than my mom. How old is my brother? ___ years old My grandma is 53 years...\nApproach: “How did you know which strategy to use when you read a riddle?” (I thought about if the person was older or younger than the person before. That helped me decide if I should count on or count backwards.) “If Kiran’s sister and brother have your birthday party together, how many candles do you need?” (27: \\(12 + 15\\) ) “How much older is Kiran’s grandma than his grandpa?” (4: \\(68-64\\) ) “How many years ago was...\nExample 2: Complete the table by writing down 4 people you know with different ages. person's first name age (years) Come up with 3 mathematical questions to ask your partner. Question 1: Question 2: Question 3: Solve your partner's riddles. Show your thinking using a number line or equation.\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will generate your own addition and subtraction stories, and use the number line as a tool to write and solve story problems. This lesson is optional because it does not address any new mathematical content standards."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson15 Riddles (optional). OPTIONAL source lesson, no new required mathematical standard. Source warm-up Notice/Wonder publisher birthday-candle photographs are image-only; teacher invites real observations, no exact candle count invented. Kiran's original linked age riddles: Kiran7, sister7+5=12, mother12+27=39, brother39−24=15, grandma15+53=68, uncle68−21=47, cousin47−33=14, grandpa age differs from cousin by50 and may require interpreting original context for direction rather than guessing. Follow the FULL dependency chain using a0..100 number line. Activity2 students write a table of FOUR actual people and their ages and THREE original partner riddles; personal responses cannot be invented and require teacher judgment. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-15/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is optional-age-riddle-model. OPTIONAL source lesson, no new required mathematical standard. Source warm-up Notice/Wonder publisher birthday-candle photographs are image-only; teacher invites real observations, no exact candle count invented. Kiran's original linked age riddles: Kiran7, sister7+5=12, mother12+27=39, brother39−24=15, grandma15+53=68, uncle68−21=47, cousin47−33=14, grandpa age differs from cousin by50 and may require interpreting original context for direction rather than guessing. Follow the FULL dependency chain using a0..100 number line. Activity2 students write a table of FOUR actual people and their ages and THREE original partner riddles; personal responses cannot be invented and require teacher judgment. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Optional personal-age-story task has genuine human choices; digital numbers do not establish real people's ages.",
        "Original SOURCE expressions/context examples (not invented image locations): 7+5=12; 12+27=39; 39−24=15; 15+53=68; 68−21=47; 47−33=14. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: optional-age-riddle-model. 3. On authentic physical pupil work interpret correctly the original source activity: OPTIONAL source lesson, no new required mathematical standard. Source warm-up Notice/Wonder publisher birthday-candle photographs are image-only; teacher invites real observations, no exact candle count invented. Kiran's original linked age riddles: Kiran7, sister7+5=12, mother12+27=39, brother39−24=15, grandma15+53=68, uncle68−21=47, cousin47−33=14, grandpa age differs from cousin by50 and may require interpreting original context for direction rather than guessing. Follow the FULL dependency chain using a0..100 number line. Activity2 students write a table of FOUR actual people and their ages and THREE original partner riddles; personal responses cannot be invented and require teacher judgment. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for optional-age-riddle-model: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: Real original birthday image observations, student-selected four real people's ages, own riddles and peer justification; never invent personal identities.",
        "IM2021 Grade2 Unit4 Lesson15 original topic Riddles (optional). Accurate published examples: 7+5=12, 12+27=39, 39−24=15, 15+53=68, 68−21=47, 47−33=14. First-party numeric practice is independent. Real original birthday image observations, student-selected four real people's ages, own riddles and peer justification; never invent personal identities."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L15 original lesson source identity drift");
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
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L15 source predecessor fields mismatch");
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
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L15 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L15 saved language drift");
       var old=persisted.Single(); var incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L15 saved prose is not an approved predecessor");
    }
}

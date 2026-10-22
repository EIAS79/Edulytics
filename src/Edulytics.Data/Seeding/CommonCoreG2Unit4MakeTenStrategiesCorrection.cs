using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L11.</summary>
public static class CommonCoreG2Unit4MakeTenStrategiesCorrection
{
    public const string Version="usccss-g2-u04-l11-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L11";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-11/preparation.html";
    private const string Sha="1ebf1543e4f042b5b77782c37c5379b33cd1a009041b4995a03e1f1015bad34c";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5"];
    private static readonly string[] Before=["Learning goal: You will represent sums and differences on the number line with an emphasis on the strategy of using a ten to count up or count back.\nIn previous lessons, you learned to look for ways to decompose a number in an expression to get to a ten when adding or subtracting numbers mentally in Number Talks. You have learned to use this method when adding or subtracting larger numbers which require composing or decomposing a ten. In this lesson, you use the number line to represent these methods.",
        "Key ideas:\n- On a number line, represent place value methods for solving addition and subtraction equations that may involve composing or decomposing a ten\n- In previous lessons, you learned to look for ways to decompose a number in an expression to get to a ten when adding or subtracting numbers mentally in Number Talks\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Find the value of each expression mentally. \\(32 - 6\\) \\(43 - 8\\) \\(51 - 5\\) \\(52 - 7\\)\nApproach: “For \\(52 - 7\\) some you decomposed the 7 to make it easier to get to a ten. How does this number line representation connect to that strategy?”\nExample 2: Diego and Tyler found the value of \\(53 - 29\\) . Their work is shown. Diego’s Method: Tyler’s Method: Use Diego’s method to find the value of \\(82 - 35\\) . Use Tyler’s method to find the value of \\(66-28\\) .\nApproach: “Is there another way we could use the number line to show a way to make it easier to find the value of \\(53-29\\) by getting to a ten?” If not suggested by you, ask: “How could we start with 29 and use a method like Diego or Tyler’s?” (Start with 29, jump over 1 to get to 30, then it's easy to see all you need to do is jump 23 to get to 53.)",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will represent sums and differences on the number line with an emphasis on the strategy of using a ten to count up or count back. In previous lessons, you learned to look for ways to decompose a number in an expression to get to a ten when adding or subtracting numbers mentally in Number Talks."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson11 Different Ways to Add and Subtract. Source Number Talk32−6=26,43−8=35,51−5=46,52−7=45. Original Diego and Tyler each solve53−29=24 with distinct strategies: tens-first versus ones-first decomposing to a ten. Source practise Diego's style82−35=47; Tyler's style66−28=38. Compare physical source 0..100 diagrams, discuss why reaching a multiple of10 can simplify a move. An independent numerical answer cannot certify which exact pictured method was used. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-11/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is reach-nearest-ten. Source Number Talk32−6=26,43−8=35,51−5=46,52−7=45. Original Diego and Tyler each solve53−29=24 with distinct strategies: tens-first versus ones-first decomposing to a ten. Source practise Diego's style82−35=47; Tyler's style66−28=38. Compare physical source 0..100 diagrams, discuss why reaching a multiple of10 can simplify a move. An independent numerical answer cannot certify which exact pictured method was used. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Use place-value tens/ones and true inverse relationships without inventing source-only diagram features.",
        "Original SOURCE expressions/context examples (not invented image locations): 32−6=26; 43−8=35; 51−5=46; 52−7=45; 53−29=24; 82−35=47; 66−28=38. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: reach-nearest-ten. 3. On authentic physical pupil work interpret correctly the original source activity: Source Number Talk32−6=26,43−8=35,51−5=46,52−7=45. Original Diego and Tyler each solve53−29=24 with distinct strategies: tens-first versus ones-first decomposing to a ten. Source practise Diego's style82−35=47; Tyler's style66−28=38. Compare physical source 0..100 diagrams, discuss why reaching a multiple of10 can simplify a move. An independent numerical answer cannot certify which exact pictured method was used. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for reach-nearest-ten: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: Teacher sees original Diego/Tyler number-line images, student tens/ones jumps and paired explanation.",
        "IM2021 Grade2 Unit4 Lesson11 original topic Different Ways to Add and Subtract. Accurate published examples: 32−6=26, 43−8=35, 51−5=46, 52−7=45, 53−29=24, 82−35=47, 66−28=38. First-party numeric practice is independent. Teacher sees original Diego/Tyler number-line images, student tens/ones jumps and paired explanation."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L11 original lesson source identity drift");
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
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L11 source predecessor fields mismatch");
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
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L11 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L11 saved language drift");
       var old=persisted.Single(),incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L11 saved prose is not an approved predecessor");
    }
}

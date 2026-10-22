using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Exact source, CCSS, prior learner content and persisted predecessor guard for G2 U04 L10.</summary>
public static class CommonCoreG2Unit4PlaceValueLineCorrection
{
    public const string Version="usccss-g2-u04-l10-original-source-repair-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L10";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-10/preparation.html";
    private const string Sha="4d89c06b99b5ee5bf8c77567b4de987c36ca9601e1f16eca2b65864e6c2f1233";
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.B.5"];
    private static readonly string[] Before=["Learning goal: You will compare representations of addition and subtraction methods represented on the number line.\nIn previous lessons, you matched addition and subtraction equations and your representations on a number line. You found the value of differences within 100 and represented your thinking on a number line. In this lesson, you add and subtract within 100 and represent computation methods on a number line.",
        "Key ideas:\n- On a number line, represent place value methods for solving addition and subtraction equations that do not require decomposing a ten\n- In previous lessons, you matched addition and subtraction equations and your representations on a number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Clare subtracted and represented her work with a base-ten diagram. Write an equation to represent Clare's work. Represent Clare’s method on the number line. Find the value of \\(58 - 24\\) . Show your thinking using a base-ten diagram. Represent how you found the value of \\(58-24\\) on the number line.\nApproach: “How are these methods the same? How are you different?” (You started at 54. Some show subtracting tens first, some show ones first. Some show subtracting the value of all the tens or all the ones. Some show subtracting each ten or each one.) “How does the number line help you see how these methods are the same?” (It helps you see that it doesn't matter if you subtract tens first or ones first. You both show...\nExample 2: Diego is finding the value of \\(33 + 45\\) . He says he can count on by tens, then by ones. He used a number line to show what he means. Write an equation to show the sum for Diego’s work. Find the value of \\(23 + 24\\) . Represent your thinking on the number line. Find the value of \\(50 - 32\\) . Represent your thinking on the number line. Find the value...\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will compare representations of addition and subtraction methods represented on the number line. In previous lessons, you matched addition and subtraction equations and your representations on a number line."];
    private static readonly string[] After=["Source-accurate IM K–5 2021 Grade2 Unit4 Lesson10 Place Value and the Number Line. Warm-up original 0..45 line and base-ten blocks: arrows0→28,28→38(+10),38→43(+5), hence28+15=43. Activity1 source Clare46−35=11 using tens and ones on blocks and number line; pupils solve58−24=34. Activity2 Diego33+45=78 using 4 tens +5 ones; also23+24=47,50−32=18,40−26=14. Compare place-value decomposition and directional jumps. Teacher inspects original Clare/Diego drawings and students' actual representations. Original source teacher preparation: https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-10/preparation.html. Teacher and source graphics are the academic source of record, and the restricted publisher Cool-down is not claimed as copied.",
        "Precise mathematical target for THIS lesson is place-value-bridge. Warm-up original 0..45 line and base-ten blocks: arrows0→28,28→38(+10),38→43(+5), hence28+15=43. Activity1 source Clare46−35=11 using tens and ones on blocks and number line; pupils solve58−24=34. Activity2 Diego33+45=78 using 4 tens +5 ones; also23+24=47,50−32=18,40−26=14. Compare place-value decomposition and directional jumps. Teacher inspects original Clare/Diego drawings and students' actual representations. Core rules: every number-line arrow's starting coordinate, direction, endpoint and equal unit length must be consistent; compare the actual representation, not only its arithmetic total. Use place-value tens/ones and true inverse relationships without inventing source-only diagram features.",
        "Original SOURCE expressions/context examples (not invented image locations): 28+15=43; 46−35=11; 58−24=34; 33+45=78; 23+24=47; 50−32=18; 40−26=14. Physical source images, teacher classroom activities and any open-ended pupil outcomes remain educator-observed. Additional randomly generated Edulytics practice cases MUST be labelled first-party, not original source data.",
        "1. Review actual source preparation and lesson image/text. 2. Identify source quantities, units and exact target relation: place-value-bridge. 3. On authentic physical pupil work interpret correctly the original source activity: Warm-up original 0..45 line and base-ten blocks: arrows0→28,28→38(+10),38→43(+5), hence28+15=43. Activity1 source Clare46−35=11 using tens and ones on blocks and number line; pupils solve58−24=34. Activity2 Diego33+45=78 using 4 tens +5 ones; also23+24=47,50−32=18,40−26=14. Compare place-value decomposition and directional jumps. Teacher inspects original Clare/Diego drawings and students' actual representations. 4. Demonstrate at least two mathematically consistent representations when allowed, including correct unit count, directed number-line arrows, base-ten blocks, diagrams or authentic group work as suited to this lesson. 5. Explain why alternative wrong method fails and verify the arithmetic against the original source values. 6. Present four separate source-aligned FIRST-PARTY numerical examples with automated exact verification; explicitly distinguish those invented examples from the genuine publisher tasks. 7. Teacher signs off original human tasks and inaccessible publisher image artifacts; digital numeric success is not academic acceptance.",
        "Potential errors for place-value-bridge: mixing a number-line endpoint with arrow length; performing operations without original direction; confusing count-by-ten place-value jumps with one-unit moves; treating a picture, classroom dice/spinner result or peer discussion not witnessed as checked; substituting invented Edulytics practice for a restricted publisher original Cool-down. Where teacher-only evidence is needed: Authentic base-ten drawings, real number-line arrow lengths, original source figure arrangement and peer MLR8 need human review.",
        "IM2021 Grade2 Unit4 Lesson10 original topic Place Value and the Number Line. Accurate published examples: 28+15=43, 46−35=11, 58−24=34, 33+45=78, 23+24=47, 50−32=18, 40−26=14. First-party numeric practice is independent. Authentic base-ten drawings, real number-line arrow lengths, original source figure arrangement and peer MLR8 need human review."];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
       d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
       d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Check(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
       if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
           throw new InvalidOperationException("G2 U04 L10 original lesson source identity drift");
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
           if(!Has(t,false))throw new InvalidOperationException("G2 U04 L10 source predecessor fields mismatch");
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
       if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U04 L10 unknown predecessor version");
       if(persisted.Count==0)return;
       if(persisted.Count!=1||persisted.Single().CultureCode!="en")throw new InvalidOperationException("G2 U04 L10 saved language drift");
       var old=persisted.Single(),incoming=l.Translations.Single(x=>x.CultureCode=="en");
       if(old.Title!=incoming.Title||!(Has(old,false)||Has(old,true)))
          throw new InvalidOperationException("G2 U04 L10 saved prose is not an approved predecessor");
    }
}

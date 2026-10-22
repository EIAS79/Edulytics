using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally sourced IM2021 G2 U02 L09, exact prior body and source guard.</summary>
public static class CommonCoreG2Unit2MixedAddSubtractTeachingCorrection
{
    public const string Version="usccss-g2-u02-l09-mixed-add-subtract-original-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L09";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-9/preparation.html";
    private const string Sha="d07bbae229eab645c91ba8806e764b9113eafa193c1bfe50096334a1fc89895f";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will add and subtract within 100, including composing and decomposing a ten, using strategies based on place value and the properties of operations.\nIn previous lessons, you explored different methods for addition and subtraction with and without composing or decomposing a ten. You used base-ten blocks, drawings, and equations to represent your methods. In this lesson, you choose your preferred methods and representations to add and subtract.",
         "Key ideas:\n- Add and subtract within 100 using strategies based on place value, including composing and decomposing a ten, and the properties of operations\n- In previous lessons, you explored different methods for addition and subtraction with and without composing or decomposing a ten\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.\nExample 2: Match each expression to a base-ten diagram. Choose 1 addition expression and find the value of the sum. Choose 1 subtraction expression and find value of the difference.\nApproach: “Which expressions did you choose to solve? Why?” (I chose \\(35 + 42\\) because it was easy for me. I knew that I could just add the ones and then add the tens.) “How could you tell if you would need to compose or decompose a ten?” (I could see that \\(5 + 2 = 7\\) , so I knew I wouldn’t need to compose a ten.)",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will add and subtract within 100, including composing and decomposing a ten, using strategies based on place value and the properties of operations. In previous lessons, you explored different methods for addition and subtraction with and without composing or decomposing a ten."],
        ["G2 Unit 2 Lesson 9 Add and Subtract Within 100 (IM K–5 2021): students choose place-value strategies and representations for BOTH addition and subtraction within100, with and without composing or decomposing a ten. The warm-up Which One Doesn't Belong? compares four original labeled A–D base-ten diagrams; their actual image details are not in the local public HTML text, so Edulytics asks the teacher to show the authentic source images and does not invent their shapes. Activity1 Sort and Find the Value uses the original printed cards for groups of 2–3 to match arithmetic expressions to base-ten diagrams, then select one addition and one subtraction to solve; source synthesis cites35+42=77 (without a new ten). Card-specific values beyond this public example are not asserted. Activity2 Add or Subtract (20 minutes): original learner problems27+47=74,55−27=28,36+38=74,82−39=43, use drawings, equations or real blocks, and MLR7 Compare and Connect peer gallery walk comparing actual methods. Source section summary gives63−18=45. Independently authored Edulytics additional math and exit, not teacher-only publisher cool-down, are checked digitally; teacher reviews original diagrams and genuine partner participation.",
         "Addition within100: add tens with tens, ones with ones; if ones total≥10, exchange ten ones for one ten. 27+47: 20+40=60,7+7=14;60+14=74. 36+38:30+30=60,6+8=14;60+14=74. No exchange needed for35+42 because5+2=7, so result77. Subtract within100: if starting ones are fewer than the ones to remove, decompose one ten. 55−27:55=40+15;40−20=20,15−7=8, result28. 82−39:82=70+12;70−30=40,12−9=3, result43. 63−18=45 because63=50+13,50−10=40,13−8=5. Source A–D warm-up picture structures and original printed Sort and Find the Value blackline cards require their actual physical materials. Compare student-chosen methods via MLR7 and MLR8; a digital number does not prove authentic class discussion. Official Addressing 2.NBT.B.5; warm-up is Building Towards 2.NBT.B.5.",
         "PUBLIC source Activity1 example35+42=77; 5+2=7 ones so no ten is composed. PUBLIC Activity2:27+47=74 with ones7+7=14, compose a new ten;55−27=28 by55=40+15;36+38=74 by6+8=14;82−39=43 by82=70+12. Public student section summary:63−18=45. Source warm-up is original A–D Which One Doesn't Belong? with teacher-only images; exact visual features not reconstructed from missing public text. Source Activity1 paper cards: expressions match diagrams in real groups, no invention of protected or unavailable card quantities. FIRST-PARTY Edulytics demonstrations:46+27=73 (compose ones6+7=13),75−48=27 (decompose75=60+15),58+26=84 (compose8+6=14).",
         "1. Warm-up: teacher displays four authentic source A–D base-ten images; students choose a different reason each picture might not belong and justify how tens and ones are grouped. 2. Activity1: in genuine teams of2–3, take original Sort and Find the Value cards, match each expression and image, justify, choose one addition and one subtraction, and explain each. Example35+42=77 can be computed by 30+40=70 and5+2=7. 3. For source27+47, add tens20+40=60; add ones7+7=14; compose one ten and sum60+14=74. 4. For55−27, exchange55=40+15, subtract20 and7 respectively, result28. 5. For36+38, add tens60 and ones14 to obtain74. 6. For82−39, exchange82=70+12, remove30 and9, result43. 7. Conduct authentic MLR7 Compare and Connect gallery walk on actual students' different representations of the SAME expression; teacher listens and checks original work and peer feedback. 8. Verify subtraction with inverse addition and complete a SEPARATE independently authored Edulytics exit, not the publisher's restricted Find the Value Your Way Cool-down.",
         "Claiming every source activity is subtraction only, despite mixed addition27+47 and36+38. Not composing a new ten when ones sum7+7=14 or6+8=14. Double-counting ten when exchanging units. Treating55−27 as55−20+7 instead of55−20−7 (correct28). Subtracting9 ones from2 in82−39 without an exchange or a mathematically valid alternate strategy; correct43. Using missing A–D picture content or unretrieved original print card amounts as if verified. Mistaking the MLR7 comparison of different ways to solve ONE expression for comparing unrelated answers. Declaring a student completed real original cards or peer gallery walk solely from their digital answers is unjustified.",
         "Source G2 U02 L09: warm-up four original A–D diagrams teacher-observed, Activity1 real Sort and Find the Value cards including35+42=77, Activity2 four problems27+47=74,55−27=28,36+38=74,82−39=43 with student-preferred place-value models; section summary63−18=45. Both add with composing and subtract with decomposing. Digital independent math and actual group dialogue remain distinct."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U02 L09 source/hash/outcome changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(CurriculumLessonContentTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U02 L09 old learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)?Version:old;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U02 L09 old version not approved");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L09 old translations not approved");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U02 L09 prior learner text changed");
    }
}

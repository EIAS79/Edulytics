using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L06 local IM2021 source review, exact six learner-field predecessor.</summary>
public static class CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection
{
    public const string Version="usccss-g2-u02-l06-target-numbers-regroup-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L06";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-6/preparation.html";
    private const string Sha="2ee2bc7faca1a769c37fcc684803314bb3d621224f137261329171fded300a37";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will compare methods for subtracting a one-digit number from a two-digit number with and without decomposing a ten.\nIn the second activity, you find the difference with and without decomposing a ten and represent your thinking using base-ten diagrams, words, or equations. You are not expected to draw your work with base-ten diagrams in a specific way. You should have access to base-ten blocks throughout the lesson and the cool-down.",
         "Key ideas:\n- Describe how methods of subtraction are the same and different when subtracting a one-digit number from a two-digit number\n- In the second activity, you find the difference with and without decomposing a ten and represent your thinking using base-ten diagrams, words, or equations\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(64 = 60 + 4\\) \\(64 = 50 + 14\\) \\(64 = 30 + 24\\)\nApproach: “What pattern did you notice?” (When the first addend went down by 10, the second addend went up by ten, so the sum stayed the same.)\nExample 2: Diego and Elena drew base-ten diagrams to find the value of \\(82-9\\) . Diego Elena Compare Deigo’s work to Elena’s. What is the same? What is different? Tyler used equations to show his thinking. \\(82 - 9\\) \\(82 = 70 + 12\\) \\(12 - 9 = 3\\) \\(70 + 3 = 73\\) Diego says Tyler’s work matches his diagram. Elena says Tyler’s work matches her diagram. Who do you...\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will compare methods for subtracting a one-digit number from a two-digit number with and without decomposing a ten. In the second activity, you find the difference with and without decomposing a ten and represent your thinking using base-ten diagrams, words, or equations."],
        ["Grade 2 Unit 2 Lesson 6 — Compare Methods for Subtraction (IM K–5 2021). Warm-up True or False? 64=60+4 is TRUE, 64=50+14 is TRUE, but 64=30+24 is FALSE because30+24=54. The value is unchanged when a ten is moved from the first addend to the second:60+4=50+14; if 50 decreases by20 to30,14 must increase by20 to34, not24. Activity1 Different Ways to Decompose: Diego and Elena draw different first-party source-aligned base-ten representations of82−9=73; Tyler uses82=70+12,12−9=3,70+3=73. One drawing trades a ten during subtraction, another starts at seven tens and twelve ones. Both can correspond to Tyler's equations when justified; compare their methods, not merely numerical answers. Activity2 Introduce Target Numbers Stage4: two players begin at99, use REAL base-ten blocks and number cards1–9 (remove cards0 and10). On a turn the player draws a card k, chooses to subtract k ONES or k TENS (10k), records an equation and updates the running number. Complete six rounds or stop when a player reaches zero; do not go below zero. Closest to zero after six rounds wins. Real center gameplay and MLR2 partner discussion are teacher-scored; original Edulytics first-party arithmetic simulations cannot attest actual board or game outcomes.",
         "Equal expressions must represent the same total:64=60+4 and64=50+14 but30+24=54 not64. One ten equals ten ones; decomposing changes form but not amount. For82−9,80+2 becomes70+12 and73 remains. Strategies can draw8 ten-rods then trade one or draw7 tens and12 singles already decomposed, yielding identical value. Source Target Numbers Stage4: start99; valid cards are integers1..9 ONLY; subtract either card ones or TEN TIMES card in tens, never a mixture in one turn. Each turn's result is the next turn's starting value; the result must remain0..99. At most six turns, first reach0 or closest after six rounds; winning is a real partner activity not deducible from one typed sum. Example card4 from99 can produce95 (minus4 ones) or59 (minus4 tens). If one digit's ones are insufficient, subtraction of ones may require exchanging a ten; subtracting tens may also need careful place-value reasoning. Synthesis50−7=43 because50=40+10 and10−7=3. The official Addressing CCSS is2.NBT.B.5 only.",
         "SOURCE warm-up:64=60+4 TRUE;64=50+14 TRUE;64=30+24 FALSE (equals54). Compare different ways of showing82−9: from8 tens+2 ones trade to7 tens+12 ones; then subtract9 to get7 tens+3 ones=73. Tyler equations:82−9,82=70+12,12−9=3,70+3=73. Source TARGET NUMBERS Stage4 original rules: begin99, card1–9, each turn choose to remove that card's ones or tens and write an equation. FIRST-PARTY example round99 with card4:99−4=95 (ones) or99−40=59 (tens); round after95 with card7:95−7=88 if one ten is decomposed, or95−70=25 without ones exchange. SOURCE synthesis50−7=43 using50=40+10. Independent original Cool-down:71−8=63,60−7=53 and92−8=84.",
         "1. Decide whether each equality for64 holds, using the equivalence of tens and ones; explain why30+24=54 is NOT64. 2. With real base-ten blocks show82 as8 tens+2 ones. Trade one ten to make7 tens+12 ones. 3. Draw or interpret Diego's and Elena's different strategies for82−9 and compare both to Tyler's 82=70+12 equations; students may reasonably agree with both because both describe the same regrouping. Teacher checks original drawing and explanation. 4. For Target Numbers Stage4 in actual pairs, begin with99 blocks and cards1–9 (cards0 and10 removed). 5. Take ONE card per turn. Choose either to subtract that many ONES or TENS, ensuring the resulting count remains nonnegative. Write the full true equation; update the current number. 6. Alternate for up to6 rounds or stop early at0. The winner after six rounds is the one with the smallest remaining nonnegative total. Teacher observes REAL turn sequence, independently created record sheets and partners' mathematical conversation. 7. Explain when an exchange was necessary, including source synthesis50−7=43 via50=40+10. 8. Complete an independently authored Edulytics digital exit; do not claim the publisher's restricted 'Mai's Method' Cool-down was copied or completed.",
         "Misclassifying64=30+24 as true;30+24=54. Exchanging one ten into ten ones AND keeping it as a ten would increase the count by10 incorrectly. Claiming Tyler's82=70+12 equations can match only one representation even though both Diego and Elena may model that same decomposition. In Target Numbers Stage4, leaving0 or10 number cards in the deck, subtracting card4 as44 or subtracting both4 ones and4 tens in one turn violates the stated choice. Continuing below zero or claiming a winner from one digital arithmetic example does not match the real six-round game. Subtracting a full ten (or multiples) without respecting the card. Treating a typed sentence as evidence of original diagrams, actual manipulation of blocks or peer discussion is not justified.",
         "Compare equivalent place-value forms and subtraction methods. Warm-up:64=60+4 TRUE;64=50+14 TRUE;64=30+24 FALSE. Diego, Elena and Tyler show82−9=73 using82=70+12. Original Target Numbers Stage4: start99, each turn draw card1..9, choose subtract that number of ones OR tens, stop on zero or after6 rounds and compare closest to0. Independently authored digital math, actual classroom pair activity teacher-reviewed."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U02 L06 source identity changed");
    }
    private static bool Equal(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Equal(t,true))continue;
            if(!Equal(t,false))throw new InvalidOperationException("G2 U02 L06 previous six fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))throw new InvalidOperationException("G2 U02 L06 previous persisted version invalid");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L06 language rows invalid");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Equal(t,false)||Equal(t,true)))
            throw new InvalidOperationException("G2 U02 L06 stored learner fields changed");
    }
}

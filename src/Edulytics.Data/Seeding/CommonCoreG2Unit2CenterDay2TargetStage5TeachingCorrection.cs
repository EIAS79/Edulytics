using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>2021 local source L10 optional Stage5 two-digit target, source rules intact.</summary>
public static class CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection
{
    public const string Version="usccss-g2-u02-l10-target-stage5-source-rule-conflict-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L10";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-10/preparation.html";
    private const string Sha="7a841f3ab2beb2bb27c0701e7e611b56bcae6ad37600cf104538895177308b14";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will practice adding and subtracting within 100 using methods based on place value understanding, the properties of operations, and the relationship between addition and subtraction.\nThis lesson is optional because it is an opportunity for extra practice with addition and subtraction within 100 that not all classes may need. In Activity 1, you learn stage 5 of the Target Numbers center which was introduced in a previous lesson. In this new stage, called Subtract Two-digit Numbers, you roll number cubes to create two-digit numbers from a starting number to get as close to 0 as possible.",
         "Key ideas:\n- Add and subtract within 100\n- This lesson is optional because it is an opportunity for extra practice with addition and subtraction within 100 that not all classes may need\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: What do you notice? What do you wonder? \\(66 - 28 \\) \\(66 = 50 + 16 \\) \\(16 - 8 = 8 \\) \\(50 - 20 = 30\\) \\(30 + 8 = 38\\) \\(66 - 28 = 38\\)\nApproach: “Both of these representations show ways to find the value of \\(66 - 28\\) . How are you the same? How are you different?” (Both show taking tens from tens and ones from ones. Both show decomposing a ten. One uses base-ten diagrams and one uses only numbers and equations.)",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will practice adding and subtracting within 100 using methods based on place value understanding, the properties of operations, and the relationship between addition and subtraction. This lesson is optional because it is an opportunity for extra practice with addition and subtraction within 100 that not all classes may need."],
        ["Grade 2, Unit 2 Lesson 10 — Center Day 2 (optional), IM K–5 2021. Warm-up Notice and Wonder compares a base-ten diagram and equations showing 66−28=38:66=50+16,16−8=8,50−20=30,30+8=38. The teacher displays the authentic image and invites a real partner conversation. Activity1 introduces Target Numbers Stage5, Subtract Two-digit Numbers: pairs start at100 with real base-ten blocks, roll THREE number cubes, assign one cube face to TENS and a DIFFERENT cube face to ONES, subtract the resulting two-digit number without going below zero, record an equation, and use the remainder as the next starting amount. Activity2 Centers: Choice Time allows Target Numbers Stages4/5, Capture Squares Stages3/4 or Five in a Row Stage6; student-selected center participation is teacher-observed. Public source contains a rule discrepancy: narrative says closest to zero after six rounds wins, while teacher launch says play until someone is below10. Those rules are recorded as different printed source statements rather than silently reconciled. No source Cool-down appears on the public archived lesson. Independent Edulytics numerical companions use rolled 4,2,5 (73−24=49 and73−52=21) but do not claim actual game completion.",
         "Number-cube rule: roll THREE physical number cubes (typically faces1–6); select the face of one cube as the tens digit and a DIFFERENT cube as the ones digit to form a two-digit amount. Matching digit values can be possible if they came from different cubes, but you cannot reuse the same cube twice. Current amount begins at100, each turn subtract only one generated two-digit number and must not go below0. Next equation begins with the prior difference; authentic turn-taking and partner recording are teacher-scored. Teacher source contains two end descriptions: closest to0 in6 rounds versus first below10; do not declare an authentic winner from a typed sum without verifying the actual center rules and play. Decompose a ten if starting ones are fewer than subtracted ones:73−24 requires73=60+13 and result49. For73−52, ones3−2 work without regrouping, result21. Warm-up:66−28=38 because66=50+16 and16−8=8,50−20=30,30+8=38. Activity2 optional choice centers include authentic prior Capture Squares and Five in a Row rules. Standards in pack: 2.NBT.B.5,2.NBT.B.9,2.OA.B.2.",
         "PUBLIC warm-up:66−28;66=50+16;16−8=8;50−20=30;30+8=38. PUBLIC source synthesis: from73 and dice showing4,2,5, possible two-digit numbers include24 and52. If choosing24, ones3 are fewer than4, exchange one ten:73−24=49. If choosing52, no exchange of a ten is needed:73−52=21. The greatest tens/ones digit arrangement available from those faces is54;73−54=19, a valid mathematical difference but NOT evidence that the real game was won. Source centers: Stage5 Target Numbers, Stage3/4 Capture Squares, Stage6 Five in a Row (including composing a ten). Independently authored Edulytics practice instead of a publisher Cool-down:82−35=47,94−56=38,70−24=46.",
         "1. Compare original Notice and Wonder teacher image with equations for66−28=38, and explain why trading one ten changes66 into50+16 but not its value. 2. In actual pairs collect three number cubes and real base-ten blocks; each student represents the starting value100. 3. Roll all THREE cubes once per real turn; choose a distinct cube face for the tens digit and one for the ones digit, creating a two-digit amount. 4. Subtract the amount without going below0; use base-ten blocks to trade a ten when there are insufficient ones. Record the equation and new current number. 5. Continue authentic rounds using the original paper rule: public teacher text has both first-below10 and nearest-zero-after-six formulations; teacher confirms the rule version in use and observes the true winner. 6. In the official source synthesis, for current73 with faces4,2,5, either choose24 (73−24=49, exchange a ten) or52 (73−52=21, no exchange), explaining both. 7. During choice time select an authentic source center: Target Numbers Stage4/5, Capture Squares Stage3/4, or Five in a Row Stage6. The teacher observes genuine partner turn-taking and center mathematics. 8. Complete a SEPARATE first-party Edulytics numerical exit; no publisher restricted activity is reproduced.",
         "Starting Target Numbers Stage5 at99 rather than100 (99 is for previous Stage4). Using only two cubes despite three being rolled, or reading the same physical cube for both digit positions rather than selecting two distinct cube positions. Forming a number by ADDING all three die faces instead of making a two-digit tens-and-ones number. For73−24 ignoring the need to decompose when3 ones cannot remove4; correct49. Claiming73−52 requires a ten exchange even though3 ones−2 ones works directly. Letting a move produce a negative remainder. Confusing the public source's two incompatible end-rule phrasings, or claiming a winner from one digital subtraction result. Asserting that an independent Edulytics number was an authentic rolled face or that classroom center choice/peer discussion occurred when not observed.",
         "Optional Center Day2. Warm-up66−28=38 using66=50+16. Target Numbers Stage5 begins100, roll3 number cubes, choose different cubes for tens and ones, subtract the two-digit number, write equation and avoid negatives. Public teacher source has both first-below10 and nearest0-after6-round descriptions; teacher resolves actual game end. Example current73 with4,2,5:73−24=49 exchanges a ten;73−52=21 does not. Choice of earlier centers remains real partner work; digital extra exercises are first-party."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.NBT.B.9","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U02 L10 original source/hash/outcomes drifted");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool newVersion)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newVersion);
    private static bool Matches(CurriculumLessonContentTranslation t,bool newVersion)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newVersion);
    private static bool Same(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U02 L10 original learner text changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)?Version:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U02 L10 previous persistence version changed");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L10 prior translations changed");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U02 L10 prior rich learner body changed");
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L13 original source-linked story equations and unknown positions.</summary>
public static class CommonCoreG2Unit2StoryEquationsTeachingCorrection
{
    public const string Version="usccss-g2-u02-l13-seed-equations-four-unknown-positions-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L13";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-13/preparation.html";
    private const string Sha="ceb1109613f7aef430f0d77e17f819b52b39dcc756df5cbb45784c204b95b6c9";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will make sense of equations and connect them to the quantities in different types of story problems.\nIn previous lessons, you interpreted different types of story problems and matched them to diagrams. In this lesson, you continue to connect story problems to diagrams and use your work to make sense of equations and symbols that represent the unknown number (MP2).",
         "Key ideas:\n- Make sense of equations that represent story problems\n- Represent and solve one-step story problems within 100\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
         "Example 1: Which one doesn’t belong? \\(27 + \\underline{\\phantom{\\hspace{1.05cm}}} = 40\\)\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.\nExample 2: Match each story problem with an equation. Explain why the cards match. Choose 2 story problems and solve them. Show your thinking.\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will make sense of equations and connect them to the quantities in different types of story problems. In previous lessons, you interpreted different types of story problems and matched them to diagrams."],
        ["Grade 2 Unit 2 Lesson 13 Story Problems and Equations (IM K–5 2021). Warm-up Which One Doesn't Belong? invites children to compare original teacher-displayed tape diagrams, including an equation27+?=40, where13 is missing. The actual four diagram images are examined in class; first-party content does not reproduce missing published artwork. Activity1 Card Sort Story Problems and Equations: groups of2–4 match authentic previously introduced story cards A–I with the original Equation Cards. More than one correct equation MAY match a story, depending on whether it expresses an action or a part-whole relationship. The individual unpublished printed card values are not invented. Activity2 Represent and Solve Story Problems: Lin began with ? seeds, WON36 and ended64, so start28 and equation?+36=64. Andre began32, WON ? to reach57, so change25,32+?=57. Diego gathered22 yellow and48 blue seeds, so total70,22+48=70. Noah and Kiran gathered92, Noah gathered53, Kiran unknown39,53+?=92. The equation representing the action can be addition even when subtraction is a helpful calculation. True tape diagrams, student-written equations, MLR8 peer dialogue and physical base-ten blocks are teacher reviewed. Original 'Match the Equation' Cool-down is restricted, independent Edulytics exit separate.",
         "Read the UNKNOWN position carefully. Start Unknown: ?+36=64, 64−36=28, Lin began28. Change Unknown:32+?=57,57−32=25, Andre won25; a subtraction may FIND the unknown, but the story describes addition. Result Unknown:22+48=? gives70, Diego collected70. Part Unknown:53+?=92,92−53=39, Kiran collected39. The warm-up equation27+?=40 has13 missing. Valid equations can express the part-whole relationship in more than one direction: 53+39=92,92−53=39 or92−39=53, but a chosen unknown must match what the actual question asks. A tape diagram should label totals and parts accurately; MLR8 discussion helps learners explain WHY equation cards match. Decomposing one ten for 64−36 and92−53, and composing for22+48, reflect place value. Original Addressing codes 2.NBT.B.5 and2.OA.A.1.",
         "SOURCE warm-up27+?=40, ?=13 (the original A-D diagrams are teacher-displayed). SOURCE Lin: ?+36=64 => ?=28, because64−36=28. SOURCE Andre:32+?=57 => ?=25; 57−32=25 can solve an add-to story even though subtraction is not the narrated action. SOURCE Diego:22+48=? =>70 total seeds (2 ones+8 ones compose a ten). SOURCE Noah/Kiran:53+?=92 => ?=39 (92−53=39). Source card sort authentic A–I story cards and separate equations may have MORE THAN ONE valid match; their printed texts must be consulted, not fabricated. Independent FIRST-PARTY Edulytics examples: unknown start?+27=63=>36; unknown change41+?=75=>34; total18+47=65; unknown part56+?=91=>35.",
         "1. Warm-up examine actual original source teacher A-D diagrams in pairs, compare labeling and match to27+?=40; find missing13 and explain why the diagram reflects the equation. 2. In real groups2–4, read ORIGINAL printed story cards A–I and Equation Cards and match them with reasons using MLR8. More than one equation can be correct; identify HOW each expresses the story quantities and unknown. 3. For Lin identify a missing START and write ?+36=64. Take36 away from64 to get28, then verify28+36=64. 4. For Andre missing change32+?=57, subtract32 from57 to find25; explain that ADDITION still represents the events in the story. 5. Diego has22 yellow and48 blue; combine them with a tens exchange for22+48=70. 6. Noah and Kiran total92, Noah53; model53+?=92 and use92−53=39. Show with a labeled tape diagram that53 and39 are parts of92. 7. Compare original student diagrams, equations and calculation methods for Noah/Kiran with peers, and explain why subtraction and addition equations may BOTH express relationships. Teacher checks authentic drawings, explanations and blocks. 8. Use a SEPARATE independently authored Edulytics exit, not original restricted Match the Equation.",
         "Thinking Lin started64−36=38 (correct28), or misidentifying missing start as missing result. Writing Andre32−?=57 even though he won more; correct story equation32+?=57, which can be solved by57−32. Missing ten composition in Diego22+48=70. Giving Noah's53 as Kiran's number instead of39. Assuming only one equation can match a story when multiple equivalent relationships may hold. Matching an equation to a publisher print card whose actual value was not consulted. Equating the act of subtracting to find an unknown with a narrated story in which the amount increases. Declaring authentic group card matching or diagram discussion complete based on a numeric digital answer is invalid.",
         "Story Problems and Equations: warm-up27+?=40 missing13. Source Lin start unknown ?+36=64 =>28; Andre change unknown32+?=57=>25; Diego combined22+48=70; Noah/Kiran missing part53+?=92=>39. Match authentic source story and equation cards, allowing multiple valid matches when justified; teacher reviews actual diagrams/discussion. Independent Edulytics practice explicitly separate from restricted Cool-down."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L13 original source identity changed");
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
            if(!Matches(t,false))throw new InvalidOperationException("G2 U02 L13 predecessor six learner fields drifted");
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
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U02 L13 persisted predecessor version unexpected");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L13 persisted rows unexpected");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U02 L13 stored predecessor learner fields drift");
    }
}

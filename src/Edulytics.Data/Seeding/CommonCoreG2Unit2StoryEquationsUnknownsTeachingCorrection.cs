using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L13 locally read public IM2021 source; preserve real equations and unknown positions.</summary>
public static class CommonCoreG2Unit2StoryEquationsUnknownsTeachingCorrection
{
    public const string Version="usccss-g2-u02-l13-story-equation-unknown-position-v1";
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
        ["Grade 2 Unit2 Lesson13 Story Problems and Equations (IM K–5 2021). Warm-up Which One Doesn't Belong? asks learners to compare authentic tape diagrams and the source equation 27+?=40 (missing13) and discuss which diagram models the unknown. Activity1 Card Sort Story Problems and Equations: in authentic groups of2–4, reuse original story Cards A–I from the PREVIOUS lesson, match them to the source Equation Cards, explain what each symbol and question mark means, and choose TWO source stories from Cards F, G, H or I to solve. The exact original printed card text and art are not available in the locally archived public HTML, so do not invent card content or claim a digital card match. Activity2 Represent and Solve Story Problems (20 minutes) has FOUR public student tasks: Lin started with an unknown number of seeds, won36 and ended with64 -> start28; Andre starts32, gains unknown to get57 -> gained25; Diego gathers22 yellow and48 blue -> total70; Noah/Kiran gather92 total, Noah53 -> Kiran39. Label original diagrams and write addition equations with question marks to show story ACTION or part-whole structure, then use addition or subtraction strategically to compute the unknown. MLR8, partner explanations and authentic personally created diagrams are teacher assessed. Source Match the Equation Cool-down requires educator sign-in; first-party Edulytics extra maths is separate.",
         "A mathematical equation should represent the STORY relationship, even when a DIFFERENT operation is useful for finding the unknown. Add To Start Unknown: ?+36=64, unknown28; can calculate64−36=28. Add To Change Unknown:32+?=57, unknown25; can calculate57−32=25 without claiming subtraction was the story's action. Put Together Total Unknown:22+48=? means70 total (22+48 needs composing a ten). Put Together Part Unknown:53+?=92, unknown39; also92−53=39 is an equivalent related difference. Warm-up 27+?=40 missing13, describing the whole-part roles in actual source diagrams. Card sort: source Cards A–I from last lesson, source equation cards, groups of2–4, two solved from F–I, but card-specific unpublished values cannot be represented as known. Original Addressing outcomes 2.NBT.B.5 and2.OA.A.1; Activity2 numbers remain within100.",
         "PUBLIC warm-up source 27+?=40: ?=13, but the point is matching authentic source diagrams to a missing addend. SOURCE Lin initial?+36=64 ->28; numerical check64−36=28. SOURCE Andre32+?=57 ->25; calculating57−32=25 is allowed although the action is ADDING. SOURCE Diego22+48=? ->70; compose ones2+8=10. SOURCE Noah/Kiran53+?=92 ->39; equivalent92−53=39. Source Activity1 printed cards A–I and equation cards are not fully public in the archived teacher page; their contents aren't invented. FIRST-PARTY Edulytics-independent extra: unknown start?+27=75 =>48; unknown added46+?=83 =>37; combined29+38=67; missing part54+?=91 =>37. These are not the restricted publisher Match the Equation Cool-down.",
         "1. Warm-up: with the teacher's true printed diagrams compare which tape diagram matches27+?=40 and identify the unknown as13; real image-based explanations teacher observed. 2. In genuine groups of2–4, reuse prior lesson's story Cards A–I, find matching original equation cards and articulate why each number and ? maps to the original relationship. Solve TWO actual source Cards F–I as specified; no unpublished card numbers are inferred. 3. For Lin, depict initial unknown, then36 WON, final64. Write ?+36=64, compute64−36=28 and check28+36=64. 4. For Andre, start32, win an unknown, finish57; label32+?=57, subtract57−32=25 to find the added amount, check32+25=57. 5. For Diego, combine22 yellow+48 blue=70, with unknown sum22+48=?; represent carrying10 ones to a ten. 6. For Noah and Kiran,92 total, Noah53, Kiran unknown, write53+?=92, compute92−53=39 and verify53+39=92. 7. Compare source diagrams, original question-mark equations and different correct calculation strategies with real partners. Teacher checks both accurate story labels and whether the mathematics reflects the action. 8. Complete original Edulytics first-party exit separately without attributing it to the restricted source assessment.",
         "Assuming the operation used to FIND a missing addend must describe the story action; Andre's story describes gaining seeds, so32+?=57 represents it while57−32=25 computes the unknown. Putting ? at the result for Lin when the UNKNOWN is the initial amount, or misreading36 as the initial count; correct?+36=64. Writing Diego22+48=60 instead of70 due to failing to compose the ten from2+8. For Noah/Kiran mistaking92+53 for total, when92 already includes53 and Kiran39. Matching source equation cards solely because numbers appear, without checking the question-mark role. Inventing real Cards F–I problem numbers not visible in the archived public HTML. Treating typed numeric answers as proof of original partner card sorting, MLR8 conversation or publisher cool-down completion is invalid.",
         "G2 U02 L13: warm-up 27+?=40 missing13; Activity1 matches REAL previous-story Cards A–I with authentic question-mark equation cards and solves TWO F–I cards; Activity2 Lin?+36=64=>28, Andre32+?=57=>25, Diego22+48=?=>70, Noah/Kiran53+?=92=>39. Equation shows story relationship; subtraction may find an addend. Teacher checks source diagram work, digital extra is first-party."]);
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
    private static bool Match(CanonicalLessonContentPackTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(CurriculumLessonContentTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(t,true))continue;
            if(!Match(t,false))throw new InvalidOperationException("G2 U02 L13 previous six learner fields changed");
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
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U02 L13 predecessor version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L13 predecessor translations changed");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Match(t,false)||Match(t,true)))
            throw new InvalidOperationException("G2 U02 L13 previous persisted text changed");
    }
}

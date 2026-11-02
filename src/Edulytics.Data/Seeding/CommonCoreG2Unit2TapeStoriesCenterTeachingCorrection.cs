using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Original optional G2 U02 L15 Tape Diagrams Center, exact source guard.</summary>
public static class CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection
{
    public const string Version="usccss-g2-u02-l15-optional-tape-stories-stage5-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L15";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-15/preparation.html";
    private const string Sha="26fcef0fc88a6f71b34e5ed2d2141d9f65e924ed7bbd0b59af1ddbe6684d044c";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will interpret diagrams, solve story problems, and add and subtract within 100.\nThis lesson is optional because it is an opportunity for extra practice interpreting diagrams and adding and subtracting within 100 that not all classes may need. In the first activity, you learn stage 5 of the Math Stories center, which was last introduced in grade 1. In this new stage, called Tape Diagrams, you interpret tape diagrams and create your own story problems to solve.",
         "Key ideas:\n- Add and subtract within 100\n- Interpret diagrams\n- Solve one-step story problems\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Find the value of each expression mentally. \\(25 - 10\\) \\(35 - 10\\) \\(35 - 20\\) \\(35 - 19\\)\nApproach: Highlight how the result of \\(35 - 20\\) can be used to calculate \\(35 - 19\\) . Record this with the equation: \\(35 - 19 = 35 - 20 + 1\\) .",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will interpret diagrams, solve story problems, and add and subtract within 100. This lesson is optional because it is an opportunity for extra practice interpreting diagrams and adding and subtracting within 100 that not all classes may need."],
        ["IM K–5 2021 Grade2 Unit2 Lesson15 is OPTIONAL Center Day3, not a standard teacher-led four-story worksheet. Warm-up Number Talk: 25−10=15,35−10=25,35−20=15,35−19=16. The final question uses compensation: subtract20 from35 to reach15, then add1 back because19 is one LESS than20. Activity1 introduces Math Stories Center Stage5 Tape Diagrams. Two students use ORIGINAL PRINTED tape-diagram cards (their diagram quantities are not visible in the public teacher HTML), each creates a fitting story problem, writes an equation, and gives it to a REAL partner to solve. They may invent Put Together/Take Apart, Add To, or Compare stories whose unknown and quantities genuinely match the selected card. Activity2 Centers: Choice Time allows previously taught Target Numbers Stages4/5, Capture Squares Stages3/4 or Five in a Row Stage6. Teacher observes actual card interpretation, original student-created stories and partner math; digital arithmetic cannot prove this occurred. There is no source Cool-down on the archived public lesson. First-party Edulytics example cards and numeric exit are explicitly NOT the protected source artwork.",
         "Interpreting a tape diagram: identify which bar is the total and which segments represent its parts, a change, or a difference. An authentic original SOURCE tape card determines the story numbers; do not invent numbers for that publisher card. An independent Edulytics invented tape diagram may use equal relations like [32][17] total49 and asks how many in all; or a whole72 and given part45 asks missing27; or compare larger64 to smaller39 to find gap25. For partner Stage5, the author writes a story AND a matching equation, while their partner solves and explains the diagram. Different valid stories may share the same bars if the relationship is respected. Warm-up 35−19=35−20+1=16, NOT35−20−1; subtracting one fewer means one more remains. Source optional centers may involve different fluency limits, Capture Squares within20, Target Numbers within100, and Five in a Row with composing tens. Source Addressing 2.NBT.B.5,2.OA.A.1,2.OA.B.2.",
         "SOURCE Number Talk:25−10=15;35−10=25;35−20=15;35−19=16 because35−20+1=16. Source Activity1: original partner Math Stories Stage5 Tape Diagrams PRINTED cards, students tell original story, write equation, partner solves; exact card quantities are unavailable in public source and must not be fabricated. Source Activity2: authentic choice between Target Numbers Stage4/5, Capture Squares Stage3/4, Five in a Row Stage6. FIRST-PARTY Edulytics invented tape model examples only:32+17=49 total,72−45=27 missing part,64−39=25 compare gap. Independent numerical exit first-party:45−19=26 using45−20+1;57+18=75;80−36=44.",
         "1. Original Number Talk: subtract tens from tens25−10=15;35−10=25;35−20=15;35−19=16 via compensate35−20+1. 2. For authentic Math Stories Stage5 pair work, look at a genuine printed tape-diagram card. Identify total, parts, comparison larger/smaller or an action change; don't assign fictitious values from a missing publisher image. 3. Write a student-created context whose quantities and question agree with the selected tape diagram, then write a mathematically matching equation with a clear unknown. 4. Give the actual card and your invented story to a real partner and let the partner solve, checking with a diagram and inverse operation when useful. 5. Exchange roles and consider whether the same diagram supports a different valid story; teacher checks authentic printed cards, peer explanation and matching symbols. 6. During choice center use real materials from Target Numbers4/5, Capture Squares3/4, or Five in a Row6, and follow that center's own rules. Teacher observes the actual stage, fair turns and participation. 7. First-party Edulytics original digital tape examples32+17,72−45,64−39 and exit45−19,57+18,80−36 provide independent arithmetic checks, never a fabricated source card or official game result.",
         "Treating this optional center day as if the publisher supplied three numerical tape diagrams in the public lesson page, even though original blackline card quantities are unavailable. Writing a story with quantities that cannot be represented by the selected card, or an equation whose ? represents the wrong segment. Describing a Compare tape diagram as a whole-plus-two-parts without justification. Mistaken compensation35−19=14 by subtracting an extra one after35−20=15, rather than adding back one to reach16. Assuming Five in a Row Stage6 forbids composing tens; the earlier source stage explicitly allows it. Treating a teacher-only real partner story, source card, turn-taking or winner as completed just because an Edulytics arithmetic number is correct.",
         "OPTIONAL Math Stories Stage5 Tape Diagrams: source warm-up15,25,15,16 and compensate35−19=35−20+1. Source physical tape diagram cards drive original student-authored stories and equations for partners, without inventing inaccessible card numbers. Source choice centers Target Numbers4/5, Capture Squares3/4, Five in a Row6. Separate first-party numerical examples and exit, all real participation teacher-assessed."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U02 L15 original source identity changed");
    }
    private static bool Equal(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Equal(t,true))continue;
            if(!Equal(t,false))throw new InvalidOperationException("G2 U02 L15 predecessor rich fields changed");
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
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U02 L15 persisted predecessor version invalid");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L15 persisted translation mismatch");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Equal(t,false)||Equal(t,true)))
            throw new InvalidOperationException("G2 U02 L15 persisted previous learner text changed");
    }
}

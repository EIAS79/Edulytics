using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L14 original two-step source and ordered outcomes SHA guarded.</summary>
public static class CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection
{
    public const string Version="usccss-g2-u02-l14-andre-and-group-two-step-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L14";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-14/preparation.html";
    private const string Sha="46ac29d0509475f2bb714dea783027593ff9013d26db5755917320303ec40ce5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will represent and solve one- and two-step story problems.\nIn previous lessons, you solved different problem types within 100. You made sense of story problems and connected story problems to diagrams and equations. The activities in this lesson can be used to assess the methods you use to make sense of and solve story problems on your own.",
         "Key ideas:\n- Use diagrams or equations to represent and solve one- and two-step story problems within 100\n- In previous lessons, you solved different problem types within 100\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
         "Example 1: Find the value of each expression mentally. \\(5 + 9 + 5\\) \\(25 + 9 + 5\\) \\(25 + 15 + 19\\) \\(25 + 30 + 15 + 19\\)\nApproach: “How did you choose which numbers to add first?” (I looked for ways to make a ten. I looked for ways to use sums I’ve done before. I added tens first then I tried to make tens with the ones.)\nExample 2: Andre gathered seeds. He has 25 sunflower seeds and 17 squash seeds. How many seeds does he have? Show your thinking. Andre gathered more seeds. He has 35 cucumber seeds. How many seeds does Andre have altogether? Show your thinking. Compare your thinking with your partner.\nApproach: “How does this equation represent Andre’s seeds?” (It shows all the seeds he has in one equation.) “What are other ways we could add to find the sum?” (We could add 25 and 35 first. Then add 17. We could add 17 and 35 and then add 25.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will represent and solve one- and two-step story problems. In previous lessons, you solved different problem types within 100."],
        ["IM K–5 2021 Grade 2 Unit2 Lesson14 Solve It Your Way. This source lesson introduces one- and TWO-step story problems with three addends. Warm-up Number Talk 5+9+5=19,25+9+5=39,25+15+19=59,25+30+15+19=89; learners form tens and select efficient addend orders. Activity1 Put It All Together presents Andre's story in TWO parts. First sunflower25 plus squash17 makes42. Then35 cucumber seeds join these, giving42+35=77 altogether; the single expression25+17+35=77 also matches. Activity2 Select, Solve, and Share: group of THREE students each solves one story, then cooperatively solves a separate second step. Priya has24 fewer than Tyler53 =>29. Jada and Mai have61, Jada39 =>Mai22. Kiran starts24, wins17 =>41. The group's combined total is29+22+41=92. Source section summary has Jada ?+28=65 => start37. Real group selections, student-written drawings, base-ten blocks and MLR7 gallery walk/MLR8 discussion need educator observation. Protected Jada's Seeds Cool-down not reproduced; independent Edulytics exit is separate.",
         "Some problems require TWO steps: compute intermediate quantities THEN combine them. For Andre 25+17=42, then42+35=77; a correct one-line equivalent is25+17+35=77. The associative property lets learners add25+35 first to make60, then+17=77. Priya has24 FEWER than Tyler53: Priya53−24=29. Jada and Mai total61 and Jada39: Mai61−39=22 (unknown part). Kiran24 gains17: Kiran24+17=41 (result unknown). Final three-person group sum is29+22+41=92; none of the three separate story answers alone is the group total. Number Talk values:19,39,59,89. Source summary Jada start unknown ?+28=65 means37. Draw a tape diagram for each story and label its unknown; use a new tape diagram or equation for the group combined total. Authentic partner presentations and methods are teacher-observed. Both 2.NBT.B.5 and 2.OA.A.1 are addressing standards.",
         "PUBLIC source mental expressions:5+9+5=19,25+9+5=39,25+15+19=59,25+30+15+19=89. Source Activity1 Andre:25 sunflower+17 squash=42 seeds after first part; then +35 cucumbers ->77 total;25+17+35=77. Source Activity2 Priya53−24=29, Mai61−39=22, Kiran24+17=41; independent sub-results must be added for group answer29+22+41=92. Source section summary Jada ?+28=65 -> start37. INDEPENDENT Edulytics analog first-party tasks:25+18+31=74; a child has67 stickers,17 fewer than another? If child A has67 and child B has17 fewer, child B50; a set of58 items includes26 green ->32 other; child starts29 and gains14 ->43. These new values are not copied protected Cool-down.",
         "1. Mentally find19,39,59,89 and choose addends that compose convenient tens. 2. In Andre's story use blocks, a diagram or equation to show25+17=42. A second part adds35 cucumbers to the SAME collection,42+35=77; compare to one equivalent equation25+17+35=77. 3. In real groups of three, assign source Priya, Mai and Kiran stories to three different partners. Each student selects their own correct diagram/equations; teacher monitors independent work. 4. Priya has53−24=29, including exchanging one ten if subtracting by place. 5. Mai's missing part61−39=22; show39+22=61. 6. Kiran wins17 from24 to get41,24+17=41. 7. Only after agreeing on those THREE answers does the real group combine29+22+41=92, preparing an original MLR7 Compare and Connect gallery display. 8. Source section summary unknown starting value37 satisfies37+28=65. 9. Complete distinctly authored Edulytics examples without claiming real group participation or copied restricted Jada's Seeds assessment.",
         "Stopping Andre's story at25+17=42 and forgetting the NEW 35 cucumbers; final result77. Adding only two of the three Activity2 results or using Tyler's53/Jada's39 as if they were the requested Priya/Mai quantities. Interpreting '24 fewer' as addition53+24=77 instead of Priya29. Solving Mai as39−61 (not a sensible count) rather than61−39=22. Confusing Kiran's initial24 with his ending41. Getting Priya29, Mai22, Kiran41 correct but omitting the second-step group total92. Claiming one typed calculation confirms real partner talk, a self-made MLR7 display or source teacher assessment. Original source Jada's Seeds Cool-down is restricted and not replicated.",
         "One/two-step story problems: Number Talk19,39,59,89. Andre first25+17=42 then+35=77. Priya53−24=29, Mai61−39=22, Kiran24+17=41; group sum29+22+41=92. Source Jada ?+28=65 =>37. Teacher assesses actual independent/group diagrams and MLR7 gallery work; separate first-party practice digital."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
           throw new InvalidOperationException("G2 U02 L14 source identity drifted");
    }
    private static bool Eq(CanonicalLessonContentPackTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Eq(CurriculumLessonContentTranslation t,bool after)=>
        Same([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Eq(t,true))continue;
            if(!Eq(t,false))throw new InvalidOperationException("G2 U02 L14 six predecessor fields changed");
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
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U02 L14 previous persisted version invalid");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L14 unexpected language rows");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Eq(t,false)||Eq(t,true)))
            throw new InvalidOperationException("G2 U02 L14 stored predecessor text drift");
    }
}

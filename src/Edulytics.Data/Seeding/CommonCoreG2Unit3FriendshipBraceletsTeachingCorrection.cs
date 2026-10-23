using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>IM K5 2021 G2 U03 L12 original two-step ribbon, exact SHA/predecessor guarded.</summary>
public static class CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection
{
    public const string Version="usccss-g2-u03-l12-friendship-two-step-ribbon-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L12";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-12/preparation.html";
    private const string Sha="d72cead79fd5a191a191af5bf4801296111b55633bfe608ddb195b27fb4b7adb";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will represent and solve one- and two-step story problems.\nIn previous lessons, you represented and solved one- and two-step story problems in a way that made sense to them. You have used tape diagrams to represent comparisons and part-part-whole relationships. Since you are familiar with centimeters and inches, the stories in this lesson include both.",
         "Key ideas:\n- Solve two-step story problems about length within 100\n- In previous lessons, you represented and solved one- and two-step story problems in a way that made sense to them\n- Choose a representation that matches the quantities and relationship, then use it to justify and check the result.",
         "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(24 = 10 + 14\\) \\(15 + 12 = 27\\) \\(26 = 10 + 6 + 10\\) \\(58 = 20 + 20 + 8\\)\nApproach: “How can you explain your answer for the last equation using what you know about tens and ones?” (There are only 4 tens on the right.)\nExample 2: Solve. Show your thinking. Use a diagram if it helps. Don’t forget the units. Lin found a piece of ribbon that is 92 cm long. She cut a piece for Noah that is 35 cm. How much ribbon does Lin have left? Then, Lin cut off 28 cm of ribbon for Jada. How much ribbon does Lin have left now?\nApproach: “How does each representation help you understand the problem?” (In the diagrams, you used labels to show what each part means. I can see how you used Lin's length of ribbon in both parts. In the equations, I can see the same numbers, but it's a little harder to tell what each part means. I can make sense of them by looking at the other diagrams people made.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will represent and solve one- and two-step story problems. In previous lessons, you represented and solved one- and two-step story problems in a way that made sense to them."],
        ["IM K–5 2021 Grade2 Unit3 Lesson12 Saree Silk Stories: Friendship Bracelets. This lesson follows ribbon take-from problems with TWO-STEP story problems involving lengths in BOTH centimeters and inches. Warm-up True/False place value:24=10+14 TRUE;15+12=27 TRUE;26=10+6+10 TRUE;58=20+20+8 FALSE (right side48). Activity1 Share Ribbon with Friends uses MLR6 Three Reads: Lin has92 cm of ribbon, gives Noah35 cm, then gives Jada28 cm. After first removal 92−35=57 cm; after second57−28=29 cm. The second step MUST use the new57cm amount, not the original92 again. Students solve with labeled tape diagrams, drawings or equations, and compare real partner work. Activity2 Friendship Bracelets and Gifts: Han has82 inches, only needs48 inches, so he can cut off82−48=34 inches. Clare's existing ribbon is27 inches; she receives this34-inch piece, so27+34=61 inches. Andre has28 inches but needs50, so he needs50−28=22 inches more. He gets22 inches from Mai, leaving Mai49 inches; Mai originally had49+22=71 inches, a START UNKNOWN. The two story sequences connect a calculated unknown from step1 to step2. Source cooldown Sharing Saree Silk Ribbon is educator restricted, so Edulytics's independent exit is separately authored. Genuine class discussion, physical models and source pictures require teacher observation.",
         "For a two-step story, identify what the FIRST question finds and carry that numerical answer, WITH THE SAME UNIT, into the second. Lin92cm−35cm=57cm then57cm−28cm=29cm. Han's unnecessary amount82in−48in=34in becomes a PART of Clare's final length27in+34in=61in. Andre's shortage50in−28in=22in equals the ribbon Mai gave away; since Mai now has49in LEFT, her starting length is22in+49in=71in. Never mix inches with centimeters; within each source story all measures share their unit. Tape diagrams show whole and known/unknown parts, but a two-step story might alternate subtract THEN add, or use addition to reconstruct a missing start. Warm-up58=20+20+8 is false because the RHS48. Original source addressing 2.MD.B.5 and2.NBT.B.5; earlier Building On1.NBT.B.3 remains a distinct precursor.",
         "SOURCE warm-up:24=10+14 T;15+12=27 T;26=10+6+10 T;58=20+20+8 F (48). SOURCE Activity1 Lin's92cm ribbon gives Noah35cm =>57cm, then Jada28cm =>29cm:92−35−28=29. SOURCE Activity2 Han82 inches only needs48 =>cuts34 inches; Clare already27 plus34 =>61 inches. Andre has28 inches needs50 =>22 inches extra; Mai now has49 inches after giving22 =>initial71 inches. SOURCE student section summary repeats Han64−28=36 inches as contextual review, not a new independent activity problem. FIRST-PARTY Edulytics practice: Iris has86cm, gives26cm then19cm, remains41cm; Noor has75in and needs46, extra29in; Omar has24in and receives29in, total53in. These figures are not publisher's restricted Sharing Saree Silk Ribbon Cool-down.",
         "1. Test four source warm-up equations as true, true, true, false; for58=20+20+8 note the right side has only four tens and eight ones, total48. 2. Use MLR6 Three Reads to understand Lin's ribbon situation before computing. Draw original tape diagrams with whole92cm and cut35cm, obtain57cm left; then remove28cm from THIS57cm to obtain29cm. 3. For Han, the extra82in−48in=34in is the exact piece handed to Clare. Draw Clare's length as27in+34in and compute61in. 4. For Andre, find amount needed50in−28in=22in; this is the ribbon Mai gives away. Mai's starting amount is unknown;49in is what remains, so add49+22=71in, don't subtract22 from49. 5. Label equations and tape diagrams with cm for Lin and inches for Han, Clare, Andre and Mai, never incorrectly combine different units. 6. In actual pairs discuss story parts and interpret diagrams; teacher observes student writing, actual base-ten materials and comparison of representations. 7. Complete independent Edulytics two-step alternatives, separately marked as first-party and NOT protected publisher Cool-down.",
         "Calculating Lin's second removal from92 again, i.e.92−28 rather than57−28. Dropping the unit 'cm' in Lin's story or incorrectly converting to inches; no unit conversion is needed. Using the extra34 inches Han cuts as if Clare should subtract it; she RECEIVES34 and adds to27 to make61. Saying Andre needs50+28=78 rather than50−28=22. Treating Mai's49 remaining as starting length and subtracting22 to get27; correct START49+22=71. Misclassifying source58=20+20+8 as true (right side48). Pretending authentic student diagrams, ribbons or MLR6 discussion are proven by digital numerical input. Misidentifying invented first-party examples as publisher's restricted Sharing Saree Silk Ribbon cooldown.",
         "Two-step ribbon stories: Lin92cm→57cm after giving35→29cm after giving28; Han82in needs48in, gives excess34in to Clare, whose27in becomes61in; Andre28in needs50in so needs22in from Mai, who now has49in and therefore started with71in. Warm-up T,T,T,F. Carry step1 answer and correct unit into step2; source pair-work teacher reviewed."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.5","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U03 L12 source identity/outcomes changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U03 L12 source predecessor fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];t.WorkedExamples=S.After[2];
            t.StepByStepSolutions=S.After[3];t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L12 old persisted version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L12 persisted language rows changed");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U03 L12 old learner fields drifted");
    }
}

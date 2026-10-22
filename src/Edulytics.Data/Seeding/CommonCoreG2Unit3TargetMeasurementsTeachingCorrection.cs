using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally archived IM2021 G2 U03 L13 Target Measurements and Number Puzzles,
/// source identity + original six-field predecessor guard; no counterfeit center completion.</summary>
public static class CommonCoreG2Unit3TargetMeasurementsTeachingCorrection
{
    public const string Version="usccss-g2-u03-l13-target-measurements-stage1-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L13";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-13/preparation.html";
    private const string Sha="5fbacc4927f2d2e2ed57f65965b4ffd2503752d6bd762d2cc24759a0ae14cfa6";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will practice estimating and measuring lengths and adding and subtracting within 100.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In Activity 1, you learn a new center, Target Measurements. In this center, you practice estimating and measuring length in inches and centimeters.",
         "Key ideas:\n- Determine the unknown number that makes an equation true\n- Estimate and measure objects\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(58 + 10\\) \\(58 + 12\\) \\(58 + 13\\) \\(67 + 14\\)\nApproach: “How can you make a ten to find the value of \\(67 + 14\\) ?” (67 needs 3 more to make 10. That 3 can come from the 4 ones in 14.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will practice estimating and measuring lengths and adding and subtracting within 100. This lesson is optional because it is an opportunity for extra practice that not all classes may need."],
        ["Grade 2 Unit 3 Lesson 13 Center Day 2 (optional), original IM K–5 2021. Warm-up Number Talk: 58+10=68,58+12=70,58+13=71,67+14=81; to make a new ten in67+14, split14 into3+11 and first combine67+3=70. Activity 1 introduces the authentic Target Measurements Stage1: Inches and Centimeters, a TWO-PARTNER straightedge line-drawing challenge. On each turn the drawer picks a target length, no more than 10 inches OR 30 centimeters, and draws a line. The other partner tells the drawer when to stop. BOTH then measure the length with the appropriate physical ruler and record target, actual and absolute difference; this difference is the score of the partner who SAID STOP. Partners alternate for 8 rounds, and lowest accumulated score wins. Activity 2 introduces Number Puzzles Stage4: Within 100 with Composing. Two partners place REAL printed digit cards into the equations on original gameboards so both addition and subtraction within100 are true, including making and unmaking a ten. After introduction, students may choose Number Puzzles, Estimate and Measure Stage2 or Target Measurements Stage1. Digital self-checks provide independently-authored example measurements and missing-number questions, not counterfeit evidence of actual drawings, card play, scores or partner participation.",
         "A ruler's zero mark must align with one endpoint; use matching units and avoid gaps or overlaps. Inches and centimeters are NOT interchangeable as identical numbers. Stage1 TARGET LIMITS: 1–10 whole inches OR 1–30 whole centimeters. Each round, one partner draws, a different partner decides STOP, and their score is the ABSOLUTE difference |actual−target| in the selected SAME unit; e.g. target12 cm, measured15 cm, score3 cm; target5 inches, measured7 inches, score2 inches. After eight real alternating rounds, lowest cumulative score wins. Original records are teacher-checked; do not infer game participation from a typed difference. Number Puzzles Stage4 uses actual digit cards and authentic printed equations. Composing when adding:67+14=81 because7+4=11 ones; decomposing when subtracting:62−27=35 because62=50+12. To complete a first-party numeric equation, compute unknown from the inverse operation. Required source outcomes 2.MD.A.1,2.MD.A.3,2.MD.A.4,2.NBT.B.5,2.OA.B.2.",
         "PUBLISHER public warm-up58+10=68;58+12=70;58+13=71;67+14=81. PUBLIC Target Measurements Stage1 rules: choose targets up to10 inches or30 cm, partner says stop, measure, difference is that STOP partner's score, exchange drawing and judging roles for8 rounds, lowest score wins. FIRST-PARTY demonstration only, not original source game: selected target12 cm, actual15 cm -> score|15−12|=3 cm; second target6 inches and actual4 inches ->2 inches. Source Number Puzzles Stage4 printed card arithmetic is unavailable as verified numbered puzzles; FIRST-PARTY companion equation 48+?=73 ->25 (with regrouping), and64−?=38 ->26 (borrowing). Edulytics independent exit: target18 cm and actual21 cm ->3 cm; target7 inches and actual5 inches ->2 inches;67+14=81.",
         "1. Number Talk68,70,71,81; explain place-value ones and forming a new ten. 2. In authentic partner Target Measurements Stage1 select SAME-unit target, at most10 whole inches OR30 whole centimeters. 3. DRAW a physical straightedge segment until the other partner tells you to stop; the other partner must not draw. 4. Measure the real line from the zero mark with the correct-inch or centimeter ruler, enter measured length and compare it to the target in the SAME unit. 5. The absolute difference is the score for the partner who called stop; switch roles. After 8 real rounds sum each partner's scores separately, lowest wins under teacher observation. 6. In genuine Number Puzzles Stage4 choose digits from the printed source card set to make each expression equation true within100, including compose/decompose when needed. Real partner check and actual board values remain teacher-reviewed. 7. Choice time: continue authentic Number Puzzles, Estimate and Measure Stage2 or Target Measurements Stage1. 8. Answer separately authored Edulytics numeric examples without claiming that the digital entry proves actual source center participation.",
         "Choosing a target of11 inches or31 cm (out of source limits); using the ruler's physical edge rather than the zero marking. Comparing an inch target to a centimeter measurement numerically without converting. Subtracting target from actual without taking absolute value when actual is shorter. Recording the difference for the DRAWER rather than the partner who said stop. Declaring a winner without eight authentic rounds and accumulated scores. Treating Number Puzzles Stage4 as addition without composing a ten or assuming source digit card numbers that have not been read. Incorrect Number Talk58+12=60 instead of70 or67+14=71 instead of81. Typed arithmetic does NOT document physical draw-stop-measure gameplay.",
         "Optional Center Day2: warm-up58+10=68,58+12=70,58+13=71,67+14=81. Target Measurements Stage1 draw a straight line toward a chosen target up to10 inches or30 cm, partner says stop, measure in same unit and score absolute difference for STOP partner. Switch roles for8 real rounds, lowest total wins. Number Puzzles Stage4 uses authentic digit cards and addition/subtraction within100 composing/decomposing a ten. First-party numeric checks do not certify genuine partner centers."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3","CCSS:2.MD.A.4","CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U03 L13 original source identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] values,bool after)=>values.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L13 original six learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L13 unauthorized stored version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U03 L13 stored translation set changed");
        var previous=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(previous.Title!=incoming.Title||!(Same(previous,false)||Same(previous,true)))
            throw new InvalidOperationException("G2 U03 L13 stored learner rich text drifted");
    }
}

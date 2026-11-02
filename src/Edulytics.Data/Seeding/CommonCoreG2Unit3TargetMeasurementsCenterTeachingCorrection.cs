using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Original 2021 G2 U03 L13 read only from local teacher lesson.
/// Guard source SHA, previous six learner fields and authorized predecessor.</summary>
public static class CommonCoreG2Unit3TargetMeasurementsCenterTeachingCorrection
{
    public const string Version="usccss-g2-u03-l13-target-measurements-and-puzzles-v1";
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
        ["Grade 2 Unit 3 Lesson 13 Center Day 2 (optional), IM K–5 2021. Warm-up Number Talk, Use Ten to Add: 58+10=68,58+12=70,58+13=71,67+14=81; decompose addends to compose the next ten. Activity 1 Target Measurements Stage 1 is a REAL two-person center using centimeter and inch rulers and a straightedge. One partner chooses a target WHOLE length in inches up to 10 inches OR centimeters up to 30 cm and draws a line. The other says STOP when they think the length is close to the target. Both measure the actual line and record target, measured length and absolute difference (error); the stopping partner receives the error as their score. Switch roles over EIGHT rounds; add scores, and the partner with the LOWEST cumulative error wins. Activity 2 introduces Number Puzzles Stage 4 Within 100 with Composing: pupils work in pairs with the authentic digit cards and gameboard to make addition/subtraction equations with missing values true, composing or decomposing a ten as needed. They then choose to continue Number Puzzles or work in Estimate and Measure Stage 2 or Target Measurements Stage 1. Authentic ruler measurements, paper gameboards, cards and peer turn-taking require teacher observation. All numeric digital examples are separately authored by Edulytics and cannot certify physical drawing, eight rounds or a real winner.",
         "Target Measurements Stage1: choose one target length IN WHOLE INCHES no longer than10 inches, or IN WHOLE CENTIMETERS no longer than30 cm; keep target and measured values in the SAME unit. Draw a straight segment with a straightedge, tell partner when to stop, measure from the ruler's ZERO mark, and find ABSOLUTE ERROR |actual−target|, always nonnegative. The partner saying stop takes that error score. Over eight authentic rounds, total points for each partner; smaller total wins, not larger. One digital integer example cannot substantiate real stop timing, partner role or winner. Number Puzzles Stage4: place digit cards into the actual missing-number board, choose digits so each equation is true, include addition composing a ten and subtraction decomposing a ten within100. Example Edulytics invented: 38+27=65 (ones8+7=15 compose one ten); 65−27=38 (ones5<7 decompose). The actual source blackline-master number puzzle grid and its exact entries are not reconstructed from missing print materials. Official five Addressing codes: 2.MD.A.1, 2.MD.A.3, 2.MD.A.4, 2.NBT.B.5 and2.OA.B.2.",
         "PUBLISHED SOURCE warm-up 58+10=68;58+12=70;58+13=71;67+14=81. For67+14, use three of the four ones in14 to make70, then add11 more ->81. ORIGINAL SOURCE center rules: Target Measurements Stage1, a pair chooses target WHOLE inches <=10 or centimeters <=30, draws and measures each turn, computes difference and accumulates EIGHT-round error scores; actual source uses real rulers, drawing and stop signal. Number Puzzles Stage4 pupils use real digit cards and the original board; specific printed equations not visible in locally archived teacher lesson. FIRST-PARTY Edulytics digital example (NOT an actual source line): target7cm, actually9cm, difference2cm; target4in, actually6in, difference2in. Independently authored missing-value digital checks:38+?=65 has27, and65−?=38 has27. Independent exit67+14=81, target8cm drawn6cm difference2cm and63−27=36.",
         "1. Compute source warm-up68,70,71,81 mentally; explain making a ten first:67 needs3, so14=3+11 and67+14=70+11=81. 2. Work with an ACTUAL partner on Target Measurements Stage1; select whole-inch target1..10 or whole-centimeter target1..30, name unit and start from a genuine ruler's zero mark. 3. One learner draws a straight segment while the other tells them to stop; BOTH use a ruler to check actual segment length. 4. Record target, actual and absolute difference in SAME unit. The student who said STOP receives the error score; then trade drawing and stopping roles. 5. After eight real rounds, sum each player's errors; lowest score is winner, teacher checks genuine scores and turns. 6. In Number Puzzles Stage4 use actual published digit cards on original board to fill missing numbers so all equations are true; if 38+27=65 compose a ten and if65−27=38 decompose one. These are only invented arithmetic examples, NOT claims about printed board entries. 7. Choose real centers from Number Puzzles, Estimate and Measure or Target Measurements. 8. Complete separate digital first-party measurement and digit checks; teachers review authentic ruler placement, straight lines, card legality and partner talk.",
         "Allowing an inch target above10 or centimeter target above30. Comparing seven CENTIMETERS with nine INCHES as if their numerical difference2 represented a valid physical error. Measuring from ruler end rather than the printed ZERO or leaving gaps. Using measured minus target with a negative score instead of absolute nonnegative difference. Awarding the score to the student who DREW rather than the student who said STOP. Treating bigger total error as the winner, or claiming eight rounds played because one online number is correct. Mistaking Number Puzzles Stage4 for the earlier Target Numbers card game or ignoring ten composition/decomposition in missing-number equations. Inventing specific original blackline-master cards or measuring actual physical objects without direct observation is unjustified.",
         "Optional G2 U03 L13: Number Talk58+10=68,58+12=70,58+13=71,67+14=81. New real center Target Measurements Stage1: pairs draw line segments against whole-inch target up to10in or whole-cm up to30cm, take absolute measurement error as STOP partner's score, and after eight rounds lowest total wins. Number Puzzles Stage4 uses genuine digit cards and original equations within100 requiring new tens/exchanges, then center choice. First-party digital arithmetic and errors are not evidence of classroom play."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.3","CCSS:2.MD.A.4","CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U03 L13 original source identity drifted");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool newer)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Same(CurriculumLessonContentTranslation t,bool newer)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Equal(string[] a,bool newer)=>a.SequenceEqual(newer?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L13 prior six fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L13 persisted predecessor invalid");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L13 persisted language rows drifted");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L13 previous persisted learner fields drifted");
    }
}

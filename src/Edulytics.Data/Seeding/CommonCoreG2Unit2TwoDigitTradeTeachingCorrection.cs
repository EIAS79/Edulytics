using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L07 archival IM2021 content/source guarded; never invent restricted card quantities.</summary>
public static class CommonCoreG2Unit2TwoDigitTradeTeachingCorrection
{
    public const string Version="usccss-g2-u02-l07-real-cards-and-two-digit-trade-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L07";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-7/preparation.html";
    private const string Sha="6d246684c5e242e12b86473c71f243dfd45405df937540dc65105fc8f01ddf10";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will subtract a two-digit number from a two-digit number when a ten is decomposed when subtracting by place.\nIn previous lessons, you learned that decomposing a ten is sometimes necessary when subtracting two numbers. You used connecting cubes and base-ten blocks to represent your methods when subtracting a one-digit number from a two-digit number. In the fist activity, you use methods that make sense to them to subtract and compare your methods with a partner.",
         "Key ideas:\n- Subtract a two-digit number from a two-digit number in a way that makes sense to them\n- In previous lessons, you learned that decomposing a ten is sometimes necessary when subtracting two numbers\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: How many do you see? How do you see them?\nApproach: “Which two images have the same value? How do you know?”(The first image and the second image both have the same amount. The first one shows 5 tens and 5 ones. I know Image 2 is the same because I know 2 groups of 5 is the same as 1 ten. So it's the same as 5 tens and 5 ones.)\nExample 2: Find the value of each difference. Show your thinking. Use blocks if it helps. \\(46 - 28 =\\underline{\\phantom{\\hspace{1.05cm}}}\\) \\(93 - 54 =\\underline{\\phantom{\\hspace{1.05cm}}}\\)\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will subtract a two-digit number from a two-digit number when a ten is decomposed when subtracting by place. In previous lessons, you learned that decomposing a ten is sometimes necessary when subtracting two numbers."],
        ["Original IM K–5 2021 G2 U02 L07, Subtract Two Digits: In the 10-minute warm-up How Many Do You See? students compare diagrams for 55; 5 tens and 5 ones matches a grouping using two groups of 5 to compose a ten. In Activity 1 What's the Difference?, students independently calculate 46−28=18 and 93−54=39, then use MLR8 discussion supports to compare partner strategies and models. Both require decomposing one ten if subtracting by place value. In Activity 2 Use Blocks to Take Away (20 min), groups of four choose authentic player cards Diego, Lin, Jada, Han, represent starting amounts with real base-ten blocks, follow shuffled action cards, write two equations for each player's changing amount, and add final amounts. Public local teacher lesson does not supply the numerical values on the printed blackline cards, so Edulytics does not invent them as publisher tasks. Physical blocks and genuine peer interaction require teacher observation. An independent digital mathematical companion and separately authored Cool-down give verifiable arithmetic without claiming actual card play.",
         "Two-digit subtraction with exchanging a ten: if subtracting by place value and the starting ones are insufficient, replace one ten with ten ones while preserving the same total. 46=4 tens+6 ones=3 tens+16 ones, so 46−28: after removing two tens and eight ones, one ten and eight ones remain, or 18. 93=9 tens+3 ones=8 tens+13 ones; remove five tens and four ones to obtain three tens and nine ones, or 39. Always check using inverse addition 18+28=46 and 39+54=93. Number-line subtraction, adding up and counting back are also valid if explained. The original warm-up uses two equal representations of 55 involving groups of five and ten. In the authentic four-player game, each player changes numbers twice on printed action cards, then the group adds the four ending values. The actual printed action-card values are not provided by the archived HTML; do not assert or simulate those as real source cards. Addressing standards in this lesson: 2.NBT.B.5, 2.NBT.B.6, 2.NBT.B.9.",
         "SOURCE warm-up: the first two teacher diagrams each show 55 (five tens and five ones, or an equivalent grouping). SOURCE Activity 1: 46−28=18, with 46=30+16 and 30−20 +16−8=18; 93−54=39, with 93=80+13 and 80−50 +13−4=39. SOURCE Activity 2: real group card game for Diego, Lin, Jada and Han; each genuine player card supplies its own unpublished starting amount and actions. Independent Edulytics example, NOT a source game card: 64−27=37 because 64=50+14; independently written 72−35=37 because 72=60+12. Independent Edulytics exit: 73−46=27, 85−57=28 and 62−34=28.",
         "1. Warm-up: inspect authentic teacher diagrams, notice that two groups of five single objects make one ten, and explain why two representations show 55; teacher checks visual discourse. 2. For 46−28, represent four tens and six ones using real blocks or a diagram, trade one ten for ten ones to obtain three tens and sixteen ones, remove two tens and eight ones, and count 18. 3. For 93−54, trade one of nine tens to create eight tens and thirteen ones, remove five tens and four ones, leaving 39. 4. Discuss a different correct strategy with a partner; use the MLR8 sentence frames, then confirm with addition. 5. Activity 2 is a REAL group-of-four center. Choose original Diego/Lin/Jada/Han player cards; use each card's actual starting number and two action cards and represent the changes with base-ten blocks. Record each equation after a change. 6. After the second change, write the authentic ending number as tens and ones, and add all four true ending amounts, explaining any tens composition. Teacher reviews authenticity of cards, group cooperation, actions and totals. 7. Separately try first-party Edulytics numerical examples and exit tasks; these are NOT the source's protected printed cool-down or player cards.",
         "Subtracting eight ones directly from six without ten decomposition (unless a valid alternate method is used). Trading ten but retaining its value in the tens place double-counts ten. Writing 46−28=28 or 93−54=49 instead of 18 and 39. Confusing subtracting tens with subtracting ones. Treating two valid student strategies as mathematically conflicting merely because the representations differ. Pretending digitally invented numbers were present on Diego, Lin, Jada or Han's original cards; their specific printed contents were not available in the locally archived public teacher HTML. Crediting a child with authentic pair or group participation on the basis of numeric input alone is unjustified.",
         "G2 U02 L07: warm-up equivalent diagrams of 55; Activity 1 original 46−28=18 and 93−54=39, regroup a ten and compare student methods; Activity 2 uses actual four-person Diego/Lin/Jada/Han cards and physical blocks, with teacher-observed two changes and a group ending sum. First-party digital 64−27=37 and 72−35=37 are mathematical illustrations, not publisher cards. Independent exit 73−46=27,85−57=28,62−34=28."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"
        &&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.NBT.B.6","CCSS:2.NBT.B.9"}))
            throw new InvalidOperationException("G2 U02 L07 source identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U02 L07 predecessor fields drifted");
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
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U02 L07 previous version unexpected");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L07 previous translations unexpected");
        var old=rows.Single();var expected=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=expected.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 U02 L07 persisted learner field drift");
    }
}

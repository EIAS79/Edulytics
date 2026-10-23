using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U02 L05 sourced from locally archived IM2021 teacher lesson:
/// guarded original body, exact predecessor, outcomes and URL.</summary>
public static class CommonCoreG2Unit2DecomposeTenTeachingCorrection
{
    public const string Version="us-ccss-g2-u02-l05-82-minus9-diego-55-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L05";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-5/preparation.html";
    private const string Sha="6a15a2ca3d216848fde68e654d824e9f4babd0bfef66cb6ca08557292a10d0a5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will subtract a one-digit number from a two-digit number and describe decomposing a ten when subtracting by place.\nIn grade 1, you added within 100 using strategies based on place value and properties of operations. When using place value strategies to add, you learned that sometimes you need to compose a ten. To this point, when subtracting within 20, you used methods like counting on, decomposing a number to get to a ten, and using known addition facts, but did not explicitly decompose a ten.",
         "Key ideas:\n- Subtract a one-digit number from a two-digit number in a way that makes sense to them\n- In grade 1, you added within 100 using strategies based on place value and properties of operations\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Find the value of each expression mentally. \\(17 - 7\\) \\(17 - 8\\) \\(26 - 6\\) \\(26 - 8\\)\nApproach: “How could you use the third expression to help you find the difference of the last expression?” (I know that taking away 6 gets me to 20, then I just took 2 more away.)\nExample 2: Find the value of \\(82 - 9\\) . Show your thinking. Use blocks if it helps.\nApproach: “Why did ___ need to change 82 to 7 tens and 12 ones to subtract ones from ones?” (There are too many ones to take away, so she traded a ten for 10 ones to show decomposing a ten.)",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will subtract a one-digit number from a two-digit number and describe decomposing a ten when subtracting by place. In grade 1, you added within 100 using strategies based on place value and properties of operations."],
        ["Subtract Your Way — Grade 2 Unit 2 Lesson 5, IM K–5 2021. Warm-up Number Talk: compute 17−7=10, 17−8=9, 26−6=20 and 26−8=18 mentally, noticing that getting to a ten makes subtracting the remaining ones easier. Activity 1 How Do You Find the Value?: solve original 82−9=73 in a way that makes sense, with connecting cubes, a counting-back strategy, or base-ten blocks. Explain the crucial exchange: 82 equals 8 tens and 2 ones AND also 7 tens and 12 ones; 12−9=3 ones leaves 7 tens and 3 ones =73. Activity 2 Subtract with Base-ten Blocks: Diego starts with 5 tens and 5 ones =55, takes away 2 tens to get 35, then takes away 8 ones. Exchange one of the remaining three tens for 10 ones: 35 =2 tens and 15 ones; 15−8=7 leaves 27. Synthesis uses 35−8=27 and 35=20+15. Actual physical manipulation, learner explanations, class discussion (5 Practices and MLR8) require teacher review; digital arithmetic can be checked separately.",
         "A ten-rod is worth 10 single units. In subtraction within100, if the ones digit of the starting amount is smaller than the ones to be subtracted, taking away by place requires exchanging ONE ten for TEN ones WITHOUT changing the whole value: 82=80+2=70+12; 82−9=70+(12−9)=73. Count-back is also valid: 82−2=80 then80−7=73. With Diego, 55−20=35; 35=30+5=20+15, so35−8=20+7=27. It is WRONG to leave the traded ten in both places, which would add ten without reason. Explain the difference between connecting cubes (tower physically split) and fixed base-ten rods (swap one rod for ten singles). Original Addressing outcome codes are CCSS 2.NBT.B.5 and 2.OA.B.2. Digital tasks cannot substantiate authentic classroom pair work or teacher observations.",
         "SOURCE Number Talk: 17−7=10;17−8=9;26−6=20;26−8=18. Use26−6=20, then20−2=18. SOURCE Activity1:82−9=73. Show 8 tens+2 ones then trade to7 tens+12 ones and subtract9 of12 ones; another valid method is82−2−7=73. SOURCE Activity2: Diego has55; removing two tens yields35, then removing eight ones requires decomposing 35 into2 tens+15 ones, giving27. SYNTHESIS: 35−8=27 since35=20+15 and15−8=7. Independent Edulytics examples: 63−7=56 (63=50+13),74−8=66 (74=60+14),42−6=36 (42=30+12); these are not the publisher's restricted Cool-down.",
         "1. Solve the original four mental Number Talk expressions. Discuss how17−8 uses17−7 then subtracts one more, and26−8 uses26−6 then subtracts two more. 2. For82−9, show 82 with real connecting cubes or base-ten blocks. Describe either counting back to80 and then73, or trading one ten:8 tens+2 ones ->7 tens+12 ones. 3. Take nine of the twelve singles; count 7 remaining tens and3 singles =73. Verify73+9=82. 4. With a partner use ACTUAL blocks for Diego's55 (five tens and five ones), take away two tens to reach35. 5. To take away eight ones, trade one remaining ten for ten ones:35=2 tens+15 ones. Remove eight and find27. Verify27+8+20=55. 6. Discuss how base-ten blocks and breakable cube towers differ when trading and why the total does not change during the exchange. 7. Explain the source synthesis35=20+15 and35−8=27 using an original learner drawing; actual partner work and verbal reasoning are assessed by a teacher. 8. Complete separate original Edulytics Cool-down problems involving decomposing a ten, without claiming to reproduce the educator-only assessment.",
         "Subtracting 9 from only2 ones in82−9 without exchanging or using an alternative mathematically valid count-back strategy. Writing82−9=83 or72 instead of73. Trading a ten but keeping all eight tens as well as twelve singles gives92 rather than82; exchanging must PRESERVE the total. For Diego, writing55−20−8=37 (correct27), or interpreting 5 tens and5 ones as10. After55−20=35, it is not possible to subtract8 separate ones from5 without exchanging one ten or selecting a valid alternate strategy. Confusing original source mathematical examples with teacher-restricted response sheets and the Cool-down. Marking written digital answers as proof a student actually manipulated cubes, drew original diagrams or spoke with peers is unjustified.",
         "Subtract a 1-digit number from a 2-digit number by thinking about place value and trading one ten for ten ones when needed. Warm-up 17−7=10,17−8=9,26−6=20,26−8=18. Source82−9=73 using82=70+12; Diego55−20=35 and35−8=27 using35=20+15. Real manipulatives and discussion need teacher review; independently authored digital checks assess numerical skills."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U02 L05 original source or ordered outcomes drifted");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U02 L05 previous six fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)?Version:prev;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev)=>
        IsTarget(d,l)&&prev==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prev,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prev))throw new InvalidOperationException("G2 U02 L05 old persisted version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L05 unexpected persisted language rows");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U02 L05 persisted learner fields changed");
    }
}

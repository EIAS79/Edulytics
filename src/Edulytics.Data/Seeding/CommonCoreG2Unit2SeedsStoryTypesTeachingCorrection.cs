using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>2021 G2 U02 L11 seed story 4 structures with original source guards.</summary>
public static class CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection
{
    public const string Version="usccss-g2-u02-l11-seed-stories-four-unknown-positions-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L11";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-11/preparation.html";
    private const string Sha="14eaf3af53e16402569aa86a7047665640dce646e3489fd7a938d30c5fc77730";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will represent and solve story problems involving addition and subtraction within 50 that require composing or decomposing a ten when adding or subtracting by place.\nIn previous lessons, you interpreted and solved story problems within 100 that did not require decomposing a ten when subtracting by place. You interpreted diagrams and equations with unknowns and connected them to story problems. In this lesson, you are encouraged to use the methods and representations that make the most sense to them as you solve problems of different types.",
         "Key ideas:\n- Represent and solve story problems within 50 in a way that makes sense to them\n- In previous lessons, you interpreted and solved story problems within 100 that did not require decomposing a ten when subtracting by place\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Diego gathered 42 orange seeds. Jada gathered 16 apple seeds. How many more seeds did Diego gather than Jada? Show your thinking.\nApproach: “How are the diagrams the same? How are you different?” (The diagrams all show Diego’s and Jada’s seeds. Some diagrams show the problem and help you see the bigger amount, the smaller amount, and the difference. Some diagrams show how we added and subtracted tens and ones.)\nExample 2: Solve each story problem. Show your thinking. Lin had 31 sunflower seeds. She gave Priya 15 seeds. How many seeds does Lin have now? Noah used yellow and blue corn seeds to make a design. He used 37 seeds altogether. He used 28 yellow seeds. How many blue seeds did he use? Elena gathered 50 pumpkin seeds. Andre collected 23 fewer pumpkin seeds than...\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will represent and solve story problems involving addition and subtraction within 50 that require composing or decomposing a ten when adding or subtracting by place. In previous lessons, you interpreted and solved story problems within 100 that did not require decomposing a ten when subtracting by place."],
        ["Grade2 Unit2 Lesson11 How Do You Solve Story Problems? (IM K–5 2021) focuses on four distinct story structures within 50, sometimes requiring decomposing a ten when solving by place. Warm-up What Do You Know About Story Problems? elicits genuine student representations, unknowns, drawings, number sentences and methods. Activity1 How Many Seeds?: Diego has42 orange seeds, Jada16 apple seeds, so Diego gathered26 MORE, a Compare Difference Unknown structure. Learners choose actual connecting cubes, base-ten blocks, tape diagrams or equations and explain42−16=26. Activity2 The Seeds of Greatness: (1) Lin begins31 sunflower seeds, gives Priya15, so16 remain, a Take From Result Unknown; (2) Noah uses37 corn seeds total and28 yellow, so9 blue, a Put Together Part Unknown; (3) Elena gathers50 pumpkin seeds, Andre collected23 FEWER, so Andre has27, a Compare Smaller Unknown. MLR2 vocabulary and 5 Practices compare student-chosen ways to show relationships versus computation. Real manipulatives and original learner/partner representations need teacher. The source Cool-down Tyler's Seeds is educator-login restricted, so independent Edulytics exit is not represented as its original content.",
         "First identify the UNKNOWN and relationship; do NOT reflexively add both numbers just because both are in the story. Compare Difference Unknown: larger42−smaller16=26 more (42=30+12;30−10=20,12−6=6). Take From Result Unknown:31−15=16 (31=20+11). Put Together Part Unknown:37 total−28 known yellow=9 blue (37=20+17), or28+?=37. Compare Smaller Unknown: Elena50, Andre23 fewer, so50−23=27 (50=40+10,40−20=20,10−3=7). Tape diagrams can show which whole/part or larger/smaller amount is unknown even when they do not themselves show borrowing; base-ten block diagrams can show decomposition to compute. Check with inverse sums26+16=42,16+15=31,9+28=37,27+23=50. All original source numbers are within50. Addressing CCSS 2.NBT.B.5 and 2.OA.A.1.",
         "SOURCE seeds: Diego42 vs Jada16 =>42−16=26 more. SOURCE Lin31 gives Priya15 =>31−15=16 left. SOURCE Noah yellow28 and total37 =>blue37−28=9. SOURCE Elena50 vs Andre23 fewer =>Andre50−23=27. Explain which quantity represents whole, part or comparison gap; for Diego/Jada, answer26 not58 (a sum would answer a different question). SOURCE warm-up open question: What do you know about story problems? No fixed numeric correct response. Independent Edulytics-original story extra checks (not publisher Cool-down):44−18=26 extra buttons,32−17=15 remaining snacks,41−26=15 missing part,50−28=22 fewer team points.",
         "1. Discuss warm-up story structures, what can be unknown, and which diagrams match contexts; teacher records real students' strategies. 2. Read Activity1 exactly: Diego42 orange seeds, Jada16 apple seeds, ask HOW MANY MORE. Label bigger/smaller and gap on a tape diagram, or build source amounts with REAL cubes. 3. Solve42−16=26 by trading42=30+12; take one ten and six singles to leave26, or count up16→42 by26. Verify16+26=42. 4. For Activity2 Lin's Take From story,31 sunflower−15 given=16 remain. 5. For Noah's part-unknown story,37 yellow-and-blue total,28 yellow, blue=9; model known part28 and missing part9. 6. For Elena/Andre's Compare Smaller Unknown story,50 pumpkin seeds for Elena, Andre has23 fewer, so Andre has27. A diagram must locate the smaller unknown, not the 23 difference as the smaller quantity. 7. Discuss drawings, tape diagrams, blocks and equations with a genuine partner under MLR2 and 5 Practices. Teacher evaluates selected methods and whether story structures are correctly interpreted. 8. Complete separate Edulytics-original exit numbers without attributing them to source restricted Tyler's Seeds assessment.",
         "Adding42+16 to get58 for Diego's how-many-more question (correct26), or calling16 the difference. Mistaking Lin's31−15 as31+15, when15 were GIVEN AWAY; result16. Finding37+28=65 for blue seeds rather than37−28=9, or interpreting yellow28 as the total. Reporting Elena50 minus23 fewer as73 (must be27). Assuming every story type uses identical drawing or gives the same role to the same numeric position. Exchanging one ten into ten singles while also keeping that ten double-counts. Marking typed numbers as proof that actual student-drawn diagrams, physical blocks, MLR2 conversation or restricted publisher Cool-down were completed is invalid.",
         "Four distinct real source seed stories within50: Compare gap42−16=26; Take From result31−15=16; Put Together missing part37−28=9; Compare smaller unknown50−23=27. Each can require a tens trade, and the representation must match the question. Teacher observes real blocks/MLR2 discussion; independently authored Edulytics exit is separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L11 source drift");
    }
    private static bool IsSame(CanonicalLessonContentPackTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool IsSame(CurriculumLessonContentTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(IsSame(en,true))continue;
            if(!IsSame(en,false))throw new InvalidOperationException("G2 U02 L11 original six fields changed");
            en.Explanation=S.After[0];en.KeyConceptsAndRules=S.After[1];
            en.WorkedExamples=S.After[2];en.StepByStepSolutions=S.After[3];
            en.CommonMistakes=S.After[4];en.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)?Version:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U02 L11 persisted version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L11 translation rows mismatch");
        var priorRow=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(priorRow.Title!=incoming.Title||!(IsSame(priorRow,false)||IsSame(priorRow,true)))
            throw new InvalidOperationException("G2 U02 L11 persisted six fields drift");
    }
}

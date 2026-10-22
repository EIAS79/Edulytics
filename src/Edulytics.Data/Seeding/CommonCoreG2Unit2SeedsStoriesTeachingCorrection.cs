using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally archived IM2021 G2 U02 L11 four distinct seeds story structures.</summary>
public static class CommonCoreG2Unit2SeedsStoriesTeachingCorrection
{
    public const string Version="usccss-g2-u02-l11-four-seed-story-types-v1";
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
        ["IM K–5 2021 Grade 2 Unit2 Lesson11 How Do You Solve Story Problems? Warm-up What Do You Know About Story Problems? asks learners to identify the situation, what quantity is unknown and how objects, tape diagrams, base-ten blocks and equations help. Activity1 How Many Seeds? Compare, Difference Unknown: Diego gathers42 orange seeds and Jada16 apple seeds. How many MORE does Diego gather? 42−16=26, not42+16. When subtracting by place value42=30+12, take away10+6 to get26. Activity2 The Seeds of Greatness includes three DISTINCT situations: Lin has31 sunflower seeds and gives15 to Priya, so31−15=16 left (Take From, Result Unknown); Noah makes a37-seed design including28 yellow, so37−28=9 blue (Put Together, Addend Unknown); Elena gathers50 pumpkins and Andre23 FEWER, so50−23=27 for Andre (Compare, Smaller Unknown). The source synthesis compares tape diagram for relationships, base-ten diagrams for calculation and real student-created representations. Partner MLR2 vocabulary and physical blocks require teacher review. Source cool-down Tyler's Seeds requires educator login; an independent Edulytics exit must be clearly separate.",
         "Choose the equation to match the STORY structure before calculating. 42 orange versus16 apple asks how many MORE, so difference42−16=26; adding42+16=58 does not answer comparison. Lin gives away15 from31:31−15=16 left. Noah has37 altogether,28 yellow and ? blue:28+?=37, unknown9; subtract37−28=9. Elena collected50, Andre collected23 FEWER: Andre=50−23=27. Every example stays within50 and may require composing or decomposing a ten by place value. A tape diagram makes relationships (whole/parts, larger/smaller/difference) explicit; a base-ten drawing also shows calculation. For42−16 regroup42=3 tens+12 ones, remove1 ten+6 ones leaves2 tens+6 ones; for50−23 regroup50=4 tens+10 ones, remove2 tens+3 ones leaves2 tens+7 ones. A matching addition fact checks subtraction. Both CCSS 2.NBT.B.5 and2.OA.A.1 apply.",
         "PUBLISHER PUBLIC SOURCE Activity1: Diego42 orange, Jada16 apple, how many more?42−16=26; teacher looks for compare bars, cubes, tape diagram and base-ten approaches. SOURCE Activity2: Lin31 gives Priya15 ->16; Noah37 total,28 yellow ->9 blue; Elena50, Andre23 fewer ->27. Elena/Andre is Compare Smaller Unknown, NOT additive. Original independent Edulytics practice only: Rita has44 stones and Eli18, how many MORE?44−18=26. Sara has33 leaves, gives17, how many LEFT?33−17=16. A collage totals45 shapes,27 circles, how many triangles?45−27=18. Different first-party exit:46−19=27 and40−16=24; no claim that these are the protected Tyler's Seeds assessment.",
         "1. Warm-up ask what a story's action is, whether it asks for total, missing part, remaining or a compare gap. 2. Compare Diego42 and Jada16. Draw two bars or line up authentic connecting cubes to see the26-seed gap. 3. If calculating by place:42=30+12, subtract one ten and six ones, result26; check16+26=42. 4. Lin's story says gave away15 from31; show original31, cross out15, result16. 5. Noah's37-seed design has28 yellow and an unknown blue part, so28+?=37, compute9. 6. Elena's50 vs Andre23 FEWER asks for Andre, not the gap between them, so50−23=27; check27+23=50. 7. For source synthesis compare actual teacher-presented tape diagram, base-ten diagram and a student's own invented diagram: some show a relationship, some show an arithmetic algorithm. Assess MLR2 student explanation and manipulatives in class. 8. Complete clearly independent Edulytics digital exit, not Tyler's Seeds restricted source assessment.",
         "Adding Diego and Jada42+16=58 instead of finding MORE,26. Confusing what 'fewer' means: Andre's quantity is50−23=27, not23,73 or a sum. Treating every story as comparison, despite Lin's change story and Noah's missing-part story. Writing31−15=26 without exchanging a ten; actual16. Thinking37−28=19 instead of9. Treating a tape diagram that shows WHOLE and parts as if it had to depict the detailed trade of ones; a base-ten diagram does that. Claiming partner discussion or real cube manipulation occurred just because an online numeric answer matched. Protected Tyler's Seeds cool-down is not copied.",
         "Four original seeds stories within50: Diego42 vs Jada16 gap26; Lin31 gives15 leaves16; Noah37 total28 yellow missing blue9; Elena50 vs Andre23 fewer means Andre27. Choose compare/take-from/part-unknown/smaller-unknown equation before computing; use base-ten, real cubes and tape diagrams, with teacher-reviewed oral explanations and first-party exit."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L11 source identity or standard drift");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Check([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Check([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Check(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U02 L11 prior six fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L11 predecessor version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L11 persisted rows changed");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U02 L11 prior persisted fields changed");
    }
}

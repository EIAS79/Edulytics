using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Original 2021 G2 U02 L12 source-specific match-diagrams-only Activity1.</summary>
public static class CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection
{
    public const string Version="usccss-g2-u02-l12-mancala-four-tape-story-structures-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L12";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-12/preparation.html";
    private const string Sha="31d6608bc608d3c38a28f381de76a5f6d5d7b0f4f31db5dea198eb7ee4dddf86";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will solve story problems of different problem types within 100.\nIn previous lessons, you solved different story problems within 50 and compared different diagrams and methods. You interpreted and used tape diagrams to represent Compare story problems. The problems in this lesson include some of the more challenging types (for example, Add To, Start Unknown).",
         "Key ideas:\n- Make sense of diagrams that represent story problems\n- Solve one-step story problems within 100\n- The addends are the quantities being combined and the sum is the resulting total; the expression must match the representation.",
         "Example 1: Circle the diagrams that match each story. Then explain your match to your partner. Clare captured 54 seeds. Han captured 16 fewer seeds than Clare. How many seeds did Han capture? Clare has 54 seeds on her side of the board. Han has 16 seeds on his side. How many seeds are on the board in all? Clare has 54 seeds. 16 seeds are in her hand. The rest of...\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.\nExample 2: Match each story problem with a diagram. Explain why the cards match. Choose 2 story problems and solve them. Show your thinking.\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will solve story problems of different problem types within 100. In previous lessons, you solved different story problems within 50 and compared different diagrams and methods."],
        ["Grade 2 Unit 2 Lesson 12 Story Problems and Diagrams, PLC Activity (IM K–5 2021). Warm-up Notice and Wonder: Mancala introduces a real seed-and-pit board game as context; teacher displays the original picture, invites questions about seed amounts/positions and does not require knowledge of one particular ruleset. Activity 1 Interpret the Diagram (15 minutes) has four different Clare/Han Mancala-like seed stories, all involving54 and16. Students should circle and explain matching authentic TAPE diagrams but explicitly DO NOT solve the problems during this activity. MLR6 Three Reads separates understanding context, known quantities and which quantity is unknown. Activity 2 Card Sort Story Problems and Diagrams (20 minutes) asks genuine pairs to match SOURCE printed cards, justify diagram relationships, then choose TWO authentic stories from cards A/B/C/D and solve them. The actual printed diagram letter images, card text and numeric values beyond the four public Activity1 stories are unavailable in the locally archived public teacher lesson; don't invent them. Teacher shows Cards L and N in synthesis and asks for a matching story. The independently authored Edulytics numeric companion may compute source-type results, but those answers are NOT a claim the original Activity1 required solving, and genuine card matching/partner discourse are teacher-assessed.",
         "The same numbers54 and16 can stand for different relationships depending on the story and question. (1) Clare captured54 seeds, Han captured16 FEWER, so Han's unknown count is54−16=38: Compare Smaller Unknown; the 16 is the DIFFERENCE. (2) Clare has54 and Han has16 on their sides of the board, so ALL seeds=54+16=70: Put Together Total Unknown; the 16 is a KNOWN part. (3) Clare has54 seeds TOTAL, holds16, remainder on board is54−16=38: Take Apart Missing Part. (4) BOARD TOTAL54, Clare's side16, Han's side unknown54−16=38: Put Together Missing Part. Each source question has a different part-whole/comparison diagram even though three share38 as numerical answer. A tape diagram identifies what is known and unknown; it does not necessarily show a tens trade. Activity1 source instruction is MATCH diagrams, NOT calculate. For subtraction by place54=40+14, subtract16=(10+6) yields38; for addition54+16=70 by 4+6 composing ten. Source card sort authentic print cards and diagrams are teacher supervised; official Addressing CCSS 2.NBT.B.5 and2.OA.A.1.",
         "PUBLIC source Activity1 story1: Clare54 captured, Han16 fewer => Han38, but learners ONLY SELECT matching source tape diagram in Activity1. Story2: Clare54 seeds and Han16 seeds, how many on board altogether? 54+16=70. Story3: Clare54 in all,16 in her hand, others on board54−16=38. Story4: board54, Clare's side16, Han's side54−16=38. These calculations are Edulytics supplemental interpretive checks AFTER the original diagram-match-only activity, not source Activity1 requirements. PUBLIC source synthesis references diagrams Cards L and N, but their picture contents are not provided in local public page. FIRST-PARTY new numeric contexts: Mai has63 counters, Priya27 fewer =>36; Mai holds63 and Priya27 =>90 altogether; of63,27 in a cup =>36 outside; total63 with27 on A side =>36 on B side. These are independent Edulytics tasks and not a reproduced publisher card sort or restricted Find the Match assessment.",
         "1. Teacher shows the original Mancala image; students notice pit positions, seed counts and discuss the context, without pretending a specific regional board/ruleset has been played. 2. Activity1 MLR6 Three Reads: read Clare54 and Han16 FEWER before revealing question, identify Clare54, comparison gap16 and Han unknown, then reveal question and select the authentic matching diagram; DO NOT solve in this original activity. 3. For the remaining three stories, determine whether54 is one part or whole, whether16 is a known part or the comparison gap, then select the source tape diagrams and explain their question marks; teacher observes. 4. Activity2 in real pairs: match original printed Story Problem and Diagram cards, justify which known/unknown quantities align, then choose exactly TWO source story cards A–D and solve with student-chosen method. 5. In the lesson synthesis compare true published Cards L and N diagram structures, discussing a story that matches N; their hidden image contents are not invented. 6. Digital companion separately computes the four original story mathematical relationships as optional checks: smaller38, combined70, remaining38, missing38. Explain54−16 by54=40+14 and54+16 by composing a ten. 7. Complete separate first-party Edulytics contextual problems; teacher evaluates original matching, MLR6 discourse and genuine source card work.",
         "Solving every source Activity1 story when the original instruction explicitly asks to identify diagrams without computing. Assuming that because all four stories use54 and16, the question marks mean the same thing, even though story2 asks for70 while the other three imply38. Treating Han's16 FEWER as the number Han captured rather than the difference; the question asks54−16=38. Marking Clare54 and Han16 as a subtract problem when question asks for seeds in ALL =>70. Matching a diagram by seeing both numbers but ignoring where question mark is located. Inventing the authentic source diagram letters/cards A–D or L/N details from teacher text which does not show them. Counting a learner's typed numeric result as proof that original partner card sort, story explanation or MLR6 Three Reads happened.",
         "G2 U02 L12 source Mancala context; Activity1 MATCH original tape diagrams ONLY for four Clare/Han scenarios (54 vs16 fewer,54+16 total,54 with16 in hand, board54 with Clare16). Optional numeric interpretations38,70,38,38 but not required source Activity1 calculations. Activity2 genuine original card sort and solve TWO source card problems, teacher-observed. First-party Edulytics companion is separate; protected publisher card/diagram assets are not fabricated."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L12 original source identity changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Match(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))throw new InvalidOperationException("G2 U02 L12 previous six learner fields changed");
            en.Explanation=S.After[0];en.KeyConceptsAndRules=S.After[1];
            en.WorkedExamples=S.After[2];en.StepByStepSolutions=S.After[3];
            en.CommonMistakes=S.After[4];en.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L12 prior persisted version changed");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U02 L12 previous language rows changed");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Match(t,false)||Match(t,true)))
            throw new InvalidOperationException("G2 U02 L12 previous learner fields no longer match");
    }
}

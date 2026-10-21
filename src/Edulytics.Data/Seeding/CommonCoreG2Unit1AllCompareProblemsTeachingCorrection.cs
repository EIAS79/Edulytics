using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Source-specific original IM2021 G2 U01 L16 true-false, library return,
/// Three Reads and all three compare unknown positions within100.
/// Canonical URL/SHA/outcomes/previous learner fields locked.
/// </summary>
public static class CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l16-all-compare-place-value-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L16";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-16/preparation.html";
    private const string Sha="a667fc480be9caef2090bc948dd790737ab15c66c7d131397c8a530e1f964db5";
    private sealed record Prose(
        string OldExplanation,string OldRules,string OldExamples,string OldSteps,
        string OldMistakes,string OldSummary,string NewExplanation,string NewRules,
        string NewExamples,string NewSteps,string NewMistakes,string NewSummary);
    private static readonly Prose T=new(
        "Learning goal: You will solve Compare problems with an unknown in all positions.\nThe number choices in the Compare problems in this lesson encourage you to use methods based on place value to find the unknown value. You may look for ways to compose a ten or subtract multiples of ten when finding unknown values within 100. You will subtract numbers other than multiples of ten within 100 in future lessons.",
        "Key ideas:\n- Solve Compare problems with unknowns in all positions within 100\n- The number choices in the Compare problems in this lesson encourage you to use methods based on place value to find the unknown value\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(18 + 5 = 18 + 2 + 3\\) \\(28 + 5 = 30 + 3\\) \\(38 + 4 = 40 + 3\\)\nApproach: \"How could you change 28 + 5 to be 30 + 3?\" (I could take 2 from 30 and add it to 3.)\nExample 2: Priya and Andre returned books to the library. Priya returned 29 books. Andre returned 8 more books than Priya. Who returned more books? Use the story problem to complete the diagram. How many books did Andre return? Andre read 45 pages of his book last night. Priya read 20 fewer pages of her book than Andre. Who read more pages? Use the story problem to...\nApproach: Represent both quantities, compare their values, and state the relationship using words, symbols, or a clear explanation.",
        "Step 1: Identify the quantities or expressions to compare.\nStep 2: Represent or calculate each value accurately.\nStep 3: Decide whether one is greater, less, or equal.\nStep 4: Write or explain the comparison.\nStep 5: Recheck both values before finalizing.",
        "Common mistake: deciding from appearance instead of actual values. Represent or calculate both quantities before comparing.",
        "You will solve Compare problems with an unknown in all positions. The number choices in the Compare problems in this lesson encourage you to use methods based on place value to find the unknown value.",
        "Solve ALL positions of Compare problems within 100 using place-value and make-ten strategies, with a tape diagram where useful. Warm-up TRUE/FALSE: 18+5=18+2+3 true (23=23);28+5=30+3 true (33=33);38+4=40+3 FALSE (42≠43), explaining decomposition. Activity 1 A Trip to the Library: Priya returns 29 books and Andre 8 MORE, so Andre returns 37; Andre reads 45 pages and Priya 20 FEWER, so Priya reads 25. Students label both tape diagrams with larger and smaller names, known number and ? before solving. Activity 2 Solve Compare Problems with Three Reads: (1) Noah27 pages, Jada10 FEWER -> Jada17; (2) Noah25 minutes, Jada30 MORE -> Jada55; (3) Jada47 pages, Noah20 pages -> difference27; (4) Jada28 books, Noah14 MORE -> Noah42. Choose appropriate equations, tape sketches, mental tens or another explained method; make ten when needed. End with a new independent Edulytics three-unknown-position Cool-down, not the publisher's restricted Compare the Pages assessment.",
        "The tape tells you the three DIFFERENT quantities: LARGER amount, SMALLER amount and the extra DIFFERENCE. When the unknown is LARGER, add smaller+difference. When SMALLER, subtract difference from larger. When DIFFERENCE, subtract smaller from larger; different equations can represent the same story.\nMake ten to add 29+8: 29+1+7=37. A subtraction of TENS such as 45−20=25 or 27−10=17 can use place value without unneeded regrouping.\nSource Activity 1 books returned: Priya29, Andre8 more => Andre37 and related equations 29+8=37, 37−8=29. Pages read: Andre45, Priya20 fewer => Priya25, 45−20=25 and 25+20=45.\nSource Activity 2 Three Reads: (A) Jada10 fewer than Noah27 =>17; (B) Jada30 more than Noah25 =>55; (C) Jada47 versus Noah20 => how many fewer Noah=27; (D) Noah14 more books than Jada28 =>42.\nRead story at least three ways: understand the CONTEXT, name countable QUANTITIES and identify the exact QUESTION. Verify a count is less than100 and that 'more/fewer' direction matches the story.\nThe student may choose tape diagrams, word equations or other accurate labeled drawings; mathematics and explanation matter more than copying a particular graphic.",
        "Warm-up: 18+5=18+2+3 true; 28+5=30+3 true; 38+4=40+3 false because 42≠43.\nLibrary books: Priya29 and Andre8 more -> Andre37. Unknown LARGER strip ?, smaller Priya29, difference8. Make ten 29+1+7=37.\nLibrary pages: Andre45 and Priya20 fewer -> Priya25. Unknown SMALLER, total45, difference20; 45−20=25.\nThree Reads: Noah27, Jada10 fewer -> Jada17. Noah25 minutes, Jada30 more -> Jada55. Jada47, Noah20 pages -> missing DIFFERENCE27. Jada28 books, Noah14 more -> Noah42.\nNew Edulytics Cool-down fiction: Hannah34 stones, Dina20 MORE ->54; Luca52 badges, Mira30 FEWER ->22; Tami63 stickers, Max41 -> difference22.",
        "Step 1: Before doing arithmetic, decide who has more and which quantity the story asks for: larger amount, smaller amount or difference. Read the question at the end rather than reacting to an isolated word.\nStep 2: Label the tape diagram if helpful: align the smaller known part with a larger bar and show dashed extra difference, placing ? on the ACTUAL missing larger/smaller/gap part. An accurately labeled alternative drawing or equation is also valid.\nStep 3: For 29 books plus8, make ten: 29+1=30 then30+7=37. For 45 pages minus20, subtract two TENS:45−20=25.\nStep 4: For the three-read routine, read the problem once for its situation, again to name and discuss countable quantities, and a third time to identify exactly which comparison is asked.\nStep 5: Solve the four source scenarios: 27−10=17, 25+30=55, 47−20=27 and 28+14=42, using counting on or tens and ones as appropriate.\nStep 6: Confirm the related equation fits the question's unknown position and story direction. Check all answers are within100 and cannot swap the two people.\nStep 7: Discuss a partner's different valid strategy or graph/tape drawing. Teacher evaluates original problem explanations, labels and genuine collaboration; computer feedback checks numerical results and selected equation structures only.",
        "Mistake: calculating 29−8=21 for Andre's MORE-books problem; Andre37 has the larger quantity, not the smaller.\nMistake: treating Andre's reading pages45 minus20 as a difference-only question; the requested amount is Priya's SMALLER total25.\nMistake: marking 38+4=40+3 as true; 42 and43 are not equal. Making a ten must preserve the SAME total.\nMistake: treating 47 and20 as quantities to COMBINE when asked how many fewer Noah read; comparison gap is27.\nMistake: turning a 30-more problem into 25−30 or pretending the second child has 30 total; Jada read 55 minutes.\nMistake: forcing only a tape-drawing method or penalizing a mathematically correct alternate diagram/equation, against the original Three Reads and accessibility goals.\nMistake: assuming correct integers prove the learner read three times, drew accurately, or talked with a partner; teacher review records that separately.",
        "Solve Compare stories with ALL three unknown positions within100 by making tens and using place value. Warm-up checks true/false equations; original library and Three Reads problems resolve to 37,25,17,55,27,42. Use correctly labeled tapes or alternative representations and complete a new Edulytics-authored Cool-down.");

    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l) =>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.OA.A.1","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U01 L16 original source identity changed");
    }
    private static bool Match(CanonicalLessonContentPackTranslation x,bool newer) =>
        x.Explanation==(newer?T.NewExplanation:T.OldExplanation)&&
        x.KeyConceptsAndRules==(newer?T.NewRules:T.OldRules)&&
        x.WorkedExamples==(newer?T.NewExamples:T.OldExamples)&&
        x.StepByStepSolutions==(newer?T.NewSteps:T.OldSteps)&&
        x.CommonMistakes==(newer?T.NewMistakes:T.OldMistakes)&&
        x.QuickSummary==(newer?T.NewSummary:T.OldSummary);
    private static bool Match(CurriculumLessonContentTranslation x,bool newer) =>
        x.Explanation==(newer?T.NewExplanation:T.OldExplanation)&&
        x.KeyConceptsAndRules==(newer?T.NewRules:T.OldRules)&&
        x.WorkedExamples==(newer?T.NewExamples:T.OldExamples)&&
        x.StepByStepSolutions==(newer?T.NewSteps:T.OldSteps)&&
        x.CommonMistakes==(newer?T.NewMistakes:T.OldMistakes)&&
        x.QuickSummary==(newer?T.NewSummary:T.OldSummary);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))
                throw new InvalidOperationException("G2 U01 L16 previous rich learner fields mismatch");
            en.Explanation=T.NewExplanation;
            en.KeyConceptsAndRules=T.NewRules;
            en.WorkedExamples=T.NewExamples;
            en.StepByStepSolutions=T.NewSteps;
            en.CommonMistakes=T.NewMistakes;
            en.QuickSummary=T.NewSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))
            throw new InvalidOperationException("G2 U01 L16 unexpected old content version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U01 L16 unexpected persisted language set");
        var old=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("G2 U01 L16 previous student prose changed");
    }
}

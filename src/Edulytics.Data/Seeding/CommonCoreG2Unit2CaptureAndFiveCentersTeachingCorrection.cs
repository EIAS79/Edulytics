using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Original 2021 optional U02 L04 Capture Squares and Five in a Row.
/// Source Stage6 includes tens COMPOSITION despite generic opening claim.</summary>
public static class CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection
{
    public const string Version="us-ccss-g2-u02-l04-capture-five-composing-true-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L04";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-4/preparation.html";
    private const string Sha="c8632e338fa6a97750d30545dc4af757c9598194029df02864cd6754dcf755e5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will practice addition and subtraction within 100 that does not require composing or decomposing a ten.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. You learn new stages of centers that were introduced in grade 1. In Activity 1, you learn stage 3 of the Capture Squares center.",
         "Key ideas:\n- Add and subtract within 100\n- This lesson is optional because it is an opportunity for extra practice that not all classes may need\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Find the value of each expression mentally. \\(6 - 3\\) \\(66 - 3\\) \\(66 - 30\\) \\(66 - 33\\)\nApproach: “How is each expression like \\(6 - 3\\) ? How are the expressions different?” (Each expression has the digits 6 and 3. In each expression you could use what you know about \\(6 - 3\\) . Some expressions you are finding 6 ones \\(-\\) 3 ones. In some expressions you are finding 6 tens \\(-\\) 3 tens.)",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will practice addition and subtraction within 100 that does not require composing or decomposing a ten. This lesson is optional because it is an opportunity for extra practice that not all classes may need."],
        ["Grade 2 Unit 2 Lesson4 is an OPTIONAL center lesson. Its original Number Talk practices subtracting by place without exchanging a ten: 6−3=3,66−3=63,66−30=36,66−33=33. Activity1 Capture Squares Stage3 was introduced in Grade1; students play in pairs, spin 6–10 (or choose on a WILD), flip a 0–10 card, add within20, find a square labelled with their sum, mark an edge between two adjacent dots, and capture a square in their color by completing its fourth side. First player to capture three squares wins. Activity2 Five in a Row Stage6 is DIFFERENT: add within100 INCLUDING sums that compose a ten. Use two paper clips selecting one number from each of two board rows; place a counter on the sum. Then partners alternate, moving exactly ONE clip each turn; get five counters in one uninterrupted row horizontal, vertical or diagonal to win. The generic opening explanation says no regrouping, but the original second center specifically includes composing a ten; Edulytics preserves the distinction rather than misrepresenting the source. Teacher assesses actual gameplay with authentic tools; first-party numerical examples and the independent Cool-down are digital companions only.",
         "Number Talk 6−3=3,66−3=63,66−30=36,66−33=33: tens and ones are distinct place values, so subtracting 30 changes tens, whereas subtracting3 changes ones. Capture Squares Stage3: spinner 6..10 or WILD choose a valid integer6..10; card 0..10; sum spinner+card always6..20. Draw ONE edge joining adjacent dots at the target sum. When a player completes a square with the fourth edge, they color and capture it; first3 captured squares wins. Five in a Row Stage6: choose one top-row and one bottom-row board number, compute a sum at most100, and mark the matching board value; after the opening move only ONE of two paper clips moves per turn. Unlike the source Number Talk and preceding lessons, Five in a Row Stage6 intentionally INCLUDES composition of a ten in some additions; for example25+8=33 needs 5 ones+8 ones=13 ones ->exchange10 ones into one ten. The winning streak is five adjacent cells across, down or diagonal. Source addressing includes 2.OA.B.2 for Capture Squares and2.NBT.B.5 for Five in a Row.",
         "PUBLIC SOURCE Number Talk: 6−3=3;66−3=63;66−30=36;66−33=33. PUBLIC SOURCE Five in a Row synthesis: one clip on 8, move other to25 so25+8=33 and mark33 as a row-completing value. The 25+8 example COMPOSES a ten: ones5+8=13. FIRST-PARTY Edulytics Capture Squares math: spinner8, card7 =>15, then find a board cell labeled15 and mark one adjacent-dot edge; this numerical answer alone does NOT capture a square. FIRST-PARTY Edulytics Five in a Row turn sequence: first top25/bottom8=>33; on the NEXT move retain top25 and move bottom8→9 =>34, so exactly one clip moved; tens exchange needed for25+9. FIRST-PARTY independent exit: Capture spinner7+card6=13; Five in a Row26+7=33 (composing a ten); subtraction75−30=45 (no exchange). No copied publisher gameboard or spinner artwork is presented.",
         "1. Number Talk mentally answer 6−3,66−3,66−30 and66−33; explain 6 ones versus6 tens, and why there is no regrouping. 2. In Capture Squares Stage3 work with a REAL partner and source board, use the spinner in6–10 or choose a number6–10 on WILD, then draw a card0–10. Add within20 and locate a square with that number. 3. Join only adjacent dots along the target square. Color a square only when the newly drawn edge closes all four sides; first student to capture three squares wins. Teacher checks real turn-taking and game state. 4. For Five in a Row Stage6, choose TWO source board entries using two paper clips, sum the selected top/bottom numbers, and cover its value with your side of a counter. 5. On each following turn, move exactly ONE clip, recompute and cover the sum. Some sums NEED a new ten; for25+8=33, trade ten of the13 singles into another ten, giving33. 6. Win only upon five consecutive own-colored counters across, vertically or diagonally. Teacher validates actual five-in-a-row board and strategic choices. 7. Compare with partner how the chosen arithmetic strategies and fair turns worked. 8. Complete distinct, original Edulytics digital checks for the Number Talk, the sum within20 and a composing-ten sum within100; numeric correctness does not assert game winner.",
         "Treating both activities as 'without composing a ten': this contradicts the original Five in a Row Stage6 explicit objective. Saying 66−30=63 rather than36 confuses tens and ones. In Capture Squares, using spinner5 or11, card11, or claiming a square captured after merely ONE edge is wrong; closure requires all four. Interpreting 'wild' as allowing any number outside6–10. In Five in a Row, changing both paper clips in one turn or confusing a numeric sum with a completed five-counter line violates the source game mechanics. Assuming25+8=213 or23 rather than33 overlooks tens exchange. Claiming that a digital arithmetic answer proves that two pupils genuinely played, drew an original board or won a full game is invalid.",
         "OPTIONAL Center Day 1: Number Talk3,63,36,33; Capture Squares Stage3 sums spinner6–10 plus card0–10, target board number, one valid edge each turn, first3 completed squares wins. Five in a Row Stage6 sums within100 INCLUDING composing a ten, alternate moving one paper clip and try for five consecutive counters. Numeric first-party checks and teacher-observed authentic gameplay are separate."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U02 L04 source identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation x,bool after)=>
        Matches([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation x,bool after)=>
        Matches([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Matches(string[] v,bool after)=>v.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U02 L04 old six fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L04 predecessor version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L04 expected one en translation");
        var old=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 U02 L04 prior persisted fields changed");
    }
}

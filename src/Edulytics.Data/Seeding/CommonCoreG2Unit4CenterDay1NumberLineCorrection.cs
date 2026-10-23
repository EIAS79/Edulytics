using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;

/// <summary>Strict original-source SHA, CCSS alignment and six-field learner-content repair for PED:US-CCSS-MATH:G2:U04:L06.</summary>
public static class CommonCoreG2Unit4CenterDay1NumberLineCorrection
{
    public const string Version="usccss-g2-u04-l06-optional-scoot-centers-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U04:L06";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-6/preparation.html";
    private const string Sha="d56627a509a6e2fe8575ec3b180b282b773ad986ac49e549e2220dfe2746a6ed";
    private static readonly string[] Before=["Learning goal: You will represent numbers on a number line and practice addition and subtraction within 100.\nThis lesson is optional because it is an opportunity for extra practice that not all classes may need. In Activity 1, you learn stage 1 of a new center called Number Line Scoot, where you generate numbers and move that interval on shared number lines. The goal is to land exactly on the end of each number line.",
        "Key ideas:\n- Represent numbers on a number line\n- This lesson is optional because it is an opportunity for extra practice that not all classes may need\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Find the value of each expression mentally. \\(65 - 25\\) \\(65 - 27\\) \\(55 - 17\\) \\(46 - 18\\)\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will represent numbers on a number line and practice addition and subtraction within 100. This lesson is optional because it is an opportunity for extra practice that not all classes may need."];
    private static readonly string[] After=["OPTIONAL IM2021 G2 U04 L06 offers center-based practice, not a new required standard sequence. Original warm-up Number Talk Decompose and Subtract: 65−25=40,65−27=38,55−17=38,46−18=28. Explain subtracting tens/ones or partial differences. Activity1 Introduce Number Line Scoot Stage1 Twos, Fives and Tens: a REAL pair uses the publisher's actual spinner, paper clip, gameboard containing shared number lines, and centimeter cubes. Each turn pupils spin and move a cube the prescribed LENGTH on one line, or DECOMPOSE the move across multiple lines. Player captures a cube ONLY when the physical move lands EXACTLY on the END tick of a number line; captured cube replaced with new cube at ZERO on that line. First player to collect FIVE cubes wins. Teacher verifies physical spinner and specific original track endpoints; don't invent hidden publisher track sizes. Activity2 center Choice Time permits previous center stages: Capture Squares3/4, Number Line Scoot Stage1, and Number Puzzles Stage4. Pupils choose one, rotate and discuss authentic tactics. These are HUMAN reviewed genuine centers; no fabricated spin outcomes or gameboard victories. Edulytics separate numerical companion uses clearly labelled made-up moves/track ends.",
        "Subtract efficiently by decomposing the subtrahend: 65−27=(65−20)−7=45−7=38;55−17=55−10−7=38;46−18=46−10−8=28. An original Scoot spin may be moved as prescribed on one real gameboard number line, or split into valid movements across multiple tracks per official directions. To CAPTURE a cube the moved marker must land EXACTLY on the track's final tick; overshooting is not an exact capture. Immediately reset with a new cube at zero. Winner first to FIVE captured cubes. Source gameboard endpoints and spinner image must be read from actual physical publisher resources; any online values such as 0..10 or synthetic 2,5,10 move examples are first-party ONLY and never asserted to be original board layouts.",
        "Original Number Talk official four warm-up answers are40,38,38,28. Source Stage1 uses a REAL spinner, THREE original shared line groups named Twos,Fives,Tens, allowing one-line movement or decomposed across multiple lines, capture on exact final tick and new cube at0, first to5 captures. Source offers Capture Squares stages3/4 and Number Puzzles stage4 as real center options; no made-up source scores. FIRST-PARTY arithmetic illustration, explicitly not original gameboard: invented track from0 to20, marker at15, invented spin5 lands exactly20 and is a capture; invented spin6 from15 would pass20 and is NOT an exact capture. Independent original calculation: 65−27=38; invented count of captured cubes3 needs two more to reach5.",
        "1. Discuss original four Number Talk subtraction values40,38,38,28 and show a decomposition strategy (e.g. subtract20 then7 for65−27). 2. In real group2 obtain authentic published Stage1 Number Line Scoot gameboard and spinner/paper clip plus counters. Inspect actual end ticks rather than assigning a made-up source track scale. 3. Spin physically; either move the given length on one line or split a spin across more than one line following publisher directions. 4. Only when an actual moved cube lands on a track's FINAL printed tick capture that cube, then add a new cube at its ZERO. First to collect5 ends the game. 5. Move to optional previously introduced centers Capture Squares3/4, Scoot Stage1 and Number Puzzles4, choose real activities and rotate with peer feedback. 6. Teacher observes legitimate spinner outcomes, gameboard endpoints, captures and center participation. 7. Separately solve Edulytics original exact subtraction, simple synthetic end-capture checks, and remaining-cubes questions, never equating them with observed publisher boards.",
        "Teaching optional center as a mandatory new academic standard. Claiming source spinner and specific track endpoints without inspecting the authentic publisher resource. Awarding a cube for overshooting a track end, or failing to reset a captured cube's next position to zero. Forgetting the first player to5 captured cubes, or assuming the spinner always moves in a single row when a permitted decomposition across tracks is allowed. Mistaking 65−27=48 (not subtracting tens) or mixing partial tens/ones. Presenting hypothetical online exercises or scores as actual partner performance in a source center.",
        "OPTIONAL source G2 U04 L06: Number Talk answers40,38,38,28; real Number Line Scoot Stage1 spinner with Twos/Fives/Tens, exact-end capture and reset-to-zero, first5 captured cubes wins; choose Capture Squares3/4 or Number Puzzles4 as alternatives. Teacher inspects physical source board and game play; first-party arithmetic and synthetic paths are separate."];
    private static readonly string[] Standards=["CCSS:2.MD.B.6","CCSS:2.NBT.A.2","CCSS:2.NBT.B.5"];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Standards))
            throw new InvalidOperationException("G2 original 2.4.6 source identity drift");
    }
    private static bool Match(string[] current,bool updated)=>current.SequenceEqual(updated?After:Before);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool updated)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    private static bool Same(CurriculumLessonContentTranslation t,bool updated)=>
        Match([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.6 prior six fields drift");
            t.Explanation=After[0];t.KeyConceptsAndRules=After[1];
            t.WorkedExamples=After[2];t.StepByStepSolutions=After[3];
            t.CommonMistakes=After[4];t.QuickSummary=After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))
            throw new InvalidOperationException("G2 2.4.6 persisted predecessor unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.6 persisted language drift");
        var old=rows.Single();var current=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=current.Title||!(Same(old,false)||Same(old,true)))
            throw new InvalidOperationException("G2 2.4.6 persisted prose conflict");
    }
}

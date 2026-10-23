using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;
/// <summary>IM 2021 G2 2.4.9, exact source URL/SHA, standards and six-field persisted predecessor guards.</summary>
public static class CommonCoreG2Unit4DifferenceStrategiesCorrection
{
    public const string Version="g2u04-l09-source-two-difference-strategies-v1";
    private const string LessonCode="PED:US-CCSS-MATH:G2:U04:L09";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-9/preparation.html";
    private const string SourceSha="54b2b10fb7e95ee9f5d5ccc701e46aae02051ff394f47a8bb70c29f4f56d0721";
    private static readonly string[] Outcomes=["CCSS:2.MD.B.5","CCSS:2.MD.B.6","CCSS:2.NBT.B.5"];
    private static readonly string[] OriginalFields=[
        "Learning goal: You will represent and compare different subtraction methods on the number line.\nIn previous lessons, you represented addition and subtraction equations on the number line and wrote equations to match representations on a number line. In this lesson, you deepen your understanding of subtraction as taking from and as an unknown addend problem. You represent subtraction methods on the number line and compare how each representation shows the difference.",
        "Key ideas:\n- On a number line, represent counting on and counting back strategies for solving subtraction equations\n- In previous lessons, you represented addition and subtraction equations on the number line and wrote equations to match representations on a number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Find the value of each expression mentally. \\(20 - 2\\) \\(20 - 17\\) \\(49 - 3\\) \\(67 - 64\\)\nApproach: “I noticed that some you counted on for \\(20 - 17\\) instead of subtracting or counting back.” “How do you decide when to count back or subtract and when to count on?” (If the numbers are close together, I count on. 17 is only 3 away from 20.)\nExample 2: What number makes this equation true? ___ \\(38 - 4 = {?}\\) Represent your thinking on the number line. What number makes this equation true? ___ \\(75 - 68 = {?}\\) Represent your thinking on the number line. What number makes this equation true? ___ \\(57 - 24 = {?}\\) Represent your thinking on the number line.\nApproach: “Both of these number lines show strategies for finding \\(75 - 68 = {?}\\) ” “How do both representations show the difference?” (In the first number line, the arrow is pointing to the difference. In the second one, the length of the jump shows the difference.)",
        "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
        "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
        "You will represent and compare different subtraction methods on the number line. In previous lessons, you represented addition and subtraction equations on the number line and wrote equations to match representations on a number line."
    ];
    private static readonly string[] CorrectedFields=[
        "IM2021 Grade2 Unit4 Lesson9 The Difference Between Numbers. Warm-up Number Talk: Add To works with20−2=18,20−17=3,49−3=46,67−64=3; pupils decide whether counting ON from a nearby smaller number or counting BACK from larger is more efficient. Original Activity1 source subtraction questions38−4=34,75−68=7,57−24=33, each solved on their own REAL0..100 number line. A subtraction 'take away' representation jumps LEFT from larger to difference; an unknown-addend representation jumps RIGHT from the smaller to larger and its jump LENGTH is the difference. These pictures do not mean one diagram arrow always lands at difference when counting on. Original Activity2 uses MLR6 Three Reads for Elena's REAL publisher story: string length65 inches, cuts33, remaining32. Students choose TWO original published visual number-line representations that correctly show either subtracting33 from65 (65→32) or counting on33→65 length32; the source also offers a distractor diagram65→98 labelled33. Han's original story:87 inches string, cuts85, remaining2; pupils represent87−85=2 and compare strategies, trying a different peer's approach. Teacher reviews real original diagrams, choice of two source drawings, physical or written student arrows and contextual explanations. Do not claim fictional source results or copy restricted Cool-down.",
        "Subtraction a−b=the nonnegative DIFFERENCE d such that b+d=a. Two valid number-line methods: TAKE AWAY start at a and move LEFT by b so END is d; or COUNT ON start at b and move RIGHT by d so END is a; the rightward JUMP LENGTH is the answer d, not the endpoint. For75−68=7, count from68 to75 distance7 or jump left68 from75 landing7 (the latter arrow quite long). For38−4=34, subtraction removes4 and lands34. For57−24=33, use tens24 then ones if useful. Elena's65−33=32 inches; counting on from33 to65 moves32. Han's87−85=2 inches; counting on from85 to87 moves2. A true representation always has correct direction, unit length, start, endpoint and meaning in physical context. Unseen arrows require teacher artwork inspection.",
        "Source warm-up results20−2=18;20−17=3;49−3=46;67−64=3. Source Activity1 38−4=34,75−68=7,57−24=33, all require actual arrow rationale. Source Activity2 Elena string65in−33in=32in; among three original figures, one shows counting on from33 to65, one shows mistaken65 to98(+33) and one shows65 left33 ending32; teacher visually confirms the two valid ones. Han string87in−85in=2in and students draw/share another actual method. INDEPENDENT Edulytics invented pair91−86=5 with count-on86→91 move5 and count-back91 by86 landing5, not a source classroom outcome.",
        "1. Solve original four Add To warm-up results18,3,46,3; compare reasons for count-on17→20 length3 versus subtracting17 backwards from20. 2. For source Activity1 use real number-line models to solve38−4=34,75−68=7 and57−24=33 using a meaningful leftward take-away OR rightward unknown-addend jump. 3. For75−68 explicitly show two valid interpretations:75 left68 lands7, or68 right7 lands75; distinguish jump length7 from endpoint75. 4. Read Elena's65in string minus33in source story THREE times, identify actual measures and target 'left'; compute32 inches. Select TWO source published drawings that show65→32 left33 or33→65 right32; reject wrong65→98 as addition33. 5. Read Han's87in minus85in string, find2in, represent and compare strategy with partner; verify equation87−85=2. 6. Teacher checks original visual evidence, student work and peer MLR6 exchange. 7. Independently solve Edulytics-authored equal-interval gap and contextual arithmetic, never claim it is publisher Cool-down.",
        "Confusing rightward count-on from68 to75 length7 with '75 − 68 =75' because endpoint75. Treating leftward take-away arrow endpoint (7 for75−68) as the amount removed, rather than the remainder. Choosing Elena's invalid original diagram65→98 as showing65−33, when it actually ADDS33. Forgetting inches as the unit of actual context or mixing Han's85 removed with2 remaining. Subtracting only tens and not ones for57−24. Treating all left jumps and right jumps as equivalent with no labeled start. Fabricating publisher artwork or claiming student partner work checked automatically.",
        "IM2021 G2U04L09: original warm-up18,3,46,3; own paper line problems38−4=34,75−68=7,57−24=33; compare left take-away with right unknown-addend. String stories Elena65−33=32 inches and Han87−85=2 inches require two source diagram choices and different genuine student methods. Teacher judges source figures and discourse; independent numeric questions separate."
    ];
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==LessonCode;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=SourceUrl||
           !string.Equals(l.CanonicalBodySha256,SourceSha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(Outcomes))
            throw new InvalidOperationException("G2 2.4.9 original source identity mismatch");
    }
    private static bool Equal(string[] a,bool updated)=>a.SequenceEqual(updated?CorrectedFields:OriginalFields);
    private static bool Same(CanonicalLessonContentPackTranslation t,bool updated)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    private static bool Same(CurriculumLessonContentTranslation t,bool updated)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],updated);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.9 learner prose predecessor changed");
            t.Explanation=CorrectedFields[0];t.KeyConceptsAndRules=CorrectedFields[1];
            t.WorkedExamples=CorrectedFields[2];t.StepByStepSolutions=CorrectedFields[3];
            t.CommonMistakes=CorrectedFields[4];t.QuickSummary=CorrectedFields[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> stored)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 2.4.9 unapproved persisted predecessor");
        if(stored.Count==0)return;
        if(stored.Count!=1||stored.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.9 persisted language rows drift");
        var previous=stored.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(previous.Title!=incoming.Title||!(Same(previous,false)||Same(previous,true)))
            throw new InvalidOperationException("G2 2.4.9 original stored fields drift");
    }
}

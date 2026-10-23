using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;
/// <summary>IM 2021 G2 2.4.8, exact source URL/SHA, standards and six-field persisted predecessor guards.</summary>
public static class CommonCoreG2Unit4EquationArrowCorrection
{
    public const string Version="g2u04-l08-original-write-arrows-v1";
    private const string LessonCode="PED:US-CCSS-MATH:G2:U04:L08";
    private const string SourceUrl="https://im.kendallhunt.com/k5/teachers/grade-2/unit-4/lesson-8/preparation.html";
    private const string SourceSha="300866021d022b74d6721c9528d53aa5684d915b9f192b6f20946436047def6d";
    private static readonly string[] Outcomes=["CCSS:2.MD.B.6","CCSS:2.NBT.A.2"];
    private static readonly string[] OriginalFields=[
        "Learning goal: You will write equations based on number lines and represent equations on the number line.\nIn the previous lesson, you interpreted representations of addition and subtraction equations on the number line. You used the direction of the arrow to determine what operation was represented and matched equations to number lines.",
        "Key ideas:\n- Use number line diagrams to represent and write addition and subtraction equations\n- In the previous lesson, you interpreted representations of addition and subtraction equations on the number line\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
        "Example 1: Represent each equation on the number line. \\(15 + 7 = 22\\) \\(18 - 6 = 12\\) \\(46 + 7 = 53\\) \\(33 - 9 = 24\\)\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.\nExample 2: For each number line, write an equation that matches the diagram. Equation: ___ Equation: ___ Equation: ___ Equation: ___ Compare your equations with your partner. Pick the 2 number lines you think are most alike. Explain your choice to your partner.\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.",
        "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
        "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
        "You will write equations based on number lines and represent equations on the number line. In the previous lesson, you interpreted representations of addition and subtraction equations on the number line."
    ];
    private static readonly string[] CorrectedFields=[
        "IM2021 Grade2 Unit4 Lesson8 Equations on a Number Line PLC Activity. Warm-up CHORAL COUNT backward by10, in three distinct runs: from99 down to49 (99,89,79,69,59,49), from98 to48 (98,88,78,68,58,48), and from95 to45 (95,85,75,65,55,45). Ones digit stays the SAME while tens digit decreases one per step. Original Activity1 REPRESENT four equations on actual equally spaced paper number lines:15+7=22 (15→22 right7),18−6=12 (18→12 left6),46+7=53 (46→53 right7),33−9=24 (33→24 left9). The printed source first two lines are0..40 by5, third0..60 by5, fourth0..40 by5; a five-labelled interval does not mean each movement is five units. Students draw arrow with correct START/END and direction, then discuss MLR8 with partner. Original Activity2 WRITE an equation from FOUR genuine diagrams:3→15 means3+12=15;15→12 means15−3=12;15→3 means15−12=3;12→15 means12+3=15. Compare which diagrams are similar and why, using ORIGINAL figures. Teacher checks physical arrow drawing, original images and peer discussion, not just computed answers. Restricted publisher Cool-down not reproduced.",
        "On any correctly scaled number line the start endpoint gives the first operand. Right arrow a→b, b>a: a+(b−a)=b. Left arrow a→b, b<a: a−(a−b)=b. A displayed line labelled by fives still has meaningful one-unit locations; do not use 'one printed tick' as one if5 apart. For official 15+7=22 put point15 and arrow RIGHT length7 to22. For 18−6=12 begin18 and arrow LEFT length6 to12. For46+7=53 start46 and go right7; for33−9=24 start33 and go left9. On source Activity2 3→15 means jump12 (3+12=15),15→12 jump3 left,15→3 jump12 left,12→15 jump3 right. Distinguish equations with same numbers but different start, direction and jump. Counts99→49,98→48,95→45 decrease by10 keeping ones digits9,8,5 respectively.",
        "Publisher original Choral Count runs:99,89,79,69,59,49;98,88,78,68,58,48;95,85,75,65,55,45. Source Activity1 four original start/movement/end triplets: (15,+7,22), (18,−6,12),(46,+7,53),(33,−9,24). Source Activity2 arrow paths (3,15),(15,12),(15,3),(12,15) yield3+12=15,15−3=12,15−12=3,12+3=15. Independent Edulytics invented case19→26 has19+7=26; invented45→31 has45−14=31. Invented examples are not original picture marks.",
        "1. Count backward orally by10 three times from99,98 and95 until49,48 and45 respectively; explain unchanged ones digit with each decade decreased. 2. Use the ACTUAL publisher Activity1 four number-line diagrams: anchor15,18,46,33; compute increment7,−6,+7,−9; draw equal-unit arrows right or left ending22,12,53,24. 3. Check correct number-line printed increments and that the physical arrow spans the proper unit count, not '7 printed fives'. 4. On original Activity2 drawings record equations for actual paths3→15,15→12,15→3,12→15: write exact start+signed-jump=end. 5. Compare with partner and discuss most alike representations using source MLR8, with teacher observing actual images and points. 6. Independently answer separately generated Edulytics numeric endpoints and jump lengths; digital correctness alone cannot certify actual arrow drawings or source artwork.",
        "Counting backwards from99 by 1 instead of10, or changing the ones digit from9 when subtracting10. Drawing the right arrow for a subtraction because result12 is smaller than18; using the result as start for addition. Treating label difference5 as a single unit in Activity1 rather than five equal one-unit positions. Misreading15→3 as15−3=12 when the endpoint is3: it must be15−12=3. Declaring source arrows equivalent just because the same three digits are involved, without checking length and start. Claiming publisher restricted Cool-down solved by a synthetic online track.",
        "IM2021 G2U04L08: original choral99/98/95 backwards by10; draw four arrows15→22,18→12,46→53,33→24; derive four equations from arrows3→15,15→12,15→3,12→15. Teacher inspects true number-line diagrams and discussion; four independent exact question families separate."
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
            throw new InvalidOperationException("G2 2.4.8 original source identity mismatch");
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
            if(!Same(t,false))throw new InvalidOperationException("G2 2.4.8 learner prose predecessor changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 2.4.8 unapproved persisted predecessor");
        if(stored.Count==0)return;
        if(stored.Count!=1||stored.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 2.4.8 persisted language rows drift");
        var previous=stored.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(previous.Title!=incoming.Title||!(Same(previous,false)||Same(previous,true)))
            throw new InvalidOperationException("G2 2.4.8 original stored fields drift");
    }
}

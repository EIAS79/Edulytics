using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Source-guarded IM2021 G2 U03 L11 all authentic ribbon quantities and start unknown.</summary>
public static class CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection
{
    public const string Version="usccss-g2-u03-l11-saree-start-unknown-source-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L11";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-11/preparation.html";
    private const string Sha="688008add1a6ab8d22f8a0d5e88f99fa7cfbf5bc6f5ff86f5b984abb09279910";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will solve Take From story problems within 100 in the context of length.\nIn previous lessons, you learned how to measure lengths using different units of measure. In this lesson, you solve Take From problems involving length measured in inches. You use the context of length to make sense of tape diagrams that represent the part-part-whole relationships between the quantities in each story (MP2).",
         "Key ideas:\n- Solve one-step story problems about length within 100\n- In previous lessons, you learned how to measure lengths using different units of measure\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: What do you notice? What do you wonder? \\(54 - 16 = {?}\\)\nApproach: “What do you think the diagram and the ribbon could have in common?” (Maybe the 54 is cm or in, so 16 is how much of the ribbon is being cut off.) “What could the question mark represent?” (The longer part of the ribbon after it is cut. It represents the value of \\(54-16\\) .)\nExample 2: What do you notice? What do you wonder? Priya had a ribbon that was 44 inches long. She cut off 18 inches. How long is Priya’s ribbon now? Andre drew this diagram to help him think about the problem. What does the “?” represent in the story? Why do you think there is a dotted line between the parts? Find the unknown value. Show your thinking. Priya’s...\nApproach: “How did the tape diagram help you think about what you needed to do to find the unknown value?” (The diagram shows that 18 was being taken away, so I decided to subtract 18 from 44.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will solve Take From story problems within 100 in the context of length. In previous lessons, you learned how to measure lengths using different units of measure."],
        ["IM K–5 2021 Grade 2 Unit3 Lesson11 Saree Silk Stories: Necklaces and Bracelets PLC Activity. Students solve take-from ribbon LENGTH stories within100 using tape diagrams and base-ten blocks. Warm-up Notice and Wonder shows original picture of ribbon and diagram with54−16=?, answer38, prompting discussion of the meaning of remaining ribbon, not a fixed real ribbon measurement. Activity1 Saree Silk Ribbon Necklaces: Priya had44 inches of sari/saree silk ribbon, cuts off18 inches, remaining ribbon44−18=26in. Andre's authentic source tape diagram shows a whole and the piece removed; the '?' identifies the piece kept, and dotted line divides parts. Activity2 Saree Silk Ribbon Projects has three distinct structures. Elena starts with58in, gives Clare27, keeps31in. Han has64in and cuts off28, retains36in. Priya cuts off25in and has38in LEFT: the START is unknown, so find25+38=63in. Even though ribbon is cut, do NOT blindly subtract for Start Unknown. Use Three Reads, label tape diagram parts and explain with actual partners. Original imagery, base-ten blocks, MLR8 conversation require teacher observation; original More Saree Ribbon Cool-down is restricted and first-party independent practice is separate.",
         "A take-from story can have the result, change or START unknown. Draw a tape diagram showing original ribbon whole = removed part + remaining part. Result unknown:44in original−18in cut=26in left;58−27=31;64−28=36. Start unknown: cut25in + kept38in = original63in. In a result-unknown problem, the remaining ribbon must be shorter than starting ribbon; a start-unknown total must be longer than either known part. Subtraction within100 may involve exchanging a ten:44=30+14,44−18=26;64=50+14,64−28=36. 58−27=31 needs no ones exchange. The source warm-up54−16=38 shows the unknown after a piece was removed. Tape diagrams represent relationships, not necessarily scaled exact physical ribbon drawing. A question mark can be located over the WHOLE (unknown initial ribbon) or over the remaining PART depending on story. Source Addressing 2.MD.B.5 and 2.NBT.B.5 as approved pack; preparation also cites broad2.OA.A without a specific sub-code and does not alter this exact ordered source outcome list.",
         "PUBLIC original warm-up54−16=38: ribbon whole54, cut16, remaining38 (unknown part). PUBLIC Activity1 Priya44−18=26 inches left. PUBLIC Activity2 Elena58−27=31 inches kept; Han64−28=36 inches left; Priya cuts25in and has38in left:25+38=63 inches BEFORE cutting, a START UNKNOWN, not38−25. Teacher-led synthesis explicitly highlights that addition solves this last take-from story. First-party Edulytics independent examples: Maya's 72in ribbon minus36 leaves36; Ali cut29in and has47in left so initial76; Leah's80in strip minus25 leaves55. These are invented practice, NOT the protected More Saree Ribbon Cool-down.",
         "1. Inspect source ribbon picture, ribbon tape diagram and54−16=?, ask what is known and what question mark means:38 length remains. 2. In Activity1 use Three Reads, discuss Priya's44 inches, cut18, and remaining unknown. Place44 as whole and18 as cut part on authentic Andre diagram, mark '?' for remaining; solve44−18=26, label inches. 3. Physically model one ten exchange if necessary44=30+14 and subtract10+8. 4. Activity2 Elena: total58, gives27, result unknown31; label start, removed and remaining. 5. Activity2 Han: total64, cuts28, result unknown36; draw and compute using64=50+14. 6. Activity2 Priya: cut25 and remainder38 already known, put '?' on WHOLE and solve25+38=63. Verify63−25=38. 7. Discuss with partner why cutting does not always mean 'subtract the two numbers given'; missing quantity controls the operation. Teacher checks original diagrams, MLR8 rehearsals and real base-ten tools. 8. Finish clearly marked original Edulytics independent exit, never claiming publisher's teacher-only More Saree Ribbon Cool-down was completed.",
         "For Priya's START UNKNOWN, subtracting38−25=13 rather than adding25+38=63 is wrong. Treating every 'cut' or 'gave' narrative as a result-unknown subtraction without identifying the unknown. Placing question mark on remaining part for a start-unknown story, or on whole for a result-unknown story. Adding44+18=62 to answer what length remained instead of44−18=26. Ignoring units or confusing actual ribbon inches with physical whole tape sketch scale. Failing to trade a ten when44−18 or64−28 is computed with tens/ones; 58−27 does not require trading. Claiming first-party practice figures or written digital submissions are original printed diagrams, authentic peer discussions or protected publisher cooldown is unsupported.",
         "Source ribbon stories: warm-up54−16=38 remaining; Priya44−18=26 left, Elena58−27=31 kept, Han64−28=36 left; final Priya cut25 and has38 left so START25+38=63. Label whole, removed part, remaining part on tape diagrams and choose subtraction or addition according to unknown. Teacher reviews authentic source image, blocks and MLR8 partner talk."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&d.VersionCode=="CCSSM-2010"&&
        d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.5","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U03 L11 authentic ribbon source identity changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(CurriculumLessonContentTranslation t,bool after)=>
        Equal([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U03 L11 original six fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];t.WorkedExamples=S.After[2];
            t.StepByStepSolutions=S.After[3];t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)&&before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U03 L11 predecessor version not approved");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L11 language rows changed");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U03 L11 stored six rich fields changed");
    }
}

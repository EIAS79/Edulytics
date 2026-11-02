using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>G2 U02 L01: original IM2021 movie snacks/cube-train source
/// correction. Exact prior six fields, body SHA, source URL and addressing
/// outcomes; replaces mathematically WRONG regrouping examples.</summary>
public static class CommonCoreG2Unit2AddSubtractCompareTeachingCorrection
{
    public const string Version="us-ccss-g2-u02-l01-source-noregroup-compare-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L01";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-1/preparation.html";
    private const string BodySha="8e445fd79a998d2d470388219246bf782699e3f09f9f109c3ca3d633de7e3139";
    private sealed record Fields(string[] Before,string[] After);
    private static readonly Fields F=new(
        ["Learning goal: You will add and subtract within 100 without composing or decomposing a ten.\nUp to this point, you have only subtracted multiples of ten from other two-digit numbers within 100. In this lesson, you build on your work with interpreting bar graphs and solving Compare story problems to add and subtract within 100 with larger numbers.",
         "Key ideas:\n- Add and subtract within 100 in a way that makes sense to them without composing or decomposing a ten\n- Solve problems within 100\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: A bar graph shows 32 sunny days and 18 cloudy days. How many more sunny days were there?\nApproach: Compare the two values with subtraction: \\(32-18=14\\). There were 14 more sunny days.\n\nExample 2: A bar graph shows 27 students chose popcorn and 15 chose pretzels. How many students chose popcorn or pretzels altogether?\nApproach: Add the two categories: \\(27+15=42\\). Then compare another pair, for example \\(34-27=7\\), to answer a how-many-more question.",
         "Step 1: Read the category values from the bar graph.\nStep 2: Decide whether the question asks for a total or a comparison.\nStep 3: For a total, add the values, such as \\(27+15=42\\).\nStep 4: For how many more or how many fewer, subtract the smaller value from the larger, such as \\(32-18=14\\).\nStep 5: Check that the answer matches the graph and the wording of the question.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will add and subtract within 100 without composing or decomposing a ten. Up to this point, you have only subtracted multiples of ten from other two-digit numbers within 100."],
        ["Grade 2 Unit 2 Lesson 1: Add and Subtract to Compare. Revisit Which One Doesn't Belong through original mathematical TYPES of representations: groups of tens and ones, aligned tape diagrams, connecting-cube trains and category bar graphs. The source lesson practices TWO-DIGIT additions and subtractions within 100 WITHOUT composing or decomposing a ten. Real source Activity 1 Movie Snacks has original bar heights nachos32, pretzels16 and popcorn21. Popcorn21 plus pretzels16 gives37, and nachos32 minus popcorn21 gives11. Activity 2 Build and Compare: Lin/Clare train examples25 and15, and learners build partner trains of46 and22. Their total68 and difference24 can be demonstrated with 4 tens +2 tens =6 tens and 6 ones+2 ones=8, or via related inverse equation 22+24=46. Students may use cubes, base-ten drawings, bar/tape comparisons, a place-value strategy or another accurate representation. Partner-created cube trains and gallery walk are teacher-reviewed classroom tasks, not inferred from digital arithmetic.",
         "Exact goal: add/subtract within 100 without regrouping, in varied representations. Decompose two-digit amounts into tens and ones: 21+16: 2 tens+1 ten=3 tens, 1 one+6 ones=7 ->37. Compare 32−21: 3 tens−2 tens=1 ten, 2 ones−1 one=1 ->11. Trains46+22 give 6 tens and8 ones=68; 46−22=24 and 22+24=46. NO number needs an exchanged ten in these source examples. A bar graph's heights give categorical counts; combine named categories by addition, compare how many MORE by subtraction. A tape diagram's extra segment is a difference. The publisher lists CCSS 2.MD.D.10 and 2.NBT.B.5 as Addressing, whereas 2.OA.A.1 is Building Towards; do not add it to official mapped outcomes. Correct alternate algorithms and drawings are allowed; classroom participation and original diagrams require teacher assessment.",
         "Source math facts, independently explained in Edulytics: Original Movie Snacks bar graph displays nachos32, pretzels16, popcorn21 (single-unit scale). How many chose popcorn OR pretzels? 21+16=37. How many MORE chose nachos than popcorn? 32−21=11. A student might count on from21 to32 or subtract tens and ones separately. Original Build and Compare prompts use cube trains of46 and22: total46+22=68; gap46−22=24; addition check22+24=46. Compare familiar25 to15 in base-ten groups: gap10. First-party extra illustrations: 34+23=57 and 74−31=43, each without regrouping. Do not use the generic previous 32−18 or27+15 examples as source-aligned exercises: those require regrouping.",
         "1. Warm-up: inspect four different first-party representations of two-digit comparisons (tens and ones, tape, cube train and bar graph). Select a representation that does not belong and justify the classification; multiple well-supported answers are possible. 2. Read the three source bar heights: nachos32, pretzels16, popcorn21. Identify which categories the question names before performing an operation. 3. Popcorn and pretzels combined: split tens and ones, (20+10)+(1+6)=37. 4. Nachos compared with popcorn: (30−20)+(2−1)=11; match the gap to a bar graph or tape. 5. Build two REAL connecting-cube trains, one46 and one22, with physical towers of ten and single cubes. Count the combined total: (40+20)+(6+2)=68. Count the difference: (40−20)+(6−2)=24. 6. Verify with 22+24=46. Use a valid alternative drawing/number-line/missing-addend strategy when appropriately explained. 7. Compare partner methods and exhibit an original labeled display during the MLR7 gallery; a teacher reviews actual object building and discourse, while the independently authored digital assessment checks only numerical calculations.",
         "Not respecting 'WITHOUT composing or decomposing a ten'—for example 27+15 would require composing a ten, and 32−18 would require decomposing one. These are not suitable target examples for this initial lesson. Misreading popcorn21 as pretzels16 or using 32−16 for the wrong category comparison changes the question. Adding32+21 when the question asks how many MORE nachos than popcorn is incorrect: subtract for gap11. Forgetting that the 46/22 cube-train TOTAL68 is different from the DIFFERENCE24. Treating a cube tower of ten as one cube leads to incorrect place value. Assuming one unique 'doesn't belong' explanation for the four source representations misses valid alternatives. Treating correct digital integers as proof the student built cube trains or participated in a gallery is false; teacher assessment remains separate.",
         "Add and subtract two-digit numbers within100 WITHOUT composing or decomposing a ten. Original Movie Snacks graph nachos32, pretzels16, popcorn21: combine popcorn+pretzels37, difference nachos−popcorn11. Original partner trains46 and22: total68 and gap24, checked by 22+24=46. Explain multiple representations, verify place value, and distinguish independent Edulytics Cool-down from publisher's restricted assessment."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" && l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,BodySha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.NBT.B.5"}))
            throw new InvalidOperationException("G2 U02 L01 source/outcome identity changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool newer)=>
        FieldsEqual([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Matches(CurriculumLessonContentTranslation t,bool newer)=>
        FieldsEqual([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool FieldsEqual(string[] a,bool newer)=>a.SequenceEqual(newer?F.After:F.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var x=l.Translations.Single(t=>t.CultureCode=="en");
            if(Matches(x,true))continue;
            if(!Matches(x,false))throw new InvalidOperationException("G2 U02 L01 original six student fields changed");
            x.Explanation=F.After[0];x.KeyConceptsAndRules=F.After[1];
            x.WorkedExamples=F.After[2];x.StepByStepSolutions=F.After[3];
            x.CommonMistakes=F.After[4];x.QuickSummary=F.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))throw new InvalidOperationException("G2 U02 L01 persisted previous version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L01 unexpected persisted translations");
        var prior=rows.Single();var incoming=l.Translations.Single(t=>t.CultureCode=="en");
        if(prior.Title!=incoming.Title||!(Matches(prior,false)||Matches(prior,true)))
            throw new InvalidOperationException("G2 U02 L01 persisted student teaching fields changed");
    }
}

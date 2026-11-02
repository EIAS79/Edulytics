using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Source-locked IM2021 G2 U02 L08; actual tasks are two-digit minus two-digit.</summary>
public static class CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection
{
    public const string Version="usccss-g2-u02-l08-lin-clare-andre-two-digit-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L08";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-8/preparation.html";
    private const string Sha="7c801d163176ecc570aeaaaed398a4ebd1dbd55fafea6102c5bb35c1e50858d4";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will make sense of different methods for subtracting a two-digit number from a two-digit number and describe how the methods are the same and different.\nIn previous lessons, you used your own methods for subtracting a two-digit number from a two-digit number. In this lesson, you make sense of different subtraction methods that use base-ten drawings to represent decomposing a ten to subtract by place. You connect base-ten drawings to equations that represent the steps of a subtraction method.",
         "Key ideas:\n- Describe how methods of subtraction are the same and different when subtracting a one-digit number from a two-digit number\n- In previous lessons, you used your own methods for subtracting a two-digit number from a two-digit number\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: Find the value of each expression mentally. \\(18 + 10 + 10\\) \\(18 + 20 + 10\\) \\(38 - 20\\) \\(48 - 30\\)\nApproach: “How are the addition expressions related to the subtraction expressions?” (The second expression is the opposite of the last expression. You are in the same fact family. The first expression helped me solve the third expression because I know \\(18 + 20 = 38\\) , so \\(38 - 20\\) must be 18.)\nExample 2: Lin and Clare made base-ten diagrams to find the value of \\(71 - 56\\) . Lin Clare What do you notice about their work? What do you wonder? Lin and Clare each wrote equations to show their thinking. Explain how you know which group of equations matches Lin’s work and which matches Clare’s work. A B \\(71 - 50 = 21\\) \\(21 = 10 + 11\\) \\(11 - 6 = 5\\) \\(10 + 5...\nApproach: “How are these methods the same?” (You both decomposed a ten. You both showed subtracting 6 in the diagram. You both subtracted 5 tens.) “How are the methods different?” (Clare took away 5 tens before she drew and before she decomposed. Then she took away the ones. Lin drew tens and decomposed first, then took away ones, and then tens. You did the same things, just in a different order and drew in different...",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will make sense of different methods for subtracting a two-digit number from a two-digit number and describe how the methods are the same and different. In previous lessons, you used your own methods for subtracting a two-digit number from a two-digit number."],
        ["G2 Unit 2 Lesson 8 Different Ways to Decompose (IM K–5 2021): Compare methods for subtracting one TWO-DIGIT number from another, using place value and diagrams. Warm-up Number Talk: 18+10+10=38,18+20+10=48,38−20=18,48−30=18. In Activity 1 Aren't You Missing Something?, Lin and Clare both solve71−56=15 by decomposing a ten. Lin shows all tens and ones and trades a ten BEFORE removing six ones and five tens. Clare subtracts five tens FIRST:71−50=21; then decomposes21=10+11 and subtracts six singles to find15. Their differently sequenced drawings may be equivalent despite Clare only drawing remaining tens. Tyler is not part of this lesson. Activity 2 Different Ways to Decompose: Andre solves65−28=37 using65−20=45,45=30+15,15−8=7,30+7=37. Student practice source 34−18=16,82−37=45,71−53=18. Teacher facilitates MLR8 discussions, original diagrams and real base-ten blocks; exact numerical digital answers cannot certify authentic discourse. The preparation teacher-facing single-digit phrasing is inconsistent with its explicitly two-digit lesson purpose and tasks; actual source task mathematics governs.",
         "Decomposing a ten means replacing ten units with ten ones without changing value. When subtracting by place you may subtract tens first then regroup remaining tens/ones, or regroup first then subtract ones and tens. 71−56=15 in both sequences. Clare:71−50=21,21=10+11,11−6=5,10+5=15. Lin:71=60+11,11−6=5,60−50=10,10+5=15. The intermediate diagrams need NOT contain the same number of tens because one representation can omit tens already subtracted; it is not mathematically missing those tens from the computation. Andre:65−28=37 because65−20=45,45=30+15,15−8=7,30+7=37. Source examples34−18=16 (34=20+14),82−37=45 (82=70+12),71−53=18 (71=60+11). Warm-up inverse operations and tens place:18+20=38 and38−20=18;18+30=48 and48−30=18. Official Addressing CCSS 2.NBT.B.5 and 2.NBT.B.9.",
         "SOURCE Number Talk: 18+10+10=38;18+20+10=48;38−20=18;48−30=18. SOURCE Activity1 Lin's 71=60+11,11−6=5,60−50=10,10+5=15 corresponds to regroup-first blocks; Clare's71−50=21,21=10+11,11−6=5,10+5=15 corresponds to tens-first blocks; both find71−56=15. SOURCE Activity2 Andre65−28=37 by65−20=45,45=30+15,15−8=7,30+7=37. SOURCE student calculations:34−18=16,82−37=45,71−53=18. FIRST-PARTY Edulytics extra practice example64−29=35 (64−20=44,44−9=35). Independent Edulytics exit:72−46=26,83−57=26 and61−25=36, not the restricted publisher Whose Method is it Anyway? Cool-down.",
         "1. Mentally compute warm-up38,48,18,18, linking tens-based subtraction to known addition. 2. Study real source Lin and Clare base-ten diagrams of71−56; Lin first draws original tens, trades a ten to get6 tens and11 ones, subtracts6 singles and5 tens to reach15. 3. For Clare subtract tens FIRST:71−50=21. Represent remaining21 as1 ten+11 ones, subtract6 ones, and obtain15. Explain why Clare does NOT need to draw already removed five tens; both strategies have tracked the entire original value. 4. Follow original Andre65−28: subtract20 to get45, exchange45=30+15, subtract8 ones to reach37. 5. Independently solve34−18=16 (2 tens and14 ones),82−37=45 (7 tens and12 ones),71−53=18 (6 tens and11 ones). 6. Work with a real partner to connect both types of diagrams and equations using MLR8 language supports; teacher judges genuine representation and reasoning. 7. Complete separately authored digital exit tasks, never presenting them as the educator-only Cool-down.",
         "Claiming Clare did not subtract the full five tens because her diagram only displays the remaining21; the equation71−50=21 accounts for the five tens already removed. Writing71−56=25 or11 instead of15. Assuming both diagram approaches must look identical or remove tens and ones in the same order. Confusing 65−28 with65−20−8 without recomputing the tens and ones of45; the correct answer is37. Source student task82−37=55 is wrong; correct45. Making a ten appear without removing it from the tens place inflates the total by ten. Mistaking the single-digit phrase in one teacher-facing learning-goals bullet for the lesson's actual two-digit activity purpose. Numerical correctness does not verify MLR8 partner conversation or authentic drawn diagrams.",
         "Compare two mathematically equivalent TWO-DIGIT subtraction methods. Warm-up38,48,18,18. Lin decomposes71 before subtracting56; Clare subtracts50 first and then exchanges21 to subtract6; both result15. Andre65−28=37. Practice34−18=16,82−37=45,71−53=18. Teacher checks representations and discussion; Edulytics independent exit uses different numbers."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.NBT.B.9"}))
            throw new InvalidOperationException("G2 U02 L08 original source, hash or standards drift");
    }
    private static bool Equal(CanonicalLessonContentPackTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(CurriculumLessonContentTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Fields(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Equal(t,true))continue;
            if(!Equal(t,false))throw new InvalidOperationException("G2 U02 L08 six learner fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L08 persisted predecessor version unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L08 persisted language rows mismatch");
        var t=rows.Single();var source=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=source.Title||!(Equal(t,false)||Equal(t,true)))
            throw new InvalidOperationException("G2 U02 L08 persisted previous fields changed");
    }
}

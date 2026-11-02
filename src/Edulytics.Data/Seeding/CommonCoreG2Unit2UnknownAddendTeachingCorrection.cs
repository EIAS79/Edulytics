using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Original 2021 Grade2 U02 L02 source-bound unknown-addend and
/// base-ten/connecting-cube comparative tools. Six old fields verified.</summary>
public static class CommonCoreG2Unit2UnknownAddendTeachingCorrection
{
    public const string Version="us-ccss-g2-u02-l02-source-unknown-addend-inverse-tools-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L02";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-2/preparation.html";
    private const string BodySha="99e8a4aa53fc2789aa45a718dca857b4d20be5c669cd5807ec2d6585433032f0";
    private sealed record Fields(string[] Before,string[] After);
    private static readonly Fields F=new(
        ["Learning goal: You will find the value of unknown addends by adding or subtracting within 100.\nIn a previous lesson, you added and subtracted within 100 without composing or decomposing a ten to solve Compare problems. In this lesson, you add or subtract within 100 without composing or decomposing a ten and share the methods you use to find unknown addends. During each synthesis, you share methods based on place value and discuss the relationship between addition and subtraction.",
         "Key ideas:\n- Describe your methods using place value understanding\n- Find the unknown addend in equations within 100\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: What is the same and what is different between these tools? Find the number that makes the equation true. Show your thinking using the cubes, blocks, or drawings. \\(41 + \\underline{\\phantom{\\hspace{1.05cm}}}=84\\)\nApproach: Represent the quantities or relationships clearly, apply the lesson idea, and check that the result answers the question.\nExample 2: Han and Mai use blocks to find the number that makes the equation true. \\(17 + \\underline{\\phantom{\\hspace{1.05cm}}}=48\\) Han starts by using blocks to show 17. Show how he could find the number that makes the equation true. Mai starts by using blocks to show 48. Show how she could find the number that makes the equation true. Try this one on your own....\nApproach: “How are the methods the same? How are you different?” (Some methods are the same, you just used different tools to show it. Some methods used the same tool, but are different because one group added tens and ones to find the unknown number, but another group took away tens and ones to find the unknown number.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will find the value of unknown addends by adding or subtracting within 100. In a previous lesson, you added and subtracted within 100 without composing or decomposing a ten to solve Compare problems."],
        ["Grade 2 Unit 2 Lesson 2 — Find the Unknown Addend. Warm-up: count backward by tens from 97 to7, looking for the stable ones digit7 and the descending tens. Activity1 How Did You Find It?: compare actual connecting cubes and base-ten blocks. Both have units representing tens and ones, but fixed tens in base-ten blocks are not detachable like connecting cubes. Solve41+?=84 with the missing43: show 41+43 as (40+40)+(1+3)=84 or subtract84−41=43, without regrouping. Activity2 You Go This Way, I'll Go That Way: Han starts with17 and adds31 to reach48; Mai starts with48 and subtracts17 to find31. Then use partner tools and independent thinking to solve21+?=96 with missing75. Lesson synthesis: 67−55=12 matches55+12=67. Classmates explain which representations worked best; computer assessment checks mathematical quantities, not actual block manipulation, original drawings or partner discussion.",
         "An UNKNOWN ADDEND is a missing part of a sum. For a+b=total, b=total−a; counting on from a to total is another correct method. Use place value to count tens with tens and ones with ones, WITHOUT composing/breaking an additional ten in this source lesson. Examples: 41+43=84; 17+31=48; 21+75=96; 55+12=67. If starting with the total, 84−41=43, 48−17=31, 96−21=75. In 97,87,77,...,7, every step of minus10 changes the tens by one and preserves the ones digit7. A base-ten rod represents TEN, a unit cube represents ONE. Physical connecting-cube towers can be pulled apart whereas place-value blocks show fixed tens; both can explain one solution, but they are different tools. The 2021 source lists 2.NBT.A.2, 2.NBT.B.5 and 2.NBT.B.9 as Addressing; only these recorded ordered official outcomes are mapped.",
         "NUMBER TALK: Choral count 97,87,77,67,57,47,37,27,17,7; each term is ten less. SOURCE ACTIVITY1:41+?=84. Draw 4 tens and1 one, and think about eight tens and4 ones: the missing part is4 tens and3 ones=43. 41+43=84; 84−41=43. SOURCE ACTIVITY2: Han begins with17 and counts up 31 to48; Mai represents48 and takes away17 to leave31; same missing part, opposite directions. Next21+?=96: nine tens and six ones minus two tens and one one leave7 tens and5 ones=75. SYNTHESIS: 67−55=12 is equivalent to55+12=67. FIRST-PARTY INDEPENDENT EXIT:34+?=78 ->44; 62+?=85 ->23; 76−54=? ->22, and54+22=76. Publisher's restricted Cool-down wording/art is not used.",
         "1. Count backward ten at a time from97 to7. Read each count out loud with a partner and explain the tens/ones pattern. 2. For the original41+?=84 problem, choose connecting cubes, base-ten rods/singles or your own tens-and-ones drawing. Compare which tool makes the missing number easy to see. 3. Find the four missing tens and three missing ones: 43. Verify41+43=84. 4. Now represent17+?=48 in TWO different ways: starting from known17 and adding tens/ones (Han), and starting from total48 and subtracting17 (Mai). Each yields31. 5. Solve21+?=96 on your own, with or without the actual tools, and find75. 6. Compare a partner's representations and discuss why both addition and subtraction methods lead to identical missing amounts. Real partner conversation, original drawings and 90–100 actual cubes are teacher-observed tasks. 7. Explain why67−55=12 and55+12=67 represent the same relationship. 8. Complete an original Edulytics numeric Cool-down and a teacher-reviewed reasoning prompt; a digital correct number is not proof of real physical blocks.",
         "Treating an equals sign as an instruction to add all shown numbers: 41+?=84 asks for the MISSING addend43, not125. Subtracting 41−84 instead of 84−41 when using inverse operations. Misreading tens as loose ones or pretending a base-ten rod contains only one unit; rods show exactly ten. Writing 17+41=48 when the unknown is31. Assuming that the person who adds on and the one who takes away must give different answers; their inverse equations agree. Counting backward from97 by one instead of by ten, or changing the ones digit away from7. Using a carry/borrow example although this source explicitly avoids regrouping in this lesson. Treating student-selected tools or partner-explanation as automatically verified by digital answer checking.",
         "Source-aligned unknown-addend and place-value lesson: count back97 to7 by tens, use real cubes or base-ten blocks to find41+43=84,17+31=48 and21+75=96, and connect67−55=12 with55+12=67. The digital Edulytics Cool-down is independently authored; physical tools and partner reasoning remain teacher reviewed."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,BodySha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.A.2","CCSS:2.NBT.B.5","CCSS:2.NBT.B.9"}))
            throw new InvalidOperationException("G2 U02 L02 source identity/standard order changed");
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,bool newer)=>
        Identical([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Matches(CurriculumLessonContentTranslation t,bool newer)=>
        Identical([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],newer);
    private static bool Identical(string[] a,bool newer)=>a.SequenceEqual(newer?F.After:F.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,true))continue;
            if(!Matches(t,false))throw new InvalidOperationException("G2 U02 L02 previous six source fields mismatch");
            t.Explanation=F.After[0];t.KeyConceptsAndRules=F.After[1];
            t.WorkedExamples=F.After[2];t.StepByStepSolutions=F.After[3];
            t.CommonMistakes=F.After[4];t.QuickSummary=F.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous)=>
        IsTarget(d,l)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string previous,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,previous))throw new InvalidOperationException("G2 U02 L02 persisted previous version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L02 unexpected persisted translations");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Matches(t,false)||Matches(t,true)))
            throw new InvalidOperationException("G2 U02 L02 previous persisted teaching body mismatch");
    }
}

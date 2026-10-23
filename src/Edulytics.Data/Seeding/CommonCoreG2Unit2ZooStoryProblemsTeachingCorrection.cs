using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>Original 2021 G2 U02 L03 four zoo stories; source URL/SHA,
/// addressing standards and previous six learner fields fail closed.</summary>
public static class CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection
{
    public const string Version="us-ccss-g2-u02-l03-zoo-stories-four-no-regroup-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L03";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-3/preparation.html";
    private const string Sha="e2401085452e7a9be779a7c680b620424b66ad46fca6810c5ac4300d883467e5";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will solve story problems involving addition and subtraction within 100 without composing or decomposing a ten.\nIn previous grades, you solved a variety of Add To, Take From, Put Together/Take Apart, and Compare problems. In previous lessons, you added and subtracted within 100 without composing or decomposing a ten using methods based on place value and the relationship between addition and subtraction. In this lesson, you are invited to solve story problems and show your thinking or computations in whatever way makes sense to them.",
         "Key ideas:\n- Describe your methods using place value understanding\n- Solve story problems involving addition and subtraction within 100 without composing or decomposing a ten\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: How many do you see? How do you see them?\nApproach: “How could we represent this number another way?” (Instead of 10 ones, we could use a ten.)\nExample 2: Some students were waiting on the bus to go to the zoo. Then 34 more students got on. Now there are 55 students on the bus. How many students were on the bus at first?\nApproach: Represent the whole and the part, write a matching subtraction relationship, then find and check the difference.",
         "Step 1: Identify the whole and the part being removed or compared.\nStep 2: Represent the situation with objects, a drawing, number line, or equation.\nStep 3: Write a subtraction relationship that matches the representation.\nStep 4: Find the difference using an efficient strategy.\nStep 5: Check the result with the original quantities or a related addition fact.",
         "Common mistake: mixing up the whole and the part being removed or compared. Check that the subtraction relationship matches the representation.",
         "You will solve story problems involving addition and subtraction within 100 without composing or decomposing a ten. In previous grades, you solved a variety of Add To, Take From, Put Together/Take Apart, and Compare problems."],
        ["Grade 2, Unit 2, Lesson 3: Add or Subtract to Solve Story Problems. Solve Add To, Start Unknown, Take From, Compare and Put Together problems within 100 WITHOUT composing or decomposing a ten. Warm-up: How Many Do You See? Notice groups of ten and single ones in a base-ten diagram, discuss different ways to count the same number, and explain when ten single objects can be represented by one ten-rod. Activity 1 Fun at the Zoo uses MLR6 Three Reads: some students are already on a bus; 34 more board; 55 now ride. The START is unknown, so 55−34=21 and 21+34=55. Activity 2 More Fun at the Zoo offers three different structures: 65 at the monkey house,23 leave ->42 stay;58 at bears and27 at lions ->31 more at bears; some birds outdoors and34 indoors,88 altogether ->54 outside. Students choose base-ten blocks, connecting cubes, a tape diagram, equations or a self-created drawing. Mathematical responses can be digitally checked, but the reading routine, the student's original representation and partner explanation require a teacher.",
         "First determine the story structure and the unknown rather than reflexively computing any two mentioned numbers. Add To, Start Unknown: start+34=55 means start=21. Take From, Result Unknown: 65−23=42. Compare, Difference Unknown: 58−27=31. Put Together, Part Unknown: outside+34=88 means outside=54. All four original examples are within 100 and require NO exchange of tens, because in every subtraction the ones of the starting amount are at least the ones being taken away. Think in tens and ones: 55−34=(50−30)+(5−4)=21; 65−23=40+2=42; 58−27=30+1=31; 88−34=50+4=54. Verify with addition. A drawing must reflect what whole and part mean, not merely show any convenient number. MLR6 Three Reads separates understanding the story, identifying quantities, and stating the question.",
         "PUBLIC SOURCE Activity1 Fun at the Zoo: read a start-unknown bus story. 34 more board, 55 now present, so first present 55−34=21. Equation ?+34=55 and checking 21+34=55. PUBLIC SOURCE Activity2: monkey house65−23=42 still inside; bears58−lions27=31 more at bears; birds altogether88−indoors34=54 outdoors. Read full question to decide operation and write an accurate relationship. Edulytics-original alternative examples for extra digital practice (NOT claimed publisher tasks): 76−25=51,43−21=22 and total=26+43=69, all without ten regrouping. Independently authored digital exit: 74−32=42, 67−25=42, 86−43=43; these are mathematical checks, not copies of the restricted 'Time to Leave' Cool-down.",
         "1. For the original warm-up, count tens-and-ones blocks, share more than one visual decomposition and discuss why ten ones may be grouped as a ten. The image discussion is teacher observed. 2. Read the bus story THREE times: first understand who is riding and what happened; next identify the known 34 added and 55 total; third reveal the question about the unknown starting number. 3. Model start+34=55 with a tape bar, ten-rods and singles, a drawing or a number line. Subtract 55−34 as 5 tens−3 tens and5 ones−4 ones to get21. Verify21+34=55. 4. Monkey house: 65−23=(60−20)+(5−3)=42; say42 people remain. 5. Bears compared to lions: 58−27=(50−20)+(8−7)=31; say31 more students visited bears. 6. Birds: 88−34=54 outdoor birds; verify54+34=88. 7. Ask a partner to compare tools and methods; teacher checks whether representation, labeling and prose faithfully show the story. 8. Solve a separate Edulytics original cool-down without regrouping. Never claim a correct digital number certifies real source partner discourse.",
         "Mistaking bus 'then 34 more got on' for a request to add 34+55, which would not find the starting number; the 55 is the TOTAL. Treating a compare problem with bears58/lions27 as a combined-total question, rather than difference31. Reporting monkey house65−23=88 or using 88−34=44 for birds (correct is54). Assuming every story should be solved by subtraction even if question actually asks for a total. Requiring ten regrouping in source tasks where every ones-column subtraction is legal without borrowing. Counting ten-rod symbols as individual ones. Presenting a prefilled digital diagram as a student's own physical cubes or actual partner explanation. Automatically accepting a story drawing or the MLR6 routine solely because a numeric answer was right.",
         "Interpret the story BEFORE calculating. Source zoo tasks: bus starts21 then34 board to reach55;65−23=42 monkey-house students remain;58−27=31 more students at bears;88−34=54 birds outdoors. Explain tens and ones without regrouping, check with addition, compare student-chosen representations, and complete a separate independently authored Edulytics exit."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
           !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L03 original source/outcome identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation x,bool after)=>
        EqualsSix([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation x,bool after)=>
        EqualsSix([x.Explanation,x.KeyConceptsAndRules,x.WorkedExamples,x.StepByStepSolutions,x.CommonMistakes,x.QuickSummary],after);
    private static bool EqualsSix(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(en,true))continue;
            if(!Same(en,false))throw new InvalidOperationException("G2 U02 L03 original learner fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L03 persisted version mismatch");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L03 persisted translations mismatch");
        var en=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(en.Title!=incoming.Title||!(Same(en,false)||Same(en,true)))
            throw new InvalidOperationException("G2 U02 L03 persisted learner predecessor changed");
    }
}

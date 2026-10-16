using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Explicit original-source comparison lessons, IM first edition 2021.
/// Read authorized teacher preparation and full student-facing lesson activities.
/// Not equivalent to independent academic approval.
/// </summary>
public static class CommonCoreG1Unit2Part4CompareSourceCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-l11-l12-original-compare-teaching-v1";
    private sealed record Repair(string Code,string SourceUrl,string Sha,string[] Outcomes,
        string PriorStepsPrefix,string PriorExamplesPrefix,string Explanation,
        string Concepts,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs=[
        new("PED:US-CCSS-MATH:G1:U02:L11","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-11/preparation.html","b0774a01ae8cf0db4cbde91a4fc46f50f42cd778964e6786a268701fe345b568",
            ["CCSS:1.OA.A.1"],
            "Step 1: Identify the two quantities, 3 and 7.\nStep 2: Start at 3 ",
            "Example 1: The source task combines 3 and 7. Start at 3 and count",
            "Today we compare two towers of cubes and work out how to make their heights equal. If one tower has more cubes than another, we can add cubes to the shorter tower or remove cubes from the taller tower. The number added or removed is the difference. Use drawings or cubes to show how you know.","Learning goal: Solve Compare, Difference Unknown problems using two towers and explain the unknown difference.\nCompare nine blue cubes and five red cubes. You can add four to the five-cube tower, or remove four from the nine-cube tower. Both show a difference of four: 5 + [4] = 9 and 9 - 5 = [4].\nUnlike a Take From story, no cubes have actually left yet: you are comparing two groups. The answer is the amount needed to make them equal.","Example 1 — Make two towers equal. A blue tower has 9 cubes and a red tower has 5. Line up their bottoms. How many cubes does red need to reach blue? Add 6, 7, 8, 9: four cubes. Write 5 + [4] = 9.\nExample 2 — Remove from the taller tower. Two towers have 8 and 3 cubes. Remove five cubes from the taller tower to match the shorter one. 8 - [5] = 3, so the difference is five.\nExample 3 — Two methods, one difference. Towers have 7 and 4 cubes. Count on from four to seven, or subtract 7 - 4. Both methods give three cubes. Explain why the same difference is found.",
            "Step 1: Show both quantities using cube towers or two labelled drawings, with their bottoms aligned.\nStep 2: Identify the taller and shorter groups and ask how many more or fewer.\nStep 3: Count cubes to add to the shorter tower or remove from the taller to make them equal.\nStep 4: Write an equation such as 5 + [4] = 9 or 9 - 5 = [4] and box the difference.\nStep 5: Explain the same comparison using the other method and check the two towers would match.","Do not add 9 + 5 when the question asks how many taller one group is; the answer is the difference, not the total.\nDo not treat the original picture as a Take From story: neither tower changed until you choose how to equalize them.\nDo not count the same five shared cubes as the difference. Count only the extra cubes in the taller tower.","I can compare two cube towers, explain how to make them equal, and use addition or subtraction to find the difference."),
        new("PED:US-CCSS-MATH:G1:U02:L12","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-12/preparation.html","4fce1045bbb704d1f2d1ce0a587ba65c6a68a757879d6ab0bedfb083216a67cb",
            ["CCSS:1.OA.A.1","CCSS:1.OA.C.5"],
            "Step 1: Represent the quantities or objects in the task.\nStep 2: ",
            "Example 1: “How is their work the same? How is it different?” (Th",
            "Today we solve stories asking how many more are needed or how many fewer there are. Use three reads to understand the situation, identify the two quantities, and notice how the question is worded. A story saying there are not enough pencils asks for the difference between students and pencils, not the total.","Learning goal: Solve Compare, Difference Unknown problems in story contexts and relate counting to addition and subtraction.\nIf nine students need a pencil but there are only seven pencils, two students will not have one. Write 7 + [2] = 9 or 9 - 7 = [2].\nThe boxed number represents the difference even if it appears before the equals sign. Count on from seven to nine or subtract nine minus seven. Support the answer with matched objects.","Example 1 — Not enough pencils. Nine students each need one pencil. There are seven pencils. Match one pencil to each student until the pencils run out; two students remain. Write 7 + [2] = 9 or 9 - 7 = [2].\nExample 2 — How many more? Eight children brought books and five brought markers. How many more brought books than markers? Line up eight counters against five, and count three extras. Equation: 8 - 5 = [3].\nExample 3 — Explain two equations. A shelf has nine notebooks and seven folders. Difference: two. Write 7 + [2] = 9 and 9 - 7 = [2]. Identify why two is boxed both times even though the equations look different.",
            "Step 1: Read the story three times: understand the context, find the quantities, and identify the question.\nStep 2: Model both groups with a one-to-one match, counters, or a labelled drawing.\nStep 3: Ask how many are not matched, how many more, or how many fewer.\nStep 4: Count on or subtract to find the difference, then write an equation with the answer boxed.\nStep 5: Check the matching objects and explain how both an addition and a subtraction equation describe the same comparison.","Do not find the total number of pencils and students when asked how many students will not get one: 9 + 7 is unrelated.\nDo not mistake the seven available pencils for the number missing; compare them to the nine students.\nDo not assume the answer must be after =. In 7 + [2] = 9, two is the actual answer to 'how many more?'.","I can read a comparison story, match the two groups, find how many more or fewer, and explain the boxed difference with addition or subtraction.")
    ];
    public static IReadOnlyCollection<string> TargetLessonCodes=>Repairs.Select(r=>r.Code).ToArray();
    private static Repair? Find(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase29-source-intelligence-en-v2-g1"
            ? Repairs.SingleOrDefault(r=>r.Code==l.LessonCode) : null;
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        Find(d,l)!=null;
    private static void Check(CanonicalLessonContentPackLesson l,Repair r)
    {
        if(l.SourceUrl!=r.SourceUrl||
           !string.Equals(l.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase)||
           !l.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException($"Original comparison source drift {l.LessonCode}");
    }
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        if(d.PackCode!=MathematicsCurriculumPackRegistry.CommonCoreCode||d.VersionCode!="CCSSM-2010")return;
        foreach(var l in d.Lessons)
        {
            var r=Find(d,l);if(r is null)continue;Check(l,r);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(t.Explanation==r.Explanation&&t.KeyConceptsAndRules==r.Concepts&&
               t.WorkedExamples==r.Examples&&t.StepByStepSolutions==r.Steps&&
               t.CommonMistakes==r.Mistakes&&t.QuickSummary==r.Summary)continue;
            if(!t.StepByStepSolutions.StartsWith(r.PriorStepsPrefix,StringComparison.Ordinal)||
               !t.WorkedExamples.StartsWith(r.PriorExamplesPrefix,StringComparison.Ordinal))
                throw new InvalidOperationException($"Unrecognized previous comparison lesson: {l.LessonCode}");
            t.Explanation=r.Explanation;
            t.KeyConceptsAndRules=r.Concepts;
            t.WorkedExamples=r.Examples;
            t.StepByStepSolutions=r.Steps;
            t.CommonMistakes=r.Mistakes;
            t.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string prior)=>IsTarget(d,l)?CorrectedContentVersion:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string oldVersion)=>
        IsTarget(d,l)&&oldVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Find(d,l)??throw new InvalidOperationException("Not source-scoped lesson");
        Check(l,r);
        if(!CanUpgradeExisting(d,l,version))
            throw new InvalidOperationException($"Wrong persisted comparison content version: {l.LessonCode}");
        if(existing.Count==0)return;
        if(existing.Count!=1)throw new InvalidOperationException("Unexpected translations");
        var t=existing.Single();
        if(t.CultureCode!="en"||
           !t.StepByStepSolutions.StartsWith(r.PriorStepsPrefix,StringComparison.Ordinal)||
           !t.WorkedExamples.StartsWith(r.PriorExamplesPrefix,StringComparison.Ordinal))
            throw new InvalidOperationException($"Persisted comparison prose drift: {l.LessonCode}");
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>G2 U03 L01 source-locked original 40cm ribbon measuring exploration.</summary>
public static class CommonCoreG2Unit3StandardUnitsTeachingCorrection
{
    public const string Version="usccss-g2-u03-l01-priya-standard-cm-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L01";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-1/preparation.html";
    private const string Sha="973bb81c2c24fcfea9547ae7f84d7b3b6929beb0d77930cdbae64b9f1df623bb";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will measure by iterating same-size length units and identify the need for standard units of measurement.\nIn this lesson, you use these measuring skills to measure the length of objects by iterating with straws and centimeter cubes. In the first activity, you use different length units when measuring the same objects and see that this leads to different measurements. In the second activity, all you use the same length unit (centimeter cubes) and see you all find the same measurement.",
         "Key ideas:\n- Measure by iterating same-size length units\n- In this lesson, you use these measuring skills to measure the length of objects by iterating with straws and centimeter cubes\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Use straws to measure the string that shows how long Priya's iguana is. Priya’s iguana is ___ straws long.\nApproach: “Why are our length measurements different?” (Some groups maybe didn’t line up your straws correctly. Maybe the straws weren’t the same size.)\nExample 2: Use the cubes to measure Priya’s string. Priya’s iguana is ___ cubes long. Compare your measurement with another group.\nApproach: “Why is every group's measurements the same, or closer to the same, length?” (You are the same because we all used the same length unit. We all used cubes that have the same length.) “If some groups used these cubes and other groups used connecting cubes, would our measurements have been the same? Why?” (It would not be the same because the cubes are not the same length.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will measure by iterating same-size length units and identify the need for standard units of measurement. In this lesson, you use these measuring skills to measure the length of objects by iterating with straws and centimeter cubes."],
        ["IM K–5 2021 Grade2 Unit3 Lesson1 Standard Units of Measure: in this transition from grade1 nonstandard measurements, students physically iterate equal-size length units and discover why standard units matter. Warm-up What Do You Know About Measuring? invites discussion of what measuring means and where to begin and end. Activity1 Priya's Pet: teacher prepares 40 cm string standing for Priya's iguana's length and gives half the groups five 10-cm straws and the other half ten 5-cm straws. If lined up with no gaps or overlaps, the 40-cm string measures FOUR long straws or EIGHT short straws, despite being the SAME physical length. Classmates compare methods, not merely answers. Activity2 Use a Standard Unit: groups use uniform 1-cm cubes on that same prepared string and should count 40 cubes, or40 centimeters. Unlike the larger connecting cubes or varying straw lengths, all true centimeter cubes have edges precisely1 cm, so different careful groups agree. Teacher observes authentic string, hand placement, no gaps/overlaps, MLR8 gestures and actual peer comparison. Independently authored digital arithmetic and original visual schematic cannot certify hands-on measuring.",
         "Measurement is the number of ITERATED equal-length units from the exact start to exact end of the object. Every unit must line up end-to-end without overlaps, spaces or a shifted zero endpoint. The same length can have DIFFERENT numerical measurements when unit lengths differ: 40-cm prepared ribbon contains4×10-cm lengths or8×5-cm lengths. A smaller length unit gives a larger count, not a longer iguana. A true centimeter cube's edge measures 1 centimeter, a standard metric length unit: 40-cm ribbon measures40 centimeter cubes. Ordinary connecting cubes and centimeter cubes can have different edge lengths; never label all toy cubes 1 cm unless verified. Physical measurement and precise manipulation remain teacher-reviewed. Source teacher calls 2.MD.A as addressing domain and2.MD.A.1/2 building-toward; the curriculum pack ordered codes are 2.MD.A.1,2.MD.A.2.",
         "ORIGINAL teacher preparation: forty-centimeter string for each real group; use five 10-cm straws for half of groups and ten 5-cm straws for others. If measured correctly on one 40-cm string:4 straws of10 cm each or8 straws of5 cm each; same physical length but differing unit counts. SOURCE Activity2: measure same string with uniform one-centimeter base-ten unit cubes:40 equal centimeter cubes, hence40 cm. SOURCE Synthesis says 'Priya's iguana is40 cubes long' versus more precise '40 centimeter cubes long'. FIRST-PARTY Edulytics numeric analog: a30-cm line uses6 pieces of5 cm, or3 pieces of10 cm, or30 exact1-cm cubes. No source protected physical cool-down 'Measure a Rectangle' copied.",
         "1. Warm-up discuss what length means, where you start measuring, and the importance of measuring one dimension consistently. 2. For the actual teacher-prepared 40-cm string, identify the start and end and place real 10-cm straws end-to-end with no gaps/overlaps. Count4 of them. 3. Another real group measures the SAME string using5-cm straws placed correctly and counts8; observe and explain why numbers differ though string does not change. 4. In authentic Activity2 all groups use uniformly sized centimeter cubes, each true 1-cm edge. Start at the end and line up40 cubes with no gaps or overlaps. 5. Explain that the length is 40 centimeters (40 cubes each1 cm), not forty unspecified toy cubes. Teacher compares centimeter and connecting cubes to show they may be DIFFERENT sizes. 6. In MLR8 real discussion explain why a unit's LENGTH, not just its name, changes the count, and how standard units improve comparability. 7. Complete only independently authored Edulytics numeric examples of interval counts; typed counts never assert the student held a ruler, straw, iguana or cube.",
         "Reporting a straw count without specifying whether each straw was5 or10 cm; these counts are not directly comparable. Claiming8 short straws represent a longer ribbon than4 long straws although both measure40 cm. Leaving gaps or overlapping real units, or not starting from the exact string endpoint. Mistakenly reporting40 10-cm straws for the40-cm string; correct4. Assuming every classroom connecting cube is a centimeter cube with a true1-cm edge. Confusing prepared 40-cm string standing for an iguana with actual measurement of a live animal. Treating an online numerical analogy as a certified measurement of the physical classroom materials.",
         "Priya's Pet (original IM2021): a prepared40-cm string is4 correctly laid 10-cm straws,8 5-cm straws, or40 true1-cm cubes. Standard centimeter unit allows consistent results; physical placement without gaps/overlap and authentic groups require teacher review. Independent first-party examples are separate from source physical tasks."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.A.1","CCSS:2.MD.A.2"}))
            throw new InvalidOperationException("G2 U03 L01 source URL/hash/outcomes changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Fields(string[] x,bool after)=>x.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U03 L01 previous six source learner fields changed");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)?Version:old;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old)=>
        IsTarget(d,l)&&old==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string old,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,old))throw new InvalidOperationException("G2 U03 L01 previous content version unknown");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L01 persisted translation mismatch");
        var t=rows.Single();var inc=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=inc.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U03 L01 source predecessor learner text changed");
    }
}

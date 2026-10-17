using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Exact source-/body-guarded Grade 1 Unit 1 L02-L03 student-teaching repair.
/// Independently authored Edulytics explanations and representations, checked
/// against archived IM 2021 learning goals and lesson activities as references.
/// These are NOT automatic historic release drift approval or P5 certification.
/// </summary>
public static class CommonCoreG1Unit1L02L03SourceTeachingCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u01-l02-l03-source-story-dot-centers-v1";
    private sealed record Repair(
        string Code,string Url,string CanonicalSha,string[] Outcomes,
        string OldFirstStep,string OldFirstExample,
        string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);

    private static readonly Repair[] Repairs =
    [
        new(
            "PED:US-CCSS-MATH:G1:U01:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html",
            "7d8c9aa59717d264f80522bcf07c5385b5f332b2808b66c5c75fb19672c70e33",
            ["CCSS:1.OA.C.5","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 3 and 2.",
            "Example 1: The source task combines 3 and 2.",
            "Look at two groups of dots, say how many are in each group, and write an addition expression that tells how to combine them. Count all the dots or count on from one group to find the sum. You can draw the dots yourself to show what an expression means. In the Check it Off center, choose a total or expression and check your work with a partner.",
            "Learning goal: Match two groups of dots to an addition expression within 10, show missing dots in a picture, and find the sum. An expression such as 3 + 2 names two addends; the sum is 5. It is okay to draw two groups of dots and point to each number. A different order such as 2 + 3 has the same total, but the groups and the picture still need to match.",
            "Example 1 - Match the dots. Draw three filled circles next to two open circles: three dark dots and two light dots. The first group has 3, the second has 2, and together they make 5: 3 + 2 = 5. Say why both addends match the picture.\n" +
            "Example 2 - Draw what is missing. You are given the expression 4 + 2. Draw four dots in one group, then two different-looking dots next to them. Count the six dots and record that the expression is 4 + 2 and the sum is 6.\n" +
            "Example 3 - Count on to check. To find 5 + 3, say 5, then count three more: 6, 7, 8. Draw five squares and three circles; the sum is 8, so 5 + 3 = 8.\n" +
            "Example 4 - Make ten. Show six dots in one color and four dots in another. Write 6 + 4 = 10; count each group separately, then count the whole group.",
            "Step 1: Count each dot group separately and say both numbers.\n" +
            "Step 2: Write the addition expression using one addend for each group, such as 3 + 2.\n" +
            "Step 3: Draw any missing dots or use counters in two groups, keeping the groups distinct.\n" +
            "Step 4: Find the sum by counting all dots or counting on from one addend: 3 + 2 = 5.\n" +
            "Step 5: Point to each part of the model, check that your expression matches the groups, and explain your sum to a partner.",
            "Mistake: drawing three dots and two dots but recording 3 + 3. Check each group count before writing the expression.\n" +
            "Mistake: reading 3 + 2 as the sum 3. The expression names the parts; counting the whole group gives 5.\n" +
            "Mistake: writing a sum without showing the groups. A matching picture or pair of dot cards explains what the addends represent.",
            "I can match dot groups to an addition expression, draw dots for both addends, find the sum within 10, and explain how the picture matches the numbers."
        ),
        new(
            "PED:US-CCSS-MATH:G1:U01:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-3/preparation.html",
            "387f0efc2ab5c5bf845363860b203c0d48bbce4c1df4612cbef5470dff9275b0",
            ["CCSS:1.OA.C.5","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 6 and 2.",
            "Example 1: The source task combines 6 and 2.",
            "Today we play Five in a Row and use counting on to add 1 or 2 within 10. When a number grows by one, say the very next number; when it grows by two, count two more steps. Show what happens with counters, a ten-frame or a short number path, and tell a partner why the result matches the expression.",
            "Learning goal: Add 1 or 2 to numbers while staying within 10 and explain the counting-on strategy. Adding 1 moves one step forward: 5 + 1 = 6. Adding 2 moves two steps forward: 5 + 2 = 7. In Five in a Row, use a number or expression card, work out the total, and cover the matching number in your row. Taking turns and checking answers are part of the center.",
            "Example 1 - Add two. Begin with six counters. Put down one more, then another: 6, 7, 8. So 6 + 2 = 8. Check with a ten-frame: six filled boxes plus two more are eight.\n" +
            "Example 2 - Add one. Place four dots on a number path. Move one step to 5, so 4 + 1 = 5. If the expression is 1 + 4, the total is still 5; count on from the four.\n" +
            "Example 3 - Count two steps. Start at 7 and say 8, 9. Record 7 + 2 = 9. Draw seven squares and two more circles as a second check.\n" +
            "Example 4 - Five in a Row. Your card shows 8 + 1. Say the next number 9, check with a partner, and cover the 9 space. Another card with 6 + 2 needs two counts, 7 and 8.",
            "Step 1: Read the expression; find the group you already know and whether you add 1 or 2.\n" +
            "Step 2: Keep the starting number in your head or show it with counters.\n" +
            "Step 3: Count exactly one or two steps forward. For 6 + 2, say 7, 8.\n" +
            "Step 4: Record the sum and show the added dot(s) on a number path or ten-frame.\n" +
            "Step 5: In Five in a Row, cover the number that matches your sum; swap turns and explain how you checked.",
            "Mistake: starting at 1 and counting all the way to 8 for 6 + 2. Start at 6 and count on two steps, 7 and 8.\n" +
            "Mistake: counting the starting number as the first step. For 5 + 2 the two new numbers are 6 and 7, not 5 and 6.\n" +
            "Mistake: treating Five in a Row as guessing. Find the sum first, then cover the matching result and check with your partner.",
            "I can add 1 or 2 within 10 by counting on, model the addition with dots or a ten-frame, and explain my answer during Five in a Row."
        )
    ];

    public static IReadOnlyList<string> TargetLessonCodes =>
        Repairs.Select(x=>x.Code).ToArray();

    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode=="US-CCSS-MATH" && doc.VersionCode=="CCSSM-2010" &&
        doc.ContentVersion=="phase29-source-intelligence-en-v2-g1"
          ? Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode) : null;

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;

    private static void Check(CanonicalLessonContentPackLesson lesson,Repair r)
    {
        if(lesson.SourceUrl!=r.Url ||
           !string.Equals(lesson.CanonicalBodySha256,r.CanonicalSha,
               StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes) ||
           lesson.Translations.Count!=1 ||
           lesson.Translations.Single().CultureCode!="en")
            throw new InvalidOperationException(
                "G1 U01 L02/L03 source/SHA/outcome/culture drift: "+lesson.LessonCode);
    }

    private static bool Matches(CanonicalLessonContentPackTranslation en,Repair r)=>
        en.Explanation==r.Explanation &&
        en.KeyConceptsAndRules==r.Concepts &&
        en.WorkedExamples==r.Examples &&
        en.StepByStepSolutions==r.Steps &&
        en.CommonMistakes==r.Mistakes &&
        en.QuickSummary==r.Summary;

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH" || doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            var r=Find(doc,lesson);
            if(r is null)continue;
            Check(lesson,r);
            var en=lesson.Translations.Single();
            if(Matches(en,r))continue;
            if(!en.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
               !en.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
                throw new InvalidOperationException(
                    "G1 U01 L02/L03 cannot overwrite unknown previous learner body: "+
                    lesson.LessonCode);
            en.Explanation=r.Explanation;
            en.KeyConceptsAndRules=r.Concepts;
            en.WorkedExamples=r.Examples;
            en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;
            en.QuickSummary=r.Summary;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion)=>
        IsTarget(doc,lesson) &&
        existingVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,
        string existingVersion,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not a G1 U01 L02/L03 target");
        Check(lesson,r);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException(
                "Unrecognized prior persisted content version for "+lesson.LessonCode);
        if(translations.Count==0)return; // JSON-body-only production rows
        if(translations.Count!=1)
            throw new InvalidOperationException("Unexpected persisted culture row count");
        var en=translations.Single();
        if(en.CultureCode!="en" ||
           !en.StepByStepSolutions.StartsWith(r.OldFirstStep,StringComparison.Ordinal) ||
           !en.WorkedExamples.StartsWith(r.OldFirstExample,StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Persisted learner text drift for "+lesson.LessonCode);
    }
}

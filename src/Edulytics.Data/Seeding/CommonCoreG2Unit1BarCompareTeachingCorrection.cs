using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Guarded source-driven original IM2021 Grade2 U01 L13. Adds missing
/// counting-on/back, compare statements and bar equations to generic prose.
/// Requires canonical source URL/SHA/outcome and original stored fields.
/// </summary>
public static class CommonCoreG2Unit1BarCompareTeachingCorrection
{
    public const string Version="us-ccss-g2-u01-l13-count-on-and-back-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U01:L13";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-13/preparation.html";
    private const string Sha="9ad5c632c0ab7878a54dda57f7331315a2d10a11e2542fd2d2a4d94640b6c5de";
    private sealed record SourceProse(
        string OldExplanation,string OldRules,string OldExamples,string OldSteps,
        string OldMistakes,string OldSummary,string NewExplanation,string NewRules,
        string NewExamples,string NewSteps,string NewMistakes,string NewSummary);
    private static readonly SourceProse Source=new(
        "Learning goal: You will make sense of and solve Compare problems using the structure of a bar graph and equations.\nIn grade 1, you use the relationship between addition and subtraction to solve and represent Compare problems. You create diagrams and write equations to represent how you think about the quantities within the Compare problems. In this lesson, you interpret bar graphs and use your visual structure to compare quantities.",
        "Key ideas:\n- Solve Compare problems with an unknown difference within 20\n- Write equations to represent Compare problems\n- Read category labels, values, and scales before comparing or calculating with a data display.",
        "Example 1: Decide if each statement is true or false. Be prepared to explain your reasoning. \\(9 + 4 = 9 + 1 + 3\\) \\(9 + 4 = 10 + 3\\) \\(9 + 5 = 10 + 6\\)\nApproach: \"How can you justify your answer without adding?\" (I see that 9 + 4 and 9 + 1 + 3 are the same because 1 + 3 = 4.)\nExample 2: A group of third grade students were asked, \"What pets do you have?\" Their responses are shown in the bar graph. What do you notice? What do you wonder? Their responses are also shown in this bar graph. How many more students have cats than have rabbits? Show two ways to find the difference.\nApproach: \"How are your methods similar? How are your methods different?\" (One method starts with the smaller number of pets and counts on to the larger number, like adding 9 more. The other starts with the larger number of pets and counts back to the smaller number, like subtracting 9.)",
        "Step 1: Read the title, labels, categories, and scale.\nStep 2: Locate the values needed for the question.\nStep 3: Compare, add, subtract, or interpret the values as required.\nStep 4: State the result in the context of the data.",
        "Common mistake: reading the wrong category or scale. Check the labels and scale before using the values.",
        "You will make sense of and solve Compare problems using the structure of a bar graph and equations. In grade 1, you use the relationship between addition and subtraction to solve and represent Compare problems.",
        "Grade 2 Unit 1 Lesson 13 teaches comparing TWO category quantities from the structure of a BAR graph and making related addition AND subtraction equations, with the unknown DIFFERENCE no more than 20. Warm-up: decide whether 9+4=9+1+3, 9+4=10+3 and 9+5=10+6 are true; justify by composing ten without recalculating both sides. Activity 1 What's the Difference?: read a first-party horizontal two-category pets bar graph. The original public lesson compares a cat bar of 17 and a rabbit bar of 8, asking how many more cats than rabbits: count ON from 8 to 17 or BACK from 17 to 8; link 8+9=17 to 17−8=9. Initially children describe differences from an unnumbered version; then read the axis to verify. Activity 2 Dogs in the Park: compare four bar categories (source data poodle 6, bulldog 20, pug 8, husky 14), write TRUE more/fewer comparison statements, and show their unknown differences with a related addition and subtraction equation. Teacher observes two counting strategies, labels, physical discussion and explanations; an independent Edulytics Cool-down tests the same compare skill without reproducing the publisher-restricted assessment.",
        "Read each BAR graph title and category labels; check the axis starting at zero and unit-one tick intervals. Bars must represent quantities in the SAME survey when comparing them.\nA MORE/Fewer comparison compares two category counts. 'How many MORE cats than rabbits?' with cats 17 and rabbits 8 asks for the UNKNOWN DIFFERENCE 9, not the total 25.\nCount ON: begin at the smaller 8 and count to the larger 17. The nine extra steps show 8+9=17. Count BACK: begin at 17 and take away the smaller 8 to get 9; record 17−8=9.\nThe original dogs graph has poodle 6, bulldog 20, pug 8, husky 14. Therefore bulldogs exceed huskies by 6 (14+6=20 and 20−14=6); pugs exceed poodles by 2 (6+2=8 and 8−6=2).\nA truthful learner statement identifies which category is larger and how many more/fewer. Each difference must match the graph's horizontal bar lengths. A sum of ALL categories is not automatically a compare problem.\nA correct question/statement may be expressed in many words or with two equivalent equations; teacher checks meaning and student explanation, not only an integer.",
        "Warm-up source facts: 9+4=9+1+3 (true because 1+3=4). 9+4=10+3 (true by making one ten). 9+5=10+6 (false; 14 is not 16).\nOriginal public source Activity 1 numerical relationship (new Edulytics graphic): rabbit 8 and cat 17. Difference is 9: count on 8,9,...,17 or count back. Equations 8+9=17 and 17−8=9.\nOriginal public source Activity 2 numerical relationships (new Edulytics graphic): poodle 6, bulldog 20, pug 8, husky 14. There are 6 more bulldogs than huskies, written 14+6=20 and 20−14=6; there are 2 more pugs than poodles, written 6+2=8 and 8−6=2.\nIndependent Edulytics exit example, different data: a hypothetical playground-choice graph has climbing 13 and sliding 7. How many fewer sliding? 13−7=6; 7+6=13.",
        "Step 1: Check the bar graph TITLE, categories and scale. Start with the unnumbered lengths as a visual comparison when offered, then read the numbered axis to find the exact counts.\nStep 2: Read the two named bars from the SAME chart. Decide which has MORE and which has FEWER; write a truthful sentence using their labels.\nStep 3: Mark or imagine the difference between the shorter bar's end and longer bar's end. COUNT ON from the smaller value to the larger, and record smaller + missing difference = larger.\nStep 4: Check by COUNTING BACK from the larger number or SUBTRACTING the smaller number. Record larger − smaller = missing difference.\nStep 5: In the original pets graph, read rabbit 8 and cat 17; show BOTH 8+9=17 and 17−8=9. Discuss why both describe the same difference.\nStep 6: In the four-category dogs graph, compare at least TWO distinct pairs. Write 'more than' and 'fewer than' statements and BOTH related equation forms for each pair; if categories have equal counts then the difference is zero.\nStep 7: Explain the number in the equation as a real CATEGORY count or a DIFFERENCE, and compare a partner's counting-on and counting-back strategies. The teacher validates original wording, drawings and explanation.",
        "Mistake: adding category counts when asked 'how many MORE/FEWER'; a comparison requires the difference (17−8=9), not the combined total 25.\nMistake: describing '8 more rabbits than cats' when the cat bar is longer; check category order and label before selecting 'more' or 'fewer'.\nMistake: using a chart's upper axis maximum as its bar height; pugs are 8, huskies 14 and bulldogs 20.\nMistake: subtracting the wrong parts of two different survey populations or treating unnumbered preliminary bars as exact scale values. Wait for the correctly labeled scale.\nMistake: recording only one calculation in the original source activity that calls for two methods. Connect counting ON and counting BACK with matching addition/subtraction equations.\nMistake: checking only an entered integer and claiming to have observed the student's comparison language, graph drawing, or real partner strategy.",
        "Use bar graph lengths and numbered axes to compare category quantities within 20. Make true 'more/fewer' statements, find the unknown difference by counting ON and counting BACK, and write the related addition and subtraction equations. The original pets/dogs source facts are redrawn independently; original student speech and strategy remain teacher-reviewed.");

    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        d.VersionCode=="CCSSM-2010" &&
        d.ContentVersion=="phase30-common-core-rich-content-v1" &&
        l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.D.10","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("Grade2 U01 L13 original source drift; refusing overwrite");
    }
    private static bool Match(CanonicalLessonContentPackTranslation t,bool isNew)=>
        t.Explanation==(isNew?Source.NewExplanation:Source.OldExplanation) &&
        t.KeyConceptsAndRules==(isNew?Source.NewRules:Source.OldRules) &&
        t.WorkedExamples==(isNew?Source.NewExamples:Source.OldExamples) &&
        t.StepByStepSolutions==(isNew?Source.NewSteps:Source.OldSteps) &&
        t.CommonMistakes==(isNew?Source.NewMistakes:Source.OldMistakes) &&
        t.QuickSummary==(isNew?Source.NewSummary:Source.OldSummary);
    private static bool Match(CurriculumLessonContentTranslation t,bool isNew)=>
        t.Explanation==(isNew?Source.NewExplanation:Source.OldExplanation) &&
        t.KeyConceptsAndRules==(isNew?Source.NewRules:Source.OldRules) &&
        t.WorkedExamples==(isNew?Source.NewExamples:Source.OldExamples) &&
        t.StepByStepSolutions==(isNew?Source.NewSteps:Source.OldSteps) &&
        t.CommonMistakes==(isNew?Source.NewMistakes:Source.OldMistakes) &&
        t.QuickSummary==(isNew?Source.NewSummary:Source.OldSummary);

    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var en=l.Translations.Single(x=>x.CultureCode=="en");
            if(Match(en,true))continue;
            if(!Match(en,false))
                throw new InvalidOperationException("Unexpected Grade2 U01 L13 prior teaching fields");
            en.Explanation=Source.NewExplanation;
            en.KeyConceptsAndRules=Source.NewRules;
            en.WorkedExamples=Source.NewExamples;
            en.StepByStepSolutions=Source.NewSteps;
            en.CommonMistakes=Source.NewMistakes;
            en.QuickSummary=Source.NewSummary;
        }
    }
    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l)?Version:before;
    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before)=>
        IsTarget(d,l) && before==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string before,
        IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,before))
            throw new InvalidOperationException("Grade2 U01 L13 unexpected previous content version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("Grade2 U01 L13 unexpected previous culture set");
        var old=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(old.Title!=incoming.Title||!(Match(old,false)||Match(old,true)))
            throw new InvalidOperationException("Grade2 U01 L13 prior database learner prose not verified");
    }
}

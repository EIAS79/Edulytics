using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>V1 G1-006: exact archived first-edition 2021 IM/Kendall Hunt
/// teacher preparation and complete teacher lesson student-facing tasks.
/// No newer AccessIM page or unlicensed publisher image is copied.</summary>
public static class CommonCoreG1Unit2CompareFiveSourceTeachingCorrections
{
    public const string CorrectedContentVersion="us-ccss-g1-u02-l11-l15-source-aligned-v1";
    private sealed record Repair(string Code,string Url,string Sha,string[] Outcomes,
        string OldStep,string OldExample,string VerifiedTitle,
        string Explanation,string Concepts,string Examples,string Steps,
        string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new("PED:US-CCSS-MATH:G1:U02:L11","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-11/preparation.html","b0774a01ae8cf0db4cbde91a4fc46f50f42cd778964e6786a268701fe345b568",new string[]["CCSS:1.OA.A.1"],
            "Step 1: Identify the two quantities, 3 and 7.\nStep 2: Start at 3 and cou","Example 1: The source task combines 3 and 7. Start at 3 and count on 7 more to reach ",
            "Make Them the Same","Two towers can be compared without putting their cubes together. Line the cubes up in matching pairs. The taller tower has more cubes. The DIFFERENCE is how many cubes have no partner. Add cubes to the shorter tower, or remove cubes from the taller tower, to make the towers the same height. Write an equation showing how many cubes are needed.","Goal: represent and solve Compare, Difference Unknown problems with connecting cube towers. Compare two groups by matching, counting on or subtracting. If one tower has nine cubes and one has five, four cubes are unmatched: 5+4=9 and 9-5=4. This is not a total-unknown story.",
            "Example 1: Nine blue cubes and five red cubes. Align them: five pairs match and four blue cubes are left. Add four red cubes so both towers have nine: 5+4=9.\nExample 2: Seven cubes and three cubes. Remove four cubes from the taller tower or add four to the shorter: 7-3=4 and 3+4=7.\nExample 3: Six cubes and five cubes differ by one. A tower of six needs no combining with five: the question is how many MORE, not how many altogether.","Step 1: Read both tower counts. Do not combine the groups.\nStep 2: Line up the two towers at the same starting point or pair cubes one to one.\nStep 3: Count unmatched cubes to find the difference; adding to the shorter or taking from the taller is acceptable.\nStep 4: Write a complete matching addition or subtraction equation and check both towers would be equal.",
            "Adding the two towers finds a total, not their difference. Do not change both towers when the problem asks how many are needed. A count of unpaired cubes belongs in a complete equation, not a bare answer. Teacher-led building and partner explanations require actual pupils.","I can compare two cube towers, find the difference, and show how to make them the same with a matching equation."),
        new("PED:US-CCSS-MATH:G1:U02:L12","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-12/preparation.html","4fce1045bbb704d1f2d1ce0a587ba65c6a68a757879d6ab0bedfb083216a67cb",new string[]["CCSS:1.OA.A.1","CCSS:1.OA.C.5"],
            "Step 1: Represent the quantities or objects in the task.\nStep 2: Describ","Example 1: “How is their work the same? How is it different?” (They both use 2, 7 and",
            "School Supplies","Compare quantities of school supplies by first understanding what the question asks. Read the story three times: identify the objects, their quantities, and whether you need a total or a difference. Pair each notebook with one pencil; count the unmatched notebooks or count on from pencils to notebooks. An addition or subtraction equation can show the comparison.","Goal: solve Compare, Difference Unknown school-supply stories and relate counting to addition and subtraction. In a class with nine notebooks but six pencils, three pencils are missing: 6+3=9 or 9-6=3. The language 'how many more' asks for the DIFFERENCE, not both collections together.",
            "Example 1: Nine notebooks and six pencils. Pair six notebook/pencil sets; three notebooks have no pencil. Write 6+3=9.\nExample 2: Eight folders and five markers. Count on 6,7,8: three more markers would make the counts equal. Write 8-5=3.\nExample 3: Seven erasers and six pens. To have one pen per eraser, one more pen is needed; 6+1=7.","Step 1: Read the school-supply story and name the two categories.\nStep 2: Read it again for the given counts, and a third time for the question ('more' or 'fewer').\nStep 3: Draw matching pairs or place counters one to one.\nStep 4: Count on from the smaller group or subtract to find the unmatched amount.\nStep 5: Write a full matching equation with the difference and check the units.",
            "A comparison is not asking for the sum of both supply counts. The larger group is not the answer to 'how many more'. Do not count one pencil for multiple notebooks. Label what the equation's numbers stand for and check they fit the same story.","I can use matching, counting on, and a complete equation to tell how many more or fewer school supplies there are."),
        new("PED:US-CCSS-MATH:G1:U02:L13","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-13/preparation.html","c96a292f5540d845faffc5fdecc78d1702ea587b17545274b70091023c8582e4",new string[]["CCSS:1.MD.C.4","CCSS:1.OA.A.1"],
            "Step 1: Identify the two quantities, 5 and 3.\nStep 2: Start at 5 and cou","Example 1: The source task combines 5 and 3. Start at 5 and count on 3 more to reach ",
            "Compare Favorite Art Supply Data","Read a chart of how many people chose crayons, markers or paint. Use one mark for each response, and compare only the named categories. A statement such as 'more chose crayons than markers' is true only if the chart confirms it. Find the difference by matching marks, counting on, or subtracting, and write an equation for the comparison. A sample chart is invented practice data, not actual class survey results.","Goal: interpret categorical data and solve Compare, Difference Unknown questions (1.MD.C.4, 1.OA.A.1). In a chart showing eight crayon votes and five marker votes, the difference is three: 5+3=8 or 8-5=3. Do not add a third category when only the first two are compared.",
            "Example 1: Chart: crayons 8, markers 5, paint 4. Count matched crayon/marker votes and find three extra crayon votes: 8-5=3.\nExample 2: Chart: crayons 7, markers 4, paint 2. The claim 'more chose crayons than markers' is true. The difference is three: 4+3=7.\nExample 3: Chart: crayons 6, markers 5, paint 3. The statement 'three more chose crayons than markers' is false because 6-5=1.","Step 1: Read each category label and its chart count.\nStep 2: Identify the TWO categories named in the comparison question.\nStep 3: Align a counter for each response or count on from the smaller category to the larger.\nStep 4: Write a complete subtraction or addition equation for the difference.\nStep 5: Check any 'true or false' claim directly against the chart; explain it to your teacher.",
            "Do not mix the category labels or count the paint votes when comparing crayons with markers. A total of all votes is not a difference. Treat sample data as fictional rather than claiming actual pupils answered a survey. Each pictograph mark represents one response unless a key says otherwise.","I can read category data, test comparison statements, find the difference and write a matching equation."),
        new("PED:US-CCSS-MATH:G1:U02:L14","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-14/preparation.html","0e88df18d97bb0f6faead369bc64bc5e7758791a87a81cd9f6e3171f8bbcae2e",new string[]["CCSS:1.OA.A.1","CCSS:1.OA.C.5","CCSS:1.OA.C.6","CCSS:1.OA.D.7"],
            "Step 1: Identify the starting quantity 8 and the amount 3 being removed ","Example 1: The source task compares or removes 3 from 8. The resulting difference is ",
            "Compare with Addition and Subtraction","Compare two quantities by finding the number needed to make them equal. Both an addition equation and a related subtraction equation can describe the same comparison. If the short cube tower has five cubes and the tall one has nine, 5+4=9 explains how to make them equal and 9-5=4 explains the unmatched cubes. Each number must describe the same story. Teacher-led partner centers are not certified by a digital answer.","Goal: match addition AND subtraction equations to a Compare, Difference Unknown story (1.OA.A.1, 1.OA.C.5, 1.OA.C.6, 1.OA.D.7). Relate a missing-addend fact to the corresponding subtraction fact for the same quantities.",
            "Example 1: A blue tower has nine cubes and a red tower five. Both 5+4=9 and 9-5=4 describe the difference of four cubes.\nExample 2: Eight counters versus six counters. Counting on from six to eight needs two: 6+2=8; subtracting gives 8-6=2.\nExample 3: Five crayons and seven markers. Either 5+2=7 or 7-5=2 answers 'how many more markers?'; writing both connects addition to subtraction.","Step 1: Read the story and identify the larger and smaller quantities.\nStep 2: Match the two groups and count the unmatched objects.\nStep 3: Write a COMPLETE addition equation with the missing addend filled in.\nStep 4: Write the related COMPLETE subtraction equation using the same three quantities.\nStep 5: Check both equations answer the difference question; compare methods with a teacher or partner.",
            "Do not write two unrelated equations or claim that 9+5=14 answers a difference question. The two equations must describe the same whole and parts. Counting on and subtraction are different strategies that may produce the same difference.","I can write and explain an addition equation and its related subtraction equation for the same comparison story."),
        new("PED:US-CCSS-MATH:G1:U02:L15","https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-15/preparation.html","fe8826ae5ada9f876f89fab1ec84535dfb501fa21ef9fb9f9bc19669d656e0b1",new string[]["CCSS:1.OA.A.1","CCSS:1.OA.B.4","CCSS:1.OA.C.6"],
            "Step 1: Identify the two quantities, 3 and 4.\nStep 2: Start at 3 and cou","Example 1: The source task combines 3 and 4. Start at 3 and count on 4 more to reach ",
            "Different Types of Story Problems","Different stories with similar numbers can ask for different unknowns. A Put Together story asks how many objects altogether; a Part Unknown story gives the total and one known part; a Compare story asks how many more or fewer. Draw the known quantities, put a question mark where the information is missing, solve using counters or number facts, and write a complete addition or subtraction equation.","Goal: solve and represent varied Put Together/Take Apart and Compare, Difference Unknown stories (1.OA.A.1, 1.OA.B.4, 1.OA.C.6). Choose the operation from the question, not from numbers alone. Whole unknown, addend unknown, and difference unknown must be distinguished.",
            "Example 1: Three triangles and four squares are on a desk. Put together: 3+4=7 blocks.\nExample 2: Nine pattern blocks, five triangles, and the rest squares. One part unknown: 5+4=9 or 9-5=4 squares.\nExample 3: Eight beads for one child and five for another. Difference unknown: 8-5=3 more beads.\nExample 4: A small group has six crayons and another group has four. Compare asks how many more: 6-4=2; Put Together asks for ten: 6+4=10.","Step 1: Read the story and identify the two groups and what the question asks.\nStep 2: Decide whether the unknown is the whole, a missing part, or the difference between groups.\nStep 3: Model ONLY the given quantities with counters or a drawing.\nStep 4: Count, add, or subtract to find the unknown.\nStep 5: Write a complete equation matching the exact story and check the answer by reading it back.",
            "Do not automatically add every pair of numbers; a comparison asks for the unmatched part. Do not expose the unknown group as though it were given. A correct number without a matching equation does not demonstrate the lesson goal. Choice centers and class discussion are teacher-facilitated.","I can identify the type of story, find the missing quantity, and write an addition or subtraction equation that matches.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode &&
        doc.VersionCode=="CCSSM-2010"
            ?Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode)
            :null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)=>Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not V1 G1-006 target");
        if(lesson.SourceUrl!=r.Url ||
           !string.Equals(lesson.CanonicalBodySha256,r.Sha,StringComparison.OrdinalIgnoreCase) ||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("V1 G1-006 source identity/hash/standard drift: "+lesson.LessonCode);
        return r;
    }
    private static bool Matches(CanonicalLessonContentPackTranslation t,Repair r)=>
        t.Title==r.VerifiedTitle&&t.Explanation==r.Explanation&&
        t.KeyConceptsAndRules==r.Concepts&&t.WorkedExamples==r.Examples&&
        t.StepByStepSolutions==r.Steps&&t.CommonMistakes==r.Mistakes&&
        t.QuickSummary==r.Summary;
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var t=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(Matches(t,r))continue;
            if(!t.StepByStepSolutions.StartsWith(r.OldStep,StringComparison.Ordinal) ||
               !t.WorkedExamples.StartsWith(r.OldExample,StringComparison.Ordinal))
                throw new InvalidOperationException("V1 G1-006 unknown old learner body: "+lesson.LessonCode);
            if(lesson.LessonCode.EndsWith(":L14",StringComparison.Ordinal) &&
               t.Title is not ("Compare with Addition and Subtraction PLC Activity" or "Compare with Addition and Subtraction"))
                throw new InvalidOperationException("V1 G1-006 unexpected original L14 title");
            t.Title=r.VerifiedTitle;
            t.Explanation=r.Explanation;
            t.KeyConceptsAndRules=r.Concepts;
            t.WorkedExamples=r.Examples;
            t.StepByStepSolutions=r.Steps;
            t.CommonMistakes=r.Mistakes;
            t.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?CorrectedContentVersion:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string existingVersion)=>
        IsTarget(doc,lesson)&&
        existingVersion==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson,
        string existingVersion,IReadOnlyCollection<CurriculumLessonContentTranslation> existing)
    {
        var r=Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,existingVersion))
            throw new InvalidOperationException("V1 G1-006 unknown persisted version: "+lesson.LessonCode);
        if(existing.Count==0)return; // deployed JSON-only lesson mode
        if(existing.Count!=1)throw new InvalidOperationException("V1 G1-006 unknown persisted cultures");
        var t=existing.Single();
        if(t.CultureCode!="en"||
           !t.StepByStepSolutions.StartsWith(r.OldStep,StringComparison.Ordinal)||
           !t.WorkedExamples.StartsWith(r.OldExample,StringComparison.Ordinal))
            throw new InvalidOperationException("V1 G1-006 persisted text mismatch: "+lesson.LessonCode);
    }
}

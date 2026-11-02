using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Locally sourced optional IM2021 Grade2 U02 L16 market authentic tasks; fail-closed six original fields.</summary>
public static class CommonCoreG2Unit2MarketInventoryTeachingCorrection
{
    public const string Version="usccss-g2-u02-l16-100-market-inventory-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U02:L16";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-2/lesson-16/preparation.html";
    private const string Sha="54da7fdf3c0ec9cc2e83cb8656793c4d5b881d673ab4cc59b915e0a0e427a875";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will apply strategies for adding and subtracting within 100 within the context of a store.\nThis lesson is optional because it does not address any new mathematical content standards. This lesson does provide you with an opportunity to apply precursor skills of mathematical modeling. In previous lessons you learned how to add and subtract within 100.",
         "Key ideas:\n- This lesson is optional because it does not address any new mathematical content standards\n- Subtraction can represent taking away, comparing quantities, or finding a missing addend; the representation must match the situation.",
         "Example 1: You sell 3 kinds of items in a store. At the beginning of each day you have: a total of 100 items less than 10 of one of the items more than 10 for the other 2 Choose 3 items to sell at your market. Write the names of the items in the first row. Fill in the second row to show how much of each item you begin the day with. Share your store set-up with your...\nApproach: “What are some ways we can prove that we have exactly 100 items?“ (Add up the three numbers and get 100. Subtract two numbers from 100 and get the last number. Add two numbers and then subtract the result from 100 to get the third number.)\nExample 2: At your table, take turns rolling a number cube. Each person should roll three times. Record each roll. Roll 1: ___ Roll 2: ___ Roll 3: ___ Buyers: You will buy a certain amount of each item. Use the numbers you rolled to make: two-digit numbers one-digit numbers Sellers: After a sale, update the total number of items you have sold on your inventory...\nApproach: “How did you decide how to use rolls strategically, based on what was in the store’s inventory?” (If there were more items, I used a two-digit number. I had to think about which digit goes first.)",
         "Step 1: Identify what the task asks you to find or show.\nStep 2: Represent the important quantities or relationships.\nStep 3: Apply the key idea from the lesson.\nStep 4: Explain how the representation supports the result.\nStep 5: Check that the result matches the question.",
         "Common mistake: jumping to an answer before checking that the representation matches the quantities and relationship in the task.",
         "You will apply strategies for adding and subtracting within 100 within the context of a store. This lesson is optional because it does not address any new mathematical content standards."],
        ["IM K–5 2021 Grade2 Unit2 Lesson16 Our Market’s Inventory (optional): a realistic mathematics-modeling lesson about maintaining 100 items in a three-product shop. Warm-up Notice and Wonder: Han and his family’s grocery cart image is displayed by the teacher; pupils share observations and discuss possible places to shop. Activity1 100 Items: learners select THREE kinds of goods. They start with EXACTLY100 items overall; ONE kind has fewer than10 items, and EACH of the other TWO kinds has more than10 (strict inequalities). They name the products and prove the total using addition or finding a missing part. Activity2 Buyers and Sellers: each partner rolls a SINGLE physical number cube THREE times and records the faces. Buyers use these rolls strategically to form one- or two-digit amounts, sometimes by rearranging digits or operating on the rolls, without purchasing more than is in stock. Sellers update the sale activity for each item immediately and prevent overselling. Optional Activity3 Sales Reports: use authentic inventory records to show starting amount, number sold, number left, and number needed to restock; restock for each product equals its sales because returning to the starting quantity requires replacing exactly what was sold. The actual partner purchase decisions, rolls, original sales reports, gestures and MLR8 discourse require teacher verification; a first-party Edulytics digital example is not proof of real play.",
         "Inventory invariants: there are exactly THREE nonnegative integer category counts and they sum to100. Exactly ONE category must have quantity LESS THAN10 (0–9) and the other two must EACH be STRICTLY MORE THAN10 (11 or greater). A correct independently authored sample stock is fruit7, pencils42, books51 (7+42+51=100). Source learners choose their OWN items and values; those sample products and quantities are not claimed as publisher tasks. For each genuine purchase, 0≤ quantity bought ≤ the currently available stock for that product; soldSoFar ≤ startingStock; remaining = startingStock − soldSoFar. Across products, sum of the three starting stocks=100, sum remaining =100−sum sold. To restore original inventory exactly, restock quantity = soldSoFar, because remaining+restock=startingStock. The buyer rolls ONE number cube THREE times (not three different cubes as in the prior lesson’s Stage5), and may form useful one- or two-digit purchase quantities from the results consistent with stock. A true sales report must use actual transactions, not first-party sample numbers. Addressing standards:2.NBT.B.5,2.NBT.B.6 and2.OA.A.1; the lesson itself is optional and no NEW mathematical content standard is introduced.",
         "SOURCE Activity1 constraints: three kinds of products, total100, one stock count LESS than10, two stock counts GREATER than10. Independently authored Edulytics example: fruit7, pencils42, books51, so7+42+51=100; the strict constraints hold. SOURCE Activity2: roll the SAME cube three times, record each value; choose strategic one- or two-digit sale quantities from rolls and verify that the specific product has enough remaining. Source does NOT prescribe the amounts actually rolled or sold by a particular class. First-party illustrative sales ONLY, NOT publisher transactions: fruit starts7, sells3, leaves4; pencils starts42, sells23, leaves19; books starts51, sells25, leaves26. Starting total100, sold3+23+25=51, left4+19+26=49. SOURCE Activity3 Sales Reports: for those original Edulytics examples, restock3,23,25 respectively to restore7,42,51; sum restock51. Independent exit: if inventory starts36 and sells19, 17 remain; restock19. Source image of Han’s grocery cart and student worksheets not reproduced.",
         "1. Warm-up: the teacher shows the authentic Han shopping-cart picture; students privately observe then discuss what their family buys with an actual partner. 2. Activity1 choose three goods and choose a FIRST stock from0..9 and two other quantities from11..100. Calculate or derive the third so the three counts add to EXACTLY100. 3. Explain how adding the three counts, or subtracting two from100, confirms the model. 4. Activity2 collect ONE real number cube per pair, roll three times and RECORD the actual three faces. Build a one- or two-digit purchase number using the source directions and context; the three rolls may be rearranged or operated on strategically. Never remove more than current stock. 5. Following each genuine purchase, update sold and remaining quantities for the affected product. Group models, seller decisions and MLR8 vocabulary (inventory, sale, restock) are teacher-reviewed. 6. In optional Activity3 calculate per product remaining=start−sold and restock=sold. Total remaining+total sold=100. Create a clear REAL sales report for a partner; the seller must be able to support every number from the actual transaction records. 7. Solve distinct first-party original Edulytics inventory examples to test calculation, WITHOUT claiming actual classroom shopping or publisher restricted resources.",
         "Starting with three counts totaling99 or101 rather than exactly100. Choosing a small category with10 rather than LESS THAN10, or a large category with10 rather than MORE THAN10. Misreading the source requirement as each product having100 items. Using three physical cubes instead of rolling ONE cube three times in this lesson. Buying a two-digit amount exceeding the stock or subtracting an order from the wrong product. Repeating a sale without updating the available stock. Setting restock equal to what remains instead of the amount sold. Treating an invented sales sample, entered numbers or unobserved transcript as proof of authentic transactions and student MLR8 discussion. Claiming the optional lesson introduces a new assessed standard, whereas its goal is application and mathematical modeling.",
         "Optional Grade2 U02 L16 Market: choose THREE products with exactly100 items, ONE stock count below10, other TWO strictly above10. Act as real buyer/seller with ONE cube rolled three times per turn; use its outcomes strategically while keeping every purchase within available inventory. Report start, sold, left and restock; restock always equals sold to return to starting stock. Teacher must observe actual source picture, transactions and MLR8 discussion. Digital sample7+42+51=100, sales3+23+25=51, remainder49 and restock51 is independently authored."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.NBT.B.5","CCSS:2.NBT.B.6","CCSS:2.OA.A.1"}))
            throw new InvalidOperationException("G2 U02 L16 publisher source identity changed");
    }
    private static bool Same(CanonicalLessonContentPackTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Same(CurriculumLessonContentTranslation t,bool after)=>
        Fields([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Fields(string[] values,bool after)=>values.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;
            Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Same(t,true))continue;
            if(!Same(t,false))throw new InvalidOperationException("G2 U02 L16 previous six fields changed");
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
        if(!CanUpgradeExisting(d,l,before))throw new InvalidOperationException("G2 U02 L16 persisted predecessor version unexpected");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")
            throw new InvalidOperationException("G2 U02 L16 persisted translations changed");
        var t=rows.Single();var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(t.Title!=incoming.Title||!(Same(t,false)||Same(t,true)))
            throw new InvalidOperationException("G2 U02 L16 persisted learner fields changed");
    }
}

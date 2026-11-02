using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reuses the exact generation/independent verification engine for Grade 5
/// source-targeted MULTIPLE-CHOICE numeric subskills only. A correct answer does
/// not grade source-required drawings, physical builds or open explanations.
/// One lesson bank is added only after the previous lesson closes.
/// </summary>
internal static class UsCcssG5Unit2SourceQuestions
{
    private sealed record SourceCase(string Prompt,string Correct,string WrongA,string WrongB);
    private static readonly IReadOnlyDictionary<string,SourceCase[]> Banks=
        new Dictionary<string,SourceCase[]>(StringComparer.Ordinal)
        {
            ["usccss.g5.u02.l01.source_fair_share_choice"]=[
                new("Two sandwiches shared fairly by five people. Choose the amount EACH person receives; also draw two equal wholes separated into five fair shares.","2/5 sandwich","5/2 sandwiches","1/5 sandwich"),
                new("Five equal sandwiches shared by two people. What share does EACH person receive? Explain why the share is more than one whole.","2 1/2 sandwiches","2/5 sandwich","1/2 sandwich"),
                new("Three sandwiches shared by two people. Choose the share received by one person; show the same total number of whole sandwiches.","1 1/2 sandwiches","2/3 sandwich","1/3 sandwich"),
                new("Two sandwiches shared by three people. Choose the share each receives.","2/3 sandwich","3/2 sandwiches","1/3 sandwich"),
                new("Three sandwiches shared by five people. Choose the share each receives.","3/5 sandwich","5/3 sandwiches","1/5 sandwich"),
                new("Five sandwiches shared by three people. Choose the amount per person and describe equal portions.","1 2/3 sandwiches","3/5 sandwich","2/3 sandwich")
            ],
                        ["usccss.g5.u02.l02.unit_share_equivalence"]=[
                new("One sandwich shared equally among 2 people. How much for each person?","1/2 sandwich","2 sandwiches","1/3 sandwich"),
                new("One sandwich shared equally among 3 people. How much for each person?","1/3 sandwich","3 sandwiches","2/3 sandwich"),
                new("One sandwich shared equally among 4 people. How much for each person?","1/4 sandwich","4 sandwiches","1/2 sandwich"),
                new("One sandwich shared equally among 5 people. How much for each person?","1/5 sandwich","5 sandwiches","1/4 sandwich"),
                new("Which equality models exactly one sandwich shared by three people?","1÷3=1/3","3÷1=1/3","1÷3=3"),
                new("Which claim about equal shares of one sandwich is correct?","Each share becomes smaller as more people share one whole","The share becomes bigger as more people share","All equal divisions always give a whole sandwich")
            ],
                        ["usccss.g5.u02.l03.fraction_division_direction"]=[
                new("THREE dancers share TWO liters. What does each get?","2/3 liter","3/2 liters","1/3 liter"),
                new("Mai claims each of three dancers takes 3/2 liters from two liters total. Which correction is true?","Each receives 2/3 liter","Each receives 3/2 liters","Each receives 2 liters"),
                new("TWO dancers share THREE liters. How much does each get?","3/2 liters","2/3 liter","1/2 liter"),
                new("Which equality represents two liters shared among three people?","2÷3=2/3","3÷2=2/3","2×3=2/3"),
                new("Which context represents the fraction 3/2 liters per person?","3 liters shared equally among 2 people","2 liters shared equally among 3 people","3 people given 2 liters each"),
                new("How many liters total if three dancers each receive 2/3 liter?","2 liters","3 liters","9/2 liters")
            ],
                        ["usccss.g5.u02.l04.blueberry_division_context"]=[
                new("Six people share four pounds of blueberries equally. What does each person receive?","4/6 pound = 2/3 pound","6/4 pound","4 pounds"),
                new("Which expression matches four pounds shared between six people?","4÷6=4/6","6÷4=4/6","4×6=4/6"),
                new("Four one-pound bars shared between six people give what share?","2/3 pound each","3/2 pounds each","6 pounds each"),
                new("Seven people share three grams of gold. How many grams per person?","3/7 gram","7/3 grams","1/7 gram"),
                new("Five people equally share two gallons of milk. How many gallons each?","2/5 gallon","5/2 gallons","1/5 gallon"),
                new("What does the fraction 4/6 pound represent in the blueberry context?","4 pounds shared equally among 6 students","6 pounds shared equally among 4 students","4 pounds given to EACH of 6 students")
            ],
                        ["usccss.g5.u02.l05.fraction_division_generalization"]=[
                new("For whole a and nonzero whole b, which identity is valid?","a÷b=a/b","a÷b=b/a","a÷0=a/0"),
                new("Five pounds shared between two people equals how much per person?","5/2 pounds","2/5 pound","5 pounds"),
                new("Two pounds shared between five people equals how much per person?","2/5 pound","5/2 pounds","2 pounds"),
                new("Four pounds shared among four students gives each what amount?","1 pound","4 pounds","1/4 pound"),
                new("Which equality for 5÷2 is correct?","5÷2=5/2=2 1/2","5÷2=5 1/2","5÷2=2/5"),
                new("What must hold to interpret a/b as a÷b?","The denominator b is not zero","The numerator must exceed the denominator","Both quantities must be equal")
            ],
                        ["usccss.g5.u02.l06.relay_division_unit_fraction"]=[
                new("Two runners equally split a THREE-mile relay. How far does each run?","3/2 miles","2/3 mile","3 miles"),
                new("Which expression equals 3÷2 miles in the relay?","3×(1/2) miles","2×(1/3) miles","3×2 miles"),
                new("If three one-mile bars are each divided into halves, how many half-mile parts in one half of the total distance?","3 half-mile parts","2 half-mile parts","6 half-mile parts"),
                new("Three copies of a 1/2-mile segment total what distance?","3/2 miles","1/6 mile","3 miles"),
                new("Which choice equals 3/2 miles?","1 1/2 miles","3 1/2 miles","2/3 mile"),
                new("If two runners each travel 3/2 miles, what whole distance is covered?","3 miles","3/2 miles","6 miles")
            ],
                        ["usccss.g5.u02.l07.unit_fraction_race_distances"]=[
                new("Mai runs exactly one fourth of a NINE-mile road. How far does she run?","9/4 miles","4/9 mile","9 miles"),
                new("Han runs exactly one fourth of a SEVEN-mile road. How far does he run?","7/4 miles","4/7 mile","7 miles"),
                new("Four runners equally share THREE miles. What distance each?","3/4 mile","4/3 miles","1/4 mile"),
                new("Which multiplication matches 9÷4?","9×1/4","4×1/9","9×4"),
                new("9/4 miles is what mixed distance?","2 1/4 miles","1 3/4 miles","4 1/2 miles"),
                new("7/4 miles is what mixed distance?","1 3/4 miles","2 1/4 miles","4 1/7 miles")
            ],
                        ["usccss.g5.u02.l08.nonunit_fraction_of_whole"]=[
                new("Three whole bars each shade TWO FIFTHS. What total shaded amount?","6/5 unit squares","2/15 unit square","5/6 unit square"),
                new("3×(1/5) is what amount?","3/5","5/3","1/15"),
                new("3×(3/5) gives which result?","9/5","3/15","5/9"),
                new("Which equivalent expression describes 3×(2/5)?","2×(3÷5)","2×(5÷3)","3×(1/4)"),
                new("How many fifth-unit pieces are shaded in 3 bars, each with 2/5 shaded?","6 fifths","2 fifths","15 fifths"),
                new("6/5 equals which mixed number?","1 1/5","1 5/6","5 1/6")
            ],
                        ["usccss.g5.u02.l09.area_whole_by_unit_fraction"]=[
                new("A rectangle is SIX units long and ONE HALF unit high. What is its area?","3 square units","6 1/2 square units","12 square units"),
                new("A rectangle is SIX units long and ONE THIRD unit high. What is its area?","2 square units","18 square units","1/18 square unit"),
                new("A rectangle is SIX units long and ONE FOURTH unit high. What is its area?","1 1/2 square units","24 square units","4 square units"),
                new("A rectangle is 1 unit long and 1/2 unit high. What is its area?","1/2 square unit","2 square units","1 1/2 square units"),
                new("A rectangle is 3 units long and 1/2 unit high. What is its area?","1 1/2 square units","6 square units","2/3 square unit"),
                new("A rectangle is 4 units long and 1/2 unit high. What is its area?","2 square units","8 square units","1/8 square unit")
            ],
                        ["usccss.g5.u02.l10.area_whole_by_nonunit_fraction"]=[
                new("The source rectangle is 4 units long and 2/3 unit high. What exact area?","8/3 square units","8 square units","4/6 square unit"),
                new("The source rectangle is 4 units long and 3/4 unit high. What area?","3 square units","12 square units","16/3 square units"),
                new("The source rectangle is 4 units long and 3/5 unit high. What area?","12/5 square units","3 square units","20/3 square units"),
                new("The shaded rectangle is 3 units long and 2/3 unit high. What area?","2 square units","9/2 square units","2/9 square unit"),
                new("Source figure X is 3 units long and 4/5 unit high. What area?","12/5 square units","15/4 square units","4/15 square unit"),
                new("Source figure Y is 5 units long and 3/4 unit high. What area?","15/4 square units","12/5 square units","20/3 square units")
            ],
                        ["usccss.g5.u02.l11.area_fraction_greater_than_one"]=[
                new("A source rectangle is 4 2/3 units wide and 4 units high. What area?","56/3 square units","20/3 square units","16 square units"),
                new("Which expression equals the area of 4 2/3 by 4?","(14/3)×4","4×(5/3)","16×(8/3)"),
                new("Another source shaded rectangle has sides 2 1/4 and 2 units. What is its area?","9/2 square units","2 1/4 square units","9 square units"),
                new("Source shaded rectangle has sides 3 3/4 and 3 units. What is its area?","45/4 square units","15/4 square units","9/4 square units"),
                new("Does 4×(5/3) represent the original 4-by-4 2/3 source rectangle?","No: 20/3 differs from 56/3","Yes: it is 56/3","Yes: it equals 16"),
                new("Which area corresponds to 10/3=5×2/3?","10/3 square units","5/3 square units","15/2 square units")
            ],
                        ["usccss.g5.u02.l12.decomposed_mixed_rectangle_area"]=[
                new("Noah's garden measures 5 yards by 6 1/4 yards. What area?","31 1/4 square yards","30 1/4 square yards","36 1/4 square yards"),
                new("Priya's garden measures 6 yards by 5 1/4 yards. What area?","31 1/2 square yards","31 1/4 square yards","35 1/4 square yards"),
                new("Whose garden is bigger and by how much?","Priya by 1/4 square yard","Noah by 1/4 square yard","They are equal"),
                new("A shaded source rectangle is 4 units by 5 1/2 units. What area?","22 square units","20 square units","24 square units"),
                new("The source expression 5×2 1/2 equals which exact quantity?","12 1/2","10 1/2","15 1/2"),
                new("What must be added to 4×5=20 to represent 4×5 1/2?","2 square units","1/2 square unit","4 square units")
            ],
                        ["usccss.g5.u02.l13.distributive_area_expressions"]=[
                new("Source Diagram C: 3 units by 5 2/5 units. What is its area?","16 1/5 square units","15 2/5 square units","17 2/5 square units"),
                new("Source Diagram B: full 5×3 rectangle minus an unshaded 5×2/5 strip. What area remains?","13 square units","15 square units","17 square units"),
                new("Original source shaded rectangle 4 units by 5 1/3 units. What is its area?","21 1/3 square units","20 1/3 square units","24 square units"),
                new("Source shaded rectangle 7 units by 3 3/4 units. What is its area?","26 1/4 square units","21 3/4 square units","28 square units"),
                new("Evaluate source warm-up (5×7)+(5×1/4).","36 1/4","35 1/4","55 1/4"),
                new("Evaluate the source warm-up 5×(7+4).","55","35","20")
            ],
                        ["usccss.g5.u02.l14.fraction_multiplication_info_gap"]=[
                new("Evaluate 1/3×18 from the optional public Student Facing activity.","6","54","1/54"),
                new("Evaluate (7/9)×3 from the optional source task.","7/3","21","7/27"),
                new("Evaluate 1/15×30.","2","450","1/450"),
                new("Evaluate the source product 9×6 2/3.","60","54 2/3","9 2/3"),
                new("Evaluate the source product 7 3/5×6.","45 3/5","42 3/5","13 3/5"),
                new("Evaluate source product 4×6 9/10.","27 3/5","24 9/10","10 9/10")
            ],
                        ["usccss.g5.u02.l15.multiply_mixed_source_number_bank"]=[
                new("The source rug is 3 feet by 5 1/2 feet. What area?","16 1/2 square feet","8 1/2 square feet","18 1/2 square feet"),
                new("A puzzle is 2 1/2 feet wide and 2 feet long. What area?","5 square feet","4 1/2 square feet","10 square feet"),
                new("The source whiteboard is 4 feet by 5 3/4 feet. What area?","23 square feet","20 3/4 square feet","24 3/4 square feet"),
                new("Which value equals source 4×5 2/3?","22 2/3","20 2/3","24 2/3"),
                new("Evaluate source warm-up 6×3/8.","2 1/4","18","3/48"),
                new("Evaluate source warm-up 7×9/10.","6 3/10","7 9/10","63")
            ],
                        ["usccss.g5.u02.l16.estimate_mixed_area_products"]=[
                new("Source garden: 9×4 2/3 square feet. What exact area?","42 square feet","35 square feet","24 square feet"),
                new("Source garden: 9×3 8/9 square feet. What exact area?","35 square feet","42 square feet","36 square feet"),
                new("Source garden: 12×2 11/12 square feet. What exact area?","35 square feet","36 square feet","24 square feet"),
                new("Source garden: 9×2 2/3 square feet. What area?","24 square feet","35 square feet","42 square feet"),
                new("Which options are CLOSE to Priya's 36-square-foot target without exceeding it?","B and C (35 square feet each)","A only (42 square feet)","D only (24 square feet)"),
                new("Which bounds the original 7×12 8/9 product?","Between 84 and 91","Between 12 and 13","Between 36 and 37")
            ],
                        ["usccss.g5.u02.l17.mosaic_rectangle_area_cost"]=[
                new("An original Edulytiks model rectangle is 3 inches long and 1 1/2 inches wide. What area?","4 1/2 square inches","3 1/2 square inches","6 square inches"),
                new("Four nonoverlapping 3 by 1 1/2 inch tiles make a mosaic. What total area is covered?","18 square inches","12 square inches","24 square inches"),
                new("Stone costs $5 per square unit. What does a mosaic of 18 square units cost?","$90","$54","$36"),
                new("Tile costs $3 per square unit. What does a mosaic of 18 square units cost?","$54","$90","$36"),
                new("Glass costs $2 per square unit. What does a mosaic of 18 square units cost?","$36","$90","$54"),
                new("Which source material rate is the LOWEST per square unit?","Glass at $2","Stone at $5","Tile at $3")
            ],
            // NEXT_LESSON_BANK
        };
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Banks.ContainsKey(family);
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G5 U02 exact source family");
        ArgumentNullException.ThrowIfNull(random);
        var cases=Banks[family];
        var index=random.Next(cases.Length);
        var slot=random.Next(3);
        var c=cases[index];
        var opts=slot switch{
            0=>new[]{c.Correct,c.WrongA,c.WrongB},
            1=>new[]{c.WrongA,c.Correct,c.WrongB},
            _=>new[]{c.WrongA,c.WrongB,c.Correct}
        };
        var prompt=c.Prompt+
            $" Choose 1: {opts[0]}; 2: {opts[1]}; 3: {opts[2]}. "+
            "Justify with a diagram or explanation for teacher review. "+
            "The numeric selection alone does not complete the open task.";
        return new Problem(family,prompt,"The equal-sharing or product model must match the same whole. "+
            "Teacher separately reviews diagrams, source activities and explanations.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>(StringComparer.Ordinal) {["case"]=index,["slot"]=slot});
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G5 U02 source item");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is not null&&p.Count==2&&
        p.TryGetValue("case",out var index)&&index>=0&&index<Banks[family!].Length&&
        p.TryGetValue("slot",out var slot)&&slot is >=0 and <=2;
}

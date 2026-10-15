using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Source-goal-bound representative mathematical visuals for the 15 B01
/// figure-source lessons that previously composed ONLY an EquationSet.
/// Shared rendering primitives and independent illustrations are reused;
/// each exact source-goal identity fails closed on content drift.
/// Does NOT imply complete source-activity fidelity, rights approval or L6.
/// </summary>
public static class UsCcssB01V9SourceFigureStructuralGapVisuals
{
    private const string Prefix = "PED:US-CCSS-MATH:";
    private sealed record Goal(string Title, string Cue);
    private static readonly IReadOnlyDictionary<string,Goal> SourceGoals =
        new Dictionary<string,Goal>(StringComparer.Ordinal)
        {
            [Prefix+"G2:U05:L07"]=new("Center Day 1 (optional)","work with place value and practice addition and subtraction within 100"),
            [Prefix+"G2:U06:L03"]=new("Specific Side Lengths","sides with a given length"),
            [Prefix+"G2:U06:L05"]=new("Center Day 1 (optional)","describing, naming, and drawing triangles"),
            [Prefix+"G2:U06:L19"]=new("More Money Problems","context of money"),
            [Prefix+"G2:U08:L02"]=new("Partners Make Pairs","pair all of the objects"),
            [Prefix+"G2:U08:L03"]=new("Is it Odd or Even? PLC Activity","even or odd"),
            [Prefix+"G2:U09:L10"]=new("What’s the Question? PLC Activity","tape diagram and number line diagram"),
            [Prefix+"G3:U04:L11"]=new("Multiplication Strategies on Ungridded Rectangles","ungridded rectangle"),
            [Prefix+"G3:U04:L15"]=new("Equal Groups, Larger Numbers","factor is a teen number"),
            [Prefix+"G3:U05:L18"]=new("Design With Fractions (optional)","create geometric designs"),
            [Prefix+"G3:U07:L01"]=new("What Attributes Do You See?","sort shapes into categories"),
            [Prefix+"G3:U07:L04"]=new("Attributes of Rectangles, Rhombuses, and Squares PLC Activity","rhombus, rectangle, or square"),
            [Prefix+"G3:U07:L06"]=new("Distance Around Shapes","perimeter"),
            [Prefix+"G3:U08:L01"]=new("Estimation Explorations with Fractions","fraction representations through estimation"),
            [Prefix+"G4:U04:L20"]=new("Add and Subtract Within 1,000,000 PLC Activity","multiple compositions or decompositions")
        };

    public static int ExpectedCount => SourceGoals.Count;

    public static IReadOnlyList<RichLessonVisual>? Build(
        string lessonCode, CanonicalLessonTranslationRecord body)
    {
        if(!SourceGoals.TryGetValue(lessonCode,out var goal))
            return null;
        if(body.CultureCode!="en" || body.Title!=goal.Title ||
           !body.Explanation.Contains(goal.Cue,StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "B01 source-bound figure purpose drift: "+lessonCode);
        // The original G2 center has students pick exactly three cards for a
        // mystery three-digit number and describe hundreds, tens, and ones.
        if(lessonCode==Prefix+"G2:U05:L07" &&
           !body.Explanation.Contains("three-digit number",StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "B01 G2 U05 L07 mystery-number center purpose drift: "+lessonCode);
        if(lessonCode==Prefix+"G3:U05:L18" &&
           (!body.WorkedExamples.Contains(@"\frac{1}{2}",StringComparison.Ordinal) ||
            !body.WorkedExamples.Contains("Connect each point",StringComparison.Ordinal)))
            throw new InvalidOperationException(
                "B01 G3 U05 L18 midpoint-fraction task changed: "+lessonCode);
        if(lessonCode==Prefix+"G2:U09:L10" &&
           (!body.WorkedExamples.Contains("Elena picked 29 apples.",StringComparison.Ordinal) ||
            !body.WorkedExamples.Contains("14 fewer apples than Han",StringComparison.Ordinal) ||
            !body.WorkedExamples.Contains("15 more apples than Diego",StringComparison.Ordinal)))
            throw new InvalidOperationException(
                "B01 G2 U09 L10 student-story quantities drift: "+lessonCode);
        return lessonCode[Prefix.Length..] switch
        {
            "G2:U05:L07" => [new RichLessonVisual {
                Kind=RichLessonVisualKind.PlaceValueChart,
                Title="Mystery number: three digit cards 3, 7 and 4",
                Description="Illustrative student-made card example, not a copied publisher diagram: 3 hundreds, 7 tens and 4 ones form 374. The 3 is worth 300, the 7 is worth 70, and the 4 is worth 4. Clue: a three-digit number between 370 and 380 with 4 ones.",
                Rows=[
                    new RichLessonVisualRow { Label="Place", Cells=["Hundreds","Tens","Ones"] },
                    new RichLessonVisualRow { Label="Card", Cells=["3","7","4"] },
                    new RichLessonVisualRow { Label="Value", Cells=["300","70","4"] }
                ]
            }],
            "G2:U06:L03" => [Rectangle("3 cm by 2 cm rectangle",
                "Rectangle with opposite 3 cm sides and opposite 2 cm sides. The display is NOT a physical-size ruler. Use a separate physical ruler for a 3 cm drawing.",
                3,2,"cm")],
            "G2:U06:L05" => [
                Polygon("Triangle: exactly three straight sides",
                    "A triangle has three straight sides and three corners. Orientation does not change whether a shape is a triangle.","triangle"),
                Polygon("Quadrilateral: exactly four straight sides",
                    "A quadrilateral is any simple closed polygon with four straight sides and four corners; a rectangle is one example.","quadrilateral"),
                Polygon("Hexagon: exactly six straight sides",
                    "A hexagon has six straight sides and six corners. Side lengths need not be equal.","hexagon")
            ],
            "G2:U06:L19" => [new RichLessonVisual{
                Kind=RichLessonVisualKind.TapeDiagram,
                Title="Find the missing part: 68 cents in all, 39 cents known",
                Description="The whole is 68 cents. A known part is 39 cents, so the unknown part is 68 - 39 = 29 cents. The two parts exactly make the whole.",
                Items=["68","39","29","0"],SecondaryLabel="cents"}],
            "G2:U08:L02" => [Pairs(8),Pairs(9)],
            "G2:U08:L03" => [Pairs(9),Pairs(10)],
            // Two independent generic maths renderers; neither loads a
            // publisher diagram. The original lesson's story gives Elena=29,
            // Han=29+14=43 and Diego=43-15=28 apples.
            "G2:U09:L10" => [
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.TapeDiagram,
                    Title="Compare Elena's 29 apples with Han's 43 apples",
                    Description="Elena picked 29 apples, 14 fewer than Han. Han therefore picked 29 + 14 = 43 apples. The longer bar is 43, made from Elena's 29 and the extra 14.",
                    Items=["43","29","14","3"],SecondaryLabel="apples"
                },
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.NumberLine,
                    Title="Compare Diego's 28 apples with Han's 43 apples",
                    Description="Han picked 43 apples, which is 15 more than Diego. Count from 28 to 43 in 15 equal steps, so Diego picked 43 - 15 = 28 apples. Both endpoints are marked.",
                    Items=Enumerable.Range(28,16).Select(n=>
                        n.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToList(),
                    SecondaryItems=["0","15"],
                    SecondaryLabel="Two G2 number-line positions"
                }
            ],
            "G3:U04:L11" => [
                Rectangle("Full ungridded 3 by 6 rectangle",
                    "A 3 by 6 rectangle has area 18 square units. Partition its width conceptually into 4 and 2: 3 times (4 + 2) = 12 + 6 = 18.",6,3,"fixed-comparison-scale"),
                Rectangle("First part: 3 by 4","3 by 4 has area 12 square units. Compare with the remaining 3 by 2 part.",4,3,"fixed-comparison-scale"),
                Rectangle("Second part: 3 by 2","3 by 2 has area 6 square units. Both areas sum to 18; no grid is required.",2,3,"fixed-comparison-scale")],
            "G3:U04:L15" => [
                Rectangle("15 feet by 4 feet mural",
                    "A 15-foot by 4-foot rectangle has area 15 times 4 = 60 square feet. Split 15 into 10 and 5: 40 + 20 = 60.",15,4,"ft")],
            "G3:U05:L18" => [new RichLessonVisual {
                Kind=RichLessonVisualKind.ShapeDecomposition,
                Title="Use one-half of every side to make nested squares",
                Description="Start with a square. Mark the exact halfway point on all four sides. Connect those four midpoints to make a smaller rotated square. Repeat on each new square to make alternating aligned and rotated squares. Every new square has one-half the area of the previous square. This is a new Edulytics geometric construction, not copied publisher artwork.",
                SecondaryLabel="Independent-Nested-Midpoint-Squares",
                Items=["0","1","2","3"]
            }],
            "G3:U07:L01" => [
                Polygon("Right triangle: right angle","A triangle has exactly three straight sides; this example has a right angle.","right triangle"),
                Polygon("Rectangle: four right angles","A rectangle is a quadrilateral with four right angles; orientation does not change its properties.","rectangle"),
                Polygon("Pentagon: five sides","A pentagon has five closed straight sides, not four; sort by count of sides and corners.","pentagon")],
            "G3:U07:L04" => [
                Polygon("Rectangle: four right angles","Opposite sides equal and four right angles; adjacent side lengths need not be equal.","rectangle"),
                Polygon("Rhombus: four equal sides","All four sides equal; the illustrated non-square rhombus need not have right angles.","rhombus"),
                Polygon("Square: rectangle and rhombus","All sides equal AND all four angles are right angles. A square belongs to both categories.","square")],
            "G3:U07:L06" => [
                Rectangle("Trace the perimeter of a 5 by 3 rectangle",
                    "The boundary has four sides, 5 + 3 + 5 + 3 = 16 units. Perimeter is a length, NOT area (15 square units).",5,3)],
            "G3:U08:L01" => [new RichLessonVisual{
                Kind=RichLessonVisualKind.FractionBars,
                Title="Estimate five eighths using a half benchmark",
                Description="The same whole is divided into eight equal-sized parts. Five of eight shaded is 5/8, a little more than 1/2 = 4/8. Each strip represents one whole.",
                Rows=[
                    new RichLessonVisualRow{Label="Estimate: one half",Numerator=4,Denominator=8},
                    new RichLessonVisualRow{Label="Five eighths",Numerator=5,Denominator=8}
                ]}],
            "G4:U04:L20" => [new RichLessonVisual{
                Kind=RichLessonVisualKind.PlaceValueChart,
                Title="Regroup 75,940 − 12,786 without changing value",
                Description="In ten-thousands, thousands, hundreds, tens, ones order: regroup 75,940 as 7|5|8|13|10. Subtract 1|2|7|8|6 to get 6|3|1|5|4, so 75,940 − 12,786 = 63,154. Check 63,154 + 12,786 = 75,940.",
                Rows=[
                    new RichLessonVisualRow{Label="Positions",Cells=["10,000s","1,000s","100s","10s","1s"]},
                    new RichLessonVisualRow{Label="Original minuend",Cells=["7","5","9","4","0"]},
                    new RichLessonVisualRow{Label="Regrouped minuend",Cells=["7","5","8","13","10"]},
                    new RichLessonVisualRow{Label="Subtract",Cells=["1","2","7","8","6"]},
                    new RichLessonVisualRow{Label="Difference",Cells=["6","3","1","5","4"]}
                ]}],
            _ => throw new InvalidOperationException("B01 source-goal guard unimplemented.")
        };
    }

    private static RichLessonVisual Pairs(int count) => new()
    {
        Kind=RichLessonVisualKind.CounterPairs,
        Title=$"Arrange {count} counters into pairs",
        Description=$"{count} counters form {count/2} complete pairs of two" +
            (count%2==0?", with zero counters left over: even.":", with one counter left over: odd."),
        Items=[count.ToString(System.Globalization.CultureInfo.InvariantCulture)]
    };
    private static RichLessonVisual Polygon(string title,string description,string shape)=>new()
    {
        Kind=RichLessonVisualKind.PolygonDiagram,Title=title,Description=description,Items=[shape]
    };
    private static RichLessonVisual Rectangle(string title,string description,int length,
        int width,string unit="units")=>new()
    {
        Kind=RichLessonVisualKind.RectangleDiagram,Title=title,
        Description=description,Items=[
            length.ToString(System.Globalization.CultureInfo.InvariantCulture),
            width.ToString(System.Globalization.CultureInfo.InvariantCulture)],
        SecondaryLabel=unit
    };
}
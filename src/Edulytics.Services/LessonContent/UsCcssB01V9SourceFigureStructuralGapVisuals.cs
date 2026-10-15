using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Source-goal-bound representative mathematical visuals for B01 lessons
/// whose actual source purpose requires more than a generic EquationSet.
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
            [Prefix+"G1:U08:L09"]=new("Solve Number Riddles","use your understanding of place value to solve number riddles"),
            [Prefix+"G2:U03:L13"]=new("Center Day 2 (optional)","estimating and measuring lengths"),
            [Prefix+"G2:U05:L07"]=new("Center Day 1 (optional)","work with place value and practice addition and subtraction within 100"),
            [Prefix+"G2:U06:L03"]=new("Specific Side Lengths","sides with a given length"),
            [Prefix+"G2:U06:L05"]=new("Center Day 1 (optional)","describing, naming, and drawing triangles"),
            [Prefix+"G2:U06:L19"]=new("More Money Problems","context of money"),
            [Prefix+"G2:U08:L02"]=new("Partners Make Pairs","pair all of the objects"),
            [Prefix+"G2:U08:L03"]=new("Is it Odd or Even? PLC Activity","even or odd"),
            [Prefix+"G2:U09:L10"]=new("What’s the Question? PLC Activity","tape diagram and number line diagram"),
            [Prefix+"G3:U03:L04"]=new("Introduction to Addition Algorithms PLC Activity","base-ten diagrams"),
            [Prefix+"G3:U04:L11"]=new("Multiplication Strategies on Ungridded Rectangles","ungridded rectangle"),
            [Prefix+"G3:U04:L15"]=new("Equal Groups, Larger Numbers","factor is a teen number"),
            [Prefix+"G3:U05:L18"]=new("Design With Fractions (optional)","create geometric designs"),
            [Prefix+"G3:U07:L01"]=new("What Attributes Do You See?","sort shapes into categories"),
            [Prefix+"G3:U07:L04"]=new("Attributes of Rectangles, Rhombuses, and Squares PLC Activity","rhombus, rectangle, or square"),
            [Prefix+"G3:U07:L06"]=new("Distance Around Shapes","perimeter"),
            [Prefix+"G3:U08:L01"]=new("Estimation Explorations with Fractions","fraction representations through estimation"),
            [Prefix+"G3:U08:L14"]=new("Estimation Exploration","measurement of fractional lengths"),
            [Prefix+"G4:U04:L20"]=new("Add and Subtract Within 1,000,000 PLC Activity","multiple compositions or decompositions"),
            [Prefix+"G4:U01:L08"]=new("Mondrian's Art (optional)","area of rectangles and factor pairs")
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
        if(lessonCode==Prefix+"G3:U08:L14" &&
           !body.WorkedExamples.Contains("nearest half or fourth of an inch",
               StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "B01 G3 U08 L14 fractional-length estimation purpose drift: "+lessonCode);
        if(lessonCode==Prefix+"G1:U08:L09" &&
           !body.Explanation.Contains("secret number",StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "B01 G1 U08 L09 source number-riddle concept drift: "+lessonCode);
        if(lessonCode==Prefix+"G2:U03:L13" &&
           !body.Explanation.Contains("Target Measurements",StringComparison.Ordinal))
            throw new InvalidOperationException(
                "B01 G2 U03 L13 ruler-target center concept drift: "+lessonCode);
        if(lessonCode==Prefix+"G2:U05:L07" &&
           !body.Explanation.Contains("three-digit number",StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                "B01 G2 U05 L07 mystery-number center purpose drift: "+lessonCode);
        if(lessonCode==Prefix+"G3:U03:L04" &&
           !body.WorkedExamples.Contains("362 + 354",StringComparison.Ordinal))
            throw new InvalidOperationException(
                "B01 G3 U03 L04 base-ten addition example drift: "+lessonCode);
        if(lessonCode==Prefix+"G4:U01:L08" &&
           (!body.WorkedExamples.Contains("18-by-24 grid",StringComparison.Ordinal) ||
            !body.WorkedExamples.Contains("at least 12 rectangles",StringComparison.Ordinal) ||
            !body.WorkedExamples.Contains("prime number",StringComparison.Ordinal)))
            throw new InvalidOperationException(
                "B01 G4 U01 L08 area-design source constraints drift: "+lessonCode);
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
            "G1:U08:L09" => [new RichLessonVisual {
                Kind=RichLessonVisualKind.PlaceValueChart,
                Title="Solve a new place-value number riddle",
                Description="Illustrative original riddle: My number has 4 ones, is greater than 30, and is less than 40. The only solution is 34, because 3 tens are 30 and 4 ones are 4. Other numbers ending in 4 fail the range clue.",
                Rows=[
                    new RichLessonVisualRow {Label="Place",Cells=["Tens","Ones"]},
                    new RichLessonVisualRow {Label="Number of units",Cells=["3","4"]},
                    new RichLessonVisualRow {Label="Value",Cells=["30","4"]}
                ]
            }],
            "G2:U03:L13" => [
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.Scale,
                    Title="Compare a 7 cm target with a 6 cm measured line",
                    Description="Independent ruler example, not a life-size printable ruler: a centimeter scale from 0 to 10 with equal unit spacing. Mark 6 cm as the measured endpoint and 7 cm as the target endpoint. The absolute distance from measured to target is 1 cm.",
                    PrimaryLabel="Centimeters (schematic, not physical size)",
                    Items=Enumerable.Range(0,11).Select(n=>n.ToString(System.Globalization.CultureInfo.InvariantCulture)).ToList(),
                    SecondaryItems=["6","7"]
                },
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.RatioTable,
                    Title="Target Measurements: check the score",
                    Description="The measured line is 6 cm and the target is 7 cm. The absolute difference is |7 - 6| = 1 cm. A difference of zero would be an exact match.",
                    Rows=[
                        new RichLessonVisualRow {Label="Quantity",Cells=["Target","Measured","Absolute difference"]},
                        new RichLessonVisualRow {Label="Length",Cells=["7 cm","6 cm","1 cm"]}
                    ]
                }
            ],
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
            "G3:U03:L04" => [new RichLessonVisual {
                Kind=RichLessonVisualKind.PlaceValueChart,
                Title="Relate a base-ten model to an addition algorithm",
                Description="An original place-value representation for 362 + 354. Add 3 hundreds + 3 hundreds, 6 tens + 5 tens, and 2 ones + 4 ones. Exchange 10 of the 11 tens for a hundred. The sum has 7 hundreds, 1 ten, 6 ones: 716. The regrouping is the same in expanded and vertical algorithms.",
                Rows=[
                    new RichLessonVisualRow {Label="Place",Cells=["Hundreds","Tens","Ones"]},
                    new RichLessonVisualRow {Label="362",Cells=["3","6","2"]},
                    new RichLessonVisualRow {Label="354",Cells=["3","5","4"]},
                    new RichLessonVisualRow {Label="Combined before regrouping",Cells=["6","11","6"]},
                    new RichLessonVisualRow {Label="After regrouping",Cells=["7","1","6"]}
                ]
            }],
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
            "G4:U01:L08" => [new RichLessonVisual{
                Kind=RichLessonVisualKind.ShapeDecomposition,
                Title="Create your own 18 by 24 rectangular area design",
                Description="One independent solution to the art-and-factors challenge: an 18 by 24 unit grid with six horizontal bands, split into 12 rectangles. Two separate rectangles both have area 2 square units, which is prime. All 12 rectangles cover exactly 432 unit squares without overlaps. Different correct designs are possible.",
                SecondaryLabel="Independent-G4-18x24-Rectangular-Partition",
                Rows=[
                    new RichLessonVisualRow {Label="Band 1",Cells=["1","2","22"]},
                    new RichLessonVisualRow {Label="Band 2",Cells=["2","1","23"]},
                    new RichLessonVisualRow {Label="Band 3",Cells=["3","8","16"]},
                    new RichLessonVisualRow {Label="Band 4",Cells=["4","5","19"]},
                    new RichLessonVisualRow {Label="Band 5",Cells=["5","10","14"]},
                    new RichLessonVisualRow {Label="Band 6",Cells=["3","12","12"]}
                ]
            }],
            "G3:U08:L14" => [
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.NumberLine,
                    Title="Quarter-inch marks for a new estimation example",
                    Description="This is a schematic evenly spaced number line in quarter-inch steps, not a physical-size ruler or the publisher earthworm photograph. A sample strip measures 1¾ inches; 1½ inches is a low estimate. The marked positions represent 1½ and 1¾ inches and differ by exactly ¼ inch.",
                    Items=["0","¼","½","¾","1","1¼","1½","1¾","2"],
                    SecondaryItems=["6","7"],
                    SecondaryLabel="Two G3 quarter-inch positions"
                },
                new RichLessonVisual {
                    Kind=RichLessonVisualKind.RatioTable,
                    Title="Estimate too low, about right, and too high",
                    Description="For this independently authored sample of a 1¾ inch strip, 1½ is below the measured length, 1¾ is exact, and 2 is above it. Real measurements must be checked with a physical ruler, not screen pixels.",
                    Rows=[
                        new RichLessonVisualRow {Label="Estimate",Cells=["Too low","About right","Too high"]},
                        new RichLessonVisualRow {Label="Length",Cells=["1½ in","1¾ in","2 in"]}
                    ]
                }
            ],
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
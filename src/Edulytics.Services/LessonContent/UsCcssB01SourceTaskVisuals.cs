using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// First-party Rich V2 pedagogy diagrams from the SAME immutable structured
/// parameters used to generate and verify a learner's exact question.
/// Never parse display strings or invent figures unrelated to the actual task.
/// Not a replacement for desktop/mobile L4/L6 visual acceptance.
/// </summary>
public static class UsCcssB01SourceTaskVisuals
{
    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if (!IsB01(lessonCode)) return [];
        var visual = new List<RichLessonVisual>(2);
        foreach (var q in questions)
        {
            if (!q.Family.StartsWith("usccss.b01.", StringComparison.Ordinal) &&
                q.Family != "measurement.scale.equal_intervals.read_value" &&
                q.Family != "fractions.equivalent.number_line")
                continue;
            var p = q.Parameters;
            int N(string k) => p[k];
            RichLessonVisual? item = q.Family switch
            {
                "usccss.b01.shape.polygon_sides" when
                    N("sides") is >= 3 and <= 6 &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.PolygonDiagram,
                        Title = $"A polygon with {N("sides")} straight sides",
                        Description = $"This closed polygon has {N("sides")} straight sides " +
                            $"and {N("sides")} corners, so it is a {q.CorrectAnswer}. " +
                            "The drawing is schematic and side lengths are not to scale.",
                        Items = [q.CorrectAnswer]
                    },
                "usccss.b01.shape.specific_subcategory" when
                    N("subcategory") is >= 0 and <= 3 &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.PolygonDiagram,
                        Title = $"Sort by defining attributes: {q.CorrectAnswer}",
                        Description = q.Family == "usccss.b01.shape.specific_subcategory" ?
                            $"The pictured {q.CorrectAnswer} has the angles and equal-side " +
                            "attributes specified in the verified question." : "",
                        Items = [q.CorrectAnswer]
                    },
                "usccss.b01.triangle.attributes" when
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.PolygonDiagram,
                        Title = "A triangle always has three straight sides",
                        Description = "This is a triangle: three straight segments form " +
                            "one closed boundary, whether rotated or stretched.",
                        Items = [N("style") switch
                        {
                            0 => "narrow triangle",
                            1 => "tilted triangle",
                            2 => "triangle",
                            3 => "right triangle",
                            _ => "triangle"
                        }]
                    },
                "usccss.b01.area.missing_composite_side" when
                    N("widthTotal")==N("widthKnown")+N("widthMissing") &&
                    N("heightLeft")!=N("heightRight") &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ShapeDecomposition,
                        Title = "Missing side in an L-shaped composite figure",
                        Description = $"First find {N("widthMissing")} = {N("widthTotal")} − " +
                            $"{N("widthKnown")}. Then add disjoint rectangle areas: " +
                            $"{N("widthKnown")}×{N("heightLeft")} + " +
                            $"{N("widthMissing")}×{N("heightRight")} = {q.CorrectAnswer}.",
                        Rows =
                        [
                            new RichLessonVisualRow
                            {
                                Label = "Left rectangle",
                                Cells = [$"Width {N("widthKnown")}",
                                    $"Height {N("heightLeft")}",
                                    $"Area {N("widthKnown")*N("heightLeft")}"]
                            },
                            new RichLessonVisualRow
                            {
                                Label = "Right rectangle",
                                Cells = [$"Missing width {N("widthMissing")}",
                                    $"Height {N("heightRight")}",
                                    $"Area {N("widthMissing")*N("heightRight")}"]
                            }
                        ]
                    },
                // IM Grade 1 U01 L02 source activity: two dot-card
                // pictures must match the exact addends and expression.
                "usccss.b01.g1.match_dot_cubes_expression" or
                "usccss.b01.g1.complete_missing_dots" or
                "usccss.b01.g1.check_off_add_within10" when
                    N("first") is >= 0 and <= 6 &&
                    N("second") is >= 0 and <= 6 &&
                    N("first") + N("second") <= 10 &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,q.Parameters,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.PlaceValueChart,
                        Title = "Match dot pictures and addition expressions",
                        Description = $"The dot groups contain {N("first")} and " +
                            $"{N("second")} dots. Match them to " +
                            $"{N("first")} + {N("second")} = " +
                            $"{N("first")+N("second")}.",
                        Rows =
                        [
                            new RichLessonVisualRow
                            {
                                Label = $"First cube: {N("first")}",
                                Cells = Enumerable.Repeat("●",N("first")).ToList()
                            },
                            new RichLessonVisualRow
                            {
                                Label = $"Second cube: {N("second")}",
                                Cells = Enumerable.Repeat("●",N("second")).ToList()
                            }
                        ]
                    },
                // A measured scale has verified equal intervals and a
                // precise target tick, never an ornamental timeline.
                "measurement.scale.equal_intervals.read_value" when
                    N("intervals") is > 0 and <= 10 &&
                    (N("end") - N("start")) % N("intervals") == 0 &&
                    N("pointer") >= 0 && N("pointer") <= N("intervals") &&
                    q.CorrectAnswer ==
                    (N("start") +
                     (N("end") - N("start")) / N("intervals") *
                     N("pointer")).ToString() =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.Scale,
                        Title = "Equal intervals on a measured scale",
                        Description = $"Start at {N("start")}; count {N("intervals")} equal " +
                            $"steps to {N("end")}. Each is " +
                            $"{(N("end") - N("start")) / N("intervals")}; " +
                            $"the marked tick is {q.CorrectAnswer}.",
                        Items = Enumerable.Range(0, N("intervals") + 1)
                            .Select(i => (N("start") + i *
                                (N("end") - N("start")) / N("intervals")).ToString())
                            .ToList()
                    },
                // Equivalent fractions occupy the same point: baseN/baseD
                // and exact engine targetN/targetD. Show the genuine equal
                // 1/baseD spacing, not uniformly-spaced arbitrary values.
                "fractions.equivalent.number_line" when
                    N("baseD") is >= 2 and <= 12 &&
                    N("baseN") >= 1 && N("baseN") < N("baseD") &&
                    N("targetD") > 0 &&
                    N("baseN") * N("targetD") ==
                      int.Parse(q.CorrectAnswer) * N("baseD") =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.NumberLine,
                        Title = "Equivalent fractions at the same number-line point",
                        Description = $"Place {N("baseN")}/{N("baseD")} and " +
                            $"{q.CorrectAnswer}/{N("targetD")} at the same " +
                            "location. The intervals are equal unit fractions.",
                        Items = Enumerable.Range(0, N("baseD") + 1)
                            .Select(i => i == 0 ? "0" :
                                i == N("baseD") ? "1" :
                                $"{i}/{N("baseD")}")
                            .ToList()
                    },
                "usccss.b01.place_value.tens_ones" when
                    N("number") == N("tens")*10+N("ones") &&
                    N("number") is >= 10 and <= 99 =>
                    BaseTenModel(N("number"),0,N("tens"),N("ones"),false),
                "usccss.b01.place_value.count_to120" when
                    N("number")==N("hundreds")*100+N("tens")*10+N("ones") &&
                    N("number") is >= 40 and <= 120 =>
                    BaseTenModel(N("number"),N("hundreds"),N("tens"),N("ones"),true),
                "usccss.b01.place_value.three_digits" when
                    N("number")==N("hundreds")*100+N("tens")*10+N("ones") &&
                    N("number") is >= 100 and <= 999 =>
                    BaseTenModel(N("number"),N("hundreds"),N("tens"),N("ones"),true),
                "usccss.b01.clock.read_on_hour" or
                "usccss.b01.clock.read_five_minutes_am_pm" when
                    N("hour") is >= 1 and <= 12 &&
                    N("minutes")%5==0 &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.ClockFace,
                        Title=$"Read the analog time: {q.CorrectAnswer}",
                        Description=$"The short hour hand is at or just past {N("hour")}. " +
                            $"The minute hand shows {N("minutes")} minutes past. " +
                            (q.Family=="usccss.b01.clock.read_on_hour"
                                ? "The hand at 12 means zero minutes."
                                : N("period")==0
                                    ? "The context says before noon: a.m."
                                    : "The context says after noon: p.m."),
                        PrimaryLabel=q.CorrectAnswer,
                        Items=[N("hour").ToString(),N("minutes").ToString()]
                    },
                "usccss.b01.clock.read_half_hour" or
                "usccss.b01.clock.read_five_minutes" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ClockFace,
                        Title = $"Read the analog clock: {N("hour")}:{N("minutes"):00}",
                        Description = $"The short hand shows hour {N("hour")}; " +
                            $"the long hand is at {N("minutes")/5 % 12} five-minute steps " +
                            $"from 12, showing {N("minutes")} minutes past the hour.",
                        Items = [N("hour").ToString(), N("minutes").ToString()]
                    },
                "usccss.b01.clock.half_quarters" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ClockFace,
                        Title = "Analog clock: hour and minute hands",
                        Description = $"The minute hand points to the {(N("position") == 0 ? "3" :
                            N("position") == 1 ? "6" : "9")}. This is " +
                            $"{15 * (N("position")+1)} minutes after the hour. " +
                            "The short hour hand moves proportionally between hours.",
                        Items = [N("hour").ToString(),
                            (15 * (N("position")+1)).ToString()]
                    },
                "usccss.b01.array.commutativity" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ShapeDecomposition,
                        Title = "The same array before and after rotation",
                        Description = $"Start with {N("rows")} × {N("columns")}. " +
                            $"After rotation it is {N("columns")} × {N("rows")}. " +
                            $"Both arrangements have {N("rows")*N("columns")} objects.",
                        Rows = Enumerable.Range(1, N("rows"))
                            .Select(row => new RichLessonVisualRow
                            {
                                Label = $"Before row {row}",
                                Cells = Enumerable.Repeat("■", N("columns")).ToList()
                            }).Concat(Enumerable.Range(1, N("columns"))
                                .Select(row => new RichLessonVisualRow
                                {
                                    Label = $"After row {row}",
                                    Cells = Enumerable.Repeat("■", N("rows")).ToList()
                                })).ToList()
                    },
                "usccss.b01.array.columns" or
                "usccss.b01.array.equal_addends" or
                "usccss.b01.array.equal_equation" or
                "usccss.b01.array.unknown_factor" or
                "usccss.b01.array.equal_groups" or
                "usccss.b01.rectangle.unit_squares" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ShapeDecomposition,
                        Title = $"An array of {N("rows")} rows and {N("columns")} columns",
                        Description = $"Count {N("columns")} equal unit squares in each of {N("rows")} rows. " +
                            $"The total is {N("rows")} × {N("columns")} = {N("rows") * N("columns")}. " +
                            $"Repeated addition is " +
                            $"{string.Join("+",Enumerable.Repeat(N("columns").ToString(),N("rows")))}" +
                            $"={N("rows")*N("columns")}.",
                        PrimaryLabel = "Unit square array",
                        Rows = Enumerable.Range(1, N("rows"))
                            .Select(row => new RichLessonVisualRow
                            {
                                Label = $"Row {row}",
                                Cells = Enumerable.Repeat("■", N("columns")).ToList()
                            }).ToList()
                    },
                "usccss.b01.fraction.name_parts" =>
                    SingleFractionBar("Fraction of one whole", N("numerator"),
                        N("denominator")),
                "usccss.b01.partition.halves_fourths" =>
                    SingleFractionBar("Equal pieces of a whole", 1,
                        N("denominator")),
                // Source IM G4 U03 L16–L17: 1/10 = 10/100.
                // For a total > 1, shade a full bar AND its fractional
                // remainder rather than incorrectly shading >100% of one bar.
                "usccss.b01.fraction.tenths_hundredths_within_one" or
                "usccss.b01.fraction.tenths_hundredths_over_one" when
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,q.Parameters,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.FractionBars,
                        Title = "Tenths become hundredths before adding",
                        Description = $"Convert {N("tenths")}/10 to " +
                            $"{10*N("tenths")}/100. Add {N("hundredths")}/100 " +
                            $"to obtain {q.CorrectAnswer}." +
                            (N("totalHundredths")>100
                                ? $" That's one whole and " +
                                  $"{N("totalHundredths")-100}/100 more."
                                : " The sum is at most one whole."),
                        Rows = new List<RichLessonVisualRow>
                        {
                            new()
                            {
                                Label = "Equivalent tenths in hundredths",
                                Numerator = 10*N("tenths"), Denominator = 100
                            },
                            new()
                            {
                                Label = "Hundredths addend",
                                Numerator = N("hundredths"), Denominator = 100
                            }
                        }.Concat(N("totalHundredths")<=100
                            ? new List<RichLessonVisualRow>
                              {
                                  new()
                                  {
                                      Label = "Total shaded hundredths",
                                      Numerator = N("totalHundredths"),
                                      Denominator = 100
                                  }
                              }
                            : new List<RichLessonVisualRow>
                              {
                                  new()
                                  {
                                      Label = "One whole",
                                      Numerator = 100, Denominator = 100
                                  },
                                  new()
                                  {
                                      Label = "Additional hundredths",
                                      Numerator = N("totalHundredths")-100,
                                      Denominator = 100
                                  }
                              }).ToList()
                    },
                "usccss.b01.fraction.decompose_same_denominator" =>
                    DecompositionBar(N("wholeN"), N("firstN"), N("denominator")),
                "usccss.b01.fraction.whole_multiply" =>
                    RepeatedFractionBars(N("numerator"), N("denominator"), N("whole")),
                "usccss.b01.number_line.move_left_right_within100" when
                    N("start")%10==0 && N("distance")%10==0 &&
                    N("end")==N("start")+N("direction")*N("distance") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.NumberLine,
                        Title="Move right to increase and left to decrease",
                        Description=$"Start at {N("start")}. Move {N("jumps")} equal " +
                            $"tick(s) {(N("direction")==1?"right":"left")}, " +
                            $"each worth 10, to finish at {q.CorrectAnswer}. " +
                            "All ticks are evenly spaced.",
                        PrimaryLabel=$"{N("start")} to {N("end")}",
                        SecondaryItems=[(N("start")/10).ToString(),
                            (N("end")/10).ToString()],
                        Items=Enumerable.Range(0,11).Select(i=>(i*10).ToString()).ToList()
                    },
                "usccss.b01.number_line.estimate_location_within100" when
                    N("upper")==N("lower")+10 &&
                    N("target")>N("lower") && N("target")<N("upper") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.NumberLine,
                        Title="Estimate a whole-number location between labelled tens",
                        Description=$"The unlabelled point at {N("target")} lies strictly " +
                            $"between {N("lower")} and {N("upper")}. " +
                            $"The closer labelled tick is {q.CorrectAnswer}.",
                        PrimaryLabel=$"Point {N("target")}: nearest {q.CorrectAnswer}",
                        Items=Enumerable.Range(0,11).Select(i=>(i*10).ToString()).ToList()
                    },
                "usccss.b01.number_line.locate_whole_to100" or
                "usccss.b01.number_line.locate_three_digits" or
                "usccss.b01.number_line.locate_large_numbers" when
                    N("step")>0 && N("tick")>=1 && N("tick")<=9 &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.NumberLine,
                        Title = "Locate the actual number on equal intervals",
                        Description = $"All ten intervals measure {N("step"):N0} " +
                            $"units. The highlighted target tick is {N("tick")}, " +
                            $"at the exact value {q.CorrectAnswer}.",
                        PrimaryLabel = $"Target tick {N("tick")} equals {q.CorrectAnswer}",
                        SecondaryItems = [N("tick").ToString()],
                        Items = Enumerable.Range(0,11)
                            .Select(i => (i*N("step")).ToString())
                            .ToList()
                    },
                "usccss.b01.number_line.equal_intervals" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.NumberLine,
                        Title = "Evenly spaced number-line ticks",
                        Description = $"Each interval is {N("step")}. There are three equal steps " +
                            $"from 0 to {3 * N("step")}.",
                        Items = [ "0", $"{N("step")}", $"{2 * N("step")}",
                            $"{3 * N("step")}" ]
                    },
                // Both ruler diagrams are derived from checked operands.
                "usccss.b01.g2.compare_tape_difference" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U01:L14" &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.TapeDiagram,
                        Title="Compare two aligned tape-diagram bars",
                        Description=$"The first group has {N("longCount")} equal units, " +
                            $"the second group has {N("shortCount")}. " +
                            $"The unmatched part is {q.CorrectAnswer} units, " +
                            "the exact difference between the aligned bars.",
                        PrimaryLabel="Compare: how many more?",
                        Items=[
                            N("longCount").ToString(),
                            N("shortCount").ToString(),
                            N("difference").ToString()
                        ]
                    },
                "usccss.b01.g3.picture_graph.scaled_compare" or "usccss.b01.g3.picture_graph.construct" when
                    (lessonCode=="PED:US-CCSS-MATH:G3:U01:L03" ||
                     lessonCode=="PED:US-CCSS-MATH:G3:U01:L04") &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PictureGraph,
                        Title="Scaled picture graph: each symbol has a value",
                        Description=$"Each ● stands for {N("scale")} responses. " +
                            $"Category A represents {N("aIcons")*N("scale")}, " +
                            $"and category B represents {N("bIcons")*N("scale")} responses. " +
                            $"The verified question answer is {q.CorrectAnswer}.",
                        PrimaryLabel=$"Key: ● = {N("scale")} responses",
                        Items=[N("scale").ToString()],
                        Rows=[
                            new RichLessonVisualRow
                            {
                                Label="Category A",
                                Cells=Enumerable.Repeat("●",N("aIcons")).ToList()
                            },
                            new RichLessonVisualRow
                            {
                                Label="Category B",
                                Cells=Enumerable.Repeat("●",N("bIcons")).ToList()
                            }
                        ]
                    },
                "usccss.b01.g2.measurement.read_line_plot" when
                    (lessonCode=="PED:US-CCSS-MATH:G2:U03:L14" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U03:L15") &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title=lessonCode.EndsWith("L15",StringComparison.Ordinal)
                            ?"Construct a line plot from measured lengths"
                            :"Interpret an X-mark line plot",
                        Description="Draw one X above each number for every object " +
                            $"measured. The verified count at {N("targetLength")} cm " +
                            $"is {q.CorrectAnswer}.",
                        PrimaryLabel="Length in centimeters",
                        Rows=Enumerable.Range(2,4)
                            .Select(length=>new RichLessonVisualRow
                            {
                                Label=$"{length} cm",
                                Cells=Enumerable.Repeat("X",N($"count{length}")).ToList()
                            }).ToList()
                    },
                "usccss.b01.ruler.compare_centimeters" when
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.Scale,
                        Title="Compare measured lengths on centimeter ruler",
                        Description=$"Every adjacent ruler tick is 1 cm. " +
                            $"The first object measures {N("longLength")} cm; " +
                            $"the other measures {N("shortLength")} cm. " +
                            $"Their difference is {q.CorrectAnswer} cm.",
                        PrimaryLabel=$"{N("longLength")} cm − {N("shortLength")} cm",
                        Items=Enumerable.Range(0,17).Select(i=>i.ToString()).ToList(),
                        SecondaryItems=[N("longLength").ToString(),
                            N("shortLength").ToString()]
                    },
                "usccss.b01.ruler.measure_without_zero" when
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.Scale,
                        Title="Measure without aligning the edge with zero",
                        Description=$"The ruler's equally spaced centimeter marks " +
                            $"show a start of {N("start")} and end of {N("end")}. " +
                            $"The length is {N("end")} − {N("start")} = {q.CorrectAnswer} cm.",
                        PrimaryLabel=$"Object: tick {N("start")} to {N("end")}",
                        Items=Enumerable.Range(0,24).Select(i=>i.ToString()).ToList(),
                        SecondaryItems=[N("start").ToString(),N("end").ToString()]
                    },
                "usccss.b01.graph.choose_scale" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.Scale,
                        Title = "Equal bar-graph scale",
                        Description = $"The scale increases by {N("step")} " +
                            $"until the maximum value of {4*N("step")}.",
                        Items = [ "0", $"{N("step")}", $"{2*N("step")}",
                            $"{3*N("step")}", $"{4*N("step")}" ]
                    },
                "usccss.b01.market.inventory_100" =>
                    MarketInventory(p),
                "usccss.b01.geometry.rectangle_area_grid60" when
                    N("rows")>0 && N("columns")>0 &&
                    N("rows")*N("columns")<=60 &&
                    q.CorrectAnswer==(N("rows")*N("columns")).ToString() =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.ShapeDecomposition,
                        Title="Find the area by counting unit squares",
                        Description=$"{N("rows")} rows of {N("columns")} equal " +
                            $"unit squares cover exactly {q.CorrectAnswer} square units " +
                            "with no gaps or overlapping squares.",
                        Rows=Enumerable.Range(1,N("rows"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Row {i}",
                                Cells=Enumerable.Repeat("■",N("columns")).ToList()
                            }).ToList()
                    },
                "usccss.b01.geometry.rectangle_area_ungridded" when
                    N("length")>0 && N("width")>0 &&
                    q.CorrectAnswer==(N("length")*N("width")).ToString() =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Dimensioned rectangle without a square grid",
                        Description=$"Length {N("length")} units, width {N("width")} " +
                            $"units, and area {q.CorrectAnswer} square units. " +
                            "The rectangle has the actual mathematical side-length ratio.",
                        Items=[N("length").ToString(),N("width").ToString()]
                    },
                "usccss.b01.geometry.composite_rectangle_area" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ShapeDecomposition,
                        Title = "Two rectangles without overlap",
                        Description = $"The first area is {N("height") * N("width1")} and the " +
                            $"second is {N("height") * N("width2")}. Add them for " +
                            $"{N("height") * (N("width1") + N("width2"))} square units.",
                        Rows =
                        [
                            new RichLessonVisualRow
                            {
                                Label = "Rectangle 1",
                                Cells = [$"Height {N("height")}", $"Width {N("width1")}",
                                         $"Area {N("height") * N("width1")}"]
                            },
                            new RichLessonVisualRow
                            {
                                Label = "Rectangle 2",
                                Cells = [$"Height {N("height")}", $"Width {N("width2")}",
                                         $"Area {N("height") * N("width2")}"]
                            }
                        ]
                    },
                "usccss.b01.place_value.ten_times" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.PlaceValueChart,
                        Title = "The same digit, one place to the left",
                        Description = $"Moving the digit {N("digit")} one place left changes its " +
                            $"value from {N("digit") * N("place")} to " +
                            $"{N("digit") * N("place") * 10}, exactly ten times larger.",
                        Rows =
                        [
                            new RichLessonVisualRow
                            {
                                Label = "First position",
                                Cells = [$"Digit {N("digit")}", $"Value {N("digit") * N("place")}"]
                            },
                            new RichLessonVisualRow
                            {
                                Label = "One place left",
                                Cells = [$"Digit {N("digit")}", $"Value {N("digit") * N("place") * 10}"]
                            }
                        ]
                    },
                _ => null
            };
            if (item is not null) visual.Add(item);
            // Keep the page focused: only one task-scoped visual per lesson;
            // the ordinary QuestionForms diagram remains available afterward.
            if (visual.Count >= 1) break;
        }
        return visual;
    }

    private static RichLessonVisual BaseTenModel(int number,int hundreds,int tens,
        int ones,bool showHundreds)
    {
        var rows=new List<RichLessonVisualRow>();
        if (showHundreds)
            rows.Add(new RichLessonVisualRow
            {
                Label="Hundreds", Cells=[hundreds.ToString(),(hundreds*100).ToString()]
            });
        rows.Add(new RichLessonVisualRow
        {
            Label="Tens",Cells=[tens.ToString(),(tens*10).ToString()]
        });
        rows.Add(new RichLessonVisualRow
        {
            Label="Ones",Cells=[ones.ToString(),ones.ToString()]
        });
        return new RichLessonVisual
        {
            Kind=RichLessonVisualKind.PlaceValueChart,
            Title=$"Build {number} from hundreds, tens, and ones",
            Description=showHundreds
                ? $"{hundreds} hundred(s) + {tens} ten(s) + {ones} one(s) = {number}."
                : $"{tens} ten(s) + {ones} one(s) = {number}.",
            PrimaryLabel="Digit and corresponding place value",
            Rows=rows
        };
    }

    private static RichLessonVisual SingleFractionBar(string title,int n,int d) =>
        new()
        {
            Kind = RichLessonVisualKind.FractionBars,
            Title = title,
            Description = $"The whole is partitioned into {d} equal parts; " +
                $"{n} out of {d} are shaded.",
            Rows = [new RichLessonVisualRow
            {
                Label = $"{n}/{d}",
                Numerator = n,
                Denominator = d
            }]
        };

    private static RichLessonVisual DecompositionBar(int total,int first,int denominator) =>
        new()
        {
            Kind = RichLessonVisualKind.FractionBars,
            Title = $"Two fractions add to {total}/{denominator}",
            Description = $"{first}/{denominator} + {total-first}/{denominator} " +
                $"equals {total}/{denominator}, with no change to the denominator.",
            Rows =
            [
                new RichLessonVisualRow
                {
                    Label = "First piece", Numerator = first, Denominator = denominator
                },
                new RichLessonVisualRow
                {
                    Label = "Second piece", Numerator = total-first, Denominator = denominator
                },
                new RichLessonVisualRow
                {
                    Label = "Combined", Numerator = total, Denominator = denominator
                }
            ]
        };

    private static RichLessonVisual RepeatedFractionBars(int n,int d,int whole) =>
        new()
        {
            Kind = RichLessonVisualKind.FractionBars,
            Title = $"{whole} equal groups of {n}/{d}",
            Description = $"Represent {whole} separate groups of {n} shaded " +
                $"parts out of {d}. Together there are {whole*n} one-{d}th parts.",
            Rows = Enumerable.Range(1,whole)
                .Select(i => new RichLessonVisualRow
                {
                    Label = $"Group {i}", Numerator = n, Denominator = d
                }).ToList()
        };

    private static RichLessonVisual MarketInventory(IReadOnlyDictionary<string,int> p)
    {
        int a = p["small"], b = p["second"], c = p["third"], sold = p["sold"];
        return new RichLessonVisual
        {
            Kind = RichLessonVisualKind.RatioTable,
            Title = "Market inventory: exactly 100 items",
            Description = $"The stock starts with {a} apples, {b} notebooks and {c} pencils. " +
                $"The seller sells {sold} notebooks, so the remaining notebook stock is {b-sold}. " +
                "All three initial quantities sum to 100.",
            PrimaryLabel = "Item | before sale | after sale",
            Rows =
            [
                new RichLessonVisualRow
                {
                    Label = "Apples", Cells = [a.ToString(),a.ToString()]
                },
                new RichLessonVisualRow
                {
                    Label = "Notebooks", Cells = [b.ToString(),(b-sold).ToString()]
                },
                new RichLessonVisualRow
                {
                    Label = "Pencils", Cells = [c.ToString(),c.ToString()]
                }
            ]
        };
    }

    internal static bool IsB01(string code)
    {
        var tokens = code.Split(':');
        if (tokens.Length != 5 ||
            tokens[0] != "PED" || tokens[1] != "US-CCSS-MATH" ||
            !tokens[2].StartsWith("G", StringComparison.Ordinal) ||
            !tokens[3].StartsWith("U", StringComparison.Ordinal) ||
            !tokens[4].StartsWith("L", StringComparison.Ordinal) ||
            !int.TryParse(tokens[2].AsSpan(1), out var grade) ||
            !int.TryParse(tokens[3].AsSpan(1), out var unit) ||
            !int.TryParse(tokens[4].AsSpan(1), out var lesson) ||
            grade < 1 || grade > 4 || unit < 1 || lesson < 1)
            return false;
        return grade < 4 || unit < 4 || (unit == 4 && lesson <= 20);
    }
}

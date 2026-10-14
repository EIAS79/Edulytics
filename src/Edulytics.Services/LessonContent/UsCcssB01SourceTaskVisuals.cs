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
        // The optional IM G2 U03 L07 center explicitly combines TWO
        // different mathematical tasks: estimate-and-measure and length
        // story problems. It needs both independently verified visuals.
        // Leave the one-visual limit unchanged for all other lessons.
        var visualLimit=lessonCode=="PED:US-CCSS-MATH:G2:U03:L07" ? 2 : 1;
        var visual = new List<RichLessonVisual>(visualLimit);
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
                "usccss.b01.g1.teen_no_regroup" or "usccss.b01.g1.teen_subtract_decompose_ten" or "usccss.b01.g2.story_without_regroup100" or "usccss.b01.g2.story_regroup50" when
                    ((lessonCode=="PED:US-CCSS-MATH:G1:U03:L12" &&
                        q.Family=="usccss.b01.g1.teen_no_regroup") ||
                     (lessonCode=="PED:US-CCSS-MATH:G1:U03:L22" &&
                        q.Family=="usccss.b01.g1.teen_subtract_decompose_ten") ||
                     (lessonCode=="PED:US-CCSS-MATH:G2:U02:L03" &&
                        q.Family=="usccss.b01.g2.story_without_regroup100") ||
                     (lessonCode=="PED:US-CCSS-MATH:G2:U02:L11" &&
                        q.Family=="usccss.b01.g2.story_regroup50")) &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title=q.Family=="usccss.b01.g1.teen_subtract_decompose_ten" ||
                            q.Family=="usccss.b01.g2.story_regroup50" && N("operation")==1
                                ?"Decompose one ten into ten ones"
                                :q.Family=="usccss.b01.g2.story_regroup50"
                                    ?"Compose ten ones into one ten"
                                    :"Keep ten bundles separate from ones",
                        PrimaryLabel="Tens | Ones",
                        Description=$"{N("left")} " +
                            (N("operation")==0?"+":"−") +
                            $" {N("right")} = {q.CorrectAnswer}. " +
                            (q.Family=="usccss.b01.g1.teen_subtract_decompose_ten" ||
                             q.Family=="usccss.b01.g2.story_regroup50" && N("operation")==1
                                ?"Before subtraction rename one ten as ten ones."
                                :q.Family=="usccss.b01.g2.story_regroup50"
                                    ?"Compose ten ones as one additional ten."
                                    :"No regrouping is required."),
                        Rows=new List<RichLessonVisualRow>
                        {
                            new()
                            {
                                Label="Starting number",
                                Cells=[(N("left")/10).ToString(),
                                    (N("left")%10).ToString()]
                            },
                            new()
                            {
                                Label=N("operation")==0?"Add":"Subtract",
                                Cells=[(N("right")/10).ToString(),
                                    (N("right")%10).ToString()]
                            }
                        }.Concat((q.Family=="usccss.b01.g1.teen_subtract_decompose_ten" ||
                                   q.Family=="usccss.b01.g2.story_regroup50" && N("operation")==1)
                            ?new[]{new RichLessonVisualRow
                            {
                                Label="Exchange one ten for ten ones",
                                Cells=[(N("left")/10-1).ToString(),
                                    (N("left")%10+10).ToString()]
                            }}
                            :Array.Empty<RichLessonVisualRow>())
                            .Append(new RichLessonVisualRow
                            {
                                Label="Result",
                                Cells=[(N("expected")/10).ToString(),
                                    (N("expected")%10).ToString()]
                            }).ToList()
                    },
                "usccss.b01.g2.add_subtract_without_regroup" or "usccss.b01.g2.subtract_decompose_ten" when
                    (((lessonCode=="PED:US-CCSS-MATH:G2:U02:L01" ||
                        lessonCode=="PED:US-CCSS-MATH:G2:U03:L17") &&
                        q.Family=="usccss.b01.g2.add_subtract_without_regroup") ||
                     (lessonCode=="PED:US-CCSS-MATH:G2:U02:L07" &&
                        q.Family=="usccss.b01.g2.subtract_decompose_ten")) &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title=q.Family=="usccss.b01.g2.subtract_decompose_ten"
                            ?"Decompose one ten to subtract"
                            :"Add or subtract without regrouping",
                        PrimaryLabel="Tens | Ones",
                        Description=$"{N("left")} " +
                            (N("operation")==0?"+":"−") +
                            $" {N("right")} = {N("expected")}. " +
                            (q.Family=="usccss.b01.g2.subtract_decompose_ten"
                                ?"Exchange one ten for ten ones before subtracting."
                                :"Neither ones nor tens require regrouping."),
                        Rows=new List<RichLessonVisualRow>
                        {
                            new()
                            {
                                Label="First number",
                                Cells=[(N("left")/10).ToString(),
                                    (N("left")%10).ToString()]
                            },
                            new()
                            {
                                Label=N("operation")==0?"Add":"Subtract",
                                Cells=[(N("right")/10).ToString(),
                                    (N("right")%10).ToString()]
                            }
                        }.Concat(q.Family=="usccss.b01.g2.subtract_decompose_ten"
                            ?new[]{new RichLessonVisualRow
                            {
                                Label="One ten exchanged for ten ones",
                                Cells=[(N("left")/10-1).ToString(),
                                    (N("left")%10+10).ToString()]
                            }}
                            :Array.Empty<RichLessonVisualRow>())
                            .Append(new RichLessonVisualRow
                            {
                                Label="Result",
                                Cells=[(N("expected")/10).ToString(),
                                    (N("expected")%10).ToString()]
                            }).ToList()
                    },
                "usccss.b01.g1.add_within100_no_compose_ten" when
                    lessonCode=="PED:US-CCSS-MATH:G1:U05:L04" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="Add tens and ones without composing a ten",
                        PrimaryLabel="Tens | Ones",
                        Description=$"{N("left")} + {N("right")} = " +
                            $"{q.CorrectAnswer}. Since the ones sum is " +
                            $"{N("left")%10+N("right")%10}, no new ten is composed.",
                        Rows=new[]{("First addend",N("left")),
                                   ("Second addend",N("right")),
                                   ("Sum",N("expected"))}
                            .Select(x=>new RichLessonVisualRow
                            {
                                Label=x.Item1,
                                Cells=[(x.Item2/10).ToString(),
                                    (x.Item2%10).ToString()]
                            }).ToList()
                    },
                "usccss.b01.g2.add_subtract_1000_no_regroup" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U07:L04" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="Three-digit addition or subtraction without regrouping",
                        PrimaryLabel="Hundreds | Tens | Ones",
                        Description=$"Compute {N("left")} " +
                            (N("operation")==0?"+":"−") +
                            $" {N("right")} = {q.CorrectAnswer}. " +
                            "No column requires composition or decomposition.",
                        Rows=new[]{("First number",N("left")),
                                   ("Second number",N("right")),
                                   ("Answer",N("expected"))}
                            .Select(x=>new RichLessonVisualRow
                            {
                                Label=x.Item1,
                                Cells=[(x.Item2/100).ToString(),
                                    (x.Item2/10%10).ToString(),
                                    (x.Item2%10).ToString()]
                            }).ToList()
                    },
                "usccss.b01.g4.decimal_number_line_greater" when
                    lessonCode=="PED:US-CCSS-MATH:G4:U04:L03" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.NumberLine,
                        Title="Two decimals on the same hundredths number line",
                        Description=$"The ticks grow by exactly 0.01. " +
                            $"The first number is at tick {N("tickA")} and " +
                            $"the second at tick {N("tickB")}. " +
                            $"The correct larger decimal is {q.CorrectAnswer}.",
                        PrimaryLabel="Compare the two marked decimal positions",
                        SecondaryLabel="Two G4 decimal positions",
                        SecondaryItems=[N("tickA").ToString(),N("tickB").ToString()],
                        Items=Enumerable.Range(0,11)
                            .Select(i=>((N("start")+i)/100m)
                                .ToString("0.00",
                                    System.Globalization.CultureInfo.InvariantCulture))
                            .ToList()
                    },
                "usccss.b01.g2.order_three_digits_number_line" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U05:L09" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.NumberLine,
                        Title="Compare three-digit positions on the same number line",
                        Description=$"The values {N("first")} (tick {N("tickA")}) " +
                            $"and {N("second")} (tick {N("tickB")}) are ten units " +
                            $"apart per tick, so {q.CorrectAnswer} lies farther right.",
                        PrimaryLabel=$"Compare {N("first")} and {N("second")}",
                        SecondaryLabel="Two G2 number-line positions",
                        SecondaryItems=[N("tickA").ToString(),N("tickB").ToString()],
                        Items=Enumerable.Range(0,11)
                            .Select(i=>(N("start")+10*i).ToString()).ToList()
                    },
                "usccss.b01.g4.whole_minus_proper_fraction" when
                    lessonCode=="PED:US-CCSS-MATH:G4:U03:L10" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.FractionBars,
                        Title="Subtract a fraction from complete wholes",
                        Description=$"{N("whole")} wholes equal " +
                            $"{N("whole")*N("denominator")}/{N("denominator")}. " +
                            $"After removing {N("numerator")}/{N("denominator")}, " +
                            $"the remaining amount is {q.CorrectAnswer}.",
                        Rows=Enumerable.Range(0,N("whole"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=i==N("whole")-1
                                    ?"Final whole minus the fractional amount"
                                    :$"Whole {i+1}",
                                Numerator=i==N("whole")-1
                                    ?N("denominator")-N("numerator")
                                    :N("denominator"),
                                Denominator=N("denominator")
                            }).ToList()
                    },
                "usccss.b01.g1.compare_order_tens_ones" when
                    lessonCode=="PED:US-CCSS-MATH:G1:U04:L17" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="Compare tens first and then ones",
                        PrimaryLabel="Tens | Ones",
                        Description=$"The increasing order is " +
                            string.Join(", ",new[]{N("first"),N("second"),N("third")}
                                .OrderBy(x=>x)) +
                            $". The verified selected number is {q.CorrectAnswer}. " +
                            "A tie in tens is resolved by the ones digit.",
                        Rows=new[]{("First",N("first")),
                                   ("Second",N("second")),
                                   ("Third",N("third"))}
                            .Select(entry=>new RichLessonVisualRow
                            {
                                Label=entry.Item1,
                                Cells=[(entry.Item2/10).ToString(),
                                    (entry.Item2%10).ToString()]
                            }).ToList()
                    },
                "usccss.b01.g1.add_subtract_10_tens" or "usccss.b01.g1.add_subtract_tens_multiples" when
                    ((lessonCode=="PED:US-CCSS-MATH:G1:U04:L03" &&
                        q.Family=="usccss.b01.g1.add_subtract_10_tens") ||
                     (lessonCode=="PED:US-CCSS-MATH:G1:U04:L04" &&
                        q.Family=="usccss.b01.g1.add_subtract_tens_multiples")) &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="Add and subtract complete bundles of ten",
                        Description=$"{N("left")/10} tens " +
                            (N("operation")==0?"+":"−") +
                            $" {N("right")/10} tens = {N("expected")/10} tens. " +
                            "All ones-place digits remain zero.",
                        PrimaryLabel="Tens | Ones",
                        Rows=[
                            new RichLessonVisualRow
                            {
                                Label="Starting number",
                                Cells=[(N("left")/10).ToString(),"0"]
                            },
                            new RichLessonVisualRow
                            {
                                Label=N("operation")==0?"Tens added":"Tens removed",
                                Cells=[(N("right")/10).ToString(),"0"]
                            },
                            new RichLessonVisualRow
                            {
                                Label="Answer",
                                Cells=[(N("expected")/10).ToString(),"0"]
                            }
                        ]
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
                "usccss.b01.g2.compose_equal_square_shapes" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U06:L06" &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.ShapeDecomposition,
                        Title="Compose a shape from equal-size square tiles",
                        Description=$"{N("rows")} rows with {N("columns")} " +
                            $"equal-size squares in each row form a {q.CorrectAnswer}. " +
                            $"There are exactly {N("tiles")} tiles.",
                        PrimaryLabel="Equal-size tiles, no gaps",
                        SecondaryLabel="Equal-size-square-grid",
                        Items=[N("rows").ToString(),N("columns").ToString()],
                        Rows=Enumerable.Range(1,N("rows"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Row {i}",
                                Cells=Enumerable.Repeat("■",N("columns")).ToList()
                            }).ToList()
                    },
                "usccss.b01.g3.multiplication_division_inverse_array" when
                    lessonCode=="PED:US-CCSS-MATH:G3:U08:L10" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.ShapeDecomposition,
                        Title="One array shows both multiplication and division",
                        Description=$"{N("rows")} rows of {N("columns")} objects " +
                            $"have exactly {N("total")} squares. " +
                            $"{N("rows")} × {N("columns")} = {N("total")} and " +
                            $"{N("total")} ÷ {N("rows")} = {N("columns")}. " +
                            $"The requested answer is {q.CorrectAnswer}.",
                        PrimaryLabel="Equal-size rows and columns",
                        Rows=Enumerable.Range(1,N("rows"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Row {i}",
                                Cells=Enumerable.Repeat("■",N("columns")).ToList()
                            }).ToList()
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
                // Original G2 U03 L06: real measured lengths under 100cm;
                // the visual holds long, short and difference with the true
                // unknown position, not generic unmeasured counters.
                "usccss.b01.g2.length_compare.all_unknown_positions100" when
                    (lessonCode=="PED:US-CCSS-MATH:G2:U03:L06" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U03:L07") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.TapeDiagram,
                        Title="Compare reptile lengths: three possible unknowns",
                        Description=$"The longer reptile is {N("longCount")} cm, " +
                            $"the shorter reptile is {N("shortCount")} cm, " +
                            $"and the difference is {N("difference")} cm. " +
                            $"Unknown position {N("unknownPosition")} has " +
                            $"the verified answer {q.CorrectAnswer} cm.",
                        PrimaryLabel="0 = difference, 1 = longer, 2 = shorter",
                        Items=[
                            N("longCount").ToString(),
                            N("shortCount").ToString(),
                            N("difference").ToString(),
                            N("unknownPosition").ToString()
                        ]
                    },
                "usccss.b01.g2.compare_tape_unknown_positions" when
                    (lessonCode=="PED:US-CCSS-MATH:G2:U01:L15" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U01:L16") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.TapeDiagram,
                        Title="Compare: find the missing quantity in the tape diagram",
                        Description=$"The longer bar contains {N("longCount")} units; " +
                            $"the shorter bar contains {N("shortCount")}. " +
                            $"The difference is {N("difference")}. " +
                            $"Missing position {N("unknownPosition")} " +
                            $"has the checked answer {q.CorrectAnswer}.",
                        PrimaryLabel="0 = difference, 1 = larger, 2 = smaller",
                        Items=[
                            N("longCount").ToString(),
                            N("shortCount").ToString(),
                            N("difference").ToString(),
                            N("unknownPosition").ToString()
                        ]
                    },
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
                "usccss.b01.g4.eighth_inch_line_plot_difference" when
                    lessonCode=="PED:US-CCSS-MATH:G4:U03:L13" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title="Interpret eighth-inch measurements and subtract fractions",
                        Description="Each equally spaced tick is 1/8 inch. " +
                            "The labels 2/8, 4/8, and 6/8 also represent " +
                            "quarters or halves. Every X is a real observation. " +
                            $"The two selected lengths differ by {q.CorrectAnswer} inch.",
                        PrimaryLabel="Shoe length (inches; nearest 1/8)",
                        Rows=Enumerable.Range(0,9)
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=(i==0 ? N("whole").ToString() :
                                    i==8 ? (N("whole")+1).ToString() :
                                    $"{N("whole")} {i}/8")+" in",
                                Cells=Enumerable.Repeat("X",N($"count{i}")).ToList()
                            }).ToList()
                    },
                "usccss.b01.g3.quarter_inch_line_plot_frequency" when
                    (lessonCode=="PED:US-CCSS-MATH:G3:U06:L04" ||
                     lessonCode=="PED:US-CCSS-MATH:G3:U06:L05" ||
                     lessonCode=="PED:US-CCSS-MATH:G4:U03:L13") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title=lessonCode=="PED:US-CCSS-MATH:G3:U06:L05"
                            ?"Construct fractional inch measurement line plots"
                            :"Interpret fractional inch measurement line plots",
                        Description="The axis advances by exactly one quarter inch " +
                            "between adjacent measurements. Every X is one " +
                            $"observation; the requested tick contains {q.CorrectAnswer}.",
                        PrimaryLabel="Inches (whole, 1/4, 1/2, 3/4)",
                        Rows=new[]{"1","1 1/4","1 1/2","1 3/4","2"}
                            .Select((name,i)=>new RichLessonVisualRow
                            {
                                Label=name+" in",
                                Cells=Enumerable.Repeat("X",N($"count{i+1}")).ToList()
                            }).ToList()
                    },
                "usccss.b01.g2.measurement.plot_interpret_nonzero_start" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U03:L16" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title="Plant-height line plot with a nonzero starting length",
                        Description="Each X is a measured plant. The plot begins " +
                            "at the smallest actual length, and each tick is 1 cm. " +
                            $"The verified answer is {q.CorrectAnswer}.",
                        PrimaryLabel="Plant height (cm)",
                        Rows=Enumerable.Range(0,N("lastTick")+1)
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"{N("start")+i} cm",
                                Cells=Enumerable.Repeat("X",N($"count{i}")).ToList()
                            }).ToList()
                    },
                // Grade 2 U09 L04: measured pencil data must actually
                // produce one X per pencil with a consistent centimeter axis.
                // G2 U03 L04: the actual centimeter measurement is
                // read at the endpoint of an equal-unit ruler. No guessed
                // measurements or extrapolated longer-object activity.
                // Repeat one 30cm ruler without shifting the reference
                // measurements: every row has real exact start/end marks.
                // Original G2 U03 L05 meter-stick reptiles. Every
                // centimeter measurement is taken from the authorized
                // source's seven tape labels; no synthetic length claim.
                "usccss.b01.g2.measure.reptile_meterstick" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U03:L05" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="One-meter sticks and leftover centimeters",
                        Description=$"The teacher's tape label " +
                            $"{(char)('A'+N("tapeIndex"))} measures " +
                            $"{N("lengthCm")} cm. " +
                            $"There are {N("fullMeters")} complete 100 cm " +
                            $"placements and {N("remainingCm")} cm remaining. " +
                            $"100 × {N("fullMeters")} + {N("remainingCm")} = " +
                            $"{N("lengthCm")} cm. " +
                            $"The verified question answer is {q.CorrectAnswer}.",
                        PrimaryLabel=$"{N("lengthCm")} cm = " +
                            $"{N("fullMeters")} m {N("remainingCm")} cm",
                        Rows=Enumerable.Range(0,N("fullMeters"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Meter-stick pass {i+1}",
                                Cells=[(i*100).ToString(),
                                    ((i+1)*100).ToString(),"100 cm"]
                            })
                            .Concat(N("remainingCm")>0
                                ? new[]
                                {
                                    new RichLessonVisualRow
                                    {
                                        Label="Final shorter section",
                                        Cells=[(N("fullMeters")*100).ToString(),
                                            N("lengthCm").ToString(),
                                            $"{N("remainingCm")} cm"]
                                    }
                                }
                                : Array.Empty<RichLessonVisualRow>())
                            .ToList()
                    },
                "usccss.b01.g2.measurement.iterate_ruler_cm" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U03:L04" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.PlaceValueChart,
                        Title="Measure a long object by repeating the ruler",
                        Description=$"Use {N("fullSpans")} complete 30 cm ruler spans " +
                            $"plus {N("remainder")} cm on the final section. " +
                            $"The measured length is {q.CorrectAnswer} cm; " +
                            "each section begins exactly where the previous ends.",
                        PrimaryLabel=$"Full 30 cm passes: {N("fullSpans")}; " +
                            $"final remainder: {N("remainder")} cm",
                        Rows=Enumerable.Range(0,N("fullSpans"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Pass {i+1}: 30 cm",
                                Cells=[(i*30).ToString(),
                                    ((i+1)*30).ToString(),"30 cm"]
                            })
                            .Concat(N("remainder")>0
                                ? new[]
                                {
                                    new RichLessonVisualRow
                                    {
                                        Label="Final partial pass",
                                        Cells=[(N("fullSpans")*30).ToString(),
                                            N("total").ToString(),
                                            $"{N("remainder")} cm"]
                                    }
                                }
                                : Array.Empty<RichLessonVisualRow>())
                            .ToList()
                    },
                "usccss.b01.g2.measurement.estimate_cm" when
                    (lessonCode=="PED:US-CCSS-MATH:G2:U03:L04" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U03:L07") &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.Scale,
                        Title="Estimate, then measure with centimeter marks",
                        Description=$"First estimate: {N("estimate")} cm. " +
                            $"Start at the ruler's 0 mark; the object ends " +
                            $"at {N("actual")} cm. The error is " +
                            $"{N("error")} cm, verified from the ruler values. " +
                            "Each adjacent ruler tick is 1 cm.",
                        PrimaryLabel=$"Measured: {N("actual")} cm; estimate: {N("estimate")} cm",
                        SecondaryItems=["0",N("actual").ToString()],
                        Items=Enumerable.Range(0,N("actual")+1)
                            .Select(i=>i.ToString()).ToList()
                    },
                "usccss.b01.g2.measurement.pencil_line_plot" when
                    lessonCode=="PED:US-CCSS-MATH:G2:U09:L04" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title="Construct a centimeter line plot from pencil measurements",
                        Description="The horizontal scale advances by exactly 1 cm. " +
                            "Each X represents a measured pencil. " +
                            $"There are {Enumerable.Range(0,7).Sum(i=>N($"count{i}"))} " +
                            "pencils in all. " +
                            $"The independently verified answer is {q.CorrectAnswer}.",
                        PrimaryLabel="Pencil length in centimeters",
                        Rows=Enumerable.Range(0,7)
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"{N("start")+i} cm",
                                Cells=Enumerable.Repeat("X",N($"count{i}")).ToList()
                            }).ToList()
                    },
                "usccss.b01.g2.measurement.read_line_plot" when
                    (lessonCode=="PED:US-CCSS-MATH:G2:U03:L14" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U03:L15" ||
                     lessonCode=="PED:US-CCSS-MATH:G2:U03:L17") &&
                    ExactSkillContractQuestionEngine.Verify(q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.LinePlot,
                        Title=lessonCode.EndsWith("L15",StringComparison.Ordinal) ||
                            lessonCode.EndsWith("L17",StringComparison.Ordinal)
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
                "usccss.b01.geometry.rectangle_area_real_world_units" when
                    N("length")>0 && N("width")>0 &&
                    (N("unitMode") is 0 or 1) &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Measure and multiply a real-world rectangular area",
                        Description=$"The two sides are {N("length")} and {N("width")} " +
                            (N("unitMode")==0?"meters":"feet") +
                            $". This gives {q.CorrectAnswer} square " +
                            (N("unitMode")==0?"meters":"feet") +
                            ", checked independently by length × width.",
                        SecondaryLabel=N("unitMode")==0?"m":"ft",
                        Items=[N("length").ToString(),N("width").ToString()]
                    },
                "usccss.b01.g3.same_perimeter_different_area" or "usccss.b01.g3.same_area_different_perimeter" when
                    ((lessonCode=="PED:US-CCSS-MATH:G3:U07:L11" &&
                        q.Family=="usccss.b01.g3.same_perimeter_different_area") ||
                     (lessonCode=="PED:US-CCSS-MATH:G3:U07:L12" &&
                        q.Family=="usccss.b01.g3.same_area_different_perimeter")) &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Rectangle A: area and perimeter",
                        Description=$"Rectangle A is {N("widthA")} by {N("heightA")}; " +
                            $"area {N("widthA")*N("heightA")} square units, " +
                            $"perimeter {2*(N("widthA")+N("heightA"))}. " +
                            $"The requested verified difference is {q.CorrectAnswer}.",
                        SecondaryLabel="fixed-comparison-scale",
                        Items=[N("widthA").ToString(),N("heightA").ToString()]
                    },
                "usccss.b01.g4.factor_pairs_two_rectangles" when
                    lessonCode=="PED:US-CCSS-MATH:G4:U01:L02" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Factor pair A: build a rectangle",
                        Description=$"Factor pair {N("widthA")}×{N("heightA")} " +
                            $"makes {N("area")} unit squares. " +
                            "The next rectangle uses a different factor pair.",
                        SecondaryLabel="fixed-comparison-scale",
                        Items=[N("widthA").ToString(),N("heightA").ToString()]
                    },
                "usccss.b01.g4.rectangle_area_is_multiple" when
                    lessonCode=="PED:US-CCSS-MATH:G4:U01:L01" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Rectangle area is a multiple of either side length",
                        PrimaryLabel=$"{N("length")} rows × {N("width")} per row",
                        Description=$"{N("length")}×{N("width")} = " +
                            $"{q.CorrectAnswer} square units. Dividing this area " +
                            $"by either {N("length")} or {N("width")} leaves a whole " +
                            "number of equal groups, so the area is a multiple of both.",
                        Items=[N("length").ToString(),N("width").ToString()]
                    },
                "usccss.b01.g3.distributive_area_partition" when
                    lessonCode=="PED:US-CCSS-MATH:G3:U04:L10" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.ShapeDecomposition,
                        Title="A real area partition demonstrates multiplication distribution",
                        Description=$"{N("height")}×({N("leftWidth")}+" +
                            $"{N("rightWidth")}) = " +
                            $"{N("height")*N("leftWidth")}+" +
                            $"{N("height")*N("rightWidth")} = " +
                            $"{q.CorrectAnswer}. Each symbol is exactly one " +
                            "unit square; the two sections have no overlaps.",
                        PrimaryLabel="Dark squares | light squares",
                        Rows=Enumerable.Range(1,N("height"))
                            .Select(i=>new RichLessonVisualRow
                            {
                                Label=$"Row {i}",
                                Cells=Enumerable.Repeat("■",N("leftWidth"))
                                    .Concat(Enumerable.Repeat("□",N("rightWidth")))
                                    .ToList()
                            }).ToList()
                    },
                "usccss.b01.g3.rectangle_area_perimeter_story" when
                    lessonCode=="PED:US-CCSS-MATH:G3:U07:L10" &&
                    ExactSkillContractQuestionEngine.Verify(
                        q.Family,p,q.CorrectAnswer) =>
                    new RichLessonVisual
                    {
                        Kind=RichLessonVisualKind.RectangleDiagram,
                        Title="Playground: compare surface area with boundary perimeter",
                        Description=$"Side lengths {N("length")} m and {N("width")} m. " +
                            $"Area is {N("length")*N("width")} square meters, " +
                            $"perimeter is {2*(N("length")+N("width"))} meters. " +
                            $"The question asks for { (N("mode")==0?"area":"perimeter")} " +
                            $"and its checked answer is {q.CorrectAnswer}.",
                        SecondaryLabel="m",
                        Items=[N("length").ToString(),N("width").ToString()]
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
            // The source comparison has TWO different actual rectangles,
            // both with the SAME fixed pixel/unit scale in the student view.
            if (item is not null &&
                (q.Family is "usccss.b01.g3.same_perimeter_different_area" or
                    "usccss.b01.g3.same_area_different_perimeter" or
                    "usccss.b01.g4.factor_pairs_two_rectangles") &&
                item.SecondaryLabel=="fixed-comparison-scale")
            {
                visual.Add(new RichLessonVisual
                {
                    Kind=RichLessonVisualKind.RectangleDiagram,
                    Title=q.Family=="usccss.b01.g4.factor_pairs_two_rectangles"
                        ?"Factor pair B: a different rectangle with the same area"
                        :"Rectangle B: area and perimeter",
                    Description=$"Rectangle B is {N("widthB")} by {N("heightB")}; " +
                        $"area {N("widthB")*N("heightB")} square units, " +
                        $"perimeter {2*(N("widthB")+N("heightB"))}. " +
                        $"Compare with rectangle A: answer {q.CorrectAnswer}.",
                    SecondaryLabel="fixed-comparison-scale",
                    Items=[N("widthB").ToString(),N("heightB").ToString()]
                });
            }
            // Keep the page focused: only one task-scoped visual per lesson;
            // the ordinary QuestionForms diagram remains available afterward.
            if (visual.Count >= visualLimit) break;
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

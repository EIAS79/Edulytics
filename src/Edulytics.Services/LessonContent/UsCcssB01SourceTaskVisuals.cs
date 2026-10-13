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
            if (!q.Family.StartsWith("usccss.b01.", StringComparison.Ordinal))
                continue;
            var p = q.Parameters;
            int N(string k) => p[k];
            RichLessonVisual? item = q.Family switch
            {
                "usccss.b01.array.equal_groups" or
                "usccss.b01.rectangle.unit_squares" =>
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.ShapeDecomposition,
                        Title = $"An array of {N("rows")} rows and {N("columns")} columns",
                        Description = $"Count {N("columns")} equal unit squares in each of {N("rows")} rows. " +
                            $"The total is {N("rows")} × {N("columns")} = {N("rows") * N("columns")}.",
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
                "usccss.b01.fraction.decompose_same_denominator" =>
                    DecompositionBar(N("wholeN"), N("firstN"), N("denominator")),
                "usccss.b01.fraction.whole_multiply" =>
                    RepeatedFractionBars(N("numerator"), N("denominator"), N("whole")),
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

    private static bool IsB01(string code)
    {
        if (!code.StartsWith("PED:US-CCSS-MATH:G",StringComparison.Ordinal))
            return false;
        var tokens=code.Split(':');
        if (tokens.Length!=4) return false;
        var grade=tokens[2];
        if (!grade.StartsWith("G",StringComparison.Ordinal) ||
            !int.TryParse(grade.AsSpan(1),out var g) || g<1 || g>4)
            return false;
        if (g<4) return true;
        return int.TryParse(tokens[3].AsSpan(1),out _) || code.CompareTo(
            "PED:US-CCSS-MATH:G4:U04:L20")<=0;
    }
}

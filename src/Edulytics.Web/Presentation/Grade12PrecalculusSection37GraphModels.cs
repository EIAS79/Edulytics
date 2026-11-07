using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party independently authored Section 3.7 rational-function graphs.
/// Each model supports a source-equivalent graph-reading exercise, not copied
/// textbook pixels or claims of numerical identity with source graphs 57–64.
/// Reuses existing typed SVG graph renderer. Stage D not run.
/// </summary>
public static class Grade12PrecalculusSection37GraphModels
{
    public sealed record GraphTask(int ExerciseNumber, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All = Build();
    private static IReadOnlyList<GraphTask> Build()
    {
        var tasks = new List<GraphTask>();
        Add(57, x => (x-1m)/((x+2m)*(x-3m)), [-2m, 3m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Simple zero; two vertical poles; numerator degree lower.");
        Add(58, x => (x+2m)/((x+3m)*(x-2m)), [-3m, 2m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Simple zero between two vertical poles.");
        Add(59, x => (x-2m)/((x+1m)*(x-4m)), [-1m, 4m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Two vertical poles and a middle-branch x-axis crossing.");
        Add(60, x => (x+1m)/((x+3m)*(x-4m)), [-3m, 4m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Three separated branches with a single zero.");
        Add(61, x => (x-1m)/(x-2m), [2m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "One vertical pole and nonzero horizontal end level.");
        Add(62, x => (x+3m)/(x+1m), [-1m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "One vertical pole, horizontal end level one.");
        Add(63, x => ((x-2m)*(x-2m))/((x+2m)*(x-4m)), [-2m, 4m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Middle branch has an even-multiplicity contact at x=2.");
        Add(64, x => ((x-1m)*(x-1m))/((x+3m)*(x-4m)), [-3m, 4m],
            "Infer one factored formula, roots and asymptotes from this independent graph.",
            "Middle branch touches the x-axis at x=1.");

        return tasks;

        void Add(int exercise, Func<decimal, decimal> evaluate,
            decimal[] excludedPoles, string prompt, string accessibleFeature)
        {
            var segments = new List<MathematicalSegment>();
            MathematicalPoint? previous = null;
            decimal? previousX = null;
            for (var index = 0; index <= 140; index++)
            {
                var x = -7m + index / 10m;
                // Do not bridge real poles with a straight segment.
                if (excludedPoles.Any(p => Math.Abs(x - p) < 0.17m))
                {
                    previous = null;
                    previousX = null;
                    continue;
                }
                var y = evaluate(x);
                if (y < -6m || y > 6m)
                {
                    previous = null;
                    previousX = null;
                    continue;
                }
                var point = new MathematicalPoint(x, y);
                if (previous is not null && previousX is decimal lastX && x - lastX <= 0.11m)
                    segments.Add(new MathematicalSegment(previous, point));
                previous = point;
                previousX = x;
            }
            var drawing = new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m, 7m, -6m, 6m, 1m, 1m),
                [], segments,
                new MathematicalVisualAccessibility(
                    $"Independent rational graph exercise {exercise}",
                    accessibleFeature + " The curves are separated at excluded poles, " +
                    "and no textbook figure has been copied."));
            MathematicalVisualSpecValidator.Validate(drawing);
            tasks.Add(new GraphTask(exercise, prompt, drawing));
        }
    }
}

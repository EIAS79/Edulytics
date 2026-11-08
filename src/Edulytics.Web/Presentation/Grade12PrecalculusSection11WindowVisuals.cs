using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independently authored source-window graphs for OpenStax Precalculus
/// Section 1.1 exercises 76–87. Physical values are mapped to dimensionless
/// normalized coordinates so even a +/-1,000,000 window uses the existing
/// bounded SVG renderer. The labels and exact output ranges remain in the
/// mathematical source units; no relaxation of global renderer limits.
/// </summary>
public static class Grade12PrecalculusSection11WindowVisuals
{
    public sealed record WindowTask(
        int ExerciseNumber, string FunctionLabel, decimal XMin,
        decimal XMax, string ExactOutputRange, CoordinatePlaneVisualSpec Graph);

    public static readonly IReadOnlyList<WindowTask> All = Create();

    private static IReadOnlyList<WindowTask> Create()
    {
        var items = new List<WindowTask>();
        Add(76, 1, -.1m, .1m, "0 ≤ y ≤ 0.01");
        Add(77, 1, -10m, 10m, "0 ≤ y ≤ 100");
        Add(78, 1, -100m, 100m, "0 ≤ y ≤ 10,000");
        Add(79, 2, -.1m, .1m, "-0.001 ≤ y ≤ 0.001");
        Add(80, 2, -10m, 10m, "-1,000 ≤ y ≤ 1,000");
        Add(81, 2, -100m, 100m, "-1,000,000 ≤ y ≤ 1,000,000");
        Add(82, 3, 0m, .01m, "0 ≤ y ≤ 0.1");
        Add(83, 3, 0m, 100m, "0 ≤ y ≤ 10");
        Add(84, 3, 0m, 10000m, "0 ≤ y ≤ 100");
        Add(85, 4, -.001m, .001m, "-0.1 ≤ y ≤ 0.1");
        Add(86, 4, -1000m, 1000m, "-10 ≤ y ≤ 10");
        Add(87, 4, -1000000m, 1000000m, "-100 ≤ y ≤ 100");
        if (items.Count != 12)
            throw new InvalidOperationException("Unexpected graph-window inventory.");
        return items;

        void Add(int exercise, int kind, decimal xMin, decimal xMax,
            string outputRange)
        {
            var symmetric = xMin < 0m;
            var curve = kind switch
            {
                1 => "y = x²",
                2 => "y = x³",
                3 => "y = √x",
                _ => "y = ∛x"
            };
            var outputMagnitude = kind switch
            {
                1 => xMax * xMax,
                2 => xMax * xMax * xMax,
                3 => (decimal)Math.Sqrt((double)xMax),
                _ => (decimal)Math.Cbrt((double)xMax)
            };
            // A bounded, dimensionless viewport encodes the exact physical
            // x/y window through displayed axis endpoint tick labels.
            var window = new MathematicalAxisWindow(
                symmetric ? -1m : 0m, 1m,
                (symmetric && kind is 2 or 4) ? -1m : 0m, 1m,
                .5m, .5m);
            var vertices = new MathematicalPoint[65];
            for (var i = 0; i <= 64; i++)
            {
                var physicalX = xMin + (xMax-xMin)*i/64m;
                var physicalY = kind switch
                {
                    1 => physicalX*physicalX,
                    2 => physicalX*physicalX*physicalX,
                    3 => (decimal)Math.Sqrt((double)physicalX),
                    _ => (decimal)Math.Cbrt((double)physicalX)
                };
                vertices[i] = new MathematicalPoint(
                    physicalX / xMax,
                    physicalY / outputMagnitude);
            }
            var segments = new MathematicalSegment[64];
            for (var i = 1; i < vertices.Length; i++)
                segments[i-1] = new MathematicalSegment(vertices[i-1],vertices[i]);
            var xTicks = symmetric
                ? new [] {
                    new MathematicalAxisTick(-1m, $"{xMin:G29}"),
                    new MathematicalAxisTick(0m,"0"),
                    new MathematicalAxisTick(1m,$"{xMax:G29}")
                }
                : new [] {
                    new MathematicalAxisTick(0m,"0"),
                    new MathematicalAxisTick(1m,$"{xMax:G29}")
                };
            var yTicks = symmetric && kind is 2 or 4
                ? new [] {
                    new MathematicalAxisTick(-1m,$"{-outputMagnitude:G29}"),
                    new MathematicalAxisTick(0m,"0"),
                    new MathematicalAxisTick(1m,$"{outputMagnitude:G29}")
                }
                : new [] {
                    new MathematicalAxisTick(0m,"0"),
                    new MathematicalAxisTick(1m,$"{outputMagnitude:G29}")
                };
            var graph = new CoordinatePlaneVisualSpec(
                window,[],segments,
                new MathematicalVisualAccessibility(
                    $"Exercise {exercise}: {curve} on [{xMin:G29},{xMax:G29}]",
                    $"First-party normalized coordinate graph. Physical x window: " +
                    $"[{xMin:G29},{xMax:G29}]. Exact range: {outputRange}. " +
                    "Axis endpoint labels are physical values; shape is " +
                    "normalized to avoid the shared SVG renderer's maximum-axis-span limit."))
            {
                XAxisTicks = xTicks,
                YAxisTicks = yTicks
            };
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new WindowTask(exercise,curve,xMin,xMax,outputRange,graph));
        }
    }

    public static WindowTask? Find(int originalNumber) =>
        All.FirstOrDefault(item => item.ExerciseNumber==originalNumber);
}

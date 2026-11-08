using System.Collections;
using System.Reflection;
using Edulytics.Core.Mathematics.Visuals;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D verifies the actual trusted-SVG renderer, not only RichV2 JSON.
/// A graph that violates a visual contract used to crash authenticated pages.
/// </summary>
public sealed class Grade12PrecalculusAllSourceGraphRenderTests
{
    [Fact]
    public void RationalFunctionsEightModelsRenderUnderSegmentLimit()
    {
        var models = Grade12PrecalculusSection37GraphModels.All;
        Assert.Equal(8, models.Count);
        foreach (var model in models)
        {
            Assert.InRange(model.Graph.Segments.Count, 1,
                MathematicalVisualSpecValidator.MaximumSegments);
            var svg = AdvancedMathematicsVisualRenderer.RenderSvg(model.Graph);
            Assert.Contains("<svg", svg, StringComparison.Ordinal);
            Assert.Contains("</svg>", svg, StringComparison.Ordinal);
            Assert.Contains("role='img'", svg, StringComparison.Ordinal);
            Assert.DoesNotContain("<script", svg, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void EveryGrade12SectionModelAdvertisedVisualRendersWithoutThrowing()
    {
        var assembly = typeof(Grade12PrecalculusSection37GraphModels).Assembly;
        var types = assembly.GetExportedTypes()
            .Where(t => t.Namespace == "Edulytics.Web.Presentation" &&
                t.Name.StartsWith("Grade12PrecalculusSection", StringComparison.Ordinal))
            .ToArray();
        var inspected = 0;
        foreach (var type in types)
        {
            var member = type.GetField("All", BindingFlags.Public | BindingFlags.Static);
            object? all = member?.GetValue(null);
            if (all is not IEnumerable items || all is string)
                continue;
            foreach (var item in items)
            {
                if (item is null) continue;
                if (item is MathematicalVisualSpec directly)
                {
                    Check(directly);
                    inspected++;
                    continue;
                }
                foreach (var prop in item.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => typeof(MathematicalVisualSpec).IsAssignableFrom(p.PropertyType)))
                {
                    if (prop.GetValue(item) is MathematicalVisualSpec visual)
                    {
                        Check(visual);
                        inspected++;
                    }
                }
            }
        }
        Assert.True(inspected >= 8, "No real Grade12 mathematical graphs were exercised.");
    }

    private static void Check(MathematicalVisualSpec visual)
    {
        MathematicalVisualSpecValidator.Validate(visual);
        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(visual);
        Assert.Contains("<svg", svg, StringComparison.Ordinal);
        Assert.Contains("</svg>", svg, StringComparison.Ordinal);
        Assert.Contains("role='img'", svg, StringComparison.Ordinal);
        Assert.DoesNotContain("<script", svg, StringComparison.OrdinalIgnoreCase);
    }
}

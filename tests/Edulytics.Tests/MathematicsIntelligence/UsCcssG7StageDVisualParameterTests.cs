using System.Text.Json;
using Edulytics.Web.Presentation;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG7StageDVisualParameterTests
{
    [Theory]
    [InlineData("usccss.g7.u01.l04.angle_and_distance_scaling", "{\"variant\":0}")]
    [InlineData("usccss.g7.u01.l04.angle_and_distance_scaling", "{\"variant\":24}")]
    [InlineData("usccss.g7.u07.complementary_angle", "{\"known\":37}")]
    [InlineData("usccss.g7.u07.vertical_angle", "{\"known\":112}")]
    [InlineData("usccss.g7.u03.l02.radius_diameter_relation", "{\"radius\":9,\"mode\":0}")]
    [InlineData("usccss.g7.u03.l02.radius_diameter_relation", "{\"radius\":9,\"mode\":1}")]
    [InlineData("usccss.g7.u03.l05.wheel_rotations_distance", "{\"mode\":0,\"rotations\":5,\"diameter\":7}")]
    [InlineData("usccss.g7.u03.l05.wheel_rotations_distance", "{\"mode\":2,\"rotations\":5,\"circumference\":22}")]
    [InlineData("usccss.g7.u03.l09.circle_fraction_area_pi_coefficient", "{\"radius\":8,\"denominator\":2}")]
    [InlineData("usccss.g7.u03.l09.circle_fraction_area_pi_coefficient", "{\"radius\":8,\"denominator\":4}")]
    public void ExactGrade7GeometryAndCircleParametersRenderDeterministically(
        string family, string parameters)
    {
        var json = "{\"parameters\":" + parameters + "}";
        var first = PracticeMathVisualRenderer.RenderSvg(family, json);
        var second = PracticeMathVisualRenderer.RenderSvg(family, json);
        Assert.NotNull(first);
        Assert.Equal(first, second);
        Assert.StartsWith("<svg", first, StringComparison.Ordinal);
        Assert.Contains("role='img'", first, StringComparison.Ordinal);
        Assert.Contains("</svg>", first, StringComparison.Ordinal);
        Assert.DoesNotContain("<script", first, StringComparison.OrdinalIgnoreCase);
    }
}
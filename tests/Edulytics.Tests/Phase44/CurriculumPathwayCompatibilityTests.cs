using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.Phase44;

public sealed class CurriculumPathwayCompatibilityTests
{
    [Theory]
    [InlineData("Extended", "Extended")]
    [InlineData("Extended", null)]
    [InlineData("Liceum ogólnokształcące", "Liceum ogólnokształcące | Technikum")]
    [InlineData("Technikum", "Liceum ogólnokształcące | Technikum")]
    [InlineData("Course/pathway mapping", null)]
    [InlineData("Component/route structure preserved in reference graph", "Pure Mathematics 1")]
    [InlineData("Component/route structure preserved in reference graph", "Mechanics")]
    public void Selected_pathway_accepts_authoritative_compatible_content(
        string selected,
        string? content)
    {
        Assert.True(
            CurriculumPathwayCompatibility.Matches(
                selected,
                content));
    }

    [Theory]
    [InlineData(null, "Advanced")]
    [InlineData("Advanced", "General")]
    [InlineData("Liceum ogólnokształcące", "Technikum")]
    public void Selected_pathway_rejects_incompatible_content(
        string? selected,
        string? content)
    {
        Assert.False(
            CurriculumPathwayCompatibility.Matches(
                selected,
                content));
    }
}

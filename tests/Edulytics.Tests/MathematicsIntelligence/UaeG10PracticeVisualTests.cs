using System.Text.Json;
using System.Xml.Linq;
using Edulytics.Core.Mathematics.Skills;
using Edulytics.Services.Mathematics;
using Edulytics.Web.Presentation;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10PracticeVisualTests
{
    [Fact]
    public void EveryDeclaredG10VisualFamilyRendersDeterministicallyAcrossSeeds()
    {
        using var stream=typeof(SkillContract).Assembly.GetManifestResourceStream(
            "Edulytics.Core.Mathematics.Generation.question-family-registry.v1.json");
        Assert.NotNull(stream);
        using var document=JsonDocument.Parse(stream!);
        string[] representations=["diagram_metadata","graph","vector","right_triangle","angle","coordinate_pair","solid_dimensions"];
        var families=document.RootElement.GetProperty("families").EnumerateArray()
            .Where(x=>x.GetProperty("id").GetString()!.StartsWith("uae.g10.general.",StringComparison.Ordinal)
                && x.TryGetProperty("lessonPracticeRouting",out var routing)&&routing.ValueKind==JsonValueKind.True
                && x.GetProperty("representations").EnumerateArray().Any(y=>representations.Contains(y.GetString())))
            .Select(x=>x.GetProperty("id").GetString()!).ToArray();
        Assert.Equal(33,families.Length);
        var engine=new ExactSkillContractQuestionEngine();
        var failures=new List<string>();
        foreach(var family in families)
        foreach(int seed in new[]{17,20260920,20261127,7241})
        {
            try
            {
                var q=engine.Generate("g10-visual-coverage",family,[family],ExactSkillQuestionDifficulty.Standard,1,seed,[]).Single();
                var json=JsonSerializer.Serialize(new{parameters=q.Parameters});
                var svg=PracticeMathVisualRenderer.RenderSvg(family,json);
                Assert.False(string.IsNullOrWhiteSpace(svg),family+" seed "+seed);
                Assert.Equal(svg,PracticeMathVisualRenderer.RenderSvg(family,json));
                var xml=XDocument.Parse(svg!);
                XNamespace ns="http://www.w3.org/2000/svg";
                Assert.Equal(ns+"svg",xml.Root!.Name);
                Assert.True(xml.Descendants(ns+"line").Any()||xml.Descendants(ns+"polyline").Any());
                Assert.Empty(xml.Descendants(ns+"script"));
                Assert.DoesNotContain("NaN",svg!,StringComparison.Ordinal);
                Assert.DoesNotContain("Infinity",svg!,StringComparison.Ordinal);
            }
            catch(Exception e){failures.Add(family+" seed "+seed+": "+e.Message);}
        }
        Assert.True(failures.Count==0,string.Join("\n",failures));
    }

    [Theory]
    [InlineData("{\"a\":0,\"h\":0,\"k\":0,\"n\":1}")]
    [InlineData("{\"a\":\"1\",\"h\":0,\"k\":0,\"n\":1}")]
    [InlineData("{\"a\":2,\"h\":0,\"k\":0}")]
    [InlineData("{\"a\":2,\"h\":8,\"k\":0,\"n\":1}")]
    public void RadicalRejectsInvalidMetadata(string json)
        =>Assert.Null(PracticeMathVisualRenderer.RenderSvg("uae.g10.general.u02.l07.radical_graph_y_value",json));

    [Fact]
    public void UnknownFamilyDoesNotReceiveGenericGraphic()
        =>Assert.Null(PracticeMathVisualRenderer.RenderSvg("uae.g10.general.unknown","{}"));

    [Fact]
    public void RadicalAndQuadraticUseTheirDifferentFunctions()
    {
        var radical=PracticeMathVisualRenderer.RenderSvg("uae.g10.general.u02.l07.radical_graph_y_value","{\"a\":1,\"h\":0,\"k\":0,\"n\":3}");
        var parabola=PracticeMathVisualRenderer.RenderSvg("uae.g10.general.u04.l01.vertex_x","{\"a\":1,\"b\":0,\"c\":0,\"probe\":1}");
        Assert.NotNull(radical);Assert.NotNull(parabola);
        XNamespace ns="http://www.w3.org/2000/svg";
        Assert.NotEqual(XDocument.Parse(radical!).Descendants(ns+"polyline").Single().Attribute("points")!.Value,
            XDocument.Parse(parabola!).Descendants(ns+"polyline").Single().Attribute("points")!.Value);
    }
}

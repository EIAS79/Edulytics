using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// U05 L04 2021 source-text and independent first-party mathematical oracles.
/// Contracts are AUTHORING-ONLY and do not certify the original image geometry.
/// </summary>
public sealed class UsCcssG3Unit5Lesson4SecretFractionsTests
{
    private static JsonElement Source()
    {
        using var stream = typeof(Edulytics.Data.Seeding.CommonCoreG3SourceTeachingCorrections)
            .Assembly.GetManifestResourceStream("Edulytics.Data.Grade3.Unit5Lesson4RichV2");
        Assert.NotNull(stream);
        using var document = JsonDocument.Parse(stream!);
        var root = document.RootElement;
        Assert.Equal(1, root.GetProperty("SchemaVersion").GetInt32());
        Assert.Equal("US-CCSS-MATH", root.GetProperty("PackCode").GetString());
        Assert.Equal("g3-u05-l04-firstedition2021-secret-fractions-pilolo-source-text-20261025-v1", root.GetProperty("ContentVersion").GetString());
        var list = root.GetProperty("Lessons");
        Assert.Equal(1, list.GetArrayLength());
        Assert.Equal("PED:US-CCSS-MATH:G3:U05:L04", list[0].GetProperty("LessonCode").GetString());
        return list[0].Clone();
    }

    [Theory]
    [InlineData(3,3,9)]
    [InlineData(7,3,21)]
    [InlineData(10,3,30)]
    [InlineData(3,17,51)]
    public void OriginalWarmupIndependentMultiplicationOracle(int a,int b,int product)
    {
        Assert.Equal(product, checked(a*b));
        Assert.Contains(product.ToString(), string.Join(" ",
            Source().GetProperty("WorkedExamples").EnumerateArray()
                .Select(x=>x.GetProperty("Answer").GetString())));
    }

    [Theory]
    [InlineData(4,8,"4/8")]
    [InlineData(2,3,"2/3")]
    [InlineData(3,4,"3/4")]
    [InlineData(5,6,"5/6")]
    public void PiloloFractionsRequireCorrectUnitIntervalCounts(int taken,int divisions,string fraction)
    {
        Assert.True(taken>0 && taken<divisions);
        Assert.Equal(fraction,$"{taken}/{divisions}");
        var examples=Source().GetProperty("WorkedExamples").EnumerateArray().ToArray();
        Assert.Contains(examples, x=>x.GetProperty("Answer").GetString()==fraction);
    }

    [Fact]
    public void VariableCardGameAndUnverifiedSourceDiagramAreNotMachineCertified()
    {
        var section=string.Join(" ",Source().GetProperty("ExplanationParagraphs")
            .EnumerateArray().Select(x=>x.GetString()!));
        Assert.Contains("two target",section,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("equal-area",section,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("cannot be fixed",section,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("have not been individually visually verified",section,StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("source-perfect",section,StringComparison.OrdinalIgnoreCase);
    }
}

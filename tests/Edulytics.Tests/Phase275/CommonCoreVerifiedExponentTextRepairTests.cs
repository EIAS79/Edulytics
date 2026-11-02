using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.Phase275;

public sealed class CommonCoreVerifiedExponentTextRepairTests
{
    private static string Hash(string s) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s))).ToLowerInvariant();

    [Fact]
    public void Grade8ExponentFixPreservesAcceptedFiveFixChainAndExactSource()
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var names = assembly.GetManifestResourceNames();
        using var baseStream = assembly.GetManifestResourceStream(
            Assert.Single(names, n => n.EndsWith("us-ccss-math.curriculum-pack.json",
                StringComparison.OrdinalIgnoreCase)))!;
        using var fiveStream = assembly.GetManifestResourceStream(
            Assert.Single(names, n => n.EndsWith("us-ccss-math.authority-text-repairs.json",
                StringComparison.OrdinalIgnoreCase)))!;
        using var addendumStream = assembly.GetManifestResourceStream(
            Assert.Single(names, n => n.EndsWith("us-ccss-math.exponent-text-repair.json",
                StringComparison.OrdinalIgnoreCase)))!;
        using var original = JsonDocument.Parse(baseStream);
        using var previous = JsonDocument.Parse(fiveStream);
        using var update = JsonDocument.Parse(addendumStream);
        var source = Assert.Single(original.RootElement.GetProperty("Nodes")
            .EnumerateArray().ToArray(), n =>
                n.GetProperty("Code").GetString() == "CCSS:8.EE.A.1");
        var patch = update.RootElement;
        Assert.Equal(5, previous.RootElement.GetProperty("replacements").GetArrayLength());
        var previousDigest = previous.RootElement.GetProperty("patchedContentDigest").GetString()!;
        Assert.Equal(previousDigest,
            patch.GetProperty("previousPatchedContentDigest").GetString());
        Assert.Equal("CCSS:8.EE.A.1", patch.GetProperty("code").GetString());
        var oldText = source.GetProperty("OfficialText").GetString()!;
        Assert.Contains("32 × 3–5", oldText, StringComparison.Ordinal);
        Assert.Equal(Hash(oldText), patch.GetProperty("expectedOldTextSha256").GetString());
        var oldHash = source.GetProperty("ContentHash").GetString()!;
        Assert.Equal(oldHash, patch.GetProperty("expectedOldContentHash").GetString());
        var corrected = patch.GetProperty("correctedOfficialText").GetString()!;
        Assert.Contains("3² × 3⁻⁵ = 3⁻³ = 1/3³ = 1/27", corrected, StringComparison.Ordinal);
        Assert.DoesNotContain("32 × 3–5", corrected, StringComparison.Ordinal);
        Assert.Equal(Hash(corrected), patch.GetProperty("correctedTextSha256").GetString());
        var code = patch.GetProperty("code").GetString()!;
        var newHash = Hash("EDULYTIKS-CCSS-TEXT-V1\n" +
                           code + "\n" + oldHash + "\n" + corrected);
        Assert.Equal(newHash, patch.GetProperty("correctedContentHash").GetString());
        var finalDigest = Hash("EDULYTIKS-CCSS-TEXT-V1\n" +
                              previousDigest + "\n" + code + ":" + newHash);
        Assert.Equal(finalDigest, patch.GetProperty("patchedContentDigest").GetString());
        Assert.StartsWith("https://www.thecorestandards.org/Math/Content/",
            patch.GetProperty("authorityUrl").GetString());
    }
}

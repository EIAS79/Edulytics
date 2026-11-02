using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.Phase275;

public sealed class CommonCoreVerifiedHighSchoolNotationRepairTests
{
    private static string Hash(string s) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(s))).ToLowerInvariant();

    [Fact]
    public void ThreeSourceVerifiedSchoolNotationFixesPreserveAllSixPredecessorCorrections()
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var names = assembly.GetManifestResourceNames();
        JsonDocument Read(string suffix)
        {
            var name = Assert.Single(names, n => n.EndsWith(suffix,
                StringComparison.OrdinalIgnoreCase));
            using var stream = assembly.GetManifestResourceStream(name)!;
            return JsonDocument.Parse(stream);
        }
        using var source = Read("us-ccss-math.curriculum-pack.json");
        using var five = Read("us-ccss-math.authority-text-repairs.json");
        using var six = Read("us-ccss-math.exponent-text-repair.json");
        using var nine = Read("us-ccss-math.hs-notation-text-repairs.json");
        var manifest = nine.RootElement;
        Assert.Equal(5, five.RootElement.GetProperty("replacements").GetArrayLength());
        Assert.Equal(five.RootElement.GetProperty("patchedContentDigest").GetString(),
            six.RootElement.GetProperty("previousPatchedContentDigest").GetString());
        var predecessor = six.RootElement.GetProperty("patchedContentDigest").GetString()!;
        Assert.Equal(predecessor,
            manifest.GetProperty("previousPatchedContentDigest").GetString());
        Assert.Equal("US-CCSS-MATH", manifest.GetProperty("packCode").GetString());
        var patches = manifest.GetProperty("replacements").EnumerateArray().ToArray();
        Assert.Equal(3, patches.Length);
        Assert.Equal(
            new[] {"CCSS:HSA-APR.C.4","CCSS:HSA-SSE.A.2","CCSS:HSN-RN.A.1"},
            patches.Select(x => x.GetProperty("code").GetString())
                .OrderBy(x => x, StringComparer.Ordinal).ToArray());
        var chained = new List<string>();
        foreach (var patch in patches)
        {
            var code = patch.GetProperty("code").GetString()!;
            var original = Assert.Single(
                source.RootElement.GetProperty("Nodes").EnumerateArray().ToArray(),
                n => n.GetProperty("Code").GetString() == code);
            var previousText = original.GetProperty("OfficialText").GetString()!;
            var correctedText = patch.GetProperty("correctedOfficialText").GetString()!;
            Assert.Equal(original.GetProperty("ContentHash").GetString(),
                patch.GetProperty("expectedOldContentHash").GetString());
            Assert.Equal(Hash(previousText),
                patch.GetProperty("expectedOldTextSha256").GetString());
            Assert.Equal(Hash(correctedText),
                patch.GetProperty("correctedTextSha256").GetString());
            var contentHash = Hash("EDULYTIKS-CCSS-TEXT-V1\n" + code + "\n" +
                original.GetProperty("ContentHash").GetString() + "\n" + correctedText);
            Assert.Equal(contentHash, patch.GetProperty("correctedContentHash").GetString());
            Assert.StartsWith("https://www.thecorestandards.org/Math/Content/",
                patch.GetProperty("authorityUrl").GetString());
            chained.Add(code + ":" + contentHash);
            switch (code)
            {
                case "CCSS:HSA-SSE.A.2":
                    Assert.Contains("x⁴ − y⁴ as (x²)² − (y²)²", correctedText);
                    Assert.Contains("(x² − y²)(x² + y²)", correctedText);
                    break;
                case "CCSS:HSA-APR.C.4":
                    Assert.Contains("(x² + y²)² = (x² − y²)² + (2xy)²", correctedText);
                    break;
                case "CCSS:HSN-RN.A.1":
                    Assert.Contains("5^(1/3)", correctedText);
                    Assert.Contains("(5^(1/3))³ = 5^((1/3)3)", correctedText);
                    break;
            }
        }
        Assert.Equal(Hash("EDULYTIKS-CCSS-TEXT-V1\n" + predecessor +
            "\n" + string.Join("\n", chained)),
            manifest.GetProperty("patchedContentDigest").GetString());
    }
}

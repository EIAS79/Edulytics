using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.Phase275;

public sealed class CommonCoreVerifiedAuthorityTextRepairTests
{
    private static readonly string[] ExpectedCodes =
    [
        "CCSS:5.G.A.1", "CCSS:5.NF.B.7",
        "CCSS:6.EE.A.2", "CCSS:7.NS.A.2"
    ];

    [Fact]
    public void RepairManifestRestoresOfficialRequirementsAndHasVerifiableHashChain()
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var embedded = assembly.GetManifestResourceNames();
        var repairResource = Assert.Single(
            embedded, x => x.EndsWith("us-ccss-math.authority-text-repairs.json",
                StringComparison.OrdinalIgnoreCase));
        var originalResource = Assert.Single(
            embedded, x => x.EndsWith("us-ccss-math.curriculum-pack.json",
                StringComparison.OrdinalIgnoreCase));

        using var repairStream = assembly.GetManifestResourceStream(repairResource)!;
        using var packStream = assembly.GetManifestResourceStream(originalResource)!;
        using var repairDocument = JsonDocument.Parse(repairStream);
        using var packDocument = JsonDocument.Parse(packStream);

        var manifest = repairDocument.RootElement;
        var original = packDocument.RootElement;
        var repairs = manifest.GetProperty("replacements").EnumerateArray().ToArray();
        var nodes = original.GetProperty("Nodes").EnumerateArray()
            .ToDictionary(x => x.GetProperty("Code").GetString()!,
                StringComparer.Ordinal);

        Assert.Equal(1, manifest.GetProperty("schemaVersion").GetInt32());
        Assert.Equal(
            original.GetProperty("ContentDigest").GetString(),
            manifest.GetProperty("baselineContentDigest").GetString());
        Assert.Equal(ExpectedCodes,
            repairs.Select(x => x.GetProperty("code").GetString()).ToArray());

        foreach (var repair in repairs)
        {
            var code = repair.GetProperty("code").GetString()!;
            var existing = nodes[code];
            var oldText = existing.GetProperty("OfficialText").GetString()!;
            var originalHash = existing.GetProperty("ContentHash").GetString()!;
            var fixedText = repair.GetProperty("correctedOfficialText").GetString()!;
            Assert.Equal(originalHash,
                repair.GetProperty("expectedOldContentHash").GetString());
            Assert.Equal(Sha(oldText),
                repair.GetProperty("expectedOldTextSha256").GetString());
            Assert.Equal(Sha(fixedText),
                repair.GetProperty("correctedTextSha256").GetString());
            Assert.Equal(Sha("EDULYTIKS-CCSS-TEXT-V1\n" + code + "\n" +
                originalHash + "\n" + fixedText),
                repair.GetProperty("correctedContentHash").GetString());
            Assert.StartsWith("https://www.thecorestandards.org/Math/Content/",
                repair.GetProperty("authorityUrl").GetString());
        }

        Assert.Contains("the 0 on each line",
            repairs[0].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("b. Interpret division of a whole number by a unit fraction",
            repairs[1].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("c. Solve real world problems involving division",
            repairs[1].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("b. Identify parts of an expression",
            repairs[2].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("c. Evaluate expressions at specific values",
            repairs[2].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("V = s³ and A = 6s²",
            repairs[2].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("c. Apply properties of operations as strategies",
            repairs[3].GetProperty("correctedOfficialText").GetString());
        Assert.Contains("d. Convert a rational number to a decimal",
            repairs[3].GetProperty("correctedOfficialText").GetString());

        var calculatedDigest = Sha(
            "EDULYTIKS-CCSS-TEXT-V1\n" +
            manifest.GetProperty("baselineContentDigest").GetString() + "\n" +
            string.Join("\n", repairs.Select(x =>
                x.GetProperty("code").GetString() + ":" +
                x.GetProperty("correctedContentHash").GetString())));
        Assert.Equal(calculatedDigest,
            manifest.GetProperty("patchedContentDigest").GetString());
    }

    private static string Sha(string text) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)))
            .ToLowerInvariant();
}
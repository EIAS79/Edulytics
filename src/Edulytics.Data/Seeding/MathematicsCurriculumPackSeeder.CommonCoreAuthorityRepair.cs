using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Seeding;

public sealed partial class MathematicsCurriculumPackSeeder
{
    // Authority-only repair. No lesson identifiers, assessment results, mastery
    // data, grades, standards links, school content, or permissions are touched.
    private static VerifiedCommonCoreAuthorityRepair LoadAuthorityRepair(Doc d)
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var matches = assembly.GetManifestResourceNames()
            .Where(x => x.EndsWith(
                "us-ccss-math.authority-text-repairs.json",
                StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (matches.Length != 1)
            throw new InvalidOperationException("Missing unique CCSS authority text repair manifest.");

        using var stream = assembly.GetManifestResourceStream(matches[0])
            ?? throw new InvalidOperationException("Cannot read CCSS authority text repair manifest.");

        var manifest = JsonSerializer.Deserialize<VerifiedCommonCoreAuthorityRepair>(
            stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Invalid CCSS authority text repair manifest.");

        if (manifest.SchemaVersion != 1 ||
            manifest.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            manifest.VersionCode != d.VersionCode ||
            manifest.BaselineContentDigest != d.ContentDigest ||
            d.SchemaVersion != 14 ||
            manifest.Replacements.Count != 4 ||
            manifest.Replacements.Select(x => x.Code).Distinct(StringComparer.Ordinal).Count() != 4)
            throw new InvalidOperationException("Unexpected CCSS authority repair baseline.");

        var byCode = d.Nodes.ToDictionary(x => x.Code, StringComparer.Ordinal);
        foreach (var patch in manifest.Replacements)
        {
            if (!byCode.TryGetValue(patch.Code, out var old) ||
                old.Kind != "Standard" ||
                !old.IsOfficial ||
                old.ContentHash != patch.ExpectedOldContentHash ||
                HashOfficialText(old.OfficialText ?? "") != patch.ExpectedOldTextSha256 ||
                HashOfficialText(patch.CorrectedOfficialText) != patch.CorrectedTextSha256 ||
                HashOfficialText(
                    "EDULYTIKS-CCSS-TEXT-V1\n" +
                    patch.Code + "\n" +
                    old.ContentHash + "\n" +
                    patch.CorrectedOfficialText) != patch.CorrectedContentHash ||
                !Uri.TryCreate(patch.AuthorityUrl, UriKind.Absolute, out var source) ||
                source.Scheme != Uri.UriSchemeHttps ||
                (source.Host != "www.thecorestandards.org" &&
                 source.Host != "thecorestandards.org"))
                throw new InvalidOperationException(
                    $"CCSS authority correction lacks accepted source integrity: {patch.Code}");
        }

        var newDigest = HashOfficialText(
            "EDULYTIKS-CCSS-TEXT-V1\n" +
            d.ContentDigest + "\n" +
            string.Join("\n",
                manifest.Replacements.Select(x => x.Code + ":" + x.CorrectedContentHash)));

        if (manifest.PatchedContentDigest != newDigest)
            throw new InvalidOperationException("CCSS authority-repaired content digest drift.");

        return manifest;
    }

    private static string HashOfficialText(string text) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text))).ToLowerInvariant();

    private static bool IsAuthorityRepairedState(
        Doc d,
        CurriculumPackImportState state,
        VerifiedCommonCoreAuthorityRepair manifest) =>
        state.FrameworkCode == d.PackCode &&
        state.VersionCode == d.VersionCode &&
        state.SourceDigest == d.SourceDigest &&
        state.ContentDigest == manifest.PatchedContentDigest &&
        state.NodeCount == d.NodeCount &&
        state.OfficialNodeCount == d.OfficialNodeCount &&
        state.UnitCount == d.UnitCount &&
        state.LessonCount == d.LessonCount &&
        state.LinkCount == d.LinkCount &&
        state.IsComplete;

    private async Task ApplyAuthorityRepairAsync(
        Doc d,
        CurriculumPackImportState state,
        Guid frameworkVersionId,
        CancellationToken ct)
    {
        if (d.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode)
            return;

        var manifest = LoadAuthorityRepair(d);
        if (!StateMatchesDocument(state, d))
            throw new InvalidOperationException("CCSS authority repair requires the exact accepted pre-repair state.");

        var codes = manifest.Replacements.Select(x => x.Code).ToArray();
        var rows = await _db.CurriculumPackContentNodes
            .Where(x => x.FrameworkVersionId == frameworkVersionId && codes.Contains(x.Code))
            .ToArrayAsync(ct);

        if (rows.Length != manifest.Replacements.Count)
            throw new InvalidOperationException("CCSS correction target count drift.");

        foreach (var patch in manifest.Replacements)
        {
            var row = rows.Single(x => x.Code == patch.Code);
            if (row.ContentHash != patch.ExpectedOldContentHash ||
                HashOfficialText(row.OfficialText ?? "") != patch.ExpectedOldTextSha256)
                throw new InvalidOperationException($"CCSS target modified unexpectedly: {patch.Code}");

            row.OfficialText = patch.CorrectedOfficialText;
            row.ContentHash = patch.CorrectedContentHash;
            row.UpdatedAtUtc = DateTime.UtcNow;
        }

        state.ContentDigest = manifest.PatchedContentDigest;
        state.ImportedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        await ValidateAuthorityRepairedRowsAsync(d, frameworkVersionId, manifest, ct);
    }

    private async Task ValidateAuthorityRepairedRowsAsync(
        Doc d,
        Guid frameworkVersionId,
        VerifiedCommonCoreAuthorityRepair manifest,
        CancellationToken ct)
    {
        var rows = await _db.CurriculumPackContentNodes.AsNoTracking()
            .Where(x => x.FrameworkVersionId == frameworkVersionId)
            .ToArrayAsync(ct);
        var links = await _db.CurriculumPackNodeLinks.AsNoTracking()
            .CountAsync(x => x.FrameworkVersionId == frameworkVersionId, ct);

        if (rows.Length != d.Nodes.Count || links != d.LinkCount)
            throw new InvalidOperationException("CCSS verified authority repair row count drift.");

        var persisted = rows.ToDictionary(x => x.Code, StringComparer.Ordinal);
        var patches = manifest.Replacements.ToDictionary(x => x.Code, StringComparer.Ordinal);

        foreach (var expected in d.Nodes)
        {
            if (!persisted.TryGetValue(expected.Code, out var row))
                throw new InvalidOperationException($"CCSS persisted node missing: {expected.Code}");
            var expectedId = G($"node|{d.PackCode}|{d.VersionCode}|{expected.Code}");
            var expectedParentId = expected.ParentCode is null ? (Guid?)null :
                G($"node|{d.PackCode}|{d.VersionCode}|{expected.ParentCode}");

            if (!PersistedNodeStaticMetadataMatches(
                    row, d, expected, expectedId, expectedParentId, frameworkVersionId))
                throw new InvalidOperationException($"CCSS authority repair static metadata drift: {expected.Code}");

            var newText = patches.TryGetValue(expected.Code, out var patch)
                ? patch.CorrectedOfficialText
                : expected.OfficialText;
            var newHash = patch is null ? expected.ContentHash : patch.CorrectedContentHash;
            if (row.OfficialText != newText || row.ContentHash != newHash)
                throw new InvalidOperationException($"CCSS authority repair text/hash drift: {expected.Code}");
        }
    }

    private sealed class VerifiedCommonCoreAuthorityRepair
    {
        public int SchemaVersion { get; set; }
        public string PackCode { get; set; } = "";
        public string VersionCode { get; set; } = "";
        public string BaselineContentDigest { get; set; } = "";
        public string PatchedContentDigest { get; set; } = "";
        public List<VerifiedCommonCoreTextPatch> Replacements { get; set; } = [];
    }

    private sealed class VerifiedCommonCoreTextPatch
    {
        public string Code { get; set; } = "";
        public string AuthorityUrl { get; set; } = "";
        public string ExpectedOldContentHash { get; set; } = "";
        public string ExpectedOldTextSha256 { get; set; } = "";
        public string CorrectedOfficialText { get; set; } = "";
        public string CorrectedTextSha256 { get; set; } = "";
        public string CorrectedContentHash { get; set; } = "";
    }
}
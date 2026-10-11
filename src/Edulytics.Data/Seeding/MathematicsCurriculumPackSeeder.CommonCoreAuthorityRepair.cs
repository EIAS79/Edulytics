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
            manifest.Replacements.Count != 5 ||
            manifest.Replacements.Select(x => x.Code).Distinct(StringComparer.Ordinal).Count() != 5)
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

    // Next immutable authority stage: five existing fixes -> Grade 8 superscript fix.
    // Preserve the previously shipped five-fix manifest/digest unchanged.
    private static VerifiedCommonCoreExponentRepair LoadExponentRepair(
        Doc d, VerifiedCommonCoreAuthorityRepair previous)
    {
        var assembly = typeof(MathematicsCurriculumPackRegistry).Assembly;
        var name = assembly.GetManifestResourceNames().SingleOrDefault(x =>
            x.EndsWith("us-ccss-math.exponent-text-repair.json",
                StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("Missing CCSS exponent authority addendum.");
        using var stream = assembly.GetManifestResourceStream(name)
            ?? throw new InvalidOperationException("Cannot read CCSS exponent authority addendum.");
        var patch = JsonSerializer.Deserialize<VerifiedCommonCoreExponentRepair>(stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Invalid CCSS exponent authority addendum.");
        var original = d.Nodes.Single(x => x.Code == "CCSS:8.EE.A.1");
        if (patch.SchemaVersion != 1 ||
            patch.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode ||
            patch.VersionCode != d.VersionCode ||
            patch.Code != original.Code ||
            patch.PreviousPatchedContentDigest != previous.PatchedContentDigest ||
            original.Kind != "Standard" || !original.IsOfficial ||
            patch.ExpectedOldContentHash != original.ContentHash ||
            patch.ExpectedOldTextSha256 != HashOfficialText(original.OfficialText ?? "") ||
            patch.CorrectedTextSha256 != HashOfficialText(patch.CorrectedOfficialText) ||
            patch.CorrectedContentHash != HashOfficialText(
                "EDULYTIKS-CCSS-TEXT-V1\n" + patch.Code + "\n" +
                original.ContentHash + "\n" + patch.CorrectedOfficialText) ||
            patch.PatchedContentDigest != HashOfficialText(
                "EDULYTIKS-CCSS-TEXT-V1\n" +
                patch.PreviousPatchedContentDigest + "\n" +
                patch.Code + ":" + patch.CorrectedContentHash) ||
            !Uri.TryCreate(patch.AuthorityUrl, UriKind.Absolute, out var url) ||
            url.Scheme != Uri.UriSchemeHttps ||
            (url.Host != "thecorestandards.org" &&
             url.Host != "www.thecorestandards.org") ||
            !patch.CorrectedOfficialText.Contains(
                "3² × 3⁻⁵ = 3⁻³ = 1/3³ = 1/27", StringComparison.Ordinal))
            throw new InvalidOperationException("CCSS exponent patch identity/hash/authority drift.");
        return patch;
    }

    private static bool IsExponentRepairedState(
        Doc d, CurriculumPackImportState state,
        VerifiedCommonCoreExponentRepair exponent) =>
        state.FrameworkCode == d.PackCode &&
        state.VersionCode == d.VersionCode &&
        state.SourceDigest == d.SourceDigest &&
        state.ContentDigest == exponent.PatchedContentDigest &&
        state.NodeCount == d.NodeCount &&
        state.OfficialNodeCount == d.OfficialNodeCount &&
        state.UnitCount == d.UnitCount &&
        state.LessonCount == d.LessonCount &&
        state.LinkCount == d.LinkCount &&
        state.IsComplete;

    private async Task ApplyExponentRepairAsync(
        Doc d, CurriculumPackImportState state, Guid frameworkVersionId,
        CancellationToken ct)
    {
        if (d.PackCode != MathematicsCurriculumPackRegistry.CommonCoreCode)
            return;
        var previous = LoadAuthorityRepair(d);
        var patch = LoadExponentRepair(d, previous);
        // Refuse altered/unknown states: exact predecessor digest only.
        if (!IsAuthorityRepairedState(d, state, previous))
            throw new InvalidOperationException("CCSS exponent transition requires exact five-fix predecessor.");
        await ValidateAuthorityRepairedRowsAsync(d, frameworkVersionId, previous, ct);

        var row = await _db.CurriculumPackContentNodes.SingleAsync(
            x => x.FrameworkVersionId == frameworkVersionId && x.Code == patch.Code, ct);
        if (row.ContentHash != patch.ExpectedOldContentHash ||
            HashOfficialText(row.OfficialText ?? "") != patch.ExpectedOldTextSha256)
            throw new InvalidOperationException("CCSS exponent row is not in its proven predecessor state.");
        row.OfficialText = patch.CorrectedOfficialText;
        row.ContentHash = patch.CorrectedContentHash;
        row.UpdatedAtUtc = DateTime.UtcNow;
        state.ContentDigest = patch.PatchedContentDigest;
        state.ImportedAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        await ValidateAuthorityRepairedRowsAsync(
            d, frameworkVersionId, previous, ct, exponentPatched: true);
    }

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
        CancellationToken ct,
        bool exponentPatched = false)
    {
        var exponent = exponentPatched ? LoadExponentRepair(d, manifest) : null;
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
            if (exponent is not null && expected.Code == exponent.Code)
            {
                newText = exponent.CorrectedOfficialText;
                newHash = exponent.CorrectedContentHash;
            }
            if (row.OfficialText != newText || row.ContentHash != newHash)
                throw new InvalidOperationException($"CCSS authority repair text/hash drift: {expected.Code}");
        }
    }

    private sealed class VerifiedCommonCoreExponentRepair
    {
        public int SchemaVersion { get; set; }
        public string PackCode { get; set; } = "";
        public string VersionCode { get; set; } = "";
        public string PreviousPatchedContentDigest { get; set; } = "";
        public string PatchedContentDigest { get; set; } = "";
        public string Code { get; set; } = "";
        public string AuthorityUrl { get; set; } = "";
        public string ExpectedOldContentHash { get; set; } = "";
        public string ExpectedOldTextSha256 { get; set; } = "";
        public string CorrectedOfficialText { get; set; } = "";
        public string CorrectedTextSha256 { get; set; } = "";
        public string CorrectedContentHash { get; set; } = "";
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
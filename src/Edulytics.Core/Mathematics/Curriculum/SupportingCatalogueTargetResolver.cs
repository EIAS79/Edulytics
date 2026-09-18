using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Curriculum;

public sealed record SupportingCatalogueTarget(
    string TargetId,
    string Domain,
    string NormalizedTarget);

/// <summary>
/// Deterministic identity resolver for source-backed Supporting lesson targets.
/// This closes catalogue ontology gaps only; it never authorizes learner-facing
/// Practice or synthesizes an official curriculum outcome.
/// </summary>
public static class SupportingCatalogueTargetResolver
{
    private const string ResourceSuffix =
        "Mathematics.Curriculum.supporting-target-domain-rules.v1.json";

    private static readonly Lazy<RulesDocument> Rules =
        new(LoadRules, LazyThreadSafetyMode.ExecutionAndPublication);

    public static SupportingCatalogueTarget? Resolve(string? title)
    {
        var normalized = NormalizeTarget(title);
        if (string.IsNullOrWhiteSpace(normalized))
            return null;

        var domain = InferDomain(normalized);
        var slug = Slugify(normalized);
        var digest = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(normalized)))
            .ToLowerInvariant()[..10];

        return new SupportingCatalogueTarget(
            $"supporting.{domain}.{slug}.{digest}",
            domain,
            normalized);
    }

    public static string NormalizeTarget(string? title)
    {
        var value = Regex.Replace(
                title ?? string.Empty,
                @"\s+",
                " ",
                RegexOptions.CultureInvariant)
            .Trim()
            .ToLowerInvariant();

        foreach (var pattern in Rules.Value.StripPatterns)
        {
            value = Regex.Replace(
                    value,
                    pattern,
                    string.Empty,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)
                .Trim();
        }

        value = Regex.Replace(
                value,
                @"\s+",
                " ",
                RegexOptions.CultureInvariant)
            .Trim(' ', '.', ':', '-', '—');

        return value;
    }

    private static string InferDomain(string normalized)
    {
        foreach (var rule in Rules.Value.DomainRules)
        {
            foreach (var pattern in rule.Patterns)
            {
                if (Regex.IsMatch(
                    normalized,
                    pattern,
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    return rule.Domain;
                }
            }
        }

        return Rules.Value.FallbackDomain;
    }

    private static string Slugify(string target)
    {
        var slug = Regex.Replace(
                target,
                @"[^a-z0-9]+",
                "_",
                RegexOptions.CultureInvariant)
            .Trim('_');

        slug = Regex.Replace(slug, @"_+", "_", RegexOptions.CultureInvariant);
        if (slug.Length == 0)
            slug = "target";

        if (slug.Length > 72)
            slug = slug[..72].TrimEnd('_');

        return slug;
    }

    private static RulesDocument LoadRules()
    {
        var assembly = typeof(SupportingCatalogueTargetResolver).Assembly;
        var resourceName = assembly
            .GetManifestResourceNames()
            .SingleOrDefault(x => x.EndsWith(ResourceSuffix, StringComparison.Ordinal))
            ?? throw new InvalidOperationException(
                "Supporting target ontology rules are not embedded.");

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                "Supporting target ontology rules resource cannot be opened.");

        return JsonSerializer.Deserialize<RulesDocument>(
                   stream,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true
                   })
               ?? throw new InvalidOperationException(
                   "Supporting target ontology rules are invalid.");
    }

    private sealed record RulesDocument(
        IReadOnlyList<string> StripPatterns,
        IReadOnlyList<DomainRule> DomainRules,
        string FallbackDomain);

    private sealed record DomainRule(
        string Domain,
        IReadOnlyList<string> Patterns);
}

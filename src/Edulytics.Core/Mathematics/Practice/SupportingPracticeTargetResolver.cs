using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record SupportingPracticeTargetSpec(
    string RuleId,
    string Profile,
    string NormalizedTitle,
    string SkillId,
    string FamilyId,
    string Mechanic);

/// <summary>
/// Single authority for generated target-specific Supporting Practice contracts.
/// Existing explicit READY_VERIFIED lesson mappings remain authoritative.
/// Every generated family remains unique to one canonical lesson target; the
/// shared rule id controls reusable deterministic mathematics implementation.
/// </summary>
public static class SupportingPracticeTargetResolver
{
    private const string RuleResource =
        "Edulytics.Core.Mathematics.Curriculum.supporting-practice-target-rules.v1.json";
    private const string MappingResource =
        "Edulytics.Core.Mathematics.Curriculum.lesson-skill-mappings.v1.json";

    private static readonly Lazy<State> Cached = new(LoadState);

    public static bool TryResolve(string? title, out SupportingPracticeTargetSpec? spec)
    {
        var normalized = NormalizeTitle(title);
        if (normalized.Length == 0)
        {
            spec = null;
            return false;
        }

        foreach (var rule in Cached.Value.Rules)
        {
            if (!rule.Patterns.Any(pattern => pattern.IsMatch(normalized)))
                continue;

            var digest = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(normalized.ToLowerInvariant())))
                .ToLowerInvariant()[..12];
            var skillId = $"supporting.skill.{rule.Id}.{digest}";
            var familyId = $"supporting.target.{rule.Id}.{digest}";
            spec = new SupportingPracticeTargetSpec(
                rule.Id,
                rule.Profile,
                normalized,
                skillId,
                familyId,
                $"SUPPORTING_{rule.Profile.ToUpperInvariant()}");
            return true;
        }

        spec = null;
        return false;
    }

    public static bool IsGeneratedFamily(string? family) =>
        !string.IsNullOrWhiteSpace(family) &&
        family.StartsWith("supporting.target.", StringComparison.Ordinal) &&
        TryGetRuleIdFromFamily(family, out _);

    public static bool TryGetRuleIdFromFamily(string family, out string ruleId)
    {
        ruleId = string.Empty;
        if (string.IsNullOrWhiteSpace(family) ||
            !family.StartsWith("supporting.target.", StringComparison.Ordinal))
        {
            return false;
        }

        var remainder = family["supporting.target.".Length..];
        var lastDot = remainder.LastIndexOf('.');
        if (lastDot <= 0 || lastDot == remainder.Length - 1)
            return false;

        var candidate = remainder[..lastDot];
        if (!Cached.Value.RuleIds.Contains(candidate))
            return false;

        var digest = remainder[(lastDot + 1)..];
        if (digest.Length != 12 || digest.Any(ch => !Uri.IsHexDigit(ch)))
            return false;

        ruleId = candidate;
        return true;
    }

    public static bool TryGetProfileForFamily(string family, out string profile)
    {
        profile = string.Empty;
        if (!TryGetRuleIdFromFamily(family, out var ruleId))
            return false;

        var rule = Cached.Value.RulesById[ruleId];
        profile = rule.Profile;
        return true;
    }

    public static bool IsExplicitReadyLesson(string? lessonCode) =>
        !string.IsNullOrWhiteSpace(lessonCode) &&
        Cached.Value.ExplicitReadyLessonCodes.Contains(lessonCode.Trim());

    public static IReadOnlyCollection<string> RuleIds => Cached.Value.RuleIds;

    public static string NormalizeTitle(string? title)
    {
        var value = Regex.Replace(title ?? string.Empty, @"\s+", " ").Trim();
        value = Regex.Replace(
            value,
            @"\s*[—-]\s*advanced reasoning\s*$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        value = Regex.Replace(
            value,
            @":\s*(?:build the idea|reason and apply)\s*$",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        value = Regex.Replace(
            value,
            @"^\s*consolidating\s+",
            string.Empty,
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        return Regex.Replace(value, @"\s+", " ").Trim();
    }

    private static State LoadState()
    {
        using var rulesDocument = LoadDocument(RuleResource)
            ?? throw new InvalidOperationException("Supporting Practice target rules are missing.");
        using var mappingsDocument = LoadDocument(MappingResource)
            ?? throw new InvalidOperationException("Lesson-skill mappings are missing.");

        var rules = new List<Rule>();
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var row in rulesDocument.RootElement.GetProperty("rules").EnumerateArray())
        {
            var id = row.GetProperty("id").GetString()?.Trim() ?? string.Empty;
            var profile = row.GetProperty("profile").GetString()?.Trim() ?? string.Empty;
            if (id.Length == 0 || profile.Length == 0 || !ids.Add(id))
                throw new InvalidOperationException("Supporting Practice target rule is invalid or duplicated.");

            var patterns = row.GetProperty("titlePatterns")
                .EnumerateArray()
                .Select(value => value.GetString())
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => new Regex(
                    value!,
                    RegexOptions.IgnoreCase |
                    RegexOptions.CultureInvariant |
                    RegexOptions.Compiled))
                .ToArray();
            if (patterns.Length == 0)
                throw new InvalidOperationException($"Supporting Practice target rule {id} has no title pattern.");

            rules.Add(new Rule(id, profile, patterns));
        }

        var explicitReady = new HashSet<string>(StringComparer.Ordinal);
        foreach (var row in mappingsDocument.RootElement.GetProperty("mappings").EnumerateArray())
        {
            if (!row.TryGetProperty("lessonCode", out var codeNode) ||
                !row.TryGetProperty("practiceReadiness", out var readinessNode) ||
                !string.Equals(
                    readinessNode.GetString(),
                    "READY_VERIFIED",
                    StringComparison.Ordinal))
            {
                continue;
            }

            var code = codeNode.GetString()?.Trim();
            if (!string.IsNullOrWhiteSpace(code))
                explicitReady.Add(code);
        }

        return new State(
            rules,
            rules.ToDictionary(rule => rule.Id, StringComparer.Ordinal),
            ids,
            explicitReady);
    }

    private static JsonDocument? LoadDocument(string logicalName)
    {
        var stream = typeof(SupportingPracticeTargetResolver)
            .Assembly
            .GetManifestResourceStream(logicalName);
        if (stream is null)
            return null;

        using (stream)
        {
            return JsonDocument.Parse(stream);
        }
    }

    private sealed record Rule(
        string Id,
        string Profile,
        IReadOnlyList<Regex> Patterns);

    private sealed record State(
        IReadOnlyList<Rule> Rules,
        IReadOnlyDictionary<string, Rule> RulesById,
        IReadOnlySet<string> RuleIds,
        IReadOnlySet<string> ExplicitReadyLessonCodes);
}

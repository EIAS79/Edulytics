using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Learner-meaningful identity for a generated Practice question.
///
/// Exposure fingerprints intentionally include every reconstructable generation
/// parameter. Semantic identity is different: it excludes parameters whose only
/// purpose is to select wording/template variation. This allows the assessment
/// composer to distinguish mathematical variety from synthetic generation
/// variety.
/// </summary>
public sealed record PracticeSemanticQuestionIdentity(
    string Family,
    string Key);

public static class PracticeSemanticQuestionIdentityPolicy
{
    private static readonly IReadOnlySet<string> NonSemanticParameterNames =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "variant"
        };

    public static PracticeSemanticQuestionIdentity Create(
        string family,
        IReadOnlyDictionary<string, int> parameters)
    {
        if (string.IsNullOrWhiteSpace(family))
            throw new ArgumentException(
                "Semantic Practice identity requires a question family.",
                nameof(family));

        ArgumentNullException.ThrowIfNull(parameters);

        var canonicalParameters = string.Join(
            ";",
            parameters
                .Where(pair =>
                    !NonSemanticParameterNames.Contains(pair.Key))
                .OrderBy(pair => pair.Key, StringComparer.Ordinal)
                .Select(pair =>
                    $"{pair.Key}={pair.Value.ToString(CultureInfo.InvariantCulture)}"));

        var normalizedFamily = family.Trim();
        var key = string.IsNullOrEmpty(canonicalParameters)
            ? normalizedFamily + "|constant"
            : normalizedFamily + "|" + canonicalParameters;

        return new PracticeSemanticQuestionIdentity(
            normalizedFamily,
            key);
    }

    /// <summary>
    /// Phase 0/1 runtime enforcement remains deliberately narrow until the
    /// assessment composer owns session-level selection. Enabling low-level
    /// rejection globally before composition would turn legacy synthetic
    /// capacity into generation failures for narrow families.
    /// </summary>
    public static bool EnforceInsideExactGenerator(string family) =>
        string.Equals(
            family?.Trim(),
            "supporting.geometry.shape_dimension",
            StringComparison.Ordinal);
}

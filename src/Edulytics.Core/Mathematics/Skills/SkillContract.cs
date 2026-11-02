using Edulytics.Core.Mathematics.Identifiers;

namespace Edulytics.Core.Mathematics.Skills;

/// <summary>
/// Curriculum-neutral contract describing one exact mathematical skill and the
/// capabilities/question families that are allowed to serve it.
/// </summary>
public sealed record SkillContract
{
    public SkillContract(
        SkillId id,
        string domain,
        string canonicalName,
        IReadOnlyList<SkillId>? prerequisites = null,
        IReadOnlyList<CapabilityId>? requiredCapabilities = null,
        IReadOnlyList<string>? allowedQuestionFamilies = null,
        IReadOnlyList<string>? expectedAnswerTypes = null,
        IReadOnlyList<string>? defaultRepresentations = null,
        int version = 1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(domain);
        ArgumentException.ThrowIfNullOrWhiteSpace(canonicalName);
        if (version < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(version));
        }

        Id = id;
        Domain = domain.Trim().ToLowerInvariant();
        CanonicalName = canonicalName.Trim();
        Prerequisites = Distinct(prerequisites);
        RequiredCapabilities = Distinct(requiredCapabilities);
        AllowedQuestionFamilies = DistinctStrings(allowedQuestionFamilies);
        ExpectedAnswerTypes = DistinctStrings(expectedAnswerTypes);
        DefaultRepresentations = DistinctStrings(defaultRepresentations);
        Version = version;

        if (Prerequisites.Contains(Id))
        {
            throw new ArgumentException("A Mathematics skill cannot depend on itself.", nameof(prerequisites));
        }
    }

    public SkillId Id { get; }
    public string Domain { get; }
    public string CanonicalName { get; }
    public IReadOnlyList<SkillId> Prerequisites { get; }
    public IReadOnlyList<CapabilityId> RequiredCapabilities { get; }
    public IReadOnlyList<string> AllowedQuestionFamilies { get; }
    public IReadOnlyList<string> ExpectedAnswerTypes { get; }
    public IReadOnlyList<string> DefaultRepresentations { get; }
    public int Version { get; }

    private static IReadOnlyList<T> Distinct<T>(IReadOnlyList<T>? values)
        where T : notnull =>
        values is null
            ? []
            : values.Distinct().ToArray();

    private static IReadOnlyList<string> DistinctStrings(IReadOnlyList<string>? values) =>
        values is null
            ? []
            : values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
}

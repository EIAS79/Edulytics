using Edulytics.Core.Mathematics.Identifiers;

namespace Edulytics.Core.Mathematics.Generation;

/// <summary>
/// Declarative contract for one reusable question family. It declares exactly
/// which skill it measures and the capabilities/verifier policy required.
/// </summary>
public sealed record QuestionFamilyManifest
{
    public QuestionFamilyManifest(
        string id,
        SkillId skill,
        IReadOnlyList<CapabilityId> requiredCapabilities,
        string answerType,
        string verificationPolicy,
        IReadOnlyList<string>? representations = null,
        IReadOnlyList<string>? misconceptionIds = null,
        int version = 1)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(answerType);
        ArgumentException.ThrowIfNullOrWhiteSpace(verificationPolicy);
        if (version < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(version));
        }

        Id = id.Trim().ToLowerInvariant();
        Skill = skill;
        RequiredCapabilities = requiredCapabilities.Distinct().ToArray();
        AnswerType = answerType.Trim();
        VerificationPolicy = verificationPolicy.Trim();
        Representations = Clean(representations);
        MisconceptionIds = Clean(misconceptionIds);
        Version = version;
    }

    public string Id { get; }
    public SkillId Skill { get; }
    public IReadOnlyList<CapabilityId> RequiredCapabilities { get; }
    public string AnswerType { get; }
    public string VerificationPolicy { get; }
    public IReadOnlyList<string> Representations { get; }
    public IReadOnlyList<string> MisconceptionIds { get; }
    public int Version { get; }

    private static IReadOnlyList<string> Clean(IReadOnlyList<string>? values) =>
        values is null
            ? []
            : values
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Distinct(StringComparer.Ordinal)
                .ToArray();
}

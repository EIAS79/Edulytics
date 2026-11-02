namespace Edulytics.Core.Mathematics.Identifiers;

/// <summary>
/// Curriculum-neutral identifier for one exact teachable Mathematics skill.
/// A SkillId describes mathematical meaning; it is never an official curriculum code.
/// </summary>
public readonly record struct SkillId
{
    public SkillId(string value)
    {
        Value = CanonicalIdentifier.Normalize(value, nameof(value));
    }

    public string Value { get; }

    public static SkillId Parse(string value) => new(value);

    public override string ToString() => Value;
}

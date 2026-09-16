namespace Edulytics.Core.Mathematics.Identifiers;

/// <summary>
/// Identifier for a reusable Mathematics engine capability, such as
/// rational.compare or equation.solve.quadratic.
/// </summary>
public readonly record struct CapabilityId
{
    public CapabilityId(string value)
    {
        Value = CanonicalIdentifier.Normalize(value, nameof(value));
    }

    public string Value { get; }

    public static CapabilityId Parse(string value) => new(value);

    public override string ToString() => Value;
}

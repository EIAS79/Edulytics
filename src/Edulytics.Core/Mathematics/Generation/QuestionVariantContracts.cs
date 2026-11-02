namespace Edulytics.Core.Mathematics.Generation;

/// <summary>
/// Stable learner-facing variation inside one mathematical question family.
/// A family owns the mathematical structure; a variant owns a distinct
/// formulation/representation of that same structure.
/// </summary>
public sealed record QuestionVariantDescriptor(
    string Id,
    int Slot);

public static class QuestionVariantPolicy
{
    public const int MaximumVariantsPerFamily = 16;
    public const int MinimumUsefulVariants = 4;
    public const int PreferredVariants = 8;

    public static int NormalizeSlot(int value)
    {
        var normalized = value % MaximumVariantsPerFamily;
        return normalized < 0
            ? normalized + MaximumVariantsPerFamily
            : normalized;
    }

    public static string IdForSlot(int value) =>
        $"v{NormalizeSlot(value) + 1:00}";

    public static QuestionVariantDescriptor Describe(
        IReadOnlyDictionary<string, int> parameters,
        int fallbackSlot)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        if (parameters.TryGetValue("mode", out var mode))
            return new QuestionVariantDescriptor(
                IdForSlot(mode),
                NormalizeSlot(mode));

        if (parameters.TryGetValue("variant", out var variant))
            return new QuestionVariantDescriptor(
                IdForSlot(variant),
                NormalizeSlot(variant));

        // A family without an explicit semantic mode/variant has one verified
        // learner-facing form today. Do not invent a variant identity merely
        // because the scheduler requested another slot.
        return new QuestionVariantDescriptor(
            IdForSlot(0),
            0);
    }
}

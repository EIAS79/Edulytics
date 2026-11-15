using System.Collections.ObjectModel;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Finite mathematically distinct UAE Grade6 question-model capacities.
/// These are genuine source-scoped educational scenario counts, not an
/// instruction to repeat the same question with cosmetic word changes.
/// Adaptive V2 must switch to another contract-approved family at exhaustion;
/// a 30-item fresh same-family claim is impossible for these finite models.
/// </summary>
public static class UaeG6FinitePracticeFamilyCapacity
{
    public static IReadOnlyDictionary<string,int> FamilyCapacities { get; } =
        new ReadOnlyDictionary<string,int>(
            new Dictionary<string,int>(StringComparer.Ordinal)
            {
                ["uae.g6.u06.l02.ten_power"] = 9,
                ["uae.g6.u06.l03.first_operation"] = 4,
                ["uae.g6.u06.l05.operation_type"] = 4,
                ["uae.g6.u06.l05.repeated"] = 5,
                ["uae.g6.u06.l06.inches_to_centimeters"] = 1,
                ["uae.g6.u06.l06.percentage_markup"] = 4,
                ["uae.g6.u07.l03.identify_subtraction_property"] = 2,
                ["uae.g6.u07.l05.addition_property"] = 2,
                ["uae.g6.u07.l08.division_property"] = 2,
                ["uae.g6.u07.l10.multiplication_property"] = 2,
                ["uae.g6.u08.l01.classify_variable"] = 2,
                ["uae.g6.u08.l02.classify"] = 2,
                ["uae.g6.u08.l03.linear_vs_square"] = 2,
                ["uae.g6.u09.l01.area_conservation"] = 2,
                ["uae.g6.u10.l07.net_faces"] = 3,
            });

    public static int CertifiedBudget(string family, int requestedMaximum)
    {
        if (requestedMaximum < 1) throw new ArgumentOutOfRangeException(
            nameof(requestedMaximum));
        return FamilyCapacities.TryGetValue(family, out var verified)
            ? Math.Min(verified, requestedMaximum)
            : requestedMaximum;
    }
}

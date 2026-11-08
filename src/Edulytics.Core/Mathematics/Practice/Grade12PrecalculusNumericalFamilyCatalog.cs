namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Independent Grade12 family to source-section registry, shared by
/// Core contracts and the Service-only numerical implementation.</summary>
public static class Grade12PrecalculusNumericalFamilyCatalog
{
    public const string Prefix = "g12.precalculus.";

    private static readonly IReadOnlyDictionary<string, string> Suffixes =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["u03.l04.polynomial_evaluate"] = "U03:L04",
            ["u03.l07.rational_asymptote"] = "U03:L07",
            ["u03.l09.direct_variation"] = "U03:L09",
            ["u05.l01.radians_to_degrees"] = "U05:L01",
            ["u05.l02.unit_circle_coordinate"] = "U05:L02",
            ["u05.l03.trigonometric_ratio"] = "U05:L03",
            ["u06.l01.sine_wave_maximum"] = "U06:L01",
            ["u06.l02.tangent_period"] = "U06:L02",
            ["u06.l03.inverse_sine_principal"] = "U06:L03",
            ["u07.l01.pythagorean_identity"] = "U07:L01",
            ["u07.l02.angle_sum_sine"] = "U07:L02",
            ["u07.l05.trig_equation_solution_count"] = "U07:L05",
            ["u07.l06.trig_model_amplitude"] = "U07:L06",
            ["u08.l01.law_of_sines"] = "U08:L01",
            ["u08.l02.law_of_cosines"] = "U08:L02",
            ["u08.l03.polar_cartesian"] = "U08:L03",
            ["u08.l04.polar_curve_evaluate"] = "U08:L04",
            ["u08.l06.parametric_point"] = "U08:L06",
            ["u08.l07.parametric_slope"] = "U08:L07",
            ["u09.l04.partial_fraction_coefficient"] = "U09:L04",
            ["u09.l07.matrix_determinant"] = "U09:L07",
            ["u10.l01.ellipse_major_axis"] = "U10:L01",
            ["u10.l02.hyperbola_transverse_axis"] = "U10:L02",
            ["u10.l03.parabola_focus"] = "U10:L03",
            ["u10.l05.polar_conic_radius"] = "U10:L05",
            ["u11.l04.arithmetic_series_sum"] = "U11:L04",
            ["u11.l05.permutations"] = "U11:L05",
            ["u11.l06.binomial_coefficient"] = "U11:L06",
            ["u12.l01.polynomial_limit"] = "U12:L01",
            ["u12.l02.removable_limit"] = "U12:L02",
            ["u12.l03.continuity_matching"] = "U12:L03"
        };

    public static IReadOnlyDictionary<string, string> All => Suffixes;

    public static bool Supports(string? family) =>
        family is not null &&
        family.StartsWith(Prefix, StringComparison.Ordinal) &&
        Suffixes.ContainsKey(family[Prefix.Length..]);

}

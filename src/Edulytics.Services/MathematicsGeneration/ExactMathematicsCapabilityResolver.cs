namespace Edulytics.Services.MathematicsGeneration;

/// <summary>
/// Curriculum-neutral exact capability resolver shared by Teacher Assessment Builder
/// and Student Private Practice. Resolution is deliberately conservative: only
/// reviewed semantic targets with solver/verifier-backed exact families are emitted.
/// </summary>
public sealed record ExactMathematicsCapabilityResolution(
    string SkillId,
    IReadOnlyList<string> QuestionFamilies,
    string ReasonCode);

public static class ExactMathematicsCapabilityResolver
{
    public static ExactMathematicsCapabilityResolution? Resolve(string? semanticContext)
    {
        if (string.IsNullOrWhiteSpace(semanticContext))
            return null;

        var text = semanticContext.Trim().ToUpperInvariant();

        // Core exact arithmetic / representation targets.
        if (ContainsAny(text, "LEAST COMMON MULTIPLE", "LEAST COMMON MULTIPLES", "LCM"))
        {
            return Exact(
                "number.lcm",
                ["number.lcm.two_numbers"],
                "ExactLeastCommonMultiple");
        }

        if (ContainsAny(text, "PERCENTAGE OF A QUANTITY", "PERCENTAGES OF QUANTITIES", "FIND A PERCENTAGE"))
        {
            return Exact(
                "percentages.of_quantity",
                ["percentages.of_quantity.direct"],
                "ExactPercentageOfQuantity");
        }

        if (ContainsAny(text, "FRACTION REPRESENTATION", "FRACTION BAR", "REPRESENT FRACTIONS"))
        {
            return Exact(
                "fractions.represent.interpret",
                ["fractions.represent.interpret.fraction_bar"],
                "ExactFractionRepresentation");
        }

        // Exact algebra
        if (ContainsAny(text, "LINEAR INEQUALITY", "LINEAR INEQUALITIES") ||
            (text.Contains("INEQUALIT", StringComparison.Ordinal) &&
             !ContainsAny(text, "QUADRATIC", "SYSTEM", "SIMULTANEOUS")))
        {
            return Exact(
                "algebra.linear.inequality.solve",
                ["algebra.linear.inequality.ax_plus_b_relation_c"],
                "ExactLinearInequality");
        }

        if (ContainsAny(text, "LINEAR EQUATION", "SOLVE EQUATIONS") &&
            !ContainsAny(text, "SIMULTANEOUS", "SYSTEM", "QUADRATIC"))
        {
            return Exact(
                "algebra.linear.solve",
                ["algebra.linear.ax_plus_b_equals_c"],
                "ExactLinearEquation");
        }

        // Exact vector targets. Keep these specific so a broad lesson that merely
        // mentions vectors does not authorize the wrong operation.
        if (ContainsAny(text, "MAGNITUDE OF A VECTOR", "VECTOR MAGNITUDE", "MAGNITUDE OF VECTOR"))
        {
            return Exact(
                "vectors.magnitude",
                ["vectors.magnitude.exact"],
                "ExactVectorMagnitude");
        }

        if (ContainsAny(text, "VECTOR BETWEEN TWO POINTS", "VECTOR FROM A TO B", "POSITION VECTOR BETWEEN"))
        {
            return Exact(
                "vectors.between_points",
                ["vectors.between_points.exact"],
                "ExactVectorBetweenPoints");
        }

        if (ContainsAny(text, "SCALAR MULTIPLICATION OF VECTORS", "MULTIPLY A VECTOR BY A SCALAR", "SCALAR MULTIPLICATION"))
        {
            return Exact(
                "vectors.scalar_multiply",
                ["vectors.scalar_multiply.exact_rational"],
                "ExactVectorScalarMultiply");
        }

        if (ContainsAny(text, "VECTOR SUBTRACTION", "SUBTRACT VECTORS", "DIFFERENCE OF VECTORS"))
        {
            return Exact(
                "vectors.subtract",
                ["vectors.subtract.exact_rational"],
                "ExactVectorSubtract");
        }

        if (ContainsAny(text, "DOT PRODUCT", "SCALAR PRODUCT OF VECTORS"))
        {
            return Exact(
                "vectors.dot.exact",
                ["vectors.dot.exact_rational"],
                "ExactVectorDotProduct");
        }

        if (ContainsAny(text, "VECTOR ADDITION", "ADD VECTORS", "SUM OF VECTORS"))
        {
            return Exact(
                "vectors.add.exact",
                ["vectors.add.exact_rational"],
                "ExactVectorAdd");
        }

        // High-school geometry / trigonometry. These are intentionally checked
        // before generic geometry wording so a trig target never collapses to
        // Pythagoras or rectangle arithmetic.
        if (ContainsAny(
                text,
                "SINE COSINE AND TANGENT",
                "TRIGONOMETRIC RATIOS",
                "RIGHT TRIANGLE TRIGONOMETRY",
                "SIN COS TAN"))
        {
            return Exact(
                "trigonometry.right_triangle.sin_cos_tan",
                [
                    "trigonometry.right_triangle.ratio_exact",
                    "trigonometry.right_triangle.find_side_exact",
                    "trigonometry.right_triangle.find_angle_exact"
                ],
                "ExactRightTriangleTrigonometry");
        }

        if (text.Contains("PYTHAGOR", StringComparison.Ordinal))
        {
            return Exact(
                "geometry.right_triangle.pythagorean",
                ["geometry.right_triangle.pythagorean.exact"],
                "ExactPythagorean");
        }

        if (ContainsAny(text, "CONGRUENCE AND SIMILARITY", "SIMILARITY"))
        {
            return Exact(
                "geometry.similarity",
                [
                    "geometry.similarity.find_missing_length",
                    "geometry.similarity.scale_factor"
                ],
                "ExactSimilarity");
        }

        if (ContainsAny(text, "CONGRUENCE", "CONGRUENT"))
        {
            return Exact(
                "geometry.congruence",
                ["geometry.congruence.identify_criterion"],
                "ExactCongruence");
        }

        if (ContainsAny(text, "SURFACE AREA AND VOLUME"))
        {
            return Exact(
                "geometry.surface_area_volume",
                [
                    "geometry.surface_area_volume.rectangular_prism_surface_area",
                    "geometry.surface_area_volume.rectangular_prism_volume"
                ],
                "ExactSurfaceAreaVolume");
        }

        if (text.Contains("SURFACE AREA", StringComparison.Ordinal))
        {
            return Exact(
                "geometry.surface_area",
                ["geometry.surface_area.rectangular_prism"],
                "ExactSurfaceArea");
        }

        if (text.Contains("VOLUME", StringComparison.Ordinal) &&
            !ContainsAny(text, "VOLUME OF REVOLUTION", "INTEGRATION"))
        {
            return Exact(
                "geometry.volume",
                ["geometry.volume.rectangular_prism"],
                "ExactVolume");
        }

        if (ContainsAny(
                text,
                "COORDINATES AND STRAIGHT-LINE GRAPHS",
                "STRAIGHT-LINE GRAPHS",
                "STRAIGHT LINE GRAPHS",
                "GRADIENT",
                "SLOPE"))
        {
            return Exact(
                "geometry.coordinate.straight_line",
                ["geometry.coordinate.gradient_between_points"],
                "ExactCoordinateGeometry");
        }

        if (ContainsAny(text, "ANGLE RELATIONSHIPS", "ANGLE FACTS"))
        {
            return Exact(
                "geometry.angles.relationships",
                [
                    "geometry.angles.parallel_lines",
                    "geometry.angles.supplementary"
                ],
                "ExactAngleRelationships");
        }

        if (ContainsAny(text, "PERIMETER AND AREA", "RECTANGLE AREA", "RECTANGLE PERIMETER"))
        {
            return Exact(
                "geometry.perimeter_area",
                [
                    "geometry.perimeter_area.rectangle_area",
                    "geometry.perimeter_area.rectangle_perimeter"
                ],
                "ExactPerimeterArea");
        }

        return null;
    }

    private static ExactMathematicsCapabilityResolution Exact(
        string skillId,
        IReadOnlyList<string> families,
        string reasonCode) =>
        new(skillId, families, reasonCode);

    private static bool ContainsAny(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));
}

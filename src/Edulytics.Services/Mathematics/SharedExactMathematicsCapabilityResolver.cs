using System.Text.RegularExpressions;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.Mathematics;

public sealed record SharedExactMathematicsCapability(
    string SkillId,
    IReadOnlyList<string> AllowedQuestionFamilies,
    string ReasonCode,
    int? CurriculumLogicalLevel = null);

/// <summary>
/// Shared exact-capability authority used by Supporting lesson Practice and the
/// universal Teacher/Student Mathematics generation surfaces. Lesson-scoped
/// capability is always explicit-contract driven. Curriculum-context resolution
/// is intentionally conservative and requires a specific reviewed mathematical
/// target phrase; broad words such as "geometry" never grant exact capability.
/// </summary>
public static class SharedExactMathematicsCapabilityResolver
{
    public static bool TryResolveLesson(
        string? lessonCode,
        out SharedExactMathematicsCapability? capability)
    {
        capability = null;
        if (!LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract) ||
            contract is null ||
            !string.Equals(contract.Readiness, "READY_VERIFIED", StringComparison.Ordinal))
        {
            return false;
        }

        capability = new SharedExactMathematicsCapability(
            contract.SkillId,
            contract.AllowedQuestionFamilies,
            "ApprovedLessonPracticeContract",
            contract.CurriculumLogicalLevel);
        return true;
    }

    public static bool TryResolveCurriculumContext(
        string? context,
        out SharedExactMathematicsCapability? capability)
    {
        capability = null;
        if (string.IsNullOrWhiteSpace(context))
            return false;

        var text = Normalize(context);

        // Fractions: most-specific phrases first.
        if (Has(text, "SIMPLIFY FRACTION", "SIMPLIFYING FRACTION", "LOWEST TERMS"))
            return Set("fractions.simplify", ["fractions.simplify.lowest_terms"], "ExactFractionSimplify", out capability);
        if (Has(text, "COMMON DENOMINATOR", "COMMON DENOMINATION"))
            return Set("fractions.common_denominator", ["fractions.common_denominator.missing_numerator"], "ExactFractionCommonDenominator", out capability);
        if (Has(text, "MULTIPLY FRACTIONS BY WHOLE", "MULTIPLY A FRACTION BY A WHOLE"))
            return Set("fractions.multiply_by_whole", ["fractions.multiply_by_whole.exact"], "ExactFractionMultiplyByWhole", out capability);
        if (Has(text, "DIVIDE FRACTIONS BY WHOLE", "DIVIDE A FRACTION BY A WHOLE"))
            return Set("fractions.divide_by_whole", ["fractions.divide_by_whole.exact"], "ExactFractionDivideByWhole", out capability);
        if (Has(text, "MULTIPLY FRACTIONS"))
            return Set("fractions.multiply", ["fractions.multiply.exact"], "ExactFractionMultiply", out capability);
        if (Has(text, "ADD AND SUBTRACT FRACTION", "ADDING AND SUBTRACTING FRACTION"))
            return Set("fractions.add_subtract", ["fractions.add_subtract.exact"], "ExactFractionAddSubtract", out capability);
        if (Has(text, "COMPARE FRACTIONS", "COMPARING FRACTIONS"))
            return Set("fractions.compare", ["fractions.compare.general"], "ExactFractionCompare", out capability);
        if (Has(text, "FRACTION NOTATION", "NUMERATOR AND DENOMINATOR"))
            return Set("fractions.notation", ["fractions.notation.identify_part"], "ExactFractionNotation", out capability);
        if (Has(text, "FRACTIONS ON A NUMBER LINE", "FRACTION NUMBER LINE"))
            return Set("fractions.number_line", ["fractions.number_line.read"], "ExactFractionNumberLine", out capability);
        if (Has(text, "MIXED NUMBER", "IMPROPER FRACTION"))
            return Set("fractions.mixed_improper.convert", ["fractions.mixed_improper.convert_to_improper_numerator"], "ExactMixedImproperFraction", out capability);
        if (Has(text, "FRACTION OF A QUANTITY", "FRACTIONS OF QUANTITIES", "FRACTION OF QUANTITY"))
            return Set("fractions.of_quantity", ["fractions.of_quantity.exact"], "ExactFractionOfQuantity", out capability);

        // Algebra.
        if (Has(text, "LINEAR INEQUALITY", "LINEAR INEQUALITIES", "SOLVE INEQUALIT"))
            return Set("algebra.linear.inequality.solve", ["algebra.linear.inequality.ax_plus_b_relation_c"], "ExactLinearInequality", out capability);
        if (Has(text, "LINEAR EQUATION", "SOLVE EQUATION"))
            return Set("algebra.linear.solve", ["algebra.linear.ax_plus_b_equals_c"], "ExactLinearEquation", out capability);

        // Geometry / trigonometry: never route the generic word "geometry".
        if (Has(text, "TRIGONOMETRIC MODELLING", "TRIGONOMETRIC MODELING"))
            return Set("trigonometry.modelling", ["trigonometry.modelling.right_triangle"], "ExactTrigModelling", out capability);
        if (Has(text, "TRIGONOMETRIC RATIO", "SINE COSINE AND TANGENT", "SIN COS TAN"))
            return Set(
                "trigonometry.right_triangle.sin_cos_tan",
                [
                    "trigonometry.right_triangle.sin_cos_tan.ratio",
                    "trigonometry.right_triangle.solve_side.special",
                    "trigonometry.right_triangle.solve_angle.special"
                ],
                "ExactRightTriangleTrig",
                out capability);
        if (Has(text, "PYTHAGORAS", "PYTHAGOREAN"))
            return Set("geometry.right_triangle.pythagorean", ["geometry.right_triangle.pythagorean.exact"], "ExactPythagorean", out capability);
        if (Has(text, "SURFACE AREA AND VOLUME", "SURFACE AREA", "VOLUME OF A CUBOID", "VOLUME OF CUBOID"))
            return Set("geometry.surface_area_volume", ["geometry.surface_area_volume.cuboid"], "ExactSurfaceAreaVolume", out capability);
        if (Has(text, "CONGRUENCE AND SIMILARITY"))
            return Set(
                "geometry.congruence_similarity",
                ["geometry.congruence.identify_criterion", "geometry.similarity.missing_length"],
                "ExactCongruenceSimilarity",
                out capability);
        if (Has(text, "CONGRUENCE", "CONGRUENT TRIANGLE"))
            return Set("geometry.congruence", ["geometry.congruence.identify_criterion"], "ExactCongruence", out capability);
        if (Has(text, "SIMILARITY", "SIMILAR SHAPE", "SCALE FACTOR"))
            return Set("geometry.similarity", ["geometry.similarity.missing_length"], "ExactSimilarity", out capability);
        if (Has(text, "ANGLE RELATIONSHIP", "ANGLE FACT", "MISSING ANGLE"))
            return Set("geometry.angles.relationships", ["geometry.angles.relationships.missing_angle"], "ExactAngleRelationship", out capability);
        if (Has(text, "GRADIENT", "SLOPE OF A LINE"))
            return Set("geometry.coordinate.straight_line", ["geometry.coordinate.straight_line.gradient"], "ExactCoordinateGradient", out capability);
        if (Has(text, "PERIMETER AND AREA", "RECTANGLE AREA", "RECTANGLE PERIMETER"))
            return Set("geometry.perimeter_area", ["geometry.perimeter_area.rectangle"], "ExactPerimeterArea", out capability);

        return false;
    }

    public static int? InferLogicalLevel(string? levelKey)
    {
        if (string.IsNullOrWhiteSpace(levelKey))
            return null;

        var match = Regex.Match(levelKey, @"(?<!\d)(?<level>1[0-3]|[1-9])(?!\d)", RegexOptions.CultureInvariant);
        return match.Success && int.TryParse(match.Groups["level"].Value, out var level)
            ? level
            : null;
    }

    private static bool Set(
        string skillId,
        IReadOnlyList<string> families,
        string reason,
        out SharedExactMathematicsCapability? capability)
    {
        capability = new SharedExactMathematicsCapability(skillId, families, reason);
        return true;
    }

    private static bool Has(string text, params string[] values) =>
        values.Any(value => text.Contains(value, StringComparison.Ordinal));

    private static string Normalize(string value) =>
        Regex.Replace(value.Trim().ToUpperInvariant(), @"\s+", " ");
}

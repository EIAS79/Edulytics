namespace Edulytics.Web.Presentation;

/// <summary>Independent educational alternatives to nine figure-skill concepts
/// from local Grade 12 Precalculus Section 3.8. No original images are copied.</summary>
public static class Grade12PrecalculusSection38FigureAlternatives
{
    public sealed record FigureSkill(int Number, string Title, string Model, string TeacherExplanation, int Exercise);
    public static readonly IReadOnlyList<FigureSkill> All =
    [
        new(1, "Cone volume and inverse radius", "For a cone with h=2r, use V=2πr³/3 to obtain r=∛(3V/(2π)). Require V≥0 and r≥0.", "An inverse of a cubic measurement model returns a physical length from volume.", 59),
        new(2, "Physical parabolic trough cross section", "Use a symmetric parabolic trough model y=x²/2 and compare widths at nonnegative depths; width w=2√(2y).", "Physical dimensions select a nonnegative root even when the mathematical parabola has both signs.", 57),
        new(3, "Parabola coordinate restriction", "For y=x²/2, restrict x≥0 to obtain x=√(2y), or x≤0 to obtain x=−√(2y).", "A full parabola is not one-to-one; each monotone half yields an inverse.", 32),
        new(4, "Cubic curve and inverse reflected about y=x", "For f(x)=x³+1, f⁻¹(x)=∛(x−1); swap ordered pairs (0,1) and (1,0).", "Inverse graphs reflect across y=x; every original input-output pair is swapped.", 36),
        new(5, "Both quadratic branches represent different inverse functions", "For f(x)=(x−4)², x≥4 gives inverse 4+√x while x≤4 gives inverse 4−√x.", "Choosing an inverse branch is dictated by the original restricted domain.", 35),
        new(6, "Quadratic and inverse branch reflection", "For f(x)=(x−2)²−1, x≥2, inverse g(x)=2+√(x+1), x≥−1.", "Graph original and inverse to display reflection and exchange of domain and range.", 39),
        new(7, "Quadratic translated vertex and inverse", "For f(x)=(x+1)²−3 on x≥−1, inverse −1+√(x+3) for x≥−3.", "The vertex coordinate (−1,−3) reverses to (−3,−1) on inverse.", 34),
        new(8, "Radical function and inverse reflection", "For f(x)=1+√(x−1), x≥1, inverse g(x)=(x−1)²+1 for x≥1.", "A square-root curve and its restricted quadratic inverse reflect about y=x.", 32),
        new(9, "Radical of a rational function requires nonnegative radicand", "For y=√((x−2)/(x+3)), use sign intervals about −3 (excluded) and 2 (included); domain (−∞,−3)∪[2,∞).", "A sign chart, not cancellation or taking real square roots of negative values, determines allowable inputs.", 42),
    ];
}

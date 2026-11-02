namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-matched CCSS supporting questions for G3 area, G5 volume, G6 reporting,
/// high-school function arithmetic/inversion and rational graph features.
/// These are exact numeric checks, not independent proof of every graphical skill.
/// </summary>
internal static partial class SupportingPracticeCompletionEngine
{
    private static Problem UsRectangleTiling(Random r, int scale)
    {
        var w=r.Next(2,12+2*scale);
        var h=r.Next(2,12+2*scale);
        return P("usccss.g3.rectangle_unit_tiles",
            $"A rectangle is {w} unit squares long and {h} unit squares wide. How many 1-by-1 squares cover it without gaps or overlaps?",
            "The area equals rows × unit squares per row, or length × width.",
            ("w",w),("h",h));
    }
    private static Problem UsUnitCubeVolume(Random r, int scale)
    {
        var l=r.Next(2,6+scale);
        var w=r.Next(2,6+scale);
        var h=r.Next(2,6+scale);
        return P("usccss.g5.unit_cube_count",
            $"A box is {l} unit cubes long, {w} unit cubes wide and {h} unit cubes high. Every cube has volume 1 cubic unit. How many cubic units fill the box?",
            "Count the unit cubes in one layer and multiply by the number of layers.",
            ("l",l),("w",w),("h",h));
    }
    private static Problem UsDataObservationCount(Random r, int scale)
    {
        var a=r.Next(2,15+scale*3);
        var b=r.Next(2,15+scale*3);
        var c=r.Next(2,15+scale*3);
        return P("usccss.g6.observation_count",
            $"A survey recorded {a} responses of 1 hour, {b} of 2 hours, and {c} of 3 hours. Report the total NUMBER OF OBSERVATIONS in the data set.",
            "The number of observations is the sum of category frequencies, not the number of categories.",
            ("a",a),("b",b),("c",c));
    }
    private static Problem UsFunctionSumValue(Random r,int scale)
    {
        var a=r.Next(2,7+scale);
        var b=r.Next(-12,13);
        var c=r.Next(2,7+scale);
        var d=r.Next(-12,13);
        var x=r.Next(-8,9);
        return P("usccss.hs.function_sum",
            $"Let f(x)={a}x+({b}) and g(x)={c}x+({d}). Find (f+g)({x}).",
            "Evaluate both functions at the same input, then add the two outputs.",
            ("a",a),("b",b),("c",c),("d",d),("x",x));
    }
    private static Problem UsFunctionComposition(Random r,int scale)
    {
        var a=r.Next(2,6+scale);
        var b=r.Next(-8,9);
        var c=r.Next(2,6+scale);
        var d=r.Next(-8,9);
        var x=r.Next(-5,6);
        return P("usccss.hs.function_composition",
            $"Let f(x)={a}x+({b}) and g(x)={c}x+({d}). Evaluate f(g({x})) by substituting the OUTPUT of g into f.",
            "Evaluate the inner function first, then apply the outer function; f(g(x)) need not equal g(f(x)).",
            ("a",a),("b",b),("c",c),("d",d),("x",x));
    }
    private static Problem UsInverseComposition(Random r,int scale)
    {
        var a=r.Next(2,7+scale);
        var b=r.Next(-16,17);
        var x=r.Next(-18,19);
        return P("usccss.hs.inverse_composition",
            $"f(x)={a}x+({b}) and g(y)=(y-({b}))/{a}. Evaluate g(f({x})) to verify that the functions undo one another.",
            "The composition of inverses returns the starting input: g(f(x))=x.",
            ("a",a),("b",b),("x",x));
    }
    private static Problem UsInverseTable(Random r,int scale)
    {
        var a=r.Next(2,9+scale);
        var b=r.Next(-18,19);
        var x=r.Next(-12,13);
        var y=a*x+b;
        var other1=x-2;
        var other2=x+3;
        return P("usccss.hs.inverse_table",
            $"An input-output table for f is: input {other1} → output {a*other1+b}; input {x} → output {y}; input {other2} → output {a*other2+b}. Read f⁻¹({y}) from the table.",
            "For an inverse lookup, reverse the arrows: the original output becomes the inverse input.",
            ("a",a),("b",b),("x",x),("y",y));
    }
    private static Problem UsRestrictInverse(Random r,int scale)
    {
        var vertex=r.Next(-12,13);
        var t=r.Next(1,12+scale*2);
        var value=t*t;
        return P("usccss.hs.restricted_inverse",
            $"The function f(x)=(x-({vertex}))² is restricted to x≥{vertex}. Find f⁻¹({value}) on this restricted domain.",
            "A quadratic is not one-to-one on the full real domain. With x≥vertex choose the nonnegative square root.",
            ("vertex",vertex),("t",t),("value",value));
    }
    private static Problem UsRationalVertical(Random r,int scale)
    {
        var pole=r.Next(-15,16);
        var root=pole+NonZero(r,-10,11);
        return P("usccss.hs.rational_vertical_asymptote",
            $"For the rational function f(x)=(x-({root}))/(x-({pole})), find the vertical asymptote x=___.",
            "An uncanceled denominator zero gives the vertical asymptote. The numerator must not also be zero there.",
            ("pole",pole),("root",root));
    }
    private static Problem UsRationalZero(Random r,int scale)
    {
        var root=r.Next(-15,16);
        var pole=root+NonZero(r,-10,11);
        return P("usccss.hs.rational_x_intercept",
            $"For f(x)=(x-({root}))/(x-({pole})), what x-value gives the zero (x-intercept) of the graph?",
            "A zero is a numerator root that is not excluded by the denominator.",
            ("root",root),("pole",pole));
    }
    private static Problem UsRationalHorizontal(Random r,int scale)
    {
        var k=NonZero(r,-6-scale,7+scale);
        var q=r.Next(2,8+scale);
        var pole=r.Next(-7,8);
        var root=pole+NonZero(r,-6,7);
        return P("usccss.hs.rational_horizontal_asymptote",
            $"Find the horizontal asymptote y=___ of f(x)=({q*k}x-({q*k*root}))/({q}x-({q*pole})).",
            "When numerator and denominator have equal degree, the ratio of leading coefficients is the horizontal asymptote.",
            ("k",k),("q",q),("pole",pole),("root",root));
    }
}
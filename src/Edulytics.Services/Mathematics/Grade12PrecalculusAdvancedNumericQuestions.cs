using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Independently authored source-skill numerical slices for 31 advanced
/// Precalculus sections. Every item has its own reconstructable parameter
/// oracle. None of these small subskills substitutes for a teacher-led
/// investigation, proof, graphing assessment, or complete textbook exercise.
/// </summary>
public static class Grade12PrecalculusAdvancedNumericQuestions
{
    public sealed record Question(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

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

    private static int N(Random r, int scale) => r.Next(2, Math.Min(25, 13 + scale));
    private static int S(Random r, int scale) =>
        r.Next(0, 2) == 0 ? N(r, scale) : -N(r, scale);

    public static Question Build(string family, Random random, int scale)
    {
        if (!Supports(family)) throw new ArgumentException("Unsupported advanced question family.", nameof(family));
        var suffix = family[Prefix.Length..];
        var a = S(random, scale);
        var b = S(random, scale);
        var c = S(random, scale);
        var d = S(random, scale);
        var x = random.Next(-6, 7);
        var n = N(random, scale);
        var k = random.Next(1, 14 + scale);
        // All generated parameters are explicit and individually reconstructable.
        // Some families restrict parameter ranges to ensure integer exact answers.
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        { ["a"] = a, ["b"] = b, ["c"] = c, ["d"] = d, ["x"] = x, ["n"] = n, ["k"] = k };
        string prompt, explanation;
        switch (suffix)
        {
            case "u03.l04.polynomial_evaluate":
                prompt = $"For the polynomial f(t)=(t−({a}))(t−({b}))(t−({c})), evaluate f({x}).";
                explanation = "Substitute the input into each factor and multiply exactly.";
                break;
            case "u03.l07.rational_asymptote":
                if (a == -b) a++; p["a"] = a;
                prompt = $"For f(t)=(t+({a}))/(t−({b})), determine the vertical asymptote t = ? The numerator does not cancel.";
                explanation = "The uncancelled denominator vanishes when t equals the indicated shift.";
                break;
            case "u03.l09.direct_variation":
                prompt = $"If y varies directly as t² with y={a}t², evaluate y at t={x}.";
                explanation = "Direct-square variation is evaluated by multiplying the constant by t squared.";
                break;
            case "u05.l01.radians_to_degrees":
                prompt = $"Convert {k}π/6 radians into degrees (answer as an integer).";
                explanation = "Multiply radians by 180/π; each π/6 corresponds to 30 degrees.";
                break;
            case "u05.l02.unit_circle_coordinate":
                p["k"] = random.Next(-16, 17);
                p["n"] = random.Next(0, 2); // 0 = cosine x-coordinate, 1 = sine y-coordinate
                prompt = $"On the unit circle at θ={p["k"]}π/2, determine the {(p["n"] == 0 ? "x-coordinate (cos θ)" : "y-coordinate (sin θ)")}.";
                explanation = "Reduce the angle modulo a full 2π revolution and read the signed cardinal coordinate.";
                break;
            case "u05.l03.trigonometric_ratio":
                p["n"] = random.Next(0, 2); // 0 asks opposite, 1 adjacent
                p["k"] = random.Next(1, 15);
                prompt = p["n"] == 0
                    ? $"If tan(theta)=3/4 and a right triangle has adjacent side {4 * p["k"]}, find the opposite side."
                    : $"If cot(theta)=4/3 and a right triangle has opposite side {3 * p["k"]}, find the adjacent side.";
                explanation = "tan is opposite/adjacent; cot is adjacent/opposite. Apply the given exact 3:4 ratio.";
                break;
            case "u06.l01.sine_wave_maximum":
                p["a"] = N(random, scale); p["b"] = S(random, scale);
                prompt = $"For y={p["a"]}sin({n}t)+({p["b"]}), give the maximum y-value.";
                explanation = "The sine wave ranges from −1 to 1; maximum equals midline plus positive amplitude.";
                break;
            case "u06.l02.tangent_period":
                prompt = $"For y=tan({n}t), the fundamental period is π divided by which positive integer?";
                explanation = "tan(t) has period π; horizontal compression by n divides the period by n.";
                break;
            case "u06.l03.inverse_sine_principal":
                p["k"] = random.Next(-6, 7) * 15;
                prompt = $"For θ={p["k"]}° (within [−90°,90°]), compute arcsin(sin θ) in degrees.";
                explanation = "The inverse sine principal interval contains this angle, so arcsin(sin θ)=θ.";
                break;
            case "u07.l01.pythagorean_identity":
                p["k"] = random.Next(3, 16);
                p["n"] = random.Next(1, p["k"]);
                prompt = $"Using sin²θ+cos²θ=1, if sinθ={p["n"]}/{p["k"]}, find the numerator of cos²θ written over {p["k"] * p["k"]}.";
                explanation = "Subtract the squared sine numerator from the squared denominator.";
                break;
            case "u07.l02.angle_sum_sine":
                p["n"] = random.Next(-8, 9); p["k"] = random.Next(-8, 9);
                prompt = $"Evaluate sin({p["n"] * 90}°+{p["k"] * 90}°) using the sine addition identity.";
                explanation = "Apply sin(A+B)=sin A cos B+cos A sin B, then use quarter-turn values.";
                break;
            case "u07.l05.trig_equation_solution_count":
                p["k"] = random.Next(1, 23);
                prompt = $"How many distinct solutions satisfy cos({p["k"]}θ)=1 for 0≤θ<2π?";
                explanation = "The equation holds at integer full revolutions of the argument; count one per period.";
                break;
            case "u07.l06.trig_model_amplitude":
                p["a"] = N(random, scale);
                prompt = $"For the periodic model y={p["a"]}sin({n}t+{c})+({b}), find its amplitude.";
                explanation = "Amplitude is the absolute value of the sine coefficient, regardless of shifts.";
                break;
            case "u08.l01.law_of_sines":
                p["a"] = N(random, scale);
                prompt = $"In a non-right triangle with angles A=30° and B=30° (C=120°), the side opposite B is {p["a"]}. By the sine law, find the side opposite A.";
                explanation = "a/sin 30° = b/sin 30°; equal angles have equal opposite sides.";
                break;
            case "u08.l02.law_of_cosines":
                p["a"] = N(random, scale); p["b"] = N(random, scale);
                prompt = $"Two sides of a triangle are {p["a"]} and {p["b"]}, enclosing 60°. Find the square of the third side using the cosine law.";
                explanation = "c²=a²+b²−2ab cos60°=a²+b²−ab.";
                break;
            case "u08.l03.polar_cartesian":
                p["k"] = random.Next(-10, 11); p["n"] = random.Next(0, 2);
                prompt = $"Convert polar point (r={a}, θ={(p["n"] == 0 ? 90 : 270)}°) to Cartesian form; give its y-coordinate.";
                explanation = "Use y=r sinθ; sin90°=1 and sin270°=−1.";
                break;
            case "u08.l04.polar_curve_evaluate":
                p["n"] = random.Next(0, 2);
                prompt = $"For the polar curve r={a}+({b})cosθ, calculate r at θ={(p["n"] == 0 ? 0 : 180)}°.";
                explanation = "Substitute cos0°=1 or cos180°=−1 into the polar equation.";
                break;
            case "u08.l06.parametric_point":
                prompt = $"For x(t)={a}t+({b}) and y(t)={c}t+({d}), find y({x}).";
                explanation = "Substitute the parameter t into the second coordinate rule.";
                break;
            case "u08.l07.parametric_slope":
                p["a"] = N(random, scale); p["c"] = S(random, scale);
                prompt = $"For x(t)={p["a"]}t+({b}), y(t)={p["a"] * p["c"]}t+({d}), calculate dy/dx.";
                explanation = "For parametric lines, dy/dx=(dy/dt)/(dx/dt); divide the two derivatives.";
                break;
            case "u09.l04.partial_fraction_coefficient":
                if (a == b) b++; p["b"] = b;
                p["n"] = random.Next(-9, 10); p["k"] = random.Next(-9, 10);
                prompt = $"Decompose (({p["n"] + p["k"]})t−({p["n"] * b + p["k"] * a}))/((t−({a}))(t−({b}))) as A/(t−({a}))+B/(t−({b})). Find A.";
                explanation = "Multiply by both denominators and evaluate the numerator identity at t=a.";
                break;
            case "u09.l07.matrix_determinant":
                prompt = $"Find det([[{a},{b}],[{c},{d}]]) for the displayed 2×2 matrix.";
                explanation = "The 2×2 determinant equals ad−bc; nonzero values permit inversion.";
                break;
            case "u10.l01.ellipse_major_axis":
                p["a"] = N(random, scale)+5; p["b"] = random.Next(2,p["a"]);
                prompt = $"The ellipse x²/{p["a"] * p["a"]}+y²/{p["b"] * p["b"]}=1 has a horizontal major axis. Find its complete major-axis length.";
                explanation = "The x-semiaxis is a; the full major axis is 2a.";
                break;
            case "u10.l02.hyperbola_transverse_axis":
                p["a"] = N(random, scale); p["b"] = N(random, scale);
                prompt = $"For x²/{p["a"] * p["a"]}−y²/{p["b"] * p["b"]}=1, find the full transverse-axis length.";
                explanation = "A horizontal hyperbola has vertices at ±a; the distance between them is 2a.";
                break;
            case "u10.l03.parabola_focus":
                p["a"] = S(random, scale);
                prompt = $"For parabola y²={4 * p["a"]}x, find the x-coordinate of its focus.";
                explanation = "Compare with y²=4px; the focus has coordinates (p,0).";
                break;
            case "u10.l05.polar_conic_radius":
                p["a"] = N(random, scale); p["b"] = random.Next(1, 9);
                prompt = $"For polar conic r={p["a"]}/(1+{p["b"]}/10 cosθ), calculate r when θ=90°.";
                explanation = "At 90° the cosine is zero, so the denominator is 1.";
                break;
            case "u11.l04.arithmetic_series_sum":
                p["n"] = 2 * random.Next(2, 12);
                prompt = $"For an arithmetic series of {p["n"]} terms with first term {a} and common difference {b}, find its total.";
                explanation = "Use S_n=n[2a+(n−1)d]/2.";
                break;
            case "u11.l05.permutations":
                p["n"] = random.Next(4, 10); p["k"] = random.Next(1,p["n"]);
                prompt = $"How many ordered selections of {p["k"]} distinct items can be made from {p["n"]} distinct objects? Compute nPr.";
                explanation = "Multiply n(n−1)…(n−r+1).";
                break;
            case "u11.l06.binomial_coefficient":
                p["n"] = random.Next(5, 12); p["k"] = random.Next(1,p["n"]);
                prompt = $"In (a+b)^{p["n"]}, find the binomial coefficient C({p["n"]},{p["k"]}) for a term.";
                explanation = "Use n!/[k!(n−k)!] and simplify using integer arithmetic.";
                break;
            case "u12.l01.polynomial_limit":
                prompt = $"Find lim(t→{x}) [{a}t²+({b})t+({c})].";
                explanation = "Polynomials are continuous: evaluate by direct substitution.";
                break;
            case "u12.l02.removable_limit":
                p["a"] = S(random,scale);
                prompt = $"For t≠{p["a"]}, find lim(t→{p["a"]}) (t²−{p["a"] * p["a"]})/(t−({p["a"]})).";
                explanation = "Factor the difference of squares and cancel the common factor only away from the removable hole.";
                break;
            case "u12.l03.continuity_matching":
                prompt = $"If f(t)={a}t+({b}) for t≠{x}, what value must f({x}) have to make f continuous?";
                explanation = "Match the separately defined point value to the limit of the affine rule.";
                break;
            default: throw new ArgumentOutOfRangeException(nameof(family));
        }
        return new Question(family, prompt, explanation, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family)) throw new ArgumentException("Unknown advanced numerical skill.",nameof(family));
        var a=p["a"];var b=p["b"];var c=p["c"];var d=p["d"];
        var x=p["x"];var n=p["n"];var k=p["k"];
        var answer=family[Prefix.Length..] switch
        {
            "u03.l04.polynomial_evaluate" => (x-a)*(x-b)*(x-c),
            "u03.l07.rational_asymptote" => b,
            "u03.l09.direct_variation" => a*x*x,
            "u05.l01.radians_to_degrees" => 30*k,
            "u05.l02.unit_circle_coordinate" => n==0
                ? ((k%4+4)%4 switch {0=>1,1=>0,2=>-1,_=>0})
                : ((k%4+4)%4 switch {0=>0,1=>1,2=>0,_=>-1}),
            "u05.l03.trigonometric_ratio" => (n==0?3:4)*k,
            "u06.l01.sine_wave_maximum" => a+b,
            "u06.l02.tangent_period" => n,
            "u06.l03.inverse_sine_principal" => k,
            "u07.l01.pythagorean_identity" => k*k-n*n,
            "u07.l02.angle_sum_sine" => ((n+k)%4+4)%4 switch {0=>0,1=>1,2=>0,_=>-1},
            "u07.l05.trig_equation_solution_count" => k,
            "u07.l06.trig_model_amplitude" => a,
            "u08.l01.law_of_sines" => a,
            "u08.l02.law_of_cosines" => a*a+b*b-a*b,
            "u08.l03.polar_cartesian" => n==0?a:-a,
            "u08.l04.polar_curve_evaluate" => a+(n==0?b:-b),
            "u08.l06.parametric_point" => c*x+d,
            "u08.l07.parametric_slope" => c,
            "u09.l04.partial_fraction_coefficient" => n,
            "u09.l07.matrix_determinant" => a*d-b*c,
            "u10.l01.ellipse_major_axis" => 2*a,
            "u10.l02.hyperbola_transverse_axis" => 2*a,
            "u10.l03.parabola_focus" => a,
            "u10.l05.polar_conic_radius" => a,
            "u11.l04.arithmetic_series_sum" => n*(2*a+(n-1)*b)/2,
            "u11.l05.permutations" => Enumerable.Range(0,k).Aggregate(1,(result,i)=>result*(n-i)),
            "u11.l06.binomial_coefficient" => Choose(n,k),
            "u12.l01.polynomial_limit" => a*x*x+b*x+c,
            "u12.l02.removable_limit" => 2*a,
            "u12.l03.continuity_matching" => a*x+b,
            _ => throw new ArgumentOutOfRangeException(nameof(family))
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    private static int Choose(int n,int k)
    {
        k=Math.Min(k,n-k);
        long value=1;
        for(var i=1;i<=k;i++)
            value=value*(n-k+i)/i;
        return checked((int)value);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? candidate) =>
        Supports(family) &&
        int.TryParse(candidate,NumberStyles.Integer,CultureInfo.InvariantCulture,out var value) &&
        string.Equals(value.ToString(CultureInfo.InvariantCulture),Solve(family,p),StringComparison.Ordinal);
}

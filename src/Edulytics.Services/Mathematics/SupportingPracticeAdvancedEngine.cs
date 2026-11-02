using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Generation;

namespace Edulytics.Services.Mathematics;

internal static class SupportingPracticeAdvancedEngine
{
    private static readonly IReadOnlySet<string> Families =
        new HashSet<string>(StringComparer.Ordinal)
        {
            "supporting.number.compare_order",
            "supporting.number.parity",
            "supporting.number.bounds_upper",
            "supporting.number.scale_power10",
            "supporting.fractions.mixed_to_improper",
            "supporting.measurement.compare",
            "supporting.geometry.shape_dimension",
            "supporting.geometry.turn_degrees",
            "supporting.geometry.triangle_area",
            "supporting.geometry.surface_area_cuboid",
            "supporting.algebra.algebraic_fraction",
            "supporting.algebra.complete_square_vertex",
            "supporting.algebra.inverse_proportion",
            "supporting.functions.inverse_linear",
            "supporting.functions.polynomial_value",
            "supporting.coordinate.line_intersection",
            "supporting.circle.angle_semicircle",
            "supporting.trigonometry.bearing",
            "supporting.trigonometry.sine_rule_exact",
            "supporting.trigonometry.cosine_rule_square",
            "supporting.trigonometry.area_sine_double",
            "supporting.trigonometry.graph_special",
            "supporting.sets.union_count",
            "supporting.statistics.grouped_mean",
            "supporting.statistics.cumulative_total",
            "supporting.statistics.histogram_density",
            "supporting.statistics.quartile",
            "supporting.probability.independent_product",
            "supporting.probability.expected_frequency",
            "supporting.probability.binomial_half",
            "supporting.probability.normal_symmetry",
            "supporting.probability.poisson_mean",
            "supporting.probability.uniform_interval",
            "supporting.statistics.hypothesis_decision",
            "supporting.calculus.differential_equation_value",
            "supporting.numerical.bisection_midpoint",
            "supporting.series.geometric_sum",
            "supporting.algebra.binomial_coefficient",
            "supporting.mechanics.projectile_velocity",
            "supporting.mechanics.energy",
            "supporting.mechanics.circular_motion",
            "supporting.mechanics.equilibrium",
            "supporting.mechanics.friction",
            "supporting.mechanics.connected_particles",
            "supporting.vectors.add",
            "supporting.vectors.magnitude",
            "supporting.circle.arc_angle_degrees",
            "supporting.probability.discrete_expected_value",
            "supporting.trigonometry.identity_missing_component",
            "supporting.trigonometry.equation_smallest_angle"
        };

    internal sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) && Families.Contains(family.Trim());

    public static Problem Build(
        string family,
        Random r,
        int scale,
        int? preferredVariant = null) =>
        family switch
        {
            "supporting.number.compare_order" => CompareOrder(r, scale),
            "supporting.number.parity" => Parity(r, scale),
            "supporting.number.bounds_upper" => BoundsUpper(r, scale),
            "supporting.number.scale_power10" => ScalePower10(r, scale),
            "supporting.fractions.mixed_to_improper" => MixedToImproper(r, scale),
            "supporting.measurement.compare" => MeasurementCompare(r, scale),
            "supporting.geometry.shape_dimension" => ShapeDimension(r, preferredVariant),
            "supporting.geometry.turn_degrees" => TurnDegrees(r),
            "supporting.geometry.triangle_area" => TriangleArea(r, scale),
            "supporting.geometry.surface_area_cuboid" => SurfaceAreaCuboid(r, scale),
            "supporting.algebra.algebraic_fraction" => AlgebraicFraction(r, scale),
            "supporting.algebra.complete_square_vertex" => CompleteSquareVertex(r, scale),
            "supporting.algebra.inverse_proportion" => InverseProportion(r, scale),
            "supporting.functions.inverse_linear" => InverseLinear(r, scale),
            "supporting.functions.polynomial_value" => PolynomialValue(r, scale),
            "supporting.coordinate.line_intersection" => LineIntersection(r, scale),
            "supporting.circle.angle_semicircle" => CircleSemicircle(r, preferredVariant),
            "supporting.trigonometry.bearing" => Bearing(r),
            "supporting.trigonometry.sine_rule_exact" => SineRule(r, scale),
            "supporting.trigonometry.cosine_rule_square" => CosineRule(r, scale),
            "supporting.trigonometry.area_sine_double" => AreaSine(r, scale),
            "supporting.trigonometry.graph_special" => TrigGraph(r),
            "supporting.sets.union_count" => UnionCount(r, scale),
            "supporting.statistics.grouped_mean" => GroupedMean(r, scale),
            "supporting.statistics.cumulative_total" => CumulativeTotal(r, scale),
            "supporting.statistics.histogram_density" => HistogramDensity(r, scale),
            "supporting.statistics.quartile" => Quartile(r, scale),
            "supporting.probability.independent_product" => IndependentProduct(r, preferredVariant),
            "supporting.probability.expected_frequency" => ExpectedFrequency(r, scale),
            "supporting.probability.binomial_half" => BinomialHalf(r),
            "supporting.probability.normal_symmetry" => NormalSymmetry(r, preferredVariant),
            "supporting.probability.poisson_mean" => PoissonMean(r, scale),
            "supporting.probability.uniform_interval" => UniformInterval(r, scale),
            "supporting.statistics.hypothesis_decision" => HypothesisDecision(r),
            "supporting.calculus.differential_equation_value" => DifferentialEquation(r, scale),
            "supporting.numerical.bisection_midpoint" => BisectionMidpoint(r, scale),
            "supporting.series.geometric_sum" => GeometricSum(r, scale),
            "supporting.algebra.binomial_coefficient" => BinomialCoefficient(r, scale),
            "supporting.mechanics.projectile_velocity" => ProjectileVelocity(r, scale),
            "supporting.mechanics.energy" => Energy(r, scale),
            "supporting.mechanics.circular_motion" => CircularMotion(r, scale),
            "supporting.mechanics.equilibrium" => Equilibrium(r, scale),
            "supporting.mechanics.friction" => Friction(r, scale),
            "supporting.mechanics.connected_particles" => ConnectedParticles(r, scale),
            "supporting.vectors.add" => VectorAdd(r, scale),
            "supporting.vectors.magnitude" => VectorMagnitude(r, scale),
            "supporting.circle.arc_angle_degrees" => CircleArcAngle(r),
            "supporting.probability.discrete_expected_value" => DiscreteExpectedValue(r, scale),
            "supporting.trigonometry.identity_missing_component" => TrigIdentityMissingComponent(r, scale),
            "supporting.trigonometry.equation_smallest_angle" => TrigEquationSmallestAngle(r, scale),
            _ => throw new InvalidOperationException($"Unsupported advanced Supporting family: {family}")
        };

    public static string Solve(string family, IReadOnlyDictionary<string, int> p) =>
        family switch
        {
            "supporting.number.compare_order" => p["left"] < p["right"] ? "<" : p["left"] > p["right"] ? ">" : "=",
            "supporting.number.parity" => p["value"] % 2 == 0 ? "even" : "odd",
            "supporting.number.bounds_upper" => (p["rounded"] + p["place"] / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.number.scale_power10" => (p["value"] * Pow10(p["power"])).ToString(CultureInfo.InvariantCulture),
            "supporting.fractions.mixed_to_improper" => $"{p["whole"] * p["denominator"] + p["numerator"]}/{p["denominator"]}",
            "supporting.measurement.compare" => p["left"] < p["right"] ? "<" : p["left"] > p["right"] ? ">" : "=",
            "supporting.geometry.shape_dimension" => p["dimension"] == 2 ? "2D" : "3D",
            "supporting.geometry.turn_degrees" => (p["quarterTurns"] * 90).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.triangle_area" => (p["base"] * p["height"] / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.surface_area_cuboid" => (2 * (p["l"] * p["w"] + p["l"] * p["h"] + p["w"] * p["h"])).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.algebraic_fraction" => ((p["a"] * p["x"] + p["b"]) / p["divisor"]).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.complete_square_vertex" => p["h"].ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.inverse_proportion" => (p["k"] / p["x"]).ToString(CultureInfo.InvariantCulture),
            "supporting.functions.inverse_linear" => ((p["y"] - p["b"]) / p["a"]).ToString(CultureInfo.InvariantCulture),
            "supporting.functions.polynomial_value" => (p["a"] * p["x"] * p["x"] + p["b"] * p["x"] + p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.coordinate.line_intersection" => p["x"].ToString(CultureInfo.InvariantCulture),
            "supporting.circle.angle_semicircle" => SolveCircleSemicircle(p),
            "supporting.trigonometry.bearing" => p["bearing"].ToString("000", CultureInfo.InvariantCulture),
            "supporting.trigonometry.sine_rule_exact" => p["targetSide"].ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.cosine_rule_square" => (p["b"] * p["b"] + p["c"] * p["c"] - p["bcTerm"]).ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.area_sine_double" => (p["a"] * p["b"] * p["sinNumerator"] / p["sinDenominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.graph_special" => p["valueNumerator"] == 0 ? "0" : $"{p["valueNumerator"]}/{p["valueDenominator"]}",
            "supporting.sets.union_count" => (p["a"] + p["b"] - p["intersection"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.grouped_mean" => ((p["m1"] * p["f1"] + p["m2"] * p["f2"]) / (p["f1"] + p["f2"])).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.cumulative_total" => (p["f1"] + p["f2"] + p["f3"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.histogram_density" => (p["frequency"] / p["width"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.quartile" => p["q"].ToString(CultureInfo.InvariantCulture),
            "supporting.probability.independent_product" => SolveIndependentProduct(p),
            "supporting.probability.expected_frequency" => (p["trials"] * p["numerator"] / p["denominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.probability.binomial_half" => $"{p["n"]}/{Pow2(p["n"])}",
            "supporting.probability.normal_symmetry" => SolveNormalSymmetry(p),
            "supporting.probability.poisson_mean" => p["lambda"].ToString(CultureInfo.InvariantCulture),
            "supporting.probability.uniform_interval" => $"{p["favourable"]}/{p["total"]}",
            "supporting.statistics.hypothesis_decision" => p["reject"] == 1 ? "reject" : "do not reject",
            "supporting.calculus.differential_equation_value" => (p["a"] * p["x"] * p["x"] + p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.numerical.bisection_midpoint" => ((p["left"] + p["right"]) / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.series.geometric_sum" => (p["first"] * (Pow(p["ratio"], p["n"]) - 1) / (p["ratio"] - 1)).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.binomial_coefficient" => (p["n"] * (p["n"] - 1) / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.projectile_velocity" => (p["u"] - p["g"] * p["t"]).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.energy" => (p["force"] * p["distance"]).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.circular_motion" => (p["speed"] * p["speed"] / p["radius"]).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.equilibrium" => p["force"].ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.friction" => (p["muNumerator"] * p["normal"] / p["muDenominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.connected_particles" => (p["force"] / (p["m1"] + p["m2"])).ToString(CultureInfo.InvariantCulture),
            "supporting.vectors.add" => $"<{p["ax"] + p["bx"]}, {p["ay"] + p["by"]}>",
            "supporting.vectors.magnitude" => p["magnitude"].ToString(CultureInfo.InvariantCulture),
            "supporting.circle.arc_angle_degrees" => (360 * p["numerator"] / p["denominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.probability.discrete_expected_value" => ((p["x1"] + p["x2"]) / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.identity_missing_component" => p["missing"].ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.equation_smallest_angle" => p["angle"].ToString(CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported advanced Supporting solver family: {family}")
        };

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string answer)
    {
        var expected = Solve(family, p);
        if (family is "supporting.fractions.mixed_to_improper" or
            "supporting.trigonometry.graph_special" or
            "supporting.probability.independent_product" or
            "supporting.probability.binomial_half" or
            "supporting.probability.normal_symmetry" or
            "supporting.probability.uniform_interval")
            return EquivalentFraction(answer, expected);

        if (family == "supporting.vectors.add")
            return NormalizePair(answer) == NormalizePair(expected);

        return Normalize(answer) == Normalize(expected);
    }

    private static Problem CircleArcAngle(Random r)
    {
        int[] denominators = [4, 6, 8, 9, 10, 12];
        var denominator = denominators[r.Next(denominators.Length)];
        var numerator = r.Next(1, denominator);
        while ((360 * numerator) % denominator != 0)
            numerator = r.Next(1, denominator);

        return P(
            "supporting.circle.arc_angle_degrees",
            $"An arc is {numerator}/{denominator} of a full circle. Find its central angle in degrees.",
            "A full circle is 360°. Multiply 360° by the arc's fraction of the whole circle.",
            ("numerator", numerator),
            ("denominator", denominator));
    }

    private static Problem DiscreteExpectedValue(Random r,int s)
    {
        var x1 = r.Next(0, 8 + s * 3);
        var offset = 2 * r.Next(1, 6 + s * 2);
        var x2 = x1 + offset;
        return P(
            "supporting.probability.discrete_expected_value",
            $"A discrete random variable X takes values {x1} and {x2}, each with probability 1/2. Find E(X).",
            "Expected value is the probability-weighted average: E(X)=x₁(1/2)+x₂(1/2).",
            ("x1", x1),
            ("x2", x2));
    }

    private static Problem TrigIdentityMissingComponent(Random r,int s)
    {
        var triples = new (int A,int B,int C)[] { (3,4,5), (5,12,13), (8,15,17), (7,24,25), (20,21,29) };
        var triple = triples[r.Next(triples.Length)];
        var multiplier = r.Next(1, 3 + s);
        var a = triple.A * multiplier;
        var b = triple.B * multiplier;
        var cc = triple.C * multiplier;
        return P(
            "supporting.trigonometry.identity_missing_component",
            $"For an acute angle θ, sin θ={a}/{cc}. Using sin²θ+cos²θ=1, find the positive integer x if cos θ=x/{cc}.",
            "Use the Pythagorean identity: x²=c²−a², then take the positive square root because θ is acute.",
            ("sinNumerator", a),
            ("denominator", cc),
            ("missing", b));
    }

    private static Problem TrigEquationSmallestAngle(Random r,int s)
    {
        var variant = r.Next(0, 4);
        var coefficient = r.Next(1, 10 + s * 4);
        var angle = variant switch { 0 => 30, 1 => 60, 2 => 45, _ => 90 };
        var function = variant switch { 0 => "sin", 1 => "cos", 2 => "tan", _ => "sin" };
        var rhs = variant switch
        {
            0 => $"{coefficient}/2",
            1 => $"{coefficient}/2",
            2 => coefficient.ToString(CultureInfo.InvariantCulture),
            _ => coefficient.ToString(CultureInfo.InvariantCulture)
        };

        return P(
            "supporting.trigonometry.equation_smallest_angle",
            $"Find the smallest θ in 0°≤θ≤180° satisfying {coefficient}{function} θ={rhs}.",
            "Divide by the coefficient, identify the exact special-angle value, and choose the smallest solution in the stated interval.",
            ("variant", variant),
            ("coefficient", coefficient),
            ("angle", angle));
    }

    private static Problem CompareOrder(Random r,int s){var a=r.Next(0,500+s*500);var b=r.Next(0,500+s*500);return P("supporting.number.compare_order",$"Compare {a} and {b}. Enter <, >, or =.","Compare digits from the greatest place value to the least.",AssessmentItemType.ShortAnswer,("left",a),("right",b));}
    private static Problem Parity(Random r,int s){var v=r.Next(0,200+s*200);return P("supporting.number.parity",$"Is {v} odd or even?","An integer is even exactly when it is divisible by 2.",AssessmentItemType.ShortAnswer,("value",v));}
    private static Problem BoundsUpper(Random r,int s){int[] places=[10,100,1000];var place=places[r.Next(Math.Min(places.Length,s+1))];var rounded=r.Next(2,50+s*20)*place;return P("supporting.number.bounds_upper",$"{rounded} is a value rounded to the nearest {place}. Find the upper bound.","The upper bound is half one rounding interval above the rounded value.",("rounded",rounded),("place",place));}
    private static Problem ScalePower10(Random r,int s){var v=r.Next(1,50+s*20);var p=r.Next(1,Math.Min(4,s+2));return P("supporting.number.scale_power10",$"Calculate {v} × 10^{p}.","Multiplying by a power of ten shifts place value by the exponent.",("value",v),("power",p));}
    private static Problem MixedToImproper(Random r,int s){var w=r.Next(1,6+s);var d=r.Next(2,8+s);var n=r.Next(1,d);return P("supporting.fractions.mixed_to_improper",$"Convert {w} {n}/{d} to an improper fraction.","Multiply the whole part by the denominator, add the numerator, and keep the denominator.",AssessmentItemType.ShortAnswer,("whole",w),("numerator",n),("denominator",d));}
    private static Problem MeasurementCompare(Random r,int s){var a=r.Next(1,100+s*50);var b=r.Next(1,100+s*50);return P("supporting.measurement.compare",$"Compare measurements {a} and {b} expressed in the same unit. Enter <, >, or =.","Once units match, compare the numerical measures directly.",AssessmentItemType.ShortAnswer,("left",a),("right",b));}
    private static Problem ShapeDimension(Random r, int? preferredVariant = null)
    {
        var variant = preferredVariant.HasValue
            ? QuestionVariantPolicy.NormalizeSlot(preferredVariant.Value)
            : r.Next(0, QuestionVariantPolicy.MaximumVariantsPerFamily);

        var form = variant / 4;
        var local = variant % 4;
        var shape = form switch
        {
            0 => new[] { 0, 4, 2, 6 }[local],
            1 => new[] { 1, 5, 3, 7 }[local],
            2 => new[] { 0, 4, 1, 5 }[local],
            _ => new[] { 2, 6, 3, 7 }[local]
        };

        var dimension = shape <= 3 ? 2 : 3;
        var name = shape switch
        {
            0 => "square",
            1 => "rectangle",
            2 => "triangle",
            3 => "circle",
            4 => "cube",
            5 => "cuboid",
            6 => "sphere",
            _ => "cylinder"
        };

        var prompt = form switch
        {
            0 => $"Is a {name} a 2D or 3D shape?",
            1 => dimension == 2
                ? $"A {name} is flat and has no solid depth. Classify it as 2D or 3D."
                : $"A {name} has solid extent and occupies space. Classify it as 2D or 3D.",
            2 =>
                $"A student says a {name} is {(dimension == 2 ? "3D" : "2D")}. Correct the classification. Enter 2D or 3D.",
            _ => $"In a real-world model, {ShapeContext(shape)} is represented by a {name}. Is the mathematical model 2D or 3D?"
        };

        var solution = form switch
        {
            0 => "2D shapes are flat; 3D shapes have solid extent.",
            1 => "Use defining dimensional properties rather than appearance: flat figures are 2D and solids with extent are 3D.",
            2 => "Check whether the named shape is flat or has solid extent, then correct the student's classification.",
            _ => "Classify the mathematical model, not the material object: flat figures are 2D and solid forms are 3D."
        };

        return P(
            "supporting.geometry.shape_dimension",
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            ("dimension", dimension),
            ("form", form),
            ("shape", shape),
            ("variant", variant));
    }

    private static string ShapeContext(int shape) =>
        shape switch
        {
            0 => "a square floor tile face",
            1 => "a rectangular book cover",
            2 => "a triangular road-sign face",
            3 => "a circular clock face",
            4 => "a cube-shaped block",
            5 => "a rectangular storage box",
            6 => "a ball",
            _ => "a drinks can"
        };
    private static Problem TurnDegrees(Random r){var q=r.Next(1,13);return P("supporting.geometry.turn_degrees",$"How many degrees are in {q} quarter-turn(s)?","One quarter-turn is 90 degrees; repeated quarter-turns can exceed one full turn.",("quarterTurns",q));}
    private static Problem TriangleArea(Random r,int s){var b=2*r.Next(2,8+s);var h=r.Next(2,8+s);return P("supporting.geometry.triangle_area",$"A triangle has base {b} and perpendicular height {h}. Find its area.","Use one half times base times perpendicular height.",("base",b),("height",h));}
    private static Problem SurfaceAreaCuboid(Random r,int s){var l=r.Next(2,7+s);var w=r.Next(2,7+s);var h=r.Next(2,6+s);return P("supporting.geometry.surface_area_cuboid",$"A cuboid has dimensions {l}, {w}, {h}. Find its total surface area.","Add the areas of the three pairs of opposite rectangular faces.",("l",l),("w",w),("h",h));}
    private static Problem AlgebraicFraction(Random r,int s){var x=r.Next(1,8+s);var d=r.Next(2,5+s);var a=d*r.Next(1,4+s);var b=d*r.Next(0,4+s);return P("supporting.algebra.algebraic_fraction",$"Evaluate ({a}x+{b})/{d} when x={x}.","Substitute x, simplify the numerator, then divide by the denominator.",("a",a),("b",b),("divisor",d),("x",x));}
    private static Problem CompleteSquareVertex(Random r,int s){var h=r.Next(-5-s,6+s);var k=r.Next(-8-s,9+s);return P("supporting.algebra.complete_square_vertex",$"The quadratic is written as (x-({h}))²+{k}. Find the x-coordinate of its vertex.","In vertex form (x-h)²+k, the vertex has x-coordinate h.",("h",h),("k",k));}
    private static Problem InverseProportion(Random r,int s){var x=r.Next(2,8+s);var y=r.Next(2,8+s);var k=x*y;return P("supporting.algebra.inverse_proportion",$"y is inversely proportional to x with constant k={k}. Find y when x={x}.","For inverse proportion y=k/x.",("k",k),("x",x));}
    private static Problem InverseLinear(Random r,int s){var a=r.Next(2,5+s);var x=r.Next(-5-s,6+s);var b=r.Next(-8-s,9+s);var y=a*x+b;return P("supporting.functions.inverse_linear",$"For f(x)={a}x{Sign(b)}, find f⁻¹({y}).","Set y=ax+b and solve for x.",("a",a),("b",b),("y",y));}
    private static Problem PolynomialValue(Random r,int s){var a=r.Next(1,4+s);var b=r.Next(-4-s,5+s);var c=r.Next(-6-s,7+s);var x=r.Next(-3-s,4+s);return P("supporting.functions.polynomial_value",$"For p(x)={a}x²{Sign(b)}x{Sign(c)}, find p({x}).","Substitute the input and apply powers before multiplication and addition.",("a",a),("b",b),("c",c),("x",x));}
    private static Problem LineIntersection(Random r,int s){var x=r.Next(-5-s,6+s);var m1=r.Next(1,4+s);var m2=m1+r.Next(1,4+s);var b1=r.Next(-6-s,7+s);var y=m1*x+b1;var b2=y-m2*x;return P("supporting.coordinate.line_intersection",$"Lines y={m1}x{Sign(b1)} and y={m2}x{Sign(b2)} intersect at x=k. Find k.","At an intersection the y-values are equal; solve the resulting linear equation.",("x",x),("m1",m1),("m2",m2),("b1",b1),("b2",b2));}
    private static Problem CircleSemicircle(Random r,int? preferredVariant=null)
    {
        var slot=QuestionVariantPolicy.NormalizeSlot(preferredVariant ?? r.Next(0,16));
        var mode=slot/4;
        if(mode==0)
            return P("supporting.circle.angle_semicircle",
                "An angle at the circumference subtends a diameter. Find the angle in degrees.",
                "An angle in a semicircle is a right angle.",
                ("mode",mode),("variant",slot));
        if(mode==1)
        {
            var given=r.Next(20,71);
            return P("supporting.circle.angle_semicircle",
                $"Triangle ABC is inscribed in a semicircle with AB as the diameter. If angle A is {given}°, find angle B.",
                "The angle at C is 90° because it subtends the diameter. The two acute angles therefore sum to 90°.",
                ("mode",mode),("givenAngle",given),("variant",slot));
        }
        if(mode==2)
        {
            int[] claims=[45,60,75,120];
            var claim=claims[r.Next(claims.Length)];
            return P("supporting.circle.angle_semicircle",
                $"A student says an angle at the circumference subtending a diameter can be {claim}°. What is the correct angle?",
                "The diameter subtends a right angle at the circumference, so the claimed non-right angle is incorrect.",
                ("mode",mode),("claim",claim),("variant",slot));
        }

        int[] ratios=[1,2,4,5,8];
        var ratio=ratios[r.Next(ratios.Length)];
        return P("supporting.circle.angle_semicircle",
            $"Triangle ABC is inscribed in a semicircle with AB as the diameter. Its two acute angles are in the ratio 1:{ratio}. Find the smaller acute angle.",
            "The angle subtending the diameter is 90°, so the two remaining angles total 90°. Split 90° in the stated ratio.",
            ("mode",mode),("ratio",ratio),("variant",slot));
    }
    private static Problem Bearing(Random r){int[] b=[30,45,60,90,120,135,180,225,270,315];var v=b[r.Next(b.Length)];return P("supporting.trigonometry.bearing",$"A direction is {v}° clockwise from north. Write the three-figure bearing.","Bearings are measured clockwise from north and written with three digits.",AssessmentItemType.ShortAnswer,("bearing",v));}
    private static Problem SineRule(Random r,int s){var baseSide=2*r.Next(2,14+s*3);var target=baseSide*2;return P("supporting.trigonometry.sine_rule_exact",$"Using a/sin A=b/sin B, let a={baseSide}, sin A=1/2 and sin B=1. Find b.","Rearrange b=a·sinB/sinA and substitute the exact sine values.",("baseSide",baseSide),("targetSide",target));}
    private static Problem CosineRule(Random r,int s){var b=r.Next(2,7+s);var c=r.Next(2,7+s);var bc=b*c;return P("supporting.trigonometry.cosine_rule_square",$"For sides b={b}, c={c} with included angle 60°, use a²=b²+c²-2bc cos60°. Find a².","Since cos60°=1/2, the final term is bc.",("b",b),("c",c),("bcTerm",bc));}
    private static Problem AreaSine(Random r,int s){var a=2*r.Next(2,7+s);var b=r.Next(2,7+s);return P("supporting.trigonometry.area_sine_double",$"Two sides are {a} and {b} with included angle 30°. Find twice the triangle area.","Area=1/2 ab sinC, so twice the area is ab sinC and sin30°=1/2.",("a",a),("b",b),("sinNumerator",1),("sinDenominator",2));}
    private static Problem TrigGraph(Random r){(int angle,int numerator,int denominator)[] values=[(0,0,1),(30,1,2),(90,1,1),(150,1,2),(180,0,1),(210,-1,2),(270,-1,1),(330,-1,2),(360,0,1),(390,1,2),(450,1,1),(510,1,2)];var v=values[r.Next(values.Length)];return P("supporting.trigonometry.graph_special",$"For the sine graph, find sin({v.angle}°) as an exact fraction.","Use periodicity and the standard unit-circle values for special angles.",AssessmentItemType.ShortAnswer,("angle",v.angle),("valueNumerator",v.numerator),("valueDenominator",v.denominator));}
    private static Problem UnionCount(Random r,int s){var inter=r.Next(1,5+s);var a=inter+r.Next(1,8+s);var b=inter+r.Next(1,8+s);return P("supporting.sets.union_count",$"Set A has {a} elements, B has {b}, and A∩B has {inter}. Find |A∪B|.","Use |A∪B|=|A|+|B|-|A∩B|.",("a",a),("b",b),("intersection",inter));}
    private static Problem GroupedMean(Random r,int s){var m1=2*r.Next(1,5+s);var m2=m1+2*r.Next(1,4+s);var f1=r.Next(1,5+s);var f2=r.Next(1,5+s);while((m1*f1+m2*f2)%(f1+f2)!=0)f2=r.Next(1,5+s);return P("supporting.statistics.grouped_mean",$"Grouped midpoints {m1},{m2} have frequencies {f1},{f2}. Find the estimated mean.","Multiply each midpoint by its frequency, add, then divide by total frequency.",("m1",m1),("m2",m2),("f1",f1),("f2",f2));}
    private static Problem CumulativeTotal(Random r,int s){var a=r.Next(1,10+s*2);var b=r.Next(1,10+s*2);var c=r.Next(1,10+s*2);return P("supporting.statistics.cumulative_total",$"Class frequencies are {a}, {b}, {c}. Find the final cumulative frequency.","The final cumulative frequency is the total frequency.",("f1",a),("f2",b),("f3",c));}
    private static Problem HistogramDensity(Random r,int s){var width=r.Next(1,5+s);var density=r.Next(1,8+s);var freq=width*density;return P("supporting.statistics.histogram_density",$"A histogram class has width {width} and frequency {freq}. Find frequency density.","Frequency density=frequency/class width.",("width",width),("frequency",freq));}
    private static Problem Quartile(Random r,int s){var q=r.Next(2,20+s*5);var variant=r.Next(1,9999);return P("supporting.statistics.quartile",$"An ordered data summary states the lower quartile is {q}. What is Q1?","Q1 is another name for the lower quartile.",("q",q),("variant",variant));}
    private static Problem IndependentProduct(Random r,int? preferredVariant=null)
    {
        var slot=QuestionVariantPolicy.NormalizeSlot(preferredVariant ?? r.Next(0,16));
        var mode=slot/4;
        int[] denominators=[2,3,4,5,6,8,10];
        var d1=denominators[r.Next(denominators.Length)];
        var d2=denominators[r.Next(denominators.Length)];
        var n1=r.Next(1,d1);
        var n2=r.Next(1,d2);

        if(mode==0)
            return P("supporting.probability.independent_product",
                $"Independent events A and B have probabilities {n1}/{d1} and {n2}/{d2}. Find P(A and B).",
                "For independent events, multiply the probabilities along the branch.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("n1",n1),("d1",d1),("n2",n2),("d2",d2),("power",2),("variant",slot));

        if(mode==1)
            return P("supporting.probability.independent_product",
                $"A spinner lands on blue with probability {n1}/{d1}. An independent second spinner lands on red with probability {n2}/{d2}. Find the probability that both events occur.",
                "The spinner outcomes are independent, so multiply their probabilities.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("n1",n1),("d1",d1),("n2",n2),("d2",d2),("power",2),("variant",slot));

        if(mode==2)
            return P("supporting.probability.independent_product",
                $"For independent events with probabilities {n1}/{d1} and {n2}/{d2}, a student adds the probabilities to find P(A and B). What is the correct probability?",
                "For an AND event on independent branches, multiply rather than add.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("n1",n1),("d1",d1),("n2",n2),("d2",d2),("power",2),("variant",slot));

        var d=denominators[r.Next(denominators.Length)];
        var n=r.Next(1,d);
        var power=slot%2==0?3:4;
        return P("supporting.probability.independent_product",
            $"An event with probability {n}/{d} is repeated independently {power} times. Find the probability that it occurs every time.",
            "Repeated independent occurrences multiply, so raise the single-event probability to the number of repetitions.",
            AssessmentItemType.ShortAnswer,
            ("mode",mode),("n1",n),("d1",d),("n2",n),("d2",d),("power",power),("variant",slot));
    }
    private static Problem ExpectedFrequency(Random r,int s){int[] den=[2,4,5,10];var d=den[r.Next(den.Length)];var n=r.Next(1,d);var trials=d*r.Next(5,15+s*5);return P("supporting.probability.expected_frequency",$"An event has probability {n}/{d} and is repeated {trials} times. Find the expected frequency.","Expected frequency=number of trials×probability.",("numerator",n),("denominator",d),("trials",trials));}
    private static Problem BinomialHalf(Random r){var n=r.Next(2,7);var variant=r.Next(1,9999);return P("supporting.probability.binomial_half",$"A fair coin is tossed {n} times. Find the probability of exactly one head as n/2^n, reduced if desired.","There are n positions for the one head among 2^n equally likely sequences.",AssessmentItemType.ShortAnswer,("n",n),("variant",variant));}
    private static Problem NormalSymmetry(Random r,int? preferredVariant=null)
    {
        var slot=QuestionVariantPolicy.NormalizeSlot(preferredVariant ?? r.Next(0,16));
        var mode=slot/4;
        int[] denominators=[4,5,8,10,20];
        var d=denominators[r.Next(denominators.Length)];
        var n=r.Next(1,Math.Max(2,d/2));

        if(mode==0)
            return P("supporting.probability.normal_symmetry",
                slot%2==0
                    ? "For a normal distribution with mean μ, find P(X>μ)."
                    : "For a normal distribution with mean μ, find P(X<μ).",
                "A normal distribution is symmetric about its mean, so half the area lies on either side of μ.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("numerator",1),("denominator",2),("variant",slot));

        if(mode==1)
            return P("supporting.probability.normal_symmetry",
                $"For a normal distribution symmetric about μ, P(X>μ+k)={n}/{d}. Find P(X<μ-k).",
                "Symmetric intervals equally far from the mean have equal tail probabilities.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("numerator",n),("denominator",d),("variant",slot));

        if(mode==2)
            return P("supporting.probability.normal_symmetry",
                $"A student claims P(X>μ)={n}/{d} for a normal distribution. What is the correct probability?",
                "Exactly half of a normal distribution lies above its mean.",
                AssessmentItemType.ShortAnswer,
                ("mode",mode),("numerator",1),("denominator",2),("claimNumerator",n),("claimDenominator",d),("variant",slot));

        return P("supporting.probability.normal_symmetry",
            $"For a normal distribution symmetric about μ, P(X<μ-k)={n}/{d}. Find P(X>μ+k).",
            "Reflect the tail across the mean: symmetric tail probabilities are equal.",
            AssessmentItemType.ShortAnswer,
            ("mode",mode),("numerator",n),("denominator",d),("variant",slot));
    }
    private static Problem PoissonMean(Random r,int s){var l=r.Next(1,20+s*5);return P("supporting.probability.poisson_mean",$"For X~Poisson({l}), find E(X).","A Poisson distribution has mean λ.",("lambda",l));}
    private static Problem UniformInterval(Random r,int s){var total=r.Next(4,10+s);var fav=r.Next(1,total);return P("supporting.probability.uniform_interval",$"A continuous uniform variable is spread evenly over an interval of length {total}. A subinterval has length {fav}. Find its probability.","For a uniform distribution, probability is favourable interval length divided by total interval length.",AssessmentItemType.ShortAnswer,("favourable",fav),("total",total));}
    private static Problem HypothesisDecision(Random r){int[] pValues=[1,2,3,4,5,6,7,8,9,12,15,20];var p=pValues[r.Next(pValues.Length)];var reject=p<5?1:0;return P("supporting.statistics.hypothesis_decision",$"At the 5% significance level, a test has p-value {(p/100.0):0.00}. Enter 'reject' or 'do not reject' for H0.","Reject H0 when p-value is less than the significance level.",AssessmentItemType.ShortAnswer,("reject",reject),("pHundredths",p),("alphaHundredths",5));}
    private static Problem DifferentialEquation(Random r,int s){var a=r.Next(1,4+s);var c=r.Next(-5-s,6+s);var x=r.Next(1,5+s);return P("supporting.calculus.differential_equation_value",$"dy/dx={2*a}x and y(0)={c}. Find y({x}).","Integrate to y=ax²+C and use the initial condition to determine C.",("a",a),("c",c),("x",x));}
    private static Problem BisectionMidpoint(Random r,int s){var left=2*r.Next(-5-s,6+s);var right=left+2*r.Next(1,4+s);return P("supporting.numerical.bisection_midpoint",$"A bisection bracket is [{left},{right}]. Find its midpoint.","Bisection tests the midpoint (left+right)/2.",("left",left),("right",right));}
    private static Problem GeometricSum(Random r,int s){var first=r.Next(1,5+s);var ratio=r.Next(2,4+s);var n=r.Next(2,5+s);return P("supporting.series.geometric_sum",$"A geometric series has first term {first}, ratio {ratio}, and {n} terms. Find the sum.","Use S_n=a(r^n-1)/(r-1) for r≠1.",("first",first),("ratio",ratio),("n",n));}
    private static Problem BinomialCoefficient(Random r,int s){var n=r.Next(3,20+s*4);return P("supporting.algebra.binomial_coefficient",$"In (1+x)^{n}, find the coefficient of x².","The x² coefficient is n choose 2=n(n-1)/2.",("n",n));}
    private static Problem ProjectileVelocity(Random r,int s){var g=10;var u=r.Next(20,40+s*5);var t=r.Next(1,Math.Max(2,u/g));return P("supporting.mechanics.projectile_velocity",$"A particle is projected vertically upward at {u} m/s. Using g={g} m/s², find its vertical velocity after {t} s.","Take upward as positive and use v=u-gt.",("u",u),("g",g),("t",t));}
    private static Problem Energy(Random r,int s){var f=r.Next(2,10+s*3);var d=r.Next(2,10+s*3);return P("supporting.mechanics.energy",$"A constant force of {f} N acts through {d} m in its direction. Find the work done in joules.","Work done=force×distance in the force direction.",("force",f),("distance",d));}
    private static Problem CircularMotion(Random r,int s){var radius=r.Next(1,5+s);var k=r.Next(2,6+s);var speed=radius*k;return P("supporting.mechanics.circular_motion",$"A particle moves at speed {speed} m/s in a circle of radius {radius} m. Find v²/r.","Centripetal acceleration magnitude is v²/r.",("speed",speed),("radius",radius));}
    private static Problem Equilibrium(Random r,int s){var f=r.Next(2,20+s*5);var variant=r.Next(1,9999);return P("supporting.mechanics.equilibrium",$"Two horizontal forces keep a particle in equilibrium. One is {f} N to the right. Find the magnitude of the leftward force.","Equilibrium requires resultant force zero.",("force",f),("variant",variant));}
    private static Problem Friction(Random r,int s){var d=10;var n=r.Next(2,10+s)*d;var mu=r.Next(1,8);return P("supporting.mechanics.friction",$"The coefficient of friction is {mu}/{d} and normal reaction is {n} N. Find limiting friction.","Limiting friction=μR.",("muNumerator",mu),("muDenominator",d),("normal",n));}
    private static Problem ConnectedParticles(Random r,int s){var m1=r.Next(1,5+s);var m2=r.Next(1,5+s);var a=r.Next(1,5+s);var force=(m1+m2)*a;return P("supporting.mechanics.connected_particles",$"Two connected particles of masses {m1} kg and {m2} kg are pulled by a net external force {force} N. Find their common acceleration.","Treat the connected system as total mass m1+m2 and use F=ma.",("m1",m1),("m2",m2),("force",force));}
    private static Problem VectorAdd(Random r,int s){var ax=NZ(r,-5-s,6+s);var ay=NZ(r,-5-s,6+s);var bx=NZ(r,-5-s,6+s);var by=NZ(r,-5-s,6+s);return P("supporting.vectors.add",$"Add vectors <{ax},{ay}> and <{bx},{by}>.","Add corresponding components.",AssessmentItemType.ShortAnswer,("ax",ax),("ay",ay),("bx",bx),("by",by));}
    private static Problem VectorMagnitude(Random r,int s){int[][] t=[[3,4,5],[5,12,13],[8,15,17],[7,24,25]];var v=t[r.Next(t.Length)];var k=r.Next(1,6+s*2);return P("supporting.vectors.magnitude",$"Find the magnitude of <{v[0]*k},{v[1]*k}>.","Use sqrt(x²+y²).",("ax",v[0]*k),("ay",v[1]*k),("magnitude",v[2]*k));}

    private static string SolveCircleSemicircle(IReadOnlyDictionary<string,int> p)
    {
        if(!p.TryGetValue("mode",out var mode))
            return "90";

        return mode switch
        {
            0 => "90",
            1 => (90-p["givenAngle"]).ToString(CultureInfo.InvariantCulture),
            2 => "90",
            _ => (90/(1+p["ratio"])).ToString(CultureInfo.InvariantCulture)
        };
    }

    private static string SolveIndependentProduct(IReadOnlyDictionary<string,int> p)
    {
        if(!p.TryGetValue("mode",out var mode))
            return "1/4";

        return mode==3
            ? $"{Pow(p["n1"],p["power"])}/{Pow(p["d1"],p["power"])}"
            : $"{p["n1"]*p["n2"]}/{p["d1"]*p["d2"]}";
    }

    private static string SolveNormalSymmetry(IReadOnlyDictionary<string,int> p)
    {
        if(!p.TryGetValue("mode",out _))
            return "1/2";

        return $"{p["numerator"]}/{p["denominator"]}";
    }

    private static Problem P(string f,string prompt,string sol,params (string,int)[] p)=>P(f,prompt,sol,AssessmentItemType.Numeric,p);
    private static Problem P(string f,string prompt,string sol,AssessmentItemType type,params (string,int)[] p)=>new(f,prompt,sol,type,p.ToDictionary(x=>x.Item1,x=>x.Item2,StringComparer.Ordinal));
    private static int Pow10(int p){var x=1;for(var i=0;i<p;i++)x*=10;return x;}
    private static int Pow2(int p){var x=1;for(var i=0;i<p;i++)x*=2;return x;}
    private static int Pow(int a,int p){var x=1;for(var i=0;i<p;i++)x=checked(x*a);return x;}
    private static int NZ(Random r,int min,int max){var x=0;while(x==0)x=r.Next(min,max);return x;}
    private static string Sign(int x)=>x>=0?$"+{x}":x.ToString(CultureInfo.InvariantCulture);
    private static string Normalize(string x)=>x.Trim().ToLowerInvariant().Replace(" ","",StringComparison.Ordinal).Replace("−","-",StringComparison.Ordinal);
    private static string NormalizePair(string x)=>Normalize(x).Replace("<","(",StringComparison.Ordinal).Replace(">",")",StringComparison.Ordinal).Replace("[","(",StringComparison.Ordinal).Replace("]",")",StringComparison.Ordinal);
    private static bool EquivalentFraction(string a,string b){return TryF(a,out var an,out var ad)&&TryF(b,out var bn,out var bd)&&an*bd==bn*ad;}
    private static bool TryF(string s,out int n,out int d){n=0;d=1;var t=s.Trim();var i=t.IndexOf('/');if(i<0)return int.TryParse(t,NumberStyles.Integer,CultureInfo.InvariantCulture,out n);return int.TryParse(t[..i].Trim(),out n)&&int.TryParse(t[(i+1)..].Trim(),out d)&&d!=0;}
}

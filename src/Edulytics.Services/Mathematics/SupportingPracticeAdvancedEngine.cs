using System.Globalization;
using Edulytics.Core.Enums;

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
            "supporting.vectors.magnitude"
        };

    internal sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) && Families.Contains(family.Trim());

    public static Problem Build(string family, Random r, int scale) =>
        family switch
        {
            "supporting.number.compare_order" => CompareOrder(r, scale),
            "supporting.number.parity" => Parity(r, scale),
            "supporting.number.bounds_upper" => BoundsUpper(r, scale),
            "supporting.number.scale_power10" => ScalePower10(r, scale),
            "supporting.fractions.mixed_to_improper" => MixedToImproper(r, scale),
            "supporting.measurement.compare" => MeasurementCompare(r, scale),
            "supporting.geometry.shape_dimension" => ShapeDimension(r),
            "supporting.geometry.turn_degrees" => TurnDegrees(r),
            "supporting.geometry.triangle_area" => TriangleArea(r, scale),
            "supporting.geometry.surface_area_cuboid" => SurfaceAreaCuboid(r, scale),
            "supporting.algebra.algebraic_fraction" => AlgebraicFraction(r, scale),
            "supporting.algebra.complete_square_vertex" => CompleteSquareVertex(r, scale),
            "supporting.algebra.inverse_proportion" => InverseProportion(r, scale),
            "supporting.functions.inverse_linear" => InverseLinear(r, scale),
            "supporting.functions.polynomial_value" => PolynomialValue(r, scale),
            "supporting.coordinate.line_intersection" => LineIntersection(r, scale),
            "supporting.circle.angle_semicircle" => CircleSemicircle(r),
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
            "supporting.probability.independent_product" => IndependentProduct(r),
            "supporting.probability.expected_frequency" => ExpectedFrequency(r, scale),
            "supporting.probability.binomial_half" => BinomialHalf(r),
            "supporting.probability.normal_symmetry" => NormalSymmetry(r),
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
            "supporting.circle.angle_semicircle" => "90",
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
            "supporting.probability.independent_product" => "1/4",
            "supporting.probability.expected_frequency" => (p["trials"] * p["numerator"] / p["denominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.probability.binomial_half" => $"{p["n"]}/{Pow2(p["n"])}",
            "supporting.probability.normal_symmetry" => "1/2",
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

    private static Problem CompareOrder(Random r,int s){var a=r.Next(0,500+s*500);var b=r.Next(0,500+s*500);return P("supporting.number.compare_order",$"Compare {a} and {b}. Enter <, >, or =.","Compare digits from the greatest place value to the least.",AssessmentItemType.ShortAnswer,("left",a),("right",b));}
    private static Problem Parity(Random r,int s){var v=r.Next(0,200+s*200);return P("supporting.number.parity",$"Is {v} odd or even?","An integer is even exactly when it is divisible by 2.",AssessmentItemType.ShortAnswer,("value",v));}
    private static Problem BoundsUpper(Random r,int s){int[] places=[10,100,1000];var place=places[r.Next(Math.Min(places.Length,s+1))];var rounded=r.Next(2,50+s*20)*place;return P("supporting.number.bounds_upper",$"{rounded} is a value rounded to the nearest {place}. Find the upper bound.","The upper bound is half one rounding interval above the rounded value.",("rounded",rounded),("place",place));}
    private static Problem ScalePower10(Random r,int s){var v=r.Next(1,50+s*20);var p=r.Next(1,Math.Min(4,s+2));return P("supporting.number.scale_power10",$"Calculate {v} × 10^{p}.","Multiplying by a power of ten shifts place value by the exponent.",("value",v),("power",p));}
    private static Problem MixedToImproper(Random r,int s){var w=r.Next(1,6+s);var d=r.Next(2,8+s);var n=r.Next(1,d);return P("supporting.fractions.mixed_to_improper",$"Convert {w} {n}/{d} to an improper fraction.","Multiply the whole part by the denominator, add the numerator, and keep the denominator.",AssessmentItemType.ShortAnswer,("whole",w),("numerator",n),("denominator",d));}
    private static Problem MeasurementCompare(Random r,int s){var a=r.Next(1,100+s*50);var b=r.Next(1,100+s*50);return P("supporting.measurement.compare",$"Compare measurements {a} and {b} expressed in the same unit. Enter <, >, or =.","Once units match, compare the numerical measures directly.",AssessmentItemType.ShortAnswer,("left",a),("right",b));}
    private static Problem ShapeDimension(Random r){var d=r.Next(0,2)==0?2:3;var variant=r.Next(1,9999);var shape=d==2?"a square":"a cube";return P("supporting.geometry.shape_dimension",$"Is {shape} a 2D or 3D shape?","2D shapes are flat; 3D shapes have solid extent.",AssessmentItemType.ShortAnswer,("dimension",d),("variant",variant));}
    private static Problem TurnDegrees(Random r){var q=r.Next(1,5);return P("supporting.geometry.turn_degrees",$"How many degrees are in {q} quarter-turn(s)?","One quarter-turn is 90 degrees.",("quarterTurns",q));}
    private static Problem TriangleArea(Random r,int s){var b=2*r.Next(2,8+s);var h=r.Next(2,8+s);return P("supporting.geometry.triangle_area",$"A triangle has base {b} and perpendicular height {h}. Find its area.","Use one half times base times perpendicular height.",("base",b),("height",h));}
    private static Problem SurfaceAreaCuboid(Random r,int s){var l=r.Next(2,7+s);var w=r.Next(2,7+s);var h=r.Next(2,6+s);return P("supporting.geometry.surface_area_cuboid",$"A cuboid has dimensions {l}, {w}, {h}. Find its total surface area.","Add the areas of the three pairs of opposite rectangular faces.",("l",l),("w",w),("h",h));}
    private static Problem AlgebraicFraction(Random r,int s){var x=r.Next(1,8+s);var d=r.Next(2,5+s);var a=d*r.Next(1,4+s);var b=d*r.Next(0,4+s);return P("supporting.algebra.algebraic_fraction",$"Evaluate ({a}x+{b})/{d} when x={x}.","Substitute x, simplify the numerator, then divide by the denominator.",("a",a),("b",b),("divisor",d),("x",x));}
    private static Problem CompleteSquareVertex(Random r,int s){var h=r.Next(-5-s,6+s);var k=r.Next(-8-s,9+s);return P("supporting.algebra.complete_square_vertex",$"The quadratic is written as (x-({h}))²+{k}. Find the x-coordinate of its vertex.","In vertex form (x-h)²+k, the vertex has x-coordinate h.",("h",h),("k",k));}
    private static Problem InverseProportion(Random r,int s){var x=r.Next(2,8+s);var y=r.Next(2,8+s);var k=x*y;return P("supporting.algebra.inverse_proportion",$"y is inversely proportional to x with constant k={k}. Find y when x={x}.","For inverse proportion y=k/x.",("k",k),("x",x));}
    private static Problem InverseLinear(Random r,int s){var a=r.Next(2,5+s);var x=r.Next(-5-s,6+s);var b=r.Next(-8-s,9+s);var y=a*x+b;return P("supporting.functions.inverse_linear",$"For f(x)={a}x{Sign(b)}, find f⁻¹({y}).","Set y=ax+b and solve for x.",("a",a),("b",b),("y",y));}
    private static Problem PolynomialValue(Random r,int s){var a=r.Next(1,4+s);var b=r.Next(-4-s,5+s);var c=r.Next(-6-s,7+s);var x=r.Next(-3-s,4+s);return P("supporting.functions.polynomial_value",$"For p(x)={a}x²{Sign(b)}x{Sign(c)}, find p({x}).","Substitute the input and apply powers before multiplication and addition.",("a",a),("b",b),("c",c),("x",x));}
    private static Problem LineIntersection(Random r,int s){var x=r.Next(-5-s,6+s);var m1=r.Next(1,4+s);var m2=m1+r.Next(1,4+s);var b1=r.Next(-6-s,7+s);var y=m1*x+b1;var b2=y-m2*x;return P("supporting.coordinate.line_intersection",$"Lines y={m1}x{Sign(b1)} and y={m2}x{Sign(b2)} intersect at x=k. Find k.","At an intersection the y-values are equal; solve the resulting linear equation.",("x",x),("m1",m1),("m2",m2),("b1",b1),("b2",b2));}
    private static Problem CircleSemicircle(Random r){var variant=r.Next(1,10000);return P("supporting.circle.angle_semicircle","An angle at the circumference subtends a diameter. Find the angle in degrees.","An angle in a semicircle is a right angle.",("variant",variant));}
    private static Problem Bearing(Random r){int[] b=[30,45,60,90,120,135,180,225,270,315];var v=b[r.Next(b.Length)];return P("supporting.trigonometry.bearing",$"A direction is {v}° clockwise from north. Write the three-figure bearing.","Bearings are measured clockwise from north and written with three digits.",AssessmentItemType.ShortAnswer,("bearing",v));}
    private static Problem SineRule(Random r,int s){var baseSide=2*r.Next(2,6+s);var target=baseSide*2;return P("supporting.trigonometry.sine_rule_exact",$"Using a/sin A=b/sin B, let a={baseSide}, sin A=1/2 and sin B=1. Find b.","Rearrange b=a·sinB/sinA and substitute the exact sine values.",("baseSide",baseSide),("targetSide",target));}
    private static Problem CosineRule(Random r,int s){var b=r.Next(2,7+s);var c=r.Next(2,7+s);var bc=b*c;return P("supporting.trigonometry.cosine_rule_square",$"For sides b={b}, c={c} with included angle 60°, use a²=b²+c²-2bc cos60°. Find a².","Since cos60°=1/2, the final term is bc.",("b",b),("c",c),("bcTerm",bc));}
    private static Problem AreaSine(Random r,int s){var a=2*r.Next(2,7+s);var b=r.Next(2,7+s);return P("supporting.trigonometry.area_sine_double",$"Two sides are {a} and {b} with included angle 30°. Find twice the triangle area.","Area=1/2 ab sinC, so twice the area is ab sinC and sin30°=1/2.",("a",a),("b",b),("sinNumerator",1),("sinDenominator",2));}
    private static Problem TrigGraph(Random r){int[] deg=[0,30,90,150,180];var d=deg[r.Next(deg.Length)];var n=d==0||d==180?0:d==30||d==150?1:2;var den=n==2?2:2;return P("supporting.trigonometry.graph_special",$"For the sine graph, find sin({d}°) as an exact fraction.","Use the standard unit-circle values for special angles.",AssessmentItemType.ShortAnswer,("angle",d),("valueNumerator",n),("valueDenominator",den));}
    private static Problem UnionCount(Random r,int s){var inter=r.Next(1,5+s);var a=inter+r.Next(1,8+s);var b=inter+r.Next(1,8+s);return P("supporting.sets.union_count",$"Set A has {a} elements, B has {b}, and A∩B has {inter}. Find |A∪B|.","Use |A∪B|=|A|+|B|-|A∩B|.",("a",a),("b",b),("intersection",inter));}
    private static Problem GroupedMean(Random r,int s){var m1=2*r.Next(1,5+s);var m2=m1+2*r.Next(1,4+s);var f1=r.Next(1,5+s);var f2=r.Next(1,5+s);while((m1*f1+m2*f2)%(f1+f2)!=0)f2=r.Next(1,5+s);return P("supporting.statistics.grouped_mean",$"Grouped midpoints {m1},{m2} have frequencies {f1},{f2}. Find the estimated mean.","Multiply each midpoint by its frequency, add, then divide by total frequency.",("m1",m1),("m2",m2),("f1",f1),("f2",f2));}
    private static Problem CumulativeTotal(Random r,int s){var a=r.Next(1,10+s*2);var b=r.Next(1,10+s*2);var c=r.Next(1,10+s*2);return P("supporting.statistics.cumulative_total",$"Class frequencies are {a}, {b}, {c}. Find the final cumulative frequency.","The final cumulative frequency is the total frequency.",("f1",a),("f2",b),("f3",c));}
    private static Problem HistogramDensity(Random r,int s){var width=r.Next(1,5+s);var density=r.Next(1,8+s);var freq=width*density;return P("supporting.statistics.histogram_density",$"A histogram class has width {width} and frequency {freq}. Find frequency density.","Frequency density=frequency/class width.",("width",width),("frequency",freq));}
    private static Problem Quartile(Random r,int s){var q=r.Next(2,20+s*5);var variant=r.Next(1,9999);return P("supporting.statistics.quartile",$"An ordered data summary states the lower quartile is {q}. What is Q1?","Q1 is another name for the lower quartile.",("q",q),("variant",variant));}
    private static Problem IndependentProduct(Random r){var variant=r.Next(1,10000);return P("supporting.probability.independent_product","Two independent fair coins are tossed. Find P(head then head).","Multiply independent branch probabilities: 1/2×1/2=1/4.",AssessmentItemType.ShortAnswer,("variant",variant));}
    private static Problem ExpectedFrequency(Random r,int s){int[] den=[2,4,5,10];var d=den[r.Next(den.Length)];var n=r.Next(1,d);var trials=d*r.Next(5,15+s*5);return P("supporting.probability.expected_frequency",$"An event has probability {n}/{d} and is repeated {trials} times. Find the expected frequency.","Expected frequency=number of trials×probability.",("numerator",n),("denominator",d),("trials",trials));}
    private static Problem BinomialHalf(Random r){var n=r.Next(2,7);var variant=r.Next(1,9999);return P("supporting.probability.binomial_half",$"A fair coin is tossed {n} times. Find the probability of exactly one head as n/2^n, reduced if desired.","There are n positions for the one head among 2^n equally likely sequences.",AssessmentItemType.ShortAnswer,("n",n),("variant",variant));}
    private static Problem NormalSymmetry(Random r){var variant=r.Next(1,9999);return P("supporting.probability.normal_symmetry","For a normal distribution with mean μ, find P(X>μ).","A normal distribution is symmetric about its mean, so half the area lies above μ.",AssessmentItemType.ShortAnswer,("variant",variant));}
    private static Problem PoissonMean(Random r,int s){var l=r.Next(1,8+s);return P("supporting.probability.poisson_mean",$"For X~Poisson({l}), find E(X).","A Poisson distribution has mean λ.",("lambda",l));}
    private static Problem UniformInterval(Random r,int s){var total=r.Next(4,10+s);var fav=r.Next(1,total);return P("supporting.probability.uniform_interval",$"A continuous uniform variable is spread evenly over an interval of length {total}. A subinterval has length {fav}. Find its probability.","For a uniform distribution, probability is favourable interval length divided by total interval length.",AssessmentItemType.ShortAnswer,("favourable",fav),("total",total));}
    private static Problem HypothesisDecision(Random r){var reject=r.Next(0,2);var p=reject==1?3:8;return P("supporting.statistics.hypothesis_decision",$"At the 5% significance level, a test has p-value 0.0{p}. Enter 'reject' or 'do not reject' for H0.","Reject H0 when p-value is less than the significance level.",AssessmentItemType.ShortAnswer,("reject",reject),("pHundredths",p));}
    private static Problem DifferentialEquation(Random r,int s){var a=r.Next(1,4+s);var c=r.Next(-5-s,6+s);var x=r.Next(1,5+s);return P("supporting.calculus.differential_equation_value",$"dy/dx={2*a}x and y(0)={c}. Find y({x}).","Integrate to y=ax²+C and use the initial condition to determine C.",("a",a),("c",c),("x",x));}
    private static Problem BisectionMidpoint(Random r,int s){var left=2*r.Next(-5-s,6+s);var right=left+2*r.Next(1,4+s);return P("supporting.numerical.bisection_midpoint",$"A bisection bracket is [{left},{right}]. Find its midpoint.","Bisection tests the midpoint (left+right)/2.",("left",left),("right",right));}
    private static Problem GeometricSum(Random r,int s){var first=r.Next(1,5+s);var ratio=r.Next(2,4+s);var n=r.Next(2,5+s);return P("supporting.series.geometric_sum",$"A geometric series has first term {first}, ratio {ratio}, and {n} terms. Find the sum.","Use S_n=a(r^n-1)/(r-1) for r≠1.",("first",first),("ratio",ratio),("n",n));}
    private static Problem BinomialCoefficient(Random r,int s){var n=r.Next(3,8+s);return P("supporting.algebra.binomial_coefficient",$"In (1+x)^{n}, find the coefficient of x².","The x² coefficient is n choose 2=n(n-1)/2.",("n",n));}
    private static Problem ProjectileVelocity(Random r,int s){var g=10;var u=r.Next(20,40+s*5);var t=r.Next(1,Math.Max(2,u/g));return P("supporting.mechanics.projectile_velocity",$"A particle is projected vertically upward at {u} m/s. Using g={g} m/s², find its vertical velocity after {t} s.","Take upward as positive and use v=u-gt.",("u",u),("g",g),("t",t));}
    private static Problem Energy(Random r,int s){var f=r.Next(2,10+s*3);var d=r.Next(2,10+s*3);return P("supporting.mechanics.energy",$"A constant force of {f} N acts through {d} m in its direction. Find the work done in joules.","Work done=force×distance in the force direction.",("force",f),("distance",d));}
    private static Problem CircularMotion(Random r,int s){var radius=r.Next(1,5+s);var k=r.Next(2,6+s);var speed=radius*k;return P("supporting.mechanics.circular_motion",$"A particle moves at speed {speed} m/s in a circle of radius {radius} m. Find v²/r.","Centripetal acceleration magnitude is v²/r.",("speed",speed),("radius",radius));}
    private static Problem Equilibrium(Random r,int s){var f=r.Next(2,20+s*5);var variant=r.Next(1,9999);return P("supporting.mechanics.equilibrium",$"Two horizontal forces keep a particle in equilibrium. One is {f} N to the right. Find the magnitude of the leftward force.","Equilibrium requires resultant force zero.",("force",f),("variant",variant));}
    private static Problem Friction(Random r,int s){var d=10;var n=r.Next(2,10+s)*d;var mu=r.Next(1,8);return P("supporting.mechanics.friction",$"The coefficient of friction is {mu}/{d} and normal reaction is {n} N. Find limiting friction.","Limiting friction=μR.",("muNumerator",mu),("muDenominator",d),("normal",n));}
    private static Problem ConnectedParticles(Random r,int s){var m1=r.Next(1,5+s);var m2=r.Next(1,5+s);var a=r.Next(1,5+s);var force=(m1+m2)*a;return P("supporting.mechanics.connected_particles",$"Two connected particles of masses {m1} kg and {m2} kg are pulled by a net external force {force} N. Find their common acceleration.","Treat the connected system as total mass m1+m2 and use F=ma.",("m1",m1),("m2",m2),("force",force));}
    private static Problem VectorAdd(Random r,int s){var ax=NZ(r,-5-s,6+s);var ay=NZ(r,-5-s,6+s);var bx=NZ(r,-5-s,6+s);var by=NZ(r,-5-s,6+s);return P("supporting.vectors.add",$"Add vectors <{ax},{ay}> and <{bx},{by}>.","Add corresponding components.",AssessmentItemType.ShortAnswer,("ax",ax),("ay",ay),("bx",bx),("by",by));}
    private static Problem VectorMagnitude(Random r,int s){int[][] t=[[3,4,5],[5,12,13],[8,15,17]];var v=t[r.Next(t.Length)];var k=r.Next(1,2+s);return P("supporting.vectors.magnitude",$"Find the magnitude of <{v[0]*k},{v[1]*k}>.","Use sqrt(x²+y²).",("ax",v[0]*k),("ay",v[1]*k),("magnitude",v[2]*k));}

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

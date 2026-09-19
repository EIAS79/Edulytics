using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Deterministic exact question families used only by reviewed Supporting-lesson
/// completion rules. The rule registry owns curriculum alignment; this engine
/// owns parameter generation, solving and independent verification.
/// Answers are never stored in generation parameters.
/// </summary>
internal static class SupportingPracticeCompletionEngine
{
    private static readonly IReadOnlySet<string> Families =
        new HashSet<string>(StringComparer.Ordinal)
        {
            "supporting.number.place_value",
            "supporting.number.rounding",
            "supporting.number.negative_operation",
            "supporting.number.order_operations",
            "supporting.number.gcf",
            "supporting.number.prime_factor",
            "supporting.number.multiply",
            "supporting.number.divide",
            "supporting.number.lcm",
            "supporting.decimals.place_value",
            "supporting.decimals.operation",
            "supporting.decimals.compare",
            "supporting.decimals.round",
            "supporting.fractions.multiply",
            "supporting.fractions.divide_whole",
            "supporting.fractions.simplify",
            "supporting.fdp.percent_equivalent",
            "supporting.measurement.unit_conversion",
            "supporting.measurement.time_elapsed",
            "supporting.measurement.money_change",
            "supporting.algebra.substitute",
            "supporting.algebra.expand",
            "supporting.algebra.factor",
            "supporting.algebra.simplify",
            "supporting.algebra.simultaneous",
            "supporting.algebra.quadratic_larger_root",
            "supporting.functions.evaluate",
            "supporting.functions.composite",
            "supporting.indices.power_or_root",
            "supporting.standard_form.power10_exponent",
            "supporting.surds.coefficient",
            "supporting.geometry.angle_around_point",
            "supporting.geometry.angle_classify",
            "supporting.geometry.parallelogram_area",
            "supporting.geometry.compound_area",
            "supporting.geometry.cuboid_volume",
            "supporting.geometry.translate_point",
            "supporting.geometry.reflect_axis",
            "supporting.geometry.rotate90",
            "supporting.geometry.midpoint",
            "supporting.geometry.axis_distance",
            "supporting.geometry.circle_circumference_pi_coefficient",
            "supporting.geometry.circle_area_pi_coefficient",
            "supporting.geometry.locus_equidistant",
            "supporting.statistics.table_total",
            "supporting.statistics.pie_sector_angle",
            "supporting.statistics.scatter_correlation",
            "supporting.statistics.experimental_probability",
            "supporting.probability.sample_space_count",
            "supporting.statistics.compare_range",
            "supporting.statistics.collection_method",
            "supporting.reasoning.multistep",
            "supporting.reasoning.verify_identity",
            "supporting.calculus.derivative_value",
            "supporting.calculus.definite_integral_linear",
            "supporting.matrices.determinant2",
            "supporting.complex.add_real_part",
            "supporting.complex.multiply_real_part",
            "supporting.mechanics.constant_acceleration_velocity",
            "supporting.probability.conditional_simple",
            "supporting.exponentials.solve_exponent",
            "supporting.logarithms.evaluate",
            "supporting.combinatorics.choose_two",
            "supporting.probability_statistics.mixed",
            "supporting.geometry.plane.mixed",
            "supporting.geometry.solid.mixed",
            "supporting.geometry.analytic.mixed",
            "supporting.algebra.equations.mixed",
            "supporting.algebra.expressions.mixed",
            "supporting.functions.core.mixed",
            "supporting.sequences.core.mixed",
            "supporting.trigonometry.core.mixed",
            "supporting.number.real.mixed",
            "supporting.calculus.optimization",
            "supporting.transformations.mixed",
            "supporting.statistics.charts.mixed"
        };

    internal sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) &&
        (Families.Contains(family.Trim()) ||
         SupportingPracticeAdvancedEngine.Supports(family.Trim()));

    public static Problem Build(string family, Random random, int scale)
    {
        if (SupportingPracticeAdvancedEngine.Supports(family))
        {
            var advanced = SupportingPracticeAdvancedEngine.Build(family, random, scale);
            return new Problem(
                advanced.Family,
                advanced.Prompt,
                advanced.Solution,
                advanced.ItemType,
                advanced.Parameters);
        }

        return family switch
        {
            "supporting.number.place_value" => PlaceValue(random, scale),
            "supporting.number.rounding" => Rounding(random, scale),
            "supporting.number.negative_operation" => NegativeOperation(random, scale),
            "supporting.number.order_operations" => OrderOperations(random, scale),
            "supporting.number.gcf" => GreatestCommonFactor(random, scale),
            "supporting.number.prime_factor" => LargestPrimeFactor(random, scale),
            "supporting.number.multiply" => WholeMultiply(random, scale),
            "supporting.number.divide" => WholeDivide(random, scale),
            "supporting.number.lcm" => LeastCommonMultipleProblem(random, scale),
            "supporting.decimals.place_value" => DecimalPlaceValue(random, scale),
            "supporting.decimals.operation" => DecimalOperation(random, scale),
            "supporting.decimals.compare" => DecimalCompare(random, scale),
            "supporting.decimals.round" => DecimalRound(random, scale),
            "supporting.fractions.multiply" => FractionMultiply(random, scale),
            "supporting.fractions.divide_whole" => FractionDivideWhole(random, scale),
            "supporting.fractions.simplify" => FractionSimplify(random, scale),
            "supporting.fdp.percent_equivalent" => FractionDecimalPercent(random, scale),
            "supporting.measurement.unit_conversion" => UnitConversion(random, scale),
            "supporting.measurement.time_elapsed" => TimeElapsed(random, scale),
            "supporting.measurement.money_change" => MoneyChange(random, scale),
            "supporting.algebra.substitute" => AlgebraSubstitute(random, scale),
            "supporting.algebra.expand" => AlgebraExpand(random, scale),
            "supporting.algebra.factor" => AlgebraFactor(random, scale),
            "supporting.algebra.simplify" => AlgebraSimplify(random, scale),
            "supporting.algebra.simultaneous" => Simultaneous(random, scale),
            "supporting.algebra.quadratic_larger_root" => QuadraticRoot(random, scale),
            "supporting.functions.evaluate" => FunctionEvaluate(random, scale),
            "supporting.functions.composite" => FunctionComposite(random, scale),
            "supporting.indices.power_or_root" => PowerOrRoot(random, scale),
            "supporting.standard_form.power10_exponent" => StandardFormExponent(random, scale),
            "supporting.surds.coefficient" => SurdCoefficient(random, scale),
            "supporting.geometry.angle_around_point" => AngleAroundPoint(random),
            "supporting.geometry.angle_classify" => AngleClassify(random),
            "supporting.geometry.parallelogram_area" => ParallelogramArea(random, scale),
            "supporting.geometry.compound_area" => CompoundArea(random, scale),
            "supporting.geometry.cuboid_volume" => CuboidVolume(random, scale),
            "supporting.geometry.translate_point" => TranslatePoint(random, scale),
            "supporting.geometry.reflect_axis" => ReflectPoint(random, scale),
            "supporting.geometry.rotate90" => RotatePoint(random, scale),
            "supporting.geometry.midpoint" => Midpoint(random, scale),
            "supporting.geometry.axis_distance" => AxisDistance(random, scale),
            "supporting.geometry.circle_circumference_pi_coefficient" => CircleCircumference(random, scale),
            "supporting.geometry.circle_area_pi_coefficient" => CircleArea(random, scale),
            "supporting.geometry.locus_equidistant" => LocusEquidistant(random),
            "supporting.statistics.table_total" => TableTotal(random, scale),
            "supporting.statistics.pie_sector_angle" => PieSector(random, scale),
            "supporting.statistics.scatter_correlation" => ScatterCorrelation(random),
            "supporting.statistics.experimental_probability" => ExperimentalProbability(random, scale),
            "supporting.probability.sample_space_count" => SampleSpaceCount(random, scale),
            "supporting.statistics.compare_range" => CompareRange(random, scale),
            "supporting.statistics.collection_method" => CollectionMethod(random),
            "supporting.reasoning.multistep" => MultiStep(random, scale),
            "supporting.reasoning.verify_identity" => VerifyIdentity(random, scale),
            "supporting.calculus.derivative_value" => DerivativeValue(random, scale),
            "supporting.calculus.definite_integral_linear" => DefiniteIntegralLinear(random, scale),
            "supporting.matrices.determinant2" => MatrixDeterminant(random, scale),
            "supporting.complex.add_real_part" => ComplexAddReal(random, scale),
            "supporting.complex.multiply_real_part" => ComplexMultiplyReal(random, scale),
            "supporting.mechanics.constant_acceleration_velocity" => MechanicsVelocity(random, scale),
            "supporting.probability.conditional_simple" => ConditionalProbability(random, scale),
            "supporting.exponentials.solve_exponent" => ExponentialSolve(random, scale),
            "supporting.logarithms.evaluate" => LogarithmEvaluate(random, scale),
            "supporting.combinatorics.choose_two" => ChooseTwo(random, scale),
            "supporting.probability_statistics.mixed" => ProbabilityStatisticsMixed(random, scale),
            "supporting.geometry.plane.mixed" => PlaneGeometryMixed(random, scale),
            "supporting.geometry.solid.mixed" => SolidGeometryMixed(random, scale),
            "supporting.geometry.analytic.mixed" => AnalyticGeometryMixed(random, scale),
            "supporting.algebra.equations.mixed" => AlgebraEquationsMixed(random, scale),
            "supporting.algebra.expressions.mixed" => AlgebraExpressionsMixed(random, scale),
            "supporting.functions.core.mixed" => FunctionsMixed(random, scale),
            "supporting.sequences.core.mixed" => SequencesMixed(random, scale),
            "supporting.trigonometry.core.mixed" => TrigonometryMixed(random, scale),
            "supporting.number.real.mixed" => RealNumberMixed(random, scale),
            "supporting.calculus.optimization" => Optimization(random, scale),
            "supporting.transformations.mixed" => TransformationsMixed(random, scale),
            "supporting.statistics.charts.mixed" => ChartsMixed(random, scale),
            _ => throw new InvalidOperationException($"Unsupported Supporting completion family: {family}")
        };
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (SupportingPracticeAdvancedEngine.Supports(family))
            return SupportingPracticeAdvancedEngine.Solve(family, p);

        return family switch
        {
            "supporting.number.place_value" =>
                (p["digit"] * Pow10(p["power"])).ToString(CultureInfo.InvariantCulture),
            "supporting.number.rounding" =>
                RoundTo(p["value"], p["place"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.negative_operation" =>
                (p["left"] + p["right"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.order_operations" =>
                (p["a"] + p["b"] * p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.gcf" =>
                Gcd(p["left"], p["right"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.prime_factor" =>
                Math.Max(p["p"], p["q"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.multiply" =>
                (p["left"] * p["right"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.divide" =>
                (p["dividend"] / p["divisor"]).ToString(CultureInfo.InvariantCulture),
            "supporting.number.lcm" =>
                Lcm(p["left"], p["right"]).ToString(CultureInfo.InvariantCulture),
            "supporting.decimals.place_value" =>
                p["digit"].ToString(CultureInfo.InvariantCulture),
            "supporting.decimals.operation" =>
                FormatHundredths(p["leftHundredths"] + p["rightHundredths"]),
            "supporting.decimals.compare" =>
                Compare(p["leftHundredths"], p["rightHundredths"]),
            "supporting.decimals.round" =>
                (p["hundredths"] >= 50 ? p["whole"] + 1 : p["whole"]).ToString(CultureInfo.InvariantCulture),
            "supporting.fractions.multiply" =>
                SimplifyFraction(p["n1"] * p["n2"], p["d1"] * p["d2"]),
            "supporting.fractions.divide_whole" =>
                SimplifyFraction(p["n"], p["d"] * p["whole"]),
            "supporting.fractions.simplify" =>
                SimplifyFraction(p["n"], p["d"]),
            "supporting.fdp.percent_equivalent" =>
                (p["numerator"] * 100 / p["denominator"]).ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.unit_conversion" =>
                (p["value"] * p["factor"]).ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.time_elapsed" =>
                (p["end"] - p["start"]).ToString(CultureInfo.InvariantCulture),
            "supporting.measurement.money_change" =>
                (p["paid"] - p["cost"]).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.substitute" =>
                (p["a"] * p["x"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.expand" =>
                $"{p["a"]}x{Signed(p["a"] * p["b"])}",
            "supporting.algebra.factor" =>
                $"{p["g"]}(x{Signed(p["b"])})",
            "supporting.algebra.simplify" =>
                $"{p["a"] + p["b"]}x{Signed(p["c"])}",
            "supporting.algebra.simultaneous" =>
                FormatPair(p["x"], p["y"]),
            "supporting.algebra.quadratic_larger_root" =>
                Math.Max(p["r1"], p["r2"]).ToString(CultureInfo.InvariantCulture),
            "supporting.functions.evaluate" =>
                (p["a"] * p["x"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "supporting.functions.composite" =>
                (p["gA"] * (p["fA"] * p["x"] + p["fB"]) + p["gB"]).ToString(CultureInfo.InvariantCulture),
            "supporting.indices.power_or_root" =>
                (p["mode"] == 0 ? IntPow(p["base"], p["exponent"]) : p["root"]).ToString(CultureInfo.InvariantCulture),
            "supporting.standard_form.power10_exponent" =>
                p["exponent"].ToString(CultureInfo.InvariantCulture),
            "supporting.surds.coefficient" =>
                p["coefficient"].ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.angle_around_point" =>
                (360 - p["a"] - p["b"] - p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.angle_classify" =>
                ClassifyAngle(p["angle"]),
            "supporting.geometry.parallelogram_area" =>
                (p["base"] * p["height"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.compound_area" =>
                (p["l1"] * p["w1"] + p["l2"] * p["w2"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.cuboid_volume" =>
                (p["length"] * p["width"] * p["height"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.translate_point" =>
                FormatPair(p["x"] + p["dx"], p["y"] + p["dy"]),
            "supporting.geometry.reflect_axis" =>
                p["axis"] == 0 ? FormatPair(p["x"], -p["y"]) : FormatPair(-p["x"], p["y"]),
            "supporting.geometry.rotate90" =>
                p["direction"] == 1 ? FormatPair(-p["y"], p["x"]) : FormatPair(p["y"], -p["x"]),
            "supporting.geometry.midpoint" =>
                FormatPair((p["x1"] + p["x2"]) / 2, (p["y1"] + p["y2"]) / 2),
            "supporting.geometry.axis_distance" =>
                Math.Abs(p["end"] - p["start"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.circle_circumference_pi_coefficient" =>
                (2 * p["radius"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.circle_area_pi_coefficient" =>
                (p["radius"] * p["radius"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.locus_equidistant" =>
                "perpendicular bisector",
            "supporting.statistics.table_total" =>
                (p["f1"] + p["f2"] + p["f3"] + p["f4"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.pie_sector_angle" =>
                (360 * p["part"] / p["total"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.scatter_correlation" =>
                p["kind"] switch { 0 => "positive", 1 => "negative", _ => "none" },
            "supporting.statistics.experimental_probability" =>
                SimplifyFraction(p["successes"], p["trials"]),
            "supporting.probability.sample_space_count" =>
                (p["firstOutcomes"] * p["secondOutcomes"]).ToString(CultureInfo.InvariantCulture),
            "supporting.statistics.compare_range" =>
                CompareLabel(p["rangeA"], p["rangeB"]),
            "supporting.statistics.collection_method" =>
                p["method"] == 0 ? "census" : "sample",
            "supporting.reasoning.multistep" =>
                p["x"].ToString(CultureInfo.InvariantCulture),
            "supporting.reasoning.verify_identity" =>
                p["isIdentity"] == 1 ? "true" : "false",
            "supporting.calculus.derivative_value" =>
                (2 * p["a"] * p["x"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "supporting.calculus.definite_integral_linear" =>
                (p["k"] * p["upper"] * p["upper"] / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.matrices.determinant2" =>
                (p["a"] * p["d"] - p["b"] * p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.complex.add_real_part" =>
                (p["a"] + p["c"]).ToString(CultureInfo.InvariantCulture),
            "supporting.complex.multiply_real_part" =>
                (p["a"] * p["c"] - p["b"] * p["d"]).ToString(CultureInfo.InvariantCulture),
            "supporting.mechanics.constant_acceleration_velocity" =>
                (p["u"] + p["a"] * p["t"]).ToString(CultureInfo.InvariantCulture),
            "supporting.probability.conditional_simple" =>
                SimplifyFraction(p["intersection"], p["given"]),
            "supporting.exponentials.solve_exponent" =>
                p["exponent"].ToString(CultureInfo.InvariantCulture),
            "supporting.logarithms.evaluate" =>
                p["exponent"].ToString(CultureInfo.InvariantCulture),
            "supporting.combinatorics.choose_two" =>
                (p["n"] * (p["n"] - 1) / 2).ToString(CultureInfo.InvariantCulture),
            "supporting.probability_statistics.mixed" =>
                p["mode"] == 0
                    ? SimplifyFraction(p["favourable"], p["total"])
                    : (p["v1"] + p["v2"] + p["v3"]) / 3 + "",
            "supporting.geometry.plane.mixed" =>
                p["mode"] == 0
                    ? (p["base"] * p["height"] / 2).ToString(CultureInfo.InvariantCulture)
                    : (180 - p["angleA"] - p["angleB"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.solid.mixed" =>
                (p["length"] * p["width"] * p["height"]).ToString(CultureInfo.InvariantCulture),
            "supporting.geometry.analytic.mixed" =>
                p["mode"] == 0
                    ? FormatPair((p["x1"] + p["x2"]) / 2, (p["y1"] + p["y2"]) / 2)
                    : Math.Abs(p["end"] - p["start"]).ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.equations.mixed" =>
                p["x"].ToString(CultureInfo.InvariantCulture),
            "supporting.algebra.expressions.mixed" =>
                (p["a"] * p["x"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "supporting.functions.core.mixed" =>
                (p["a"] * p["x"] + p["b"]).ToString(CultureInfo.InvariantCulture),
            "supporting.sequences.core.mixed" =>
                (p["first"] + (p["n"] - 1) * p["difference"]).ToString(CultureInfo.InvariantCulture),
            "supporting.trigonometry.core.mixed" =>
                SimplifyFraction(p["opposite"], p["hypotenuse"]),
            "supporting.number.real.mixed" =>
                p["mode"] == 0
                    ? Gcd(p["left"], p["right"]).ToString(CultureInfo.InvariantCulture)
                    : RoundTo(p["value"], p["place"]).ToString(CultureInfo.InvariantCulture),
            "supporting.calculus.optimization" =>
                p["vertexX"].ToString(CultureInfo.InvariantCulture),
            "supporting.transformations.mixed" =>
                FormatPair(p["x"] + p["dx"], p["y"] + p["dy"]),
            "supporting.statistics.charts.mixed" =>
                (p["f1"] + p["f2"] + p["f3"] + p["f4"]).ToString(CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported Supporting completion solver family: {family}")
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string answer)
    {
        if (SupportingPracticeAdvancedEngine.Supports(family))
            return SupportingPracticeAdvancedEngine.Verify(family, p, answer);

        if (!Supports(family))
            return false;

        var expected = Solve(family, p);
        if (family is
            "supporting.fractions.multiply" or
            "supporting.fractions.divide_whole" or
            "supporting.fractions.simplify" or
            "supporting.statistics.experimental_probability" or
            "supporting.probability.conditional_simple" or
            "supporting.probability_statistics.mixed" or
            "supporting.trigonometry.core.mixed")
        {
            return EquivalentFraction(answer, expected);
        }

        if (family is
            "supporting.geometry.translate_point" or
            "supporting.geometry.reflect_axis" or
            "supporting.geometry.rotate90" or
            "supporting.geometry.midpoint" or
            "supporting.algebra.simultaneous" or
            "supporting.geometry.analytic.mixed" or
            "supporting.transformations.mixed")
        {
            return EquivalentPair(answer, expected);
        }

        if (family is
            "supporting.decimals.operation")
        {
            return decimal.TryParse(answer, NumberStyles.Number, CultureInfo.InvariantCulture, out var actual) &&
                decimal.TryParse(expected, NumberStyles.Number, CultureInfo.InvariantCulture, out var expectedDecimal) &&
                actual == expectedDecimal;
        }

        if (family is
            "supporting.algebra.expand" or
            "supporting.algebra.factor" or
            "supporting.algebra.simplify")
        {
            return NormalizeExpression(answer) == NormalizeExpression(expected);
        }

        return string.Equals(
            NormalizeText(answer),
            NormalizeText(expected),
            StringComparison.Ordinal);
    }

    private static Problem PlaceValue(Random r, int s)
    {
        var power = r.Next(0, Math.Min(6, 2 + s * 2));
        var digit = r.Next(1, 10);
        var lower = r.Next(0, Math.Max(1, Pow10(power)));
        var upper = r.Next(1, 20 + s * 20) * Pow10(power + 1);
        var number = upper + digit * Pow10(power) + lower;
        return P("supporting.number.place_value",
            $"In the number {number}, what is the value of the digit {digit} in the 10^{power} place?",
            "A digit's value is digit × place value. Identify the correct power of ten, then multiply.",
            ("digit", digit), ("power", power), ("number", number));
    }

    private static Problem Rounding(Random r, int s)
    {
        int[] places = [10, 100, 1000];
        var place = places[r.Next(Math.Min(places.Length, s + 1))];
        var quotient = r.Next(2, 50 + s * 50);
        var remainder = r.Next(0, place);
        if (remainder == place / 2) remainder++;
        var value = quotient * place + remainder;
        return P("supporting.number.rounding",
            $"Round {value} to the nearest {place}.",
            "Locate the two multiples of the rounding place on either side and use the halfway point to choose the nearer multiple.",
            ("value", value), ("place", place));
    }

    private static Problem NegativeOperation(Random r, int s)
    {
        var left = r.Next(-15 * s, 16 * s);
        var right = NonZero(r, -12 * s, 13 * s);
        return P("supporting.number.negative_operation",
            $"Calculate {left} + ({right}).",
            "Treat directed numbers on a number line or combine signs carefully, then check by reversing the addition.",
            ("left", left), ("right", right));
    }

    private static Problem OrderOperations(Random r, int s)
    {
        var a = r.Next(1, 8 * s + 4);
        var b = r.Next(2, 6 * s + 3);
        var c = r.Next(2, 6 * s + 3);
        return P("supporting.number.order_operations",
            $"Calculate {a} + {b} × {c}.",
            "Apply multiplication before addition, then check each operation separately.",
            ("a", a), ("b", b), ("c", c));
    }

    private static Problem GreatestCommonFactor(Random r, int s)
    {
        var g = r.Next(2, 5 + s * 2);
        var a = g * r.Next(2, 7 + s);
        var b = g * r.Next(2, 7 + s);
        return P("supporting.number.gcf",
            $"Find the greatest common factor of {a} and {b}.",
            "List common factors or use prime factorisation. The answer must divide both numbers and no larger common factor may do so.",
            ("left", a), ("right", b));
    }

    private static Problem LargestPrimeFactor(Random r, int s)
    {
        int[] primes = [2, 3, 5, 7, 11];
        var p = primes[r.Next(Math.Min(primes.Length, 2 + s))];
        var q = primes[r.Next(Math.Min(primes.Length, 3 + s))];
        var n = p * q;
        return P("supporting.number.prime_factor",
            $"The number {n} is a product of two prime factors. What is its largest prime factor?",
            "Factor the number into primes and select the larger prime factor; multiply the factors to verify the original number.",
            ("p", p), ("q", q), ("number", n));
    }

    private static Problem WholeMultiply(Random r, int s)
    {
        var left = r.Next(2, 20 + s * 30);
        var right = r.Next(2, 10 + s * 8);
        return P("supporting.number.multiply",
            $"Calculate {left} × {right}.",
            "Use place-value partitioning or a formal multiplication algorithm, then verify with division.",
            ("left", left), ("right", right));
    }

    private static Problem WholeDivide(Random r, int s)
    {
        var divisor = r.Next(2, 10 + s * 4);
        var quotient = r.Next(2, 20 + s * 20);
        var dividend = divisor * quotient;
        return P("supporting.number.divide",
            $"Calculate {dividend} ÷ {divisor}.",
            "Use a formal division method or known multiplication facts, then verify divisor × quotient = dividend.",
            ("dividend", dividend), ("divisor", divisor));
    }

    private static Problem LeastCommonMultipleProblem(Random r, int s)
    {
        var a = r.Next(2, 7 + s);
        var b = r.Next(2, 7 + s);
        return P("supporting.number.lcm",
            $"Find the least common multiple of {a} and {b}.",
            "List multiples or use prime factors. The answer must be divisible by both numbers and be the smallest positive such number.",
            ("left", a), ("right", b));
    }

    private static Problem DecimalPlaceValue(Random r, int s)
    {
        var whole = r.Next(0, 50 + s * 10);
        var tenths = r.Next(0, 10);
        var hundredths = r.Next(1, 10);
        return P("supporting.decimals.place_value",
            $"In {whole}.{tenths}{hundredths}, what digit is in the hundredths place?",
            "The first digit after the decimal point is tenths and the second is hundredths.",
            ("whole", whole), ("tenths", tenths), ("digit", hundredths));
    }

    private static Problem DecimalOperation(Random r, int s)
    {
        var a = r.Next(10, 300 + s * 150);
        var b = r.Next(10, 200 + s * 100);
        return P("supporting.decimals.operation",
            $"Calculate {FormatHundredths(a)} + {FormatHundredths(b)}.",
            "Align decimal places, add hundredths exactly, then place the decimal point two places from the right.",
            ("leftHundredths", a), ("rightHundredths", b));
    }

    private static Problem DecimalCompare(Random r, int s)
    {
        var a = r.Next(1, 500 + s * 100);
        var b = r.Next(1, 500 + s * 100);
        return P("supporting.decimals.compare",
            $"Compare {FormatHundredths(a)} and {FormatHundredths(b)}. Enter <, >, or =.",
            "Write the decimals with the same number of decimal places and compare corresponding place values from left to right.",
            ("leftHundredths", a), ("rightHundredths", b));
    }

    private static Problem DecimalRound(Random r, int s)
    {
        var whole = r.Next(0, 50 + s * 20);
        var hundredths = r.Next(0, 100);
        return P("supporting.decimals.round",
            $"Round {whole}.{hundredths:00} to the nearest whole number.",
            "Look at the tenths digit. Five or more rounds the whole-number part up; otherwise keep it.",
            ("whole", whole), ("hundredths", hundredths));
    }

    private static Problem FractionMultiply(Random r, int s)
    {
        var d1 = r.Next(2, 7 + s);
        var d2 = r.Next(2, 7 + s);
        var n1 = r.Next(1, d1);
        var n2 = r.Next(1, d2);
        return P("supporting.fractions.multiply",
            $"Calculate {n1}/{d1} × {n2}/{d2}. Give the answer in simplest form.",
            "Multiply numerators, multiply denominators, then divide both by their greatest common factor.",
            ("n1", n1), ("d1", d1), ("n2", n2), ("d2", d2));
    }

    private static Problem FractionDivideWhole(Random r, int s)
    {
        var d = r.Next(2, 7 + s);
        var n = r.Next(1, d);
        var whole = r.Next(2, 5 + s);
        return P("supporting.fractions.divide_whole",
            $"Calculate {n}/{d} ÷ {whole}. Give the answer in simplest form.",
            "Dividing by a whole number is multiplying by its reciprocal: multiply the denominator by the whole number and simplify.",
            ("n", n), ("d", d), ("whole", whole));
    }

    private static Problem FractionSimplify(Random r, int s)
    {
        var g = r.Next(2, 5 + s);
        var d0 = r.Next(3, 8 + s);
        var n0 = r.Next(1, d0);
        return P("supporting.fractions.simplify",
            $"Simplify {n0 * g}/{d0 * g}.",
            "Divide numerator and denominator by their greatest common factor and verify the value is unchanged.",
            ("n", n0 * g), ("d", d0 * g));
    }

    private static Problem FractionDecimalPercent(Random r, int s)
    {
        int[] denominators = [2, 4, 5, 10, 20, 25, 50, 100];
        var d = denominators[r.Next(denominators.Length)];
        var n = r.Next(1, d);
        while ((n * 100) % d != 0)
            n = r.Next(1, d);
        return P("supporting.fdp.percent_equivalent",
            $"What percentage is equivalent to the fraction {n}/{d}?",
            "Convert the fraction to an equivalent fraction over 100 or multiply its decimal value by 100.",
            ("numerator", n), ("denominator", d));
    }

    private static Problem UnitConversion(Random r, int s)
    {
        int[] factors = [10, 100, 1000];
        var factor = factors[r.Next(Math.Min(factors.Length, s + 1))];
        var value = r.Next(2, 30 + s * 20);
        return P("supporting.measurement.unit_conversion",
            $"A measurement is {value} larger units, with {factor} smaller units in each larger unit. How many smaller units is that?",
            "Multiply by the conversion factor and check the direction of conversion.",
            ("value", value), ("factor", factor));
    }

    private static Problem TimeElapsed(Random r, int s)
    {
        var start = r.Next(6 * 60, 18 * 60);
        var duration = r.Next(10, 60 + s * 40);
        var end = start + duration;
        return P("supporting.measurement.time_elapsed",
            $"A journey starts at {Clock(start)} and ends at {Clock(end)}. How many minutes does it take?",
            "Convert both times to minutes after midnight or bridge through the hour, then subtract start from end.",
            ("start", start), ("end", end));
    }

    private static Problem MoneyChange(Random r, int s)
    {
        var cost = r.Next(150, 2000 + s * 1000);
        var paid = ((cost + 499) / 500) * 500;
        if (paid == cost) paid += 500;
        return P("supporting.measurement.money_change",
            $"An item costs {cost} cents and you pay {paid} cents. How many cents change should you receive?",
            "Subtract the cost from the amount paid and verify cost + change equals the amount paid.",
            ("cost", cost), ("paid", paid));
    }

    private static Problem AlgebraSubstitute(Random r, int s)
    {
        var a = NonZero(r, -5 - s, 6 + s);
        var b = r.Next(-10 * s, 10 * s + 1);
        var x = r.Next(-5 * s, 5 * s + 1);
        return P("supporting.algebra.substitute",
            $"Evaluate {a}x {Signed(b)} when x = {x}.",
            "Replace x with the given value, apply multiplication first, then combine the constant.",
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem AlgebraExpand(Random r, int s)
    {
        var a = r.Next(2, 5 + s);
        var b = NonZero(r, -6 - s, 7 + s);
        return P("supporting.algebra.expand",
            $"Expand {a}(x {Signed(b)}).",
            "Multiply every term inside the bracket by the outside factor, preserving the sign of the constant.",
            ("a", a), ("b", b));
    }

    private static Problem AlgebraFactor(Random r, int s)
    {
        var g = r.Next(2, 5 + s);
        var b = NonZero(r, -6 - s, 7 + s);
        return P("supporting.algebra.factor",
            $"Factorise {g}x {Signed(g * b)} completely.",
            "Take out the greatest common numerical factor from both terms and check by expanding.",
            ("g", g), ("b", b));
    }

    private static Problem AlgebraSimplify(Random r, int s)
    {
        var a = NonZero(r, -5 - s, 6 + s);
        var b = NonZero(r, -5 - s, 6 + s);
        var c = r.Next(-10 * s, 10 * s + 1);
        return P("supporting.algebra.simplify",
            $"Simplify {a}x {Signed(b)}x {Signed(c)}.",
            "Collect like x-terms by adding their coefficients; keep the constant term unchanged.",
            ("a", a), ("b", b), ("c", c));
    }

    private static Problem Simultaneous(Random r, int s)
    {
        var x = r.Next(1, 8 + s * 3);
        var y = r.Next(1, 8 + s * 3);
        var total = x + y;
        var difference = x - y;
        return P("supporting.algebra.simultaneous",
            $"Solve the system x + y = {total} and x − y = {difference}. Give the answer as (x, y).",
            "Add the equations to eliminate y, solve for x, then substitute to find y and check both original equations.",
            ("x", x), ("y", y), ("total", total), ("difference", difference));
    }

    private static Problem QuadraticRoot(Random r, int s)
    {
        var r1 = NonZero(r, -6 - s, 7 + s);
        var r2 = NonZero(r, -6 - s, 7 + s);
        while (r2 == r1) r2 = NonZero(r, -6 - s, 7 + s);
        var sum = r1 + r2;
        var product = r1 * r2;
        return P("supporting.algebra.quadratic_larger_root",
            $"The equation x² − ({sum})x + ({product}) = 0 has two integer roots. Find the larger root.",
            "Factor the quadratic using two numbers whose sum is the x-coefficient sum and whose product is the constant, then identify the larger root.",
            ("r1", r1), ("r2", r2), ("sum", sum), ("product", product));
    }

    private static Problem FunctionEvaluate(Random r, int s)
    {
        var a = NonZero(r, -5 - s, 6 + s);
        var b = r.Next(-10 * s, 10 * s + 1);
        var x = r.Next(-5 * s, 5 * s + 1);
        return P("supporting.functions.evaluate",
            $"If f(x) = {a}x {Signed(b)}, find f({x}).",
            "Substitute the input into the function rule and simplify exactly.",
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem FunctionComposite(Random r, int s)
    {
        var fA = NonZero(r, -4 - s, 5 + s);
        var fB = r.Next(-6 * s, 6 * s + 1);
        var gA = NonZero(r, -4 - s, 5 + s);
        var gB = r.Next(-6 * s, 6 * s + 1);
        var x = r.Next(-4 * s, 4 * s + 1);
        return P("supporting.functions.composite",
            $"Let f(x) = {fA}x {Signed(fB)} and g(x) = {gA}x {Signed(gB)}. Find g(f({x})).",
            "Evaluate the inner function first, then use that result as the input to the outer function.",
            ("fA", fA), ("fB", fB), ("gA", gA), ("gB", gB), ("x", x));
    }

    private static Problem PowerOrRoot(Random r, int s)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var @base = r.Next(2, 5 + s);
            var exponent = r.Next(2, Math.Min(5, 2 + s) + 1);
            return P("supporting.indices.power_or_root",
                $"Evaluate {@base}^{exponent}.",
                "Multiply the base by itself the stated number of times and verify by repeated multiplication.",
                ("mode", mode), ("base", @base), ("exponent", exponent), ("root", 0));
        }

        var root = r.Next(2, 8 + s * 2);
        return P("supporting.indices.power_or_root",
            $"Evaluate √{root * root}.",
            "Find the positive number whose square equals the radicand.",
            ("mode", mode), ("base", 0), ("exponent", 0), ("root", root));
    }

    private static Problem StandardFormExponent(Random r, int s)
    {
        var exponent = r.Next(2, 4 + s * 2);
        var coefficient = r.Next(1, 10);
        var number = coefficient * Pow10(exponent);
        return P("supporting.standard_form.power10_exponent",
            $"{number} = {coefficient} × 10^n. Find n.",
            "Count how many powers of ten scale the coefficient to the original number.",
            ("coefficient", coefficient), ("exponent", exponent), ("number", number));
    }

    private static Problem SurdCoefficient(Random r, int s)
    {
        var coefficient = r.Next(2, 5 + s);
        int[] squareFree = [2, 3, 5, 6, 7];
        var radicandBase = squareFree[r.Next(squareFree.Length)];
        var radicand = coefficient * coefficient * radicandBase;
        return P("supporting.surds.coefficient",
            $"Simplify √{radicand} = k√{radicandBase}. Find k.",
            "Factor out the largest perfect-square factor from the radicand and take its positive square root outside the radical.",
            ("coefficient", coefficient), ("radicandBase", radicandBase), ("radicand", radicand));
    }

    private static Problem AngleAroundPoint(Random r)
    {
        var a = r.Next(40, 111);
        var b = r.Next(40, 111);
        var c = r.Next(40, 111);
        while (a + b + c >= 340)
        {
            a = r.Next(30, 91);
            b = r.Next(30, 91);
            c = r.Next(30, 91);
        }
        return P("supporting.geometry.angle_around_point",
            $"Three angles around a point are {a}°, {b}° and {c}°. Find the remaining angle.",
            "Angles around a point total 360°. Subtract the known angles and check the complete turn.",
            ("a", a), ("b", b), ("c", c));
    }

    private static Problem AngleClassify(Random r)
    {
        int[] choices = [25, 45, 90, 120, 175, 225];
        var angle = choices[r.Next(choices.Length)];
        return P("supporting.geometry.angle_classify",
            $"Classify an angle of {angle}° as acute, right, obtuse, straight, or reflex.",
            "Compare the angle with 90°, 180° and 360° and choose the standard classification.",
            AssessmentItemType.ShortAnswer,
            ("angle", angle));
    }

    private static Problem ParallelogramArea(Random r, int s)
    {
        var b = r.Next(3, 10 + s * 3);
        var h = r.Next(2, 8 + s * 2);
        return P("supporting.geometry.parallelogram_area",
            $"A parallelogram has base {b} and perpendicular height {h}. Find its area.",
            "Use area = base × perpendicular height, not the sloping side length.",
            ("base", b), ("height", h));
    }

    private static Problem CompoundArea(Random r, int s)
    {
        var l1 = r.Next(2, 8 + s * 2);
        var w1 = r.Next(2, 7 + s * 2);
        var l2 = r.Next(2, 8 + s * 2);
        var w2 = r.Next(2, 7 + s * 2);
        return P("supporting.geometry.compound_area",
            $"A compound shape is split into non-overlapping rectangles {l1}×{w1} and {l2}×{w2}. Find the total area.",
            "Find each rectangle's area separately and add them because the parts do not overlap.",
            ("l1", l1), ("w1", w1), ("l2", l2), ("w2", w2));
    }

    private static Problem CuboidVolume(Random r, int s)
    {
        var l = r.Next(2, 8 + s * 2);
        var w = r.Next(2, 7 + s * 2);
        var h = r.Next(2, 6 + s * 2);
        return P("supporting.geometry.cuboid_volume",
            $"A cuboid has length {l}, width {w} and height {h}. Find its volume.",
            "Volume of a cuboid is length × width × height. Verify the answer uses cubic units.",
            ("length", l), ("width", w), ("height", h));
    }

    private static Problem TranslatePoint(Random r, int s)
    {
        var x = r.Next(-6 * s, 6 * s + 1);
        var y = r.Next(-6 * s, 6 * s + 1);
        var dx = NonZero(r, -5 - s, 6 + s);
        var dy = NonZero(r, -5 - s, 6 + s);
        return P("supporting.geometry.translate_point",
            $"Translate the point ({x}, {y}) by vector <{dx}, {dy}>. Give the image as (x, y).",
            "Add the translation vector componentwise to the original coordinates.",
            AssessmentItemType.ShortAnswer,
            ("x", x), ("y", y), ("dx", dx), ("dy", dy));
    }

    private static Problem ReflectPoint(Random r, int s)
    {
        var x = NonZero(r, -6 * s, 6 * s + 1);
        var y = NonZero(r, -6 * s, 6 * s + 1);
        var axis = r.Next(0, 2);
        return P("supporting.geometry.reflect_axis",
            $"Reflect ({x}, {y}) in the {(axis == 0 ? "x-axis" : "y-axis")}. Give the image as (x, y).",
            "Reflection in the x-axis changes the sign of y; reflection in the y-axis changes the sign of x.",
            AssessmentItemType.ShortAnswer,
            ("x", x), ("y", y), ("axis", axis));
    }

    private static Problem RotatePoint(Random r, int s)
    {
        var x = NonZero(r, -5 * s, 5 * s + 1);
        var y = NonZero(r, -5 * s, 5 * s + 1);
        var direction = r.Next(0, 2) == 0 ? 1 : -1;
        return P("supporting.geometry.rotate90",
            $"Rotate ({x}, {y}) by 90° {(direction == 1 ? "anticlockwise" : "clockwise")} about the origin. Give the image as (x, y).",
            "For 90° anticlockwise use (x,y)→(−y,x); for clockwise use (x,y)→(y,−x).",
            AssessmentItemType.ShortAnswer,
            ("x", x), ("y", y), ("direction", direction));
    }

    private static Problem Midpoint(Random r, int s)
    {
        var x1 = r.Next(-5 * s, 5 * s + 1) * 2;
        var y1 = r.Next(-5 * s, 5 * s + 1) * 2;
        var x2 = r.Next(-5 * s, 5 * s + 1) * 2;
        var y2 = r.Next(-5 * s, 5 * s + 1) * 2;
        return P("supporting.geometry.midpoint",
            $"Find the midpoint of ({x1}, {y1}) and ({x2}, {y2}).",
            "Average the x-coordinates and average the y-coordinates, then check the midpoint lies halfway in both directions.",
            AssessmentItemType.ShortAnswer,
            ("x1", x1), ("y1", y1), ("x2", x2), ("y2", y2));
    }

    private static Problem AxisDistance(Random r, int s)
    {
        var start = r.Next(-10 * s, 10 * s + 1);
        var end = r.Next(-10 * s, 10 * s + 1);
        return P("supporting.geometry.axis_distance",
            $"Two points lie on a horizontal line at x={start} and x={end}. Find the distance between them.",
            "Distance on one axis is the absolute difference between the coordinates.",
            ("start", start), ("end", end));
    }

    private static Problem CircleCircumference(Random r, int s)
    {
        var radius = r.Next(2, 8 + s * 2);
        return P("supporting.geometry.circle_circumference_pi_coefficient",
            $"A circle has radius {radius}. Its circumference is kπ. Find k.",
            "Use C = 2πr and report the exact coefficient of π.",
            ("radius", radius));
    }

    private static Problem CircleArea(Random r, int s)
    {
        var radius = r.Next(2, 8 + s * 2);
        return P("supporting.geometry.circle_area_pi_coefficient",
            $"A circle has radius {radius}. Its area is kπ. Find k.",
            "Use A = πr² and report the exact coefficient of π.",
            ("radius", radius));
    }

    private static Problem LocusEquidistant(Random r)
    {
        var variant = r.Next(1, 1000);
        return P("supporting.geometry.locus_equidistant",
            "What is the locus of points that are exactly the same distance from two fixed points A and B?",
            "Points equidistant from A and B lie on the perpendicular bisector of segment AB.",
            AssessmentItemType.ShortAnswer,
            ("variant", variant));
    }

    private static Problem TableTotal(Random r, int s)
    {
        var f1 = r.Next(1, 10 + s * 3);
        var f2 = r.Next(1, 10 + s * 3);
        var f3 = r.Next(1, 10 + s * 3);
        var f4 = r.Next(1, 10 + s * 3);
        return P("supporting.statistics.table_total",
            $"A frequency table has counts {f1}, {f2}, {f3}, {f4}. Find the total frequency.",
            "Add all category frequencies and check every row has been included exactly once.",
            ("f1", f1), ("f2", f2), ("f3", f3), ("f4", f4));
    }

    private static Problem PieSector(Random r, int s)
    {
        int[] totals = [12, 18, 20, 24, 30, 36];
        var total = totals[r.Next(totals.Length)];
        var divisors = Enumerable.Range(1, total - 1).Where(x => (360 * x) % total == 0).ToArray();
        var part = divisors[r.Next(divisors.Length)];
        return P("supporting.statistics.pie_sector_angle",
            $"A category has frequency {part} out of {total}. Find its pie-chart sector angle.",
            "The sector is the same fraction of 360° as the category is of the total.",
            ("part", part), ("total", total));
    }

    private static Problem ScatterCorrelation(Random r)
    {
        var kind = r.Next(0, 3);
        var description = kind switch
        {
            0 => "as x increases, y generally increases",
            1 => "as x increases, y generally decreases",
            _ => "there is no clear upward or downward trend"
        };
        return P("supporting.statistics.scatter_correlation",
            $"A scatter graph shows that {description}. State the correlation: positive, negative, or none.",
            "Correlation describes the direction of the overall trend, not whether every point follows it.",
            AssessmentItemType.ShortAnswer,
            ("kind", kind));
    }

    private static Problem ExperimentalProbability(Random r, int s)
    {
        var trials = r.Next(20, 50 + s * 30);
        var successes = r.Next(1, trials);
        return P("supporting.statistics.experimental_probability",
            $"An event occurred {successes} times in {trials} trials. Estimate its probability as a simplified fraction.",
            "Experimental probability is successes divided by trials. Simplify the fraction and check it lies between 0 and 1.",
            AssessmentItemType.ShortAnswer,
            ("successes", successes), ("trials", trials));
    }

    private static Problem SampleSpaceCount(Random r, int s)
    {
        var a = r.Next(2, 5 + s);
        var b = r.Next(2, 5 + s);
        return P("supporting.probability.sample_space_count",
            $"Experiment A has {a} possible outcomes and independent experiment B has {b}. How many ordered outcome pairs are in the combined sample space?",
            "Use the product rule: each outcome of A can pair with every outcome of B.",
            ("firstOutcomes", a), ("secondOutcomes", b));
    }

    private static Problem CompareRange(Random r, int s)
    {
        var rangeA = r.Next(2, 10 + s * 4);
        var rangeB = r.Next(2, 10 + s * 4);
        return P("supporting.statistics.compare_range",
            $"Distribution A has range {rangeA}; distribution B has range {rangeB}. Which has the greater range? Enter A, B, or equal.",
            "Compare the numerical spreads directly. A larger range means greater max-minus-min spread.",
            AssessmentItemType.ShortAnswer,
            ("rangeA", rangeA), ("rangeB", rangeB));
    }

    private static Problem CollectionMethod(Random r)
    {
        var method = r.Next(0, 2);
        var prompt = method == 0
            ? "A school wants information from every student enrolled this year. Is this a census or a sample?"
            : "A school surveys 80 students chosen from 800 students. Is this a census or a sample?";
        return P("supporting.statistics.collection_method",
            prompt,
            "A census includes the entire population; a sample includes only part of the population.",
            AssessmentItemType.ShortAnswer,
            ("method", method));
    }

    private static Problem MultiStep(Random r, int s)
    {
        var x = r.Next(2, 10 + s * 3);
        var a = r.Next(2, 5 + s);
        var b = r.Next(1, 8 + s * 2);
        var total = a * x + b;
        return P("supporting.reasoning.multistep",
            $"A quantity x is multiplied by {a} and then {b} is added to give {total}. Find x.",
            "Translate the steps into ax+b=total, undo addition, then undo multiplication and substitute back to check.",
            ("x", x), ("a", a), ("b", b), ("total", total));
    }

    private static Problem VerifyIdentity(Random r, int s)
    {
        var a = r.Next(2, 6 + s);
        var b = r.Next(1, 7 + s);
        var isIdentity = r.Next(0, 2);
        var rightConstant = a * b + (isIdentity == 1 ? 0 : 1);
        return P("supporting.reasoning.verify_identity",
            $"Is {a}(x+{b}) = {a}x+{rightConstant} true for every x? Enter true or false.",
            "Expand the left-hand side and compare coefficients and constants. An identity must match for every x.",
            AssessmentItemType.ShortAnswer,
            ("a", a), ("b", b), ("isIdentity", isIdentity), ("rightConstant", rightConstant));
    }

    private static Problem DerivativeValue(Random r, int s)
    {
        var a = NonZero(r, -4 - s, 5 + s);
        var b = r.Next(-6 * s, 6 * s + 1);
        var x = r.Next(-4 * s, 4 * s + 1);
        return P("supporting.calculus.derivative_value",
            $"For f(x)={a}x²{Signed(b)}x, find f'({x}).",
            "Differentiate term by term: f'(x)=2ax+b, then substitute the requested x-value.",
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem DefiniteIntegralLinear(Random r, int s)
    {
        var k = 2 * r.Next(1, 4 + s);
        var upper = r.Next(2, 5 + s);
        return P("supporting.calculus.definite_integral_linear",
            $"Evaluate the definite integral of {k}x from x=0 to x={upper}.",
            "An antiderivative is (k/2)x². Evaluate at the upper and lower bounds and subtract.",
            ("k", k), ("upper", upper));
    }

    private static Problem MatrixDeterminant(Random r, int s)
    {
        var a = NonZero(r, -5 - s, 6 + s);
        var b = r.Next(-5 - s, 6 + s);
        var c = r.Next(-5 - s, 6 + s);
        var d = NonZero(r, -5 - s, 6 + s);
        return P("supporting.matrices.determinant2",
            $"Find det([[{a},{b}],[{c},{d}]]).",
            "For a 2×2 matrix [[a,b],[c,d]], determinant = ad−bc.",
            ("a", a), ("b", b), ("c", c), ("d", d));
    }

    private static Problem ComplexAddReal(Random r, int s)
    {
        var a = r.Next(-6 * s, 6 * s + 1);
        var b = r.Next(-6 * s, 6 * s + 1);
        var c = r.Next(-6 * s, 6 * s + 1);
        var d = r.Next(-6 * s, 6 * s + 1);
        return P("supporting.complex.add_real_part",
            $"Find the real part of ({a}{Imag(b)}) + ({c}{Imag(d)}).",
            "Add real parts together and imaginary parts together. The question asks only for the real component.",
            ("a", a), ("b", b), ("c", c), ("d", d));
    }

    private static Problem ComplexMultiplyReal(Random r, int s)
    {
        var a = r.Next(-5 * s, 5 * s + 1);
        var b = r.Next(-5 * s, 5 * s + 1);
        var c = r.Next(-5 * s, 5 * s + 1);
        var d = r.Next(-5 * s, 5 * s + 1);
        return P("supporting.complex.multiply_real_part",
            $"Find the real part of ({a}{Imag(b)})({c}{Imag(d)}).",
            "Expand and use i²=−1. The real part is ac−bd.",
            ("a", a), ("b", b), ("c", c), ("d", d));
    }

    private static Problem MechanicsVelocity(Random r, int s)
    {
        var u = r.Next(0, 10 + s * 5);
        var a = NonZero(r, -4 - s, 5 + s);
        var t = r.Next(1, 6 + s * 2);
        return P("supporting.mechanics.constant_acceleration_velocity",
            $"A particle has initial velocity {u} m/s and constant acceleration {a} m/s² for {t} s. Find the final velocity in m/s.",
            "Use v=u+at and keep the signed acceleration.",
            ("u", u), ("a", a), ("t", t));
    }

    private static Problem ConditionalProbability(Random r, int s)
    {
        var given = r.Next(3, 10 + s * 3);
        var intersection = r.Next(1, given + 1);
        return P("supporting.probability.conditional_simple",
            $"Among {given} outcomes where B occurred, {intersection} also have A. Find P(A|B) as a simplified fraction.",
            "Conditional probability restricts the sample space to B, so divide the A∩B count by the B count.",
            AssessmentItemType.ShortAnswer,
            ("intersection", intersection), ("given", given));
    }

    private static Problem ExponentialSolve(Random r, int s)
    {
        var @base = r.Next(2, 5 + s);
        var exponent = r.Next(1, 4 + s);
        var target = IntPow(@base, exponent);
        return P("supporting.exponentials.solve_exponent",
            $"Solve {@base}^x = {target}.",
            "Express the target as a power of the same base; the matching exponent is x.",
            ("base", @base), ("exponent", exponent), ("target", target));
    }

    private static Problem LogarithmEvaluate(Random r, int s)
    {
        var @base = r.Next(2, 5 + s);
        var exponent = r.Next(1, 4 + s);
        var argument = IntPow(@base, exponent);
        return P("supporting.logarithms.evaluate",
            $"Evaluate log base {@base} of {argument}.",
            "A logarithm asks for the exponent: find x such that base^x equals the argument.",
            ("base", @base), ("exponent", exponent), ("argument", argument));
    }

    private static Problem ChooseTwo(Random r, int s)
    {
        var n = r.Next(4, 8 + s * 3);
        return P("supporting.combinatorics.choose_two",
            $"How many unordered pairs can be chosen from {n} distinct objects?",
            "Use n choose 2 = n(n−1)/2 because each pair is counted twice by ordered selection.",
            ("n", n));
    }

    private static Problem ProbabilityStatisticsMixed(Random r, int s)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var total = r.Next(4, 10 + s * 2);
            var favourable = r.Next(1, total);
            return P("supporting.probability_statistics.mixed",
                $"There are {favourable} favourable outcomes out of {total} equally likely outcomes. Find the probability.",
                "Use favourable outcomes divided by total outcomes and simplify.",
                AssessmentItemType.ShortAnswer,
                ("mode", mode), ("favourable", favourable), ("total", total),
                ("v1", 0), ("v2", 0), ("v3", 0));
        }

        var mean = r.Next(3, 12 + s * 3);
        var d = r.Next(1, 4 + s);
        return P("supporting.probability_statistics.mixed",
            $"Find the mean of {mean-d}, {mean}, {mean+d}.",
            "Add the three values and divide by 3.",
            ("mode", mode), ("favourable", 0), ("total", 1),
            ("v1", mean-d), ("v2", mean), ("v3", mean+d));
    }

    private static Problem PlaneGeometryMixed(Random r, int s)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var b = 2 * r.Next(2, 8 + s);
            var h = r.Next(2, 8 + s);
            return P("supporting.geometry.plane.mixed",
                $"A triangle has base {b} and perpendicular height {h}. Find its area.",
                "Use one half times base times perpendicular height.",
                ("mode", mode), ("base", b), ("height", h), ("angleA", 0), ("angleB", 0));
        }

        var a = r.Next(30, 100);
        var bAngle = r.Next(30, 140 - a);
        return P("supporting.geometry.plane.mixed",
            $"Two angles of a triangle are {a}° and {bAngle}°. Find the third.",
            "Interior angles of a triangle total 180°.",
            ("mode", mode), ("base", 0), ("height", 0), ("angleA", a), ("angleB", bAngle));
    }

    private static Problem SolidGeometryMixed(Random r, int s)
    {
        var l = r.Next(2, 8 + s);
        var w = r.Next(2, 7 + s);
        var h = r.Next(2, 6 + s);
        return P("supporting.geometry.solid.mixed",
            $"A rectangular prism is {l} by {w} by {h}. Find its volume.",
            "Multiply the three perpendicular dimensions.",
            ("length", l), ("width", w), ("height", h));
    }

    private static Problem AnalyticGeometryMixed(Random r, int s)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var x1 = 2 * r.Next(-5 * s, 5 * s + 1);
            var y1 = 2 * r.Next(-5 * s, 5 * s + 1);
            var x2 = 2 * r.Next(-5 * s, 5 * s + 1);
            var y2 = 2 * r.Next(-5 * s, 5 * s + 1);
            return P("supporting.geometry.analytic.mixed",
                $"Find the midpoint of ({x1},{y1}) and ({x2},{y2}).",
                "Average x-coordinates and y-coordinates separately.",
                AssessmentItemType.ShortAnswer,
                ("mode", mode), ("x1", x1), ("y1", y1), ("x2", x2), ("y2", y2),
                ("start", 0), ("end", 0));
        }

        var start = r.Next(-10 * s, 10 * s + 1);
        var end = r.Next(-10 * s, 10 * s + 1);
        return P("supporting.geometry.analytic.mixed",
            $"Find the horizontal distance between x={start} and x={end}.",
            "Use the absolute difference of the coordinates.",
            ("mode", mode), ("x1", 0), ("y1", 0), ("x2", 0), ("y2", 0),
            ("start", start), ("end", end));
    }

    private static Problem AlgebraEquationsMixed(Random r, int s)
    {
        var x = r.Next(1, 10 + s * 3);
        var a = r.Next(2, 5 + s);
        var b = r.Next(-8 * s, 8 * s + 1);
        var right = a * x + b;
        return P("supporting.algebra.equations.mixed",
            $"Solve {a}x {Signed(b)} = {right}.",
            "Undo the constant term, divide by the coefficient and substitute back to verify.",
            ("x", x), ("a", a), ("b", b), ("right", right));
    }

    private static Problem AlgebraExpressionsMixed(Random r, int s)
    {
        var a = NonZero(r, -5 - s, 6 + s);
        var b = r.Next(-8 * s, 8 * s + 1);
        var x = r.Next(-4 * s, 4 * s + 1);
        return P("supporting.algebra.expressions.mixed",
            $"Evaluate {a}x {Signed(b)} when x={x}.",
            "Substitute the value and simplify using the correct order of operations.",
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem FunctionsMixed(Random r, int s)
    {
        var a = NonZero(r, -4 - s, 5 + s);
        var b = r.Next(-8 * s, 8 * s + 1);
        var x = r.Next(-5 * s, 5 * s + 1);
        return P("supporting.functions.core.mixed",
            $"For f(x)={a}x{Signed(b)}, find f({x}).",
            "Substitute the input into the function rule.",
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem SequencesMixed(Random r, int s)
    {
        var first = r.Next(-5 * s, 8 * s + 1);
        var difference = NonZero(r, -4 - s, 5 + s);
        var n = r.Next(4, 10 + s * 4);
        return P("supporting.sequences.core.mixed",
            $"An arithmetic sequence starts {first}, {first+difference}, {first+2*difference}, ... Find term {n}.",
            "Use a_n=a_1+(n−1)d and check against the constant difference.",
            ("first", first), ("difference", difference), ("n", n));
    }

    private static Problem TrigonometryMixed(Random r, int s)
    {
        int[][] triples = [[3,4,5],[5,12,13],[8,15,17]];
        var t = triples[r.Next(triples.Length)];
        var k = r.Next(1, 2 + s);
        var opposite = t[0] * k;
        var hypotenuse = t[2] * k;
        return P("supporting.trigonometry.core.mixed",
            $"In a right triangle, the side opposite θ is {opposite} and the hypotenuse is {hypotenuse}. Find sin θ as a simplified fraction.",
            "Use sin θ = opposite/hypotenuse and simplify.",
            AssessmentItemType.ShortAnswer,
            ("opposite", opposite), ("hypotenuse", hypotenuse));
    }

    private static Problem RealNumberMixed(Random r, int s)
    {
        var mode = r.Next(0, 2);
        if (mode == 0)
        {
            var g = r.Next(2, 5 + s);
            var a = g * r.Next(2, 8 + s);
            var b = g * r.Next(2, 8 + s);
            return P("supporting.number.real.mixed",
                $"Find the greatest common factor of {a} and {b}.",
                "Use factors or prime factorisation and verify the answer divides both numbers.",
                ("mode", mode), ("left", a), ("right", b), ("value", 0), ("place", 10));
        }

        var place = 100;
        var value = r.Next(10, 500 + s * 100) * 10 + r.Next(0, 10);
        return P("supporting.number.real.mixed",
            $"Round {value} to the nearest {place}.",
            "Compare with the midpoint between adjacent multiples of the rounding place.",
            ("mode", mode), ("left", 1), ("right", 1), ("value", value), ("place", place));
    }

    private static Problem Optimization(Random r, int s)
    {
        var vertexX = r.Next(-5 * s, 6 * s);
        var a = r.Next(1, 4 + s);
        return P("supporting.calculus.optimization",
            $"The quadratic f(x)={a}(x−({vertexX}))²+3 has its minimum at x=k. Find k.",
            "A quadratic in vertex form a(x−h)²+c with a>0 has its minimum at x=h.",
            ("vertexX", vertexX), ("a", a));
    }

    private static Problem TransformationsMixed(Random r, int s)
    {
        var x = r.Next(-5 * s, 6 * s);
        var y = r.Next(-5 * s, 6 * s);
        var dx = NonZero(r, -4 - s, 5 + s);
        var dy = NonZero(r, -4 - s, 5 + s);
        return P("supporting.transformations.mixed",
            $"Translate ({x},{y}) by vector <{dx},{dy}>.",
            "Add the translation vector componentwise.",
            AssessmentItemType.ShortAnswer,
            ("x", x), ("y", y), ("dx", dx), ("dy", dy));
    }

    private static Problem ChartsMixed(Random r, int s)
    {
        var f1 = r.Next(1, 10 + s * 2);
        var f2 = r.Next(1, 10 + s * 2);
        var f3 = r.Next(1, 10 + s * 2);
        var f4 = r.Next(1, 10 + s * 2);
        return P("supporting.statistics.charts.mixed",
            $"A chart represents category frequencies {f1}, {f2}, {f3}, {f4}. Find the total frequency.",
            "Add all represented category counts once.",
            ("f1", f1), ("f2", f2), ("f3", f3), ("f4", f4));
    }

    private static Problem P(
        string family,
        string prompt,
        string solution,
        params (string Name, int Value)[] p) =>
        P(family, prompt, solution, AssessmentItemType.Numeric, p);

    private static Problem P(
        string family,
        string prompt,
        string solution,
        AssessmentItemType type,
        params (string Name, int Value)[] p) =>
        new(family, prompt, solution, type, p.ToDictionary(x => x.Name, x => x.Value, StringComparer.Ordinal));

    private static int NonZero(Random r, int minInclusive, int maxExclusive)
    {
        var value = 0;
        while (value == 0)
            value = r.Next(minInclusive, maxExclusive);
        return value;
    }

    private static int Pow10(int power)
    {
        var result = 1;
        for (var i = 0; i < power; i++) result = checked(result * 10);
        return result;
    }

    private static int IntPow(int value, int exponent)
    {
        var result = 1;
        for (var i = 0; i < exponent; i++) result = checked(result * value);
        return result;
    }

    private static int RoundTo(int value, int place)
    {
        var remainder = value % place;
        var lower = value - remainder;
        return remainder * 2 < place ? lower : lower + place;
    }

    private static int Lcm(int a, int b) =>
        checked(Math.Abs(a / Gcd(a, b) * b));

    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0) (a, b) = (b, a % b);
        return Math.Max(1, a);
    }

    private static string SimplifyFraction(int n, int d)
    {
        if (d == 0) throw new InvalidOperationException("Zero denominator.");
        if (d < 0) { n = -n; d = -d; }
        var g = Gcd(Math.Abs(n), d);
        n /= g; d /= g;
        return d == 1 ? n.ToString(CultureInfo.InvariantCulture) : $"{n}/{d}";
    }

    private static bool EquivalentFraction(string answer, string expected)
    {
        if (!TryFraction(answer, out var an, out var ad) || !TryFraction(expected, out var en, out var ed))
            return false;
        return ad != 0 && ed != 0 && an * ed == en * ad;
    }

    private static bool TryFraction(string text, out int n, out int d)
    {
        n = 0; d = 1;
        var value = text.Trim();
        var slash = value.IndexOf('/');
        if (slash < 0)
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out n);
        return int.TryParse(value[..slash].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out n) &&
            int.TryParse(value[(slash+1)..].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out d) &&
            d != 0;
    }

    private static string FormatPair(int x, int y) => $"({x}, {y})";

    private static bool EquivalentPair(string answer, string expected) =>
        NormalizePair(answer) == NormalizePair(expected);

    private static string NormalizePair(string value) =>
        value.Trim().Replace(" ", "", StringComparison.Ordinal)
            .Replace("<", "(", StringComparison.Ordinal)
            .Replace(">", ")", StringComparison.Ordinal)
            .Replace("[", "(", StringComparison.Ordinal)
            .Replace("]", ")", StringComparison.Ordinal);

    private static string FormatHundredths(int value)
    {
        var sign = value < 0 ? "-" : "";
        var abs = Math.Abs(value);
        return $"{sign}{abs / 100}.{abs % 100:00}";
    }

    private static string Compare(int left, int right) =>
        left < right ? "<" : left > right ? ">" : "=";

    private static string CompareLabel(int left, int right) =>
        left > right ? "A" : right > left ? "B" : "equal";

    private static string ClassifyAngle(int angle) =>
        angle < 90 ? "acute" :
        angle == 90 ? "right" :
        angle < 180 ? "obtuse" :
        angle == 180 ? "straight" : "reflex";

    private static string Signed(int value) =>
        value >= 0 ? $"+{value}" : value.ToString(CultureInfo.InvariantCulture);

    private static string Imag(int value) =>
        value >= 0 ? $"+{value}i" : $"{value}i";

    private static string NormalizeExpression(string value) =>
        value.Trim().ToLowerInvariant()
            .Replace(" ", "", StringComparison.Ordinal)
            .Replace("−", "-", StringComparison.Ordinal)
            .Replace("*", "", StringComparison.Ordinal);

    private static string NormalizeText(string value) =>
        value.Trim().ToLowerInvariant()
            .Replace("−", "-", StringComparison.Ordinal)
            .Replace(" ", "", StringComparison.Ordinal);

    private static string Clock(int minutes)
    {
        var m = ((minutes % 1440) + 1440) % 1440;
        return $"{m/60:00}:{m%60:00}";
    }
}

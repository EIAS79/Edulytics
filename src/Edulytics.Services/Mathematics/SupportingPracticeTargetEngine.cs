using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Deterministic shared implementation for target-specific Supporting Practice
/// families. The family id remains unique to one canonical target while this
/// engine reuses exact mathematical implementations across related targets.
/// </summary>
internal static class SupportingPracticeTargetEngine
{
    internal sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        string CorrectAnswer,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        !string.IsNullOrWhiteSpace(family) &&
        SupportingPracticeTargetResolver.IsGeneratedFamily(family);

    public static Problem Build(
        string family,
        Random random,
        ExactSkillQuestionDifficulty difficulty)
    {
        if (!SupportingPracticeTargetResolver.TryGetRuleIdFromFamily(family, out var ruleId))
            throw new InvalidOperationException($"Unsupported Supporting target family: {family}");

        var scale = (int)difficulty;
        return ruleId switch
        {
            "addition_subtraction" => AddSubtract(family, random, scale),
            "multiplication_division" or "scale_by_powers10" => MultiplyDivide(family, random, scale, ruleId),
            "place_value" => PlaceValue(family, random, scale),
            "rounding_estimation" => Rounding(family, random, scale),
            "decimals" => Decimals(family, random, scale),
            "fractions.multiply_divide" => FractionMultiplyDivide(family, random),
            "fractions.mixed" => MixedNumber(family, random, scale),
            "fractions.equivalence" => FractionEquivalent(family, random),
            "fractions.represent" => FractionRepresent(family, random),
            "percentages" => Percentage(family, random, scale),
            "ratio" => Ratio(family, random, scale),
            "sequences" => Sequence(family, random, scale),

            "algebra.linear" => LinearEquation(family, random, scale),
            "algebra.systems" => LinearSystem(family, random, scale),
            "algebra.expression" => ExpressionSubstitution(family, random, scale),
            "algebra.expand_factor" => ExpandBracket(family, random, scale),
            "algebra.surds" => IntegerPower(family, random, scale),
            "quadratic.solve" => QuadraticRoots(family, random, scale),
            "quadratic.expression" => QuadraticEvaluate(family, random, scale),

            "functions.graphs" => FunctionEvaluate(family, random, scale),
            "functions.transform" => FunctionTransform(family, random, scale),
            "functions.exponential" => IntegerPower(family, random, scale),

            "geometry.angles_shapes" => GeometryAngles(family, random),
            "geometry.area_perimeter" => AreaPerimeter(family, random, scale),
            "geometry.volume" => Volume(family, random, scale),
            "geometry.coordinate" => Coordinate(family, random, scale),
            "geometry.similarity" => Similarity(family, random, scale),
            "geometry.transform" => Transform(family, random, scale),
            "geometry.symmetry" => Symmetry(family, random, scale),
            "geometry.circle" => Circle(family, random, scale),
            "geometry.construction" => Construction(family, random),

            "measurement.units" => Measurement(family, random, scale),

            "trig.right" or "trig.modelling" => RightTriangle(family, random, scale),
            "trig.sine_rule" => FormulaChoice(
                family,
                random,
                "Which formula is the sine rule?",
                ["a/sin A = b/sin B = c/sin C", "a² = b² + c² − 2bc cos A", "A = 1/2 bh", "s = rθ"],
                0,
                "The sine rule relates each side to the sine of its opposite angle."),
            "trig.cosine_rule" => FormulaChoice(
                family,
                random,
                "Which formula is the cosine rule for side a?",
                ["a² = b² + c² − 2bc cos A", "a/sin A = b/sin B", "A = πr²", "tan A = adjacent/opposite"],
                0,
                "The cosine rule is a² = b² + c² − 2bc cos A."),
            "trig.area_sine" => FormulaChoice(
                family,
                random,
                "Which formula gives the area of a triangle from sides a, b and included angle C?",
                ["1/2 ab sin C", "ab cos C", "a+b+c", "1/2(a+b)C"],
                0,
                "The exact included-angle area formula is 1/2 ab sin C."),
            "trig.bearings" => FormulaChoice(
                family,
                random,
                "A three-figure bearing is measured in which way?",
                ["Clockwise from north", "Anticlockwise from east", "Clockwise from south", "From the nearest axis only"],
                0,
                "Bearings are measured clockwise from north and written with three figures."),
            "trig.circular_measure" => CircularMeasure(family, random, scale),
            "trig.graphs_equations" => TrigSpecial(family, random),

            "vectors" => Vector(family, random, scale),
            "matrices_complex" => MatrixOrComplex(family, random, scale),

            "statistics.center_spread" => StatisticsCenter(family, random, scale),
            "statistics.scatter" => CorrelationChoice(family, random),
            "statistics.cumulative" => CumulativeFrequency(family, random, scale),
            "statistics.grouped" => GroupedData(family, random, scale),
            "statistics.representation" => DataRepresentationChoice(family, random),

            "probability.simple" or "sets.combinatorics" => SimpleProbability(family, random, scale),
            "probability.experimental" => ExperimentalProbability(family, random, scale),
            "probability.tree" or "probability.combined" => CombinedProbability(family, random),
            "probability.normal" => FormulaChoice(
                family,
                random,
                "For a symmetric normal distribution, which statement is true?",
                ["Mean = median = mode", "Mean is always zero", "All values are positive", "Standard deviation equals the mean"],
                0,
                "A normal distribution is symmetric about its mean, so mean, median and mode coincide."),
            "probability.poisson_binomial" => BinomialExpectation(family, random, scale),
            "probability.random_variable" => RandomVariableExpectation(family, random, scale),

            "calculus.derivative" => DerivativeValue(family, random, scale),
            "calculus.integral" => IntegralValue(family, random, scale),
            "numerical.methods" => BisectionMidpoint(family, random, scale),

            "mechanics.constant_acceleration" or "mechanics.projectile" => ConstantAcceleration(family, random, scale),
            "mechanics.force" => Force(family, random, scale),
            "mechanics.momentum" => Momentum(family, random, scale),
            "mechanics.energy" => Power(family, random, scale),

            "reasoning" => FormulaChoice(
                family,
                random,
                "Which statement describes a valid way to disprove a universal mathematical claim?",
                ["Give one valid counterexample", "Check one example that works", "Assume the conclusion", "Ignore boundary cases"],
                0,
                "A single valid counterexample is enough to disprove a universal claim."),

            _ => throw new InvalidOperationException($"Supporting target rule {ruleId} has no exact implementation.")
        };
    }

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string answer)
    {
        if (!SupportingPracticeTargetResolver.TryGetRuleIdFromFamily(family, out var ruleId))
            return false;

        if (parameters.TryGetValue("answerKind", out var kind))
        {
            return kind switch
            {
                1 => VerifyInteger(parameters, answer),
                2 => VerifyFraction(parameters, answer),
                3 => VerifyVector2(parameters, answer),
                4 => VerifyPair(parameters, answer),
                5 => VerifyChoice(parameters, answer),
                6 => VerifyMixed(parameters, answer),
                _ => false
            };
        }

        return false;
    }

    private static bool VerifyInteger(IReadOnlyDictionary<string, int> p, string answer) =>
        int.TryParse(answer.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) &&
        p.TryGetValue("expected", out var expected) &&
        value == expected;

    private static bool VerifyFraction(IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!TryFraction(answer, out var n, out var d) || d == 0 ||
            !p.TryGetValue("numerator", out var en) ||
            !p.TryGetValue("denominator", out var ed) ||
            ed == 0)
        {
            return false;
        }

        return n * ed == en * d;
    }

    private static bool VerifyVector2(IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!TryPair(answer, out var x, out var y))
            return false;
        return p.TryGetValue("x", out var ex) &&
            p.TryGetValue("y", out var ey) &&
            x == ex && y == ey;
    }

    private static bool VerifyPair(IReadOnlyDictionary<string, int> p, string answer) =>
        VerifyVector2(p, answer);

    private static bool VerifyChoice(IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!p.TryGetValue("correct", out var correct) || correct is < 0 or > 3)
            return false;
        var normalized = answer.Trim().ToUpperInvariant();
        return normalized == ((char)('A' + correct)).ToString() ||
            normalized == (correct + 1).ToString(CultureInfo.InvariantCulture);
    }

    private static bool VerifyMixed(IReadOnlyDictionary<string, int> p, string answer)
    {
        if (!p.TryGetValue("whole", out var whole) ||
            !p.TryGetValue("numerator", out var numerator) ||
            !p.TryGetValue("denominator", out var denominator) ||
            denominator <= 0)
        {
            return false;
        }

        var normalized = string.Join(" ", answer.Trim().Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        return string.Equals(
            normalized,
            $"{whole} {numerator}/{denominator}",
            StringComparison.Ordinal);
    }

    private static Problem AddSubtract(string family, Random random, int scale)
    {
        var a = random.Next(20 * scale, 80 * scale + 50);
        var b = random.Next(1, a + 1);
        var add = random.Next(0, 2) == 0;
        var expected = add ? a + b : a - b;
        return Integer(
            family,
            $"Calculate {a} {(add ? "+" : "−")} {b}.",
            "Use place-value aligned addition or subtraction, then check with the inverse operation.",
            expected,
            ("a", a), ("b", b), ("operation", add ? 0 : 1));
    }

    private static Problem MultiplyDivide(string family, Random random, int scale, string ruleId)
    {
        if (ruleId == "scale_by_powers10")
        {
            var value = random.Next(2, 90);
            var factor = random.Next(0, 2) == 0 ? 10 : 100;
            var multiply = random.Next(0, 2) == 0;
            var start = multiply ? value : value * factor;
            var expected = multiply ? value * factor : value;
            return Integer(
                family,
                $"Calculate {start} {(multiply ? "×" : "÷")} {factor}.",
                "Multiplying or dividing by a power of ten shifts place values while preserving digit order.",
                expected,
                ("value", start), ("factor", factor), ("multiply", multiply ? 1 : 0));
        }

        var a = random.Next(2, 12 + scale * 5);
        var b = random.Next(2, 10 + scale * 4);
        if (random.Next(0, 2) == 0)
        {
            return Integer(
                family,
                $"Calculate {a} × {b}.",
                "Multiply the factors and verify by division.",
                a * b,
                ("a", a), ("b", b), ("operation", 2));
        }

        return Integer(
            family,
            $"Calculate {a * b} ÷ {a}.",
            "Divide the dividend by the divisor and verify by multiplication.",
            b,
            ("a", a * b), ("b", a), ("operation", 3));
    }

    private static Problem PlaceValue(string family, Random random, int scale)
    {
        var power = random.Next(1, Math.Min(7, 3 + scale * 2));
        var digit = random.Next(1, 10);
        var number = digit * Pow10(power) + random.Next(0, Pow10(power));
        var expected = digit * Pow10(power);
        return Integer(
            family,
            $"In the number {number}, what value is contributed by the digit {digit} in the 10^{power} place?",
            "A digit's value equals the digit multiplied by its place value.",
            expected,
            ("digit", digit), ("power", power), ("number", number));
    }

    private static Problem Rounding(string family, Random random, int scale)
    {
        var place = random.Next(1, Math.Min(4, 1 + scale + 1));
        var unit = Pow10(place);
        var value = random.Next(2, 500 * scale + 200);
        var expected = ((value + unit / 2) / unit) * unit;
        return Integer(
            family,
            $"Round {value} to the nearest {unit}.",
            "Locate the rounding digit, inspect the next digit, then check the rounded value is the nearest multiple of the target unit.",
            expected,
            ("value", value), ("unit", unit));
    }

    private static Problem Decimals(string family, Random random, int scale)
    {
        var a = random.Next(10, 100 + scale * 50);
        var b = random.Next(1, 80);
        return Integer(
            family,
            $"Write the result of {a}/10 + {b}/10 as tenths. Enter only the numerator over 10.",
            "Add quantities expressed in the same tenths unit.",
            a + b,
            ("aTenths", a), ("bTenths", b));
    }

    private static Problem FractionMultiplyDivide(string family, Random random)
    {
        var a = random.Next(1, 6);
        var b = random.Next(a + 1, 9);
        var c = random.Next(1, 6);
        var d = random.Next(c + 1, 9);
        var divide = random.Next(0, 2) == 0;
        var n = divide ? a * d : a * c;
        var den = divide ? b * c : b * d;
        return Fraction(
            family,
            $"Calculate {a}/{b} {(divide ? "÷" : "×")} {c}/{d}. Give an exact fraction.",
            divide
                ? "Multiply by the reciprocal of the divisor, then simplify."
                : "Multiply numerators and denominators, then simplify.",
            n,
            den,
            ("a", a), ("b", b), ("c", c), ("d", d), ("divide", divide ? 1 : 0));
    }

    private static Problem MixedNumber(string family, Random random, int scale)
    {
        var denominator = random.Next(2, 8 + scale);
        var whole = random.Next(1, 5 + scale);
        var numerator = random.Next(1, denominator);
        var improper = whole * denominator + numerator;
        var p = Params(("answerKind", 6), ("whole", whole), ("numerator", numerator), ("denominator", denominator), ("improper", improper));
        return new(
            family,
            $"Convert {improper}/{denominator} to a mixed number.",
            "Divide the numerator by the denominator. The quotient is the whole-number part and the remainder is the fractional numerator.",
            AssessmentItemType.ShortAnswer,
            $"{whole} {numerator}/{denominator}",
            p);
    }

    private static Problem FractionEquivalent(string family, Random random)
    {
        var n = random.Next(1, 6);
        var d = random.Next(n + 1, 10);
        var factor = random.Next(2, 7);
        return Integer(
            family,
            $"Complete the equivalent fraction: {n}/{d} = ?/{d * factor}.",
            "Multiply numerator and denominator by the same non-zero factor.",
            n * factor,
            ("n", n), ("d", d), ("factor", factor));
    }

    private static Problem FractionRepresent(string family, Random random)
    {
        var denominator = random.Next(2, 10);
        var numerator = random.Next(1, denominator + 1);
        return Fraction(
            family,
            $"A whole is split into {denominator} equal parts and {numerator} are selected. Write the fraction.",
            "The denominator counts equal parts in the whole; the numerator counts selected parts.",
            numerator,
            denominator,
            ("selected", numerator), ("parts", denominator));
    }

    private static Problem Percentage(string family, Random random, int scale)
    {
        int[] choices = [10, 20, 25, 40, 50, 60, 75, 80];
        var percent = choices[random.Next(choices.Length)];
        var quantity = 20 * random.Next(2, 5 + scale * 2);
        var expected = quantity * percent / 100;
        return Integer(
            family,
            $"Find {percent}% of {quantity}.",
            "Convert the percentage to a fraction over 100 and multiply by the quantity.",
            expected,
            ("percent", percent), ("quantity", quantity));
    }

    private static Problem Ratio(string family, Random random, int scale)
    {
        var a = random.Next(1, 5 + scale);
        var b = random.Next(1, 5 + scale);
        var unit = random.Next(2, 8 + scale);
        var total = (a + b) * unit;
        return Integer(
            family,
            $"Divide {total} in the ratio {a}:{b}. Find the first share.",
            "Add the ratio parts, find the value of one part, then multiply by the requested number of parts.",
            a * unit,
            ("a", a), ("b", b), ("total", total));
    }

    private static Problem Sequence(string family, Random random, int scale)
    {
        var first = random.Next(-4 * scale, 8 * scale + 4);
        var difference = NonZero(random, -4 - scale, 5 + scale);
        var n = random.Next(5, 10 + scale * 4);
        return Integer(
            family,
            $"The arithmetic sequence starts {first}, {first + difference}, {first + 2 * difference}, ... Find term {n}.",
            "Use aₙ = a₁ + (n−1)d and verify by repeated addition of the common difference.",
            first + (n - 1) * difference,
            ("first", first), ("difference", difference), ("n", n));
    }

    private static Problem LinearEquation(string family, Random random, int scale)
    {
        var x = NonZero(random, -8 * scale - 5, 8 * scale + 6);
        var a = NonZero(random, 1, 5 + scale);
        var b = random.Next(-10 * scale, 10 * scale + 1);
        var c = a * x + b;
        return Integer(
            family,
            $"Solve {a}x {(b >= 0 ? "+" : "−")} {Math.Abs(b)} = {c}.",
            "Undo the constant term, divide by the non-zero coefficient, then substitute the result into the original equation.",
            x,
            ("a", a), ("b", b), ("c", c));
    }

    private static Problem LinearSystem(string family, Random random, int scale)
    {
        var x = random.Next(-5 * scale, 6 * scale + 1);
        var y = random.Next(-5 * scale, 6 * scale + 1);
        var sum = x + y;
        var difference = x - y;
        return Pair(
            family,
            $"Solve x + y = {sum} and x − y = {difference}. Give the ordered pair (x, y).",
            "Add the equations to isolate x, then substitute to find y and verify both original equations.",
            x,
            y,
            ("sum", sum), ("difference", difference));
    }

    private static Problem ExpressionSubstitution(string family, Random random, int scale)
    {
        var x = random.Next(-5 * scale, 6 * scale + 1);
        var a = NonZero(random, -5 - scale, 6 + scale);
        var b = random.Next(-10, 11);
        return Integer(
            family,
            $"Evaluate {a}x {(b >= 0 ? "+" : "−")} {Math.Abs(b)} when x = {x}.",
            "Substitute the given value for x, preserve operation order, and simplify.",
            a * x + b,
            ("a", a), ("b", b), ("x", x));
    }

    private static Problem ExpandBracket(string family, Random random, int scale)
    {
        var a = NonZero(random, 2, 5 + scale);
        var b = random.Next(1, 8 + scale);
        return Pair(
            family,
            $"Expand {a}(x + {b}). Enter the coefficient of x and the constant as (coefficient, constant).",
            "Apply the distributive property to every term inside the bracket.",
            a,
            a * b,
            ("a", a), ("b", b));
    }

    private static Problem IntegerPower(string family, Random random, int scale)
    {
        var @base = random.Next(2, 5 + scale);
        var exponent = random.Next(2, Math.Min(6, 3 + scale + 1));
        return Integer(
            family,
            $"Evaluate {@base}^{exponent}.",
            "An integer power means repeated multiplication of the base by itself.",
            Pow(@base, exponent),
            ("base", @base), ("exponent", exponent));
    }

    private static Problem QuadraticRoots(string family, Random random, int scale)
    {
        var r1 = NonZero(random, -5 - scale, 6 + scale);
        var r2 = NonZero(random, -5 - scale, 6 + scale);
        return Pair(
            family,
            $"Solve (x − ({r1}))(x − ({r2})) = 0. Give the roots as (first, second).",
            "Use the zero-product property: each factor may equal zero. Substitute both roots to verify.",
            r1,
            r2,
            ("r1", r1), ("r2", r2));
    }

    private static Problem QuadraticEvaluate(string family, Random random, int scale)
    {
        var x = random.Next(-4 - scale, 5 + scale);
        var a = NonZero(random, 1, 4 + scale);
        var b = random.Next(-5, 6);
        var c = random.Next(-8, 9);
        return Integer(
            family,
            $"Evaluate {a}x² {(b >= 0 ? "+" : "−")} {Math.Abs(b)}x {(c >= 0 ? "+" : "−")} {Math.Abs(c)} when x = {x}.",
            "Substitute x into every term, square before multiplying, then combine like signed values.",
            a * x * x + b * x + c,
            ("a", a), ("b", b), ("c", c), ("x", x));
    }

    private static Problem FunctionEvaluate(string family, Random random, int scale)
    {
        var x = random.Next(-5 - scale, 6 + scale);
        var m = NonZero(random, -4 - scale, 5 + scale);
        var c = random.Next(-8, 9);
        return Integer(
            family,
            $"For f(x) = {m}x {(c >= 0 ? "+" : "−")} {Math.Abs(c)}, find f({x}).",
            "Replace x with the stated input and simplify. The output must satisfy the original function rule.",
            m * x + c,
            ("m", m), ("c", c), ("x", x));
    }

    private static Problem FunctionTransform(string family, Random random, int scale)
    {
        var x = random.Next(-5 - scale, 6 + scale);
        var shift = NonZero(random, -4 - scale, 5 + scale);
        return Integer(
            family,
            $"Let f(x)=x² and g(x)=f(x)+{shift}. Find g({x}).",
            "Evaluate the base function first, then apply the stated vertical transformation.",
            x * x + shift,
            ("x", x), ("shift", shift));
    }

    private static Problem GeometryAngles(string family, Random random)
    {
        var a = random.Next(25, 130);
        return Integer(
            family,
            $"Two adjacent angles on a straight line include an angle of {a}°. Find the other angle.",
            "Angles on a straight line sum to 180°.",
            180 - a,
            ("known", a));
    }

    private static Problem AreaPerimeter(string family, Random random, int scale)
    {
        var l = random.Next(3, 10 + scale * 3);
        var w = random.Next(2, 8 + scale * 2);
        var area = random.Next(0, 2) == 0;
        return Integer(
            family,
            area
                ? $"A rectangle has length {l} and width {w}. Find its area."
                : $"A rectangle has length {l} and width {w}. Find its perimeter.",
            area
                ? "Area of a rectangle is length × width."
                : "Perimeter of a rectangle is twice the sum of length and width.",
            area ? l * w : 2 * (l + w),
            ("length", l), ("width", w), ("area", area ? 1 : 0));
    }

    private static Problem Volume(string family, Random random, int scale)
    {
        var l = random.Next(2, 7 + scale);
        var w = random.Next(2, 6 + scale);
        var h = random.Next(2, 6 + scale);
        return Integer(
            family,
            $"A rectangular prism has length {l}, width {w} and height {h}. Find its volume.",
            "Volume of a rectangular prism is length × width × height.",
            l * w * h,
            ("length", l), ("width", w), ("height", h));
    }

    private static Problem Coordinate(string family, Random random, int scale)
    {
        var x1 = random.Next(-5 * scale, 5 * scale + 1);
        var run = random.Next(1, 5 + scale);
        var m = NonZero(random, -4 - scale, 5 + scale);
        var y1 = random.Next(-6 * scale, 6 * scale + 1);
        var x2 = x1 + run;
        var y2 = y1 + m * run;
        return Integer(
            family,
            $"Find the gradient of the line through ({x1}, {y1}) and ({x2}, {y2}).",
            "Gradient is change in y divided by change in x.",
            m,
            ("x1", x1), ("y1", y1), ("x2", x2), ("y2", y2));
    }

    private static Problem Similarity(string family, Random random, int scale)
    {
        var source = random.Next(2, 8 + scale);
        var factor = random.Next(2, 5 + scale);
        return Integer(
            family,
            $"A side of length {source} is enlarged by scale factor {factor}. Find the corresponding side length.",
            "Corresponding lengths in similar figures are multiplied by the linear scale factor.",
            source * factor,
            ("source", source), ("factor", factor));
    }

    private static Problem Transform(string family, Random random, int scale)
    {
        var x = random.Next(-5 * scale, 6 * scale + 1);
        var y = random.Next(-5 * scale, 6 * scale + 1);
        var dx = NonZero(random, -4 - scale, 5 + scale);
        var dy = NonZero(random, -4 - scale, 5 + scale);
        return Pair(
            family,
            $"Translate the point ({x}, {y}) by vector <{dx}, {dy}>. Give the image as (x, y).",
            "A translation adds the vector components to the point coordinates.",
            x + dx,
            y + dy,
            ("x0", x), ("y0", y), ("dx", dx), ("dy", dy));
    }

    private static Problem Symmetry(string family, Random random, int scale)
    {
        var x = NonZero(random, -5 - scale, 6 + scale);
        var y = random.Next(-5 - scale, 6 + scale);
        return Pair(
            family,
            $"Reflect the point ({x}, {y}) in the y-axis. Give the image as (x, y).",
            "Reflection in the y-axis changes the sign of x and keeps y unchanged.",
            -x,
            y,
            ("x0", x), ("y0", y));
    }

    private static Problem Circle(string family, Random random, int scale)
    {
        var radius = random.Next(2, 10 + scale);
        return Integer(
            family,
            $"A circle has radius {radius}. Find its diameter.",
            "The diameter is twice the radius.",
            2 * radius,
            ("radius", radius));
    }

    private static Problem Construction(string family, Random random) =>
        FormulaChoice(
            family,
            random,
            "Which locus contains all points that are equally distant from two fixed points A and B?",
            ["The perpendicular bisector of AB", "The circle with centre A only", "The line AB only", "Any line parallel to AB"],
            0,
            "Points equidistant from A and B lie on the perpendicular bisector of segment AB.");

    private static Problem Measurement(string family, Random random, int scale)
    {
        var metres = random.Next(1, 20 + scale * 10);
        return Integer(
            family,
            $"Convert {metres} metres to centimetres.",
            "One metre equals 100 centimetres.",
            metres * 100,
            ("metres", metres));
    }

    private static Problem RightTriangle(string family, Random random, int scale)
    {
        var triples = new (int A, int B, int C)[] { (3,4,5), (5,12,13), (8,15,17), (7,24,25) };
        var t = triples[random.Next(triples.Length)];
        var k = random.Next(1, Math.Max(2, scale + 1));
        return Integer(
            family,
            $"A right triangle has perpendicular sides {t.A * k} and {t.B * k}. Find the hypotenuse.",
            "Use Pythagoras: c²=a²+b², then take the positive square root.",
            t.C * k,
            ("a", t.A * k), ("b", t.B * k), ("c", t.C * k));
    }

    private static Problem CircularMeasure(string family, Random random, int scale)
    {
        var radius = random.Next(2, 8 + scale);
        var theta = random.Next(1, 4 + scale);
        return Integer(
            family,
            $"An arc has radius {radius} and central angle {theta} radians. Find its arc length.",
            "For radians, arc length s = rθ.",
            radius * theta,
            ("radius", radius), ("theta", theta));
    }

    private static Problem TrigSpecial(string family, Random random)
    {
        var angle = new[] { 30, 45, 60 }[random.Next(3)];
        var correct = angle switch { 30 => 0, 45 => 1, _ => 2 };
        return FormulaChoice(
            family,
            random,
            $"Which exact value equals sin({angle}°)?",
            angle switch
            {
                30 => ["1/2", "√2/2", "√3/2", "1"],
                45 => ["√2/2", "1/2", "√3/2", "0"],
                _ => ["√3/2", "√2/2", "1/2", "1"]
            },
            0,
            "Use the exact special-angle values from the 30°–60°–90° and 45°–45°–90° triangles.");
    }

    private static Problem Vector(string family, Random random, int scale)
    {
        var ax = NonZero(random, -5 - scale, 6 + scale);
        var ay = NonZero(random, -5 - scale, 6 + scale);
        var bx = NonZero(random, -5 - scale, 6 + scale);
        var by = NonZero(random, -5 - scale, 6 + scale);
        return Vector2(
            family,
            $"Let a=<{ax},{ay}> and b=<{bx},{by}>. Find a+b.",
            "Add corresponding vector components.",
            ax + bx,
            ay + by,
            ("ax", ax), ("ay", ay), ("bx", bx), ("by", by));
    }

    private static Problem MatrixOrComplex(string family, Random random, int scale)
    {
        var a = random.Next(-5 - scale, 6 + scale);
        var b = random.Next(-5 - scale, 6 + scale);
        var c = random.Next(-5 - scale, 6 + scale);
        var d = random.Next(-5 - scale, 6 + scale);
        return Pair(
            family,
            $"For complex numbers ({a}+{b}i) and ({c}+{d}i), add them. Give (real, imaginary).",
            "Add real parts together and imaginary coefficients together.",
            a + c,
            b + d,
            ("a", a), ("b", b), ("c", c), ("d", d));
    }

    private static Problem StatisticsCenter(string family, Random random, int scale)
    {
        var mean = random.Next(3, 12 + scale * 3);
        var d1 = random.Next(1, 4 + scale);
        var d2 = random.Next(1, 4 + scale);
        var values = new[] { mean-d1, mean-d2, mean, mean+d2, mean+d1 };
        return Integer(
            family,
            $"Find the mean of {string.Join(", ", values)}.",
            "Add all values and divide by the number of values.",
            mean,
            ("v1", values[0]), ("v2", values[1]), ("v3", values[2]), ("v4", values[3]), ("v5", values[4]));
    }

    private static Problem CorrelationChoice(string family, Random random) =>
        FormulaChoice(
            family,
            random,
            "As x increases, y also tends to increase. Which description fits the scatter pattern?",
            ["Positive correlation", "Negative correlation", "No correlation", "Impossible data"],
            0,
            "When larger x-values tend to accompany larger y-values, the association is positive.");

    private static Problem CumulativeFrequency(string family, Random random, int scale)
    {
        var total = 4 * random.Next(5, 15 + scale * 4);
        return Integer(
            family,
            $"A cumulative-frequency table contains {total} observations. At what cumulative frequency is the lower quartile located?",
            "The lower quartile is at one quarter of the ordered observations.",
            total / 4,
            ("total", total));
    }

    private static Problem GroupedData(string family, Random random, int scale)
    {
        var low = 2 * random.Next(1, 10 + scale);
        var width = 2 * random.Next(2, 6 + scale);
        var high = low + width;
        return Integer(
            family,
            $"A grouped class interval runs from {low} to {high}. Find its class midpoint.",
            "The class midpoint is the average of the lower and upper class boundaries.",
            (low + high) / 2,
            ("low", low), ("high", high));
    }

    private static Problem DataRepresentationChoice(string family, Random random) =>
        FormulaChoice(
            family,
            random,
            "Which display is most appropriate for showing frequencies of separate categories?",
            ["Bar chart", "Scatter graph", "Cumulative-frequency curve", "Line of best fit"],
            0,
            "A bar chart represents frequencies for distinct categories.");

    private static Problem SimpleProbability(string family, Random random, int scale)
    {
        var total = random.Next(4, 10 + scale * 2);
        var favourable = random.Next(1, total);
        return Fraction(
            family,
            $"There are {total} equally likely outcomes and {favourable} are favourable. Find the probability.",
            "For equally likely outcomes, probability is favourable outcomes divided by total outcomes.",
            favourable,
            total,
            ("favourable", favourable), ("total", total));
    }

    private static Problem ExperimentalProbability(string family, Random random, int scale)
    {
        var trials = 10 * random.Next(2, 8 + scale);
        var successes = random.Next(1, trials);
        return Fraction(
            family,
            $"An event occurred {successes} times in {trials} trials. Estimate its experimental probability.",
            "Experimental probability is relative frequency: successes divided by trials.",
            successes,
            trials,
            ("successes", successes), ("trials", trials));
    }

    private static Problem CombinedProbability(string family, Random random)
    {
        var a = random.Next(1, 5);
        var b = random.Next(a + 1, 7);
        var c = random.Next(1, 5);
        var d = random.Next(c + 1, 7);
        return Fraction(
            family,
            $"Independent events have probabilities {a}/{b} and {c}/{d}. Find the probability that both occur.",
            "For independent events, multiply the probabilities and simplify.",
            a * c,
            b * d,
            ("a", a), ("b", b), ("c", c), ("d", d));
    }

    private static Problem BinomialExpectation(string family, Random random, int scale)
    {
        var n = 2 * random.Next(2, 8 + scale);
        return Integer(
            family,
            $"A binomial model has n={n} independent trials and success probability 1/2. Find the expected number of successes.",
            "For a binomial random variable, E(X)=np.",
            n / 2,
            ("n", n), ("pNumerator", 1), ("pDenominator", 2));
    }

    private static Problem RandomVariableExpectation(string family, Random random, int scale)
    {
        var a = random.Next(0, 10 + scale * 2);
        var b = a + 2 * random.Next(1, 6 + scale);
        return Integer(
            family,
            $"A random variable takes values {a} and {b}, each with probability 1/2. Find E(X).",
            "Multiply each value by its probability and add the results.",
            (a + b) / 2,
            ("a", a), ("b", b));
    }

    private static Problem DerivativeValue(string family, Random random, int scale)
    {
        var a = NonZero(random, 1, 5 + scale);
        var n = random.Next(2, 5 + scale);
        var x = random.Next(1, 4 + scale);
        return Integer(
            family,
            $"For f(x)={a}x^{n}, find f'({x}).",
            "Differentiate with the power rule, then substitute the stated x-value.",
            a * n * Pow(x, n - 1),
            ("a", a), ("n", n), ("x", x));
    }

    private static Problem IntegralValue(string family, Random random, int scale)
    {
        var a = 2 * random.Next(1, 4 + scale);
        var upper = random.Next(1, 4 + scale);
        return Integer(
            family,
            $"Evaluate the definite integral of {a}x from 0 to {upper}.",
            "An antiderivative of ax is (a/2)x². Evaluate at the bounds and subtract.",
            (a / 2) * upper * upper,
            ("a", a), ("upper", upper));
    }

    private static Problem BisectionMidpoint(string family, Random random, int scale)
    {
        var low = random.Next(-10 * scale, 10 * scale + 1);
        var width = 2 * random.Next(1, 5 + scale);
        var high = low + width;
        return Integer(
            family,
            $"A bisection step uses the interval [{low}, {high}]. Find its midpoint.",
            "The bisection midpoint is the average of the interval endpoints.",
            (low + high) / 2,
            ("low", low), ("high", high));
    }

    private static Problem ConstantAcceleration(string family, Random random, int scale)
    {
        var u = random.Next(0, 10 + scale * 3);
        var a = random.Next(1, 5 + scale);
        var t = random.Next(1, 5 + scale);
        return Integer(
            family,
            $"An object has initial velocity {u} m/s and constant acceleration {a} m/s² for {t} s. Find its final velocity.",
            "Use v=u+at and check the units are metres per second.",
            u + a * t,
            ("u", u), ("a", a), ("t", t));
    }

    private static Problem Force(string family, Random random, int scale)
    {
        var mass = random.Next(1, 10 + scale * 3);
        var acceleration = random.Next(1, 6 + scale);
        return Integer(
            family,
            $"A mass of {mass} kg accelerates at {acceleration} m/s². Find the resultant force in newtons.",
            "Use Newton's second law F=ma.",
            mass * acceleration,
            ("mass", mass), ("acceleration", acceleration));
    }

    private static Problem Momentum(string family, Random random, int scale)
    {
        var mass = random.Next(1, 8 + scale);
        var u = random.Next(0, 8 + scale);
        var v = u + random.Next(1, 6 + scale);
        return Integer(
            family,
            $"A {mass} kg object changes velocity from {u} m/s to {v} m/s. Find the impulse magnitude.",
            "Impulse equals change in momentum: m(v−u).",
            mass * (v - u),
            ("mass", mass), ("u", u), ("v", v));
    }

    private static Problem Power(string family, Random random, int scale)
    {
        var time = random.Next(1, 6 + scale);
        var power = random.Next(2, 20 + scale * 5);
        var work = time * power;
        return Integer(
            family,
            $"A machine transfers {work} J of energy in {time} s. Find its power in watts.",
            "Power is work or energy transferred divided by time.",
            power,
            ("work", work), ("time", time));
    }

    private static Problem FormulaChoice(
        string family,
        Random random,
        string question,
        IReadOnlyList<string> options,
        int correctOriginalIndex,
        string solution)
    {
        var indexed = options
            .Select((text, index) => (Text: text, Original: index, Key: random.Next()))
            .OrderBy(x => x.Key)
            .ToArray();
        var correct = Array.FindIndex(indexed, x => x.Original == correctOriginalIndex);
        var prompt = question + " " + string.Join(
            "  ",
            indexed.Select((item, index) => $"{(char)('A' + index)}) {item.Text}"));
        var p = Params(("answerKind", 5), ("correct", correct));
        return new(
            family,
            prompt,
            solution,
            AssessmentItemType.MultipleChoice,
            ((char)('A' + correct)).ToString(),
            p);
    }

    private static Problem Integer(
        string family,
        string prompt,
        string solution,
        int expected,
        params (string Name, int Value)[] extra)
    {
        var p = Params(extra.Prepend(("expected", expected)).Prepend(("answerKind", 1)).ToArray());
        return new(
            family,
            prompt,
            solution,
            AssessmentItemType.Numeric,
            expected.ToString(CultureInfo.InvariantCulture),
            p);
    }

    private static Problem Fraction(
        string family,
        string prompt,
        string solution,
        int numerator,
        int denominator,
        params (string Name, int Value)[] extra)
    {
        var gcd = Gcd(Math.Abs(numerator), Math.Abs(denominator));
        numerator /= gcd;
        denominator /= gcd;
        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var values = extra
            .Prepend(("denominator", denominator))
            .Prepend(("numerator", numerator))
            .Prepend(("answerKind", 2))
            .ToArray();
        return new(
            family,
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            $"{numerator}/{denominator}",
            Params(values));
    }

    private static Problem Pair(
        string family,
        string prompt,
        string solution,
        int x,
        int y,
        params (string Name, int Value)[] extra)
    {
        var values = extra
            .Prepend(("y", y))
            .Prepend(("x", x))
            .Prepend(("answerKind", 4))
            .ToArray();
        return new(
            family,
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            $"({x}, {y})",
            Params(values));
    }

    private static Problem Vector2(
        string family,
        string prompt,
        string solution,
        int x,
        int y,
        params (string Name, int Value)[] extra)
    {
        var values = extra
            .Prepend(("y", y))
            .Prepend(("x", x))
            .Prepend(("answerKind", 3))
            .ToArray();
        return new(
            family,
            prompt,
            solution,
            AssessmentItemType.ShortAnswer,
            $"<{x}, {y}>",
            Params(values));
    }

    private static IReadOnlyDictionary<string, int> Params(
        params (string Name, int Value)[] values) =>
        values.ToDictionary(value => value.Name, value => value.Value, StringComparer.Ordinal);

    private static int NonZero(Random random, int minInclusive, int maxExclusive)
    {
        var value = 0;
        while (value == 0)
            value = random.Next(minInclusive, maxExclusive);
        return value;
    }

    private static int Pow10(int exponent)
    {
        var result = 1;
        for (var i = 0; i < exponent; i++)
            result = checked(result * 10);
        return result;
    }

    private static int Pow(int value, int exponent)
    {
        var result = 1;
        for (var i = 0; i < exponent; i++)
            result = checked(result * value);
        return result;
    }

    private static int Gcd(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
            (a, b) = (b, a % b);
        return a == 0 ? 1 : a;
    }

    private static bool TryFraction(string answer, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 1;
        var text = answer.Trim();
        var parts = text.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 1)
        {
            return int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator);
        }

        return parts.Length == 2 &&
            int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator) &&
            int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator) &&
            denominator != 0;
    }

    private static bool TryPair(string answer, out int x, out int y)
    {
        x = 0;
        y = 0;
        var text = answer.Trim();
        if (text.Length >= 2 &&
            ((text[0] == '(' && text[^1] == ')') ||
             (text[0] == '<' && text[^1] == '>') ||
             (text[0] == '[' && text[^1] == ']')))
        {
            text = text[1..^1];
        }

        var parts = text.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return parts.Length == 2 &&
            int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out x) &&
            int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out y);
    }
}

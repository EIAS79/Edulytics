using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

public static class UsCcssAlg1Unit4FunctionQuestions
{
    public const string FunctionRelationFamily = "functions.relations.unique_output.classify";
    public const string FunctionOneToOneFamily = "functions.relations.one_to_one.classify";
    public const string FunctionImplicitClassificationFamily = "functions.relations.implicit.classify";
    public const string FunctionTableLookupFamily = "functions.table.lookup.invert";
    public const string FunctionQuadraticRootsFamily = "functions.quadratic.output_roots.solve";
    public const string FunctionNotationFamily = "functions.notation.evaluate_context";
    public const string FunctionDomainRangeFamily = "functions.domain_range.context_bounds";
    public const string FunctionPiecewiseFamily = "functions.piecewise.evaluate_interval";
    public const string FunctionAbsoluteErrorFamily = "functions.absolute_error.from_target";
    public const string FunctionAbsoluteTranslateFamily = "functions.absolute_value.translate_evaluate";
    public const string LinearExponentialCompareFamily = "functions.growth.linear_exponential.compare";
    public const string ExponentialGrowthEvaluateFamily = "functions.exponential.growth.evaluate";
    public const string ExponentialDecayHalfFamily = "functions.exponential.decay.half_evaluate";
    public const string QuadraticExpandMiddleFamily = "functions.quadratic.expand.middle_coefficient";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is FunctionRelationFamily or FunctionOneToOneFamily or FunctionImplicitClassificationFamily or FunctionTableLookupFamily or FunctionQuadraticRootsFamily or FunctionNotationFamily or FunctionDomainRangeFamily or FunctionPiecewiseFamily or FunctionAbsoluteErrorFamily or FunctionAbsoluteTranslateFamily or LinearExponentialCompareFamily or ExponentialGrowthEvaluateFamily or ExponentialDecayHalfFamily or QuadraticExpandMiddleFamily;

    public static Problem Build(string family, Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        if (family == FunctionNotationFamily)
        {
            var initial = random.Next(0, 21);
            var rate = random.Next(1, 10);
            var input = random.Next(0, 7);
            var variant = random.Next(0, 16);
            var output = initial + rate * input;
            return new(family,
                $"A named function H gives a quantity in metres after t hours and obeys H(t)={initial}+{rate}t. Evaluate H({input}). Enter the integer output only. Explain with the teacher what the input and output mean in context.",
                $"H({input})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["initial"] = initial, ["rate"] = rate,
                    ["input"] = input, ["variant"] = variant
                });
        }

        if (family == FunctionDomainRangeFamily)
        {
            var duration = random.Next(2, 9);
            var rate = random.Next(2, 10);
            var start = duration * rate;
            var variant = random.Next(0, 16);
            return new(family,
                $"A tank starts with {start} litres and drains at {rate} litres per hour. V(t)={start}-{rate}t is modeled only from t=0 through t={duration} hours. Enter the endpoint form exactly as D=0,{duration};R=0,{start}. Explain with the teacher why the context restricts both sets.",
                $"The modeled domain is 0 through {duration} hours and the range is 0 through {start} litres.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["duration"] = duration, ["rate"] = rate,
                    ["start"] = start, ["variant"] = variant
                });
        }

        if (family == FunctionPiecewiseFamily)
        {
            var boundary = random.Next(1, 6);
            var leftSlope = random.Next(1, 5);
            var leftIntercept = random.Next(0, 8);
            var rightSlope = random.Next(1, 5);
            var rightIntercept = random.Next(0, 8);
            var side = random.Next(0, 2);
            var input = side == 0 ? boundary : boundary + 1;
            var variant = random.Next(0, 16);
            var output = input <= boundary
                ? leftSlope * input + leftIntercept
                : rightSlope * input + rightIntercept;
            return new(family,
                $"A piecewise function P uses P(x)={leftSlope}x+{leftIntercept} for x<= {boundary}, and P(x)={rightSlope}x+{rightIntercept} for x>{boundary}. Find P({input}). Enter the integer output only. Explain which rule applies and how the endpoint should be drawn.",
                $"Because x={input} {(input <= boundary ? "is in the first interval" : "is in the second interval")}, P({input})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["boundary"] = boundary, ["leftSlope"] = leftSlope,
                    ["leftIntercept"] = leftIntercept, ["rightSlope"] = rightSlope,
                    ["rightIntercept"] = rightIntercept, ["side"] = side,
                    ["variant"] = variant
                });
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            var target = random.Next(20, 81);
            var delta = random.Next(1, 16) * (random.Next(0, 2) == 0 ? -1 : 1);
            var guess = target + delta;
            var variant = random.Next(0, 16);
            return new(family,
                $"The target value is {target} and a guess is {guess}. Find the absolute error |{guess}-{target}|. Enter the nonnegative integer only. Explain with the teacher why absolute error is a distance.",
                $"The absolute error is {Math.Abs(delta)}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["target"] = target, ["guess"] = guess, ["variant"] = variant
                });
        }

        if (family == FunctionAbsoluteTranslateFamily)
        {
            var center = random.Next(-5, 6);
            var vertical = random.Next(-4, 7);
            var input = random.Next(-8, 9);
            var variant = random.Next(0, 16);
            var output = Math.Abs(input - center) + vertical;
            return new(family,
                $"Let f(x)=|x-({center})|+({vertical}). Find f({input}). Enter the integer output only. Explain with the teacher how the center and vertical shift locate the V-shaped graph.",
                $"f({input})=|{input}-({center})|+({vertical})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["center"] = center, ["vertical"] = vertical, ["input"] = input, ["variant"] = variant
                });
        }

        if (family == LinearExponentialCompareFamily)
        {
            var initial = random.Next(2, 9);
            var addition = random.Next(2, 9);
            var factor = random.Next(2, 5);
            var steps = random.Next(2, 6);
            var linear = initial + addition * steps;
            var exponential = initial * (int)Math.Pow(factor, steps);
            return new(family,
                $"A begins at {initial} and adds {addition} each round. B begins at {initial} and multiplies by {factor} each round. Which is larger after {steps} rounds? Answer A, B, or E for equal. Explain both growth patterns to the teacher.",
                $"A={linear}, B={exponential}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["initial"] = initial, ["addition"] = addition, ["factor"] = factor, ["steps"] = steps
                });
        }

        if (family == ExponentialGrowthEvaluateFamily)
        {
            var initial = random.Next(2, 21);
            var factor = random.Next(2, 5);
            var exponent = random.Next(0, 6);
            var output = initial * (int)Math.Pow(factor, exponent);
            return new(family,
                $"An exponential quantity is given by G(t)={initial}*{factor}^t. Find G({exponent}) and enter the integer output. Explain the meaning of initial value and constant multiplier to the teacher.",
                $"G({exponent})={output}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["initial"] = initial, ["factor"] = factor, ["exponent"] = exponent
                });
        }

        if (family == ExponentialDecayHalfFamily)
        {
            var exponent = random.Next(1, 6);
            var multiplier = random.Next(1, 13);
            var initial = multiplier * (1 << exponent);
            return new(family,
                $"A model is D(t)={initial}*(1/2)^t. Find D({exponent}) and enter the integer result. Explain with the teacher why this represents a constant-factor decay.",
                $"D({exponent})={multiplier}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["initial"] = initial, ["exponent"] = exponent
                });
        }

        if (family == QuadraticExpandMiddleFamily)
        {
            var factorA = random.Next(-8, 9);
            var factorB = random.Next(-8, 9);
            return new(family,
                $"Expand (x+({factorA}))(x+({factorB})). What is the coefficient of x in the expanded standard form? Enter the integer only; show the full distribution and area diagram to the teacher.",
                $"The expanded form is x^2+({factorA + factorB})x+({factorA * factorB}).",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["a"] = factorA, ["b"] = factorB
                });
        }

        if (family == FunctionImplicitClassificationFamily)
        {
            var variant = random.Next(0, 5);
            var relation = variant switch
            {
                0 => "2x+3y=9",
                1 => "x=y²",
                2 => "x²+y²=9",
                3 => "y=x³",
                _ => "y=√(4−x)"
            };
            return new(family,
                $"Determine whether {relation} defines y as a function of real x in its permitted domain. Enter yes or no; justify domain and branches to the teacher.",
                variant is 0 or 3 or 4
                    ? "The relation assigns at most one real y-value to each permitted x."
                    : "At some permitted x the relation contains two different real y-values.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["variant"] = variant });
        }

        if (family == FunctionTableLookupFamily)
        {
            var first = random.Next(0, 50);
            var inverse = random.Next(0, 2);
            var prompt = $"Table: x=[1,2,3,4,5]; f(x)=[{first+1},{first+7},{first+2},{first+7},{first+3}].";
            prompt += inverse == 1
                ? $" Find all x-values in the table with f(x)={first+7}. Enter both inputs separated by commas."
                : " Evaluate f(3) using the table. Enter the integer output.";
            return new(family, prompt,
                inverse == 1 ? "Both x=2 and x=4 have the requested output." : $"f(3)={first+2}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["first"] = first, ["inverse"] = inverse });
        }

        if (family == FunctionQuadraticRootsFamily)
        {
            var rootA = random.Next(-5, 6);
            var rootB = random.Next(-5, 6);
            if (rootB == rootA)
                rootB = rootA == 5 ? -5 : rootA + 1;
            return new(family,
                $"Given f(x)=(x-({rootA}))(x-({rootB})), solve f(x)=0 over the reals. Enter both distinct integer roots separated by commas; show the zero-product reasoning to the teacher.",
                $"The two zero-product roots are {Math.Min(rootA,rootB)} and {Math.Max(rootA,rootB)}.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int> { ["rootA"] = rootA, ["rootB"] = rootB });
        }

        if (family == FunctionOneToOneFamily)
        {
            // Exactly one output per input in every generated problem.
            // The other question is injectivity: do any distinct inputs
            // have a shared output? This is not a graph-drawing grader.
            var x0 = random.Next(-4, 9);
            var y0 = random.Next(-9, 10);
            var step = random.Next(1, 5);
            var injective = random.Next(0, 2);
            var y1 = y0 + step;
            var y2 = injective == 1 ? y0 + 2 * step : y0;
            return new(family,
                $"A relation has ordered pairs ({x0},{y0}), ({x0 + 1},{y1}), ({x0 + 2},{y2}). Is it a one-to-one function of the first coordinate? Enter yes or no. Explain to your teacher why repeating an output is different from repeating an input.",
                $"All inputs are distinct, so it is a function. {(injective == 1 ? "All outputs are distinct; it is one-to-one." : "The first and third inputs share an output; it is not one-to-one.")}",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["x0"] = x0, ["y0"] = y0,
                    ["step"] = step, ["injective"] = injective
                });
        }

        if (family != FunctionRelationFamily)
            throw new InvalidOperationException("Unsupported family.");

        var x = random.Next(-4, 9);
        var a = random.Next(-9, 10);
        var b = random.Next(-9, 10);
        var repeats = random.Next(0, 2);
        if (repeats == 1 && a == b) b = a == 9 ? 8 : a + 1;
        var lastX = repeats == 1 ? x : x + 1;
        var relationVariant = random.Next(0, 16);
        return new(family,
            $"Is the relation ({x},{a}), ({x + 2},{a + 3}), ({lastX},{b}) a function of its first coordinate? Enter yes or no. Explain graph features with the teacher.",
            "A function assigns exactly one output to each input.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["x"] = x, ["a"] = a, ["b"] = b,
                ["repeats"] = repeats, ["variant"] = relationVariant
            });
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (family == FunctionNotationFamily)
        {
            if (!ValidateNotation(parameters, out var initial, out var rate, out var input, out _))
                throw new InvalidOperationException("Invalid function-notation parameters.");
            return (initial + rate * input).ToString();
        }

        if (family == FunctionDomainRangeFamily)
        {
            if (!ValidateDomainRange(parameters, out var duration, out _, out var start, out _))
                throw new InvalidOperationException("Invalid domain-range parameters.");
            return $"D=0,{duration};R=0,{start}";
        }

        if (family == FunctionPiecewiseFamily)
        {
            if (!ValidatePiecewise(parameters, out var boundary, out var leftSlope, out var leftIntercept,
                    out var rightSlope, out var rightIntercept, out var side, out _))
                throw new InvalidOperationException("Invalid piecewise parameters.");
            var input = side == 0 ? boundary : boundary + 1;
            return (input <= boundary ? leftSlope * input + leftIntercept : rightSlope * input + rightIntercept).ToString();
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            if (!ValidateAbsoluteError(parameters, out var target, out var guess, out _))
                throw new InvalidOperationException("Invalid absolute-error parameters.");
            return Math.Abs(guess - target).ToString();
        }

        if (family == FunctionAbsoluteTranslateFamily)
        {
            if (!ValidateAbsoluteTranslate(parameters, out var center, out var vertical, out var input, out _))
                throw new InvalidOperationException("Invalid absolute-value translation parameters.");
            return (Math.Abs(input - center) + vertical).ToString();
        }

        if (family == LinearExponentialCompareFamily)
        {
            if (!ValidateGrowth(parameters, out var initial, out var addition, out var factor, out var steps))
                throw new InvalidOperationException("Invalid growth parameters.");
            var linear = initial + addition * steps;
            var exponential = initial * (int)Math.Pow(factor, steps);
            return linear > exponential ? "A" : exponential > linear ? "B" : "E";
        }

        if (family == ExponentialGrowthEvaluateFamily)
        {
            if (!ValidateExponentialGrowth(parameters, out var initial, out var factor, out var exponent))
                throw new InvalidOperationException("Invalid exponential growth parameters.");
            return (initial * (int)Math.Pow(factor, exponent)).ToString();
        }

        if (family == ExponentialDecayHalfFamily)
        {
            if (!ValidateDecayHalf(parameters, out var initial, out var exponent))
                throw new InvalidOperationException("Invalid exponential-decay parameters.");
            return (initial / (1 << exponent)).ToString();
        }

        if (family == QuadraticExpandMiddleFamily)
        {
            if (!ValidateQuadraticExpand(parameters, out var factorA, out var factorB))
                throw new InvalidOperationException("Invalid quadratic expansion parameters.");
            return (factorA + factorB).ToString();
        }

        if (family == FunctionImplicitClassificationFamily)
        {
            if (!ValidateImplicit(parameters, out var variant))
                throw new InvalidOperationException("Invalid implicit relation parameters.");
            return variant is 0 or 3 or 4 ? "yes" : "no";
        }
        if (family == FunctionTableLookupFamily)
        {
            if (!ValidateTableLookup(parameters, out var first, out var inverse))
                throw new InvalidOperationException("Invalid tabular lookup parameters.");
            return inverse == 1 ? "2,4" : (first+2).ToString();
        }
        if (family == FunctionQuadraticRootsFamily)
        {
            if (!ValidateQuadraticRoots(parameters, out var rootA, out var rootB))
                throw new InvalidOperationException("Invalid quadratic root parameters.");
            return $"{Math.Min(rootA,rootB)},{Math.Max(rootA,rootB)}";
        }

        if (family == FunctionOneToOneFamily)
        {
            if (!ValidateOneToOne(parameters, out _, out _, out _, out var injective))
                throw new InvalidOperationException("Invalid one-to-one classification parameters.");
            return injective == 1 ? "yes" : "no";
        }

        if (family != FunctionRelationFamily ||
            !ValidateRelation(parameters, out _, out var a, out var b, out var repeats, out _))
            throw new InvalidOperationException("Invalid relation parameters.");
        return repeats == 1 && a != b ? "no" : "yes";
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer)
    {
        if (string.IsNullOrWhiteSpace(answer)) return false;

        if (family == FunctionNotationFamily)
        {
            if (!ValidateNotation(parameters, out _, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family == FunctionDomainRangeFamily)
        {
            if (!ValidateDomainRange(parameters, out _, out _, out _, out _)) return false;
            var normalized = new string(answer.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToUpperInvariant();
            return normalized == Solve(family, parameters).ToUpperInvariant();
        }

        if (family == FunctionPiecewiseFamily)
        {
            if (!ValidatePiecewise(parameters, out _, out _, out _, out _, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family == FunctionAbsoluteErrorFamily)
        {
            if (!ValidateAbsoluteError(parameters, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family == FunctionAbsoluteTranslateFamily)
        {
            if (!ValidateAbsoluteTranslate(parameters, out _, out _, out _, out _) ||
                !int.TryParse(answer.Trim(), out var submitted))
                return false;
            return submitted.ToString() == Solve(family, parameters);
        }

        if (family == LinearExponentialCompareFamily)
            return ValidateGrowth(parameters, out _, out _, out _, out _) &&
                   string.Equals(answer.Trim(), Solve(family, parameters), StringComparison.OrdinalIgnoreCase);

        if (family == ExponentialGrowthEvaluateFamily)
            return ValidateExponentialGrowth(parameters, out _, out _, out _) &&
                   int.TryParse(answer.Trim(), out var value) &&
                   value.ToString() == Solve(family, parameters);

        if (family == ExponentialDecayHalfFamily)
            return ValidateDecayHalf(parameters, out _, out _) &&
                   int.TryParse(answer.Trim(), out var value) &&
                   value.ToString() == Solve(family, parameters);

        if (family == QuadraticExpandMiddleFamily)
            return ValidateQuadraticExpand(parameters, out _, out _) &&
                   int.TryParse(answer.Trim(), out var value) &&
                   value.ToString() == Solve(family, parameters);

        if (family == FunctionImplicitClassificationFamily)
        {
            if (!ValidateImplicit(parameters, out _)) return false;
            var value = answer.Trim().ToLowerInvariant();
            return (value is "yes" or "no") && value == Solve(family, parameters);
        }
        if (family == FunctionTableLookupFamily)
        {
            if (!ValidateTableLookup(parameters, out var first, out var inverse))
                return false;
            if (inverse == 0)
                return int.TryParse(answer.Trim(), out var output) && output == first + 2;
            return EqualIntegerPair(answer, 2, 4);
        }
        if (family == FunctionQuadraticRootsFamily)
        {
            if (!ValidateQuadraticRoots(parameters, out var rootA, out var rootB))
                return false;
            return EqualIntegerPair(answer, rootA, rootB);
        }

        if (family == FunctionOneToOneFamily)
        {
            if (!ValidateOneToOne(parameters, out _, out _, out _, out _))
                return false;
            var normalized = answer.Trim().ToLowerInvariant();
            return normalized is "yes" or "no" &&
                   normalized == Solve(family, parameters);
        }

        if (family != FunctionRelationFamily ||
            !ValidateRelation(parameters, out _, out _, out _, out _, out _))
            return false;

        var relationAnswer = answer.Trim().ToLowerInvariant() switch
        {
            "yes" or "y" or "true" => "yes",
            "no" or "n" or "false" => "no",
            _ => ""
        };
        return relationAnswer.Length > 0 && relationAnswer == Solve(family, parameters);
    }

    private static bool ValidateRelation(IReadOnlyDictionary<string, int>? p,
        out int x, out int a, out int b, out int repeats, out int variant)
    {
        x = a = b = repeats = variant = 0;
        return p is not null &&
            p.TryGetValue("x", out x) && x is >= -4 and <= 8 &&
            p.TryGetValue("a", out a) && a is >= -9 and <= 9 &&
            p.TryGetValue("b", out b) && b is >= -9 and <= 10 &&
            p.TryGetValue("repeats", out repeats) && repeats is >= 0 and <= 1 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateImplicit(IReadOnlyDictionary<string,int>? p,
        out int variant)
    {
        variant = 0;
        return p is not null && p.TryGetValue("variant", out variant) &&
            variant is >= 0 and <= 4;
    }

    private static bool ValidateTableLookup(IReadOnlyDictionary<string,int>? p,
        out int first, out int inverse)
    {
        first = inverse = 0;
        return p is not null &&
            p.TryGetValue("first", out first) && first is >= 0 and <= 49 &&
            p.TryGetValue("inverse", out inverse) && inverse is >= 0 and <= 1;
    }

    private static bool ValidateQuadraticRoots(IReadOnlyDictionary<string,int>? p,
        out int rootA, out int rootB)
    {
        rootA = rootB = 0;
        return p is not null &&
            p.TryGetValue("rootA", out rootA) && rootA is >= -5 and <= 5 &&
            p.TryGetValue("rootB", out rootB) && rootB is >= -5 and <= 5 &&
            rootA != rootB;
    }

    private static bool EqualIntegerPair(string answer, int expectedA, int expectedB)
    {
        var parts = answer.Split([',', ';'], StringSplitOptions.TrimEntries);
        return parts.Length == 2 &&
            int.TryParse(parts[0], out var a) &&
            int.TryParse(parts[1], out var b) &&
            a != b &&
            ((a == expectedA && b == expectedB) ||
             (a == expectedB && b == expectedA));
    }

    private static bool ValidateOneToOne(IReadOnlyDictionary<string, int>? p,
        out int x0, out int y0, out int step, out int injective)
    {
        x0 = y0 = step = injective = 0;
        return p is not null &&
            p.TryGetValue("x0", out x0) && x0 is >= -4 and <= 8 &&
            p.TryGetValue("y0", out y0) && y0 is >= -9 and <= 9 &&
            p.TryGetValue("step", out step) && step is >= 1 and <= 4 &&
            p.TryGetValue("injective", out injective) && injective is >= 0 and <= 1;
    }

    private static bool ValidateNotation(IReadOnlyDictionary<string, int>? p,
        out int initial, out int rate, out int input, out int variant)
    {
        initial = rate = input = variant = 0;
        return p is not null &&
            p.TryGetValue("initial", out initial) && initial is >= 0 and <= 20 &&
            p.TryGetValue("rate", out rate) && rate is >= 1 and <= 9 &&
            p.TryGetValue("input", out input) && input is >= 0 and <= 6 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateDomainRange(IReadOnlyDictionary<string, int>? p,
        out int duration, out int rate, out int start, out int variant)
    {
        duration = rate = start = variant = 0;
        return p is not null &&
            p.TryGetValue("duration", out duration) && duration is >= 2 and <= 8 &&
            p.TryGetValue("rate", out rate) && rate is >= 2 and <= 9 &&
            p.TryGetValue("start", out start) && start == duration * rate &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidatePiecewise(IReadOnlyDictionary<string, int>? p,
        out int boundary, out int leftSlope, out int leftIntercept, out int rightSlope,
        out int rightIntercept, out int side, out int variant)
    {
        boundary = leftSlope = leftIntercept = rightSlope = rightIntercept = side = variant = 0;
        return p is not null &&
            p.TryGetValue("boundary", out boundary) && boundary is >= 1 and <= 5 &&
            p.TryGetValue("leftSlope", out leftSlope) && leftSlope is >= 1 and <= 4 &&
            p.TryGetValue("leftIntercept", out leftIntercept) && leftIntercept is >= 0 and <= 7 &&
            p.TryGetValue("rightSlope", out rightSlope) && rightSlope is >= 1 and <= 4 &&
            p.TryGetValue("rightIntercept", out rightIntercept) && rightIntercept is >= 0 and <= 7 &&
            p.TryGetValue("side", out side) && side is >= 0 and <= 1 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateAbsoluteError(IReadOnlyDictionary<string, int>? p,
        out int target, out int guess, out int variant)
    {
        target = guess = variant = 0;
        return p is not null &&
            p.TryGetValue("target", out target) && target is >= 20 and <= 80 &&
            p.TryGetValue("guess", out guess) && guess is >= 5 and <= 95 &&
            Math.Abs(guess - target) is >= 1 and <= 15 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }


    private static bool ValidateAbsoluteTranslate(IReadOnlyDictionary<string, int>? p,
        out int center, out int vertical, out int input, out int variant)
    {
        center = vertical = input = variant = 0;
        return p is not null &&
            p.TryGetValue("center", out center) && center is >= -5 and <= 5 &&
            p.TryGetValue("vertical", out vertical) && vertical is >= -4 and <= 6 &&
            p.TryGetValue("input", out input) && input is >= -8 and <= 8 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateGrowth(IReadOnlyDictionary<string, int>? p,
        out int initial, out int addition, out int factor, out int steps)
    {
        initial = addition = factor = steps = 0;
        return p is not null &&
            p.TryGetValue("initial", out initial) && initial is >= 2 and <= 8 &&
            p.TryGetValue("addition", out addition) && addition is >= 2 and <= 8 &&
            p.TryGetValue("factor", out factor) && factor is >= 2 and <= 4 &&
            p.TryGetValue("steps", out steps) && steps is >= 2 and <= 5;
    }

    private static bool ValidateExponentialGrowth(IReadOnlyDictionary<string, int>? p,
        out int initial, out int factor, out int exponent)
    {
        initial = factor = exponent = 0;
        return p is not null &&
            p.TryGetValue("initial", out initial) && initial is >= 2 and <= 20 &&
            p.TryGetValue("factor", out factor) && factor is >= 2 and <= 4 &&
            p.TryGetValue("exponent", out exponent) && exponent is >= 0 and <= 5;
    }

    private static bool ValidateDecayHalf(IReadOnlyDictionary<string, int>? p,
        out int initial, out int exponent)
    {
        initial = exponent = 0;
        return p is not null &&
            p.TryGetValue("initial", out initial) && initial > 0 &&
            p.TryGetValue("exponent", out exponent) && exponent is >= 1 and <= 5 &&
            initial % (1 << exponent) == 0 &&
            initial / (1 << exponent) is >= 1 and <= 12;
    }

    private static bool ValidateQuadraticExpand(IReadOnlyDictionary<string, int>? p,
        out int a, out int b)
    {
        a = b = 0;
        return p is not null &&
            p.TryGetValue("a", out a) && a is >= -8 and <= 8 &&
            p.TryGetValue("b", out b) && b is >= -8 and <= 8;
    }

}

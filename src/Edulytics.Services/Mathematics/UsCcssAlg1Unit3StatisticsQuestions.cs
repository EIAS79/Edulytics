using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Bounded first-party objective questions for Algebra 1 Unit 3 statistics.
/// Graphing, genuine data collection, contextual interpretations and causal
/// arguments are reserved for teacher review.
/// </summary>
public static class UsCcssAlg1Unit3StatisticsQuestions
{
    public const string TwoWayCount = "statistics.two_way.marginal_count";
    public const string TwoWayRelative = "statistics.two_way.conditional_relative";
    public const string TwoWayAssociation = "statistics.two_way.association";
    public const string LinearPrediction = "statistics.linear_model.predict";
    public const string FitSelection = "statistics.linear_model.fit_select";
    public const string Residual = "statistics.linear_model.residual";
    public const string CorrelationSign = "statistics.correlation.direction";
    public const string CorrelationRounded = "statistics.correlation.rounded";
    public const string AssociationCausality = "statistics.association.confounder";

    public sealed record Problem(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is TwoWayCount or TwoWayRelative or TwoWayAssociation or
            LinearPrediction or FitSelection or Residual or
            CorrelationSign or CorrelationRounded or AssociationCausality;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 U03 statistics family.");
        ArgumentNullException.ThrowIfNull(random);
        var option = random.Next(0, 3);
        var index = random.Next(0, 4);
        var sign = random.Next(0, 2);
        var variant = random.Next(0, 16);
        var p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["option"] = option,
            ["index"] = index,
            ["sign"] = sign,
            ["variant"] = variant
        };
        if (family == TwoWayCount)
        {
            return new(family,
                "In a fictional club survey, Group A has Sports12 and Arts8; " +
                "Group B has Sports9 and Arts11. How many respondents selected Sports in total?",
                "Add the Sports counts from both groups.",
                AssessmentItemType.Numeric, p);
        }
        if (family == TwoWayRelative)
        {
            var q = option switch
            {
                0 => "What percent of Group A chose Sports? (row-relative)",
                1 => "Of all Sports respondents, what percent were in Group A? (column-relative)",
                2 => "What percent of all respondents were Group A Sports? (joint-relative)",
                _ => throw new InvalidOperationException()
            };
            return new(family,
                "Fictional two-way data: Group A Sports12/Arts8, Group B Sports9/Arts11. " +
                q + " Enter a percent rounded to two decimal places without the % sign.",
                "Use the denominator specified by the group, Sports column, or entire sample.",
                AssessmentItemType.Numeric, p);
        }
        if (family == TwoWayAssociation)
        {
            return new(family,
                "Fictional preferences: Group A has Sports12/Arts8 and Group B has " +
                "Sports9/Arts11, with 20 respondents in each group. Which group has the " +
                "higher within-group percentage choosing Sports? Enter A or B. " +
                "Interpreting association versus causation remains teacher-reviewed.",
                "Compare 12/20 with 9/20, not raw column totals.",
                AssessmentItemType.ShortAnswer, p);
        }
        if (family == LinearPrediction)
        {
            return new(family,
                $"A hand-fit model predicts y=2.4x+1.9. What does it predict at x=${index + 2}? " +
                "Enter the number rounded to one decimal place. State contextual limitations with the teacher.",
                "Substitute x into the linear model.",
                AssessmentItemType.Numeric, p);
        }
        if (family == FitSelection)
        {
            var betterFirst = option != 1;
            var a = betterFirst ? "y=2.4x+1.9" : "y=3x+2";
            var b = betterFirst ? "y=3x+2" : "y=2.4x+1.9";
            p["option"] = betterFirst ? 0 : 1;
            return new(family,
                "For points (0,2),(1,4),(2,7),(3,9), which line has smaller " +
                $"sum of squared residuals? A) ${a}; B) ${b}. Enter A or B. " +
                "Explain the visual goodness-of-fit with the teacher.",
                "Compute each observed minus predicted residual, square and sum.",
                AssessmentItemType.ShortAnswer, p);
        }
        if (family == Residual)
        {
            var observed = new[] { 2, 4, 7, 9 };
            return new(family,
                $"At x=${index}, observed y=${observed[index]} while predicted y=2.4x+1.9. " +
                "What is observed minus predicted? Enter the signed residual to one decimal place.",
                "Residual = observed - model prediction; preserve the sign.",
                AssessmentItemType.Numeric, p);
        }
        if (family == CorrelationSign)
        {
            return new(family,
                sign == 0
                    ? "Points (1,2),(2,4),(3,6),(4,8) lie on a perfect line. " +
                      "Does r indicate a positive or negative linear relationship?"
                    : "Points (1,8),(2,6),(3,4),(4,2) lie on a perfect line. " +
                      "Does r indicate a positive or negative linear relationship?",
                "The sign of the slope gives the sign of Pearson r for a perfect nonconstant line.",
                AssessmentItemType.ShortAnswer, p);
        }
        if (family == CorrelationRounded)
        {
            var data = sign == 0
                ? "(0,2),(1,4),(2,7),(3,9)"
                : "(0,9),(1,7),(2,4),(3,2)";
            return new(family,
                $"For paired observations ${data}, calculate Pearson's correlation coefficient r " +
                "rounded to three decimal places. Enter a signed decimal. " +
                "Show the calculation and discuss limits with the teacher.",
                "Use sum centered cross-products divided by sqrt(x squared-deviation sum × y squared-deviation sum).",
                AssessmentItemType.Numeric, p);
        }

        var context = option switch
        {
            0 => "Used-car mileage and selling price may both depend on what unrecorded feature?",
            1 => "Ice cream sales and beach accidents may both depend on what unrecorded variable?",
            2 => "Study time and exam marks may both depend on what prior characteristic?",
            _ => throw new InvalidOperationException()
        };
        var correct = option switch { 0 => "vehicle age", 1 => "weather temperature", 2 => "prior academic preparation", _ => "" };
        return new(family,
            context + " Enter A: " + correct +
            "; B: the row number in the table; C: the chart's font size. " +
            "Causal identification and study design remain teacher-reviewed.",
            "Identify a plausible common cause, not an arbitrary display property.",
            AssessmentItemType.ShortAnswer, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family) || !Valid(p, out var option, out var index, out var sign, out _))
            throw new InvalidOperationException("Invalid ALG1 U03 parameters.");

        return family switch
        {
            TwoWayCount => "21",
            TwoWayRelative => option switch
            {
                0 => "60",
                1 => "57.14",
                2 => "30",
                _ => throw new InvalidOperationException()
            },
            TwoWayAssociation => "a",
            LinearPrediction => ((24m * (index + 2) + 19m) / 10m)
                .ToString(CultureInfo.InvariantCulture),
            FitSelection => option == 0 ? "a" : "b",
            Residual => index switch
            {
                0 => "0.1", 1 => "-0.3", 2 => "0.3", 3 => "-0.1",
                _ => throw new InvalidOperationException()
            },
            CorrelationSign => sign == 0 ? "positive" : "negative",
            CorrelationRounded => sign == 0 ? "0.997" : "-0.997",
            AssociationCausality => "a",
            _ => throw new InvalidOperationException()
        };
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer) ||
            !Valid(p, out _, out _, out _, out _))
            return false;

        var expected = Solve(family, p);
        if (family is TwoWayCount or TwoWayRelative or LinearPrediction or Residual or CorrelationRounded)
        {
            const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
            return decimal.TryParse(answer.Trim(), styles, CultureInfo.InvariantCulture, out var actual) &&
                decimal.TryParse(expected, styles, CultureInfo.InvariantCulture, out var target) &&
                actual == target;
        }

        var submitted = answer.Trim().ToLowerInvariant();
        if (submitted.StartsWith("option ", StringComparison.Ordinal))
            submitted = submitted["option ".Length..];
        return submitted == expected;
    }

    private static bool Valid(IReadOnlyDictionary<string, int>? p,
        out int option, out int index, out int sign, out int variant)
    {
        option = index = sign = variant = 0;
        return p is not null &&
            p.TryGetValue("option", out option) && option is >= 0 and <= 2 &&
            p.TryGetValue("index", out index) && index is >= 0 and <= 3 &&
            p.TryGetValue("sign", out sign) && sign is >= 0 and <= 1 &&
            p.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }
}

using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Approved in-place Cambridge content upgrades for legacy lessons that were
/// replaced during the official mapping rebuild. Lesson identities remain
/// stable while learner-facing bodies move to reviewed Cambridge-aligned scope.
/// </summary>
public static class CambridgeOfficialMappingContentCorrections
{
    public const string CorrectionContentVersion =
        "cambridge-official-mapping-content-v2";

    private const string PriorCorrectionContentVersion =
        "cambridge-official-mapping-content-v1";

    private const string PriorSupportingPracticeContentVersion =
        "supporting-practice-remediation-v1";

    private const string CurrentSupportingPracticeContentVersion =
        "supporting-practice-remediation-v2";

    private static readonly HashSet<string> TargetLessonCodes =
        new(StringComparer.Ordinal)
        {
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-1:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-1:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S4:4F-2:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-5:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-5:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S6:6NPV-4:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:03:07:PYTHAGORAS-THEOREM",
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:03:07:PYTHAGORAS-THEOREM",
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:03:09:RIGHT-TRIANGLE-REASONING",
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:03:09:TRIGONOMETRIC-RATIOS-FOUNDATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L10:CORE:07:02:VECTOR-ARITHMETIC",
            "PED:CAMBRIDGE-INTL-MATH:L11:CORE:07:02:CONSOLIDATING-VECTOR-ARITHMETIC",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:02:08:CORRELATION-AND-REGRESSION-REASONING",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:03:01:PROJECTILES",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:03:05:CIRCULAR-MOTION-FOUNDATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:03:06:EQUILIBRIUM-OF-RIGID-BODIES"
        };

    private static readonly HashSet<string> SemanticRepairLessonCodes =
        new(StringComparer.Ordinal)
        {
            "PED:CAMBRIDGE-INTL-MATH:L10:CORE:01:01:INTEGERS-AND-ORDER-OF-OPERATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:01:01:INTEGERS-AND-ORDER-OF-OPERATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L11:CORE:01:01:CONSOLIDATING-INTEGERS-AND-ORDER-OF-OPERATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:01:01:CONSOLIDATING-INTEGERS-AND-ORDER-OF-OPERATIONS",
            "PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:10:05:QUADRATIC-FORMULA",
            "PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:10:05:CONSOLIDATING-QUADRATIC-FORMULA",
            "PED:CAMBRIDGE-INTL-MATH:L10:EXTENDED:11:03:SIMILARITY-WITH-AREA-AND-VOLUME",
            "PED:CAMBRIDGE-INTL-MATH:L11:EXTENDED:11:03:CONSOLIDATING-SIMILARITY-WITH-AREA-AND-VOLUME",
            "PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:02:QUADRATIC-FUNCTIONS",
            "PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:12:INTEGRATION",
            "PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:13:APPLICATIONS-OF-INTEGRATION",
            "PED:CAMBRIDGE-INTL-MATH:L12:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:02:07:BINOMIAL-DISTRIBUTION",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:07:INTEGRATION-TECHNIQUES",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:01:11:COMPLEX-MODELLING-WITH-FUNCTIONS",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:02:02:CONTINUOUS-RANDOM-VARIABLES",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:02:04:ESTIMATION",
            "PED:CAMBRIDGE-INTL-MATH:L13:COMPONENT-ROUTE-STRUCTURE-PRESERVED-IN-REFERENCE-GRAPH:02:07:LINEAR-COMBINATIONS-OF-RANDOM-VARIABLES",
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:01:08:STANDARD-FORM-AND-ESTIMATION",
            "PED:CAMBRIDGE-INTL-MATH:L7:SHARED:03:06:SURFACE-AREA-AND-VOLUME",
            "PED:CAMBRIDGE-INTL-MATH:L8:SHARED:03:06:SURFACE-AREA-AND-VOLUME",
            "PED:CAMBRIDGE-INTL-MATH:L9:SHARED:03:06:SURFACE-AREA-AND-VOLUME",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-1:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-1:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-2:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-2:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-3:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S4:4G-3:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-2:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-2:APPLY",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-3:BUILD",
            "PED:CAMBRIDGE-INTL-MATH:S5:5NPV-3:APPLY"
        };

    public static bool IsTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        string.Equals(
            document.PackCode,
            CambridgePrimaryStage6LessonContentCorrections.PackCode,
            StringComparison.Ordinal) &&
        (TargetLessonCodes.Contains(lesson.LessonCode) ||
         SemanticRepairLessonCodes.Contains(lesson.LessonCode));

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string priorExpectedVersion) =>
        IsTarget(document, lesson)
            ? CorrectionContentVersion
            : priorExpectedVersion;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        IsTarget(document, lesson) &&
        (
            string.Equals(
                existingContentVersion,
                document.ContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                CambridgePrimaryStage6LessonContentCorrections.CorrectionContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                PriorCorrectionContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                PriorSupportingPracticeContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                CurrentSupportingPracticeContentVersion,
                StringComparison.Ordinal) ||
            string.Equals(
                existingContentVersion,
                CorrectionContentVersion,
                StringComparison.Ordinal)
        );

    public static void ApplyApprovedCorrections(
        CanonicalLessonContentPackDocument document)
    {
        if (!string.Equals(
                document.PackCode,
                CambridgePrimaryStage6LessonContentCorrections.PackCode,
                StringComparison.Ordinal))
        {
            return;
        }

        foreach (var lesson in document.Lessons)
        {
            if (!SemanticRepairLessonCodes.Contains(lesson.LessonCode))
                continue;

            var translation =
                lesson.Translations.FirstOrDefault(x =>
                    x.CultureCode.StartsWith(
                        "en",
                        StringComparison.OrdinalIgnoreCase))
                ?? throw new InvalidOperationException(
                    $"Cambridge semantic repair requires English content for {lesson.LessonCode}.");

            ApplySemanticRepair(lesson.LessonCode, translation);
        }
    }

    private static void ApplySemanticRepair(
        string lessonCode,
        CanonicalLessonContentPackTranslation translation)
    {
        if (lessonCode.Contains(
                "INTEGERS-AND-ORDER-OF-OPERATIONS",
                StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Integers include positive values, zero and negative values. Order of operations determines which calculation is performed first, while integer sign rules determine the direction and value of each result.",
                "Use brackets first, then powers, multiplication and division, then addition and subtraction. For directed numbers, keep each negative sign attached to its number and verify the final sign.",
                "Example: evaluate -6 + 14 ÷ 2 × 3. Division and multiplication come before addition: 14 ÷ 2 = 7, then 7 × 3 = 21, so -6 + 21 = 15. The negative integer -6 is not changed until the final addition.",
                "Identify the operation priority, calculate each higher-priority operation from left to right, then combine the remaining signed integers and check the sign on a number line.",
                "Do not work strictly left to right across different operation priorities, and do not drop a negative sign.",
                "Apply operation priority and directed-number sign rules together.");
            return;
        }

        if (lessonCode.Contains("QUADRATIC-FORMULA", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "The quadratic formula solves ax² + bx + c = 0 when a is non-zero. It is especially useful when a quadratic does not factorise conveniently.",
                "For ax² + bx + c = 0, use x = (-b ± √(b² - 4ac)) / (2a). The discriminant b² - 4ac indicates whether the real roots are distinct, repeated or absent.",
                "Example: solve x² - 5x + 3 = 0. Here a=1, b=-5, c=3. The discriminant is 25-12=13, so x=(5±√13)/2. Substituting either root into the quadratic gives zero.",
                "Write the equation in standard quadratic form, identify a, b and c with signs, calculate the discriminant, substitute into the formula, simplify both roots, then check by substitution.",
                "Do not lose the sign of b, forget the ± symbol, or divide only the square-root term by 2a.",
                "Use the quadratic formula with the correct coefficients and verify the roots.");
            return;
        }

        if (lessonCode.Contains(
                "SIMILARITY-WITH-AREA-AND-VOLUME",
                StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Similar solids have equal corresponding angles and a constant linear scale factor. Area and surface area scale with the square of the linear factor, while volume scales with its cube.",
                "If the linear scale factor is k, corresponding lengths multiply by k, areas by k² and volumes by k³.",
                "Example: two similar solids have linear scale factor 3. A surface area of 20 cm² becomes 20×3²=180 cm². A volume of 8 cm³ becomes 8×3³=216 cm³. The different powers reflect two-dimensional area and three-dimensional volume.",
                "Find the linear scale factor from corresponding lengths, square it for area or surface area, cube it for volume, multiply the known measure, and check that the units are squared or cubed appropriately.",
                "Do not use the linear scale factor directly for area or volume, and do not mix corresponding dimensions.",
                "For similar shapes and solids, use k for length, k² for area and k³ for volume.");
            return;
        }

        if (lessonCode.EndsWith(":01:02:QUADRATIC-FUNCTIONS", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "A quadratic function has a highest power of two and its graph is a parabola. Its roots, turning point and axis of symmetry describe key features of the function.",
                "For f(x)=ax²+bx+c with a non-zero, solve f(x)=0 for roots and use x=-b/(2a) for the axis of symmetry.",
                "Example: f(x)=x²-4x+3=(x-1)(x-3). The roots are 1 and 3. The axis of symmetry is x=2, and f(2)=-1, so the turning point is (2,-1).",
                "Identify a, b and c, find roots where possible, calculate the axis of symmetry, evaluate the function there, and relate these values to the parabola.",
                "Do not confuse the roots f(x)=0 with the y-intercept f(0), and do not treat a quadratic function as linear.",
                "Connect the algebraic form of a quadratic function to its roots and turning point.");
            return;
        }

        if (lessonCode.Contains("INTEGRATION", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Integration reverses differentiation and accumulates change. Indefinite integrals give a family of antiderivatives, while definite integrals measure signed accumulation over an interval.",
                "For n≠-1, ∫x^n dx = x^(n+1)/(n+1)+C. For a definite integral, evaluate an antiderivative at the upper and lower limits and subtract.",
                "Example: ∫(3x²+2) dx = x³+2x+C. Over 0≤x≤2, ∫₀²(3x²+2) dx = [x³+2x]₀² = 12. For area under a curve, interpret the sign and interval in context.",
                "Choose a valid integration rule, find the antiderivative term by term, include C for an indefinite integral, apply limits for a definite integral, and differentiate the result as a check.",
                "Do not forget the constant of integration for indefinite work or reverse the upper-minus-lower evaluation.",
                "Integrate using a valid antiderivative and verify by differentiation.");
            return;
        }

        if (lessonCode.EndsWith(":02:07:BINOMIAL-DISTRIBUTION", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "A binomial distribution models the number of successes in a fixed number of independent trials when each trial has the same success probability.",
                "If X~B(n,p), then P(X=r)=C(n,r)p^r(1-p)^(n-r), with mean np and variance np(1-p).",
                "Example: if X~B(5,0.4), then P(X=2)=C(5,2)(0.4)²(0.6)³=0.3456. The distribution assigns a probability to each possible success count from 0 to 5.",
                "Check the binomial conditions, identify n and p, choose the required success count or range, calculate the relevant probability terms, and check the result lies between 0 and 1.",
                "Do not use a binomial model when trials are dependent or the success probability changes from trial to trial.",
                "Use the binomial distribution only when its fixed-trial, independent, constant-probability conditions hold.");
            return;
        }

        if (lessonCode.EndsWith(":01:11:COMPLEX-MODELLING-WITH-FUNCTIONS", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "A function model links an input variable to an output according to a defined rule. Composite, inverse or transformed functions can represent multi-stage relationships.",
                "State the domain that is meaningful in context, evaluate the function consistently, and interpret parameters and outputs rather than treating the formula as context-free.",
                "Example: f(t)=120e^(-0.2t) models a decreasing quantity. The function gives f(0)=120 and f(5)=120e^-1. Comparing these outputs shows how the model changes with time.",
                "Define the variables and domain, choose the function rule, substitute inputs, calculate outputs, inspect the behaviour of the function, and interpret the result in the modelled context.",
                "Do not use inputs outside the meaningful domain or confuse an algebraic output with its contextual units.",
                "A valid function model must match both the mathematical rule and the context.");
            return;
        }

        if (lessonCode.Contains(":02:02:CONTINUOUS-RANDOM-VARIABLES", StringComparison.Ordinal) ||
            lessonCode.Contains(":02:04:ESTIMATION", StringComparison.Ordinal) ||
            lessonCode.Contains(":02:07:LINEAR-COMBINATIONS-OF-RANDOM-VARIABLES", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Probability distributions describe how probability is assigned to values of a random variable. Continuous variables use density over intervals, while estimation uses sample information to infer an unknown population quantity.",
                "For a valid continuous density, total area is 1 and interval probabilities are areas under the density. For linear combinations, expectation is linear; for independent variables, variances combine with squared coefficients.",
                "Example: if a continuous random variable X has density f(x)=2x for 0≤x≤1, then P(X≤0.5)=∫₀^0.5 2x dx=0.25. A sample estimate should state the statistic used and the population parameter it estimates.",
                "Identify the random variable and distribution model, state the required probability or estimate, apply the relevant density, expectation, variance or estimation rule, then check probability bounds and assumptions.",
                "Do not confuse a density value with a probability at a single point, and do not report an estimate without identifying what population quantity it targets.",
                "Use the correct distribution or estimation rule and state its assumptions.");
            return;
        }

        if (lessonCode.Contains("STANDARD-FORM-AND-ESTIMATION", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Standard form writes a non-zero number as a×10^n where 1≤|a|<10. Estimation uses nearby convenient values to judge the size of a result before or after exact calculation.",
                "Move the decimal point to create a coefficient between 1 and 10 and count places for the power of 10. For an estimate, round inputs to sensible values while preserving the expected order of magnitude.",
                "Example: 4,900,000 = 4.9×10^6 in standard form. To estimate 4.9×10^6 + 2.1×10^6, use 5×10^6 + 2×10^6 ≈ 7×10^6, close to the exact 7.0×10^6.",
                "Convert each value to valid standard form, apply index laws where required, estimate with compatible rounded values, calculate, and compare the exact answer with the estimate.",
                "Do not write a coefficient outside the interval from 1 up to 10, and do not estimate so roughly that the power of ten changes incorrectly.",
                "Use a valid coefficient and power of ten, then estimate to check the scale of the result.");
            return;
        }

        if (lessonCode.Contains("SURFACE-AREA-AND-VOLUME", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Surface area is the total area of the exposed faces of a three-dimensional solid, while volume measures the space inside the solid.",
                "For a cuboid with length l, width w and height h, surface area is 2(lw+lh+wh) and volume is lwh. Area uses square units and volume uses cubic units.",
                "Example: a cuboid 5 cm by 3 cm by 2 cm has surface area 2(15+10+6)=62 cm² and volume 5×3×2=30 cm³. The surface area adds the area of all six faces.",
                "Identify the solid and its dimensions, choose the correct face-area and volume formulas, substitute consistent units, calculate, and label square or cubic units.",
                "Do not confuse surface area with volume, omit hidden faces, or attach cm² to a volume.",
                "Add face areas for surface area and use cubic measure for volume.");
            return;
        }

        if (lessonCode.Contains(":S4:4G-1:", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Coordinates locate vertices precisely so a polygon can be constructed from stated positions and geometric conditions.",
                "Plot each ordered pair as (x,y), join vertices in the required order, and verify the polygon using side directions, lengths, angles or parallel and perpendicular relationships.",
                "Example: plot A(1,1), B(5,1), C(5,4) and D(1,4). Joining A-B-C-D forms a rectangle: opposite sides are parallel, adjacent sides are perpendicular, and the vertices are fixed by their coordinates.",
                "Plot every coordinate accurately, join the points in order, identify the polygon, then check its stated geometric properties from the coordinate grid.",
                "Do not swap x and y coordinates or infer a polygon property from appearance without checking the coordinates.",
                "Use coordinates to construct a polygon and verify its properties.");
            return;
        }

        if (lessonCode.Contains(":S4:4G-2:", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Perimeter is the total distance around a polygon. Regular polygons have equal side lengths; irregular polygons may require several different side lengths to be added.",
                "Add every boundary side exactly once. For a regular n-sided polygon with side length s, perimeter is n×s.",
                "Example: a regular pentagon with side 6 cm has perimeter 5×6=30 cm. An irregular quadrilateral with sides 4 cm, 7 cm, 5 cm and 6 cm has perimeter 4+7+5+6=22 cm.",
                "Trace the polygon boundary, list each side length, calculate any missing side if justified, add all boundary lengths, and attach a linear unit.",
                "Do not calculate area when perimeter is required and do not omit a side of an irregular polygon.",
                "Perimeter is the sum of all side lengths around the polygon.");
            return;
        }

        if (lessonCode.Contains(":S4:4G-3:", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "A line of symmetry divides a 2D shape into two matching halves that coincide when one half is reflected across the line.",
                "Test a proposed symmetry line by reflection: corresponding points must be the same perpendicular distance from the line.",
                "Example: a square has four lines of symmetry: one vertical, one horizontal and two diagonal. A non-square rectangle has two lines of symmetry, through the midpoints of opposite sides.",
                "Identify the 2D shape, propose a symmetry line, compare corresponding vertices and sides under reflection, and count only lines that make the two halves coincide.",
                "Do not count a diagonal as a symmetry line unless reflection across it maps the whole shape onto itself.",
                "A valid line of symmetry maps the entire 2D shape onto itself by reflection.");
            return;
        }

        if (lessonCode.Contains(":S5:5NPV-2:", StringComparison.Ordinal) ||
            lessonCode.Contains(":S5:5NPV-3:", StringComparison.Ordinal))
        {
            SetBody(
                translation,
                "Decimal fractions use place value to represent parts of one. Tenths are fractions with denominator 10 and hundredths have denominator 100, so decimal notation and fraction notation describe the same quantities.",
                "In 5.37, the 3 represents 3/10 and the 7 represents 7/100. On a number line, 5.37 lies between 5.3 and 5.4 and its position reflects those decimal fractions.",
                "Example: 5.37 = 5 + 3/10 + 7/100. Also, 0.6=6/10=60/100. Comparing 5.37 and 5.4 gives 5.37<5.40 because 37 hundredths is less than 40 hundredths.",
                "Decompose the decimal into ones, tenths and hundredths, rewrite parts as fractions when useful, locate the value on a number line, then compare corresponding place values from left to right.",
                "Do not compare decimals by the number of written digits and do not treat 0.4 as 4/100.",
                "Use tenths and hundredths to connect decimal fractions, place value and number-line position.");
            return;
        }

        throw new InvalidOperationException(
            $"Unsupported Cambridge semantic repair target: {lessonCode}.");
    }

    private static void SetBody(
        CanonicalLessonContentPackTranslation translation,
        string explanation,
        string keyConcepts,
        string workedExamples,
        string stepByStepSolutions,
        string commonMistakes,
        string quickSummary)
    {
        translation.Explanation = explanation;
        translation.KeyConceptsAndRules = keyConcepts;
        translation.WorkedExamples = workedExamples;
        translation.StepByStepSolutions = stepByStepSolutions;
        translation.CommonMistakes = commonMistakes;
        translation.QuickSummary = quickSummary;
    }
}

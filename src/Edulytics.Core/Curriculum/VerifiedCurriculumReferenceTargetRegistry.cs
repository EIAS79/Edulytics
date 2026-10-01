namespace Edulytics.Core.Curriculum;

/// <summary>
/// Explicit, auditable curriculum-reference targets that may be projected into the
/// operational LearningOutcome table when a source-linked curriculum does not publish
/// reusable official outcome prose/codes for the selected scope.
///
/// These are NOT synthetic official outcomes. Codes use a REF namespace and descriptions
/// are independently authored generation labels grounded in the cited source structure.
/// </summary>
public sealed record VerifiedCurriculumReferenceTarget(
    string PackCode,
    int LogicalLevel,
    string? Pathway,
    string Code,
    string TopicName,
    string Description,
    string SourcePeriod,
    string SourceAuthority,
    string SourceUrl,
    string SourceLocator,
    string? OfficialReferenceCode = null);

public sealed record Cambridge9709RouteDefinition(
    string Code,
    string Label,
    int LogicalLevel,
    IReadOnlyList<string> Components);

public static class VerifiedCurriculumReferenceTargetRegistry
{
    private const string CambridgeSource =
        "https://www.cambridgeinternational.org/Images/697427-2026-2027-syllabus.pdf";

    private const string UaeGrade3Evidence =
        "https://njah.online/download/85ca40dfd13b0e14807c33b6252b0877.pdf/?direct=1";

    private const string UaeGrade4Evidence =
        "https://fliphtml5.com/kvqlv/vizf/Math_Mock_Exam-Grade_4-V2_251122_133044/";

    private const string UaeGrade7Evidence =
        "https://www.school-uae.com/2025/09/grade-7-advanced-integrated-math-reveal-student-book-term1-2025-2026.html";

    private const string UaeGrade8Evidence =
        "https://emirats-school.com/%D9%83%D8%AA%D8%A7%D8%A8-%D8%A7%D9%84%D8%B7%D8%A7%D9%84%D8%A8-reveal-%D8%B1%D9%8A%D8%A7%D8%B6%D9%8A%D8%A7%D8%AA-%D9%84%D9%84%D8%B5%D9%81-%D8%A7%D9%84%D8%AB%D8%A7%D9%85%D9%86-%D8%A7%D9%84%D9%81%D8%B5/";

    private const string UaeGrade11Evidence =
        "https://www.school-uae.com/2025/09/math-grade-11-advanced-term-1-uae-student-guide.html";

    private const string UaeGrade12Evidence =
        "https://www.scribd.com/document/948020975/";

    public static IReadOnlyList<Cambridge9709RouteDefinition> Cambridge9709Routes { get; } =
    [
        new(
            "AS-PURE",
            "AS Level — Pure Mathematics 1 + Pure Mathematics 2",
            12,
            ["Pure Mathematics 1", "Pure Mathematics 2"]),
        new(
            "AS-MECHANICS",
            "AS Level — Pure Mathematics 1 + Mechanics",
            12,
            ["Pure Mathematics 1", "Mechanics"]),
        new(
            "AS-STATISTICS",
            "AS Level — Pure Mathematics 1 + Probability & Statistics 1",
            12,
            ["Pure Mathematics 1", "Probability & Statistics 1"]),
        new(
            "A-MECHANICS",
            "A Level — Pure Mathematics 1 + Pure Mathematics 3 + Mechanics + Probability & Statistics 1",
            13,
            ["Pure Mathematics 1", "Pure Mathematics 3", "Mechanics", "Probability & Statistics 1"]),
        new(
            "A-STATISTICS",
            "A Level — Pure Mathematics 1 + Pure Mathematics 3 + Probability & Statistics 1 + Probability & Statistics 2",
            13,
            ["Pure Mathematics 1", "Pure Mathematics 3", "Probability & Statistics 1", "Probability & Statistics 2"])
    ];

    public static IReadOnlyList<VerifiedCurriculumReferenceTarget> All { get; } =
        Build();

    public static IReadOnlyList<VerifiedCurriculumReferenceTarget> ForScope(
        string packCode,
        int logicalLevel,
        string? selectedPathway)
    {
        var candidates = All
            .Where(x =>
                string.Equals(x.PackCode, packCode, StringComparison.Ordinal) &&
                x.LogicalLevel == logicalLevel)
            .ToArray();

        if (candidates.Length == 0)
            return [];

        var route = Cambridge9709Routes.SingleOrDefault(
            x =>
                x.LogicalLevel == logicalLevel &&
                string.Equals(x.Code, selectedPathway, StringComparison.OrdinalIgnoreCase));

        if (route is not null)
        {
            return candidates
                .Where(x =>
                    x.Pathway is not null &&
                    route.Components.Contains(x.Pathway, StringComparer.OrdinalIgnoreCase))
                .ToArray();
        }

        return candidates
            .Where(x => CurriculumPathwayCompatibility.Matches(selectedPathway, x.Pathway))
            .ToArray();
    }

    public static void Validate()
    {
        if (All.Count == 0)
            throw new InvalidOperationException("Verified curriculum reference targets are required.");

        var duplicate = All
            .GroupBy(x => (x.PackCode, x.LogicalLevel, x.Code))
            .FirstOrDefault(x => x.Count() > 1);
        if (duplicate is not null)
            throw new InvalidOperationException($"Duplicate curriculum reference target: {duplicate.Key}.");

        foreach (var target in All)
        {
            if (target.LogicalLevel is < 1 or > 13)
                throw new InvalidOperationException($"Invalid logical level for {target.Code}.");

            if (!Uri.TryCreate(target.SourceUrl, UriKind.Absolute, out var uri) ||
                uri.Scheme != Uri.UriSchemeHttps)
            {
                throw new InvalidOperationException($"HTTPS evidence source required: {target.Code}.");
            }

            if (string.IsNullOrWhiteSpace(target.Description) ||
                string.IsNullOrWhiteSpace(target.SourceLocator))
            {
                throw new InvalidOperationException($"Incomplete curriculum reference target: {target.Code}.");
            }

            if (target.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                (!target.Code.StartsWith("UAE:REF:2025-26:", StringComparison.Ordinal) ||
                 target.SourcePeriod != "2025/2026" ||
                 target.OfficialReferenceCode is not null))
            {
                throw new InvalidOperationException(
                    $"UAE reference target must remain explicitly non-official and 2025/2026 scoped: {target.Code}.");
            }

            if (target.PackCode == MathematicsCurriculumPackRegistry.CambridgeCode &&
                (!target.Code.StartsWith("CAM:REF:9709:", StringComparison.Ordinal) ||
                 target.SourcePeriod != "2026-2027" ||
                 target.OfficialReferenceCode != target.Code))
            {
                throw new InvalidOperationException(
                    $"Cambridge 9709 reference target must preserve the exact official reference identifier: {target.Code}.");
            }
        }

        foreach (var scope in new[]
        {
            (MathematicsCurriculumPackRegistry.CambridgeCode, 12, CurriculumPathwayCompatibility.CambridgeAdvancedAggregatePathway),
            (MathematicsCurriculumPackRegistry.UaeCode, 3, "Common"),
            (MathematicsCurriculumPackRegistry.UaeCode, 4, "Common"),
            (MathematicsCurriculumPackRegistry.UaeCode, 7, "Advanced"),
            (MathematicsCurriculumPackRegistry.UaeCode, 8, "Advanced"),
            (MathematicsCurriculumPackRegistry.UaeCode, 11, "Advanced"),
            (MathematicsCurriculumPackRegistry.UaeCode, 12, "Advanced")
        })
        {
            if (ForScope(scope.Item1, scope.Item2, scope.Item3).Count < 5)
                throw new InvalidOperationException(
                    $"Reference target scope must provide at least five usable targets: {scope.Item1} L{scope.Item2} {scope.Item3}.");
        }

        if (Cambridge9709Routes.Count != 5 ||
            Cambridge9709Routes.Any(x => x.Components.Count < 2))
        {
            throw new InvalidOperationException("Cambridge 9709 route contract is incomplete.");
        }
    }

    private static IReadOnlyList<VerifiedCurriculumReferenceTarget> Build()
    {
        var result = new List<VerifiedCurriculumReferenceTarget>();

        AddCambridgeComponent(result, 1, "Pure Mathematics 1",
        [
            "Quadratics",
            "Functions",
            "Coordinate geometry",
            "Circular measure",
            "Trigonometry",
            "Series",
            "Differentiation",
            "Integration"
        ]);

        AddCambridgeComponent(result, 2, "Pure Mathematics 2",
        [
            "Algebra",
            "Logarithmic and exponential functions",
            "Trigonometry",
            "Differentiation",
            "Integration",
            "Numerical solution of equations"
        ]);

        AddCambridgeComponent(result, 3, "Pure Mathematics 3",
        [
            "Algebra",
            "Logarithmic and exponential functions",
            "Trigonometry",
            "Differentiation",
            "Integration",
            "Numerical solution of equations",
            "Vectors",
            "Differential equations",
            "Complex numbers"
        ]);

        AddCambridgeComponent(result, 4, "Mechanics",
        [
            "Forces and equilibrium",
            "Kinematics of motion in a straight line",
            "Momentum",
            "Newton's laws of motion",
            "Energy, work and power"
        ]);

        AddCambridgeComponent(result, 5, "Probability & Statistics 1",
        [
            "Representation of data",
            "Permutations and combinations",
            "Probability",
            "Discrete random variables",
            "The normal distribution"
        ]);

        AddCambridgeComponent(result, 6, "Probability & Statistics 2",
        [
            "The Poisson distribution",
            "Linear combinations of random variables",
            "Continuous random variables",
            "Sampling and estimation",
            "Hypothesis tests"
        ]);

        AddUae(
            result,
            3,
            "Common",
            UaeGrade3Evidence,
            "UAE MoE Grade 3 Mathematics/Reveal, Term 1 2025/2026 exam scope",
            [
                ("T1-U2-PLACEVALUE", "Place value and addition/subtraction within 1,000", "Use place value to solve addition and subtraction problems within 1,000."),
                ("T1-U3-MULT", "Multiplication concepts", "Represent and solve multiplication situations using equal groups, arrays and related facts."),
                ("T1-U3-DIV", "Division concepts", "Represent and solve division situations using sharing, grouping and the inverse relationship with multiplication."),
                ("T1-U4-PATTERNS", "Multiplication patterns for 0, 1, 2, 5 and 10", "Use number patterns and known facts to multiply by 0, 1, 2, 5 and 10."),
                ("T1-U5-PROPERTIES", "Multiplication properties for 3, 4, 6, 7, 8 and 9", "Use properties and decomposed facts to multiply by 3, 4, 6, 7, 8 and 9.")
            ]);

        AddUae(
            result,
            4,
            "Common",
            UaeGrade4Evidence,
            "UAE MoE Grade 4 Mathematics/Reveal 4, Units 1-6, Term 1 2025/2026",
            [
                ("T1-U2-PLACEVALUE", "Generalize place-value structure", "Read, compare and round multi-digit whole numbers using place-value relationships."),
                ("T1-U3-ADDSUB", "Addition and subtraction strategies and algorithms", "Add and subtract multi-digit whole numbers and check the reasonableness of results."),
                ("T1-U4-COMPARE", "Multiplicative comparison", "Represent and solve multiplicative comparison problems with whole numbers."),
                ("T1-U5-FACTORS", "Factors, multiples and number patterns", "Identify factors and multiples and generate or analyze number patterns."),
                ("T1-U6-MULTI-DIGIT-MULT", "Multiplication strategies with multi-digit numbers", "Multiply multi-digit whole numbers using place value, properties and efficient written strategies.")
            ]);

        AddUae(
            result,
            7,
            "Advanced",
            UaeGrade7Evidence,
            "Reveal Math UAE Grade 7 Advanced Student Book, Term 1 2025/2026",
            [
                ("T1-01-PROPORTIONS", "Proportional relationships", "Represent and solve proportional relationships using tables, equations and graphs."),
                ("T1-02-PERCENT", "Percent problems", "Solve percent problems including increases, decreases and real-world applications."),
                ("T1-03-INTEGERS", "Operations with integers", "Perform and interpret operations with positive and negative integers."),
                ("T1-04-RATIONAL", "Operations with rational numbers", "Perform operations with fractions, decimals and signed rational numbers."),
                ("T1-05-EXPRESSIONS", "Simplify algebraic expressions", "Simplify algebraic expressions using properties and like terms."),
                ("T1-06-EQUATIONS", "Write and solve equations", "Translate situations into equations and solve for unknown values."),
                ("T1-07-INEQUALITIES", "Write and solve inequalities", "Model and solve one-variable inequalities and interpret their solution sets."),
                ("T1-08-GEOMETRY", "Geometric figures", "Use properties and relationships of geometric figures to solve problems."),
                ("T1-09-MEASURE", "Measure figures", "Calculate and reason about measurements of two- and three-dimensional figures."),
                ("T1-10-PROBABILITY", "Probability", "Determine probabilities and use probability models to analyze chance events."),
                ("T1-11-STATISTICS", "Sampling and statistics", "Use samples and statistical summaries to describe and compare populations.")
            ]);

        AddUae(
            result,
            8,
            "Advanced",
            UaeGrade8Evidence,
            "Reveal Math UAE Edition Grade 8 Advanced, 2025/2026",
            [
                ("01-EXPONENTS", "Exponents and scientific notation", "Use exponent rules and scientific notation to represent and calculate with very large or small quantities."),
                ("02-REAL-NUMBERS", "Real numbers", "Classify, compare and operate with rational and irrational numbers."),
                ("03-EQUATIONS", "Equations with variables on each side", "Solve linear equations that contain variables on both sides."),
                ("04-LINEAR-SLOPE", "Linear relationships and slope", "Analyze linear relationships and determine or interpret slope."),
                ("05-FUNCTIONS", "Functions", "Represent, compare and interpret functions using multiple representations."),
                ("06-SYSTEMS", "Systems of linear equations", "Model and solve systems of two linear equations."),
                ("07-PYTHAGOREAN", "Triangles and the Pythagorean theorem", "Apply properties of triangles and the Pythagorean theorem to solve problems."),
                ("08-TRANSFORMATIONS", "Transformations", "Analyze translations, rotations, reflections and dilations on the coordinate plane."),
                ("09-CONGRUENCE", "Congruence and similarity", "Use transformations and proportional reasoning to determine congruence and similarity."),
                ("10-VOLUME", "Volume", "Solve volume problems for three-dimensional figures."),
                ("11-DATA", "Scatter plots and two-way tables", "Analyze bivariate data using scatter plots and two-way tables.")
            ]);

        AddUae(
            result,
            11,
            "Advanced",
            UaeGrade11Evidence,
            "UAE Grade 11 Advanced interactive Mathematics student guide, Term 1 2025/2026",
            [
                ("T1-U01-POWER-POLYNOMIAL-RATIONAL", "Power, polynomial and rational functions", "Analyze power, polynomial and rational functions through their equations, graphs and key features."),
                ("T1-U02-EXP-LOG", "Exponential and logarithmic functions", "Represent and analyze exponential and logarithmic functions and their inverse relationship."),
                ("T1-U03-TRIG-FUNCTIONS", "Trigonometric functions", "Represent and analyze trigonometric functions and their graphs."),
                ("T1-U04-TRIG-IDENTITIES-EQUATIONS", "Trigonometric identities and equations", "Use trigonometric identities and solve trigonometric equations."),
                ("T1-U05-SYSTEMS-MATRICES", "Systems of equations and matrices", "Represent and solve systems of equations using algebraic and matrix methods."),
                ("T1-U06-CONICS-PARAMETRIC", "Conic sections and parametric equations", "Analyze conic sections and represent relationships using parametric equations."),
                ("T1-U07-VECTORS", "Vectors", "Represent vectors and use vector operations to solve mathematical problems."),
                ("T1-U08-POLAR-COMPLEX", "Polar coordinates and complex numbers", "Work with polar representations and complex numbers in multiple forms."),
                ("T1-U09-SEQUENCES-SERIES", "Sequences and series", "Analyze sequences and series and use their patterns and formulas."),
                ("T1-U10-STATISTICS-PROBABILITY", "Statistics and probability", "Use statistical and probability models to analyze data and uncertainty."),
                ("T1-U11-FUNCTIONS-CALCULUS", "Functions from a calculus perspective", "Interpret function behavior through rates of change and calculus-oriented representations."),
                ("T1-U12-CALCULUS", "Calculus", "Apply introductory differentiation and integration ideas to mathematical problems.")
            ]);

        AddUae(
            result,
            12,
            "Advanced",
            UaeGrade12Evidence,
            "UAE Grade 12 Advanced Mathematics Term 1 2025/2026 published exam scope",
            [
                ("T1-C2L1-PREVIEW", "Calculus preview: tangent lines and curve length", "Connect average and instantaneous change to tangent-line and curve-length ideas."),
                ("T1-C2L2-LIMIT", "The concept of limit", "Interpret limits numerically, graphically and algebraically."),
                ("T1-C2L3-LIMITS", "Computation of limits", "Evaluate limits using algebraic and analytic techniques."),
                ("T1-C2L4-CONTINUITY", "Continuity and its consequences", "Determine continuity and use continuity properties to reason about functions."),
                ("T1-C2L5-INFINITY", "Limits involving infinity and asymptotes", "Analyze end behavior, infinite limits and asymptotes."),
                ("T1-C3L1-TANGENT-VELOCITY", "Tangent lines and velocity", "Relate tangent-line slope to instantaneous velocity and rate of change."),
                ("T1-C3L2-DERIVATIVE", "The derivative", "Interpret and determine derivatives as instantaneous rates of change."),
                ("T1-C3L3-POWER-RULE", "Computation of derivatives: power rule", "Differentiate power functions and combinations using the power rule.")
            ]);

        return result;
    }

    private static void AddCambridgeComponent(
        ICollection<VerifiedCurriculumReferenceTarget> result,
        int component,
        string pathway,
        IReadOnlyList<string> topics)
    {
        for (var index = 0; index < topics.Count; index++)
        {
            var section = $"{component}.{index + 1}";
            var code = $"CAM:REF:9709:{section}";
            result.Add(
                new VerifiedCurriculumReferenceTarget(
                    MathematicsCurriculumPackRegistry.CambridgeCode,
                    12,
                    pathway,
                    code,
                    pathway,
                    $"Cambridge 9709 {pathway}: {topics[index]}. Use the official section reference as scope; Edulytics does not reproduce copyrighted objective prose.",
                    "2026-2027",
                    "Cambridge International Education",
                    CambridgeSource,
                    section,
                    code));

            result.Add(
                new VerifiedCurriculumReferenceTarget(
                    MathematicsCurriculumPackRegistry.CambridgeCode,
                    13,
                    pathway,
                    code,
                    pathway,
                    $"Cambridge 9709 {pathway}: {topics[index]}. Use the official section reference as scope; Edulytics does not reproduce copyrighted objective prose.",
                    "2026-2027",
                    "Cambridge International Education",
                    CambridgeSource,
                    section,
                    code));
        }
    }

    private static void AddUae(
        ICollection<VerifiedCurriculumReferenceTarget> result,
        int logicalLevel,
        string pathway,
        string evidenceUrl,
        string sourceLocator,
        IReadOnlyList<(string Key, string Title, string Description)> targets)
    {
        foreach (var target in targets)
        {
            result.Add(
                new VerifiedCurriculumReferenceTarget(
                    MathematicsCurriculumPackRegistry.UaeCode,
                    logicalLevel,
                    pathway,
                    $"UAE:REF:2025-26:G{logicalLevel}:{target.Key}",
                    $"Grade {logicalLevel} {pathway} — 2025/2026 curriculum reference",
                    target.Description,
                    "2025/2026",
                    "UAE Ministry of Education curriculum / published 2025/2026 learning material",
                    evidenceUrl,
                    $"{sourceLocator}; {target.Title}",
                    null));
        }
    }
}

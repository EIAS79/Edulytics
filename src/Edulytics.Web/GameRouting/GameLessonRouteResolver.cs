namespace Edulytics.Web.GameRouting;

/// <summary>
/// Application-facing lesson route resolver. It keeps the stable router contract
/// while applying corrections that must be evaluated before broad unit matches.
/// Supporting lessons can require lesson-content grounding so a broad unit match
/// can never silently fall back to an unrelated generic practice mechanic.
/// </summary>
public static class GameLessonRouteResolver
{
    public const string LessonGroundedRendererKey = "lesson-grounded";

    public static GameLessonRoute Resolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle) =>
        Resolve(lessonCode, unitTitle, lessonTitle, null, false);

    public static GameLessonRoute Resolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string? lessonContext,
        bool requireLessonGrounding) =>
        Resolve(
            lessonCode,
            unitTitle,
            lessonTitle,
            lessonContext,
            requireLessonGrounding,
            MathematicsV2ProductMigrationPolicy.IsEnabledFromEnvironment());

    public static GameLessonRoute Resolve(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string? lessonContext,
        bool requireLessonGrounding,
        bool enableMathematicsV2Pilot)
    {
        if (MathematicsV2ProductMigrationPolicy.TryGetApprovedGrade16Entry(
                lessonCode,
                out var approvedStage17)
            && approvedStage17 is not null)
        {
            var framework = FrameworkFromLessonCode(lessonCode);
            return LessonSkillRoute(
                lessonCode,
                framework,
                WorkspaceForStage17Domain(approvedStage17.Domain),
                approvedStage17.Mechanic,
                enableMathematicsV2Pilot);
        }

        if (!string.IsNullOrWhiteSpace(lessonContext))
        {
            var grounded = ResolveFromLessonContent(
                lessonCode,
                unitTitle,
                lessonTitle,
                lessonContext,
                enableMathematicsV2Pilot);
            if (grounded is not null)
                return grounded;
        }

        if (requireLessonGrounding)
        {
            var framework = FrameworkFromLessonCode(lessonCode);
            return new GameLessonRoute(
                framework,
                "NEEDS_REVIEW",
                "NEEDS_REVIEW",
                null,
                LocaleForFramework(framework),
                false,
                "lesson-grounding-required");
        }

        if (IsUaeGeometryAndData(lessonCode, unitTitle))
        {
            var title = Normalize(lessonTitle);
            if (ContainsAny(title, "data", "graph", "chart", "table"))
            {
                return new GameLessonRoute(
                    "UAE-MOE-MATH",
                    "DATA_STATISTICS",
                    DataMechanic(title),
                    GameLessonRouter.UniversalRendererKey,
                    "ar",
                    true,
                    "uae-mixed-title-v2");
            }

            if (ContainsAny(title, "shape", "geometry", "angle", "position", "symmetry"))
            {
                return new GameLessonRoute(
                    "UAE-MOE-MATH",
                    "GEOMETRY",
                    GeometryMechanic(title),
                    GameLessonRouter.UniversalRendererKey,
                    "ar",
                    true,
                    "uae-mixed-title-v2");
            }

            return new GameLessonRoute(
                "UAE-MOE-MATH",
                "COMPOSITE_SESSION",
                "COMPOSITE_SESSION",
                GameLessonRouter.UniversalRendererKey,
                "ar",
                true,
                "uae-mixed-unit-v2");
        }

        return GameLessonRouter.Resolve(lessonCode, unitTitle, lessonTitle);
    }

    private static GameLessonRoute? ResolveFromLessonContent(
        string lessonCode,
        string unitTitle,
        string lessonTitle,
        string lessonContext,
        bool enableMathematicsV2Pilot)
    {
        var framework = FrameworkFromLessonCode(lessonCode);
        var title = Normalize(lessonTitle);
        var semantic = Normalize(string.Join(
            " ",
            unitTitle,
            lessonTitle,
            lessonContext));

        // Exact lesson skills must win before any broad topic classification.
        // These routes are deliberately title/skill specific: incidental words
        // such as "addition", "ratio" or "fraction" in a lesson body are not
        // sufficient to choose a question mechanic.
        if (ContainsAny(title, "solve problems with 2 unknowns", "solve problems with two unknowns"))
        {
            return LessonSkillRoute(
                lessonCode,
                framework,
                "REASONING_MODELING",
                "TWO_UNKNOWNS",
                enableMathematicsV2Pilot);
        }

        if (title.Contains("reading scales", StringComparison.Ordinal) &&
            ContainsAny(semantic, "interval", "intervals", "scale", "scales"))
        {
            return LessonSkillRoute(
                lessonCode,
                framework,
                "MEASUREMENT",
                "SCALE_READING",
                enableMathematicsV2Pilot);
        }

        if (title.Contains("find equivalent fractions", StringComparison.Ordinal) &&
            ContainsAny(semantic, "equivalent fraction", "equivalent fractions", "equivalence"))
        {
            return LessonSkillRoute(
                lessonCode,
                framework,
                "FRACTIONS",
                "FRACTION_EQUIVALENT",
                enableMathematicsV2Pilot);
        }

        if (title.Contains("compare fractions", StringComparison.Ordinal) &&
            ContainsAny(semantic, "different denominators", "unlike denominators"))
        {
            return LessonSkillRoute(
                lessonCode,
                framework,
                "FRACTIONS",
                "FRACTION_COMPARE_UNLIKE",
                enableMathematicsV2Pilot);
        }

        // Existing lesson-grounded relationship models remain available, but
        // only when the lesson title itself expresses the intended relationship.
        // A mention in worked examples or checking guidance cannot promote an
        // unrelated lesson into one of these broad mechanics.
        var titleHasRatio = ContainsAny(
            title,
            "ratio relationship",
            "ratio relationships",
            "ratio and proportion");
        var titleHasAdditiveRelationships = ContainsAny(
            title,
            "additive structure",
            "additive structures",
            "additive relationship",
            "additive relationships");
        var hasRatio = ContainsAny(
            semantic,
            "ratio relationship",
            "ratio relationships",
            "ratio and proportion",
            "ratio",
            "proportion");
        var hasAdditiveRelationships = ContainsAny(
            semantic,
            "part-whole",
            "part whole",
            "change relationship",
            "difference relationship",
            "additive structure",
            "additive relationship");
        var hasRepresentation = ContainsAny(
            semantic,
            "number line",
            "equation",
            "represent",
            "diagram");
        var hasChecking = ContainsAny(
            semantic,
            "inverse",
            "estimate",
            "verify",
            "check");

        if (titleHasRatio && hasRatio && hasAdditiveRelationships && hasRepresentation && hasChecking)
        {
            return new GameLessonRoute(
                framework,
                "REASONING_MODELING",
                "ADDITIVE_RATIO_RELATIONSHIPS",
                LessonGroundedRendererKey,
                LocaleForFramework(framework),
                true,
                "lesson-content-grounding");
        }

        if (titleHasRatio && hasRatio && hasRepresentation)
        {
            return new GameLessonRoute(
                framework,
                "RATIO_ALGEBRA",
                "RATIO_RELATIONSHIPS",
                LessonGroundedRendererKey,
                LocaleForFramework(framework),
                true,
                "lesson-content-grounding");
        }

        if (titleHasAdditiveRelationships && hasAdditiveRelationships && hasRepresentation)
        {
            return new GameLessonRoute(
                framework,
                "REASONING_MODELING",
                "ADDITIVE_RELATIONSHIPS",
                LessonGroundedRendererKey,
                LocaleForFramework(framework),
                true,
                "lesson-content-grounding");
        }

        return null;
    }

    private static GameLessonRoute LessonSkillRoute(
        string lessonCode,
        string framework,
        string workspace,
        string mechanic,
        bool enableMathematicsV2Pilot)
    {
        var usePilot = MathematicsV2ProductMigrationPolicy.ShouldUsePilot(
            lessonCode,
            mechanic,
            enableMathematicsV2Pilot);

        return new GameLessonRoute(
            framework,
            workspace,
            mechanic,
            usePilot
                ? MathematicsV2ProductMigrationPolicy.RendererKey
                : LessonGroundedRendererKey,
            LocaleForFramework(framework),
            true,
            usePilot
                ? MathematicsV2ProductMigrationPolicy.ClassificationSource
                : "exact-lesson-skill-v2");
    }

    private static bool IsUaeGeometryAndData(string lessonCode, string unitTitle) =>
        lessonCode.Contains(":UAE-MOE-MATH:", StringComparison.OrdinalIgnoreCase) &&
        Normalize(unitTitle).Contains("geometry and data", StringComparison.Ordinal);

    private static string DataMechanic(string title)
    {
        if (ContainsAny(title, "bar chart", "bar graph")) return "BAR_CHART_BUILD";
        if (ContainsAny(title, "pictogram")) return "PICTOGRAM_BUILD";
        if (ContainsAny(title, "table")) return "TABLE_BUILD";
        if (ContainsAny(title, "mean", "median", "mode", "range")) return "STATISTIC_CALCULATE";
        if (ContainsAny(title, "sort", "categor")) return "DATA_SORT";
        return "PLOT_READ";
    }

    private static string GeometryMechanic(string title)
    {
        if (ContainsAny(title, "angle")) return "ANGLE_LAB";
        if (ContainsAny(title, "symmetry")) return "SYMMETRY_MIRROR";
        if (ContainsAny(title, "position", "direction")) return "POSITION_ROUTE";
        if (ContainsAny(title, "coordinate")) return "COORDINATE_SHAPE";
        return "SHAPE_IDENTIFY_COMPARE";
    }

    private static string FrameworkFromLessonCode(string lessonCode)
    {
        if (lessonCode.Contains(":CAMBRIDGE-INTL-MATH:", StringComparison.OrdinalIgnoreCase))
            return "CAMBRIDGE-INTL-MATH";
        if (lessonCode.Contains(":UK-NC-ENG-MATH:", StringComparison.OrdinalIgnoreCase))
            return "UK-NC-ENG-MATH";
        if (lessonCode.Contains(":US-CCSS-MATH:", StringComparison.OrdinalIgnoreCase))
            return "US-CCSS-MATH";
        if (lessonCode.Contains(":PL-NATIONAL-MATH:", StringComparison.OrdinalIgnoreCase))
            return "PL-NATIONAL-MATH";
        if (lessonCode.Contains(":UAE-MOE-MATH:", StringComparison.OrdinalIgnoreCase))
            return "UAE-MOE-MATH";
        return "UNKNOWN";
    }

    private static string LocaleForFramework(string framework) => framework switch
    {
        "PL-NATIONAL-MATH" => "pl",
        "UAE-MOE-MATH" => "ar",
        _ => "en"
    };

    private static string Normalize(string value) =>
        (value ?? string.Empty).Trim().ToLowerInvariant();

    private static bool ContainsAny(string value, params string[] terms) =>
        terms.Any(term => value.Contains(term, StringComparison.Ordinal));
}

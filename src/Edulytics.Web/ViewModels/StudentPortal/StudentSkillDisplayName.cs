namespace Edulytics.Web.ViewModels.StudentPortal;

public static class StudentSkillDisplayName
{
    private const string CambridgeOutcomePrefix =
        "outcome:CAM:OUT:0096:";

    private static readonly IReadOnlyDictionary<string, string>
        CambridgeStage4 = new Dictionary<string, string>(
            StringComparer.Ordinal)
        {
            ["4Nc.01"] = "Counting steps",
            ["4Nc.02"] = "Odd/even patterns",
            ["4Nc.03"] = "Missing quantities",
            ["4Nc.04"] = "Number sequences",
            ["4Nc.05"] = "Square numbers",

            ["4Ni.01"] = "Whole numbers",
            ["4Ni.02"] = "Add & subtract",
            ["4Ni.03"] = "Multiplication grouping",
            ["4Ni.04"] = "Times tables",
            ["4Ni.05"] = "Multiply whole numbers",
            ["4Ni.06"] = "Divide whole numbers",
            ["4Ni.07"] = "Factors & multiples",
            ["4Ni.08"] = "Divisibility tests",

            ["4Np.01"] = "Place value",
            ["4Np.02"] = "×/÷ 10 & 100",
            ["4Np.03"] = "Regroup whole numbers",
            ["4Np.04"] = "Order signed numbers",
            ["4Np.05"] = "Rounding numbers",

            ["4Nf.01"] = "Fraction size",
            ["4Nf.02"] = "Fractions as division",
            ["4Nf.03"] = "Fraction operators",
            ["4Nf.04"] = "Equivalent fractions",
            ["4Nf.05"] = "Add/subtract fractions",
            ["4Nf.06"] = "Percentages",
            ["4Nf.07"] = "Compare fractions",

            ["4Gt.01"] = "Convert time units",
            ["4Gt.02"] = "Read clocks",
            ["4Gt.03"] = "Timetables",
            ["4Gt.04"] = "Time intervals",

            ["4Gg.01"] = "Combine 2D shapes",
            ["4Gg.02"] = "Perimeter & area",
            ["4Gg.03"] = "Rectangle area & perimeter",
            ["4Gg.04"] = "Irregular area",
            ["4Gg.05"] = "2D faces",
            ["4Gg.06"] = "Shape nets",
            ["4Gg.07"] = "Lines of symmetry",
            ["4Gg.08"] = "Classify angles",
            ["4Gg.09"] = "Measuring scales",

            ["4Gp.01"] = "Position & direction",
            ["4Gp.02"] = "Coordinates",
            ["4Gp.03"] = "Shape reflections",

            ["4Ss.01"] = "Statistical questions",
            ["4Ss.02"] = "Represent data",
            ["4Ss.03"] = "Interpret data",

            ["4Sp.01"] = "Probability language",
            ["4Sp.02"] = "Chance experiments"
        };

    public static string Format(
        string? skillKey,
        string? skillName)
    {
        var fallback =
            string.IsNullOrWhiteSpace(skillName)
                ? "Skill"
                : skillName.Trim();

        if (string.IsNullOrWhiteSpace(skillKey) ||
            !skillKey.StartsWith(
                CambridgeOutcomePrefix,
                StringComparison.Ordinal))
        {
            return fallback;
        }

        var code =
            skillKey[CambridgeOutcomePrefix.Length..];

        return CambridgeStage4.TryGetValue(
            code,
            out var displayName)
                ? displayName
                : fallback;
    }
}

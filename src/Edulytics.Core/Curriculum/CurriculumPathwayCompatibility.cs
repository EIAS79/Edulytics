namespace Edulytics.Core.Curriculum;

/// <summary>
/// Central pathway compatibility policy for projecting verified curriculum content
/// into a selected school curriculum level/pathway.
/// </summary>
public static class CurriculumPathwayCompatibility
{
    public const string CambridgeAdvancedAggregatePathway =
        "Component/route structure preserved in reference graph";

    public const string CommonCoreHighSchoolAggregatePathway =
        "Course/pathway mapping";

    public static bool Matches(
        string? selectedPathway,
        string? contentPathway)
    {
        var selected = Normalize(selectedPathway);
        var content = Normalize(contentPathway);

        // A shared school scope must not silently absorb pathway-specific content.
        if (selected is null)
            return content is null;

        // Shared official content applies to every selected pathway.
        if (content is null)
            return true;

        if (string.Equals(
                selected,
                content,
                StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // These registered pathway values are aggregate product scopes rather than
        // literal official content pathways.
        if (string.Equals(
                selected,
                CambridgeAdvancedAggregatePathway,
                StringComparison.OrdinalIgnoreCase) ||
            string.Equals(
                selected,
                CommonCoreHighSchoolAggregatePathway,
                StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Some authoritative packs represent one node as applicable to multiple
        // pathways, for example "Liceum ogólnokształcące | Technikum".
        return content
            .Split(
                '|',
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries)
            .Any(
                candidate =>
                    string.Equals(
                        candidate,
                        selected,
                        StringComparison.OrdinalIgnoreCase));
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
}

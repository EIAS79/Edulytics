namespace Edulytics.Core.Curriculum;

/// <summary>
/// Keeps curriculum-source research notes inside the authoring record instead of
/// displaying them as learner/teacher Quick Summary bullets. The original
/// SummaryPoints are never modified or discarded, and unrelated grades retain
/// their existing behavior.
/// </summary>
public static class RichLessonSummaryVisibility
{
    private const string Grade5CodePrefix = "PED:US-CCSS-MATH:G5:";

    // Source/rights notes must stay in Git and the embedded JSON for curriculum
    // audits; they are not mathematical instructional points.
    private static readonly string[] InternalMarkers =
    [
        "https://", "http://", "publisher", "kendall",
        "illustrative mathematics", "cc by", "not_verified",
        "student task pdf", "source-owned", "source image",
        "source figures", "asset rights", "image licensing",
        "image copyrights", "original image binaries",
        "rights not verified", "first edition 2021"
    ];

    private static readonly string[] InternalPrefixes =
    [
        "source:", "source checked:", "source compared:",
        "source evidence:", "original 2021", "2021 original teacher",
        "original im 2021", "2021 im k", "2021 first-edition",
        "public 2021", "optional prior:", "publisher reference:",
        "original student task pdf"
    ];

    public static IReadOnlyList<string> ForReader(
        string lessonCode,
        IEnumerable<string> summaryPoints)
    {
        ArgumentNullException.ThrowIfNull(summaryPoints);

        if (string.IsNullOrWhiteSpace(lessonCode) ||
            !lessonCode.StartsWith(Grade5CodePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return summaryPoints.ToArray();
        }

        return summaryPoints
            .Where(point => !IsGrade5InternalSourceNote(point))
            .ToArray();
    }

    public static bool IsGrade5InternalSourceNote(string? point)
    {
        if (string.IsNullOrWhiteSpace(point))
            return false;

        var text = point.Trim();
        return InternalMarkers.Any(marker =>
                text.Contains(marker, StringComparison.OrdinalIgnoreCase)) ||
            InternalPrefixes.Any(prefix =>
                text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}

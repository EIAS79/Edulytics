namespace Edulytics.Core.Mathematics.Identifiers;

internal static class CanonicalIdentifier
{
    public static string Normalize(string value, string parameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);

        var normalized = value.Trim().ToLowerInvariant();
        if (normalized.Length > 160)
        {
            throw new ArgumentOutOfRangeException(
                parameterName,
                "Canonical Mathematics identifiers cannot exceed 160 characters.");
        }

        var segments = normalized.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0 || segments.Any(segment => !IsValidSegment(segment)))
        {
            throw new ArgumentException(
                "Canonical Mathematics identifiers must use dot-separated lowercase letters, digits, underscores, or hyphens.",
                parameterName);
        }

        return string.Join('.', segments);
    }

    private static bool IsValidSegment(string segment)
    {
        if (segment.Length == 0)
        {
            return false;
        }

        foreach (var character in segment)
        {
            if (!char.IsAsciiLetterOrDigit(character) &&
                character is not '_' and not '-')
            {
                return false;
            }
        }

        return true;
    }
}

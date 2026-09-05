namespace Edulytics.Web.Presentation;

public static class LearningOutcomePresentation
{
    public static string DisplayCode(string? code)
    {
        var value = (code ?? string.Empty).Trim();
        if (value.Length == 0)
            return string.Empty;

        var separator = value.LastIndexOf(':');
        return separator >= 0 && separator < value.Length - 1
            ? value[(separator + 1)..]
            : value;
    }
}

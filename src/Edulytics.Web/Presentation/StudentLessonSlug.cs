using System.Globalization;
using System.Text;
using Edulytics.Services.LessonContent;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Readable student URLs use the stable curriculum-specific lesson code as
/// their identity and include the learner-facing title for readability.
/// </summary>
public static class StudentLessonSlug
{
    public static string Create(StudentLessonSummary lesson) =>
        Create(lesson.LessonCode, lesson.Title);

    public static string Create(string lessonCode, string title) =>
        $"{CodeToken(lessonCode)}--{Normalize(title, 90)}";

    public static string CodeToken(string lessonCode)
    {
        var code = lessonCode.StartsWith("PED:", StringComparison.OrdinalIgnoreCase)
            ? lessonCode[4..]
            : lessonCode;
        return Normalize(code, 180);
    }

    public static string? GetCodeToken(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug) || slug.Length > 280)
            return null;

        var marker = slug.IndexOf("--", StringComparison.Ordinal);
        if (marker < 1 || marker >= slug.Length - 2)
            return null;

        var code = slug[..marker];
        var title = slug[(marker + 2)..];
        return code == Normalize(code, 180) &&
               title == Normalize(title, 90)
            ? code
            : null;
    }

    private static string Normalize(string? value, int maxLength)
    {
        var source = (value ?? string.Empty).Normalize(NormalizationForm.FormD);
        var result = new StringBuilder();
        var separator = false;

        foreach (var current in source)
        {
            if (char.GetUnicodeCategory(current) is
                UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.EnclosingMark)
                continue;

            var letter = current switch
            {
                'Ł' or 'ł' => 'l',
                _ => char.ToLowerInvariant(current)
            };

            if (char.IsLetterOrDigit(letter))
            {
                if (separator && result.Length > 0 && result.Length < maxLength)
                    result.Append('-');
                separator = false;
                if (result.Length < maxLength)
                    result.Append(letter);
            }
            else
            {
                separator = true;
            }

            if (result.Length >= maxLength)
                break;
        }

        return result.ToString().TrimEnd('-') is { Length: > 0 } slug
            ? slug
            : "lesson";
    }
}

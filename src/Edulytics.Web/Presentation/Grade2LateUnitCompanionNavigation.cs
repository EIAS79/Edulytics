using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Web.Presentation;

/// <summary>Whitelisted source activity companions for the 44 final Grade 2 lessons.</summary>
public static class Grade2LateUnitCompanionNavigation
{
    private static readonly Regex CanonicalCode = new(
        @"^PED:US-CCSS-MATH:G2:U(07|08|09):L(0[1-9]|1[0-9])$",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public sealed record Link(string Controller, int UnitNumber, int LessonNumber);

    public static Link? Resolve(string? lessonCode)
    {
        if (string.IsNullOrEmpty(lessonCode)) return null;
        var match = CanonicalCode.Match(lessonCode);
        if (!match.Success) return null;

        var unit = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var lesson = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var maxLesson = unit == 7 ? 18 : 13;
        if (lesson < 1 || lesson > maxLesson) return null;

        return new Link($"Grade2Unit{unit}Activities", unit, lesson);
    }
}
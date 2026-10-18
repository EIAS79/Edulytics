using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party analogue clock diagrams for original 2021 Grade 1 U07
/// whole-hour and half-hour digital subquestions. The source lesson's
/// physical clock handling and PLC activities remain separate tasks.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string? GradeOneFullGradeClockVisual(string family, JsonElement p)
    {
        int hour = GetInt(p, "a"), card = GetInt(p, "b");
        int c = GetInt(p, "c"), mode = GetInt(p, "mode");
        if (hour is < 1 or > 12 || card is < 1 or > 12 ||
            c != 0 || mode is < 0 or > 3)
            throw new JsonException("Outside Grade 1 whole/half-hour clock parameters.");

        bool isHalf = family switch
        {
            "usccss.g1.u07.l13.hour_clock" => false,
            "usccss.g1.u07.l14.half_hour_clock" => true,
            "usccss.g1.u07.l15.write_hours_halfhours" => mode % 2 != 0,
            "usccss.g1.u07.l17.center_geometry_and_time" when mode % 2 != 0 => true,
            _ => false
        };
        // U07 L17 even mode is a numerical center (not a clock problem).
        if (family == "usccss.g1.u07.l17.center_geometry_and_time" && mode % 2 == 0)
            return null;

        int minutes = isHalf ? 30 : 0;
        const double cx = 210, cy = 129;
        var svg = SvgStart("An analog clock dial with two hands. Read the time from the diagram.");
        svg.Append("<circle cx='210' cy='129' r='99' class='shape fill' data-role='clock-face'/>");
        svg.Append("<circle cx='210' cy='129' r='5' class='point'/>");

        for (int k = 1; k <= 12; k++)
        {
            double angle = k * Math.PI / 6 - Math.PI / 2;
            Text(svg, cx + 77 * Math.Cos(angle) - (k >= 10 ? 9 : 4),
                cy + 77 * Math.Sin(angle) + 5, k.ToString(), "hint");
        }

        // Angle starts at 12 o'clock; 30 min shifts the hour hand half a step.
        double hourAngle = ((hour % 12) + minutes / 60.0) * Math.PI / 6 - Math.PI / 2;
        double minuteAngle = minutes * Math.PI / 30 - Math.PI / 2;
        svg.Append($"<line x1='210' y1='129' x2='{F(cx + 47 * Math.Cos(hourAngle))}' " +
            $"y2='{F(cy + 47 * Math.Sin(hourAngle))}' stroke='currentColor' " +
            "stroke-width='7' stroke-linecap='round' data-role='hour-hand'/>");
        svg.Append($"<line x1='210' y1='129' x2='{F(cx + 68 * Math.Cos(minuteAngle))}' " +
            $"y2='{F(cy + 68 * Math.Sin(minuteAngle))}' stroke='currentColor' " +
            "stroke-width='3' stroke-linecap='round' data-role='minute-hand'/>");
        Text(svg, 210, 251, "Read the hands; write the time yourself.", "hint");
        return SvgEnd(svg);
    }
}

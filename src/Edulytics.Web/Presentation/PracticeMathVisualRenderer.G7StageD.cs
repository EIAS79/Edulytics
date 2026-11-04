using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    // Grade 7 Stage D: first-party deterministic drawings from question parameters.
    // None of these drawings states whether a comparison is true or gives a hidden answer.
    private static string? G7ScaledPolygon(JsonElement p)
    {
        var variant = GetInt(p, "variant");
        if (variant is < 0 or > 24) return null;
        var phase = variant / 5;
        (double X, double Y)[] candidate = (variant % 5) switch
        {
            0 => [(0,0),(8,0),(8,6),(0,6)],
            1 => [(0,0),(8,0),(8,3),(0,3)],
            2 => [(0,0),(4,0),(4,6),(0,6)],
            3 => [(0,0),(8,0),(11,6),(3,6)],
            _ => [(0,0),(-2,0),(-2,-1.5),(0,-1.5)]
        };
        candidate = candidate.Select(v => (v.X + 2 * phase, v.Y + phase)).ToArray();
        var minX = candidate.Min(v => v.X);
        var minY = candidate.Min(v => v.Y);
        var sb = SvgStart("Original rectangle and proposed scaled polygon on equal coordinate scales; determine matching lengths and angles.");
        // One diagram unit has the same pixel length in both figures.
        sb.Append("<polygon points='75,190 139,190 139,142 75,142' class='shape fill'/>");
        var pts = string.Join(" ", candidate.Select(v =>
            F(225 + (v.X - minX) * 14) + "," + F(190 - (v.Y - minY) * 14)));
        sb.Append("<polygon points='").Append(pts).Append("' class='shape fill'/>");
        Text(sb, 90, 226, "Original", "label");
        Text(sb, 270, 226, "Candidate", "label");
        return SvgEnd(sb);
    }

    private static string? G7AngleVisual(string family, JsonElement p)
    {
        var angle = GetInt(p, "known");
        if (family == "usccss.g7.u07.complementary_angle" && angle is < 10 or > 80)
            return null;
        if (family == "usccss.g7.u07.vertical_angle" && angle is < 20 or > 160)
            return null;
        var sb = SvgStart("Two given angles with the known measure marked; calculate the related angle.");
        if (family == "usccss.g7.u07.complementary_angle")
        {
            sb.Append("<line x1='110' y1='210' x2='320' y2='210' class='shape'/>");
            sb.Append("<line x1='110' y1='210' x2='110' y2='40' class='shape'/>");
            var radians = angle * Math.PI / 180;
            sb.Append("<line x1='110' y1='210' x2='").Append(F(110 + 145 * Math.Cos(radians)))
              .Append("' y2='").Append(F(210 - 145 * Math.Sin(radians)))
              .Append("' class='shape'/>");
        }
        else
        {
            var radians = angle * Math.PI / 180;
            var dx = 115 * Math.Cos(radians / 2);
            var dy = 115 * Math.Sin(radians / 2);
            sb.Append("<line x1='").Append(F(210 - dx)).Append("' y1='").Append(F(130 - dy))
              .Append("' x2='").Append(F(210 + dx)).Append("' y2='").Append(F(130 + dy))
              .Append("' class='shape'/>");
            sb.Append("<line x1='").Append(F(210 - dx)).Append("' y1='").Append(F(130 + dy))
              .Append("' x2='").Append(F(210 + dx)).Append("' y2='").Append(F(130 - dy))
              .Append("' class='shape'/>");
        }
        Text(sb, 280, 52, "Known angle: " + angle + "°", "label");
        return SvgEnd(sb);
    }

    private static string? G7CircleLessonVisual(string family, JsonElement p)
    {
        var sb = SvgStart("Circle diagram tied to the generated mathematics question.");
        sb.Append("<circle cx='210' cy='132' r='78' class='shape fill'/>");
        sb.Append("<circle cx='210' cy='132' r='3' class='point'/>");
        if (family == "usccss.g7.u03.l02.radius_diameter_relation")
        {
            var radius = GetInt(p, "radius");
            var mode = GetInt(p, "mode");
            if (radius < 1 || mode is < 0 or > 1) return null;
            if (mode == 0)
            {
                sb.Append("<line x1='210' y1='132' x2='288' y2='132' class='shape'/>");
                Text(sb, 210, 235, "Given radius: " + radius, "label");
            }
            else
            {
                sb.Append("<line x1='132' y1='132' x2='288' y2='132' class='shape'/>");
                Text(sb, 210, 235, "Given diameter: " + (2 * radius), "label");
            }
        }
        else if (family == "usccss.g7.u03.l05.wheel_rotations_distance")
        {
            var mode = GetInt(p, "mode");
            var rotations = GetInt(p, "rotations");
            if (mode is < 0 or > 2 || rotations < 1) return null;
            sb.Append("<line x1='40' y1='210' x2='380' y2='210' class='axis'/>");
            sb.Append("<line x1='210' y1='132' x2='280' y2='98' class='shape'/>");
            Text(sb, 210, 242, mode == 0
                ? "Given diameter: " + GetInt(p, "diameter") + ", turns: " + rotations
                : "Given circumference: " + GetInt(p, "circumference"), "hint");
        }
        else if (family == "usccss.g7.u03.l09.circle_fraction_area_pi_coefficient")
        {
            var radius = GetInt(p, "radius");
            var denominator = GetInt(p, "denominator");
            if (radius < 1 || denominator is not (2 or 4)) return null;
            if (denominator == 2)
                sb.Append("<path d='M132 132 A78 78 0 0 1 288 132 L210 132 Z' class='shape fill'/>");
            else
                sb.Append("<path d='M210 132 L288 132 A78 78 0 0 0 210 54 Z' class='shape fill'/>");
            Text(sb, 210, 240, (denominator == 2 ? "Semicircle" : "Quarter-circle") + " radius: " + radius, "label");
        }
        else return null;
        return SvgEnd(sb);
    }
}
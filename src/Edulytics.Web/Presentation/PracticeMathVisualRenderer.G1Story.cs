using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    // First-party picture for an IM-inspired Grade 1 story task. Show the
    // start and what moves in/out; do not render a solved equation or response.
    private static string GradeOneCenterStoryPicture(JsonElement p)
    {
        var start = GetInt(p, "start");
        var change = GetInt(p, "change");
        var action = GetInt(p, "action");
        var flip = GetInt(p, "flip");
        if (start is < 4 or > 8 || change is < 1 or > 3 ||
            start + change > 10 || action is < 0 or > 1 ||
            flip is < 0 or > 1)
            throw new JsonException("Invalid source-scoped Grade 1 story visual parameters.");

        var sb = SvgStart(action == 0
            ? "Picture story: a starting group of balls with more balls rolling into the scene."
            : "Picture story: a starting group of balls with some balls rolling away.");
        Text(sb, 104, 35, "START", "label");
        Text(sb, 312, 35, action == 0 ? "JOINING" : "LEAVING", "label");
        for (var i = 0; i < start; i++)
        {
            var x = 52 + (i % 4) * 34;
            var y = 83 + (i / 4) * 45;
            sb.Append($"<circle cx='{x}' cy='{y}' r='13' class='shape fill'/>");
        }
        for (var i = 0; i < change; i++)
        {
            var x = 282 + (i % 3) * 34;
            var y = 98 + (i / 3) * 45;
            sb.Append($"<circle cx='{x}' cy='{y}' r='13' class='shape fill'/>");
        }
        if (action == 0)
        {
            sb.Append("<path d='M 260 140 L 214 140 M 226 129 L 214 140 L 226 151' class='shape'/>");
        }
        else
        {
            sb.Append("<path d='M 214 140 L 263 140 M 250 129 L 263 140 L 250 151' class='shape'/>");
        }
        Text(sb, 210, 223, "Tell a story, then choose the matching equation.", "hint");
        return SvgEnd(sb);
    }
}

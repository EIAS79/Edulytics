using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>First-party two-blank number-card board for original 2021
/// Grade 1 U03 L14. Checking no-reuse in a digital board does not prove
/// original physical partner center participation.</summary>
public static partial class PracticeMathVisualRenderer
{
    private static string? GradeOneFullGradeNumberPuzzleBoard(JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a<11||a>20||b<1||b>9||b>=a||mode<0||mode>3)
            throw new JsonException("Invalid source Number Puzzles parameters");
        if(mode<=1)
        {
            if(c!=0)throw new JsonException("Unexpected digit card for basic arithmetic");
            return null;
        }
        if(c<1||c>9||c>=a||c==b)
            throw new JsonException("Digit cards must be distinct and in range");
        var svg=SvgStart("Two different digit cards fill two number puzzle blanks. Each card may only be used once.");
        Text(svg,35,39,"Place one digit card in EACH box.","hint");
        svg.Append("<rect x='52' y='69' width='48' height='47' class='shape'/>");
        Text(svg,115,99,$"+ {a-b} = {a}","label");
        svg.Append("<rect x='169' y='145' width='48' height='47' class='shape'/>");
        Text(svg,52,176,$"{a} −","label");
        Text(svg,233,176,$"= {a-c}","label");
        Text(svg,210,237,"Different cards, no repetition.","hint");
        return SvgEnd(svg);
    }
}

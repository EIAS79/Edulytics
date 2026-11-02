using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>First-party equal-area fraction diagrams for original Grade 1
/// Unit 7 Lessons 9-11: same whole, equal halves and equal fourths.
/// Visual comprehension is tested, never physical folding/cutting claims.</summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneSourceEqualAreaVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a<1||a>12||b<1||b>12||c!=0||mode<0||mode>3)
            throw new JsonException("Invalid source fractions operands");
        if(family is not ("usccss.g1.u07.l09.equal_halves_fourths" or
                          "usccss.g1.u07.l10.parts_make_whole" or
                          "usccss.g1.u07.l11.bigger_piece_halves_fourths"))
            throw new JsonException("Source task has no equal-area visual");
        var svg=SvgStart(family=="usccss.g1.u07.l10.parts_make_whole"&&mode>=2?
            "Original shaded-piece sort: inspect whether the depicted areas are equal, not merely the number of regions.":
            "First grade equal-area fractions, partitioned into matching equal pieces.");
        bool fourths=mode%2==1;
        if(family=="usccss.g1.u07.l11.bigger_piece_halves_fourths")
        {
            // Identical sized wholes: compare a shaded one-half vs one-fourth.
            FractionCircle(svg,112,124,2);
            FractionCircle(svg,308,124,4);
            Text(svg,112,236,"Piece A","hint");
            Text(svg,308,236,"Piece B","hint");
        }
        else if(family=="usccss.g1.u07.l10.parts_make_whole"&&mode>=2)
        {
            // Student must inspect a first-party source-inspired picture:
            // mode 2 has unequal regions; mode 3 has four equal regions.
            svg.Append("<rect class='shape' x='126' y='39' width='168' height='168' fill='none'/>");
            if(mode==2)
            {
                svg.Append("<rect x='126' y='39' width='61' height='168' fill='rgba(33,128,184,.28)'/>");
                svg.Append("<line class='partition-unequal' x1='187' y1='39' x2='187' y2='207' stroke='currentColor' stroke-width='2'/>");
            }
            else
            {
                svg.Append("<rect x='126' y='39' width='84' height='84' fill='rgba(33,128,184,.28)'/>");
                svg.Append("<line class='partition-equal' x1='210' y1='39' x2='210' y2='207' stroke='currentColor' stroke-width='2'/>");
                svg.Append("<line class='partition-equal' x1='126' y1='123' x2='294' y2='123' stroke='currentColor' stroke-width='2'/>");
            }
            Text(svg,210,240,"Compare the areas of the shaded and unshaded parts.","hint");
        }
        else if(fourths)
        {
            FractionCircle(svg,210,123,4);
            Text(svg,210,241,"Equal pieces of ONE circle","hint");
        }
        else
        {
            svg.Append("<rect x='126' y='39' width='168' height='168' fill='none' class='shape'/>");
            svg.Append("<rect x='126' y='39' width='84' height='168' fill='rgba(33,128,184,.28)' stroke='none'/>");
            svg.Append("<line x1='210' y1='39' x2='210' y2='207' stroke='currentColor' stroke-width='2'/>");
            Text(svg,210,240,"Equal pieces of ONE rectangle","hint");
        }
        return SvgEnd(svg);
    }
    private static void FractionCircle(System.Text.StringBuilder svg,int cx,int cy,int denominator)
    {
        const int r=78;
        svg.Append($"<circle cx='{cx}' cy='{cy}' r='{r}' class='shape'/>");
        if(denominator==2)
        {
            svg.Append($"<path d='M {cx} {cy-r} A {r} {r} 0 0 0 {cx} {cy+r} Z' fill='rgba(33,128,184,.28)' stroke='none'/>");
            svg.Append($"<line x1='{cx}' y1='{cy-r}' x2='{cx}' y2='{cy+r}' stroke='currentColor' stroke-width='2'/>");
        }
        else
        {
            svg.Append($"<path d='M {cx} {cy} L {cx} {cy-r} A {r} {r} 0 0 1 {cx+r} {cy} Z' fill='rgba(33,128,184,.28)' stroke='none'/>");
            svg.Append($"<line x1='{cx-r}' y1='{cy}' x2='{cx+r}' y2='{cy}' stroke='currentColor' stroke-width='2'/>");
            svg.Append($"<line x1='{cx}' y1='{cy-r}' x2='{cx}' y2='{cy+r}' stroke='currentColor' stroke-width='2'/>");
        }
    }
}

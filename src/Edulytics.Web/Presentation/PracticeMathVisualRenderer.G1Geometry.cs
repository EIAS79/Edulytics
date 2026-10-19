using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party SVG representations of original 2021 Grade 1 U07 solid and
/// flat-shape assessment families. All symbols are reconstructed geometry,
/// not copied publisher diagrams and not a record of classroom construction.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string? GradeOneSourceShapeVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a<1||a>12||b<1||b>12||c!=0||mode<0||mode>3)
            throw new JsonException("Invalid original first-grade shape parameters");
        bool isArithmetic=family is
            "usccss.g1.u07.l08.center_shapes_add_subtract" or
            "usccss.g1.u07.l12.center_shape_number" or
            "usccss.g1.u07.l17.center_geometry_and_time";
        if(isArithmetic&&mode%2==0)return null;
        var svg=SvgStart("A flat or solid mathematical shape model for first-grade visual reasoning.");
        switch(family)
        {
            case "usccss.g1.u07.l01.sort_solids_attributes":
                switch(mode)
                {
                    case 0:
                        svg.Append("<path class='shape' d='M143 99 L230 63 L303 96 L217 134 Z M143 99 V173 L217 207 V134 M303 96 V170 L217 207 M230 63 V134'/>");
                        break;
                    case 1:
                        svg.Append("<circle cx='210' cy='132' r='79' class='shape'/>");
                        svg.Append("<ellipse cx='210' cy='132' rx='79' ry='24' class='shape' stroke-dasharray='6 4'/>");
                        break;
                    case 2:
                        svg.Append("<ellipse cx='210' cy='69' rx='65' ry='23' class='shape'/>");
                        svg.Append("<path class='shape' d='M145 69 V181 A65 23 0 0 0 275 181 V69 M145 181 A65 23 0 0 0 275 181'/>");
                        break;
                    default:
                        svg.Append("<ellipse cx='210' cy='191' rx='80' ry='23' class='shape'/>");
                        svg.Append("<path class='shape' d='M210 38 L130 191 M210 38 L290 191'/>");
                        break;
                }
                break;
            case "usccss.g1.u07.l02.compose_solids_components":
                // Every first block (cube) or second block (rectangular prism)
                // is one countable piece, unlike a mere text statement of a+b.
                for(int n=0;n<a+b;n++)
                {
                    int col=n%6,row=n/6;
                    int x=42+col*59,y=49+row*47;
                    if(n<a)
                        svg.Append($"<rect class='shape-cube' x='{x}' y='{y}' width='27' height='27' fill='rgba(33,128,184,.22)' stroke='currentColor' stroke-width='1.5'/>");
                    else
                        svg.Append($"<rect class='shape-prism' x='{x}' y='{y+5}' width='39' height='18' fill='rgba(204,134,41,.23)' stroke='currentColor' stroke-width='1.5'/>");
                }
                Text(svg,210,249,"Count each shape component once.","hint");
                break;
            case "usccss.g1.u07.l03.sort_flat_shapers":
            case "usccss.g1.u07.l04.draw_shape_attribute_check":
                DrawFlatShape(svg,mode%2==0?3:4,false);
                break;
            case "usccss.g1.u07.l05.defining_triangles":
                DrawFlatShape(svg,3,false);
                break;
            case "usccss.g1.u07.l06.square_and_rectangle_properties":
                DrawFlatShape(svg,4,mode%2==1);
                break;
            case "usccss.g1.u07.l07.compose_flat_shapers":
                svg.Append("<rect x='117' y='50' width='186' height='154' fill='none' class='shape'/>");
                svg.Append("<path class='shape' d='M117 50 L303 204'/>");
                break;
            case "usccss.g1.u07.l08.center_shapes_add_subtract":
            case "usccss.g1.u07.l12.center_shape_number":
                DrawFlatShape(svg,3,false);
                break;
            case "usccss.g1.u07.l17.center_geometry_and_time":
                DrawFlatShape(svg,4,true);
                break;
            default:
                throw new JsonException("Not an original G1 unit 7 shape visual family");
        }
        return SvgEnd(svg);
    }
    private static void DrawFlatShape(System.Text.StringBuilder svg,int sides,bool rectangle)
    {
        if(sides==3)
            svg.Append("<polygon class='shape' points='202,51 108,201 312,201'/>");
        else if(rectangle)
            svg.Append("<rect x='98' y='83' width='224' height='105' class='shape'/>");
        else
            svg.Append("<rect x='132' y='49' width='156' height='156' class='shape'/>");
    }
}

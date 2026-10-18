using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneUnit2EquationStoryFiveVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),s=GetInt(p,"sum"),mode=GetInt(p,"mode");
        if(a is <2 or >6||b is <2 or >6||s!=a+b||s>10||mode is <0 or >2)
            throw new JsonException("Source-aligned Grade 1 story parameters invalid.");
        var sb=SvgStart("Number-story parts and unknowns, without revealing answers.");
        Text(sb,208,30,"Story model: start, change, total","label");
        var x=45;
        for(var n=0;n<a;n++)
        {
            sb.Append($"<circle cx='{x+(n%5)*20}' cy='{81+(n/5)*25}' r='8' fill='#71a5d6' stroke='#28466b'/>");
        }
        x=235;
        for(var n=0;n<b;n++)
        {
            sb.Append($"<circle cx='{x+(n%5)*20}' cy='{81+(n/5)*25}' r='8' fill='#e8c16e' stroke='#74552a'/>");
        }
        Text(sb,92,163,"First collection","label");
        Text(sb,305,163,"Second collection","label");
        var hint=family switch
        {
            "usccss.g1.u02.story.compare_structure_and_equation"
                =>"Name the unknown and match the story's action.",
            "usccss.g1.u02.story.match_equation_with_unknown_box"
                =>"A box stands for an unknown, NOT zero.",
            "usccss.g1.u02.story.two_matching_equations"
                =>"Describe the same quantities with TWO equations.",
            "usccss.g1.u02.story.compose_scaffold_from_equation"
                =>"Make your own joining or removing story.",
            _=>throw new JsonException("Unknown 2021 story family")
        };
        Text(sb,210,223,hint,"hint");
        return SvgEnd(sb);
    }
}

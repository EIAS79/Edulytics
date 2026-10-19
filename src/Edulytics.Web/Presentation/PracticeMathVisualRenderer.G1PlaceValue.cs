using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>First-party visual tens-and-ones representations for original
/// new Grade 1 Unit4/6/8 source tasks; supports quantities through 120.
/// Counts are mathematically represented, but drawing actual student cubes
/// or collecting real physical measurements remains unobserved.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneNewTensOnesVisual(string family,JsonElement p)
    {
        int tens,ones;
        if(family.StartsWith("usccss.g1.u04.",StringComparison.Ordinal))
        {
            tens=GetInt(p,"tens");
            ones=GetInt(p,"ones");
            if(tens<1||tens>9||ones<0||ones>9||
               GetInt(p,"unused")!=0||
               GetInt(p,"mode") is <0 or >3)
                throw new JsonException("Invalid first-grade tens/ones");
        }
        else
        {
            int value=GetInt(p,"a");
            if(GetInt(p,"b")!=0||GetInt(p,"c")!=0||
               GetInt(p,"mode") is <0 or >3)
                throw new JsonException("Invalid original number representation parameters");
            int min=family switch
            {
                "usccss.g1.u06.l08.count_to110_cube_groups"=>20,
                "usccss.g1.u06.l09.write_number_to120"=>100,
                "usccss.g1.u08.l08.same_number_tens_ones"=>20,
                _=>throw new JsonException("Unrelated source number family")
            };
            int max=family switch
            {
                "usccss.g1.u06.l08.count_to110_cube_groups"=>110,
                "usccss.g1.u06.l09.write_number_to120"=>120,
                "usccss.g1.u08.l08.same_number_tens_ones"=>99,
                _=>throw new JsonException("Unrelated source number family")
            };
            if(value<min||value>max)throw new JsonException("Out of source scope number");
            tens=value/10;
            ones=value%10;
        }
        var svg=SvgStart("Count the base-ten rods and single cubes. Each rod represents exactly ten equal units.");
        // Each ten-rod has a physical division for every one of its ten cubes.
        for(int n=0;n<tens;n++)
        {
            int col=n%4,row=n/4;
            int x=27+col*99,y=37+row*58;
            svg.Append($"<rect class='g1-ten-rod' x='{x}' y='{y}' width='87' height='28' fill='rgba(33,128,184,.21)' stroke='currentColor' stroke-width='1.4'/>");
            for(int t=1;t<10;t++)
            {
                var xx=x+87*t/10.0;
                svg.Append($"<line x1='{F(xx)}' y1='{y}' x2='{F(xx)}' y2='{y+28}' stroke='currentColor' stroke-width='.7'/>");
            }
        }
        for(int n=0;n<ones;n++)
        {
            int x=32+n*38;
            svg.Append($"<rect class='g1-one-cube' x='{x}' y='212' width='23' height='23' fill='rgba(204,134,41,.28)' stroke='currentColor' stroke-width='1.4'/>");
        }
        return SvgEnd(svg);
    }

    private static string? GradeOneNewCountOnPath(JsonElement p)
    {
        int start=GetInt(p,"a"),steps=GetInt(p,"b");
        if(GetInt(p,"c")!=0||GetInt(p,"mode") is <0 or >3||
           start<1||start>119||steps<1||steps>120-start)
            throw new JsonException("Invalid original count-on task");
        if(steps>10)return null; // Avoid a misleading compressed large path.
        var svg=SvgStart("Start at the shown number, then count one more for each blank hop.");
        svg.Append("<line x1='52' y1='137' x2='375' y2='137' stroke='currentColor' stroke-width='2'/>");
        var dx=300.0/steps;
        Text(svg,52,99,start.ToString(),"label");
        for(int n=0;n<=steps;n++)
        {
            var x=52+n*dx;
            svg.Append($"<circle cx='{F(x)}' cy='137' r='4' fill='currentColor'/>");
            if(n>0)
                svg.Append($"<path d='M {F(x-dx+7)} 128 Q {F(x-dx/2)} 72 {F(x-7)} 128' stroke='currentColor' stroke-width='2' fill='none'/>");
        }
        Text(svg,210,235,"Count each hop aloud; find the final number.","hint");
        return SvgEnd(svg);
    }
}

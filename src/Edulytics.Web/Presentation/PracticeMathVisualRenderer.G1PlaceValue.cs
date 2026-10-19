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
                "usccss.g1.u08.l07.count_on_to120"=>20,
                _=>throw new JsonException("Unrelated source number family")
            };
            int max=family switch
            {
                "usccss.g1.u06.l08.count_to110_cube_groups"=>110,
                "usccss.g1.u06.l09.write_number_to120"=>120,
                "usccss.g1.u08.l08.same_number_tens_ones"=>99,
                "usccss.g1.u08.l07.count_on_to120"=>120,
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

    // Source-inspired, first-party Unit4 L04 bag-of-cubes drawings. They
    // illustrate the quantities, not student-created physical tower work.
    private static string GradeOneOriginalBagsOfTenVisual(JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(c!=0||mode<0||mode>3||a<2||a>7||b<1||
           (mode%2==0?b>10-a:b>a)||
           (mode==0&&(a!=3||b!=2))||
           (mode==1&&(a!=7||b!=4)))
            throw new JsonException("Invalid original Grade 1 U04 L04 bag quantities");

        var svg=SvgStart("First-party Grade 1 tens representation. Each rod consists of ten equal cubes; original classroom manipulatives and drawings are teacher assessed.");
        const int step=49,rodWidth=42;
        if(mode%2==0)
        {
            Text(svg,24,33,mode==0?"Jada: Bag A":"First group of tens","label");
            Text(svg,24,143,mode==0?"Jada: Bag B":"Second group of tens","label");
            for(int group=0;group<2;group++)
            {
                int count=group==0?a:b,y=group==0?57:166;
                for(int k=0;k<count;k++)
                {
                    int x=26+k*step;
                    svg.Append($"<rect class='g1-source-bag-ten-{group}' x='{x}' y='{y}' width='{rodWidth}' height='30' fill='rgba(33,128,184,.22)' stroke='currentColor' stroke-width='1.5'/>");
                    for(int n=1;n<10;n++)
                        svg.Append($"<line x1='{F(x+rodWidth*n/10d)}' y1='{y}' x2='{F(x+rodWidth*n/10d)}' y2='{y+30}' stroke='currentColor' stroke-width='.7'/>");
                }
            }
        }
        else
        {
            Text(svg,24,33,mode==1?"Tyler: Bag C before taking away":"Tens before subtraction","label");
            Text(svg,24,166,"Outlined rods are taken away.","hint");
            for(int k=0;k<a;k++)
            {
                int x=26+k*step,y=66;
                string fill=k<b?"rgba(204,134,41,.20)":"rgba(33,128,184,.22)";
                svg.Append($"<rect class='g1-source-bag-ten-{(k<b?"removed":"remaining")}' x='{x}' y='{y}' width='{rodWidth}' height='30' fill='{fill}' stroke='currentColor' stroke-width='1.5'/>");
                for(int n=1;n<10;n++)
                    svg.Append($"<line x1='{F(x+rodWidth*n/10d)}' y1='{y}' x2='{F(x+rodWidth*n/10d)}' y2='{y+30}' stroke='currentColor' stroke-width='.7'/>");
            }
        }
        return SvgEnd(svg);
    }

    private static string? GradeOneNewCountOnPath(JsonElement p)
    {
        int start=GetInt(p,"a"),steps=GetInt(p,"b");
        int mode=GetInt(p,"mode");
        if(GetInt(p,"c")!=0||mode is <0 or >3)
            throw new JsonException("Invalid original counting task");
        if(mode>=2)
        {
            if(start<20||start>120||steps!=0)
                throw new JsonException("Invalid original 120-collection task");
            return GradeOneNewTensOnesVisual(
                "usccss.g1.u08.l07.count_on_to120",p);
        }
        if(start<1||start>119||steps<1||steps>120-start)
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

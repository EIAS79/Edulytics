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

    // 2021 U04 L05 Check It Off: board numbers 0..90, with one checked
    // and the desired target free. The learner must choose ADD or SUBTRACT.
    private static string GradeOneOriginalCheckItOffBoard(JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(c!=0||mode is <4 or >5||
           (mode==4&&(a!=5||b!=4))||
           (mode==5&&(a<2||a>7||b<1||b>Math.Min(a,9-a))))
            throw new JsonException("Invalid Grade 1 Check It Off board");
        int crossed=mode==4?90:10*(a-b);
        int needed=mode==4?10:10*(a+b);
        var svg=SvgStart("Source-inspired Check It Off 0 to 90 number board. One number is already crossed out and another is available. Choose the equation and operator yourself.");
        Text(svg,20,32,mode==4?"Cards 40 and 50":"Two number cards shown in the question","label");
        for(int k=0;k<10;k++)
        {
            int n=k*10,x=20+(k%5)*78,y=49+(k/5)*79;
            string state=n==crossed?"g1-checked-off":
                n==needed?"g1-free-target":"g1-other-board";
            svg.Append($"<rect class='{state}' x='{x}' y='{y}' width='64' height='54' rx='5' fill='none' stroke='currentColor' stroke-width='1.5'/>");
            Text(svg,x+15,y+36,n.ToString(),"label");
            if(n==crossed)
                svg.Append($"<line class='g1-board-cross' x1='{x+5}' y1='{y+5}' x2='{x+59}' y2='{y+49}' stroke='currentColor' stroke-width='2.4'/>");
        }
        Text(svg,18,232,"Which operation covers an unchecked number? Explain.","hint");
        return SvgEnd(svg);
    }

    // Original 2021 U04 L01: a fixed 40-object bag in Activity 1,
    // and Bags A-F containing respectively 20,40,50,60,30,10 in Activity 2.
    // First-party dots represent countable pieces, NOT actual student sorting.
    private static string GradeOneOriginalSourceCountCollectionsVisual(JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a<1||a>6||b!=0||c!=0||mode<0||mode>3||(mode==0&&a!=4))
            throw new JsonException("Invalid original Grade 1 U04 L01 collection");
        string label=mode==0?"Activity 1: fixed 2021 collection":
            mode==1?"Activity 2: Bag "+(a switch{1=>"F",2=>"A",3=>"E",4=>"B",5=>"C",_=>"D"}):
            mode==2?"Ten-frame representation":"Tower representation";
        var svg=SvgStart("Grade 1 original counting collections. Count each unnumbered group of ten dots; source classroom manipulation and student drawings require teacher review.");
        Text(svg,16,32,label,"label");
        for(int group=0;group<a;group++)
        {
            int x=20+(group%3)*133,y=48+(group/3)*103;
            svg.Append($"<rect class='g1-original-collection-frame' x='{x}' y='{y}' width='115' height='79' rx='5' fill='none' stroke='currentColor' stroke-width='1.5'/>");
            for(int dot=0;dot<10;dot++)
            {
                int cx=x+15+(dot%5)*21,cy=y+24+(dot/5)*29;
                svg.Append($"<circle class='g1-original-collection-dot' cx='{cx}' cy='{cy}' r='6' fill='rgba(33,128,184,.45)' stroke='currentColor' stroke-width='.8'/>");
            }
        }
        Text(svg,16,248,"Count all the pieces. Each complete frame contains ten.","hint");
        return SvgEnd(svg);
    }

    // Original 2021 U04 L03: starting tower(s) and eight as-yet-unfilled
    // addition/removal steps; blanks deliberately do NOT reveal eight totals.
    private static string GradeOneOriginalTenTowerSequenceVisual(JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(c!=0||b!=1||mode<2||mode>3||
           (mode==2&&a!=9)||(mode==3&&a!=1))
            throw new JsonException("Invalid original source ten-tower sequence");
        bool adding=mode==2;
        var svg=SvgStart(adding
            ?"Original Grade 1: start with one full tower of ten cubes, add a whole ten tower eight times. Empty outlines are unfilled steps, not answers."
            :"Original Grade 1: start with nine full towers of ten cubes, take one full tower away eight times. Shaded rods are marked for removal, not scored answers.");
        Text(svg,20,36,adding?"Start: ONE TEN. Add one ten 8 times.":"Start: NINE TENS. Remove one ten 8 times.","label");
        const int rodWidth=38,step=43,rodY=78;
        for(int k=0;k<9;k++)
        {
            int x=20+k*step;
            string marker=adding&&k>0?"step-blank":!adding&&k<8?"step-remove":"step-start";
            string fill=adding&&k>0?"none":!adding&&k<8?
                "rgba(204,134,41,.20)":"rgba(33,128,184,.22)";
            svg.Append($"<rect class='g1-original-ten-{marker}' x='{x}' y='{rodY}' width='{rodWidth}' height='56' fill='{fill}' stroke='currentColor' stroke-width='1.6' stroke-dasharray='{(adding&&k>0?"3 3":"none")}'/>");
            if(!adding||k==0)
                for(int m=1;m<10;m++)
                {
                    var y=rodY+56*m/10d;
                    svg.Append($"<line x1='{x}' y1='{F(y)}' x2='{x+rodWidth}' y2='{F(y)}' stroke='currentColor' stroke-width='.65'/>");
                }
        }
        Text(svg,20,181,adding
            ?"Write the 8 totals after each new tower.":"Write the 8 totals after removing each tower.","hint");
        Text(svg,20,217,"Every tower contains exactly TEN equal cubes.","hint");
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

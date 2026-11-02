using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneUnit2NextFiveVisual(string family,JsonElement p)
    {
        var sb=SvgStart("First-party Grade 1 mathematical representation. No answer is shown.");
        switch (family)
        {
            case "usccss.g1.u02.pets.write_two_equations":
            {
                var a=GetInt(p,"a");var b=GetInt(p,"b");
                if(a is <2 or >5 || b is <2 or >5 || a==b || a+b>10)throw new JsonException();
                Text(sb,95,35,"Cats","label");Text(sb,314,35,"Dogs","label");
                // Distinct first-party animal icons rather than abstract circles/rectangles.
                // The exact number in each group is the source-given information.
                for(var i=0;i<a;i++)
                {
                    var x=44+i*28;
                    sb.Append($"<g data-animal='cat' transform='translate({x} 111)'><path d='M-12 -5 L-11 -20 L-3 -14 L5 -14 L13 -20 L12 -5 C16 5 11 17 0 17 C-12 17 -16 5 -12 -5 Z' class='shape fill'/><circle cx='-5' cy='0' r='2'/><circle cx='5' cy='0' r='2'/></g>");
                }
                for(var i=0;i<b;i++)
                {
                    var x=258+i*28;
                    sb.Append($"<g data-animal='dog' transform='translate({x} 111)'><path d='M-12 -7 Q0 -17 12 -7 L11 10 Q0 21 -11 10 Z' class='shape fill'/><path d='M-11 -8 Q-21 -13 -19 3 L-12 9 M11 -8 Q21 -13 19 3 L12 9' class='shape'/><circle cx='-5' cy='0' r='2'/><circle cx='5' cy='0' r='2'/></g>");
                }
                Text(sb,210,207,"Count both animal groups; write two equations.","hint");
                break;
            }
            case "usccss.g1.u02.shake_spill.enumerate_partitions":
            {
                var n=GetInt(p,"total");if(n is <1 or >10)throw new JsonException();
                Text(sb,205,37,"Two-sided counters: shake and spill","label");
                // Show unassigned red/yellow reversible counters, NOT a pre-solved split.
                // The pupil must generate the color arrangements and equations.
                for(var i=0;i<n;i++)
                {
                    var x=69+(i%5)*68;var y=96+(i/5)*55;
                    sb.Append($"<g data-role='unassigned-two-color-counter'><circle cx='{x}' cy='{y}' r='18' class='shape' style='fill:#bc4550'/><path d='M{x} {y-18} A18 18 0 0 1 {x} {y+18} Z' style='fill:#e8c94d'/></g>");
                }
                Text(sb,210,226,"Choose your own red/yellow splits and record equations.","hint");
                break;
            }
            case "usccss.g1.u02.shake_cover.write_missing_addend":
            {
                var n=GetInt(p,"total");var visible=GetInt(p,"visible");
                if(n is <6 or >10 || visible<2 || visible>=n)throw new JsonException();
                Text(sb,103,40,"Visible counters","label");
                for(var i=0;i<visible;i++)
                    sb.Append($"<circle cx='{42+(i%5)*35}' cy='{92+(i/5)*42}' r='13' class='shape fill'/>");
                sb.Append("<path d='M273 85 H372 L365 166 H281 Z' class='shape fill'/>");
                Text(sb,321,128,"?","label");Text(sb,321,41,"Under cup","label");
                Text(sb,210,222,$"{n} counters in all; find the covered part.","hint");
                break;
            }
            case "usccss.g1.u02.story.compare_unknowns_equations":
            {
                var n=GetInt(p,"total");var a=GetInt(p,"a");var mode=GetInt(p,"mode");
                if(n is <6 or >10||a<2||a>=n||mode is <0 or >2)throw new JsonException();
                // Three distinct original story types need three different diagrams.
                // Never display an invented split for the both-parts-unknown task.
                if(mode==0)
                {
                    Text(sb,99,34,"Turtles","label");Text(sb,320,34,"Lizards","label");
                    for(var i=0;i<a;i++)
                    {
                        var x=47+(i%4)*36;var y=91+(i/4)*44;
                        sb.Append($"<g data-animal='turtle' transform='translate({x} {y})'><ellipse rx='10' ry='8' class='shape fill'/><circle cx='13' cy='0' r='4' class='shape fill'/></g>");
                    }
                    for(var i=0;i<n-a;i++)
                    {
                        var x=267+(i%4)*34;var y=91+(i/4)*44;
                        sb.Append($"<g data-animal='lizard' transform='translate({x} {y})'><path d='M-13 0 L-4 -7 L9 -4 L16 0 L9 4 L-4 7 Z' class='shape fill'/><line x1='-13' y1='0' x2='-19' y2='0' class='shape'/></g>");
                    }
                    Text(sb,210,226,"Count both groups to find how many in all.","hint");
                }
                else if(mode==1)
                {
                    Text(sb,95,38,$"Reptiles altogether: {n}","label");
                    Text(sb,314,38,$"Turtles: {a}","label");
                    for(var i=0;i<a;i++)
                    {
                        var x=245+(i%4)*38;var y=94+(i/4)*48;
                        sb.Append($"<g data-animal='turtle' transform='translate({x} {y})'><ellipse rx='10' ry='8' class='shape fill'/><circle cx='13' cy='0' r='4' class='shape fill'/></g>");
                    }
                    Text(sb,96,123,"Lizards: ?","label");
                    Text(sb,210,225,"Model only the known part; work out the hidden part.","hint");
                }
                else
                {
                    Text(sb,206,35,$"Reptiles altogether: {n}","label");
                    for(var i=0;i<n;i++)
                    {
                        var x=65+(i%5)*70;var y=94+(i/5)*49;
                        sb.Append($"<circle data-role='unassigned-counter' cx='{x}' cy='{y}' r='13' class='shape fill'/>");
                    }
                    Text(sb,93,198,"Turtles: ?","label");Text(sb,306,198,"Lizards: ?","label");
                    Text(sb,210,231,"Choose two different ways to split the whole.","hint");
                }
                break;
            }
            case "usccss.g1.u02.center.capture_squares.select_legal_edge":
            {
                var x=GetInt(p,"first");var y=GetInt(p,"second");var mask=GetInt(p,"mask");
                if(x is <1 or >5||y is <1 or >5||x+y is <6 or >9 ||
                    !(mask==0||mask is 7 or 11 or 13 or 14))throw new JsonException();
                Text(sb,100,36,$"Dice: {x} and {y}","label");
                const int gridX=210,gridY=38,cellW=88,cellH=86;
                for(var r=0;r<2;r++)
                for(var c=0;c<2;c++)
                {
                    var label=r==0?(c==0?"A1":"A2"):(c==0?"B1":"B2");
                    var num=6+2*r+c;
                    var left=gridX+cellW*c;var top=gridY+cellH*r;
                    // Dashed outlines are NOT previously drawn edges.
                    sb.Append($"<rect x='{left}' y='{top}' width='{cellW}' height='{cellH}' class='tick dash' fill='none'/>");
                    Text(sb,left+43,top+31,label,"label");
                    Text(sb,left+43,top+62,num.ToString(),"label");
                    if(num!=x+y||mask==0)continue;
                    if((mask&1)!=0)sb.Append($"<line x1='{left}' y1='{top}' x2='{left+cellW}' y2='{top}' class='shape' stroke-width='7'/>");
                    if((mask&2)!=0)sb.Append($"<line x1='{left+cellW}' y1='{top}' x2='{left+cellW}' y2='{top+cellH}' class='shape' stroke-width='7'/>");
                    if((mask&4)!=0)sb.Append($"<line x1='{left}' y1='{top+cellH}' x2='{left+cellW}' y2='{top+cellH}' class='shape' stroke-width='7'/>");
                    if((mask&8)!=0)sb.Append($"<line x1='{left}' y1='{top}' x2='{left}' y2='{top+cellH}' class='shape' stroke-width='7'/>");
                }
                Text(sb,210,232,mask==0
                    ?"Find a legal first edge on the matching square."
                    :"Three thick edges: close the square.","hint");
                break;
            }
            default:throw new JsonException("Unsupported G1 U02 L06–L10 visual.");
        }
        return SvgEnd(sb);
    }
}

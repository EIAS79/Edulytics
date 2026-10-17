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
                Text(sb,92,34,"Cats","label");Text(sb,300,34,"Dogs","label");
                for(int i=0;i<a;i++)sb.Append($"<circle cx='{47+i*27}' cy='105' r='11' class='shape fill'/>");
                for(int i=0;i<b;i++)sb.Append($"<rect x='{252+i*27}' y='94' width='21' height='21' rx='4' class='shape fill'/>");
                Text(sb,210,205,"Write both matching addition equations.","hint");
                break;
            }
            case "usccss.g1.u02.shake_spill.enumerate_partitions":
            {
                var n=GetInt(p,"total");if(n is <4 or >8)throw new JsonException();
                Text(sb,205,39,"Shake and Spill: split the counters","label");
                for(var i=0;i<n;i++)
                    sb.Append($"<circle cx='{55+(i%5)*53}' cy='{92+(i/5)*49}' r='17' class='shape fill'/>");
                Text(sb,205,221,"How many different two-color splits?","hint");
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
                Text(sb,208,34,"Reptiles in two groups","label");
                for(var i=0;i<n;i++)
                    sb.Append($"<circle cx='{42+(i%5)*35}' cy='{90+(i/5)*44}' r='12' class='shape fill'/>");
                Text(sb,205,224,mode==2
                    ?"Find two different splits of the total."
                    :"Write equations that fit this story.","hint");
                break;
            }
            case "usccss.g1.u02.center.capture_squares.select_legal_edge":
            {
                var x=GetInt(p,"first");var y=GetInt(p,"second");
                if(x is <1 or >5||y is <1 or >5||x+y is <6 or >9)throw new JsonException();
                Text(sb,100,36,$"Dice: {x} and {y}","label");
                const int gridX=210,gridY=38,cellW=88,cellH=86;
                for(var r=0;r<2;r++)
                for(var c=0;c<2;c++)
                {
                    var label= r==0?(c==0?"A1":"A2"):(c==0?"B1":"B2");
                    var num=6+2*r+c;
                    sb.Append($"<rect x='{gridX+cellW*c}' y='{gridY+cellH*r}' width='{cellW}' height='{cellH}' class='shape' fill='none'/>");
                    Text(sb,gridX+cellW*c+43,gridY+cellH*r+31,label,"label");
                    Text(sb,gridX+cellW*c+43,gridY+cellH*r+62,num.ToString(),"label");
                }
                Text(sb,210,232,"Find the sum. Choose its cell and one edge.","hint");
                break;
            }
            default:throw new JsonException("Unsupported G1 U02 L06–L10 visual.");
        }
        return SvgEnd(sb);
    }
}

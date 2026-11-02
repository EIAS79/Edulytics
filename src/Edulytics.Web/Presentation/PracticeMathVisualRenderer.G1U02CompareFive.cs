using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneUnit2CompareFiveVisual(string family,JsonElement p)
    {
        int smaller=GetInt(p,"small"),larger=GetInt(p,"big");
        int other=GetInt(p,"other"),mode=GetInt(p,"mode");
        if(smaller is <2 or >7||larger<=smaller||larger>10||
           other is <1 or >7 || mode is <0 or >2)throw new JsonException();
        var sb=SvgStart("First-party comparison diagram, unknown difference is not revealed.");
        void Stack(int x,int y,int count,string label,string fill)
        {
            Text(sb,x+27,32,label,"label");
            for(var i=0;i<count;i++)
                sb.Append($"<rect x='{x+(i%5)*18}' y='{y+(i/5)*27}' width='14' height='19' rx='2' class='shape' style='fill:{fill}'/>");
        }
        switch(family)
        {
            case "usccss.g1.u02.compare.equal_cubes_write_equation":
            case "usccss.g1.u02.compare.school_supplies_difference":
            case "usccss.g1.u02.compare.write_related_equations":
                Stack(48,88,larger,family.Contains("supplies",StringComparison.Ordinal)
                    ?"Notebooks":"Tall tower","#70a4de");
                Stack(265,88,smaller,family.Contains("supplies",StringComparison.Ordinal)
                    ?"Pencils":"Short tower","#e8b85b");
                Text(sb,210,213,"Match pairs. How many are unmatched?","hint");
                break;
            case "usccss.g1.u02.compare.art_supply_data_difference":
                Text(sb,92,36,"Crayons","label");
                Text(sb,210,36,"Markers","label");
                Text(sb,334,36,"Paint","label");
                var labels=new[]{larger,smaller,other};
                var coords=new[]{45,164,283};
                var colors=new[]{"#d77478","#6ca7d6","#e6c26a"};
                for(var category=0;category<3;category++)
                    for(var n=0;n<labels[category];n++)
                        sb.Append($"<circle cx='{coords[category]+(n%4)*18}' cy='{85+(n/4)*28}' r='7' class='shape' style='fill:{colors[category]}'/>");
                Text(sb,210,223,"Compare Crayons with Markers only.","hint");
                break;
            case "usccss.g1.u02.compare.mixed_story_equation":
                if(mode==0)
                {
                    Stack(46,88,smaller,"Red blocks","#cb6767");
                    Stack(267,88,larger-smaller,"Blue blocks","#70a4de");
                    Text(sb,210,219,"Find the total of both groups.","hint");
                }
                else if(mode==1)
                {
                    Stack(45,88,smaller,"Triangles","#e6ba5c");
                    Text(sb,324,36,"Squares: ?","label");
                    Text(sb,210,219,$"Total: {larger}. Find the hidden part.","hint");
                }
                else
                {
                    Stack(48,88,larger,"More beads","#70a4de");
                    Stack(265,88,smaller,"Fewer beads","#cb6767");
                    Text(sb,210,219,"Find the difference, not the total.","hint");
                }
                break;
            default:throw new JsonException("Unsupported G1 U02 L11-L15 visual family");
        }
        return SvgEnd(sb);
    }
}

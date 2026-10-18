using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>Original, nonpublisher Grade 1 lesson 41–45 visual scaffolds.
/// No answer strings or completed values are emitted to the SVG.</summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneNextSourceFiveVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a is <0 or >19 || b is <0 or >19 || c is <0 or >19 ||
           mode is <0 or >3)
            throw new JsonException("Invalid G1-009 mathematical visual inputs.");
        var sb=SvgStart("Source-grounded Grade 1 mathematical model; solve the missing parts.");
        if(family.Contains(".l08.ten_unit.",StringComparison.Ordinal))
        {
            Text(sb,210,26,"10 ones become ONE ten","label");
            // A fixed tower of ten unnumbered squares is the concept scaffold;
            // no teen-number answer or extra ones from the question are shown.
            for(int i=0;i<10;i++)
                sb.Append($"<rect x='98' y='{42+i*16}' width='21' height='15' rx='2' fill='#6294bd' stroke='#274f6c'/>");
            Text(sb,175,110,"ONE TEN","label");
            for(int i=0;i<6;i++)
                sb.Append($"<rect x='{242+(i%3)*28}' y='{84+(i/3)*28}' width='20' height='20' rx='3' fill='none' stroke='#688a9b' stroke-dasharray='3 3'/>");
            Text(sb,285,170,"? extra ones","hint");
            Text(sb,210,223,"Bundle ten before counting leftover singles","hint");
        }
        else if(family.Contains(".l04.make_ten.",StringComparison.Ordinal))
        {
            Text(sb,210,25,"Ten-frame: every slot is one counter","label");
            for(int i=0;i<10;i++)
            {
                var x=88+(i%5)*62;var y=65+(i/5)*55;
                sb.Append($"<rect x='{x}' y='{y}' width='43' height='41' rx='7' fill='#eef6f9' stroke='#547f98'/>");
                sb.Append($"<circle cx='{x+21}' cy='{y+21}' r='11' fill='none' stroke='#89aabd' stroke-dasharray='3 3'/>");
            }
            Text(sb,210,204,"Explore every red/yellow split of ten","hint");
        }
        else if(family.Contains(".l06.story.",StringComparison.Ordinal))
        {
            Text(sb,210,27,"Read the STORY before writing an equation","label");
            sb.Append("<rect x='125' y='63' width='170' height='120' rx='15' fill='#eef5f9' stroke='#48718d' stroke-width='3'/>");
            Text(sb,210,116,"? counters","hint");
            Text(sb,210,167,"start / change / total","hint");
            Text(sb,210,226,"The unknown may be at ANY position","hint");
        }
        else if(family.Contains(".l07.center.",StringComparison.Ordinal))
        {
            Text(sb,210,27,"Evaluate the WHOLE expression on each card","label");
            for(int i=0;i<2;i++)
            {
                var x=120+i*180;
                sb.Append($"<rect x='{x-65}' y='70' width='130' height='115' rx='12' fill='{(i==0?"#edf5fa":"#fff5e8")}' stroke='#527d9b' stroke-width='2'/>");
                Text(sb,x,110,i==0?"FIRST card":"SECOND card","label");
                Text(sb,x,146,"?","hint");
            }
            Text(sb,210,226,"Greater value wins; equal values TIE","hint");
        }
        else
        {
            Text(sb,210,28,"Subtract with counters or use a related addition","label");
            for(int i=0;i<8;i++)
            {
                var x=62+(i%8)*43;var y=96;
                sb.Append($"<circle cx='{x}' cy='{y}' r='14' fill='#78a6cb' stroke='#426989'/>");
            }
            sb.Append("<rect x='132' y='151' width='155' height='47' rx='9' fill='#eef5fa' stroke='#718da4'/>");
            Text(sb,210,178,"? + ? = ?","hint");
            Text(sb,210,222,"Count on, take away, and check","hint");
        }
        return SvgEnd(sb);
    }
}

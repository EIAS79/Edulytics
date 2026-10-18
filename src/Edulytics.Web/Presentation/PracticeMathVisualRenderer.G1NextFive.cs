using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>First-party diagrams for Grade 1 original 2021 ordinals 36–40.
/// Never reproduce original publisher artwork or render a completed answer.</summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneNextFiveVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a is <1 or >6 || b is <1 or >6 || c is <1 or >9 ||
           a+b>10 || mode is <0 or >2)
            throw new JsonException("Invalid G1-008 visual parameters.");
        var sb=SvgStart("Grade 1 counters, story data, and equation structures.");
        var survey=family.Contains(".survey.",StringComparison.Ordinal);
        var equation=family.Contains(".expressions.",StringComparison.Ordinal);
        if(survey)
        {
            Text(sb,210,28,"Read the labeled sample data groups","label");
            var groups=family.Contains(".survey.compare_question",StringComparison.Ordinal)?2:3;
            for(int g=0;g<groups;g++)
            {
                var count=g==0?a:g==1?b:c;
                var x=70+g*140;
                Text(sb,x,65,$"Group {(char)('A'+g)}","label");
                for(int n=0;n<count;n++)
                {
                    var cx=x+(n%4)*18-28;
                    var cy=95+(n/4)*20;
                    sb.Append($"<circle cx='{cx}' cy='{cy}' r='7' fill='{(g==0?"#5897cb":g==1?"#edc174":"#79b59b")}'/>");
                }
            }
            Text(sb,210,195,"Use the counts in matching equations","hint");
        }
        else if(equation)
        {
            Text(sb,210,30,"Equal sign means SAME VALUE","label");
            for(int i=0;i<3;i++)
            {
                var x=75+i*135;
                sb.Append($"<rect x='{x-48}' y='60' width='95' height='86' rx='9' fill='#edf5fa' stroke='#527da3'/>");
                Text(sb,x,85,$"Card {(char)('A'+i)}","label");
                Text(sb,x,115,"? + ?","hint");
            }
            Text(sb,210,200,"Compare sums; do not guess from addend order","hint");
        }
        else if(family.Contains("number_puzzles",StringComparison.Ordinal)||
                family.Contains("related_missing_values",StringComparison.Ordinal))
        {
            Text(sb,210,32,"Place the missing number in each box","label");
            sb.Append("<rect x='87' y='75' width='90' height='80' rx='7' fill='#eaf3fa' stroke='#325b83'/>");
            sb.Append("<rect x='244' y='75' width='90' height='80' rx='7' fill='#fff3dc' stroke='#866b38'/>");
            Text(sb,132,116,"?", "hint");
            Text(sb,289,116,"?", "hint");
            Text(sb,210,203,"Check both equations and digit-card rules","hint");
        }
        else
        {
            Text(sb,210,30,"Two parts make one total","label");
            for(int g=0;g<2;g++)
            {
                var count=g==0?a:b;
                var x=100+g*215;
                for(int n=0;n<count;n++)
                {
                    sb.Append($"<circle cx='{x+(n%4)*18-28}' cy='{91+(n/4)*23}' r='8' fill='{(g==0?"#5897cb":"#e7bb63")}' stroke='#38536b'/>");
                }
                Text(sb,x,165,g==0?"First addend":"Second addend","label");
            }
            Text(sb,210,218,"Count, explain, or reflect on the strategy","hint");
        }
        return SvgEnd(sb);
    }
}

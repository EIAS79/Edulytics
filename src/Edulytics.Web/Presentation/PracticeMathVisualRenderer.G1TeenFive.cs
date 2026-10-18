using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>Original first-party G1-010 ten-frame and story scaffolds.
/// No publisher artwork or answer-side hidden counters.</summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneTeenFiveVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(a is <0 or >20||b is <0 or >20||c is <0 or >3||mode is <0 or >3)
            throw new JsonException("Invalid G1-010 first-party model parameters.");
        var sb=SvgStart("Ten and ones model with blank learner work areas.");
        var story=family.Contains(".story_",StringComparison.Ordinal)||
                  family.Contains(".take_from",StringComparison.Ordinal)||
                  family.Contains(".change_unknown",StringComparison.Ordinal)||
                  family.Contains(".result_unknown",StringComparison.Ordinal);
        if(story)
        {
            Text(sb,210,28,"Match the STORY to the missing value","label");
            var names=new[]{"START","CHANGE","RESULT"};
            for(int i=0;i<3;i++)
            {
                int x=88+i*123;
                sb.Append($"<rect x='{x-50}' y='63' width='102' height='105' rx='12' fill='#edf5fa' stroke='#5e8ca9'/>");
                Text(sb,x,90,names[i],"label");
                Text(sb,x,136,"?","hint");
            }
            Text(sb,210,212,"Read the question before placing the blank","hint");
        }
        else
        {
            Text(sb,210,25,"A full ten-frame represents ONE TEN","label");
            for(int i=0;i<10;i++)
            {
                var x=40+(i%5)*39;var y=62+(i/5)*44;
                sb.Append($"<rect x='{x}' y='{y}' width='32' height='35' rx='4' fill='#78a7c9' stroke='#466d88' data-role='ten-counter'/>");
            }
            for(int i=0;i<10;i++)
            {
                var x=241+(i%5)*30;var y=67+(i/5)*41;
                sb.Append($"<rect x='{x}' y='{y}' width='24' height='30' rx='4' fill='none' stroke='#8eabbc' stroke-dasharray='3 3' data-role='empty-frame-slot'/>");
            }
            Text(sb,131,187,"one ten","label");
            Text(sb,310,187,"? ones","hint");
            Text(sb,210,226,"Empty slots are SPACE, not counted counters","hint");
        }
        return SvgEnd(sb);
    }
}

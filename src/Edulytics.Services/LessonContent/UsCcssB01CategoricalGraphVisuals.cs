using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// B01 categorical bar graphs: verified counts from the exact data question,
/// limited to original K-5 publisher graph lessons. A proportional chart is
/// not proof of completed academic review or all source activities.
/// </summary>
public static class UsCcssB01CategoricalGraphVisuals
{
    private static readonly HashSet<string> Lessons = new(StringComparer.Ordinal)
    {
        "PED:US-CCSS-MATH:G2:U01:L08",
        "PED:US-CCSS-MATH:G2:U01:L09",
        // Original Grade 2 U01 L10-L11: construct, read and answer questions
        // from the actual categorical picture/bar graphs, not equations only.
        "PED:US-CCSS-MATH:G2:U01:L10",
        "PED:US-CCSS-MATH:G2:U01:L11",
        "PED:US-CCSS-MATH:G2:U01:L13",
        "PED:US-CCSS-MATH:G2:U07:L18",
        // Original IM G3 U01 L01-L02 explicitly interpret or construct
        // picture and bar graphs; these source-verified counts can be drawn
        // from the actual verified categorical question parameters.
        "PED:US-CCSS-MATH:G3:U01:L01",
        "PED:US-CCSS-MATH:G3:U01:L02",
        "PED:US-CCSS-MATH:G3:U01:L04",
        "PED:US-CCSS-MATH:G3:U01:L05",
        "PED:US-CCSS-MATH:G3:U01:L07",
        "PED:US-CCSS-MATH:G3:U01:L08",
        "PED:US-CCSS-MATH:G3:U08:L07"
    };

    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if (!Lessons.Contains(lessonCode)) return [];
        foreach (var q in questions)
        {
            if (!ExactSkillContractQuestionEngine.Verify(
                q.Family, q.Parameters, q.CorrectAnswer))
                continue;
            var p=q.Parameters;
            string[] keys = q.Family switch
            {
                "supporting.statistics.charts.mixed" or
                "supporting.statistics.table_total" => ["f1", "f2", "f3", "f4"],
                "supporting.statistics.category_total" => ["a", "b", "c"],
                "supporting.statistics.compare_category_counts" => ["a", "b"],
                "usccss.b01.graph.scaled_compare" => ["countA", "countB"],
                _ => []
            };
            if (keys.Length == 0 || keys.Any(k=>!p.ContainsKey(k)))
                continue;
            var values=keys.Select(k=>p[k]).ToArray();
            // Grade 3's official scaled graphs compare counts within 100.
            // Other elementary category-total questions use smaller counts.
            var limit = q.Family=="usccss.b01.graph.scaled_compare" ? 100 : 50;
            if (values.Any(x=>x<=0 || x>limit)) continue;
            var total=values.Sum();
            // Total-frequency question must agree with the actual bar heights.
            if (q.Family=="usccss.b01.graph.scaled_compare")
            {
                if (!p.ContainsKey("unit") || !p.ContainsKey("ticksA") ||
                    !p.ContainsKey("ticksB") ||
                    p["unit"] is not (2 or 5 or 10) ||
                    values[0]!=p["ticksA"]*p["unit"] ||
                    values[1]!=p["ticksB"]*p["unit"] ||
                    q.CorrectAnswer!=Math.Abs(values[0]-values[1]).ToString())
                    continue;
            }
            else if (q.Family == "supporting.statistics.compare_category_counts")
            {
                // This Practice family asks WHICH category has more (A/B/equal),
                // not the numeric difference. Independently check the source
                // question's categorical answer against the plotted counts.
                var expected=values[0]>values[1] ? "A" :
                    values[1]>values[0] ? "B" : "equal";
                if (!string.Equals(q.CorrectAnswer,expected,
                    StringComparison.Ordinal)) continue;
            }
            else if (!string.Equals(q.CorrectAnswer,total.ToString(),
                StringComparison.Ordinal)) continue;

            return [new RichLessonVisual
            {
                Kind=RichLessonVisualKind.BarGraph,
                Title="Compare categorical counts in a bar graph",
                Description=q.Family=="usccss.b01.graph.scaled_compare"
                    ? $"Each tick represents {p["unit"]} items. " +
                      $"Category A has {values[0]}, category B has {values[1]}, " +
                      $"and their difference is {q.CorrectAnswer}. " +
                      "Both bars share one linear scale."
                    : q.Family=="supporting.statistics.compare_category_counts"
                      ? $"Category A has {values[0]} and Category B has {values[1]} items. " +
                        (q.CorrectAnswer=="equal"
                            ? "The categories have equal frequencies. "
                            : $"Category {q.CorrectAnswer} has more items. ") +
                        "Both bars share one linear scale and can be compared directly."
                      : $"Each bar shows the exact recorded frequency. " +
                        $"The categories contain {string.Join(", ", values)} items " +
                        $"and the total is {total}. The longest bar represents " +
                        "the largest count; bar lengths share one common linear scale.",
                PrimaryLabel="Category and frequency",
                Rows=values.Select((count,i)=>new RichLessonVisualRow
                {
                    Label=$"Category {(char)('A'+i)}",
                    Cells=[count.ToString()]
                }).ToList()
            }];
        }
        return [];
    }
}

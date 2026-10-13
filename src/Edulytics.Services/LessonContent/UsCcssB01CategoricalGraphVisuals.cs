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
        "PED:US-CCSS-MATH:G2:U01:L13",
        "PED:US-CCSS-MATH:G2:U07:L18",
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
                _ => []
            };
            if (keys.Length == 0 || keys.Any(k=>!p.ContainsKey(k)))
                continue;
            var values=keys.Select(k=>p[k]).ToArray();
            if (values.Any(x=>x<=0 || x>50)) continue;
            var total=values.Sum();
            // Total-frequency question must agree with the actual bar heights.
            if (q.Family != "supporting.statistics.compare_category_counts" &&
                !string.Equals(q.CorrectAnswer, total.ToString(),
                    StringComparison.Ordinal))
                continue;

            return [new RichLessonVisual
            {
                Kind=RichLessonVisualKind.BarGraph,
                Title="Compare categorical counts in a bar graph",
                Description=$"Each bar shows the exact recorded frequency. " +
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

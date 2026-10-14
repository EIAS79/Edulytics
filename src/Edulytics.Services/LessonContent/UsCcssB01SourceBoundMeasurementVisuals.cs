using System.Globalization;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Publisher-source-scoped illustrations from VERIFIED learner operands.
/// G1 U06 L07: count physically contiguous equal units with no gaps.
/// G2 U09 L03: two adjacent measured map-path distances in centimeters.
/// These do not falsely represent the entire publisher activity or an L4/L6 PASS.
/// </summary>
public static class UsCcssB01SourceBoundMeasurementVisuals
{
    private const string Grade1UnitIteration = "PED:US-CCSS-MATH:G1:U06:L07";
    private const string Grade2MapLengths = "PED:US-CCSS-MATH:G2:U09:L03";

    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if (lessonCode != Grade1UnitIteration && lessonCode != Grade2MapLengths)
            return [];

        foreach (var q in questions)
        {
            if (!ExactSkillContractQuestionEngine.Verify(
                    q.Family,q.Parameters,q.CorrectAnswer))
                continue;

            var p = q.Parameters;
            if (lessonCode == Grade1UnitIteration &&
                q.Family == "supporting.measurement.unit_iteration" &&
                p.TryGetValue("units",out var units) &&
                units is >= 2 and <= 60 &&
                int.TryParse(q.CorrectAnswer,NumberStyles.Integer,
                    CultureInfo.InvariantCulture,out var result) &&
                result == units)
            {
                // One mark per exact source-unit tile; the endpoint is
                // never inferred from natural language or fabricated content.
                return
                [
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.Scale,
                        Title = "Measure with equal tiles, no gaps or overlaps",
                        Description = $"The same-sized unit tiles touch end-to-end. " +
                            $"There are exactly {units} equal units between 0 " +
                            $"and {units}. The object measures {units} unit tiles. " +
                            "Using differently sized units changes the count; " +
                            "this picture does not assert an unmeasured second count.",
                        PrimaryLabel = $"{units} equal unit tiles",
                        Items = Enumerable.Range(0,units+1)
                            .Select(i=>i.ToString(CultureInfo.InvariantCulture))
                            .ToList()
                    }
                ];
            }
            if (lessonCode == Grade2MapLengths &&
                q.Family == "usccss.b01.measure.map_lengths" &&
                p.TryGetValue("a",out var a) && p.TryGetValue("b",out var b) &&
                p.TryGetValue("mode",out var mode) &&
                a is >= 2 and <= 10 && b is >= 2 and <= 10 &&
                a+b <= 20 && mode is 0 or 1 &&
                int.TryParse(q.CorrectAnswer,NumberStyles.Integer,
                    CultureInfo.InvariantCulture,out var resultMap) &&
                resultMap == (mode == 0 ? a+b : b))
            {
                var total=a+b;
                return
                [
                    new RichLessonVisual
                    {
                        Kind = RichLessonVisualKind.Scale,
                        Title = "Measure adjacent map routes in centimeters",
                        Description = $"Route A measures {a} cm from tick 0 " +
                            $"to {a}. The adjoining route B measures {b} cm " +
                            $"from tick {a} to {total}. Each ruler interval " +
                            "is exactly 1 centimeter. " +
                            (mode==0
                              ? $"The combined map length is {a} + {b} = {resultMap} cm."
                              : $"The whole route is {total} cm and route A is {a} cm. " +
                                $"The remaining route is {total} − {a} = {resultMap} cm."),
                        PrimaryLabel = $"A: {a} cm; B: {b} cm",
                        SecondaryItems = ["0",a.ToString(CultureInfo.InvariantCulture),
                            total.ToString(CultureInfo.InvariantCulture)],
                        Items = Enumerable.Range(0,total+1)
                            .Select(i=>i.ToString(CultureInfo.InvariantCulture))
                            .ToList()
                    }
                ];
            }
        }
        return [];
    }
}

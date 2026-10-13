using System.Globalization;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Official IM Grade 2 Unit 4 number-line arithmetic activities.
/// Draws exact equal-distance ticks from real verified within-100
/// Practice operands. It does not pretend to certify all strategies
/// or equations with unknowns, which require separate L4/L5 evidence.
/// </summary>
public static class UsCcssB01SourceBoundNumberLineVisuals
{
    private static readonly HashSet<string> SourceLessons = new(
        Enumerable.Range(7,9).Select(x =>
            $"PED:US-CCSS-MATH:G2:U04:L{x:00}"),
        StringComparer.Ordinal);

    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if (!SourceLessons.Contains(lessonCode)) return [];
        foreach (var q in questions)
        {
            if (!q.Family.StartsWith("number.whole.add_subtract.",StringComparison.Ordinal))
                continue;
            var p=q.Parameters;
            if (!p.TryGetValue("left",out var left) ||
                !p.TryGetValue("right",out var right) ||
                !p.TryGetValue("operation",out var operation) ||
                !p.TryGetValue("expected",out var result) ||
                right<=0 || left<0 || left>100 || right>100 ||
                operation is not (0 or 1) ||
                result!=(operation==0?left+right:left-right) ||
                result<0 || result>100 ||
                !ExactSkillContractQuestionEngine.Verify(
                    q.Family,p,q.CorrectAnswer))
                continue;

            var first=Math.Min(left,result);
            var last=Math.Max(left,result);
            if (last-first!=right) continue;
            // All tick intervals have the same real numeric length.
            // A third tick is only included when within 0–100.
            var ticks = new List<int>{first,last};
            if (first>=right)
                ticks.Insert(0,first-right);
            else if (last+right<=100)
                ticks.Add(last+right);
            var operationSymbol=operation==0?"+":"−";
            return
            [
                new RichLessonVisual
                {
                    Kind=RichLessonVisualKind.NumberLine,
                    Title="Move right to add, left to subtract",
                    Description=
                        $"Start at {left}. Move {(operation==0?"right":"left")} " +
                        $"by {right} on the number line to arrive at {result}. " +
                        $"This shows {left} {operationSymbol} {right} = {result}. " +
                        $"Each displayed tick interval is exactly {right} units.",
                    Items=ticks.Select(x=>x.ToString(CultureInfo.InvariantCulture))
                        .ToList()
                }
            ];
        }
        return [];
    }
}

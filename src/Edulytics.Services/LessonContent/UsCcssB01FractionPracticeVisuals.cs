using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Exact B01 fraction bars / quarter-inch rulers from real verified question
/// parameters. Never converts unverified prose or arbitrary numerator/denominator
/// into a shaded picture; does not certify source completeness.
/// </summary>
public static class UsCcssB01FractionPracticeVisuals
{
    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode, IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if (!UsCcssB01SourceTaskVisuals.IsB01(lessonCode))
            return [];

        foreach (var q in questions)
        {
            // Never paint a fraction from unverified or tampered question data.
            if (!ExactSkillContractQuestionEngine.Verify(
                    q.Family, q.Parameters, q.CorrectAnswer))
                continue;
            var p = q.Parameters;
            bool Has(string key) => p.TryGetValue(key, out _);
            bool Complete(params string[] keys) => keys.All(Has);
            var pairs = new List<(string Label, int Numerator, int Denominator)>();
            var source = q.Family;

            if (source == "usccss.b01.measure.quarter_inch" &&
                Complete("quarters", "wholeInches") &&
                p["quarters"] is > 0 and < 4 &&
                p["wholeInches"] is >= 0 and <= 3 &&
                q.CorrectAnswer ==
                    $"{p["wholeInches"]*4 + p["quarters"]}/4")
            {
                var count = p["wholeInches"]*4 + p["quarters"];
                var ruler = new RichLessonVisual
                {
                    Kind = RichLessonVisualKind.NumberLine,
                    Title = "Ruler marked in quarters of an inch",
                    Description = $"{count} equal quarter-inch steps from zero " +
                        $"measure {q.CorrectAnswer} inches.",
                    Items = Enumerable.Range(0,count+1)
                        .Select(i => $"{i}/4").ToList()
                };
                var bar = FractionBar("The fractional part of an inch",
                    [("Quarter-inch portion",p["quarters"],4)]);
                return [ruler,bar];
            }

            if (source == "usccss.b01.fraction.unit_parts" &&
                Complete("denominator","shape"))
                pairs.Add(("Exactly one equal part",1,p["denominator"]));
            else if (source == "fractions.represent.interpret.fraction_bar" &&
                Complete("numerator","denominator"))
                pairs.Add(("Shaded amount",p["numerator"],p["denominator"]));
            else if (source.StartsWith("fractions.equivalent.",StringComparison.Ordinal))
            {
                if (Complete("baseN","baseD","targetD") &&
                    int.TryParse(q.CorrectAnswer,out var targetN) &&
                    p["baseN"]*p["targetD"] == targetN*p["baseD"])
                    pairs.Add(("Equivalent base fraction",p["baseN"],p["baseD"]));
                else if (Complete("largeN","largeD","targetD") &&
                    int.TryParse(q.CorrectAnswer,out var reducedN) &&
                    p["largeN"]*p["targetD"] == reducedN*p["largeD"])
                    pairs.Add(("Reduced equivalent fraction",reducedN,p["targetD"]));
            }
            else if ((source.StartsWith("fractions.compare.unlike.",StringComparison.Ordinal) ||
                source.StartsWith("fractions.add_subtract.",StringComparison.Ordinal) ||
                source == "supporting.fractions.multiply") &&
                Complete("n1","d1","n2","d2"))
            {
                pairs.Add(("First part",p["n1"],p["d1"]));
                pairs.Add(("Second part",p["n2"],p["d2"]));
            }
            else if (source == "supporting.fractions.divide_whole" &&
                Complete("n","d","whole") && p["whole"]>1)
                pairs.Add(("Fraction being divided",p["n"],p["d"]));

            if (pairs.Count == 0 ||
                pairs.Any(x => x.Denominator<2 || x.Denominator>12 ||
                     x.Numerator<1 || x.Numerator>x.Denominator))
                continue;

            return [FractionBar("Equal fractional parts in the exact example",pairs)];
        }
        return [];
    }

    private static RichLessonVisual FractionBar(
        string title, IReadOnlyList<(string Label,int Numerator,int Denominator)> values)
    {
        return new RichLessonVisual
        {
            Kind = RichLessonVisualKind.FractionBars,
            Title = title,
            Description = "Each bar represents one whole divided into " +
                "equal-sized parts. The fraction shaded is determined from " +
                "the original question's verified numerator and denominator.",
            Rows = values.Select(x => new RichLessonVisualRow
            {
                Label = $"{x.Label}: {x.Numerator}/{x.Denominator}",
                Numerator = x.Numerator,
                Denominator = x.Denominator
            }).ToList()
        };
    }
}

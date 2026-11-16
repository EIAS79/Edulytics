using System.Globalization;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class ScatterCorrelationFullSessionTests
{
    [Fact]
    public void Grade8StatisticsHasDiverseSelfConsistentTenQuestionSessions()
    {
        const string family = "supporting.statistics.scatter_correlation";
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));

        foreach (var seed in Enumerable.Range(1, 40))
        {
            var random = new Random(150719 + seed * 773);
            var generated = Enumerable.Range(0, 10)
                .Select(_ => SupportingPracticeCompletionEngine.Build(family, random, 2))
                .ToArray();
            Assert.Equal(10, generated.Select(x => x.Prompt).Distinct(StringComparer.Ordinal).Count());

            foreach (var problem in generated)
            {
                var p = problem.Parameters;
                var kind = p["kind"];
                Assert.InRange(kind, 0, 2);
                Assert.InRange(p["step"], 1, 8);
                Assert.InRange(p["xStep"], 1, 4);
                var expected = kind switch
                {
                    0 => "positive",
                    1 => "negative",
                    _ => "none"
                };
                Assert.Equal(expected, SupportingPracticeCompletionEngine.Solve(family, p));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, expected));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family, p, expected == "positive" ? "negative" : "positive"));
                Assert.Contains("(", problem.Prompt);
            }
        }
    }
}

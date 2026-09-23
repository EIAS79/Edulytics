using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeSemanticQuestionIdentityTests
{
    [Fact]
    public void WordingVariantDoesNotChangeSemanticIdentity()
    {
        const string family = "supporting.geometry.shape_dimension";

        var first = PracticeSemanticQuestionIdentityPolicy.Create(
            family,
            new Dictionary<string, int>
            {
                ["shape"] = 0,
                ["dimension"] = 2,
                ["variant"] = 0
            });

        var wordingOnlyVariant = PracticeSemanticQuestionIdentityPolicy.Create(
            family,
            new Dictionary<string, int>
            {
                ["shape"] = 0,
                ["dimension"] = 2,
                ["variant"] = 8
            });

        Assert.Equal(first.Key, wordingOnlyVariant.Key);
        Assert.DoesNotContain(
            "variant=",
            first.Key,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MathematicalSubjectChangeChangesSemanticIdentity()
    {
        const string family = "supporting.geometry.shape_dimension";

        var square = PracticeSemanticQuestionIdentityPolicy.Create(
            family,
            new Dictionary<string, int>
            {
                ["shape"] = 0,
                ["dimension"] = 2,
                ["variant"] = 0
            });

        var rectangle = PracticeSemanticQuestionIdentityPolicy.Create(
            family,
            new Dictionary<string, int>
            {
                ["shape"] = 1,
                ["dimension"] = 2,
                ["variant"] = 1
            });

        Assert.NotEqual(square.Key, rectangle.Key);
    }

    [Fact]
    public void EveryRuntimePracticeFamilyProducesDeterministicSemanticIdentity()
    {
        var families = LessonPracticeContractRegistry.All
            .SelectMany(contract => contract.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(family => family, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(families);

        var engine = new ExactSkillContractQuestionEngine();
        var failures = new List<string>();

        for (var index = 0; index < families.Length; index++)
        {
            var family = families[index];

            try
            {
                var first = Assert.Single(engine.Generate(
                    "semantic-identity-certification",
                    family,
                    [family],
                    ExactSkillQuestionDifficulty.Standard,
                    1,
                    620000 + index,
                    []));

                var second = Assert.Single(engine.Generate(
                    "semantic-identity-certification",
                    family,
                    [family],
                    ExactSkillQuestionDifficulty.Standard,
                    1,
                    620000 + index,
                    []));

                var firstIdentity =
                    PracticeSemanticQuestionIdentityPolicy.Create(
                        first.Family,
                        first.Parameters);
                var secondIdentity =
                    PracticeSemanticQuestionIdentityPolicy.Create(
                        second.Family,
                        second.Parameters);

                if (string.IsNullOrWhiteSpace(firstIdentity.Key))
                    failures.Add($"{family}: blank semantic identity");

                if (!string.Equals(
                        firstIdentity.Key,
                        secondIdentity.Key,
                        StringComparison.Ordinal))
                {
                    failures.Add($"{family}: semantic identity is not deterministic");
                }

                if (firstIdentity.Key.Contains(
                        "variant=",
                        StringComparison.OrdinalIgnoreCase))
                {
                    failures.Add($"{family}: synthetic variant leaked into semantic identity");
                }
            }
            catch (Exception ex)
            {
                failures.Add(
                    $"{family}: {ex.GetType().Name}: {ex.Message}");
            }
        }

        Assert.True(
            failures.Count == 0,
            "Semantic Practice identity certification failures: " +
            string.Join(" | ", failures.Take(100)) +
            (failures.Count > 100
                ? $" (+{failures.Count - 100} more)"
                : string.Empty));
    }
}

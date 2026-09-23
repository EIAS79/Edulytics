using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class PracticeAssessmentTaxonomyTests
{
    [Fact]
    public void EveryRuntimePracticeFamilyHasAtLeastOneQuestionFormCapability()
    {
        var families = LessonPracticeContractRegistry.All
            .SelectMany(contract => contract.AllowedQuestionFamilies)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(family => family, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(families);

        var failures = families
            .Where(family =>
                PracticeQuestionFormCapabilityRegistry
                    .Resolve(family)
                    .Count == 0)
            .ToArray();

        Assert.Empty(failures);
    }

    [Fact]
    public void ShapeDimensionDeclaresFourGenuineCognitiveFormsAcrossAllVariantSlots()
    {
        const string family = "supporting.geometry.shape_dimension";

        var capabilities =
            PracticeQuestionFormCapabilityRegistry.Resolve(family);

        Assert.Equal(4, capabilities.Count);
        Assert.Equal(
            new[]
            {
                PracticeQuestionForm.Identify,
                PracticeQuestionForm.Classify,
                PracticeQuestionForm.ErrorAnalysis,
                PracticeQuestionForm.Transfer
            },
            capabilities.Select(x => x.Form).ToArray());

        Assert.Equal(
            new[]
            {
                PracticeCognitiveOperation.Recall,
                PracticeCognitiveOperation.Understand,
                PracticeCognitiveOperation.Reason,
                PracticeCognitiveOperation.Transfer
            },
            capabilities
                .Select(x => x.CognitiveOperation)
                .ToArray());

        for (var slot = 0; slot < 16; slot++)
        {
            Assert.NotNull(
                PracticeQuestionFormCapabilityRegistry
                    .ResolveForVariant(
                        family,
                        slot));
        }
    }
}

using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Shared Practice must initialize successfully with the added UAE G1
/// exact-question metadata; this catches failures that UAE-only grading
/// tests miss while inspecting US curricula.
/// </summary>
public sealed class UaeGrade1ArabicSharedPracticeInteractionRegistryTests
{
    [Fact]
    public void EveryAddedGrade1Family_UsesSupportedSharedPracticeAnswerContracts()
    {
        var rows = LessonPracticeInteractionRegistry.All
            .Where(x => x.FamilyId.StartsWith("uae.g1.", StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(100, rows.Length);
        Assert.Equal(35, rows.Count(x =>
            x.AnswerType == "exact_integer" &&
            x.InteractionKind == LessonPracticeInteractionKind.IntegerKeypad));
        Assert.Equal(63, rows.Count(x =>
            x.AnswerType == "contract_defined" &&
            x.InteractionKind == LessonPracticeInteractionKind.ContractDriven));
        Assert.Equal(2, rows.Count(x =>
            x.AnswerType == "enum_text" &&
            x.InteractionKind == LessonPracticeInteractionKind.StructuredEntry));
        Assert.DoesNotContain(rows, x => x.AnswerType == "numeric");
    }
}

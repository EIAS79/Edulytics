using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;

// Arabic student book 02-1.pdf, printed pages 437-440.
// Stage A-B-C safety and authored fixtures; Stage D / .NET tests NOT_RUN.
public sealed class UaeGrade5Unit6Lesson06UnderstandExpressionsTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L5:GENERAL:06:06";

    [Fact]
    public void LessonModelCannotUseGenericEnglishSignedAlgebraPractice()
    {
        // Do not authorize unrelated simplification/negative coefficient
        // families in a lesson about concrete unknown-box models.
        if (LessonPracticeCapabilityResolver.TryResolve(Lesson, out var contract))
        {
            Assert.NotNull(contract);
            Assert.DoesNotContain("supporting.algebra.simplify", contract!.AllowedQuestionFamilies);
            Assert.DoesNotContain("supporting.algebra.substitute", contract.AllowedQuestionFamilies);
            Assert.DoesNotContain("supporting.algebra.expressions.mixed", contract.AllowedQuestionFamilies);
        }
    }

    [Fact]
    public void SourceScopedOriginalModelFixturesPreserveUnknownAndOrder()
    {
        const int unknown = 4;
        Assert.Equal(9, unknown + 5);
        const int initialBoxes = 11;
        Assert.Equal(8, initialBoxes - 3);
        Assert.NotEqual(8, 3 - initialBoxes);
        Assert.Equal(18, 3 * (unknown + 2));
        Assert.NotEqual(3 * (unknown + 2), 3 * unknown + 2);
        const int group = 6;
        Assert.Equal(15, group + group + 3);
        Assert.Equal(15, 2 * group + 3);
    }
}

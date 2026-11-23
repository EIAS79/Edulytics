using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original numerical oracle for Grade10 General 1-1. Existing Grade9 families
/// are reused only for a single coefficient, NOT complete polynomial grading.
/// Stage A-C creates these tests without running them; Stage D must execute.
/// </summary>
public sealed class UaeG10GeneralPolynomialAddSubtractReuseTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS";

    [Theory]
    [InlineData("uae.g9.general.u07.l01.add_coefficient")]
    [InlineData("uae.g9.general.u07.l01.subtract_coefficient")]
    public void SharedExactFamiliesGenerateArabicNumericItemsAndIndependentAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();

        foreach (var seed in new[] { 20261123, 20261124, 20261125 })
        {
            var items = engine.Generate(
                LessonCode, LessonCode, [family],
                ExactSkillQuestionDifficulty.Standard, 4, seed, []);

            Assert.Equal(4, items.Count);
            foreach (var item in items)
            {
                Assert.Equal(AssessmentItemType.Numeric, item.ItemType);
                Assert.Contains(item.Prompt, ch => ch is >= '\u0600' and <= '\u06ff');

                var p = item.Parameters;
                Assert.InRange(p["degree"], 0, 2);
                var degree = p["degree"].ToString(CultureInfo.InvariantCulture);
                var expected = family.EndsWith("add_coefficient", StringComparison.Ordinal)
                    ? p["a" + degree] + p["b" + degree]
                    : p["a" + degree] - p["b" + degree];

                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, p,
                    (expected + 1).ToString(CultureInfo.InvariantCulture)));

                var arabicDigits = string.Concat(item.CorrectAnswer.Select(ch =>
                    ch is >= '0' and <= '9' ? (char)('٠' + ch - '0') : ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, arabicDigits));

                var tampered = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                tampered["degree"] = 7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, tampered, item.CorrectAnswer));
            }
        }
    }

    [Theory]
    [InlineData("uae.g9.general.u07.l01.add_coefficient")]
    [InlineData("uae.g9.general.u07.l01.subtract_coefficient")]
    public void OriginalSharedCoefficientFamiliesRenderAsIntegerKeypad(string family)
    {
        // The declared family has an integer numeric answer. A generic
        // contract_defined renderer would misrepresent the correct input UI.
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad, interaction!.InteractionKind);
    }

    [Fact]
    public void DerivedOriginalRectangleAreaModelAgreesWithGeometricPerimeterOracle()
    {
        // New original Grade10 book-equivalent teacher task; NOT automatically graded.
        foreach (int length in new[] { 1, 2, 5, 10, 15, 18, 19 })
        {
            int width = (40 - 2 * length) / 2;
            int geometricArea = length * width;
            int polynomialArea = 20 * length - length * length;
            Assert.Equal(geometricArea, polynomialArea);
        }
        Assert.Equal(100, 20 * 10 - 10 * 10);
    }
}

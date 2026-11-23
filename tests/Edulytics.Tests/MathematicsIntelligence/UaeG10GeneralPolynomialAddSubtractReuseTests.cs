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
    public void Grade10WholePolynomialFamilyUsesSharedExactEngineAndIndependentVerifier()
    {
        const string family="uae.g10.general.u01.l01.full_polynomial_add_subtract_expression";
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,ui!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261123,20261124})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                           question.Parameters,question.CorrectAnswer));
                string ascii=question.CorrectAnswer.Replace("س","x").Replace("²","^2");
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,
                           question.Parameters,ascii));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                            question.Parameters,question.CorrectAnswer+"+1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,
                            question.Parameters,"x^3+1"));
                var corrupt=question.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                corrupt["subtract"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupt,question.CorrectAnswer));
            }
        }
    }


    [Theory]
    [InlineData("٢س²+٢س+٥", true)]
    [InlineData("2x^2+2x+5", true)]
    [InlineData("2س² + ٢س + ٥", true)]
    [InlineData("س²+٢س+٥", false)]
    [InlineData("2x^2+2x+6", false)]
    [InlineData("x^3+2x+5", false)]
    [InlineData("x²+سص+5", false)]
    [InlineData("2x^2+1/2x+5", false)]
    public void Grade10WholeExpressionVerifierChecksArabicAndWesternDigits(string submitted, bool expected)
    {
        const string family="uae.g10.general.u01.l01.full_polynomial_add_subtract_expression";
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["a2"]=3, ["a1"]=-4, ["a0"]=7,
            ["b2"]=-1,["b1"]=6,["b0"]=-2,
            ["subtract"]=0
        };
        Assert.Equal(expected,ExactSkillContractQuestionEngine.Verify(family,p,submitted));
    }

    [Fact]
    public void Grade10WholeExpressionHandlesSubtractionCancellationAndInvalidParameters()
    {
        const string family="uae.g10.general.u01.l01.full_polynomial_add_subtract_expression";
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["a2"]=3, ["a1"]=-4, ["a0"]=7,
            ["b2"]=-1,["b1"]=6,["b0"]=-2,
            ["subtract"]=1
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٤س²−١٠س+٩"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"4x^2-10x+9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"٤س²−١٠س−٩"));
        var cancelled=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["a2"]=2, ["a1"]=1, ["a0"]=-4,
            ["b2"]=-2,["b1"]=-1,["b0"]=4,
            ["subtract"]=0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,cancelled,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,cancelled,"س"));
        p["a2"]=100;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"٤س²−١٠س+٩"));
        p["a2"]=3;
        p["subtract"]=2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"٤س²−١٠س+٩"));
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

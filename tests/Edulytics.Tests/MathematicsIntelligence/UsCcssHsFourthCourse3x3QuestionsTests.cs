using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssHsFourthCourse3x3QuestionsTests
{
    private static readonly string[] Families =
    [
        UsCcssHsFourthCourse3x3Questions.Coefficients,
        UsCcssHsFourthCourse3x3Questions.Inverse,
        UsCcssHsFourthCourse3x3Questions.SolveSystem,
        UsCcssHsFourthCourse3x3Questions.RowReduce
    ];

    [Fact]
    public void Exact3x3FamiliesGenerateVariedSolvableMatricesAndIndependentlyVerify()
    {
        foreach (var family in Families)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (var seed = 1; seed <= 25; seed++)
            {
                var p = UsCcssHsFourthCourse3x3Questions.Build(family, new Random(seed));
                var answer = UsCcssHsFourthCourse3x3Questions.Solve(family, p.Parameters);
                Assert.True(UsCcssHsFourthCourse3x3Questions.Verify(family, p.Parameters, answer));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family, p.Parameters, answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, p.Parameters, "1+1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, p.Parameters, "1,2"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family, p.Parameters, new string('x', 300)));
                seen.Add(p.Prompt);
            }
            Assert.True(seen.Count >= 3, $"No adequate variation in {family}.");
        }
    }

    [Fact]
    public void CoefficientQuestionMustPresentEquationsNotTheRequestedMatrix()
    {
        for (var seed = 1; seed <= 30; seed++)
        {
            var item = UsCcssHsFourthCourse3x3Questions.Build(
                UsCcssHsFourthCourse3x3Questions.Coefficients, new Random(seed));
            Assert.DoesNotContain("A=[", item.Prompt);
            Assert.DoesNotContain("b=[", item.Prompt);
            Assert.Contains("variable order x,y,z", item.Prompt);
            Assert.Equal(3, item.Prompt.Count(c => c == '='));
            Assert.Contains("x", item.Prompt);
            Assert.Contains("y", item.Prompt);
            Assert.Contains("z", item.Prompt);
            Assert.True(UsCcssHsFourthCourse3x3Questions.Verify(
                UsCcssHsFourthCourse3x3Questions.Coefficients,
                item.Parameters,
                UsCcssHsFourthCourse3x3Questions.Solve(
                    UsCcssHsFourthCourse3x3Questions.Coefficients, item.Parameters)));
        }
    }

    [Fact]
    public void Coder8PublisherPaintingReceiptMatrixGradesExactAlgebraAndKeepsContextForTeacher()
    {
        // Coder 8's source-bound painting/brush/tape receipt scenario.
        var receipt = new Dictionary<string, int>
        {
            ["a00"] = 2, ["a01"] = 0, ["a02"] = 1,
            ["a10"] = 1, ["a11"] = 4, ["a12"] = 0,
            ["a20"] = 0, ["a21"] = 2, ["a22"] = 1,
            ["b0"] = 30, ["b1"] = 20, ["b2"] = 10
        };

        Assert.Equal("12,2,6", UsCcssHsFourthCourse3x3Questions.Solve(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, receipt));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.Coefficients, receipt,
            "2,0,1;1,4,0;0,2,1|30,20,10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.Coefficients, receipt,
            "2,1,0;1,4,0;0,2,1|30,20,10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.Coefficients, receipt,
            "2,0,1;1,4;0,2,1|30,20,10"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.Inverse, receipt,
            "4/10,2/10,-4/10;-1/10,2/10,1/10;2/10,-4/10,8/10"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, receipt,
            "24/2,4/2,12/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, receipt, "12,6,2"));

        // Changing only the third receipt to 50 makes a negative price:
        // algebraically valid but contextually invalid; teaching rationale is human-scored.
        receipt["b2"] = 50;
        Assert.Equal("-4,6,38", UsCcssHsFourthCourse3x3Questions.Solve(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, receipt));
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, receipt, "-4,6,38"));
    }

    [Fact]
    public void ExactInverseAndSolutionsAcceptEquivalentFractionsAndRejectWrongMatrices()
    {
        var a = new Dictionary<string, int>
        {
            ["a00"] = 1, ["a01"] = 1, ["a02"] = 1,
            ["a10"] = 2, ["a11"] = -1, ["a12"] = 1,
            ["a20"] = 1, ["a21"] = 2, ["a22"] = -1,
            ["b0"] = 6, ["b1"] = 3, ["b2"] = 2
        };
        Assert.Equal("1,2,3",
            UsCcssHsFourthCourse3x3Questions.Solve(UsCcssHsFourthCourse3x3Questions.RowReduce, a));
        Assert.True(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, a, "7/7,14/7,21/7"));
        Assert.False(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.SolveSystem, a, "1,3,2"));
        Assert.True(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.Inverse, a,
            "-1/7,3/7,2/7;3/7,-2/7,1/7;5/7,-1/7,-3/7"));
        Assert.False(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.Inverse, a,
            "1,0,0;0,1,0;0,0,1"));
        Assert.False(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.Inverse, a,
            "-1/7,3/7;3/7,-2/7"));
    }

    [Fact]
    public void SingularMatricesAreNotAccidentallyGradedAsInvertible()
    {
        var a = new Dictionary<string, int>
        {
            ["a00"] = 1, ["a01"] = 2, ["a02"] = 3,
            ["a10"] = 2, ["a11"] = 4, ["a12"] = 6,
            ["a20"] = 3, ["a21"] = 6, ["a22"] = 9,
            ["b0"] = 1, ["b1"] = 2, ["b2"] = 3
        };
        Assert.False(UsCcssHsFourthCourse3x3Questions.Verify(
            UsCcssHsFourthCourse3x3Questions.Inverse, a,
            "1,0,0;0,1,0;0,0,1"));
    }
}

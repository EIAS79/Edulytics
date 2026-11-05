using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlg1Unit2AlgebraQuestionsTests
{
    [Fact]
    public void ConstraintFeasibilityRequiresCapacityAndBudget()
    {
        var feasible = new Dictionary<string, int>
        {
            ["fixedCost"] = 8, ["itemCost"] = 14, ["capacity"] = 3,
            ["budget"] = 120, ["guests"] = 18, ["items"] = 6, ["variant"] = 0
        };
        Assert.Equal("yes", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.ConstraintFeasibilityFamily, feasible));

        var overBudget = new Dictionary<string, int>(feasible) { ["items"] = 9 };
        Assert.Equal("no", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.ConstraintFeasibilityFamily, overBudget));
    }

    [Fact]
    public void LinearRelationshipEquationUsesRateAndFixedAmount()
    {
        var p = new Dictionary<string, int>
        {
            ["rate"] = 7, ["fixedAmount"] = 95, ["variant"] = 0
        };
        Assert.Equal("y=7x+95", UsCcssAlg1Unit2AlgebraQuestions.Solve(
            UsCcssAlg1Unit2AlgebraQuestions.LinearRelationshipEquationFamily, p));
        Assert.True(UsCcssAlg1Unit2AlgebraQuestions.Verify(
            UsCcssAlg1Unit2AlgebraQuestions.LinearRelationshipEquationFamily, p, " y = 7*x + 95 "));
    }
}

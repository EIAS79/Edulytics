using System.Numerics;
using Edulytics.Core.Mathematics.Ast;
using Edulytics.Core.Mathematics.Difficulty;
using Edulytics.Core.Mathematics.Domains;
using Edulytics.Core.Mathematics.Planning;
using Edulytics.Services.Mathematics.Contracts;
using Edulytics.Services.Mathematics.Difficulty;
using Edulytics.Services.Mathematics.Generation;
using Edulytics.Services.Mathematics.Planning;
using Edulytics.Services.Mathematics.Verification;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class MathematicsReasoningIntelligenceTests
{
    [Fact]
    public void StrategyPlanner_SelectsDeterministicEliminationPlan_ForTwoByTwoSystem()
    {
        var x = new SymbolNode("x");
        var y = new SymbolNode("y");
        var problem = new EquationSystemNode([
            new EquationNode(new AddNode([x, y]), I(7)),
            new EquationNode(new AddNode([new MultiplyNode([I(2), x]), new NegateNode(y)]), I(5))
        ]);
        var planner = new DeterministicMathematicsStrategyPlanner();

        var plan = planner.Plan(new MathematicsPlanningRequest(problem, []));

        Assert.Equal(MathematicsPlanningStatus.Planned, plan.Status);
        Assert.Equal("systems.elimination.exact", plan.PrimaryStrategyId);
        Assert.Equal(2, plan.Candidates.Count);
        Assert.Equal(2, plan.Complexity.UnknownCount);
        Assert.True(plan.Complexity.ExpressionNodeCount > 0);
    }

    [Fact]
    public void StrategyPlanner_RecordsModellingBurden_ForMechanicsModels()
    {
        var problem = new FunctionCallNode("mechanics_power_exact", [
            new FunctionCallNode("quantity_si", [I(20), new SymbolNode("J")]),
            new FunctionCallNode("quantity_si", [I(4), new SymbolNode("s")])
        ]);
        var planner = new DeterministicMathematicsStrategyPlanner();

        var plan = planner.Plan(new MathematicsPlanningRequest(problem, []));

        Assert.Equal(MathematicsPlanningStatus.Planned, plan.Status);
        Assert.Equal("modelling.mechanics-model-units-solve-verify", plan.PrimaryStrategyId);
        Assert.True(plan.Complexity.ModellingBurden >= 3);
        Assert.True(plan.Complexity.RepresentationSwitchCount >= 1);
    }

    [Fact]
    public void StrategyPlanner_FailsClosed_WhenAstBudgetIsExceeded()
    {
        var problem = new AddNode([I(1), I(2), I(3), I(4)]);
        var planner = new DeterministicMathematicsStrategyPlanner();

        var plan = planner.Plan(new MathematicsPlanningRequest(problem, [], MaxAstNodes: 2));

        Assert.Equal(MathematicsPlanningStatus.ResourceLimit, plan.Status);
        Assert.Null(plan.PrimaryStrategyId);
    }

    [Fact]
    public void AnswerEquivalence_NormalizesExactRationalAndCommutativeExpressions()
    {
        var evaluator = new MathematicsAnswerEquivalenceV2();
        var half = evaluator.Evaluate(
            new DivideNode(I(1), I(2)),
            new RationalNode(new ExactRational(2, 4)),
            new MathematicsAnswerEvaluationPolicy());
        var symbolic = evaluator.Evaluate(
            new AddNode([new SymbolNode("x"), I(2)]),
            new AddNode([I(2), new SymbolNode("x")]),
            new MathematicsAnswerEvaluationPolicy());

        Assert.True(half.IsEquivalent);
        Assert.Equal("exact-rational-normalization", half.Method);
        Assert.True(symbolic.IsEquivalent);
        Assert.Equal("canonical-symbolic-structure", symbolic.Method);
    }

    [Fact]
    public void AnswerEquivalence_RecognizesUnorderedFiniteSolutionSets_ButPreservesVectorOrder()
    {
        var evaluator = new MathematicsAnswerEquivalenceV2();
        Assert.True(evaluator.AreEquivalentSolutionSets(
            new FiniteSolutionSet([I(2), I(1)]),
            new FiniteSolutionSet([I(1), I(2)])));

        var ordered = evaluator.Evaluate(
            new VectorNode([I(1), I(2)]),
            new VectorNode([I(2), I(1)]),
            new MathematicsAnswerEvaluationPolicy());
        Assert.False(ordered.IsEquivalent);
    }

    [Fact]
    public void MisconceptionEngine_ReturnsOnlyProvablyWrongDistinctDistractors()
    {
        var evaluator = new MathematicsAnswerEquivalenceV2();
        var engine = new ReviewedMisconceptionEngine(evaluator);
        var correct = new RationalNode(new ExactRational(3, 4));

        var distractors = engine.Generate(new EquationNode(new SymbolNode("x"), correct), correct, 3);

        Assert.True(distractors.Count >= 2);
        Assert.Equal(distractors.Count, distractors.Select(x => x.MisconceptionId).Distinct(StringComparer.Ordinal).Count());
        Assert.All(distractors, distractor =>
            Assert.False(evaluator.Evaluate(distractor.Answer, correct, new MathematicsAnswerEvaluationPolicy()).IsEquivalent));
    }

    [Fact]
    public void MisconceptionEngine_PreservesMechanicsUnit_WhenMutatingScalar()
    {
        var engine = new ReviewedMisconceptionEngine();
        var correct = new FunctionCallNode("quantity_si", [I(5), new SymbolNode("N")]);

        var distractors = engine.Generate(new FunctionCallNode("mechanics_newton_second_law_force_exact", []), correct, 2);

        Assert.NotEmpty(distractors);
        Assert.All(distractors, distractor =>
        {
            var quantity = Assert.IsType<FunctionCallNode>(distractor.Answer);
            Assert.Equal("quantity_si", quantity.FunctionName);
            Assert.Equal(new SymbolNode("N"), quantity.Arguments[1]);
        });
    }

    [Fact]
    public void DifficultyEngine_ProjectsComplexityAndAdaptiveStateDeterministically()
    {
        var engine = new MathematicsDifficultyEngine();
        var plan = new MathematicsStrategyPlan(
            MathematicsPlanningStatus.Planned,
            "test",
            [new MathematicsStrategyCandidate("test", "test", 5, 1, [])],
            new MathematicsComplexityVector(
                ExpressionNodeCount: 30,
                ExpressionDepth: 6,
                RequiredTransformationCount: 5,
                RequiredStrategyCount: 2,
                BranchCount: 2,
                UnknownCount: 2,
                RepresentationSwitchCount: 1,
                ProofBurden: 2),
            []);

        var assessment = engine.Assess(plan);
        var low = engine.Recommend(new MathematicsAdaptiveLearnerState(0.2m, 0.4m, 0.3m, 2, 1));
        var high = engine.Recommend(new MathematicsAdaptiveLearnerState(0.95m, 0.9m, 0.9m, 0, 8));

        Assert.Equal(MathematicsDifficultyBand.Challenging, assessment.Band);
        Assert.True(assessment.ComplexityScore > 55);
        Assert.Equal(MathematicsDifficultyBand.Easy, low.RecommendedBand);
        Assert.Equal(MathematicsDifficultyBand.Challenging, high.RecommendedBand);
        Assert.True(high.TargetComplexityScore > low.TargetComplexityScore);
    }

    private static IntegerNode I(int value) => new(new BigInteger(value));
}

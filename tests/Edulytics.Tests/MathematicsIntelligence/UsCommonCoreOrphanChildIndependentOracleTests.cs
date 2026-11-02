using System.Globalization;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// An independently written arithmetic/geometry oracle for 15 source-orphan
/// Common Core CHILD STANDARD candidates. The oracle never calls the engine's
/// Solve method or derives the expected answer from CorrectAnswer. Numerical
/// agreement is engineering evidence only; missing publisher evidence and
/// independent academic signoffs are NOT resolved by this test.
/// </summary>
public sealed class UsCommonCoreOrphanChildIndependentOracleTests
{
    private static readonly (string Clause, string Family)[] Families =
    [
        ("3.MD.C.7.a", "usccss.g3.rectangle_unit_tiles"),
        ("5.MD.C.3.a", "usccss.g5.unit_cube_count"),
        ("6.SP.B.5.a", "usccss.g6.observation_count"),
        ("HSF-BF.A.1.c", "usccss.hs.function_composition"),
        ("HSF-BF.B.4.b", "usccss.hs.inverse_composition"),
        ("HSF-BF.B.4.c", "usccss.hs.inverse_table"),
        ("HSF-BF.B.4.d", "usccss.hs.restricted_inverse"),
        ("HSF-IF.C.7.d", "usccss.hs.rational_hole_or_pole"),
        ("HSN-VM.B.4.a", "usccss.vector.add_x"),
        ("HSN-VM.B.4.b", "usccss.vector.resultant_angle"),
        ("HSN-VM.B.4.c", "usccss.vector.subtract_y"),
        ("HSN-VM.B.5.a", "usccss.vector.scalar_y"),
        ("HSN-VM.B.5.b", "usccss.vector.scalar_magnitude"),
        ("HSS-MD.B.5.a", "usccss.probability.expected_net_payoff"),
        ("HSS-MD.B.5.b", "usccss.probability.compare_expected_cost")
    ];

    private static long IndependentAnswer(
        string family, IReadOnlyDictionary<string, int> p)
    {
        int Get(string key) => p[key];

        return family switch
        {
            "usccss.g3.rectangle_unit_tiles" =>
                checked((long)Get("w") * Get("h")),

            "usccss.g5.unit_cube_count" =>
                checked((long)Get("l") * Get("w") * Get("h")),

            "usccss.g6.observation_count" =>
                checked((long)Get("a") + Get("b") + Get("c")),

            "usccss.hs.function_composition" =>
                checked((long)Get("a") * ((long)Get("c") * Get("x") + Get("d")) +
                    Get("b")),

            "usccss.hs.inverse_composition" =>
                IndependentInverseComposition(p),

            "usccss.hs.inverse_table" =>
                IndependentInverseTable(p),

            "usccss.hs.restricted_inverse" =>
                IndependentRestrictedInverse(p),

            "usccss.hs.rational_hole_or_pole" =>
                IndependentDiscontinuity(p),

            "usccss.vector.add_x" =>
                checked((long)Get("ax") + Get("bx")),

            "usccss.vector.resultant_angle" =>
                IndependentResultantAngle(p),

            "usccss.vector.subtract_y" =>
                checked((long)Get("ay") - Get("by")),

            "usccss.vector.scalar_y" =>
                checked((long)Get("k") * Get("y")),

            "usccss.vector.scalar_magnitude" =>
                IndependentScalarMagnitude(p),

            "usccss.probability.expected_net_payoff" =>
                IndependentExpectedNet(p),

            "usccss.probability.compare_expected_cost" =>
                IndependentLowerExpectedCost(p),

            _ => throw new InvalidOperationException("Unrecognized orphan family " + family)
        };
    }

    private static long IndependentInverseComposition(IReadOnlyDictionary<string, int> p)
    {
        var a = p["a"];
        Assert.NotEqual(0, a);
        var x = p["x"];
        var forwardValue = checked((long)a * x + p["b"]);
        var inverseNumerator = forwardValue - p["b"];
        Assert.Equal(0L, inverseNumerator % a);
        return inverseNumerator / a;
    }

    private static long IndependentInverseTable(IReadOnlyDictionary<string, int> p)
    {
        var a = p["a"];
        var b = p["b"];
        var y = p["y"];
        Assert.NotEqual(0, a);
        var recovered = (long)y - b;
        Assert.Equal(0L, recovered % a);
        var result = recovered / a;
        Assert.Equal((long)y, checked(a * result + b));
        Assert.Equal(p["x"], result);
        return result;
    }

    private static long IndependentRestrictedInverse(IReadOnlyDictionary<string, int> p)
    {
        var t = p["t"];
        var value = p["value"];
        Assert.True(t > 0);
        Assert.Equal((long)value, (long)t * t);
        var root = (long)Math.Sqrt(value);
        Assert.Equal((long)value, root * root);
        var x = (long)p["vertex"] + root;
        Assert.True(x >= p["vertex"]);
        Assert.Equal((long)value, (x - p["vertex"]) * (x - p["vertex"]));
        return x;
    }

    private static long IndependentDiscontinuity(IReadOnlyDictionary<string, int> p)
    {
        var removed = p["removed"];
        var pole = p["pole"];
        var root = p["root"];
        var ask = p["ask"];
        Assert.NotEqual(removed, pole);
        Assert.NotEqual(root, pole);
        Assert.True(ask == removed || ask == pole);
        // At 'removed', only the canceled common factor causes the exclusion.
        // At 'pole', the simplified denominator remains zero.
        return ask == removed ? 1 : 2;
    }

    private static long IndependentResultantAngle(IReadOnlyDictionary<string, int> p)
    {
        var x = (long)p["ux"] + p["vx"];
        var y = (long)p["uy"] + p["vy"];
        Assert.True(x > 0 && y > 0, "Sampler promises a first-quadrant resultant.");
        // Deliberately use a positive-quadrant slope and arctan, not the engine's
        // atan2 expression. Half-up rounding matches the printed instruction.
        var degrees = Math.Atan((double)y / x) * (180d / Math.PI);
        Assert.True(degrees > 0 && degrees < 90);
        return (long)Math.Floor(degrees + 0.5d);
    }

    private static long IndependentScalarMagnitude(IReadOnlyDictionary<string, int> p)
    {
        var f = p["factor"];
        var k = p["k"];
        Assert.True(f > 0 && k != 0);
        var vx = checked(3L * f * k);
        var vy = checked(4L * f * k);
        var squared = checked(vx * vx + vy * vy);
        var magnitude = (long)Math.Sqrt(squared);
        Assert.Equal(squared, magnitude * magnitude);
        return magnitude;
    }

    private static long IndependentExpectedNet(IReadOnlyDictionary<string, int> p)
    {
        var denominator = p["q"];
        var prize = p["prize"];
        Assert.True(denominator > 1 && prize >= 0);
        Assert.Equal(0, prize % denominator);
        // Multiply all possible net outcomes by their probabilities:
        // winning (prize-ticket)/q and losing -(q-1)*ticket/q.
        var total = (long)prize - (long)denominator * p["ticket"];
        Assert.Equal(0L, total % denominator);
        return total / denominator;
    }

    private static long IndependentLowerExpectedCost(IReadOnlyDictionary<string, int> p)
    {
        var q = p["q"];
        var extra = p["extra"];
        Assert.True(q > 1);
        Assert.Equal(0, extra % q);
        var planBExpectedTotalNumerator = (long)q * p["baseCost"] + extra;
        var planAExpectedTotalNumerator = (long)q * p["fixedCost"];
        return planAExpectedTotalNumerator < planBExpectedTotalNumerator ? 1 :
            planAExpectedTotalNumerator > planBExpectedTotalNumerator ? 2 : 0;
    }

    [Fact]
    public void AllFifteenOrphanCandidates_HaveIndependentParameterOracles()
    {
        Assert.Equal(15, Families.Length);
        Assert.Equal(15, Families.Select(x => x.Family)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.Equal(15, Families.Select(x => x.Clause)
            .Distinct(StringComparer.Ordinal).Count());

        var engine = new ExactSkillContractQuestionEngine();
        var total = 0;
        foreach (var (clause, family) in Families)
        {
            Assert.StartsWith("usccss.", family, StringComparison.Ordinal);
            foreach (var difficulty in new[]
                     {
                         ExactSkillQuestionDifficulty.Standard,
                         ExactSkillQuestionDifficulty.Stretch,
                         ExactSkillQuestionDifficulty.Challenge
                     })
            {
                for (var seed = 1; seed <= 20; seed++)
                {
                    var item = Assert.Single(engine.Generate(
                        "us-ccss-unreleased-independent-oracle",
                        "orphan-clause/" + clause,
                        [family],
                        difficulty,
                        1,
                        710000 + total * 100 + seed,
                        []));

                    Assert.Equal(family, item.Family);
                    var independentlyDerived = IndependentAnswer(family, item.Parameters);
                    var answer = independentlyDerived.ToString(CultureInfo.InvariantCulture);
                    Assert.Equal(answer, item.CorrectAnswer);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, answer),
                        "Valid independent answer rejected for " + clause + "/" + family);

                    var wrong = checked(independentlyDerived + 1L)
                        .ToString(CultureInfo.InvariantCulture);
                    Assert.NotEqual(answer, wrong);
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family, item.Parameters, wrong),
                        "Off-by-one answer unexpectedly accepted for " + clause + "/" + family);

                    total++;
                }
            }
        }

        Assert.Equal(900, total);
    }
}

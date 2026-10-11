using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Context-rich fixed-response checks; these do not purport to grade freehand
/// graphs, drawn vector arrows or open-ended written mathematical arguments.
/// </summary>
public sealed class UsCommonCoreContextualGraphReasoningTests
{
    private static readonly (string Family, string Lesson)[] Cases =
    [
        ("usccss.hs.rational_hole_or_pole", "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17"),
        ("usccss.hs.rational_branch_limit_sign", "PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L17"),
        ("usccss.vector.head_tail_endpoint_y", "PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01"),
        ("usccss.vector.scalar_direction_class", "PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01"),
        ("usccss.probability.expected_vs_worst_case", "PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05")
    ];

    [Fact]
    public void CorrectContextualFamiliesGenerateAndVerifyWithIndependentMath()
    {
        var engine = new ExactSkillContractQuestionEngine();
        int count = 0, hole=0, pole=0, positive=0, negative=0,
            same=0, reverse=0, zero=0, expected=0, worst=0;

        foreach (var (family, lesson) in Cases)
        {
            foreach (var difficulty in new[]
            {
                ExactSkillQuestionDifficulty.Standard,
                ExactSkillQuestionDifficulty.Stretch,
                ExactSkillQuestionDifficulty.Challenge
            })
            {
                for (int seed = 0; seed < 24; seed++)
                {
                    var q = Assert.Single(engine.Generate("usccss-visual-context",
                        lesson, [family], difficulty, 1, 10500000+count, []));
                    Assert.Equal(family,q.Family);
                    Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
                    var p = q.Parameters;
                    var expectedAnswer = family switch
                    {
                        "usccss.hs.rational_hole_or_pole" =>
                            p["ask"]==p["removed"] ? 1 : 2,
                        "usccss.hs.rational_branch_limit_sign" =>
                            (p["numerator"]>0)==(p["right"]==1) ? 1 : 2,
                        "usccss.vector.head_tail_endpoint_y" =>
                            p["startY"]+p["ay"]+p["by"],
                        "usccss.vector.scalar_direction_class" =>
                            p["k"]>0 ? 1 : p["k"]<0 ? 2 : 0,
                        "usccss.probability.expected_vs_worst_case" =>
                            p["objective"]==0 ? 2 : 1,
                        _ => throw new InvalidOperationException(family)
                    };
                    Assert.Equal(expectedAnswer.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);

                    if (family=="usccss.hs.rational_hole_or_pole")
                    {
                        Assert.NotEqual(p["removed"],p["pole"]);
                        if (expectedAnswer==1) hole++; else pole++;
                    }
                    if (family=="usccss.hs.rational_branch_limit_sign")
                    {
                        if (expectedAnswer==1) positive++; else negative++;
                    }
                    if (family=="usccss.vector.scalar_direction_class")
                    {
                        if (expectedAnswer==1) same++;
                        else if (expectedAnswer==2) reverse++;
                        else zero++;
                    }
                    if (family=="usccss.probability.expected_vs_worst_case")
                    {
                        var avgB=p["baseCost"]+p["extra"]/p["denom"];
                        var worstB=p["baseCost"]+p["extra"];
                        Assert.True(avgB<p["fixedCost"]);
                        Assert.True(p["fixedCost"]<worstB);
                        if (p["objective"]==0) expected++; else worst++;
                    }
                    count++;
                }
            }
        }

        Assert.Equal(5*3*24,count);
        Assert.True(hole>0 && pole>0);
        Assert.True(positive>0 && negative>0);
        Assert.True(same>0 && reverse>0 && zero>0);
        Assert.True(expected>0 && worst>0);
    }
}

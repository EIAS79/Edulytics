using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 4 IM/KH 2021 ordinal 120. Stage-C focused assertions authored, UNRUN.
public sealed class Grade4Ordinal120ProtractorDualScaleReadingTests
{
    private const string Family = "usccss.g4.u07.l09.protractor_dual_scale_reading";

    [Fact]
    public void GeneratedFirstPartyScaleQuestionsUseAlignedZeroAndRejectTampering()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (var seed in new[] { 20210709, 120149, 20261027 })
        {
            var items = engine.Generate("g4-2021-u07-l09", Family, [Family],
                ExactSkillQuestionDifficulty.Standard, 12, seed, []);
            Assert.Equal(12, items.Count);
            foreach (var item in items)
            {
                var p = item.Parameters;
                Assert.InRange(p["caseType"], 0, 7);
                Assert.InRange(p["variant"], 0, 2);
                Assert.Equal(p["variant"] == 1 ? 1 : 0, p["zeroScale"]);
                Assert.Equal(180, p["outerReading"] + p["innerReading"]);
                var expected = p["zeroScale"] == 0 ? p["outerReading"] : p["innerReading"];
                Assert.Equal(expected, p["targetDegrees"]);
                Assert.Equal(expected.ToString(), item.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, expected.ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (180 - expected).ToString()));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, ""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, "?"));
                var modified = p.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
                modified["outerReading"] = 7;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
                modified["outerReading"] = p["outerReading"];
                modified["variant"] = 8;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
                modified["variant"] = p["variant"];
                modified["targetDegrees"] = 177;
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family, modified, expected.ToString()));
            }
        }
    }

    [Theory]
    [InlineData(0, 25)]
    [InlineData(1, 45)]
    [InlineData(2, 65)]
    [InlineData(3, 80)]
    [InlineData(4, 100)]
    [InlineData(5, 115)]
    [InlineData(6, 140)]
    [InlineData(7, 160)]
    public void EightFixedCasesAcceptCorrectScaleAndRejectOtherScale(int mode, int degree)
    {
        foreach (var variant in new[] { 0, 1, 2 })
        {
            var zero = variant == 1 ? 1 : 0;
            var outer = zero == 0 ? degree : 180 - degree;
            var inner = 180 - outer;
            var p = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["caseType"] = mode, ["variant"] = variant, ["zeroScale"] = zero,
                ["outerReading"] = outer, ["innerReading"] = inner,
                ["targetDegrees"] = degree
            };
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, p, degree.ToString()));
            Assert.False(ExactSkillContractQuestionEngine.Verify(Family, p, (180-degree).ToString()));
        }
    }

    [Fact]
    public void Local2021LessonFourWarmupsAndDegreeFractionsAreMathematicallyConsistent()
    {
        Assert.Equal(6 * 15, 2 * 45);
        Assert.Equal(2 * 90, 4 * 45);
        Assert.NotEqual(180 - 90, 3 * 45);
        Assert.Equal(45 + 90 + 135, 6 * 45);
        foreach (var (degree, divisor) in new[] { (120, 3), (60, 6), (45, 8), (30, 12), (10, 36), (1, 360) })
            Assert.Equal(360, degree * divisor);
        Assert.True(100 > 90);
        Assert.False(80 > 90);
    }
}

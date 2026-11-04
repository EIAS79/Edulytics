using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>G7 U01:L04 scoped mathematical oracles. A-C AUTHORING NOT_RUN.</summary>
public sealed class UsCcssG7Unit1Lesson4SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:G7:U01:L04";
    private const string Dist = "usccss.g7.u01.l04.hidden_corresponding_distance";
    private const string Angles = "usccss.g7.u01.l04.angle_and_distance_scaling";
    private const string Reuse = "usccss.g7.u01.l01.scaled_copy_correspondence";

    [Fact]
    public void ExactFamiliesAreLessonScoped_AndReuseExistingPolygonKernelFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var c));
        Assert.NotNull(c);
        Assert.Equal("READY_VERIFIED", c.Readiness);
        Assert.Equal(new[] { Dist, Angles, Reuse }, c.AllowedQuestionFamilies);
        Assert.All(c.AllowedQuestionFamilies,
            f => Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)));
        Assert.True(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:G7:U01:L03", out _));
    }

    private static Dictionary<string,int> D(int f, int hidden, int reverse) =>
        new() { ["factor"]=f, ["hidden"]=hidden, ["reverse"]=reverse };

    [Theory]
    [InlineData(0, 2, 0, "8")]
    [InlineData(1, 2, 0, "10")]
    [InlineData(2, 2, 0, "12")]
    [InlineData(1, 5, 0, "25")]
    [InlineData(1, 5, 1, "10")]
    [InlineData(0, 2, 1, "4")]
    [InlineData(2, 2, 1, "4")]
    public void AllCorrespondingDistancesEvenHidden_AreScaledWithDirection(
        int factor, int hidden, int reverse, string answer)
    {
        var p = D(factor, hidden, reverse);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Dist, p, answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Dist, p, (decimal.Parse(answer)+1m).ToString(
                System.Globalization.CultureInfo.InvariantCulture)));
        // The shared numeric submission layer is permitted to normalize harmless whitespace.
    }

    [Theory]
    [InlineData(0,true)]  // scale 2, preserved rectangular angles & distances
    [InlineData(1,false)] // equal right angles, horizontal ratio 2, vertical 1
    [InlineData(2,false)] // equal right angles, horizontal ratio 1, vertical 2
    [InlineData(3,false)] // shear changes matching angles, diagonal
    [InlineData(4,true)]  // rotated/reflected half-size
    public void AnglesAloneDoNotProveScaling(int variant, bool expected)
    {
        var p = new Dictionary<string,int> { ["variant"]=variant };
        Assert.True(ExactSkillContractQuestionEngine.Verify(
            Angles, p, expected ? "1" : "0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Angles, p, expected ? "0" : "1"));
    }

    [Fact]
    public void BadParametersAndForeignAnswersFailClosed()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Dist, D(3,2,0), "8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Dist, D(1,7,0), "35"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Dist, D(1,2,2), "10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            Angles, new Dictionary<string,int>{["variant"]=5}, "1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(
            "usccss.g7.u01.l05.not_authorized",D(0,2,0),"1"));
    }

    [Fact]
    public void GeneratedSelectedFamiliesHaveDeterministicCorrectAnswer()
    {
        var engine = new ExactSkillContractQuestionEngine();
        foreach(var family in new[] { Dist, Angles })
        {
            var items = engine.Generate("g7-u01-l04", Lesson, [family],
                ExactSkillQuestionDifficulty.Standard, 3, 704,
                Array.Empty<string>());
            Assert.Equal(3, items.Count);
            foreach(var item in items)
            {
                Assert.Equal(family,item.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family,item.Parameters,item.CorrectAnswer));
                Assert.DoesNotContain("CorrectAnswer",item.Prompt);
            }
        }
    }
}

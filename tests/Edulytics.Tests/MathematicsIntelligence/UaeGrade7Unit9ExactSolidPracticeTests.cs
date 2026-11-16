using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Regression guard: every Arabic G7 Unit 9 lesson must assess its own
/// cylinder/cone/sphere geometry or dimensional scaling, not a prerequisite.
/// The expected values are recomputed here independently of the runtime Solve.
/// </summary>
public sealed class UaeGrade7Unit9ExactSolidPracticeTests
{
    [Theory]
    [InlineData("09:01:CYLINDER-VOLUME", "uae.g7.u09.l01.cylinder_volume_pi_coefficient")]
    [InlineData("09:02:CONE-VOLUME", "uae.g7.u09.l02.cone_volume_pi_coefficient")]
    [InlineData("09:03:SPHERE-VOLUME", "uae.g7.u09.l03.sphere_volume_pi_coefficient")]
    [InlineData("09:04:CYLINDER-SURFACE-AREA", "uae.g7.u09.l04.cylinder_surface_area_pi_coefficient")]
    [InlineData("09:05:CONE-SURFACE-AREA", "uae.g7.u09.l05.cone_surface_area_pi_coefficient")]
    [InlineData("09:06:SCALE-DIMENSIONS-AREA-VOLUME", "uae.g7.u09.l06.scale_area_factor")]
    [InlineData("09:06:SCALE-DIMENSIONS-AREA-VOLUME", "uae.g7.u09.l06.scale_volume_factor")]
    public void Unit9PracticeFamiliesAreExactLessonAlignedAndRejectIncorrectAnswers(
        string lessonSuffix, string family)
    {
        var code = "PED:UAE-MOE-MATH:L7:GENERAL:" + lessonSuffix;
        Assert.True(LessonPracticeContractRegistry.TryResolve(code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Contains(family, contract.AllowedQuestionFamilies);
        Assert.All(contract.AllowedQuestionFamilies,
            item => Assert.StartsWith("uae.g7.u09.", item, StringComparison.Ordinal));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));

        foreach (var seed in Enumerable.Range(1, 32))
        {
            var generated = SupportingPracticeCompletionEngine.Build(
                family, new Random(seed * 7919), 2);
            Assert.Equal(family, generated.Family);
            Assert.Contains(generated.Prompt, ch => ch is >= '\u0600' and <= '\u06FF');
            var p = generated.Parameters;
            var expected = family switch
            {
                "uae.g7.u09.l01.cylinder_volume_pi_coefficient" =>
                    p["radius"] * p["radius"] * p["height"],
                "uae.g7.u09.l02.cone_volume_pi_coefficient" =>
                    p["radius"] * p["radius"] * p["height"] / 3,
                "uae.g7.u09.l03.sphere_volume_pi_coefficient" =>
                    4 * p["radius"] * p["radius"] * p["radius"] / 3,
                "uae.g7.u09.l04.cylinder_surface_area_pi_coefficient" =>
                    2 * p["radius"] * (p["radius"] + p["height"]),
                "uae.g7.u09.l05.cone_surface_area_pi_coefficient" =>
                    p["radius"] * (p["radius"] + p["slantHeight"]),
                "uae.g7.u09.l06.scale_area_factor" =>
                    p["scale"] * p["scale"],
                "uae.g7.u09.l06.scale_volume_factor" =>
                    p["scale"] * p["scale"] * p["scale"],
                _ => throw new InvalidOperationException("Unknown lesson family")
            };
            var answer = expected.ToString(CultureInfo.InvariantCulture);
            Assert.Equal(answer, SupportingPracticeCompletionEngine.Solve(family, p));
            Assert.True(SupportingPracticeCompletionEngine.Verify(family, p, answer));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, answer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family, p, (expected + 1).ToString(CultureInfo.InvariantCulture)));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "NaN"));
            if (family == "uae.g7.u09.l02.cone_volume_pi_coefficient")
                Assert.Equal(0, p["height"] % 3);
            if (family == "uae.g7.u09.l03.sphere_volume_pi_coefficient")
                Assert.Equal(0, p["radius"] % 3);
            if (family == "uae.g7.u09.l05.cone_surface_area_pi_coefficient")
                Assert.True(p["slantHeight"] > p["radius"]);
        }
    }
}

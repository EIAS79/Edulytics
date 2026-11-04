using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L10 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson10BoundaryAreaTests{
[Fact]public void ContractReusesCircumferenceAndAreaFamilies(){Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L10",out var c));Assert.NotNull(c);foreach(var f in new[]{"supporting.geometry.circle_circumference_pi_coefficient","supporting.geometry.circle_area_pi_coefficient","usccss.g7.u03.l02.radius_diameter_relation"}){Assert.Contains(f,c.AllowedQuestionFamilies);Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));}Assert.Equal("READY_VERIFIED",c.Readiness);}}

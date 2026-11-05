using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L07 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson7CircleAreaTests{
[Fact]public void ContractReusesCircleAreaAndProportionalityFamilies(){Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L07",out var c));Assert.NotNull(c);foreach(var f in new[]{"supporting.geometry.circle_area_pi_coefficient","usccss.g7.u03.l02.radius_diameter_relation","usccss.g7.u02.l07.table_proportional_or_not"}){Assert.Contains(f,c.AllowedQuestionFamilies);Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));}Assert.Equal("READY_VERIFIED",c.Readiness);}}

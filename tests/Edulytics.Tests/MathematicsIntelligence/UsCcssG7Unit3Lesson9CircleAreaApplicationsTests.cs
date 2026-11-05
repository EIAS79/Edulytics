using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L09 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson9CircleAreaApplicationsTests{
const string F="usccss.g7.u03.l09.circle_fraction_area_pi_coefficient";
[Fact]public void ContractUsesPartialCircleExtension(){Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L09",out var c));Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));Assert.Contains("supporting.geometry.circle_area_pi_coefficient",c.AllowedQuestionFamilies);Assert.Equal("READY_VERIFIED",c.Readiness);}}

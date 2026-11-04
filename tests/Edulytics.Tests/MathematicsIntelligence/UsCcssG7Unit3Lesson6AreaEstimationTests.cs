using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L06 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson6AreaEstimationTests{
[Fact]public void ContractReusesAreaFamilies(){Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L06",out var c));Assert.NotNull(c);foreach(var f in new[]{"supporting.geometry.compound_area","supporting.geometry.triangle_area","supporting.geometry.parallelogram_area"}){Assert.Contains(f,c.AllowedQuestionFamilies);Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));}Assert.Equal("READY_VERIFIED",c.Readiness);}}

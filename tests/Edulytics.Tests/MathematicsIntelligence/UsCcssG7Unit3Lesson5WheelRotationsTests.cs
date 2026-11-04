using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L05 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson5WheelRotationsTests{
const string F="usccss.g7.u03.l05.wheel_rotations_distance";
[Fact]public void ContractUsesSupportingEngineExtension(){Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L05",out var c));Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));Assert.Equal("READY_VERIFIED",c.Readiness);}
}

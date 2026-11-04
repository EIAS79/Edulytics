using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U09 L11 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit9Lesson11ContractTests
{
    [Fact] public void ContractResolvesPrimaryReusedFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U09:L11",out var c));
        Assert.NotNull(c);Assert.Contains("supporting.geometry.circle_circumference_pi_coefficient",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("supporting.geometry.circle_circumference_pi_coefficient"));
    }
}

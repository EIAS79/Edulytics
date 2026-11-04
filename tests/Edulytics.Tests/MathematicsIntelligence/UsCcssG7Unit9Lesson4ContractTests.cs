using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U09 L04 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit9Lesson4ContractTests
{
    [Fact] public void ContractResolvesPrimaryReusedFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U09:L04",out var c));
        Assert.NotNull(c);Assert.Contains("usccss.g7.u01.l02.scale_factor",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g7.u01.l02.scale_factor"));
    }
}

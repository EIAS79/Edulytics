using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U04 L04 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit4Lesson4FractionalChangeTests
{
    const string F="usccss.g7.u04.fractional_change";
    [Fact] public void ContractUsesSupportingExtension()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U04:L04",out var c));
        Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
    }
}

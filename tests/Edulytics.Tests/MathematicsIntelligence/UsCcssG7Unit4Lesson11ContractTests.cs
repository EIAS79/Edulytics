using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U04 L11 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit4Lesson11ContractTests
{
    [Fact] public void ContractResolvesPrimaryExactFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U04:L11",out var c));
        Assert.NotNull(c);
        Assert.Contains("usccss.g7.u04.reverse_percent_original",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g7.u04.reverse_percent_original"));
    }
}

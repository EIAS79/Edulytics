using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U09 L03 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit9Lesson3ContractTests
{
    [Fact] public void ContractResolvesPrimaryReusedFamily()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U09:L03",out var c));
        Assert.NotNull(c);Assert.Contains("usccss.g7.u02.l08.classify_equations",c.AllowedQuestionFamilies);
        Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("usccss.g7.u02.l08.classify_equations"));
    }
}

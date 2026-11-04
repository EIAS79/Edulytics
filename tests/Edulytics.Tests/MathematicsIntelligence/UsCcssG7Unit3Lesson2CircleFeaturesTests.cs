using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L02 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson2CircleFeaturesTests
{
    const string F="usccss.g7.u03.l02.radius_diameter_relation";
    [Fact] public void ContractUsesSupportingEngineExtension()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L02",out var c));
        Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
    }
}

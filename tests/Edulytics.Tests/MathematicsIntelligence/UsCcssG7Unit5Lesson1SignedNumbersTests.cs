using Edulytics.Core.Mathematics.Practice;using Edulytics.Services.Mathematics;using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U05 L01 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit5Lesson1SignedNumbersTests
{
    const string F="usccss.g7.u05.compare_signed_rational";
    [Fact] public void ContractUsesSharedSignedRationalExtension()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U05:L01",out var c));
        Assert.NotNull(c);Assert.Contains(F,c.AllowedQuestionFamilies);Assert.Equal("READY_VERIFIED",c.Readiness);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
    }
}

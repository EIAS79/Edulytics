using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G7 U03 L01 A-C authoring fixture; intentionally not executed during A-C.</summary>
public sealed class UsCcssG7Unit3Lesson1MeasurementTests
{
    [Fact] public void ContractReusesExistingClosedFamilies()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G7:U03:L01",out var c));
        Assert.NotNull(c);
        foreach(var f in new[]{"usccss.g7.u02.l07.table_proportional_or_not","usccss.g7.u02.l10.graphs_origin_slope","supporting.geometry.parallelogram_area"})
        { Assert.Contains(f,c.AllowedQuestionFamilies); Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f)); }
        Assert.Equal("READY_VERIFIED",c.Readiness);
    }
}

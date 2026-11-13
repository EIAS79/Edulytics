using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Grade 5 Arabic Unit11 L06 pp.821–826. Authored, Stage D NOT_RUN.
public sealed class UaeGrade5Unit11Lesson06PairedDataTests
{
    [Theory]
    [InlineData("uae.g5.u11.l06.scatter_midpoint_interpolation")]
    [InlineData("uae.g5.u11.l06.scatter_linear_extrapolation")]
    public void PairedDataGraphHasIndependentNumericOracle(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var cases=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:06",
            family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var item in cases)
        {
            int a=item.Parameters["a"],b=item.Parameters["b"];
            int oracle=family.EndsWith("scatter_midpoint_interpolation",StringComparison.Ordinal)?a+b:a+3*b;
            Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.False(item.Parameters.ContainsKey("result"));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"99999"));
        }
    }
}

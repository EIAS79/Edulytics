using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// Stage C reusable family and runtime-contract regression; Stage D not run.
public sealed class UaeGrade5Unit11Lesson05LineAnalysisRoutingTests
{
    [Theory]
    [InlineData("uae.g5.u11.l02.line_plot_read_point")]
    [InlineData("uae.g5.u11.l02.line_plot_total_change")]
    [InlineData("uae.g5.u11.l02.line_plot_max_value")]
    [InlineData("uae.g5.u11.l04.linear_next_prediction")]
    [InlineData("uae.g5.u11.l04.linear_future_prediction")]
    public void AllReusedFamiliesHaveExactRuntimeContract(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:05",
            family,[family],ExactSkillQuestionDifficulty.Standard,4,20261113,[]);
        foreach(var item in items)
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"not-an-integer"));
        }
    }
}

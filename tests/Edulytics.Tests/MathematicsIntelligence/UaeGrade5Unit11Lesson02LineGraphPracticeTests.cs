using System.Globalization;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

// Source-aligned Stage C tests authored only; Stage D NOT_RUN.
public sealed class UaeGrade5Unit11Lesson02LineGraphPracticeTests
{
    [Theory]
    [InlineData("uae.g5.u11.l02.line_plot_read_point")]
    [InlineData("uae.g5.u11.l02.line_plot_total_change")]
    [InlineData("uae.g5.u11.l02.line_plot_max_value")]
    public void ExactLinePlotReadsValuesAndRejectsWrongAnswers(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.NotNull(ui);
        var engine=new ExactSkillContractQuestionEngine();
        var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:02",family,[family],
            ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
        foreach(var item in questions)
        {
            int a=item.Parameters["a"],b=item.Parameters["b"],c=item.Parameters["c"],
                d=item.Parameters["d"],e=item.Parameters["e"];
            int oracle=family.EndsWith("line_plot_read_point",StringComparison.Ordinal)?c:
                family.EndsWith("line_plot_total_change",StringComparison.Ordinal)?Math.Abs(e-a):
                Math.Max(Math.Max(Math.Max(a,b),Math.Max(c,d)),e);
            Assert.Equal(oracle.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
            Assert.False(item.Parameters.ContainsKey("result"));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"9999"));
        }
    }
}

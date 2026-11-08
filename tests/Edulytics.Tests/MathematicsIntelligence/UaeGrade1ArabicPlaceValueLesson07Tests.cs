using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

// WRITTEN_NOT_RUN during A–B–C. Stage D should execute fixed-seed tests.
public sealed class UaeGrade1ArabicPlaceValueLesson07Tests
{
    [Fact]
    public void SourceScopedFamily_UsesExactMathGradingWithoutEnglishFallback()
    {
        const string code="PED:UAE-MOE-MATH:L1:COMMON:06:07";
        const string family="uae.g1.u06.l07.make_table";
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var contract));
        Assert.Equal(family,Assert.Single(contract!.AllowedQuestionFamilies));
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine=new ExactSkillContractQuestionEngine();
        var items=engine.Generate("stage18",code,new[]{family},
            ExactSkillQuestionDifficulty.Standard,12,52007,Array.Empty<string>());
        Assert.NotEmpty(items);
        Assert.All(items,item=>
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(
                family,item.Parameters,"999"));
            Assert.InRange(item.Parameters["n"],0,120);
        });
    }
}

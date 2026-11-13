using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// 2025–2026 source student pp99–104, Stage D tests NOT_RUN.
public sealed class UaeGrade5Unit2Lesson04MultiplicationPatternsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:02:04";
    [Fact] public void ExactMultiplicationPatternFamiliesAreReadyForStageD()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("supporting.indices.roots_standard_form",c!.SkillId);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }
    [Fact] public void SourcePatternProductsAndMissingExponentAreExact()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l04.multiply_by_power_ten",new Dictionary<string,int>{{"factor",13},{"pow",2},{"mode",0}},"1300"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l04.product_with_trailing_zeros",new Dictionary<string,int>{{"a",4},{"b",7},{"za",1},{"zb",3},{"mode",1}},"280000"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u02.l04.find_missing_power_ten",new Dictionary<string,int>{{"factor",40},{"product",4000},{"mode",2}},"2"));
    }
}

using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit3Lesson13CenterDay2QuestionsTests
{
    [Fact]
    public void FourMeasurementAndPuzzleFamiliesHaveNumericStudentPresentation()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L13",out var c));
        Assert.NotNull(c);Assert.Equal(4,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
        }
    }
    [Fact]
    public void GeneratedRulerAndMissingEquationParametersAreSelfChecking()
    {
        var engine=new ExactSkillContractQuestionEngine();
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(
            "PED:US-CCSS-MATH:G2:U03:L13",out var c));
        foreach(var f in c!.AllowedQuestionFamilies)
        foreach(var seed in new[]{20261022,20261023})
        {
            var qs=engine.Generate("im2021-g2-u03-l13",f,[f],
                ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,qs.Count);
            foreach(var q in qs)
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                var modified=new Dictionary<string,int>(q.Parameters,StringComparer.Ordinal);
                if(modified.ContainsKey("target"))modified["target"]=50;
                else if(modified.ContainsKey("total"))modified["total"]=999;
                else modified["start"]=999;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,modified,q.CorrectAnswer));
            }
        }
    }
}

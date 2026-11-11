using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson03SubtractionModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:03";
    [Fact]
    public void SubtractionModelsGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(8,7,3,"4","3")]
    [InlineData(6,4,3,"1","3")]
    [InlineData(12,9,3,"6","3")]
    public void SubtractsAndRecoversRemovedParts(int d,int total,int removed,string remaining,string removedResult)
    {
        var p=new Dictionary<string,int>{{"denominator",d},{"totalParts",total},{"removedParts",removed}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l03.model_remaining_parts",p,remaining));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l03.model_removed_parts",p,removedResult));
        Assert.Equal(total,int.Parse(remaining)+int.Parse(removedResult));
    }
    [Fact]
    public void CannotSubtractMorePartsThanAvailable()
    {
        var p=new Dictionary<string,int>{{"denominator",8},{"totalParts",4},{"removedParts",5}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l03.model_remaining_parts",p,"-1"));
    }
    [Fact]
    public void RichArabicLessonIncludesSubtractionModelsAndChecks()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: استخدام النماذج في طرح الكسور المتشابهة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

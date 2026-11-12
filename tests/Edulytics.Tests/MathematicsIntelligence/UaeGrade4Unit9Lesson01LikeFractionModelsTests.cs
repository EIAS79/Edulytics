using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson01LikeFractionModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:01";
    [Fact]
    public void ModelsGenerateAndVerifyLikeDenominatorOperations()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(8,3,2,"5","2")]
    [InlineData(6,1,4,"5","4")]
    [InlineData(12,3,4,"7","4")]
    public void AdditionAndDecompositionUseSameSizedParts(int d,int a,int b,string sum,string second)
    {
        var p=new Dictionary<string,int>{{"denominator",d},{"a",a},{"b",b}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l01.model_add_like_denominators",p,sum));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l01.model_decompose_fraction",p,second));
    }
    [Fact]
    public void RejectsPartsThatExceedWholeModel()
    {
        var p=new Dictionary<string,int>{{"denominator",6},{"a",4},{"b",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l01.model_add_like_denominators",p,"7"));
    }
    [Fact]
    public void AcceptsArabicIntegerAnswer()
    {
        var p=new Dictionary<string,int>{{"denominator",8},{"a",3},{"b",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l01.model_add_like_denominators",p,"٥"));
    }
    [Fact]
    public void ArabicRichSourceContainsFractionStripExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: استخدام النماذج في جمع الكسور المتشابهة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

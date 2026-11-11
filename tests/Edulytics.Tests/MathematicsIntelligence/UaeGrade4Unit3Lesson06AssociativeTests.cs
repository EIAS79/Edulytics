using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson06AssociativeTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:06";
    [Fact]
    public void AllThreeAssociativeFamiliesRouteGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(3,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                bad["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void AssociativityAndUnknownFactorAreIndependent()
    {
        var p=new Dictionary<string,int>{{"a",9},{"b",2},{"c",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l06.triple_product",p,"٧٢"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l06.grouped_product",p,"72"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l06.missing_factor",p,"٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l06.triple_product",p,"36"));
    }
    [Fact]
    public void ArabicStudentLessonHasWorkedAssociativeModels()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("خاصية التجميع في الضرب",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(rich.Visuals).Kind);
    }
}

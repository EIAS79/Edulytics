using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson05AdditionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:05";
    [Fact]
    public void MissingBookLessonIsInBlueprintAndCanonicalRegistry()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("جمع الأعداد الكلية",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.PlaceValueChart,Assert.Single(rich.Visuals).Kind);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
    }
    [Fact]
    public void AllExactAdditionFamiliesGenerateAndVerifyWithoutAnswerInParameters()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract!.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-99"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentAdditionFixturesAreExact()
    {
        var p=new Dictionary<string,int>{{"a",6824},{"b",1349}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l05.add_four_digits",p,"٨٬١٧٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l05.missing_addend",p,"1349"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l05.add_four_digits",p,"8174"));
        p["a"]=58713;p["b"]=43827;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l05.add_six_digits",p,"102540"));
        p["a"]=158713;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l05.add_six_digits",p,"٢٠٢٬٥٤٠"));
    }
}

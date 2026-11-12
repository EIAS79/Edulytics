using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson02PatternsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:02";
    [Fact]
    public void FourExactNumberPatternFamiliesAreAvailable()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],
                        ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                altered["step"]=-3;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentPatternValuesAndDigits()
    {
        var p=new Dictionary<string,int>{{"start",78245},{"step",1000}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l02.add_place_step",p,"79245"));
        p["start"]=278245;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l02.add_place_step",p,"٢٧٩٬٢٤٥"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l02.subtract_place_step",p,"277245"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l02.find_missing_step",p,"1000"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l02.continue_pattern",p,"281245"));
    }
    [Fact]
    public void ArabicRichVisualAndExamplesPresent()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("أنماط الجمع والطرح",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.NumberLine,Assert.Single(l.Visuals).Kind);
    }
}

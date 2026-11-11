using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson01PropertiesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:01";
    [Fact]
    public void AllFiveBookPropertiesAreRoutedAndVerified()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(5,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var item in engine.Generate(Lesson,family,[family],
                     ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"-7"));
                Assert.Equal(3,item.Parameters.Count);
                Assert.DoesNotContain("result",item.Parameters.Keys);
                var altered=item.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                altered["a"]=100;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,item.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentRulesAndArabicDigitsBehaveCorrectly()
    {
        var p=new Dictionary<string,int>{{"a",44},{"b",18},{"c",22}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.commutative_add",p,"٤٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.associative_add",p,"٨٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.additive_identity",p,"44"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.subtract_zero",p,"44"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.subtract_self",p,"٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l01.subtract_self",p,"44"));
    }
    [Fact]
    public void StudentArabicLessonHasExplanationsExercisesAndVisual()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("خواص الجمع وقواعد الطرح",l!.Title);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.CalculationChain,Assert.Single(l.Visuals).Kind);
        Assert.Contains(l.CommonMistakes,x=>x.Mistake.Contains("الطرح"));
    }
}

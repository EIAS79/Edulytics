using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson03MentalTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:03";
    [Fact]
    public void FourMentalArithmeticFamiliesRouteAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-42"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var corrupted=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                corrupted["a"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,corrupted,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void MentalAdditionAndSubtractionReferenceFixtures()
    {
        var p=new Dictionary<string,int>{{"a",499},{"b",99}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l03.compensate_addition",p,"٥٩٨"));
        p["a"]=184;p["b"]=76;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l03.break_apart_subtraction",p,"108"));
        p["a"]=284;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l03.break_apart_subtraction",p,"208"));
        p["a"]=82000;p["b"]=76000;
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l03.subtract_thousands",p,"6000"));
        p["a"]=182000;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l03.subtract_thousands",p,"١٠٦٬٠٠٠"));
    }
    [Fact]
    public void StudentArabicLessonHasStrategyExamplesAndOriginalDiagram()
    {
        var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(lesson);
        Assert.Equal("الجمع والطرح الذهني",lesson!.Title);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.CalculationChain,Assert.Single(lesson.Visuals).Kind);
        Assert.Contains(lesson.CommonMistakes,x=>x.Mistake.Contains("تعويض"));
    }
}

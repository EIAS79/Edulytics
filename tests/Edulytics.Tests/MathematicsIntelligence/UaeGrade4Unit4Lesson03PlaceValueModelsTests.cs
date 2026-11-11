using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson03PlaceValueModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:03";
    [Fact]
    public void ThreePlaceValueModelFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-15"));
                Assert.Equal(3,q.Parameters.Count);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["multiplier"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void TensAndOnesRecombineIntoProduct()
    {
        var p=new Dictionary<string,int>{{"multiplier",2},{"tens",3},{"ones",2}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"٦٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_tens_product",p,"60"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_ones_product",p,"٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"62"));
    }
    [Fact]
    public void ArabicRichStudentLessonIncludesBlocks()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("القيمة المكانية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

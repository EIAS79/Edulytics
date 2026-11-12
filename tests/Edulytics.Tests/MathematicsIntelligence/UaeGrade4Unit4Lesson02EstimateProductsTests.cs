using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson02EstimateProductsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:02";
    [Fact]
    public void EstimateQuestionFamiliesAreRoutedAndVerified()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                bad["factor"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void EstimatesCanBeAboveOrBelowProducts()
    {
        var p=new Dictionary<string,int>{{"factor",8},{"number",2496}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l02.estimate_rounded_product",p,"١٦٬٠٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l02.estimate_above_below",p,"٠"));
        p["number"]=8562;p["factor"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l02.estimate_rounded_product",p,"27000"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l02.estimate_above_below",p,"1"));
    }
    [Fact]
    public void StudentLessonRichArabicHasEstimationExplanations()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("التقريب لتقدير ناتج الضرب",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(rich.Visuals).Kind);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit5Lesson03AreaDistributiveTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:05:03";
    [Fact]
    public void ReusedExtendedEngineSupportsAreaProducts()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
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
    [Fact]
    public void PartialRectanglesCombineToCorrectProduct()
    {
        var p=new Dictionary<string,int>{{"first",36},{"second",24}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_two_digit_product",p,"٨٦٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_tens_partial",p,"٧٢٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_ones_partial",p,"١٤٤"));
        p["first"]=12;p["second"]=15;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_two_digit_product",p,"١٨٠"));
    }
    [Fact]
    public void ArabicRichLessonShowsAreaModelExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: استخدام خاصية التوزيع في الضرب",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

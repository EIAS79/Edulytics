using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit5Lesson04ColumnMultiplicationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:05:04";
    [Fact]
    public void ColumnMultiplicationReusesAllAreaFamilies()
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
    [Fact]
    public void TextbookVerticalProductsAgreeWithPartialProducts()
    {
        var p=new Dictionary<string,int>{{"first",56},{"second",29}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_two_digit_product",p,"١٦٢٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_tens_partial",p,"١١٢٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_ones_partial",p,"٥٠٤"));
        p["first"]=35;p["second"]=24;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u05.l03.area_two_digit_product",p,"٨٤٠"));
    }
    [Fact]
    public void ArabicContentReflectsVerticalAlgorithm()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب في عدد مكون من رقمين",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

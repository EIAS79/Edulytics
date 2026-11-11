using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson05ColumnMultiplicationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:05";
    [Fact]
    public void NewlyAppendedLessonReusesExistingVerifiedFamilies()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(3,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void ColumnAndPartialProductsAgreeForStudentExample()
    {
        var p=new Dictionary<string,int>{{"multiplier",2},{"tens",2},{"ones",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"٤٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_tens_product",p,"40"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_ones_product",p,"٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l03.model_two_digit_product",p,"44"));
    }
    [Fact]
    public void ArabicLessonAddedWithOriginalExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب في عدد مكون من رقمين",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson02EstimateDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:02";
    [Fact]
    public void CompatibleNumberFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void StudentBookCompatibleNumberEstimatesAreValid()
    {
        var p=new Dictionary<string,int>{{"dividend",642},{"divisor",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l02.estimate_compatible_quotient",p,"٨٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l02.compatible_dividend",p,"٦٤٠"));
        p["dividend"]=1168;p["divisor"]=6;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l02.estimate_compatible_quotient",p,"٢٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l02.compatible_dividend",p,"١٢٠٠"));
    }
    [Fact]
    public void ArabicRichHasCompatibleDivision()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("تقدير نواتج القسمة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

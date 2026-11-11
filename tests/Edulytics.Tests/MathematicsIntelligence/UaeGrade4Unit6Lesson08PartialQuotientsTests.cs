using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson08PartialQuotientsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:08";
    [Fact]
    public void ReusedDivisionFamiliesVerifyGeneratedProblems()
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
            }
        }
    }
    [Fact]
    public void StudentBookPartialQuotientExamplesWork()
    {
        var p=new Dictionary<string,int>{{"dividend",375},{"divisor",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"٧٥"));
        p["dividend"]=762;p["divisor"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"٢٥٤"));
        p["dividend"]=426;p["divisor"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"٢١٣"));
    }
    [Fact]
    public void ArabicRichContentDescribesDistribution()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("نواتج القسمة الجزئية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

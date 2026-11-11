using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson03BaseTenDivisionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:03";
    [Fact]
    public void QuotientAndRemainderFamiliesGenerateCorrectly()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-2"));
                Assert.Equal(2,q.Parameters.Count);
            }
        }
    }
    [Fact]
    public void BaseTenQuotientAndRemainderAgreeWithOriginalExamples()
    {
        var p=new Dictionary<string,int>{{"dividend",39},{"divisor",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"١٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"٠"));
        p["dividend"]=68;p["divisor"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"١٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"٣"));
        p["dividend"]=43;p["divisor"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"١٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"١"));
    }
    [Fact]
    public void RichArabicBaseTenExamplesAreAvailable()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("القيمة المكانية للقسمة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

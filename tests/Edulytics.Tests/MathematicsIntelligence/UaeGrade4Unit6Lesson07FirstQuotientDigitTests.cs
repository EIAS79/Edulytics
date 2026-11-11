using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit6Lesson07FirstQuotientDigitTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:06:07";
    [Fact]
    public void ReusedExpandedDivisionFamiliesValidate()
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
            }
        }
    }
    [Fact]
    public void ThreeDigitDivisionAndFirstDigitPlacementAreCorrect()
    {
        var p=new Dictionary<string,int>{{"dividend",125},{"divisor",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"٣١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"١"));
        p["dividend"]=145;p["divisor"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_quotient",p,"٤٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u06.l03.base_ten_remainder",p,"١"));
    }
    [Fact]
    public void ArabicRichContentPreservesFirstDigitSkill()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("وضع الرقم الأول",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

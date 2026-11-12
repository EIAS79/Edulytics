using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson11ZerosTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:11";
    [Fact]
    public void ExistingMultidigitFamiliesAreReusedAndValidated()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Fact]
    public void ZerosInPlaceValueDoNotAlterPlacePosition()
    {
        var p=new Dictionary<string,int>{{"factor",6},{"number",108}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.multidigit_product",p,"٦٤٨"));
        p["factor"]=3;p["number"]=2025;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.multidigit_product",p,"٦٠٧٥"));
        p["factor"]=9;p["number"]=9002;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l09.multidigit_product",p,"٨١٠١٨"));
    }
    [Fact]
    public void ArabicRichLessonPreservesZeroSkill()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب مع وجود الأصفار",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}

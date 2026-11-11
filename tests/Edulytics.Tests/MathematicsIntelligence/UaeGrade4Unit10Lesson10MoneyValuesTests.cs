using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson10MoneyValuesTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:10";
 [Fact] public void MoneyFamiliesGenerateVerifyAndRejectWrongAnswers()
 {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
    Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var f in c.AllowedQuestionFamilies)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
        Assert.NotNull(ui);
        foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261112,[]))
        {
            Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
        }
    }
 }
 [Theory]
 [InlineData(2,1,0,0,"60")]
 [InlineData(0,0,0,0,"0")]
 [InlineData(1,2,1,3,"53")]
 public void CalculatesCoinValues(int c25,int c10,int c5,int c1,string expected)
 {
   var p=new Dictionary<string,int>{{"a25",c25},{"a10",c10},{"a5",c5},{"a1",c1},{"b25",0},{"b10",0},{"b5",0},{"b1",0}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l10.coin_total_fils",p,expected));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l10.coin_wallet_difference",p,expected));
 }
 [Fact] public void InvalidMoneyInputsAreRejected()
 {
    var p=new Dictionary<string,int>{{"a25",-1}};
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l10.coin_total_fils",p,"0"));
 }
 [Fact] public void RichArabicLessonExplainsCurrency()
 {
    var l=RichLessonContentV2Registry.Find(Lesson,"ar");
    Assert.NotNull(l);
    Assert.Equal("قيمة العملات المعدنية والأوراق النقدية",l!.Title);
    Assert.True(l.WorkedExamples.Count>=4);
    Assert.True(l.KeyConcepts.Count>=3);
    Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

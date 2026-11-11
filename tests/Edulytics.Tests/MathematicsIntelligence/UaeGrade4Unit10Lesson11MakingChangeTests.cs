using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson11MakingChangeTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:11";
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
 [InlineData(500,77,"423","23")]
 [InlineData(500,345,"155","55")]
 [InlineData(200,95,"105","5")]
 public void CalculatesChangeAndNextWholeDirham(int paid,int cost,string change,string next)
 {
   var p=new Dictionary<string,int>{{"paidFils",paid},{"costFils",cost}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l11.change_fils",p,change));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l11.count_up_to_next_dirham",p,next));
 }
 [Fact] public void InvalidMoneyInputsAreRejected()
 {
    var p=new Dictionary<string,int>{{"paidFils",100},{"costFils",120}};
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l11.change_fils",p,"-20"));
 }
 [Fact] public void RichArabicLessonExplainsCurrency()
 {
    var l=RichLessonContentV2Registry.Find(Lesson,"ar");
    Assert.NotNull(l);
    Assert.Equal("تحديد الباقي",l!.Title);
    Assert.True(l.WorkedExamples.Count>=4);
    Assert.True(l.KeyConcepts.Count>=3);
    Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

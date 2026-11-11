using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit11Lesson03EstimateOrExactTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:11:03";
 [Fact] public void EstimationFamiliesGenerateAndVerify()
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
 [InlineData(850,3,"27","2550")]
 [InlineData(460,2,"10","920")]
 [InlineData(125,4,"4","500")]
 public void ComparesEstimatedAndExactTicketCosts(int price,int count,string approx,string exact)
 {
    var p=new Dictionary<string,int>{{"priceFils",price},{"count",count}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l03.approximate_ticket_cost",p,approx));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l03.exact_ticket_cost_fils",p,exact));
 }
 [Fact] public void InvalidInputsAreRejected()
 {
   var p=new Dictionary<string,int>{{"priceFils",100},{"count",0}};
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l03.exact_ticket_cost_fils",p,"0"));
 }
 [Fact] public void ArabicLessonHasWorkedExamples()
 {
   var l=RichLessonContentV2Registry.Find(Lesson,"ar");
   Assert.NotNull(l);
   Assert.Equal("استقصاء حل المسائل: التقدير أم الإجابة الدقيقة",l!.Title);
   Assert.True(l.WorkedExamples.Count>=4);
   Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

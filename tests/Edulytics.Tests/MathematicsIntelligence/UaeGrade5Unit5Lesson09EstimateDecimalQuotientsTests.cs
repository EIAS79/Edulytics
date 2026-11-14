using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// G5 selected Arabic pp353–358. Stage D tests authored NOT_RUN.
public sealed class UaeGrade5Unit5Lesson09EstimateDecimalQuotientsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:09";
 [Fact] public void EstimationFamiliesUseSharedExactEngine()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("number.estimate_quotient_compatible_numbers",contract!.SkillId);
  Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var f in contract.AllowedQuestionFamilies)
  {
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
   Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
   Assert.NotNull(ui);
   foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
  }
 }
 [Fact] public void CompatibleDecimalCasesMatchSourceExamples()
 {
  var p=new Dictionary<string,int>{{"dividendHundredths",1518},{"divisor",2}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l09.estimate_decimal_quotient",p,"8"));
  p["dividendHundredths"]=3257;p["divisor"]=3;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l09.estimate_decimal_quotient",p,"11"));
  p["dividendHundredths"]=7874;p["divisor"]=42;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l09.estimate_decimal_quotient",p,"2"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l09.estimate_decimal_quotient",p,"3"));
  p["divisor"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l09.estimate_decimal_quotient",p,"2"));
 }
}
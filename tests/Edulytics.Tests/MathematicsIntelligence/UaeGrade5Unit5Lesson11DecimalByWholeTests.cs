using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit5Lesson11DecimalByWholeTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:11";
 [Fact] public void ThreeExactAndRoundedFamiliesRoute()
 {
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
  Assert.Equal("number.division_model_word_problem",contract!.SkillId);
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
 [Fact] public void ExactThousandthVsRoundedHundredthAreDifferent()
 {
  var p=new Dictionary<string,int>{{"dividendHundredths",654},{"divisor",12}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l11.exact_decimal_by_whole",p,"٠٫٥٤٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l11.exact_decimal_by_whole",p,"0.55"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l11.round_decimal_quotient_hundredth",p,"0.55"));
  p["dividendHundredths"]=2211;p["divisor"]=11;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l11.exact_decimal_by_whole",p,"2.01"));
  p["divisor"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l11.exact_decimal_by_whole",p,"0"));
 }
}
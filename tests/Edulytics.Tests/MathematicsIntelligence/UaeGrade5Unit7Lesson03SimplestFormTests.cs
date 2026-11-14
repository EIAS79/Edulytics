using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Grade5 selected Arabic student pp498–503. Tests WRITTEN_NOT_RUN.
public sealed class UaeGrade5Unit7Lesson03SimplestFormTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:07:03";
 [Fact] public void FamiliesReuseFractionEquivalenceEngineWithExactInput()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
   Assert.Equal("supporting.fractions.multiply_divide",contract!.SkillId);
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
 [Fact] public void EquivalentButNotSimplestFractionIsRejected()
 {
   var p=new Dictionary<string,int>{{"n",6},{"d",8},{"k",2}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l03.simplify_fraction",p,"٣/٤"));
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l03.simplify_fraction",p,"6/8"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l03.gcf_for_simplification",p,"٢"));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u07.l03.reduced_numerator",p,"3"));
 }
}

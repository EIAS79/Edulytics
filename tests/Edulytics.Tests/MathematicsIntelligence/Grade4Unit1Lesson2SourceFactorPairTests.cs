using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G4 U01 L02 A-B-C source-authoring regression; written, NOT RUN until Stage D.</summary>
public sealed class Grade4Unit1Lesson2SourceFactorPairTests
{
 [Fact]
 public void ContractIsSpecificAndNotGenericPrimeOrLcmSubstitution()
 {
  Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G4:U01:L02",out var c));
  Assert.NotNull(c);
  Assert.Equal("supporting.number.factors_primes",c.SkillId);
  Assert.Equal(3,c.AllowedQuestionFamilies.Count);
  Assert.All(c.AllowedQuestionFamilies,f=>Assert.StartsWith("usccss.g4.u01.l02.",f));
 }
 [Fact]
 public void OriginalRectangleFactorsAreReconstructedAndBadAnswersRejected()
 {
  Assert.True(LessonPracticeContractRegistry.TryResolve("PED:US-CCSS-MATH:G4:U01:L02",out var c));
  Assert.NotNull(c);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(var family in c.AllowedQuestionFamilies)
  {
   var sample=engine.Generate("g4-l2-original-factor-regression","PED:US-CCSS-MATH:G4:U01:L02",[family],ExactSkillQuestionDifficulty.Standard,6,437,[]);
   Assert.Equal(6,sample.Count);
   foreach(var q in sample)
   {
    Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
    Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,"-999"));
    Assert.Equal(family,q.Family);
   }
  }
 }
}

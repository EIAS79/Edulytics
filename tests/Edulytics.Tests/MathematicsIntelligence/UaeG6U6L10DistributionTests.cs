using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L10DistributionTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:10:STUDENT-AR-128";
 [Fact] public void OriginalArabicRichAndSevenFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(7,c!.AllowedQuestionFamilies.Count);
  foreach(var family in c.AllowedQuestionFamilies)
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
 }
 [Fact] public void ExpandedLinearAndFactoredGcfAreExact()
 {
  var p=new Dictionary<string,int>{{"a",5},{"b",2},{"c",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.expand_two_term",p,"١٠س+٣٥"));
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.expand_two_term",p,"10س+7"));
  var q=new Dictionary<string,int>{{"coeff",36},{"constant",30}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.factor_common",q,"٦(٦س+٥)"));
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.factor_common",q,"6*(6س+5)"));
  q["constant"]=31;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.factor_common",q,"6(6س+5)"));
 }
 [Fact] public void MixedFractionAndInvalidDistribution()
 {
  var p=new Dictionary<string,int>{{"a",9},{"whole",4},{"n",1},{"d",3}};
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.mixed_decimal_distribute",p,"39")); // unsupported denominator 3
  p["a"]=5;p["whole"]=2;p["n"]=3;p["d"]=5;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.mixed_decimal_distribute",p,"١٣"));
  var q=new Dictionary<string,int>{{"a",9},{"b",4},{"mode",1}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.distribution_true_false",q,"لا"));
  q["mode"]=0;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u06.l10.distribution_true_false",q,"نعم"));
 }
}

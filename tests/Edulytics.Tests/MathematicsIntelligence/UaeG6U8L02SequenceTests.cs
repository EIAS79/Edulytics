using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L02SequenceTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:02:STUDENT-AR-128";
 [Fact] public void ArabicContentAndSixQuestionFamilies()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");Assert.NotNull(rich);
  Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(6,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void ArithmeticGeometricAndNthTermExact()
 {
  var p=new Dictionary<string,int>{{"first",7},{"delta",7},{"n",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.arithmetic_nth",p,"35"));
  var g=new Dictionary<string,int>{{"first",2},{"ratio",2},{"n",7}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.geometric_nth",g,"128"));
  g["ratio"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.geometric_nth",g,"0"));
 }
 [Fact] public void ClassifyAndDecimalArithmeticSafe()
 {
  var m=new Dictionary<string,int>{{"mode",0}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.classify",m,"حسابية"));
  m["mode"]=1;
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.classify",m,"هندسية"));
  var p=new Dictionary<string,int>{{"first10",45},{"diff10",-5},{"n",5}};
  Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.decimal_arithmetic",p,"٢٫٥"));
  p["n"]=0;
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u08.l02.decimal_arithmetic",p,"0"));
 }
}

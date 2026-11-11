using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson09RelevantInformationTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:09";
 [Fact] public void ContractGeneratesAndVerifies()
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
 [InlineData(3,27,"57","0.57")]
 [InlineData(2,4,"24","0.24")]
 [InlineData(3,18,"48","0.48")]
 public void IrrelevantDaysNeverChangesSum(int tenths,int hundredths,string sum,string dec)
 {
   var p=new Dictionary<string,int>{{"tenths",tenths},{"hundredths",hundredths},{"otherDays",4}};
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l09.relevant_sum",p,sum));
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l09.relevant_decimal",p,dec));
   p["otherDays"]=7;
   Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l09.relevant_sum",p,sum));
 }
 [Fact] public void MissingSourceInputsRejected()
 {
  var p=new Dictionary<string,int>{{"tenths",3},{"otherDays",4}};
  Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l09.relevant_sum",p,"30"));
 }
 [Fact] public void ArabicContentAddressesIrrelevantInformation()
 {
  var l=RichLessonContentV2Registry.Find(Lesson,"ar");
  Assert.NotNull(l);
  Assert.Equal("استقصاء حل المسائل: تحديد المعلومات الإضافية أو الناقصة",l!.Title);
  Assert.True(l.WorkedExamples.Count>=4);
  Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

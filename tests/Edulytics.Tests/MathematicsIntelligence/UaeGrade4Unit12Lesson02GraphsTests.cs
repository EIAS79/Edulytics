using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit12Lesson02GraphsTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:12:02";
 [Fact] public void GraphQuestionFamiliesAreReadyAndCanVerify()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
   Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
   var engine=new ExactSkillContractQuestionEngine();
   foreach(var f in contract.AllowedQuestionFamilies)
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
 [InlineData(30,32,36,44,46,"46","16")]
 [InlineData(20,24,28,35,42,"42","22")]
 [InlineData(10,15,25,30,40,"40","30")]
 public void ChartExamplesReturnCorrectMeasurements(int a,int b,int c,int d,int e,string one,string two)
 {
    var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d},{"e",e}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l02.line_graph_max",p,one));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l02.line_graph_increase",p,two));
 }
 [Fact] public void InvalidChartDataRejected()
 {
   var p=new Dictionary<string,int>{{"a",-1},{"b",2},{"c",3},{"d",4},{"e",5}};
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u12.l02.line_graph_max",p,"0"));
 }
 [Fact] public void ArabicRichContentCoversGraphRepresentation()
 {
   var l=RichLessonContentV2Registry.Find(Lesson,"ar");
   Assert.NotNull(l);
   Assert.Equal("التمثيلات البيانية الخطية",l!.Title);
   Assert.True(l.WorkedExamples.Count>=4);
   Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

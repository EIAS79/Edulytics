using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit11Lesson01DecimalRoundingTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:11:01";
 [Fact] public void GenerateRoundingQuestionsVerifyTheirAnswers()
 {
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
   Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
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
 [InlineData(2800,"uae.g4.u11.l01.round_whole","3")]
 [InlineData(4350,"uae.g4.u11.l01.round_tenth","4.4")]
 [InlineData(5476,"uae.g4.u11.l01.round_hundredth","5.48")]
 public void RoundsFromThousandths(int n,string family,string answer)
 {
   var p=new Dictionary<string,int>{{"thousandthsValue",n}};
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,answer));
 }
 [Fact] public void InvalidDecimalInputRejected()
 {
   var p=new Dictionary<string,int>{{"thousandthsValue",-5}};
   Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l01.round_whole",p,"0"));
 }
 [Fact] public void RichLessonModelsRounding()
 {
   var l=RichLessonContentV2Registry.Find(Lesson,"ar");
   Assert.NotNull(l);
   Assert.Equal("تقريب الأعداد العشرية",l!.Title);
   Assert.True(l.WorkedExamples.Count>=4);
   Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}

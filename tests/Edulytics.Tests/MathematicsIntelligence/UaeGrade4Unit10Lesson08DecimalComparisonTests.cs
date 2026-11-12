using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson08DecimalComparisonTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:08";
  [Fact] public void ContractCanGenerateAndVerifyQuestions()
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
  [InlineData(70,69,"2","70")]
  [InlineData(20,40,"0","40")]
  [InlineData(58,53,"2","58")]
  public void ComparesHundredths(int a,int b,string comparison,string larger)
  {
    var p=new Dictionary<string,int>{{"firstHundredths",a},{"secondHundredths",b}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l08.decimal_compare",p,comparison));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l08.larger_hundredths",p,larger));
  }
  [Fact] public void RejectsMalformedParameters()
  {
    var p=new Dictionary<string,int>{{"firstHundredths",150},{"secondHundredths",69}};
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l08.decimal_compare",p,"2"));
  }
  [Fact] public void ArabicContentContainsPlaceValueComparison()
  {
    var l=RichLessonContentV2Registry.Find(Lesson,"ar");
    Assert.NotNull(l);
    Assert.Equal("مقارنة الأعداد العشرية وترتيبها",l!.Title);
    Assert.True(l.WorkedExamples.Count>=4);
    Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
  }
}

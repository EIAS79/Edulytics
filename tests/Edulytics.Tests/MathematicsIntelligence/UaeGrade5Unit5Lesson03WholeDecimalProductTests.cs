using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Drafted from local G5 book printed pp315-320; NOT_RUN in A-B-C.
public sealed class UaeGrade5Unit5Lesson03WholeDecimalProductTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:03";
  [Fact] public void ThreeExactFamiliesUseReusedEngine()
  {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
    Assert.Equal("supporting.decimals.core",contract!.SkillId);
    Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var f in contract.AllowedQuestionFamilies)
    {
      Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
      Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
      Assert.NotNull(ui);
      foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
      {
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
        Assert.DoesNotContain("result",q.Parameters.Keys);
      }
    }
  }
  [Fact] public void ExactHundredthsAndArabicAnswers()
  {
    var p=new Dictionary<string,int>{{"hundredths",96},{"whole",3}};
    const string f="uae.g5.u05.l03.decimal_times_whole";
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٢٫٨٨"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"288"));
    p["hundredths"]=5;p["whole"]=2;
    Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"0.10"));
    p["whole"]=0;
    Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"0"));
  }
}
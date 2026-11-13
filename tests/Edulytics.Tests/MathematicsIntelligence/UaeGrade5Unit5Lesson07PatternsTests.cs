using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade5Unit5Lesson07PatternsTests
{
  private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:05:07";
  [Fact] public void FourPatternFamiliesAreRegistered()
  {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
    Assert.Equal("number.numeric_sequences_identify_extend",contract!.SkillId);
    Assert.Equal(4,contract.AllowedQuestionFamilies.Count);
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
  [Fact] public void DecimalPatternAndDiscountUseExactScale()
  {
    var p=new Dictionary<string,int>{{"seedMilli",30},{"direction",1},{"length",4}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l07.decimal_factor_ten_pattern",p,"300"));
    p["seedMilli"]=784500;p["direction"]=-1;
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l07.decimal_factor_ten_pattern",p,"٠٫٠٧٨٤٥"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l07.decimal_factor_ten_pattern",p,"0.7845"));
    var d=new Dictionary<string,int>{{"priceCents",3470}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l07.ten_percent_discount_pattern",d,"31.23"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g5.u05.l07.ten_percent_discount_pattern",d,"3.47"));
  }
}
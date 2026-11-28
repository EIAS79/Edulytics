using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10PostulatesTests
{
 private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U18:L01:C146";
 private const string Prefix="uae.g10.general.u18.l01.";
 [Theory]
 [InlineData("necessary_sufficient_threshold")]
 [InlineData("incidence_postulate")]
 [InlineData("always_sometimes_never")]
 [InlineData("midpoint_total")]
 [InlineData("shared_midpoint_half")]
 [InlineData("unordered_pair_count")]
 [InlineData("wheel_count_reasoning")]
 [InlineData("travel_time_comparison")]
 [InlineData("slope_requirement")]
 public void FamilyDispatchAndIndependentReasoning(string suffix)
 {
  string family=Prefix+suffix;Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
  var engine=new ExactSkillContractQuestionEngine();
  foreach(int seed in new[]{146,20261126,20261127,723})
  {
   var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);Assert.Equal(4,tasks.Count);
   foreach(var task in tasks)
   {
    var p=task.Parameters;int a=p["a"],b=p["b"],m=p["m"],n=p["n"],x=p["x"],v=p["v"],w=p["w"];
    int expected=suffix switch{"necessary_sufficient_threshold"=>a==b?3:a<b?1:2,"incidence_postulate"=>m,"always_sometimes_never"=>new[]{1,2,3,2,1,3,1}[m-1],"midpoint_total"=>2*a,"shared_midpoint_half"=>a,"unordered_pair_count"=>n*(n-1)/2,"wheel_count_reasoning"=>x,"travel_time_comparison"=>a*w==b*v?3:a*w<b*v?1:2,"slope_requirement"=>a>=4*b?1:0,_=>throw new InvalidOperationException()};
    Assert.Equal(expected,int.Parse(task.CorrectAnswer));Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString()));Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
    string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
    var invalid=p.ToDictionary(z=>z.Key,z=>z.Value,StringComparer.Ordinal);invalid["n"]=999;Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
   }
  }
 }
 [Fact] public void GoldenBoundary(){var p=new Dictionary<string,int>{{"a",12},{"b",3},{"m",7},{"n",8},{"x",4},{"v",40},{"w",10}};
 Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"unordered_pair_count",p,"28"));Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"travel_time_comparison",p,"3"));Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"slope_requirement",p,"1"));p["a"]=11;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"slope_requirement",p,"0"));}
}

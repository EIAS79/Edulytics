using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U11 L09, 03.pdf printed pp. 841-846. Stage D is not claimed.
public sealed class UaeGrade5Unit11Lesson09MedianModeTests
{
 [Theory]
 [InlineData("uae.g5.u11.l09.median_odd_five")]
 [InlineData("uae.g5.u11.l09.median_even_four")]
 [InlineData("uae.g5.u11.l09.single_mode")]
 [InlineData("uae.g5.u11.l09.two_modes_count")]
 [InlineData("uae.g5.u11.l09.no_mode_count")]
 public void ExactMedianAndModeAgreeWithIndependentDataOracle(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
  Assert.NotNull(interaction);
  var engine=new ExactSkillContractQuestionEngine();
  var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:09",
    family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
  Assert.NotEmpty(questions);
  foreach(var q in questions)
  {
   var p=q.Parameters;
   decimal expected=family switch
   {
    "uae.g5.u11.l09.median_odd_five"=>
      new[]{p["a"],p["b"],p["c"],p["d"],p["e"]}.OrderBy(x=>x).ElementAt(2),
    "uae.g5.u11.l09.median_even_four"=>
      (new[]{p["a"],p["b"],p["c"],p["d"]}.OrderBy(x=>x).ElementAt(1)+
       new[]{p["a"],p["b"],p["c"],p["d"]}.OrderBy(x=>x).ElementAt(2))/2m,
    "uae.g5.u11.l09.single_mode"=>p["a"],
    "uae.g5.u11.l09.two_modes_count"=>
      new[]{p["a"],p["a"],p["b"],p["b"],p["c"],p["d"]}.GroupBy(x=>x).Count(g=>g.Count()==2),
    _=>0m
   };
   Assert.Equal(expected.ToString("0.##",CultureInfo.InvariantCulture),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"not a number"));
   var tampered=p.ToDictionary(kv=>kv.Key,kv=>kv.Value);
   tampered.Add("untrusted_expected",99999);
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
  }
 }
}

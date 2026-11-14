using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U11 L10, Arabic textbook printed pp. 847-852.
// Tests focused exact contracts; Stage D complete-suite not claimed.
public sealed class UaeGrade5Unit11Lesson10DotPlotTests
{
 [Theory]
 [InlineData("uae.g5.u11.l10.dot_frequency")]
 [InlineData("uae.g5.u11.l10.dot_total")]
 [InlineData("uae.g5.u11.l10.dot_median")]
 [InlineData("uae.g5.u11.l10.dot_mode")]
 [InlineData("uae.g5.u11.l10.dot_range")]
 public void DotPlotsProduceIndependentlyCheckableAnswers(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
  Assert.NotNull(interaction);
  var engine=new ExactSkillContractQuestionEngine();
  var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:10",
      family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
  Assert.NotEmpty(questions);
  foreach(var q in questions)
  {
   var p=q.Parameters;
   int start=p["a"],step=p["s"];
   int[] counts=Enumerable.Range(0,5).Select(i=>p[$"f{i}"]).ToArray();
   int[] ordered=Enumerable.Range(0,5)
      .SelectMany(i=>Enumerable.Repeat(start+i*step,counts[i])).OrderBy(x=>x).ToArray();
   decimal expected=family switch
   {
    "uae.g5.u11.l10.dot_frequency"=>ordered.Count(x=>x==start+2*step),
    "uae.g5.u11.l10.dot_total"=>ordered.Length,
    "uae.g5.u11.l10.dot_median"=>(ordered[(ordered.Length-1)/2]+ordered[ordered.Length/2])/2m,
    "uae.g5.u11.l10.dot_mode"=>ordered.GroupBy(x=>x).OrderByDescending(g=>g.Count()).First().Key,
    "uae.g5.u11.l10.dot_range"=>ordered.Max()-ordered.Min(),
    _=>throw new InvalidOperationException("Unsupported family")
   };
   Assert.Equal(expected.ToString("0.##",CultureInfo.InvariantCulture),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"not a number"));
   var altered=p.ToDictionary(kv=>kv.Key,kv=>kv.Value);
   altered.Add("untrusted_expected",99999);
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,altered,q.CorrectAnswer));
  }
 }
}

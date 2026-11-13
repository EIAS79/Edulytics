using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 U11 L11 stem-and-leaf, printed pp. 853-858.
// Stage D suite is separate; generated interpretation questions are exact.
public sealed class UaeGrade5Unit11Lesson11StemLeafTests
{
 [Theory]
 [InlineData("uae.g5.u11.l11.leaf_count_at_stem")]
 [InlineData("uae.g5.u11.l11.stem_median")]
 [InlineData("uae.g5.u11.l11.stem_range")]
 [InlineData("uae.g5.u11.l11.stem_mode_frequency")]
 [InlineData("uae.g5.u11.l11.ordered_kth")]
 public void StemLeafUsesEveryLeafAndIndependentKeyReconstruction(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
  Assert.NotNull(interaction);
  var engine=new ExactSkillContractQuestionEngine();
  var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:11:11",
      family,[family],ExactSkillQuestionDifficulty.Standard,8,20261113,[]);
  Assert.NotEmpty(questions);
  foreach(var q in questions)
  {
   var p=q.Parameters;
   int n=p["n"];
   int[] sorted=Enumerable.Range(0,n).Select(i=>p[$"v{i}"]).OrderBy(x=>x).ToArray();
   decimal expected=family switch
   {
    "uae.g5.u11.l11.leaf_count_at_stem"=>sorted.Count(x=>x/10==p["targetStem"]),
    "uae.g5.u11.l11.stem_median"=>(sorted[(n-1)/2]+sorted[n/2])/2m,
    "uae.g5.u11.l11.stem_range"=>sorted.Max()-sorted.Min(),
    "uae.g5.u11.l11.stem_mode_frequency"=>sorted.GroupBy(x=>x).Max(g=>g.Count()),
    "uae.g5.u11.l11.ordered_kth"=>sorted[p["k"]-1],
    _=>throw new InvalidOperationException("Unsupported stem family")
   };
   Assert.Equal(expected.ToString("0.##",CultureInfo.InvariantCulture),q.CorrectAnswer);
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"99999"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"not a number"));
   var extra=p.ToDictionary(kv=>kv.Key,kv=>kv.Value);
   extra.Add("result",99999);
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,extra,q.CorrectAnswer));
  }
 }
}

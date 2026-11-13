using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade 5 Arabic U12 L03, original printed pp.889-894.
// Explicitly exclude invalid/degenerated triangle triples such as (2,2,4).
public sealed class UaeGrade5Unit12Lesson03TriangleClassificationTests
{
 [Theory]
 [InlineData("uae.g5.u12.l03.triangle_side_type")]
 [InlineData("uae.g5.u12.l03.triangle_angle_type")]
 public void ClassificationUsesValidTrianglesAndIndependentOracle(string family)
 {
  Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
  Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var route));
  Assert.NotNull(route);
  var engine=new ExactSkillContractQuestionEngine();
  for(int seed=11;seed<17;seed++)
  {
   var questions=engine.Generate("PED:UAE-MOE-MATH:L5:GENERAL:12:03",
     family,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
   Assert.NotEmpty(questions);
   foreach(var item in questions)
   {
    int a=item.Parameters["a"],b=item.Parameters["b"],c=item.Parameters["c"];
    bool isSide=family=="uae.g5.u12.l03.triangle_side_type";
    int expected;
    if(isSide)
    {
     Assert.True(a+b>c && a+c>b && b+c>a);
     expected=a==b&&b==c?0:a==b||b==c||a==c?1:2;
    }
    else
    {
     Assert.Equal(180,a+b+c);
     expected=a>90||b>90||c>90?2:a==90||b==90||c==90?1:0;
    }
    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),item.CorrectAnswer);
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,item.CorrectAnswer));
    string arabic=new(item.CorrectAnswer.Select(ch=>(char)('٠'+ch-'0')).ToArray());
    Assert.True(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,arabic));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"99999"));
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,item.Parameters,"invalid"));
    var modified=item.Parameters.ToDictionary(k=>k.Key,v=>v.Value);
    modified.Add("expected",expected);
    Assert.False(ExactSkillContractQuestionEngine.Verify(family,modified,item.CorrectAnswer));
   }
  }
 }
}

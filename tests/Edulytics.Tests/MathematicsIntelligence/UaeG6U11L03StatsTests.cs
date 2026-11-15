using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U11L03StatsTests
{
 private const string L="PED:UAE-MOE-MATH:L6:GENERAL:11:03:STUDENT-AR-128";
 [Fact] public void ArabicLessonAndExactPracticeExist()
 {
   var r=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(r);Assert.Equal(8,r!.WorkedExamples.Count);
   Assert.True(r.Visuals.Count>=3);
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
   Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
   foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Fact] public void CorrectAnswerAndNegativeCase()
 {
   var p=new Dictionary<string,int>{{"a",9},{"b",3},{"c",5}};
   const string family="uae.g6.u11.l03.median";
   Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,"٥"));
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"0"));
 }
}

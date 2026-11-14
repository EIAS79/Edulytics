using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U8L06InequalityModelsTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:08:06:STUDENT-AR-128";
 const string M="uae.g6.u08.l06.candidate_membership",
  E="uae.g6.u08.l06.endpoint_inclusion",
  D="uae.g6.u08.l06.graph_direction";
 [Fact] public void ArabicLabAndPracticeContractsExist()
 {
   var lesson=RichLessonContentV2Registry.Find(L,"ar");
   Assert.NotNull(lesson);Assert.Equal(10,lesson!.WorkedExamples.Count);
   Assert.All(lesson.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
   Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
   Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
   foreach(var f in c.AllowedQuestionFamilies)
      Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Theory]
 [InlineData(0,7,5,"نعم")]
 [InlineData(0,7,7,"لا")]
 [InlineData(1,7,7,"نعم")]
 [InlineData(2,5,5,"لا")]
 [InlineData(3,5,5,"نعم")]
 [InlineData(0,-2,-4,"نعم")]
 public void InequalityMembershipPreservesStrictVsInclusive(int op,int bound,int candidate,string expected)
 {
   var p=new Dictionary<string,int>{{"op",op},{"bound",bound},{"candidate",candidate}};
   Assert.True(ExactSkillContractQuestionEngine.Verify(M,p,expected));
 }
 [Fact] public void BoundaryAndDirectionFollowOperator()
 {
   var p=new Dictionary<string,int>{{"op",0}};
   Assert.True(ExactSkillContractQuestionEngine.Verify(E,p,"مفتوحة"));
   Assert.True(ExactSkillContractQuestionEngine.Verify(D,p,"يسار"));
   p["op"]=3;
   Assert.True(ExactSkillContractQuestionEngine.Verify(E,p,"مغلقة"));
   Assert.True(ExactSkillContractQuestionEngine.Verify(D,p,"يمين"));
   p["op"]=4;
   Assert.False(ExactSkillContractQuestionEngine.Verify(E,p,"مغلقة"));
 }
}

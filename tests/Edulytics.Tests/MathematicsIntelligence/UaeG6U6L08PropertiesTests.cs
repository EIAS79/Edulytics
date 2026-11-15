using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U6L08PropertiesTests
{
 const string L="PED:UAE-MOE-MATH:L6:GENERAL:06:08:STUDENT-AR-128";
 const string Classify="uae.g6.u06.l08.classify_property",Rewrite="uae.g6.u06.l08.rewrite_associative",
 Area="uae.g6.u06.l08.triangle_area_reordered";
 [Fact] public void ArabicRichAndReusedProperties()
 {
  var rich=RichLessonContentV2Registry.Find(L,"ar");
  Assert.NotNull(rich);Assert.Equal(10,rich!.WorkedExamples.Count);
  Assert.All(rich.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
  Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
  Assert.Equal(8,c!.AllowedQuestionFamilies.Count);
  foreach(var f in c.AllowedQuestionFamilies)
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
 }
 [Theory]
 [InlineData(0,"التبديل")]
 [InlineData(1,"التجميع")]
 [InlineData(2,"المحايد")]
 [InlineData(3,"غير متكافئين")]
 [InlineData(4,"غير متكافئين")]
 [InlineData(5,"التبديل")]
 [InlineData(6,"التجميع")]
 [InlineData(7,"المحايد")]
 public void ClassifierDistinguishesSubtractionAndDivision(int mode,string answer)
 {
  var p=new Dictionary<string,int>{{"mode",mode},{"a",20},{"b",5},{"c",3}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(Classify,p,answer));
  Assert.False(ExactSkillContractQuestionEngine.Verify(Classify,p,"خاصية مجهولة"));
 }
 [Fact] public void AssociativeRewritingAndTriangleAreaStayExact()
 {
  var p=new Dictionary<string,int>{{"a",1},{"b",4}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(Rewrite,p,"س+١+٤"));
  Assert.True(ExactSkillContractQuestionEngine.Verify(Rewrite,p,"س+٥"));
  var q=new Dictionary<string,int>{{"b",15},{"h",12}};
  Assert.True(ExactSkillContractQuestionEngine.Verify(Area,q,"٩٠"));
  Assert.False(ExactSkillContractQuestionEngine.Verify(Area,q,"180"));
  q["h"]=31;
  Assert.False(ExactSkillContractQuestionEngine.Verify(Area,q,"232.5"));
 }
}

using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal108OriginalSourceGuardTests
{
 [Fact] public void RichContentSourceIdentityAndRevisionStayGuarded()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(x=>CommonCoreG2Unit7NextFourCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L06").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("a5db512e710043d2b8dd898d5fc0309e10c51379aef59bbfe5a1f267839cac2a",target.L.CanonicalBodySha256);
  Assert.Contains("IM2021 Grade2 Unit7 Lesson6",target.L.Translations.Single(x=>x.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7NextFourCorrections.VersionFor(108),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  var old=target.L.CanonicalBodySha256;
  try{target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7NextFourCorrections.Apply(target.D));}
  finally{target.L.CanonicalBodySha256=old;}
 }
}
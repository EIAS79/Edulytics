using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal109OriginalSourceGuardTests
{
 [Fact] public void RichContentSourceIdentityAndRevisionStayGuarded()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(x=>CommonCoreG2Unit7NextFourCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L07").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("86d860dff91f48b270f0952ce2ab3d7429c5914db6b46878cd0eafeaf736fdaa",target.L.CanonicalBodySha256);
  Assert.Contains("IM2021 Grade2 Unit7 Lesson7",target.L.Translations.Single(x=>x.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7NextFourCorrections.VersionFor(109),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  var old=target.L.CanonicalBodySha256;
  try{target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7NextFourCorrections.Apply(target.D));}
  finally{target.L.CanonicalBodySha256=old;}
 }
}
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal111OriginalSourceGuardTests
{
 [Fact] public void OriginalSourceHashAndSixReviewedFieldsAreStrict()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
    d=>d.Lessons.Where(x=>CommonCoreG2Unit7LastTenCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L09").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("4fb3d4ecbac1648e7aa09b95fd8a7c7395e8ad06b903b0b8c08681748cc050ec",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit7 Lesson9",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7LastTenCorrections.VersionFor(111),
    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var original=item.L.CanonicalBodySha256;
  try {item.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7LastTenCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=original;}
 }
}
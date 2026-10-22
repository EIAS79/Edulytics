using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal118OriginalSourceGuardTests
{
 [Fact] public void OriginalSourceHashAndSixReviewedFieldsAreStrict()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
    d=>d.Lessons.Where(x=>CommonCoreG2Unit7LastTenCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L16").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("7bb457d7e5f20b336db5403c56a916b837f016b1b602f6884c0f2720ca1c5e1f",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit7 Lesson16",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7LastTenCorrections.VersionFor(118),
    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var original=item.L.CanonicalBodySha256;
  try {item.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7LastTenCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=original;}
 }
}
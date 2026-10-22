using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal117OriginalSourceGuardTests
{
 [Fact] public void OriginalSourceHashAndSixReviewedFieldsAreStrict()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
    d=>d.Lessons.Where(x=>CommonCoreG2Unit7LastTenCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L15").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("78c45196c086fcaec7aee0981fd6c91394c44f882a1c3584abf551cc1f0a5ea5",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit7 Lesson15",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7LastTenCorrections.VersionFor(117),
    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var original=item.L.CanonicalBodySha256;
  try {item.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7LastTenCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=original;}
 }
}
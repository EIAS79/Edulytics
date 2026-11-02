using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal119OriginalSourceGuardTests
{
 [Fact] public void OriginalSourceHashAndSixReviewedFieldsAreStrict()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
    d=>d.Lessons.Where(x=>CommonCoreG2Unit7LastTenCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L17").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("394ce0a0fe5dba47575c368b508524e64350174ee97bfa2e5a17253978b99530",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit7 Lesson17",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7LastTenCorrections.VersionFor(119),
    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var original=item.L.CanonicalBodySha256;
  try {item.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7LastTenCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=original;}
 }
}
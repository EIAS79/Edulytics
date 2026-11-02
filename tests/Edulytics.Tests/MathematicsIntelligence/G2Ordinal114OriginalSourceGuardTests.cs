using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal114OriginalSourceGuardTests
{
 [Fact] public void OriginalSourceHashAndSixReviewedFieldsAreStrict()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
    d=>d.Lessons.Where(x=>CommonCoreG2Unit7LastTenCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L12").Select(x=>(D:d,L:x))).Single();
  Assert.Equal("e92d5358ee0ff2900186e485200b249fb36135a16fea4b107a9d5026044724b6",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit7 Lesson12",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7LastTenCorrections.VersionFor(114),
    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var original=item.L.CanonicalBodySha256;
  try {item.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7LastTenCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=original;}
 }
}
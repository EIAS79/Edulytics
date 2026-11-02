using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal137OriginalSourceGuardTests {
 [Fact] public void GuardedSourceIdentityAndAllSixReviewedFieldsExist(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(t=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,t)&&t.LessonCode=="PED:US-CCSS-MATH:G2:U09:L04").Select(t=>(D:d,L:t))).Single();
  Assert.Equal("39879a4c0f2c36d601282549dce30b6d37eb3fc19f757d10f3dd35eb87d55272",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson4",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(137),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var original=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=original;}
 }
}
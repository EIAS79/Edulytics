using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal135OriginalSourceGuardTests {
 [Fact] public void GuardedSourceIdentityAndAllSixReviewedFieldsExist(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(t=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,t)&&t.LessonCode=="PED:US-CCSS-MATH:G2:U09:L02").Select(t=>(D:d,L:t))).Single();
  Assert.Equal("65b106327d18820a0e2ca62b2242fb04873d806550f0ffd24119c771b41de6b3",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson2",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(135),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var original=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=original;}
 }
}
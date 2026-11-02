using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal134OriginalSourceGuardTests {
 [Fact] public void GuardedSourceIdentityAndAllSixReviewedFieldsExist(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(t=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,t)&&t.LessonCode=="PED:US-CCSS-MATH:G2:U09:L01").Select(t=>(D:d,L:t))).Single();
  Assert.Equal("3a39adaea697e812889cb50b04fd27cb13e85d426bb649fe55080732731229d9",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson1",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(134),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var original=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=original;}
 }
}
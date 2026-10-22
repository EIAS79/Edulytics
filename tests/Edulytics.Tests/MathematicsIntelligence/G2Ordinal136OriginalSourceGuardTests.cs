using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal136OriginalSourceGuardTests {
 [Fact] public void GuardedSourceIdentityAndAllSixReviewedFieldsExist(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(t=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,t)&&t.LessonCode=="PED:US-CCSS-MATH:G2:U09:L03").Select(t=>(D:d,L:t))).Single();
  Assert.Equal("e67bd2743cc7c9afebe7a6ca4703f99681551351dabe6c0d3d0a120e85a85ac9",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson3",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(136),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var original=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=original;}
 }
}
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal140OriginalSourceGuardTests {
 [Fact] public void SourceShaRichTextAndVersionRejectSourceDrift(){
  var v=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U09:L07").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("014c75bdaadb875fca85d5c0ef3f48ea9ff9522937385ddb68643bd5185c6743",v.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson7",v.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(140),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(v.D,v.L));
  var prior=v.L.CanonicalBodySha256;
  try {v.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(v.D));}
  finally {v.L.CanonicalBodySha256=prior;}
 }
}
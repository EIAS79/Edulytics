using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal138OriginalSourceGuardTests {
 [Fact] public void SourceShaRichTextAndVersionRejectSourceDrift(){
  var v=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U09:L05").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("eadddc5d9f13e9433dc593a4b6a7cdc0a5b99e64e1b245a66fab459069ae4f25",v.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson5",v.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(138),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(v.D,v.L));
  var prior=v.L.CanonicalBodySha256;
  try {v.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(v.D));}
  finally {v.L.CanonicalBodySha256=prior;}
 }
}
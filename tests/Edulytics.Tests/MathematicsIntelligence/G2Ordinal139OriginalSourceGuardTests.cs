using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal139OriginalSourceGuardTests {
 [Fact] public void SourceShaRichTextAndVersionRejectSourceDrift(){
  var v=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U09:L06").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("b2a01addd694fd4bbbff6fc55751a8fdcb4e27c19a1a984638a210e306555a4f",v.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson6",v.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(139),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(v.D,v.L));
  var prior=v.L.CanonicalBodySha256;
  try {v.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(v.D));}
  finally {v.L.CanonicalBodySha256=prior;}
 }
}
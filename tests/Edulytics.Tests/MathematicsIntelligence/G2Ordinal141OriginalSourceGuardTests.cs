using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal141OriginalSourceGuardTests {
 [Fact] public void SourceShaRichTextAndVersionRejectSourceDrift(){
  var v=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U09:L08").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("5f6d35dffa7a83a1c0a93391728e75fe2d46806f0e7b2fda7c551bbc02f3c416",v.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson8",v.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(141),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(v.D,v.L));
  var prior=v.L.CanonicalBodySha256;
  try {v.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(v.D));}
  finally {v.L.CanonicalBodySha256=prior;}
 }
}
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal125OriginalSourceGuardTests {
 [Fact] public void EffectiveRichContentAndSourceHashAreGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U08:L05").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("079c96edc04f4dfaf7c69c41144d4d589219a6b5532d0435865ecb2cc90e903a",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson5",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(125),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var old=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=old;}
 }
}
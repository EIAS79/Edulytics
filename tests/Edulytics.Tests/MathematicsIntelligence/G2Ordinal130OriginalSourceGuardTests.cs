using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal130OriginalSourceGuardTests {
 [Fact] public void EffectiveRichContentAndSourceHashAreGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U08:L10").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("3e31c5d65eba1e6839a052f2d5e823e6e0d0229ae979fbcd8b7aec49c78615ed",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson10",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(130),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var old=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=old;}
 }
}
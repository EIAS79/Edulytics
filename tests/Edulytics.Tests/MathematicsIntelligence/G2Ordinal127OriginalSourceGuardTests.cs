using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal127OriginalSourceGuardTests {
 [Fact] public void EffectiveRichContentAndSourceHashAreGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U08:L07").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("2e4275d29eaa9fd2312771c81e52435a7810bcd34bed33d6b3725962ae7ec4be",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson7",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(127),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var old=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=old;}
 }
}
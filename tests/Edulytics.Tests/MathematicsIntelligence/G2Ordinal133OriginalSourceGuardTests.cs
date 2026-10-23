using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal133OriginalSourceGuardTests {
 [Fact] public void EffectiveRichContentAndSourceHashAreGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U08:L13").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("0e2da35fe5896a5b9d33957631e30236394805f2619dea1fbbea3cb3fc6de2ab",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson13",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(133),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var old=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=old;}
 }
}
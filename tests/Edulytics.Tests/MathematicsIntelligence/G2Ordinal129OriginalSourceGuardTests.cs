using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal129OriginalSourceGuardTests {
 [Fact] public void EffectiveRichContentAndSourceHashAreGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
    d.Lessons.Where(l=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,l)&&l.LessonCode=="PED:US-CCSS-MATH:G2:U08:L09").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("bcb72af780ce091cc45149ffbe48c2e8b3bf7f65bd96ca98538d30d5ea880d61",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson9",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(129),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var old=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=old;}
 }
}
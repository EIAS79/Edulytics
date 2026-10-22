using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal142OriginalSourceGuardTests {
 [Fact] public void SourceExactShaAndFullRichContentCannotDrift(){
  var z=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(x=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U09:L09")
   .Select(x=>(D:d,L:x))).Single();
  Assert.Equal("f98057f3c3fa8bf15d987df9251191a0d6ac5d962b08fcdf91d4139ef2581c4b",z.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson9",z.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(142),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(z.D,z.L));
  var sha=z.L.CanonicalBodySha256;
  try{z.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(z.D));}
  finally{z.L.CanonicalBodySha256=sha;}
 }
}
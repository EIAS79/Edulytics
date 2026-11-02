using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal145OriginalSourceGuardTests {
 [Fact] public void SourceExactShaAndFullRichContentCannotDrift(){
  var z=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(x=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U09:L12")
   .Select(x=>(D:d,L:x))).Single();
  Assert.Equal("3d8f619b82e233f6d61276aaf5154f0aa416685ba9e153b8121bf9f8b5ad60ef",z.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson12",z.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(145),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(z.D,z.L));
  var sha=z.L.CanonicalBodySha256;
  try{z.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(z.D));}
  finally{z.L.CanonicalBodySha256=sha;}
 }
}
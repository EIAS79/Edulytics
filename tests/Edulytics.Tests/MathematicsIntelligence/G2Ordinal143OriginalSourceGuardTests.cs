using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal143OriginalSourceGuardTests {
 [Fact] public void SourceExactShaAndFullRichContentCannotDrift(){
  var z=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(x=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U09:L10")
   .Select(x=>(D:d,L:x))).Single();
  Assert.Equal("4852071d4ef4e24610dff7ceaa8225b8b27d3a02773987226234ac049372e253",z.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson10",z.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(143),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(z.D,z.L));
  var sha=z.L.CanonicalBodySha256;
  try{z.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(z.D));}
  finally{z.L.CanonicalBodySha256=sha;}
 }
}
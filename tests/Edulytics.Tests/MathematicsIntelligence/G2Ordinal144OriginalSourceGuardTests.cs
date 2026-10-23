using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal144OriginalSourceGuardTests {
 [Fact] public void SourceExactShaAndFullRichContentCannotDrift(){
  var z=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(x=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U09:L11")
   .Select(x=>(D:d,L:x))).Single();
  Assert.Equal("a9cd3e8f1ae6a0d9c2a7053216464815e1572fa49ff3edab2db8a61f43e0d8dd",z.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson11",z.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(144),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(z.D,z.L));
  var sha=z.L.CanonicalBodySha256;
  try{z.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(z.D));}
  finally{z.L.CanonicalBodySha256=sha;}
 }
}
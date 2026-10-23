using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal146OriginalSourceGuardTests{
 [Fact] public void LastOriginalLessonSourceAndRichSixFieldsRemainBoundToSha(){
  var l=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(z=>CommonCoreG2Unit9SourceCorrections.IsTarget(d,z)&&z.LessonCode=="PED:US-CCSS-MATH:G2:U09:L13").Select(z=>(D:d,L:z))).Single();
  Assert.Equal("d06a0120de3209c2c81684edfe7db8065fff36692067056ed038ba3456358a9e",l.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit9 Lesson13",l.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit9SourceCorrections.VersionFor(146),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(l.D,l.L));
  var old=l.L.CanonicalBodySha256;
  try{l.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit9SourceCorrections.Apply(l.D));}
  finally{l.L.CanonicalBodySha256=old;}
 }
}
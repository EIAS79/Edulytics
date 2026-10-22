using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal105OriginalSourceGuardTests
{
 [Fact] public void RichBodyAndVersionKeepOfficialShaIdentity()
 {
  var item=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(
   d=>d.Lessons.Where(l=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,l)&&
   l.LessonCode=="PED:US-CCSS-MATH:G2:U07:L03").Select(l=>(D:d,L:l))).Single();
  Assert.Equal("202d7795eb6b9e9c2737e8375711da40fcb1edf3ee112628a66363693fb71ff3",item.L.CanonicalBodySha256);
  Assert.Contains("IM2021 Grade2 Unit7 Lesson3",item.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit7FirstFourCorrections.VersionFor(105),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.D,item.L));
  var before=item.L.CanonicalBodySha256;
  try{item.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7FirstFourCorrections.Apply(item.D));}
  finally{item.L.CanonicalBodySha256=before;}
 }
}
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal103OriginalSourceGuardTests
{
 [Fact] public void OfficialBodyIdentityAndSixReviewedFieldsAreShaGuarded()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U07:L01").Select(l=>(D:d,L:l))).Single();
  var english=target.L.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("IM2021 Grade2 Unit7 Lesson1",english.Explanation);
  Assert.Contains("First-party",english.KeyConceptsAndRules,StringComparison.OrdinalIgnoreCase);
  Assert.Contains("Public publisher task mathematics",english.WorkedExamples);
  Assert.Equal(CommonCoreG2Unit7FirstFourCorrections.VersionFor(103),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  Assert.False(CommonCoreG2Unit7FirstFourCorrections.CanUpgradeExisting(target.D,target.L,"unrecognized"));
 }
 [Fact] public void AlteredSourceHashFailsClosed()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U07:L01").Select(l=>(D:d,L:l))).Single();
  var prior=target.L.CanonicalBodySha256;
  Assert.Equal("2da4188392d740b0c0d36219b5b548bd82ee75d3ec57a87ee2ed6f1f6ed1f04d",prior);
  try {target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7FirstFourCorrections.Apply(target.D));}
  finally {target.L.CanonicalBodySha256=prior;}
 }
}
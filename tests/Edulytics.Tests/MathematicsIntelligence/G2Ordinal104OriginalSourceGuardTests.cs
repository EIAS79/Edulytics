using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal104OriginalSourceGuardTests
{
 [Fact] public void OfficialBodyIdentityAndSixReviewedFieldsAreShaGuarded()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U07:L02").Select(l=>(D:d,L:l))).Single();
  var english=target.L.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("IM2021 Grade2 Unit7 Lesson2",english.Explanation);
  Assert.Contains("First-party",english.WorkedExamples,StringComparison.OrdinalIgnoreCase);
  Assert.Equal(CommonCoreG2Unit7FirstFourCorrections.VersionFor(104),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  Assert.False(CommonCoreG2Unit7FirstFourCorrections.CanUpgradeExisting(target.D,target.L,"unrecognized"));
 }
 [Fact] public void AlteredSourceHashFailsClosed()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U07:L02").Select(l=>(D:d,L:l))).Single();
  var prior=target.L.CanonicalBodySha256;
  Assert.Equal("b82d311093f66de2c059a2a34451df799df4af84983e4316795dea3702fe30d0",prior);
  try {target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7FirstFourCorrections.Apply(target.D));}
  finally {target.L.CanonicalBodySha256=prior;}
 }
}
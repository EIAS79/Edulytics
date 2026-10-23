using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal73OriginalSourceGuardTests
{
 [Fact] public void StrictOriginalSourceAndSixRichFieldsAreRepaired()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(d,l) &&
       l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L06").Select(l=>(D:d,L:l))).Single();
  var en=target.L.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("forty-eight",en.Explanation);
  Assert.Contains("FIRST-PARTY",en.Explanation+en.KeyConceptsAndRules+en.WorkedExamples+en.StepByStepSolutions+en.QuickSummary);
  Assert.Contains("ORIGINAL",en.StepByStepSolutions);
  Assert.Equal(CommonCoreG2Unit5OriginalRichContentCorrections.VersionFor(73),
      CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  Assert.False(CommonCoreG2Unit5OriginalRichContentCorrections.CanUpgradeExisting(
      target.D,target.L,"unapproved-version"));
 }
 [Fact] public void AlteredSourceShaIsRejectedBeforeRepair()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(d,l) &&
      l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L06").Select(l=>(D:d,L:l))).Single();
  var old=target.L.CanonicalBodySha256;
  try {target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>
      CommonCoreG2Unit5OriginalRichContentCorrections.Apply(target.D));
  } finally {target.L.CanonicalBodySha256=old;}
 }
}

using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal68OriginalSourceGuardTests
{
 [Fact]
 public void ExactOriginalLessonIdentityAndSixFieldRichTeachingHold()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(d,l) &&
        l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L01").Select(l=>(D:d,L:l))).Single();
  var en=target.L.Translations.Single(x=>x.CultureCode=="en");
  Assert.Contains("96",en.Explanation);
  Assert.Contains("FIRST-PARTY",en.Explanation+en.KeyConceptsAndRules+en.WorkedExamples+en.StepByStepSolutions+en.QuickSummary);
  Assert.Contains("ORIGINAL",en.StepByStepSolutions);
  Assert.Equal(CommonCoreG2Unit5OriginalRichContentCorrections.VersionFor(68),
      CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  Assert.False(CommonCoreG2Unit5OriginalRichContentCorrections
      .CanUpgradeExisting(target.D,target.L,"unapproved-version"));
 }
 [Fact]
 public void PublisherSourceShaDriftRefusesToApply()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5OriginalRichContentCorrections.IsTarget(d,l) &&
      l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L01").Select(l=>(D:d,L:l))).Single();
  var before=target.L.CanonicalBodySha256;
  try {
   target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit5OriginalRichContentCorrections.Apply(target.D));
  } finally {target.L.CanonicalBodySha256=before;}
 }
}

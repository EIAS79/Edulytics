using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal74OriginalSourceGuardTests
{
 [Fact] public void ExactPublisherIdentityAndSixSourceTeachingFields()
 {
  var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(d,l)&&
      l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L07").Select(l=>(D:d,L:l))).Single();
  var en=row.L.Translations.Single(x=>x.CultureCode=="en");
  Assert.Contains("Mystery Number",en.Explanation);
  Assert.Contains("ORIGINAL",en.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
  Assert.Equal(CommonCoreG2Unit5ComparisonSourceCorrections.VersionFor(74),
      CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.D,row.L));
  Assert.False(CommonCoreG2Unit5ComparisonSourceCorrections.CanUpgradeExisting(
      row.D,row.L,"arbitrary-unapproved"));
 }
 [Fact] public void TamperedCanonicalSourceHashThrowsBeforeApply()
 {
  var row=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(d,l)&&
      l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L07").Select(l=>(D:d,L:l))).Single();
  var original=row.L.CanonicalBodySha256;
  try{row.L.CanonicalBodySha256=new string('0',64);
      Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit5ComparisonSourceCorrections.Apply(row.D));}
  finally{row.L.CanonicalBodySha256=original;}
 }
}

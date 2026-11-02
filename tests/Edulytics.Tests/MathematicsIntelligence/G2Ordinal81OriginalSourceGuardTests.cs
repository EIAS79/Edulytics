using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal81OriginalSourceGuardTests
{
 [Fact] public void RichTeachingIsBoundToOriginalPublisherAndSixFields()
 {
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
    .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(d,l)&&
       l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L14").Select(l=>(Doc:d,Lesson:l))).Single();
  var en=x.Lesson.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("529",en.Explanation);
  Assert.Contains("original",en.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
  Assert.Equal(CommonCoreG2Unit5ComparisonSourceCorrections.VersionFor(81),
     CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.Doc,x.Lesson));
  Assert.False(CommonCoreG2Unit5ComparisonSourceCorrections.CanUpgradeExisting(x.Doc,x.Lesson,"unapproved"));
 }
 [Fact] public void ModifiedCanonicalBodyShaRefusesSourceMaterialization()
 {
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
    .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit5ComparisonSourceCorrections.IsTarget(d,l)&&
       l.LessonCode=="PED:US-CCSS-MATH:G2:U05:L14").Select(l=>(Doc:d,Lesson:l))).Single();
  var previous=x.Lesson.CanonicalBodySha256;
  try {
   x.Lesson.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit5ComparisonSourceCorrections.Apply(x.Doc));
  } finally {x.Lesson.CanonicalBodySha256=previous;}
 }
}

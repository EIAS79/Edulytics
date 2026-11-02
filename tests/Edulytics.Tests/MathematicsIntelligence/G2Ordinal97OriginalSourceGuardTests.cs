using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal97OriginalSourceGuardTests
{
 [Fact] public void OriginalLessonShaAndSixRichFieldsRemainVersionGuarded()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit6SourceCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U06:L16").Select(l=>(D:d,L:l))).Single();
  var english=target.L.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("IM2021 G2 Unit6 Lesson16",english.Explanation);
  Assert.Contains("FIRST-PARTY",english.WorkedExamples);
  Assert.Equal(CommonCoreG2Unit6SourceCorrections.VersionFor(97),
   CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.D,target.L));
  Assert.False(CommonCoreG2Unit6SourceCorrections.CanUpgradeExisting(target.D,target.L,"unapproved"));
 }
 [Fact] public void UnrecognizedChangedOriginalBodyHashFailsClosed()
 {
  var target=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
   .SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit6SourceCorrections.IsTarget(d,l)&&
    l.LessonCode=="PED:US-CCSS-MATH:G2:U06:L16").Select(l=>(D:d,L:l))).Single();
  var before=target.L.CanonicalBodySha256;
  try {target.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit6SourceCorrections.Apply(target.D));}
  finally {target.L.CanonicalBodySha256=before;}
 }
}

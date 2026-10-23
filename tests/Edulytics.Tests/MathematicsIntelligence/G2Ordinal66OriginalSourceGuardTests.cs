using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal66OriginalSourceGuardTests
{
 [Fact] public void SourceContentAndApprovedPredecessorGuard()
 {
  var a=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4JumpTheLineCenterCorrection.IsTarget(d,l)).Select(l=>(D:d,L:l))).Single();
  var en=a.L.Translations.Single(t=>t.CultureCode=="en");
  Assert.Contains("Source-accurate IM K",en.Explanation);
  Assert.Contains("FIRST-PARTY",en.StepByStepSolutions);
  Assert.Equal(CommonCoreG2Unit4JumpTheLineCenterCorrection.Version,CanonicalLessonContentMaterializer.GetEffectiveContentVersion(a.D,a.L));
  Assert.False(CommonCoreG2Unit4JumpTheLineCenterCorrection.CanUpgradeExisting(a.D,a.L,"unapproved"));
 }
 [Fact] public void ModifiedPublisherSourceHashFailsClosed()
 {
  var a=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>d.Lessons.Where(l=>CommonCoreG2Unit4JumpTheLineCenterCorrection.IsTarget(d,l)).Select(l=>(D:d,L:l))).Single();
  var old=a.L.CanonicalBodySha256;
  try {a.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit4JumpTheLineCenterCorrection.Apply(a.D));}
  finally{a.L.CanonicalBodySha256=old;}
 }
}

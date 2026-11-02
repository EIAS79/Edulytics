using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal124OriginalSourceGuardTests{
 [Fact] public void OfficialIdentityAndRichTeachingStayShaGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(z=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,z)&&z.LessonCode=="PED:US-CCSS-MATH:G2:U08:L04")
   .Select(z=>(D:d,L:z))).Single();
  Assert.Equal("44e41667d1493f8c8c0c7fc42233ac5bef3e504f97e817eff0fbe2f47a13cbae",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson4",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(124),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var prior=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=prior;}
 }
}
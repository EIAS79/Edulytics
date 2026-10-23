using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal123OriginalSourceGuardTests{
 [Fact] public void OfficialIdentityAndRichTeachingStayShaGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(z=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,z)&&z.LessonCode=="PED:US-CCSS-MATH:G2:U08:L03")
   .Select(z=>(D:d,L:z))).Single();
  Assert.Equal("41ca33c004b6c33458ca2f0e9003ccfc115f9077702d9c57ac358b6ece291f9c",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson3",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(123),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var prior=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=prior;}
 }
}
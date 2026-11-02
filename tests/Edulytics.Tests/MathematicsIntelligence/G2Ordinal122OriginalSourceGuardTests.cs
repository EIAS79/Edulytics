using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal122OriginalSourceGuardTests{
 [Fact] public void OfficialIdentityAndRichTeachingStayShaGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(z=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,z)&&z.LessonCode=="PED:US-CCSS-MATH:G2:U08:L02")
   .Select(z=>(D:d,L:z))).Single();
  Assert.Equal("660fafd839d002c655c9495a460b037892e07cea3cc2a907d0bd7ba17246a28a",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson2",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(122),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var prior=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=prior;}
 }
}
using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal121OriginalSourceGuardTests{
 [Fact] public void OfficialIdentityAndRichTeachingStayShaGuarded(){
  var x=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
   d.Lessons.Where(z=>CommonCoreG2Unit8SourceCorrections.IsTarget(d,z)&&z.LessonCode=="PED:US-CCSS-MATH:G2:U08:L01")
   .Select(z=>(D:d,L:z))).Single();
  Assert.Equal("1ddbcdadb7bf76ed1b9b1807dab711656c98756f0fbdf2d902f9701aa40901cd",x.L.CanonicalBodySha256);
  Assert.Contains("IM2021 G2 Unit8 Lesson1",x.L.Translations.Single(t=>t.CultureCode=="en").Explanation);
  Assert.Equal(CommonCoreG2Unit8SourceCorrections.VersionFor(121),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(x.D,x.L));
  var prior=x.L.CanonicalBodySha256;
  try{x.L.CanonicalBodySha256=new string('0',64);
   Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit8SourceCorrections.Apply(x.D));}
  finally{x.L.CanonicalBodySha256=prior;}
 }
}
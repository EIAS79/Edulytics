using Edulytics.Data.Seeding;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal105OriginalSourceGuardTests {
 [Fact] public void ShaAndSourceRichTeachingAreGuards(){
 var result=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments().SelectMany(d=>
 d.Lessons.Where(x=>CommonCoreG2Unit7FirstFourCorrections.IsTarget(d,x)&&x.LessonCode=="PED:US-CCSS-MATH:G2:U07:L03").Select(x=>(D:d,L:x))).Single();
 Assert.Equal("202d7795eb6b9e9c2737e8375711da40fcb1edf3ee112628a66363693fb71ff3",result.L.CanonicalBodySha256);
 Assert.Contains("IM2021 Grade2 Unit7 Lesson3",result.L.Translations.Single(x=>x.CultureCode=="en").Explanation);
 Assert.Equal(CommonCoreG2Unit7FirstFourCorrections.VersionFor(105),CanonicalLessonContentMaterializer.GetEffectiveContentVersion(result.D,result.L));
 var original=result.L.CanonicalBodySha256;
 try{result.L.CanonicalBodySha256=new string('0',64);Assert.Throws<InvalidOperationException>(()=>CommonCoreG2Unit7FirstFourCorrections.Apply(result.D));}
 finally{result.L.CanonicalBodySha256=original;}
 }
}